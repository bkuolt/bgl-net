
using OpenTK.Graphics.OpenGL;


using OpenTK.Windowing.Desktop;  // GameWindow


using OpenTK.Windowing.Common;  // events
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace bgl
{
    class Render
    {
        void test()
        {
            GL.ClearColor(1.0f, 0.3f, 0.3f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            //Code goes here.

            var shader = new bgl.Shader("Shader/shader.vert");
        }
    }

    //using GL = OpenTK.Graphics.OpenGL.GL;



}  // namespace bgl