using System;
using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace Fitch
{
    public class Game
    {
        GameWindow window;
        World world;
        Player player;
        public static int i;
        List<Block> blocks;
        Vector2 removePos = new Vector2(int.MaxValue, int.MaxValue);

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
            player = new Player(new Vector2(60, 10), 50, 70, new Vector2(10, -5));

            blocks = World.LoadFromFile(world, "level1.fl");
            i = 0;

        }

        void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        void Window_UpdateFrame(object sender, FrameEventArgs e)
        {

			Physics.updatePhysics(ref player, blocks, world);

        }

        void Window_RenderFrame(object sender, FrameEventArgs e)
        {

            Matrix4 projMat = Matrix4.CreateOrthographicOffCenter(0, window.Width, window.Height, 0, 0, 1);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projMat);

            Camera.ApplyTransform(ref player, window);

            GL.ClearColor(Color.Aqua);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            foreach (Block block in blocks)
            {
                SpriteBatch.DrawBlock(block.Type, block.Position, block.Size);
            }

            SpriteBatch.DrawPlayer(player);

            blocks.RemoveAll(new Predicate<Block>(blockSearch));

            window.SwapBuffers();

            GC.Collect();

        }

        bool blockSearch(Block block)
        {

            if (block.Position == removePos)
            {
                removePos = new Vector2(int.MaxValue, int.MaxValue);
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
