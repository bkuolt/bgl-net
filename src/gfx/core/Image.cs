using System.Security.AccessControl;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenGL = OpenTK.Graphics.OpenGL;

using FreeImageAPI;

namespace bgl
{
    public class Image
    {
        public byte[] Data
        {
            get => _data;
        }
        public uint Width;
        public uint Height;

        public readonly OpenGL.PixelFormat Format = OpenGL.PixelFormat.Rgba;
        public readonly OpenGL.PixelType Type = OpenGL.PixelType.Byte;

        System.IO.MemoryStream CreateMemoryStream(uint width, uint height)
        {
            _data = new byte[Width * Height * 4];
            return new System.IO.MemoryStream(_data);
        }

        public Image(in string path)
        {
            FREE_IMAGE_FORMAT format = FreeImage.GetFileType(
                path,
                0 /* unused */
            );
            if (format == FREE_IMAGE_FORMAT.FIF_UNKNOWN) { }
            FIBITMAP _bitmap = FreeImage.Load(format, path, 0);
            _bitmap = FreeImage.ConvertTo32Bits(_bitmap);
            _bitmap = FreeImage.ConvertToType(_bitmap, FREE_IMAGE_TYPE.FIT_BITMAP, true);

            Width = FreeImage.GetWidth(_bitmap);
            Height = FreeImage.GetHeight(_bitmap);
            const int channels = 4;

            uint size = Width * Height * channels * (FreeImage.GetBPP(_bitmap) / 8);
            _data = new byte[size];

            System.IO.MemoryStream stream = new System.IO.MemoryStream(_data);
            FreeImage.SaveToStream(_bitmap, stream, format);
            FreeImage.Unload(_bitmap);
        }

#if false
        public Image(byte[] data, in string extension)
        {
            FIBITMAP _bitmap = new FIBITMAP();
            FREE_IMAGE_FORMAT format = FreeImage.GetFileType(extension, 0 /* unused */);

            _data = (byte[]) data.Clone();
            System.IO.MemoryStream stream = new System.IO.MemoryStream(_data);
            var flags = FreeImageAPI.FREE_IMAGE_LOAD_FLAGS.DEFAULT;
            _bitmap = FreeImage.LoadFromStream(stream, flags);
            FreeImage.Unload(_bitmap);
        }
#endif
        ~Image() { }

        private byte[] _data;
    }

    public interface ImageLoader
    {
        public Image Load(in string path)
        {
            string extension = System.IO.Path.GetExtension(path);
            return loaders[extension].Load(path);
        }

        public static void Add(in string extension, in ImageLoader loader)
        {
            if (loader == null)
            {
                throw new System.Exception("invalid loader 'null'");
            }
            string trimmedExtension = extension.Trim();
            loaders.Add(trimmedExtension, loader);
        }

        public static void Remove(in ImageLoader loader)
        {
            // TODO
        }

        private static Dictionary<string, ImageLoader> loaders;
    }

    // TODO: tests
}
