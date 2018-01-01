using System;
using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace Fitch
{
    public class Game
    {
        GameWindow window;
        Texture2D texture;
        World world;
        Player player;

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

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);

            GL.Enable(EnableCap.Texture2D);

            world = new World(50, new Vector2(10, 10));
            texture = ContentPipe.LoadTexture("penguin.png");
            player = new Player(new Vector2(0, 0), 50, 70);

        }

        void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        void Window_UpdateFrame(object sender, FrameEventArgs e)
        {

        }

        void Window_RenderFrame(object sender, FrameEventArgs e)
        {

            Matrix4 projMat = Matrix4.CreateOrthographicOffCenter(0, window.Width, window.Height, 0, 0, 1);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projMat);

            GL.ClearColor(Color.Aqua);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            SpriteBatch.DrawBlock(BlockType.Solid, new Vector2(0, 0), world.blockSize);

            window.SwapBuffers();

        }
    }
}
