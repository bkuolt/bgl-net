using System.Security.AccessControl;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenGL = OpenTK.Graphics.OpenGL;



namespace bgl
{
    public class Image
    {
        public readonly byte[] Data;
        public uint Width;
        public uint Height;

        public readonly OpenGL.PixelFormat Format;
        public readonly OpenGL.DataType Type;

        public Image()
        {
            // TODO
        }

        public Image(uint width, uint height, byte[] data, OpenGL.PixelFormat format, OpenGL.PixelType type)
        {
            // TODO
        }
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
}