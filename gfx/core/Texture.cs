using OpenTK.Graphics.OpenGL;
using System.Drawing.Imaging;
using System.Drawing;

using OpenGL = OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace bgl

{
    class Texture
    {
        public const PixelInternalFormat internalFormat = PixelInternalFormat.Rgba32f;

        public Texture(in byte[] pixels,
                       in int width, in int height,
                       in OpenGL.PixelFormat format, in PixelType type)
        {
            handle = GL.GenTexture();
            GL.BindTexture(target, handle);
            Upload(pixels, width, height, format, type);
        }

        ~Texture()
        {
            GL.DeleteTexture(handle);
        }

        public void Bind()
        {
            GL.BindTexture(target, handle);
        }

        public void Upload(in byte[] pixels,
                           in int width, in int height,
                           in OpenGL.PixelFormat format, in PixelType type)
        {
            GL.TexImage2D(target, 0, internalFormat, width, height, 0, format, type, pixels);
            // TODO: mip-maps
        }

        private int handle;
        private TextureTarget target = TextureTarget.Texture2D;
    }

    class TextureLoader
    {
        public static Texture Load(in string path)
        {
            Image image = Image.FromFile(path);
            var format = GetPixelFormat(image);
            var type = GetPixelType(image);
            byte[] pixels = new byte[0];  // TODO

            return new Texture(pixels, image.Width, image.Height, format, type);
        }

        private static OpenGL.PixelFormat GetPixelFormat(in Image image)
        {
            switch (image.PixelFormat)
            {
                case PixelFormat.Format32bppRgb:
                    return OpenGL.PixelFormat.Rgb;
                case PixelFormat.Format32bppArgb:
                    return OpenGL.PixelFormat.Rgba;
                default:
                    throw new System.Exception(image.PixelFormat.ToString() + " is not supported yet");
            }
        }

        private static OpenGL.PixelType GetPixelType(in Image image)
        {
            return OpenGL.PixelType.Byte;
        }
    }

}  // namespace bgl