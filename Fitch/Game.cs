using System;
using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Timers;
using OpenTK.Audio.OpenAL;
using System.IO;

namespace Fitch
{

    public class Game
    {

        //All public and local things are declared here to make programming easier
#region Declare

        public static GameWindow window;

        World world;
        public static Player player;

        Vector2 removePos = new Vector2(int.MaxValue, int.MaxValue);

        Texture2D logoTex;
        Texture2D font;
        Texture2D gameOverTex;
        Texture2D saveText;
        SoundSource levelMusic;

        public static Texture2D playerTexture;
        public static Texture2D playerIdleTex;
        public static Texture2D background;
        public static Texture2D textureLife;

        public static Save save = new Save("fitch", new SaveData(1, 3));

        public const double TVELOCITY = 8;

        public static bool running = false;
        public static bool titlescreen;
        public static bool goal = false;
        public static bool gameOver = false;

        public int levelCounter = 1;
        public static int i;
        public static string fps;
        public static string levelName;
        public static List<string> logStrings = new List<string>();

        List<Block> blocks;
        Block[,] level;
        public static Block playerStart;
        public static Block GoalBlock;

        public static List<Powerup> powerups = new List<Powerup>();
        public static List<Powerup> collectedPowerups = new List<Powerup>();

        public static Timer deathTimer = new Timer();
        public static Timer goalTimer = new Timer();
        public static Timer titleScreenTimer = new Timer();
        public static Timer gameOverTimer = new Timer();

        public static List<RectBufferF> rectBuffer = new List<RectBufferF>();

        public static List<Trigger> TriggerBuffer = new List<Trigger>();

#endregion

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

            #region Load

            Animation.Initialize();
            //Enable all the tings (skrrrrrrrrrra)
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);

            GL.Enable(EnableCap.Texture2D);

            //Initialize sound
            Sound.Init();

            //Load level
            levelCounter = save.Data.LevelNum;
            levelName = "level" + levelCounter.ToString() + ".fl";
            world = new World(50, new Vector2(200, 50));
            blocks = World.LoadFromFile(world.blockSize, levelName);
            level = World.LoadFromFile(world, levelName);
            background = ContentPipe.LoadTexture("background.png");

            //Initialize player
            playerTexture = ContentPipe.LoadTexture("player.png");
            playerIdleTex = playerTexture;
            player = new Player(new Vector2(0, 0), playerTexture.Width, playerTexture.Height, Vector2.Zero);
            player.Position = playerStart.ScreenPos - new Vector2(-player.Width, player.Height);
            player.Lives = save.Data.Lives;

            //Load sound effects
            levelMusic = ContentPipe.LoadSound("levelMusic.wav", true);

            //Load font
            font = ContentPipe.LoadTexture("text.jpg");

            //Load logo
            logoTex = ContentPipe.LoadTexture("logo.png");

            //Load gameOver texture
            gameOverTex = ContentPipe.LoadTexture("gameOver.png");

            //Load misc textures
            textureLife = ContentPipe.LoadTexture("lives.png");
            saveText = ContentPipe.LoadTexture("savingText.png");

            titlescreen = true;

            if (MainClass.data.GetKey("Fullscreen") == "true")
            {
                window.WindowState = WindowState.Fullscreen;
                window.CursorVisible = false;
            }

            if (MainClass.data.GetKey("VSync") == "false")
            {
                window.VSync = VSyncMode.Off;
            }

            fps = ((int)window.RenderFrequency).ToString();

            //Initialize timers
            i = 0;
            deathTimer.Interval = 1000;
            deathTimer.Elapsed += DeathTimer_Elapsed;
            goalTimer.Interval = 2000;
            goalTimer.Elapsed += GoalTimer_Elapsed;
            titleScreenTimer.Interval = 2000;
            titleScreenTimer.Elapsed += TitleScreenTimer_Elapsed;
            gameOverTimer.Interval = 5000;
            gameOverTimer.Elapsed += GameOverTimer_Elapsed;

            goal = false;

            //DEBUG
            //titlescreen = false;

            #endregion

        }

        private void TitleScreenTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            titlescreen = false;
            try
            {
                if (MainClass.data.GetKey("Sound") == "on")
                    AL.SourcePlay(levelMusic.ID);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                logStrings.Add(DateTime.Now.ToLongTimeString() + ": " + ex.Message);
            }
            titleScreenTimer.Stop();
        }

        void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            window.Dispose();
            AL.DeleteSource(levelMusic.ID);

            //Write log to a file.
            string fileName = @"logs\fitch";
            int logNum = 1;

            if (!Directory.Exists("logs"))
                Directory.CreateDirectory("logs");

            while (File.Exists(fileName + logNum.ToString() + ".log"))
            {

                logNum++;

            }

            fileName = fileName + logNum.ToString() + ".log";

            File.WriteAllLines(fileName, logStrings);

        }

        void Window_UpdateFrame(object sender, FrameEventArgs e)
        {
            //Increment tick counter
            i++;

            //Check if player is at the goal
            if (player.Position.X >= GoalBlock.ScreenPos.X && player.Position.X <= GoalBlock.ScreenPos.X + GoalBlock.Size && !goalTimer.Enabled)
            {
                goal = true;
                goalTimer.Start();
            }

            player.isRunning = false;

            #region InputHandling

            //Input handling

            //Save
            if (Input.KeyPress(OpenTK.Input.Key.F5))
            {

                Save.Write(new Save(save.FileName, new SaveData(levelCounter, player.Lives)));

                //Add a rectangle to be rendered to the RectBuffer
                rectBuffer.Add(new RectBufferF(new RectangleF((window.Width / 2) - (saveText.Width / 2), window.Height - saveText.Height, saveText.Width, saveText.Height), saveText, (Int32.Parse(fps) * 2)));
                
            }

            //Load
            if (Input.KeyPress(OpenTK.Input.Key.F6))
            {
                save = Save.Load(save.FileName);
                levelCounter = save.Data.LevelNum;
                player.Lives = save.Data.Lives;

                //Reload level
                levelName = "level" + levelCounter.ToString() + ".fl";
                world = new World(50, new Vector2(200, 50));
                blocks = World.LoadFromFile(world.blockSize, levelName);
                level = World.LoadFromFile(world, levelName);

                player = Player.Reset(player, playerStart);

                List<Powerup> checkP = powerups;

                System.Threading.Thread.Sleep(1000);

                try
                {

                    foreach (Powerup p in checkP)
                    {
                        foreach (Powerup q in collectedPowerups)
                        {
                            if (q.Position == p.Position)
                            {
                                powerups.Remove(p);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }

            }

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

            if (Input.KeyDown(OpenTK.Input.Key.F) && titlescreen)
                TitleScreenTimer_Elapsed(null, null);

            if (Input.KeyDown(OpenTK.Input.Key.F) && gameOver)
            {

                player.isDead = false;
                deathTimer.Stop();
                DeathTimer_Elapsed(null, null);

                gameOver = false;
                gameOverTimer.Stop();
                GameOverTimer_Elapsed(null, null);

            }

            if (Input.KeyDown(OpenTK.Input.Key.Space) && !player.isRunning)
            {
                player.isJumping = true;
            }

            if (Input.KeyPress(OpenTK.Input.Key.Space) && player.isStanding)
            {
                player.Velocity += new Vector2(0, -15);
                player.isJumping = true;
            }
            if (Input.KeyDown(OpenTK.Input.Key.D))
            {

                player.Facing = Direction.Right;

                if (!(player.Velocity.X >= TVELOCITY))
                    player.Velocity += new Vector2(0.3f, 0);

                player.isRunning = true;

            }

            if (Input.KeyDown(OpenTK.Input.Key.A))
            {

                if (!(Math.Abs(player.Velocity.X) >= TVELOCITY) || Input.KeyDown(OpenTK.Input.Key.D))
                {
                    player.Velocity += new Vector2(-0.3f, 0);
                    player.Facing = Direction.Left;
                }

                player.isRunning = true;
            }

            if (Input.KeyDown(OpenTK.Input.Key.Escape))
            {
                window.Close();
            }

            Input.Update();

            #endregion

            //Calculate physics
            if (!player.isDead && !goal)
                Physics.updatePhysics(ref player, blocks, world, level);

            if (player.Lives >= 5)
            {
                player.Lives = 5;
            }

            Animation.Update(ref player);

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

                SpriteBatch.DrawRect(logoTex, new RectangleF(0, 0, window.Width, window.Height));

                titleScreenTimer.Start();
            }
            else if (gameOver)
            {

                Matrix4 projMat = Matrix4.CreateOrthographicOffCenter(0, window.Width, window.Height, 0, 0, 1);
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadMatrix(ref projMat);

                GL.ClearColor(Color.Black);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                SpriteBatch.DrawRect(gameOverTex, new RectangleF(0, 0, window.Width, window.Height));

            }
            else
            {

                Matrix4 projMat = Matrix4.CreateOrthographicOffCenter(0, window.Width, window.Height, 0, 0, 1);
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadMatrix(ref projMat);
                GL.ClearColor(Color.Aqua);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                //Draw background
                SpriteBatch.DrawRect(background, new RectangleF(0, 0, window.Width, window.Height));

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

                try
                {
                    foreach (Powerup p in powerups)
                    {

                        SpriteBatch.DrawPowerup(p);

                    }
                }
                catch (Exception ex)
                {


                }

                //Stuff that is frame-counter dependent
                if (i % 30 == 0)
                    fps = ((int)window.RenderFrequency).ToString();

                SpriteBatch.DrawPlayer(playerTexture, player);
                player.Width = playerTexture.Width;
                player.Height = playerTexture.Height;

                //Non-moving elements
                GL.LoadMatrix(ref projMat);
                SpriteBatch.DrawText(fps, new Vector2(0, 0), 30, font);
                if (player.isDead)
                    SpriteBatch.DrawRect(new Rectangle(0, 0, window.Width, window.Height), Color.FromArgb(128, 255, 0, 0));
                if (goal)
                    SpriteBatch.DrawRect(new Rectangle(0, 0, window.Width, window.Height), Color.FromArgb(128, 0, 255, 0));
                
                for (int k = 0;k < player.Lives;k++)
                {

                    SpriteBatch.DrawRect(textureLife, new Rectangle(window.Width - playerIdleTex.Width * k, 0, playerIdleTex.Width, playerIdleTex.Height));

                }

                //Render all the RectBufferF objects
                for (int k = 0; k < rectBuffer.Count; k++)
                {

                    rectBuffer[k].Frames--;

                    SpriteBatch.DrawRect(rectBuffer[k].Texture, rectBuffer[k].Rectangle);

                    if (rectBuffer[k].Frames <= 0)
                    {
                        rectBuffer.RemoveAt(k);
                        break;
                    }

                }

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

        /// <summary>
        /// [ Deprecated v1.2.0 ]: Use the Block[,] override of load instead. <see cref="World.LoadFromFile(World, string))"/>
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Executed when the player dies.
        /// </summary>
        /// <param name="player">The player that died.</param>
        public static void playerDeath(ref Player player)
        {

            deathTimer.Start();
            Animation.animCounter = 0;
            player.isDead = true;

        }

        void DeathTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            
            player.Lives--;

            if (player.Lives < 1)
            {
                gameOver = true;
                gameOverTimer.Start();
            }

            if (!(player.Lives < 1))
            {
                player.isDead = false;
                player.Position = new Vector2(playerStart.ScreenPos.X + player.Width, playerStart.ScreenPos.Y - player.Height);
                player.isJumping = false;
                player.isRunning = false;
                player.isStanding = false;
                player.Velocity = Vector2.Zero;
                deathTimer.Stop();
            }

        }

        void GoalTimer_Elapsed(object sender, ElapsedEventArgs e)
        {

            blocks.Clear();
            levelCounter++;
            levelName = "level" + levelCounter.ToString() + ".fl";
            blocks = World.LoadFromFile(world.blockSize, levelName);
            level = World.LoadFromFile(world, levelName);
            player = Player.Reset(player, playerStart);
            goal = false;
            goalTimer.Stop();

        }

        private void GameOverTimer_Elapsed(object sender, ElapsedEventArgs e)
        {

            gameOver = false;
            levelCounter = 1;
            player.Lives = 5;

            //Reload level
            levelName = "level" + levelCounter.ToString() + ".fl";
            world = new World(50, new Vector2(200, 50));
            blocks = World.LoadFromFile(world.blockSize, levelName);
            level = World.LoadFromFile(world, levelName);

            gameOverTimer.Stop();

        }
    }
}