

namespace bgl {
    public struct vec2 {
        public vec2() {
            x = 0;
            y = 0;
        }
        public int x;
        public int y;
    }

        public struct BoundingBox {
            public BoundingBox() {
                min.x = System.Int32.MaxValue;
                min.y = System.Int32.MaxValue;
                max.x = System.Int32.MinValue;
                max.y = System.Int32.MinValue;
            }


            public vec2 Max(BoundingBox a, BoundingBox b)
            {
                vec2 max;
                max.x = System.Math.Max(a.max.x, b.max.x);
                max.y = System.Math.Max(a.max.y, b.max.y);
                return max;
            }

            public int GetWidth() {
                return max.x - min.x;
            }

            public int GetHeight() {
                return max.x - min.x;;
            }            

             public vec2 min;
             public vec2 max;
        }
}