using System.Windows;
using OpenTK.Mathematics;

namespace bgl.Input
{
    public partial class Arcball
    {
        /// <summary>
        /// The current radius, which is the distance to center of the sphere
        /// </summary>
        public float Radius
        {
            get { return _radius; }
            set
            {
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
            set { _zoomSpeed = System.MathF.Max(value, System.Single.Epsilon); }
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

        public Arcball(in UIElement element, in Camera camera)
        {
            if (element != null)
            {
                element.MouseMove += OnMouseMotion;
                element.MouseWheel += OnMouseWheel;
                _element = element;
            }
        }

        ~Arcball()
        {
            if (_element != null)
            {
                _element.MouseMove -= OnMouseMotion;
                _element.MouseWheel -= OnMouseWheel;
            }
        }
        private partial void OnMouseWheel(object sender, RoutedEventArgs e);

        private partial void OnMouseMotion(object sender, RoutedEventArgs e);

        public partial Vector3 GetPosition();
        public partial Vector3 GetUpVector();
        public partial Matrix4 GetViewMatrix();

        readonly UIElement? _element;
        private Point? _lastMousePostion;

        private float _minRadius = System.Single.Epsilon;
        private float _maxRadius = System.Single.MaxValue;
        private float _radius = 5.0f;
        private float _zoomSpeed = 1.0f;
    }

}  // namespace bgl