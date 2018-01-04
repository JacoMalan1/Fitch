using System;
using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Threading;

namespace Fitch
{
    public class Game
    {
        GameWindow window;
        World world;
        Player player;
        public static int i;
        List<Block> blocks;
        Block[,] level;
        Vector2 removePos = new Vector2(int.MaxValue, int.MaxValue);
        Texture2D playerTexture;
        Texture2D font;
        public static double TVELOCITY = 8;
        public static bool running = false;

        public Game(ref GameWindow window)
        {

            window.Load += Window_Load;
            window.Closing += Window_Closing;
            window.UpdateFrame += Window_UpdateFrame;
            window.RenderFrame += Window_RenderFrame;

            this.window = window;

            Input.Initialize(ref window);
            window.KeyUp += Input.Game_KeyUp;
            window.KeyDown += Input.Game_KeyDown;

        }

        void Window_Load(object sender, EventArgs e)
        {

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);

            GL.Enable(EnableCap.Texture2D);

            world = new World(50, new Vector2(50, 50));
            player = new Player(new Vector2(60, 10), 50, 70, new Vector2(0, -5), false);

            blocks = World.LoadFromFile(world.blockSize, "level1.fl");

            level = World.LoadFromFile(world, "level1.fl");

            playerTexture = ContentPipe.LoadTexture("player.png");
            font = ContentPipe.LoadTexture("text.jpg");

        }

        void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        void Window_UpdateFrame(object sender, FrameEventArgs e)
        {
            player.isRunning = false;

            //Input handling
            if (Input.KeyPress(OpenTK.Input.Key.Space) && player.isStanding)
            {
                player.Velocity += new Vector2(0, -10);
                player.isJumping = true;
			}
            if (Input.KeyDown(OpenTK.Input.Key.D) && !(player.Velocity.X >= TVELOCITY))
            {

                player.Velocity += new Vector2(0.4f, 0);
                player.isRunning = true;

            }
            if (Input.KeyDown(OpenTK.Input.Key.A) && !(player.Velocity.X >= TVELOCITY))
            {

                player.Velocity += new Vector2(-0.4f, 0);
                player.isRunning = true;

            }
            Input.Update();

            

			//Calculate physics
			Physics.updatePhysics(ref player, blocks, world, level);

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

            string fps = ((int)window.RenderFrequency).ToString();

            SpriteBatch.DrawPlayer(playerTexture, player);
            SpriteBatch.DrawText(fps, new Vector2(0, 0), 30, font);

            window.SwapBuffers();

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
