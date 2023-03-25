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
            Initialize();

            this.Render += OnRender;
            this.MouseDown += OnMouseDown;

           // this.AddHandler(MouseDownEvent, new RoutedEventHandler(OnMouseDown));
        }

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


        // --------------------------------------------------------------------------------------------------
        private double ConvertToRadians(double angle)
        {
            return (System.Math.PI / 180) * angle;
        }

        private static System.Diagnostics.Stopwatch _stopwatch = new System.Diagnostics.Stopwatch();


        private void Initialize()
        {
            try
            {
                /* ---------------------------- Shader Creation ----------------------- */
                CreateShaderProgram();
                _uniformLocations.ModelMatrix = GL.GetUniformLocation(_program, "ModelMatrix");
                _uniformLocations.ViewMatrix = GL.GetUniformLocation(_program, "ViewMatrix");
                _uniformLocations.ProjectionMatrix = GL.GetUniformLocation(_program, "ProjectionMatrix");
                _uniformLocations.LightDirection = GL.GetUniformLocation(_program, "LightDirection");

                /* ---------------------------- Buffer Creation ----------------------- */
                CreateVertexBuffer();
                CreateIndexBuffer();
                CreateVertexArray();

                _stopwatch.Start();
            }
            catch (System.Exception exception)
            {
                System.Windows.MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void UpdateUniforms()
        {
            // ----------------- View Matrix --------------
            Math.Vector3 lightDirection = new Math.Vector3(1, 1, 1);
            Math.Matrix4 viewMatrix = Math.Matrix4.LookAt(new Vector3(0, 1, -10),
                                                           new Vector3(0, 0, 0),
                                                           new Vector3(0, 1, 0));
            // ----------------- Model Matrix --------------
            Math.Matrix4 modelMatrix;

            double radiansPerSecond = ConvertToRadians(60.0);
            _angle = (float)(_stopwatch.Elapsed.TotalSeconds * radiansPerSecond);
            Math.Matrix4.CreateRotationY(_angle, out modelMatrix);  // Math.Matrix4.CreateScale(3);

            // ----------------- Projection Matrix --------------
            Math.Matrix4 projectionMatrix = Math.Matrix4.CreatePerspectiveFieldOfView(0.5f, 1.0f, 0.1f, 100);

            // ----------------- OpenGL Uniform Upload --------------
            GL.UniformMatrix4(_uniformLocations.ViewMatrix, false, ref viewMatrix);
            GL.UniformMatrix4(_uniformLocations.ProjectionMatrix, false, ref projectionMatrix);
            GL.UniformMatrix4(_uniformLocations.ModelMatrix, false, ref modelMatrix);
            GL.Uniform3(_uniformLocations.LightDirection, ref lightDirection);
        }

        protected void OnRender(System.TimeSpan delta)
        {
            GL.UseProgram(_program);
            UpdateUniforms();

            // TODO: count frames per second
            GL.ClearColor(211 / 256.0f, 211 / 256.0f, 211 / 256.0f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            GL.BindBuffer(OpenGL.BufferTarget.ArrayBuffer, _vbo);
            GL.BindBuffer(OpenGL.BufferTarget.ElementArrayBuffer, _ibo);
            GL.BindVertexArray(_vao);

            GL.Disable(OpenGL.EnableCap.CullFace);

            GL.DrawElements(OpenGL.PrimitiveType.Triangles, (3 * 2) * 2, OpenGL.DrawElementsType.UnsignedInt, 0);
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

                in vec3 Vertex;

                void main() {
                  //  vec4 position = vec4(vertices[gl_VertexID], 1.0);
                    mat4 modelViewProjection = ProjectionMatrix * ViewMatrix * ModelMatrix;
                    gl_Position = modelViewProjection * vec4(Vertex, 1);  //modelViewProjection * position;
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

            var position = mouseEvent.GetPosition(this);
            MessageBox.Show(position.X.ToString() + "," + position.Y.ToString());
        }


        void CreateVertexBuffer()
        {
            float[] vertices = new float[] {
                -1, -1, -1,
                -1,  1, -1,
                 1,  1, -1,
                 1, -1, -1,
                -1, -1, 1,
                -1,  1, 1,
                 1,  1, 1,
                 1, -1, 1,
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
                                   3, VertexAttribPointerType.Float, false,
                                   0, 0);

            _vao = array[0];
        }

        void CreateIndexBuffer()
        {
            System.Console.WriteLine("Created index buffer");

            /*
                5-------- 6
               / 
             /    
            1 ------- 2
            |         |
            |   4     |  7
            |  /      | /
            0 --------3   
             */

            uint[] indices = new uint[] {
                // front side
                0, 1, 2,
                0, 2, 3,
                // backside
                4, 5, 6,
                4, 6, 7,
                
                // left side 
                5,4,0,
                0,1,5,

                // right side 
                3,2,6,
                6,7,3
            };
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