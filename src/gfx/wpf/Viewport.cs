using OpenTK.Wpf;
using OpenTK.Graphics.OpenGL;
using System.Windows;
using OpenTK.Mathematics;

namespace bgl.WPF
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
            if (mouseEvent == null) return;

            var position = mouseEvent.GetPosition(null);
            MessageBox.Show(position.X.ToString());
        }


        private bgl.Input.Arcball _arcball;
        private Scene _scene;

    }
    // DataModel

}  // namespace bgl