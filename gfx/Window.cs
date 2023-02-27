using OpenTK.Graphics.OpenGL;

using OpenTK.Windowing.Desktop; // GameWindow

using OpenTK.Windowing.Common; // events
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace bgl
{
    public class Window : GameWindow
    {
        public Window(int width, int height, string title)
            : base(
                GameWindowSettings.Default,
                new NativeWindowSettings() { Size = (width, height), Title = title }
            )
        {
            // TODO
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            KeyboardState input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            bgl.Renderer.Draw();
            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }
    }
} // namespace bgl
