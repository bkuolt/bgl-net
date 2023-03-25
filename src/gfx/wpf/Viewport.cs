using OpenTK.Wpf;
using OpenTK.Graphics.OpenGL;
using System.Windows;
using OpenTK.Mathematics;

namespace bgl.WPF
{
    using OpenGL = OpenTK.Graphics.OpenGL;
    using Math = OpenTK.Mathematics;

    class IViewport
    {
        // TODO
    }

    public class Viewport : OpenTK.Wpf.GLWpfControl
    {
        public Viewport()
        {
            var settings = new GLWpfControlSettings
            {
                MajorVersion = 3,
                MinorVersion = 3
            };

            // this.Width = width;
            // this.Height = height;
            this.Start(settings);

            this.Render += OnRender;
            this.MouseDown += OnMouseDown;

            this.AddHandler(MouseDownEvent, new RoutedEventHandler(OnMouseDown));
        }

        /* ----------------------------------- Rendering ----------------------------------  */

        protected struct UniformLocations
        {
            public int ModelMatrix;
            public int ViewMatrix;
            public int ProjectionMatrix;
            public int LightDirection;
        };

        private bool _initialized = false;
        private int _program;
        private UniformLocations _uniformLocations = new UniformLocations();

        float _angle = 0;

        protected void OnRender(System.TimeSpan delta)
        {
            // TODO: count frames per second
            GL.ClearColor(1, 0, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            if (!_initialized)
            {
                /* ---------------------------- Shader creation ----------------------- */
                CreateShaderProgram();
                _uniformLocations.ModelMatrix = GL.GetUniformLocation(_program, "ModelMatrix");
                _uniformLocations.ViewMatrix = GL.GetUniformLocation(_program, "ViewMatrix");
                _uniformLocations.ProjectionMatrix = GL.GetUniformLocation(_program, "ProjectionMatrix");
                _uniformLocations.LightDirection = GL.GetUniformLocation(_program, "LightDirection");
                /* ---------------------------- buffer creation ----------------------- */
                CreateVertexBuffer();
                CreateIndexBuffer();
                CreateVertexArray();
                _initialized = true;
            }

            /* --------------------------- Update Uniforms ----------------------- */
            Math.Vector3 lightDirection = new Math.Vector3(1, 1, 1);
            Math.Matrix4 viewMatrix = Math.Matrix4.LookAt(new Vector3(0, 1, -50),
                                                           new Vector3(0, 0, 0),
                                                           new Vector3(0, 1, 0));
            Math.Matrix4 modelMatrix;
            Math.Matrix4.CreateRotationY(_angle += 0.01f, out modelMatrix);  // Math.Matrix4.CreateScale(3);

            Math.Matrix4 projectionMatrix = Math.Matrix4.CreatePerspectiveFieldOfView(0.5f, 1.0f, 0.1f, 100);
            // Math.Matrix4.CreateOrthographic(3, 3, -10, 10);//  

            GL.UseProgram(_program);
            GL.UniformMatrix4(_uniformLocations.ViewMatrix, false, ref viewMatrix);
            GL.UniformMatrix4(_uniformLocations.ProjectionMatrix, false, ref projectionMatrix);
            GL.UniformMatrix4(_uniformLocations.ModelMatrix, false, ref modelMatrix);
            GL.Uniform3(_uniformLocations.LightDirection, ref lightDirection);

            // TODO: create ibo

            /* --------------------------- Draw ----------------------------------- */
            GL.Disable(OpenGL.EnableCap.CullFace);

            GL.BindBuffer(OpenGL.BufferTarget.ElementArrayBuffer, _ibo);
            GL.BindBuffer(OpenGL.BufferTarget.ArrayBuffer, _vbo);
            GL.BindVertexArray(_vao);

            GL.DrawElements(OpenGL.PrimitiveType.TriangleStrip, 4, OpenGL.DrawElementsType.UnsignedInt, 0);
        }

        private void CreateShaderProgram()
        {
            int vs = GL.CreateShader(OpenGL.ShaderType.VertexShader);
            int fs = GL.CreateShader(OpenGL.ShaderType.FragmentShader);

            string vsSource =
            """
                #version 330
                uniform mat4 ModelMatrix;
                uniform mat4 ViewMatrix;
                uniform mat4 ProjectionMatrix;
                uniform vec3 LightDirection;

                in vec2 Vertex;

                void main() {
                  //  vec4 position = vec4(vertices[gl_VertexID], 1.0);
                    mat4 modelViewProjection = ProjectionMatrix * ViewMatrix * ModelMatrix;
                    gl_Position = modelViewProjection * vec4(Vertex, 0, 1);  //modelViewProjection * position;
                }
            """;

            string fsSource =
            """
                #version 330 
                void main() {
                    gl_FragColor = vec4(0.0, 1.0, 1.0, 1.0);
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

        private void OnMouseDown(object sender, RoutedEventArgs e)
        {
            var mouseEvent = e as System.Windows.Input.MouseButtonEventArgs;
            if (mouseEvent == null) return;

            var position = mouseEvent.GetPosition(null);
            MessageBox.Show(position.X.ToString());
        }


        void CreateVertexBuffer()
        {
            float[] vertices = new float[] {
                -1, -1,
                -1,  1,
                 1,  -1,
                 1, 1
            };
            int size = sizeof(float) * vertices.Length;

            int[] buffer = new int[1];
            GL.CreateBuffers(1, buffer);
            GL.BindBuffer(OpenGL.BufferTarget.ArrayBuffer, buffer[0]);
            GL.BufferData(OpenGL.BufferTarget.ArrayBuffer, size, vertices, OpenGL.BufferUsageHint.DynamicDraw);

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
            GL.VertexAttribPointer(0,
                                   2, VertexAttribPointerType.Float, false,
                                   0, 0);

            _vao = array[0];
        }

        void CreateIndexBuffer()
        {
            System.Console.WriteLine("Created index buffer");

            uint[] indices = new uint[] { 0, 1, 2, 3 };
            int size = sizeof(uint) * indices.Length;

            int[] buffer = new int[1];
            GL.CreateBuffers(1, buffer);
            GL.BindBuffer(OpenGL.BufferTarget.ElementArrayBuffer, buffer[0]);
            GL.BufferData(OpenGL.BufferTarget.ElementArrayBuffer, size, indices, OpenGL.BufferUsageHint.DynamicDraw);

            _ibo = buffer[0];
            System.Console.WriteLine("Created index buffer id=" + _ibo);
        }

        private int _vbo;
        private int _ibo;
        private int _vao;

        private bgl.Input.Arcball? _arcball;
        private Scene? _scene;

    }
    // DataModel

}  // namespace bgl