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
        public Viewport(int width = 300, int height = 300)
        {
            var settings = new GLWpfControlSettings
            {
                MajorVersion = 4,
                MinorVersion = 5
            };

            this.Width = width;
            this.Height = height;
            this.Start(settings);

            this.Render += OnRender;
            this.MouseDown += OnMouseDown;

            this.AddHandler(MouseDownEvent, new RoutedEventHandler(OnMouseDown));
        }

        /* ----------------------------------- Rendering ----------------------------------  */
        
        protected struct UniformLocations  {
            public int ModelMatrix;
            public int ViewMatrix;
            public int ProjectionMatrix;
            public int LightDirection;
        };

        private bool _initialized = false;
        private int _program;
        private UniformLocations _uniformLocations = new UniformLocations();

        protected void OnRender(System.TimeSpan delta)
        {
            // TODO: count frames per second
            GL.ClearColor(1, 0, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            /* ---------------------------- Shader creation ----------------------- */
            if (!_initialized) {
                CreateShaderProgram();
                _uniformLocations.ModelMatrix = GL.GetUniformLocation(_program, "ModelMatrix");
                _uniformLocations.ViewMatrix  = GL.GetUniformLocation(_program, "ViewMatrix");
                _uniformLocations.ProjectionMatrix  = GL.GetUniformLocation(_program, "ProjectionMatrix");
                _uniformLocations.LightDirection  = GL.GetUniformLocation(_program, "LightDirection");
                _initialized = true;
            }

            /* --------------------------- Update Uniforms ----------------------- */
            Math.Vector3 lightDirection = new Math.Vector3(1, 1, 1);
            Math.Matrix4 viewMatrix  = Math.Matrix4.LookAt(new Vector3(0, 0, -5),
                                                           new Vector3(0, 0, 0),
                                                           new Vector3(0, 1, 0));
            Math.Matrix4 modelMatrix =  Math.Matrix4.CreateScale(3);
            Math.Matrix4 projectionMatrix = Math.Matrix4.CreateOrthographic(2, 2, -10, 10);//  .CreatePerspectiveFieldOfView(0.5f, 1.0f, 0.1f, 100);
         
            GL.UseProgram(_program);
            GL.UniformMatrix4(_uniformLocations.ViewMatrix, false, ref viewMatrix);
            GL.UniformMatrix4(_uniformLocations.ProjectionMatrix, false, ref projectionMatrix);
            GL.UniformMatrix4(_uniformLocations.ModelMatrix, false, ref modelMatrix);
            GL.Uniform3(_uniformLocations.LightDirection, ref lightDirection);

            // TODO: create ibo

            /* --------------------------- Draw ----------------------------------- */
            GL.Disable(OpenGL.EnableCap.CullFace);
            GL.DrawArrays(OpenGL.PrimitiveType.Quads, 0, 8);
        }

        private void CreateShaderProgram() {
            int vs = GL.CreateShader(OpenGL.ShaderType.VertexShader);
            int fs = GL.CreateShader(OpenGL.ShaderType.FragmentShader);

            string vsSource = 
            """
                #version 330 core
                uniform mat4 ModelMatrix;
                uniform mat4 ViewMatrix;
                uniform mat4 ProjectionMatrix;
                uniform vec3 LightDirection;

                const vec3 vertices[8] = {
                    vec3(-1, -1, 1),
                    vec3(-1,  1, 1),
                    vec3( 1,  1, 1),
                    vec3( 1, -1, 1),

                    vec3(-1, -1, -1),
                    vec3(-1,  1, -1),
                    vec3( 1,  1, -1),
                    vec3( 1, -1, -1)
                };

                void main() {
                    vec4 position = vec4(vertices[gl_VertexID], 1.0);
                    mat4 modelViewProjection = ProjectionMatrix * ViewMatrix * ModelMatrix;
                    gl_Position = modelViewProjection * position;
                }
            """;

            string fsSource = 
            """
                #version 330 core
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
            
            
            _program = GL.CreateProgram();
            GL.AttachShader(_program, vs);
            GL.AttachShader(_program, fs);
            GL.LinkProgram(_program);
            
            int linkStatus;
            GL.GetProgram(_program, OpenGL.GetProgramParameterName.LinkStatus, out linkStatus);
            if (linkStatus == 0) {
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


        private bgl.Input.Arcball _arcball;
        private Scene _scene;

    }
    // DataModel

}  // namespace bgl