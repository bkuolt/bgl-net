

using System.Windows;
using OpenTK.Mathematics;

namespace bgl {
    public class SceneView : bgl.WPF.Viewport {

        public SceneView() {
            _scene = new Scene();
            _camera = new Camera();
            _arcball = new Input.Arcball(this, _camera);

            // TODO:: set default background
        }

        private void OnMouseDown(object sender, RoutedEventArgs e)
        {
            var mouseEvent = e as System.Windows.Input.MouseButtonEventArgs;
            if (mouseEvent == null) {
                return;
            }

            var position = mouseEvent.GetPosition(this);

            _scene.Pick( new Vector2((float) position.X, (float) position.Y),
                         new Vector2((float) this.Width, (float) this.Height));


            MessageBox.Show(position.X.ToString() + "," + position.Y.ToString());
        }


        private bgl.Scene _scene;
        private Camera _camera;
        private Input.Arcball _arcball;
    };

}