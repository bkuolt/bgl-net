using OpenTK.Graphics.OpenGL;

namespace bgl {


class Mesh {
    public Mesh(in byte[] indices, in byte[] vertices, in BufferView[] bufferViews) {
       vbo = CreateBuffer(BufferTarget.ArrayBuffer, vertices);
       ibo = CreateBuffer(BufferTarget.ElementArrayBuffer, indices);
       // TODO: create VAO from buffer views      
    }

    ~Mesh() {
        // TODO
    }
    public void Draw()
    {
        // TODO: bind VAO
        // TODO: bind textures;
        program.Use();
    }

    private static int CreateBuffer(BufferTarget target, byte[] data) {
        int handle = GL.GenBuffer();
        GL.BindBuffer(target, handle);
        const BufferUsageHint usageHint = BufferUsageHint.DynamicDraw;
        GL.BufferData(target, data.Length, data, usageHint);
        return handle;
    }
    
    private int vbo;
    private int ibo;
    private int vao;

    private bgl.ShaderProgram program;

};

}