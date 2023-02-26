
using OpenTK.Graphics.OpenGL;


using OpenTK.Windowing.Desktop;  // GameWindow


using OpenTK.Windowing.Common;  // events
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace bgl
{
    public class Renderer
    {
        private static bgl.ShaderProgram program; 

        public static void Draw()
        {
            GL.ClearColor(1.0f, 0.3f, 0.3f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            //Code goes here.

            // crashes --> program = new bgl.ShaderProgram( new string[]{ "Shader/shader.vs" });
        }
    }

    //using GL = OpenTK.Graphics.OpenGL.GL;



}  // namespace bgl