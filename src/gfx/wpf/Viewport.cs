using OpenTK.Wpf;
using OpenTK.Graphics.OpenGL;
using System.Windows;
using OpenTK.Mathematics;

using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace bgl.WPF
{
    using OpenGL = OpenTK.Graphics.OpenGL;
    using Math = OpenTK.Mathematics;
    using ImageSharp = SixLabors.ImageSharp;

    public class Texture
    {
        byte[] _pixels = new byte[0];
        int _width;
        int _height;
        int _texture;

        public Texture(string path)
        {
            _texture = GL.GenTexture();
            LoadImage(path);
            Upload();
        }

        public void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, _texture);
        }

        public void Bind(uint textureUnit)
        {
            Bind();
            GL.ActiveTexture(OpenGL.TextureUnit.Texture0 + (int)textureUnit);
        }

        private void LoadImage(string path)
        {
            //Load the image
            var image = ImageSharp.Image.Load<Rgba32>(path);
            image.Mutate(x => x.Flip(FlipMode.Vertical));

            _pixels = new byte[4 * image.Width * image.Height];
            image.CopyPixelDataTo(_pixels);
            _width = image.Width;
            _height = image.Height;

            System.Console.WriteLine("Loaded image " + path);
        }

        private void Upload()
        {
            Bind();
            float[] borderColor = { 1.0f, 1.0f, 0.0f, 1.0f };
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureBorderColor,
                borderColor
            );
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureWrapS,
                (int)TextureWrapMode.Repeat
            );
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureWrapT,
                (int)TextureWrapMode.Repeat
            );
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Linear
            );
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Linear
            );
            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                _width,
                _height,
                0,
                PixelFormat.Rgba,
                PixelType.UnsignedByte,
                _pixels
            );
        }
    }

    class IViewport
    {
        // TODO
    }

    public class Viewport : OpenTK.Wpf.GLWpfControl
    {
        public Viewport()
        {
            var settings = new GLWpfControlSettings { MajorVersion = 3, MinorVersion = 3 };

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

                GL.Uniform1(textureLocation, (int) 1);

                
                /* ---------------------------- Buffer Creation ----------------------- */
                CreateVertexBuffer();
                CreateIndexBuffer();
                CreateVertexArray();

                _texture = new Texture("tests/glTF/Default_metalRoughness.jpg");

                _stopwatch.Start();
            }
            catch (System.Exception exception)
            {
                System.Windows.MessageBox.Show(
                    exception.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
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

            _texture.Bind(1);

            GL.DrawElements(
                OpenGL.PrimitiveType.Triangles,
                (3 * 2) * 2,
                OpenGL.DrawElementsType.UnsignedInt,
                0
            );
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

                in vec3 Vertex;

                out vec2 TexCoord;

                void main() {
                  //  vec4 position = vec4(vertices[gl_VertexID], 1.0);
                    mat4 modelViewProjection = ProjectionMatrix * ViewMatrix * ModelMatrix;
                    gl_Position = modelViewProjection * vec4(Vertex, 1);  //modelViewProjection * position;
                    
                    
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

                void main() {

                    gl_FragColor = texture(Texture, TexCoord);
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
            if (mouseEvent == null)
                return;

            var position = mouseEvent.GetPosition(this);
            MessageBox.Show(position.X.ToString() + "," + position.Y.ToString());
        }

        void CreateVertexBuffer()
        {
            float[] vertices = new float[]
            {
                -1,
                -1,
                -1, // position
                -1,
                1,
                -1, // position
                1,
                1,
                -1, // position
                1,
                -1,
                -1, // position
                -1,
                -1,
                1, // position
                -1,
                1,
                1, // position
                1,
                1,
                1, // position
                1,
                -1,
                1, // position
            };

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
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

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

            uint[] indices = new uint[]
            {
                // front side
                0,
                1,
                2,
                0,
                2,
                3,
                // backside
                4,
                5,
                6,
                4,
                6,
                7,
                // left side
                5,
                4,
                0,
                0,
                1,
                5,
                // right side
                3,
                2,
                6,
                6,
                7,
                3
            };
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

        private Texture _texture;

        private bgl.Input.Arcball? _arcball;
        private Scene? _scene;
    }

    // DataModel
} // namespace bgl
