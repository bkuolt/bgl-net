using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace LearnOpenTK
{
    public static class Program
    {
        public static void Main()
        {
            // This line creates a new instance, and wraps the instance in a using statement so it's automatically disposed once we've exited the block.
            using (bgl.Window window = new bgl.Window(800, 600, "BGL.NET Tech Demo"))
            {
                window.Run();
            }

            // TODO:: enable console (WPF only) 
            // TODO: test GLTF parser;
            // TODO: show model information
        }

    }

}

