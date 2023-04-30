using OpenTK.Graphics.OpenGL;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace bgl.Graphics.Core
{
    using ImageSharp = SixLabors.ImageSharp;
    using OpenGL = OpenTK.Graphics.OpenGL;

/*


        public readonly OpenGL.PixelFormat Format = OpenGL.PixelFormat.Rgba;  // TODO
        public readonly OpenGL.PixelType Type = OpenGL.PixelType.Byte;        // TODO

 */
    public class Texture
    {
        public const PixelInternalFormat internalFormat = PixelInternalFormat.Rgba32f;

        // TODO: set sampler
        // TODO: destuctor

        public Texture(string path)
        {
            _texture = GL.GenTexture();
            LoadImage(path);
            Upload();
        }

        public void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, _texture);
        }

        public void Bind(uint textureUnit)
        {
            Bind();
            GL.ActiveTexture(OpenGL.TextureUnit.Texture0 + (int)textureUnit);
        }

        private void LoadImage(string path)
        {
            //Load the image
            var image = ImageSharp.Image.Load<Rgba32>(path);
            image.Mutate(x => x.Flip(FlipMode.Vertical));

            _pixels = new byte[4 * image.Width * image.Height];
            image.CopyPixelDataTo(_pixels);
            _width = image.Width;
            _height = image.Height;

            System.Console.WriteLine("Loaded image " + path);
        }

        private void Upload()
        {
            Bind();
            float[] borderColor = { 1.0f, 1.0f, 0.0f, 1.0f };
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureBorderColor,
                borderColor
            );
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureWrapS,
                (int)TextureWrapMode.Repeat
            );
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureWrapT,
                (int)TextureWrapMode.Repeat
            );
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Linear
            );
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Linear
            );
            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                _width,
                _height,
                0,
                OpenGL.PixelFormat.Rgba,
                PixelType.UnsignedByte,
                _pixels
            );
        }

        byte[] _pixels = new byte[0];
        int _width;
        int _height;
        int _texture;

    }

    class TextureLoader
    {
        public static Texture? Load(in string path)
        {
            Image image = new Image(""); /////// TODODODODOO"!!!!!!!! Image.FromFile(path);
            // var format = GetPixelFormat(image);
            //   var type = GetPixelType(image);
            byte[] pixels = new byte[0]; // TODO

            //    return new Texture(pixels, (int) image.Width, (int) image.Height, format, type);
            return null; // new Texture(pixels, 0,0, 0, 0)
        }
        /*
                private static OpenGL.PixelFormat GetPixelFormat(in Image image)
                {

                    switch (image.Format)
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
            */
    }
} // namespace bgl
