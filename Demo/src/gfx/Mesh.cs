using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace bgl
{
    using OpenGL = OpenTK.Graphics.OpenGL;
    using Math = OpenTK.Mathematics;
    using Texture = bgl.Graphics.Core.Texture;






    class Mesh
    {
        static Dictionary<string, Texture> _textures = new Dictionary<string, Texture>();

        Texture LoadTexture(string path)
        {
            Texture? texture;
            if (!_textures.TryGetValue(path, out texture))
            {
                texture = new Texture(path);
                _textures.Add(path, texture);
            }

            return texture;
        }


        public Mesh(in Assimp.Scene scene, in Assimp.Mesh mesh)
        {
            _mesh = mesh;
            _scene = scene;
            CreateVertexBuffer();
            CreateIndexBuffer();
            CreateVertexArray();

            _texture = LoadTexture("tests/glTF/Default_metalRoughness.jpg");

            CreateShaderProgram();
            GetUniformLocations();
            if (!_stopwatch.IsRunning)
            {
                _stopwatch.Start();
            }
        }

        ~Mesh()
        {
            // TODO: free resources
        }

        public void Render(System.TimeSpan delta)
        {
            GL.UseProgram(_program);
            UpdateUniforms();

            if (_mesh == null)
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

            GL.Disable(OpenGL.EnableCap.CullFace);

            var mesh = _scene.Meshes[0];
            GL.DrawElements(
                OpenGL.PrimitiveType.Triangles,
                mesh.GetIndices().Length,
                OpenGL.DrawElementsType.UnsignedInt,
                0
            );
        }

        void CreateVertexBuffer()
        {
            float[] vertices = new float[(_mesh.VertexCount * 3) * 2];
            for (int i = 0; i < _mesh.VertexCount; ++i)
            {
                vertices[(i * 6) + 0] = _mesh.Vertices[i].X;
                vertices[(i * 6) + 1] = _mesh.Vertices[i].Y;
                vertices[(i * 6) + 2] = _mesh.Vertices[i].Z;
                vertices[(i * 6) + 3] = _mesh.Normals[i].X;
                vertices[(i * 6) + 4] = _mesh.Normals[i].Y;
                vertices[(i * 6) + 5] = _mesh.Normals[i].Z;
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
            int[] indices = _mesh.GetIndices();
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
        }

        void UpdateUniforms()
        {





            // ----------------- View Matrix --------------
            var width = _mesh.BoundingBox.Max.X - _mesh.BoundingBox.Min.X;
            var height = _mesh.BoundingBox.Max.Y - _mesh.BoundingBox.Min.Y;
            var depth = _mesh.BoundingBox.Max.Z - _mesh.BoundingBox.Min.Z;


            Vector3 center = new Vector3(0, 0, 0);

            Vector3 position = new Vector3(0, 0, -10);

          
          
            Math.Vector3 lightDirection = new Math.Vector3(1, 1, 1);
            Math.Matrix4 viewMatrix = Math.Matrix4.LookAt(
                position,
                center,
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

            if (vsLog.Length > 0)
            {
                System.Console.WriteLine("Vertex Shader: " + vsLog);
            }
            if (fsLog.Length > 0)
            {
                System.Console.WriteLine("Fragment Shader: " + fsLog);
            }

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

        void GetUniformLocations()
        {
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
        }
        double ConvertToRadians(double angle)
        {
            return (System.Math.PI / 180) * angle;
        }

        readonly Assimp.Scene _scene;
        readonly Assimp.Mesh _mesh;

        int _vbo;
        int _ibo;
        int _vao;
        bgl.Graphics.Core.Texture? _texture;

        protected struct UniformLocations
        {
            public int ModelMatrix;
            public int ViewMatrix;
            public int ProjectionMatrix;
            public int LightDirection;
        };


        float _angle = 0;

        int _program;
        UniformLocations _uniformLocations = new UniformLocations();
        static System.Diagnostics.Stopwatch _stopwatch = new System.Diagnostics.Stopwatch();

    }
}  // namespace bgl