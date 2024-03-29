namespace bgl.Graphics.Core
{
    public class Material
    {
        public readonly Texture[]? Textures;
        public readonly UniformBuffer? UniformBuffer;
        public readonly ShaderProgram? Program;

        public Material(in Texture[] textures, in UniformBuffer ubo, in ShaderProgram program)
        {
            // TODO
        }

        public Material()
        {
            // TODO
        }

        public void Bind()
        {
#if false
            int unit = (int)OpenGL.TextureUnit.Texture0;
            for (uint index = 0; index < Textures.Length; ++index, ++unit)
            {
                GL.BindTextureUnit(unit, Textures[index]);
            }
                UniformBuffer.Bind();
#endif
        }
    }
}
