using System;
using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace Fitch
{
    public class Game
    {
        GameWindow window;
        public Game(GameWindow window)
        {

            window.Load += Window_Load;
            window.Closing += Window_Closing;
            window.UpdateFrame += Window_UpdateFrame;
            window.RenderFrame += Window_RenderFrame;

            this.window = window;

        }

        void Window_Load(object sender, EventArgs e)
        {

        }

        void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        void Window_UpdateFrame(object sender, FrameEventArgs e)
        {

        }

        void Window_RenderFrame(object sender, FrameEventArgs e)
        {

            GL.ClearColor(Color.Aqua);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            window.SwapBuffers();

        }
    }
}
