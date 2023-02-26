using System;

using OpenTK.Graphics.OpenGL;
using OpenGL = OpenTK.Graphics.OpenGL;

namespace bgl
{

    class Shader
    {
        private int shader;

        public Shader(in string path)
        {
            ShaderType shaderType = GetShaderType(path);
            shader = GL.CreateShader(shaderType);
            LoadSource(path);
            Compile();
        }
        ~Shader()
        {
            GL.DeleteShader(shader);
        }

        public ShaderType GetShaderType(in string path)
        {
            string? extension = System.IO.Path.GetExtension(path);
            if (extension == null)
            {
                throw new System.Exception("invalid extension");
            }

            switch (extension)
            {
                case ".fs": return ShaderType.VertexShader;
                case ".vs": return ShaderType.FragmentShader;
                case ".gs": return ShaderType.GeometryShader;
            }

            throw new System.Exception("could not determine vertex type");
        }

        private void LoadSource(in string path)
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            int[] lengths = { };
            GL.ShaderSource(shader, lines.Length, lines, lengths);
        }

        private void Compile()
        {

            GL.CompileShader(shader);

            string log = GL.GetShaderInfoLog(shader);

            System.Console.Write("GLSL Log: " + log);


        }
    };


    class ShaderProgram
    {

        private int shader;
        private int program;

        public ShaderProgram(in string paths)
        {
            //    LoadSource(path)
            Link();
        }

        ~ShaderProgram()
        {
            // TODO: delete shaders
        }

        private void Link()
        {
            // TODO
            GL.CreateProgram();
            //GL.LinkProgram(program);
            //GL.GetShaderInfoLog();

        }

        public void Use()
        {

        }
    };

    public class UniformBuffer : Buffer {
        public UniformBuffer(in byte[] data)
            : base(OpenGL.BufferTarget.UniformBuffer, data)
        {}
    }

}