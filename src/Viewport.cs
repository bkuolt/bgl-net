using OpenTK.Wpf;
using OpenTK.Graphics.OpenGL;
using System.Windows;


using OpenTK.Mathematics;
namespace bgl
{

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

        protected void OnRender(System.TimeSpan delta)
        {
            // TODO: count frames per second

            GL.ClearColor(1, 0, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        private void OnMouseDown(object sender, RoutedEventArgs e)
        {
            var mouseEvent = e as System.Windows.Input.MouseButtonEventArgs;

            var position = mouseEvent.GetPosition(null);
            MessageBox.Show(position.X.ToString());
        }


        private Arcball _arcball;
        private Scene _scene;

    }

    // DataModel


    class Arcball
    {
        public float Radius;
        public float[] Angles = new float[2];

        public float ZoomSpeed = 1.0f;    // TODO: declare as property
        public float? MinRadius = 1.0f;   // TODO: declare as property
        public float? MaxRadius = 10.0f;  // TODO: declare as property

        public Arcball(in UIElement element)
        {
            element.MouseMove += OnMouseMotion;
            element.MouseWheel += OnMouseWheel;
            _element = element;
        }
      
        ~Arcball()
        {
            _element.MouseMove -= OnMouseMotion;
            _element.MouseWheel -= OnMouseWheel;
        }

        private void OnMouseWheel(object sender, RoutedEventArgs e)
        {
            var mouseWheelEvent = e as System.Windows.Input.MouseWheelEventArgs;
            if (mouseWheelEvent == null)
            {
                return;
            }

            Radius += mouseWheelEvent.Delta * ZoomSpeed;
            if (MinRadius != null)
            {
                Radius = System.MathF.Max(Radius, MinRadius.Value);
            }
            else
            {
                Radius = System.MathF.Max(Radius, 0.0f);
            }

            if (MaxRadius != null)
            {
                Radius = System.MathF.Min(Radius, MaxRadius.Value);
            }
        }

        private void OnMouseMotion(object sender, RoutedEventArgs e)
        {
            var mouseEvent = e as System.Windows.Input.MouseEventArgs;
            if (mouseEvent == null)
            {
                return;
            }

            if (mouseEvent.XButton1 == System.Windows.Input.MouseButtonState.Pressed)
            {
                var delta = _lastMousePostion != null ? _lastMousePostion - mouseEvent.GetPosition(null) : new Vector();
                _lastMousePostion = mouseEvent.GetPosition(null);
                Angles[0] += (float)delta.Value.X;
                Angles[1] += (float)delta.Value.Y;
            }
        }

        public Vector3 GetPosition()
        {
            return new Vector3();  // TODO
        }

        public Vector3 GetUpVector()
        {
            return new Vector3();  // TODO
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(GetPosition(), new Vector3(0, 0, 0), GetUpVector());
        }

        private Point? _lastMousePostion;
        readonly UIElement _element;
    }

}  // namespace bgl