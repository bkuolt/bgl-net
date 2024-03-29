using OpenTK.Wpf;
using System.Windows;

namespace bgl.WPF
{

    class IViewport
    {
        // TODO
    }

    public class Viewport : OpenTK.Wpf.GLWpfControl
    {
        public Viewport()
        {
            var settings = new GLWpfControlSettings { MajorVersion = 3, MinorVersion = 3 };
            this.Start(settings);

            _renderer = new bgl.Renderer();
            // TODO: _arcball = new bgl.Input.Arcball(this);
            
            this.Render += OnRender;
            this.MouseDown += OnMouseDown;
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

        public bgl.Renderer _renderer;
        private bgl.Input.Arcball? _arcball;
    }

    // DataModel
} // namespace bgl
