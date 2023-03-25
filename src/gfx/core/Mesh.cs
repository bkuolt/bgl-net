using OpenTK.Graphics.OpenGL;
using OpenGL = OpenTK.Graphics.OpenGL;

namespace bgl.Graphics.Core
{
    using Matrix4D = OpenTK.Mathematics.Matrix4;

    public class VertexBuffer : Buffer
    {
        public VertexBuffer(byte[] data)
            : base(BufferTarget.ElementArrayBuffer, data) { }
    }

    public class IndexBuffer : Buffer
    {
        public IndexBuffer(byte[] data)
            : base(BufferTarget.ArrayBuffer, data) { }
    }

    public class Mesh
    {
        public Mesh() { }

        public Mesh(
            in BufferView vertexBufferView,
            in BufferView indexBufferView,
            in VertexArray vao,
            OpenGL.PrimitiveType primitiveType
        )
        {
            //        vertexBufferView = new BufferView();
            //       indexBufferView = new BufferView();
            this.primitiveType = primitiveType;
        }

        ~Mesh()
        {
            // TODO
        }

        public void Draw(Matrix4D w, Matrix4D s)
        {
#if false

            vertexArray.Bind();
            GL.DrawElements(
                primitiveType,
                (int)indexAcessor.Count,
                OpenGL.DrawElementsType.UnsignedInt,
                indexAcessor.BufferView.Offset
            );
#endif
        }

        private Accessor? indexAcessor;
        private Accessor[]? vertexAcessors;

        private VertexArray? vertexArray;
        private OpenGL.PrimitiveType? primitiveType;
        private Material? material;
    };
}
