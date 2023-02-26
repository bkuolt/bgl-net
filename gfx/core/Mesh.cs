using OpenTK.Graphics.OpenGL;
using OpenGL = OpenTK.Graphics.OpenGL;

namespace bgl
{
    public class VertexBuffer : Buffer
    {
        public VertexBuffer(byte[] data)
            : base(BufferTarget.ElementArrayBuffer, data)
        { }
    }
    public class IndexBuffer : Buffer
    {
        public IndexBuffer(byte[] data)
            : base(BufferTarget.ArrayBuffer, data)
        { }
    }

    class Mesh
    {
        public Mesh(in BufferView vertexBufferView,
                    in BufferView indexBufferView,
                    in VertexArray vao,
                    OpenGL.PrimitiveType primitiveType)
        {
    //        vertexBufferView = new BufferView();
     //       indexBufferView = new BufferView();
            this.primitiveType = primitiveType;
        }

        ~Mesh()
        {
            // TODO
        }
        public void Draw()
        {
            vertexArray.Bind();
            GL.DrawElements(primitiveType, (int) indexAcessor.Count, OpenGL.DrawElementsType.UnsignedInt, indexAcessor.BufferView.Offset);
        }

        private Accessor indexAcessor;
        private Accessor[] vertexAcessors;

        private VertexArray vertexArray;
        private OpenGL.PrimitiveType primitiveType;
        private Material? material;
    };

}