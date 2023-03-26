using OpenTK.Wpf;
using OpenTK.Graphics.OpenGL;
using System.Windows;

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
            var settings = new GLWpfControlSettings { MajorVersion = 3, MinorVersion = 3 };

            // this.Width = width;
            // this.Height = height;
            this.Start(settings);

            _renderer = new bgl.Renderer();


            this.Render += OnRender;
            this.MouseDown += OnMouseDown;

            // this.AddHandler(MouseDownEvent, new RoutedEventHandler(OnMouseDown));
        }


        private void OnMouseDown(object sender, RoutedEventArgs e)
        {
            var mouseEvent = e as System.Windows.Input.MouseButtonEventArgs;
            if (mouseEvent == null)
                return;

            var position = mouseEvent.GetPosition(this);
            MessageBox.Show(position.X.ToString() + "," + position.Y.ToString());
        }


        protected void OnRender(System.TimeSpan delta)
        {
            _renderer.Render(delta);
        }

        private bgl.Renderer _renderer;
        private bgl.Input.Arcball? _arcball;

    }

    // DataModel
} // namespace bgl
