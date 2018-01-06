using System;
using System.IO;
using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using IniParser;
using IniParser.Model;

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
        Texture2D logoTex;
        public static string fps;
        Texture2D font;
        public static double TVELOCITY = 8;
        public static bool running = false;
        public static bool titlescreen;

        public Game(ref GameWindow window)
        {

            window.Load += Window_Load;
            window.Closing += Window_Closing;
            window.UpdateFrame += Window_UpdateFrame;
            window.RenderFrame += Window_RenderFrame;
            window.Resize += Window_Resize;

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

            logoTex = ContentPipe.LoadTexture("logo.png");

            titlescreen = true;

            if (MainClass.data.GetKey("Fullscreen") == "true")
            {
                window.WindowState = WindowState.Fullscreen;

                window.CursorVisible = false;
            }

            fps = ((int)window.RenderFrequency).ToString();

            i = 0;

        }

        void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            window.Dispose();
        }

        void Window_UpdateFrame(object sender, FrameEventArgs e)
        {
            player.isRunning = false;

            //Input handling
            if (Input.KeyPress(OpenTK.Input.Key.F11))
            {
                if (window.WindowState == WindowState.Normal)
                {
                    window.WindowState = WindowState.Fullscreen;
                }
                else if (window.WindowState == WindowState.Fullscreen)
                {
                    window.WindowState = WindowState.Normal;
                }
                else
                {
                    window.WindowState = WindowState.Fullscreen;
                }
            }

            if (Input.KeyPress(OpenTK.Input.Key.Space) && player.isStanding)
            {
                player.Velocity += new Vector2(0, -15);
                player.isJumping = true;
			}
            if (Input.KeyDown(OpenTK.Input.Key.D) && !(player.Velocity.X >= TVELOCITY))
            {

                player.Velocity += new Vector2(0.4f, 0);
                player.isRunning = true;

            }
            if (Input.KeyDown(OpenTK.Input.Key.A))
            {
                if (!(Math.Abs(player.Velocity.X) >= TVELOCITY) || Input.KeyDown(OpenTK.Input.Key.D))
                {
                    player.Velocity += new Vector2(-0.4f, 0);
                    player.isRunning = true;
                }

            }
            if (Input.KeyDown(OpenTK.Input.Key.Escape))
            {
                window.Close();
            }
            Input.Update();

            //Calculate physics
            Physics.updatePhysics(ref player, blocks, world, level);
        }

        void Window_RenderFrame(object sender, FrameEventArgs e)
        {

            if (titlescreen)
            {
                Matrix4 projMat = Matrix4.CreateOrthographicOffCenter(0, window.Width, window.Height, 0, 0, 1);
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadMatrix(ref projMat);

                GL.ClearColor(Color.Black);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                for (int alpha = 0; alpha <= 255; alpha++)
                {
                    SpriteBatch.DrawRect(logoTex, new RectangleF(0, 0, window.Width, window.Height), alpha);
                    window.SwapBuffers();
                }

                titlescreen = false;
            }
            else
            {


                Matrix4 projMat = Matrix4.CreateOrthographicOffCenter(0, window.Width, window.Height, 0, 0, 1);
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadMatrix(ref projMat);
                GL.ClearColor(Color.Aqua);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                Matrix4 transform = Camera.ApplyTransform(ref player, window);

                foreach (Block block in blocks)
                {
                    SpriteBatch.DrawBlock(block.Type, block.Position, block.Size);
                }

                if (i % 30 == 0)
                    fps = ((int)window.RenderFrequency).ToString();

                SpriteBatch.DrawPlayer(playerTexture, player);
                
                GL.LoadMatrix(ref projMat);
                SpriteBatch.DrawText(fps, new Vector2(0, 0), 30, font);
                i++;
                
            }
            window.SwapBuffers();
        }

        void Window_Resize(object sender, EventArgs e)
        {
			Matrix4 projMat = Matrix4.CreateOrthographicOffCenter(0, window.Width, window.Height, 0, 0, 1);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadMatrix(ref projMat);

            Camera.ApplyTransform(ref player, window);

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
