using OpenGL = OpenTK.Graphics.OpenGL;
using FreeImageAPI;
using System;

namespace bgl.Graphics.Core
{
    interface IImage
    {
        public byte[] Data { get; }
        public uint Width { get; }
        public uint Height { get; }
        public uint Channels { get; }

        public void Load(in string path);
        public void Load(System.IO.MemoryStream stream, String extension);
    }
 
#if BGL_USE_FREEIMAGE
    public class FreeImageImage : Image
    {
        public byte[] Data { get => _data }
        public uint Width { get => _width; }
        public uint Height { get => _heigth; }
        public uint Channels { get => _channels }

        public Image(in string path)
        {
            Load(path);
        }

        public Image(System.IO.MemoryStream stream, FREE_IMAGE_FORMAT format)
        {
            FIBITMAP bitmap = FreeImage.LoadFromStream(stream);
            Load(bitmap, format);
        }

        public void Load(String path)
        {
            FREE_IMAGE_FORMAT format = FreeImage.GetFileType(path, 0);
            if (format == FREE_IMAGE_FORMAT.FIF_UNKNOWN)
            {
                throw new System.Exception("unknown image format");
            }

            FIBITMAP bitmap = FreeImage.Load(format, path, 0);
            Load(bitmap, format);
        }

        public void Load(FIBITMAP bitmap, String format)
        {
            throw new Exception("not implemented yet");  // TODO
        }

        private void Load(FIBITMAP bitmap, FREE_IMAGE_FORMAT format)
        {
            if (bitmap.IsNull)
            {
                throw new System.Exception("could not load image");
            }

            _width = FreeImage.GetWidth(bitmap);
            _height = FreeImage.GetHeight(bitmap);
            uint bytesPerPixel = FreeImage.GetLine(bitmap) / Width;
            _channels = bytesPerPixel / sizeof(byte);

            bitmap = ConvertImage(bitmap);

            uint size = Width * Height * Channels * (FreeImage.GetBPP(bitmap) / 8);
            _data = new byte[size];

            System.IO.MemoryStream stream = new System.IO.MemoryStream(Data);
            FreeImage.SaveToStream(bitmap, stream, format);
            FreeImage.Unload(bitmap);
        }

        private FIBITMAP ConvertImage(FIBITMAP bitmap)
        {
            var oldImage = bitmap;
            switch (Channels)
            {
                case 4:
                    bitmap = FreeImage.ConvertTo32Bits(bitmap);
                    break;
                case 3:
                    bitmap = FreeImage.ConvertTo24Bits(bitmap);
                    break;
                default:
                    throw new System.Exception("not yet supported");  // TODO
            }

            FreeImage.Unload(oldImage);
            if (bitmap.IsNull)
            {
                throw new System.Exception("image could be converted");
            }

            bitmap = FreeImage.ConvertToType(bitmap, FREE_IMAGE_TYPE.FIT_BITMAP, true);
            if (bitmap.IsNull)
            {
                throw new System.Exception("image could be converted");
            }

            return bitmap;
        }

        private byte[] _data;
        uint _width;
        uint _height;
        uint _channels;
    }
#endif

#if BGL_USE_IMAGESHARP
    public class ImageSharpImage : IImage
    {
        public byte[] Data { get; }
        public uint Width { get => _width; }
        public uint Height { get; }
        public uint Channels { get; }

        /* 
            var image = ImageSharp.Image.Load<Rgba32>(path);
            image.Mutate(x => x.Flip(FlipMode.Vertical));

            _pixels = new byte[4 * image.Width * image.Height];
            image.CopyPixelDataTo(_pixels);
            _width = image.Width;
            _height = image.Height;
        */
    }
#endif

    class ImageLoader {
        public static IImage LoadImage(String path) {
            return null;  // TODO
        }
    }

}
