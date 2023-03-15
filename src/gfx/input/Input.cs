using System.Windows;
using OpenTK.Mathematics;

namespace bgl.Input
{
    class Arcball
    {
        /// <summary>
        /// The current radius, which is the distance to center of the sphere
        /// </summary>
        public float Radius
        {
            get { return _radius; }
            set {
                _radius = System.MathF.Max(value, System.Single.Epsilon);  // make sure that the radius is not negative
                _radius = System.MathF.Min(MaxRadius, _radius);
            }
        }

        public float[] Angles = new float[2];

        /// <summary>
        /// Zoom speed in world units per second.
        /// </summary>
        public float ZoomSpeed
        {
            get { return _zoomSpeed; }
            set { _zoomSpeed =  System.MathF.Max(value, System.Single.Epsilon); }
        }
        
        public float MinRadius
        {
            get { return _minRadius; }
            set { _minRadius = System.MathF.Max(value, System.Single.Epsilon); }
        }

        public float MaxRadius
        {
            get { return _maxRadius; }
            set { _maxRadius = System.MathF.Max(value, 0); }  // make sure it's not negative
        }

        public Arcball(in UIElement element)
        {
            if (element != null) {
                element.MouseMove += OnMouseMotion;
                element.MouseWheel += OnMouseWheel;
                _element = element;
            }

        }

        ~Arcball()
        {
            if (_element != null) {
                _element.MouseMove -= OnMouseMotion;
                _element.MouseWheel -= OnMouseWheel;
            }
        }

        private void OnMouseWheel(object sender, RoutedEventArgs e)
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
        readonly UIElement? _element;
        private Point? _lastMousePostion;

        private float _minRadius = System.Single.Epsilon;
        private float _maxRadius = System.Single.MaxValue;
        private float _radius = 5.0f;
        private float _zoomSpeed = 1.0f;
    }

}  // namespace bgl