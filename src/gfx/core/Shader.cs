using System;

using OpenTK.Graphics.OpenGL;
using OpenGL = OpenTK.Graphics.OpenGL;

namespace bgl.Graphics.Core
{
    class Shader
    {
        private int handle;

        public Shader(in string path)
        {
            ShaderType shaderType = GetShaderType(path);
            handle = GL.CreateShader(shaderType);
            LoadSource(path);
            Compile();
        }

        ~Shader()
        {
            GL.DeleteShader(handle);
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
                case ".fs":
                    return ShaderType.VertexShader;
                case ".vs":
                    return ShaderType.FragmentShader;
                case ".gs":
                    return ShaderType.GeometryShader;
            }

            throw new System.Exception("could not determine vertex type");
        }

        public static implicit operator int(Shader shader) => shader.handle;

        private void LoadSource(in string path)
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            int[] lengths = { };
            GL.ShaderSource(handle, lines.Length, lines, lengths);
        }

        private void Compile()
        {
            GL.CompileShader(handle);
            string log = GL.GetShaderInfoLog(handle);
            System.Console.Write("GLSL Log: " + log);
        }
    };

    public class ShaderProgram
    {
        public ShaderProgram()
        {
            handle = GL.CreateProgram();
        }

        public ShaderProgram(in string[] paths)
            : base()
        {
            foreach (var path in paths)
            {
                Shader shader = new Shader(path);
                Attach(shader);
            }
            Link();
        }

        ~ShaderProgram()
        {
            GL.DeleteProgram(handle);
        }

        private void Attach( /* in OpenGL.ShaderType type,*/
            in Shader shader
        )
        {
            GL.AttachShader(handle, shader);
        }

        private void Link()
        {
            GL.LinkProgram(handle);

            int linkStatus;
            GL.GetProgram(handle, OpenGL.GetProgramParameterName.LinkStatus, out linkStatus);

            if (linkStatus == 0)
            {
                byte[] buffer = new byte[4096];
                string infoLog;
                int length;
                GL.GetProgramInfoLog(handle, buffer.Length, out length, out infoLog);
                Console.Write(infoLog);
            }
        }

        public void Use()
        {
            GL.UseProgram(handle);
        }

        private int handle;
    };

    public class UniformBuffer : Buffer
    {
        public UniformBuffer(in byte[] data)
            : base(OpenGL.BufferTarget.UniformBuffer, data) { }
    }
}
