using OpenTK.Graphics.OpenGL;

namespace bgl
{


    class Mesh
    {
        public Mesh(in byte[] indices, in byte[] vertices, in BufferView[] bufferViews)
        {

            // TODO: create VAO from buffer views      
        }

        ~Mesh()
        {
            // TODO
        }
        public void Draw()
        {
            // TODO: bind VAO
            // TODO: bind textures;
            program.Use();
        }

        private bgl.Buffer vbo;
        private bgl.Buffer ibo;
        private bgl.VertexArray vao;

        private bgl.ShaderProgram program;

    };

}