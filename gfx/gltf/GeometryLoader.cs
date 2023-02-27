#if false

using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace wpf_demo
{
    public class GLTFImport
    {
        record BufferView
        {
            public readonly int ByteOffset;

            int size;
            int stride;
        };

        class Accessor
        {

            public readonly int ByteOffset;
            public readonly int ByteStride;
            public readonly int Count;

            string componentType;
            string accessorType;
            int count;

            int GetSize(string accesorType, string type)
            {

                switch (accessorType)
                {
                    case "SCALAR":
                    case "VEC2":
                    case "VEC3":
                    case "VEC4":
                    case "MAT2":
                    case "MAT3":
                    case "MAT4":
                        return 0;
                }

                return 0;//GetSize(0,0);
            }

            int GetTypeSize(int componentCount, int componentSize)
            {
                //return GetTypeSize(type) * 
                return 0;
            }

            BufferView bufferView;
        };

        class Buffer
        {
            public readonly byte[] _data;

            public Buffer(in string path)
            {
                long size = new System.IO.FileInfo(path).Length;
                System.IO.FileStream stream = System.IO.File.OpenRead(path);
                var writer = new System.IO.BinaryReader(stream, System.Text.Encoding.UTF8, false);
                _data = new byte[size];
            }

            byte[] GetData(in Accessor accessor, in BufferView bufferView)
            {
                for (int i = 0; i < accessor.Count; ++i)
                {
                    int index = bufferView.ByteOffset + accessor.ByteOffset + (accessor.ByteOffset * i);
                }
                return new byte[1];
                // return new System.ReadOnlySpan<byte>(_data, accessor.BytesOffset, accessor.Count);
            }
        };

        Point3DCollection? GetPositions(Buffer buffer, in Accessor accessor)
        {
            return null;
        }

        Vector3DCollection? GetNormals(Buffer buffer, in Accessor accessor)
        {
            return null;
        }

        PointCollection? GetTextureCoordinates(in Buffer buffer, in Accessor accessor)
        {
            return null;
        }

        Int32Collection? GetIndices(in Buffer buffer, in Accessor accessor)
        {
            return null;
        }

        /// <summary>
        /// /////////////////////////////////////////////
        /// </summary>
        /// <value></value>


        record Mesh
        {
            BufferView position;
            BufferView? normal;
            BufferView? textureCoordinates;
            // TODO
        };

        public class GeometryLoader
        {
            private Point3DCollection _positions;
            private Vector3DCollection _normals;
            private Int32Collection _indices;

            public MeshGeometry3D GetGeometry()
            {
                MeshGeometry3D geometry = new MeshGeometry3D();
                geometry.Positions = _positions;
                geometry.Normals = _normals;
                geometry.TriangleIndices = _indices;
                return geometry;
            }
#if __0
        public MeshGeometry3D GetGeometry(in BufferView view,) {
            return new MeshGeometry3D();  // TODO
        }
#endif
        }

    }

}


#endif
