using OpenTK.Wpf;
using OpenTK.Graphics.OpenGL;
using System.Windows;
using OpenTK.Mathematics;
using System.Windows.Controls;
using System.Windows.Media;

namespace bgl
{
    using OpenGL = OpenTK.Graphics.OpenGL;
    using Math = OpenTK.Mathematics;

    public class Renderer
    {
        


public void SetListView(bgl.ListView listView) {
    _listView = listView;

}
        public Renderer()
        {
           
        }


        public void Render(System.TimeSpan delta)
        {
            GL.UseProgram(_program);
            UpdateUniforms();

            // TODO: count frames per second
            GL.ClearColor(211 / 256.0f, 211 / 256.0f, 211 / 256.0f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (_scene == null)
            {
                return;  // there is nothing to draw
            }

            GL.BindBuffer(OpenGL.BufferTarget.ArrayBuffer, _vbo);
            GL.BindBuffer(OpenGL.BufferTarget.ElementArrayBuffer, _ibo);
            GL.BindVertexArray(_vao);

            GL.Disable(OpenGL.EnableCap.CullFace);
            GL.Enable(OpenGL.EnableCap.DepthTest);

            if (_texture != null)
            {
                _texture.Bind(1);
            }

            if (_scene.Meshes.Count <= 0)
            {
                return;  // there is no mesh to draw
            }

            var mesh = _scene.Meshes[0];
            GL.DrawElements(
                OpenGL.PrimitiveType.Triangles,
                mesh.GetIndices().Length,
                OpenGL.DrawElementsType.UnsignedInt,
                0
            );
        }


        // --------------------------------------------------------------------------------------------------
        private double ConvertToRadians(double angle)
        {
            return (System.Math.PI / 180) * angle;
        }

        private static System.Diagnostics.Stopwatch _stopwatch = new System.Diagnostics.Stopwatch();

        public void Initialize()
        {
            try
            {
                LoadMesh();

                /* ---------------------------- Shader Creation ----------------------- */
                CreateShaderProgram();
                _uniformLocations.ModelMatrix = GL.GetUniformLocation(_program, "ModelMatrix");
                _uniformLocations.ViewMatrix = GL.GetUniformLocation(_program, "ViewMatrix");
                _uniformLocations.ProjectionMatrix = GL.GetUniformLocation(
                    _program,
                    "ProjectionMatrix"
                );
                _uniformLocations.LightDirection = GL.GetUniformLocation(
                    _program,
                    "LightDirection"
                );


                var textureLocation = GL.GetUniformLocation(
                    _program,
                    "Texture"
                );

                GL.Uniform1(textureLocation, (int)1);


                /* ---------------------------- Buffer Creation ----------------------- */
                if (_scene != null)
                {
                    CreateVertexBuffer();
                    CreateIndexBuffer();
                    CreateVertexArray();
                }

                _texture = new bgl.Graphics.Core.Texture("tests/glTF/Default_metalRoughness.jpg");

                _stopwatch.Start();
            }
            catch (System.Exception exception)
            {
                System.Windows.MessageBox.Show(
                    exception.Message,
                    "Error",
                    MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error
                );
            }
        }

        void UpdateUniforms()
        {
            // ----------------- View Matrix --------------
            Math.Vector3 lightDirection = new Math.Vector3(1, 1, 1);
            Math.Matrix4 viewMatrix = Math.Matrix4.LookAt(
                new Vector3(0, 1, -10),
                new Vector3(0, 0, 0),
                new Vector3(0, 1, 0)
            );
            // ----------------- Model Matrix --------------
            Math.Matrix4 modelMatrix;

            double radiansPerSecond = ConvertToRadians(60.0);
            _angle = (float)(_stopwatch.Elapsed.TotalSeconds * radiansPerSecond);
            Math.Matrix4.CreateRotationY(_angle, out modelMatrix); // Math.Matrix4.CreateScale(3);

            // ----------------- Projection Matrix --------------
            Math.Matrix4 projectionMatrix = Math.Matrix4.CreatePerspectiveFieldOfView(
                0.5f,
                1.0f,
                0.1f,
                100
            );

            // ----------------- OpenGL Uniform Upload --------------
            GL.UniformMatrix4(_uniformLocations.ViewMatrix, false, ref viewMatrix);
            GL.UniformMatrix4(_uniformLocations.ProjectionMatrix, false, ref projectionMatrix);
            GL.UniformMatrix4(_uniformLocations.ModelMatrix, false, ref modelMatrix);
            GL.Uniform3(_uniformLocations.LightDirection, ref lightDirection);
        }

        private void CreateShaderProgram()
        {
            int vs = GL.CreateShader(OpenGL.ShaderType.VertexShader);
            int fs = GL.CreateShader(OpenGL.ShaderType.FragmentShader);

            string vsSource = """
                #version 330
                uniform mat4 ModelMatrix;
                uniform mat4 ViewMatrix;
                uniform mat4 ProjectionMatrix;
                uniform vec3 LightDirection;


                layout (location = 0) in vec3 Vertex;
                layout (location = 1) in vec3 inNormal;

                out vec2 TexCoord;
                out vec3 Normal;
                out vec3 Color;

                void main() {
                  //  vec4 position = vec4(vertices[gl_VertexID], 1.0);
                    mat4 modelViewProjection = ProjectionMatrix * ViewMatrix * ModelMatrix;
                    gl_Position = modelViewProjection * vec4(Vertex, 1);  //modelViewProjection * position;
                   
                    mat4 normalMatrix = inverse(ViewMatrix * ModelMatrix);
                    Normal =  (normalMatrix * vec4(inNormal, 1)).xyz;


                    Color = vec3(gl_VertexID % 256 / 256.0);
                    
                    switch (gl_VertexID % 4) {
                        case 0:
                            TexCoord = vec2(0, 0);
                            break;
                        case 1:
                            TexCoord = vec2(0, 1);
                            break;
                        case 2:
                            TexCoord = vec2(1, 1);
                            break;
                        case 3:
                            TexCoord = vec2(1, 0);
                            break;
                    }
                }
            """;

            string fsSource = """
                #version 330 

                uniform sampler2D Texture;

                in vec2 TexCoord;
                in vec3 Normal;
                in vec3 Color;
                
                void main() {
                    vec3 N = Normal;
                    vec3 L = vec3(1);
                    vec3 itensity = vec3( max(dot(N, L), 0));

                    gl_FragColor =  vec4(Color, 1.0);  //texture(Texture, TexCoord);
                }
            """;

            GL.ShaderSource(vs, vsSource);
            GL.ShaderSource(fs, fsSource);

            GL.CompileShader(vs);
            GL.CompileShader(fs);

            string vsLog = GL.GetShaderInfoLog(vs);
            string fsLog = GL.GetShaderInfoLog(fs);

            System.Console.WriteLine("VS: " + vsLog);
            System.Console.WriteLine("FS: " + fsLog);

            GL.Disable(OpenGL.EnableCap.CullFace);

            _program = GL.CreateProgram();
            GL.AttachShader(_program, vs);
            GL.AttachShader(_program, fs);
            GL.LinkProgram(_program);

            int linkStatus;
            GL.GetProgram(_program, OpenGL.GetProgramParameterName.LinkStatus, out linkStatus);
            if (linkStatus == 0)
            {
                throw new System.Exception("Could not link shader proram");
            }
        }

        void CreateVertexBuffer()
        {
            if (_scene == null)
            {
                return;
            }

            if (_scene.Meshes.Count <= 0)
            {
                return;
            }

            var mesh = _scene.Meshes[0];

            float[] vertices = new float[(mesh.VertexCount * 3) * 2];
            for (int i = 0; i < mesh.VertexCount; ++i)
            {
                vertices[(i * 6) + 0] = mesh.Vertices[i].X;
                vertices[(i * 6) + 1] = mesh.Vertices[i].Y;
                vertices[(i * 6) + 2] = mesh.Vertices[i].Z;
                vertices[(i * 6) + 3] = mesh.Normals[i].X;
                vertices[(i * 6) + 4] = mesh.Normals[i].Y;
                vertices[(i * 6) + 5] = mesh.Normals[i].Z;
            }

            int size = sizeof(float) * vertices.Length;


            int[] buffer = new int[1];
            GL.CreateBuffers(1, buffer);
            GL.BindBuffer(OpenGL.BufferTarget.ArrayBuffer, buffer[0]);
            GL.BufferData(
                OpenGL.BufferTarget.ArrayBuffer,
                size,
                vertices,
                OpenGL.BufferUsageHint.DynamicDraw
            );

            _vbo = buffer[0];
            System.Console.WriteLine("Created vertex buffer id=" + _vbo);
        }

        void CreateVertexArray()
        {
            //
            int[] array = new int[1];
            GL.CreateVertexArrays(1, array);

            GL.BindVertexArray(array[0]);
            GL.EnableVertexArrayAttrib(array[0], 0);

            int stride = sizeof(float) * 6;
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, sizeof(float) * 3 /* position */ );

            _vao = array[0];
        }

        void CreateIndexBuffer()
        {
            if (_scene == null)
            {
                return;
            }

            if (_scene.Meshes.Count <= 0)
            {
                return;
            }

            var mesh = _scene.Meshes[0];

            int[] indices = mesh.GetIndices();
            int size = sizeof(uint) * indices.Length;

            int[] buffer = new int[1];
            GL.CreateBuffers(1, buffer);
            GL.BindBuffer(OpenGL.BufferTarget.ElementArrayBuffer, buffer[0]);
            GL.BufferData(
                OpenGL.BufferTarget.ElementArrayBuffer,
                size,
                indices,
                OpenGL.BufferUsageHint.DynamicDraw
            );

            _ibo = buffer[0];
            System.Console.WriteLine("Created index buffer id=" + _ibo);
        }

        private int _vbo;
        private int _ibo;
        private int _vao;
        private bgl.Graphics.Core.Texture? _texture;


        /* ----------------------------------- Rendering ----------------------------------  */

        protected struct UniformLocations
        {
            public int ModelMatrix;
            public int ViewMatrix;
            public int ProjectionMatrix;
            public int LightDirection;
        };

        private int _program;
        private UniformLocations _uniformLocations = new UniformLocations();

        float _angle = 0;

        Assimp.Scene? _scene;
        bgl.ListView? _listView;

        void LoadMesh()
        {
            const string fileName = "tests/glTF/DamagedHelmet.gltf";

            var importer = new Assimp.AssimpContext();
            _scene = importer.ImportFile(fileName, Assimp.PostProcessPreset.TargetRealTimeMaximumQuality);
            _listView?.SetScene(_scene);

            System.Console.WriteLine("Loaded mesh vertices=" + _scene.Meshes[0].VertexCount);
        }


    }

    //using GL = OpenTK.Graphics.OpenGL.GL;
} // namespace bgl
