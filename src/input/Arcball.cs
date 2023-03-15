using System.Windows;
using OpenTK.Mathematics;

namespace bgl.Input
{
    public partial class Arcball
    {
        private partial void OnMouseWheel(object sender, RoutedEventArgs e)
        {
            var mouseWheelEvent = e as System.Windows.Input.MouseWheelEventArgs;
            if (mouseWheelEvent == null)
            {
                return;
            }

            Radius += mouseWheelEvent.Delta * ZoomSpeed;
            Radius = System.MathF.Max(Radius, MinRadius);
            Radius = System.MathF.Min(Radius, MaxRadius);
        }

        private partial void OnMouseMotion(object sender, RoutedEventArgs e)
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

        public partial Vector3 GetPosition()
        {
            return new Vector3();  // TODO
        }

        public partial Vector3 GetUpVector()
        {
            return new Vector3();  // TODO
        }

        public partial Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(GetPosition(), new Vector3(0, 0, 0), GetUpVector());
        }
    }

}  // namespace bgl