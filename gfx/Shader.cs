using OpenTK.Graphics.OpenGL;


namespace bgl {


class Shader {

    private int shader;
    private int program;

    public Shader(string path) {
        // TODO
    }

    ~Shader() {
        // TODO: delete shaders
    }

    private string LoadSource() {
        return ""; // TODO
    }

    private void Compile() {
        // TODO
    
        GL.CreateProgram();

        //GL.LinkProgram(program);
        //GL.GetShaderInfoLog();

        string[] sourceCode = { "" };
        int[] lengths = { 0 };
        GL.ShaderSource(shader, sourceCode.Length, sourceCode, lengths);
    }

    public void asds() {

        int shader = GL.CreateShader(ShaderType.FragmentShader);
     


    }

};



}