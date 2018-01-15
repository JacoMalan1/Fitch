using System;
using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Timers;

namespace Fitch
{
    public class Game
    {
        public static GameWindow window;
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
        public const double TVELOCITY = 8;
        public static bool running = false;
        public static bool titlescreen;
        public static Block playerStart;
        public static Timer deathTimer;
        public static Block GoalBlock;
        public bool goal = false;
        public static Timer goalTimer;
        public int levelCounter = 1;

        public Game(ref GameWindow window)
        {

            //Do I really need to explain this?
            window.Load += Window_Load;
            window.Closing += Window_Closing;
            window.UpdateFrame += Window_UpdateFrame;
            window.RenderFrame += Window_RenderFrame;
            window.Resize += Window_Resize;

            Game.window = window;

            Input.Initialize(ref window);
            window.KeyUp += Input.Game_KeyUp;
            window.KeyDown += Input.Game_KeyDown;

        }

        void Window_Load(object sender, EventArgs e)
        {

            //Enable all the tings (skrrrrrrrrrra)
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);

            GL.Enable(EnableCap.Texture2D);

            //Load level
            string levelName = "level" + levelCounter.ToString() + ".fl";
            world = new World(50, new Vector2(200, 50));
            blocks = World.LoadFromFile(world.blockSize, levelName);
            level = World.LoadFromFile(world, levelName);

            //Initialize player
            player = new Player(new Vector2(0, 0), 50, 70, Vector2.Zero);
            player.Position = playerStart.ScreenPos - new Vector2(-player.Width, player.Height);
            playerTexture = ContentPipe.LoadTexture("player.png");

            //Load font
            font = ContentPipe.LoadTexture("text.jpg");

            //Load logo
            logoTex = ContentPipe.LoadTexture("logo.png");

            titlescreen = true;

            if (MainClass.data.GetKey("Fullscreen") == "true")
            {
                window.WindowState = WindowState.Fullscreen;
                window.CursorVisible = false;
            }

            fps = ((int)window.RenderFrequency).ToString();

            //Initialize timers
            i = 0;
            deathTimer = new Timer();
            deathTimer.Interval = 1000;
            deathTimer.Elapsed += DeathTimer_Elapsed;
            goalTimer = new Timer();
            goalTimer.Interval = 2000;
            goalTimer.Elapsed += GoalTimer_Elapsed;

            goal = false;

            //DEBUG
            //titlescreen = false;

        }

        void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            window.Dispose();
        }

        void Window_UpdateFrame(object sender, FrameEventArgs e)
        {

            //Check if player is at the goal
            if (player.Position.X >= GoalBlock.ScreenPos.X && player.Position.X <= GoalBlock.ScreenPos.X + GoalBlock.Size && !goalTimer.Enabled)
            {
                goal = true;
                goalTimer.Start();
            }

            player.isRunning = false;

            //Input handling
            if (Input.KeyPress(OpenTK.Input.Key.F11))
            {
                
                if (window.WindowState == WindowState.Normal)
                {
                    window.WindowState = WindowState.Fullscreen;
                    window.CursorVisible = !window.CursorVisible;
                }

                else if (window.WindowState == WindowState.Fullscreen)
                {
                    window.WindowState = WindowState.Normal;
                    window.CursorVisible = !window.CursorVisible;
                }

                else
                {
                    window.WindowState = WindowState.Fullscreen;
                    window.CursorVisible = false;
                }

            }

            if (Input.KeyPress(OpenTK.Input.Key.Space) && player.isStanding)
            {
                player.Velocity += new Vector2(0, -15);
                player.isJumping = true;
			}
            if (Input.KeyDown(OpenTK.Input.Key.D) && !(player.Velocity.X >= TVELOCITY))
            {

                player.Velocity += new Vector2(0.3f, 0);
                player.isRunning = true;

            }

            if (Input.KeyDown(OpenTK.Input.Key.A))
            {
                if (!(Math.Abs(player.Velocity.X) >= TVELOCITY) || Input.KeyDown(OpenTK.Input.Key.D))
                {
                    player.Velocity += new Vector2(-0.3f, 0);
                    player.isRunning = true;
                }
            }

            if (Input.KeyDown(OpenTK.Input.Key.Escape))
            {
                window.Close();
            }

            Input.Update();

            //Calculate physics
            if (!player.isDead && !goal)
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

                    if (block.Type == BlockType.PlayerStart)
                    {
                        player.Position = new Vector2(block.ScreenPos.X + player.Width, block.ScreenPos.Y - player.Height);
                        continue;
                    }
                    if (block.Type == BlockType.Goal)
                    {
                        SpriteBatch.DrawGoal(block);
                        continue;
                    }

                    SpriteBatch.DrawBlock(block.Type, block.Position, block.Size);
                }

                //Stuff that is frame-counter dependent
                if (i % 30 == 0)
                    fps = ((int)window.RenderFrequency).ToString();

                SpriteBatch.DrawPlayer(playerTexture, player);

                //Non-moving elements
                GL.LoadMatrix(ref projMat);
                SpriteBatch.DrawText(fps, new Vector2(0, 0), 30, font);
                if (player.isDead)
                    SpriteBatch.DrawRect(new Rectangle(0, 0, window.Width, window.Height), Color.FromArgb(128, 255, 0, 0));
                if (goal)
                    SpriteBatch.DrawRect(new Rectangle(0, 0, window.Width, window.Height), Color.FromArgb(128, 0, 255, 0));
                i++;
                
            }
            window.SwapBuffers();

        }

        void Window_Resize(object sender, EventArgs e)
        {

            GL.Viewport(window.ClientRectangle);
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

        public static void playerDeath(ref Player player)
        {

            deathTimer.Start();
            player.isDead = true;

        }

        void DeathTimer_Elapsed(object sender, ElapsedEventArgs e)
        {

			player.isDead = false;
			player.Position = new Vector2(playerStart.ScreenPos.X + player.Width, playerStart.ScreenPos.Y - player.Height);
			player.isJumping = false;
			player.isRunning = false;
			player.isStanding = false;
			player.Velocity = Vector2.Zero;
            deathTimer.Stop();

        }

        void GoalTimer_Elapsed(object sender, ElapsedEventArgs e)
        {

            levelCounter++;
            string levelName = "level" + levelCounter.ToString() + ".fl";
            blocks = World.LoadFromFile(world.blockSize, levelName);
            level = World.LoadFromFile(world, levelName);
            player = Player.Reset(player, playerStart);
            goal = false;
            goalTimer.Stop();

        }
    }
}
