using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Fitch
{
    public class SpriteBatch
    {
        public static Texture2D textureSolid = ContentPipe.LoadTexture("solid.png");
        public static Texture2D textureSpike = ContentPipe.LoadTexture("spike.png");
        public static Texture2D textureGoal = ContentPipe.LoadTexture("goal.png");
        public static Texture2D textureOU = ContentPipe.LoadTexture("oneUp.png");

        /// <summary>
        /// Draws a square to the screen.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        public static void DrawSquare(Texture2D texture, Vector2 position, float size)
        {
            GL.Color3(Color.White);
            Vector2[] vertices = new Vector2[4]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };

            int[] indices = new int[6]
            {
                0, 1, 3, 0, 2, 3
            };

            GL.BindTexture(TextureTarget.Texture2D, texture.ID);

            GL.Begin(PrimitiveType.Triangles);
            foreach (int i in indices)
            {

                GL.TexCoord2(vertices[i].X, vertices[i].Y);
                GL.Vertex2(vertices[i].X * size + position.X, vertices[i].Y * size + position.Y);

            }
            GL.End();
        }

        /// <summary>
        /// Draws a player to the screen.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="player"></param>
        public static void DrawPlayer(Texture2D texture, Player player)
        {

            GL.Color3(Color.White);
            GL.BindTexture(TextureTarget.Texture2D, texture.ID);

            Vector2[] vertices = new Vector2[4]
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 0),
                new Vector2(1, 1)
            };

            int[] indices = new int[6]
            {
                0, 1, 3, 0, 2, 3
            };

            GL.Begin(PrimitiveType.Triangles);
            foreach (int i in indices)
            {
                //Check if player should be flipped
                if (player.Facing == Direction.Left)
                    GL.TexCoord2(1 - vertices[i].X, vertices[i].Y);
                else
                    GL.TexCoord2(vertices[i].X, vertices[i].Y);

                GL.Vertex2(vertices[i].X * player.Width + player.Position.X, vertices[i].Y * player.Height  + player.Position.Y);

            }
            GL.End();

        }

        /// <summary>
        /// Draws the goal igloo to the screen.
        /// </summary>
        /// <param name="goal"></param>
        public static void DrawGoal(Block goal)
        {

            Texture2D texture = textureGoal;
            DrawRect(texture, new Rectangle((int)goal.ScreenPos.X, (int)goal.ScreenPos.Y - (int)goal.Size * 3, (int)goal.Size * 4, (int)goal.Size * 4));

        }

        /// <summary>
        /// Draws a powerup to the screen (Same as DrawGoal).
        /// </summary>
        /// <param name="powerup"></param>
        public static void DrawPowerup(Powerup powerup)
        {

            Texture2D texture;
            switch (powerup.Type)
            {
                case PowerupType.UPOne:
                    texture = textureOU;
                    break;
                default:
                    texture = textureOU;
                    break;
            }
            DrawRect(texture, new Rectangle((int)powerup.ScreenPos.X, (int)powerup.ScreenPos.Y, (int)powerup.Size.X, (int)powerup.Size.Y));

        }

        /// <summary>
        /// Draws a block to the screen.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        public static void DrawBlock(BlockType type, Vector2 position, float size)
		{
            GL.Color3(Color.White);
			Vector2[] vertices = new Vector2[4]
			{
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
			};

			int[] indices = new int[6]
			{
				0, 1, 3, 0, 2, 3
			};

            Texture2D texture;
            switch (type)
            {
                
                case BlockType.Solid:
                    texture = textureSolid;
                    break;
                case BlockType.Spike:
                    texture = textureSpike;
                    break;
                default:
                    texture = textureSolid;
                    break;
            }

            GL.BindTexture(TextureTarget.Texture2D, texture.ID);

			GL.Begin(PrimitiveType.Triangles);
			foreach (int i in indices)
			{

				GL.TexCoord2(vertices[i].X, vertices[i].Y);
                GL.Vertex2((vertices[i].X + position.X) * size, (vertices[i].Y + position.Y) * size);

			}
			GL.End();
		}

        /// <summary>
        /// Draws a rectangle to the screen.
        /// </summary>
        /// <param name="rect">The rectangle to be drawn. <see cref="Rectangle"/></param>
        /// <param name="color"></param>
        public static void DrawRect(Rectangle rect, Color color)
        {

            Vector2[] vertices = new Vector2[4]
            {

                new Vector2(rect.X, rect.Y),
                new Vector2(rect.X + rect.Width, rect.Y),
                new Vector2(rect.X, rect.Y + rect.Height),
                new Vector2(rect.X + rect.Width, rect.Y + rect.Height),

            };

            int[] indices = new int[4]
            {
                0, 1, 3, 2
            };

            GL.Begin(PrimitiveType.Quads);
            
            GL.Color4(color);

            foreach (int i in indices)
            {
                
                GL.Vertex2(vertices[i].X, vertices[i].Y);

            }

            GL.Color4(255, 255, 255, 255);

            GL.End();

        }

        /// <summary>
        /// Draws a rectangle with a texture to the screen.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="rect"></param>
        /// <param name="alpha"></param>
        public static void DrawRect(Texture2D texture, RectangleF rect, float alpha = 255)
        {

            Vector2[] coords = new Vector2[4]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };

            Vector2[] vertices = new Vector2[4]
            {

                new Vector2(rect.X, rect.Y),
                new Vector2(rect.X + rect.Width, rect.Y),
                new Vector2(rect.X, rect.Y + rect.Height),
                new Vector2(rect.X + rect.Width, rect.Y + rect.Height),

            };

            int[] indices = new int[6]
            {
                0, 1, 3, 0, 2, 3
            };

            GL.BindTexture(TextureTarget.Texture2D, texture.ID);

            GL.Color4(255, 255, 255, alpha);

            GL.Begin(PrimitiveType.Triangles);
            foreach (int i in indices)
            {

                GL.TexCoord2(coords[i].X, coords[i].Y);
                GL.Vertex2(vertices[i].X, vertices[i].Y);

            }
            GL.End();

        }

        /// <summary>
        /// Draws text to the screen using the font in "Content/font.png".
        /// </summary>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="texture"></param>
        public static void DrawText(string text, Vector2 position, float size, Texture2D texture)
        {

            Texture2D font = texture;

            float offset = 0;
            int j = 0;
            
            GL.BindTexture(TextureTarget.Texture2D, font.ID);

            foreach (char c in text)
            {
                Vector2[] coords;

                if (Int32.Parse(c.ToString()) < 5)
                {

                    int num = Int32.Parse(c.ToString());

                    coords = new Vector2[4]
                    {
                        new Vector2(num * (font.Width / 5), 0),
                        new Vector2((num + 1) * (font.Width / 5), 0),
                        new Vector2(num * (font.Width / 5), font.Height / 2),
                        new Vector2((num + 1) * (font.Width / 5),  font.Height / 2)
                    };

                }
                else
                {
                    int num = Int32.Parse(c.ToString());
                    num = num % 5;

                    coords = new Vector2[4]
                    {
                        new Vector2(num * (font.Width / 5), font.Height / 2),
                        new Vector2((num + 1) * (font.Width / 5), font.Height / 2),
                        new Vector2(num * (font.Width / 5), font.Height),
                        new Vector2((num + 1) * (font.Width / 5),  font.Height)
                    };
                }

                Vector2[] vertices = new Vector2[4]
                {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
                };

                int[] indices = new int[6]
                {
                    0, 1, 3, 0, 2, 3
                };

                GL.Begin(PrimitiveType.Triangles);

                foreach (int i in indices)
                {

                    GL.TexCoord2(coords[i].X / font.Width, coords[i].Y / font.Height);
                    GL.Vertex2((vertices[i].X * size + position.X + offset), (vertices[i].Y  * size + position.Y));

                }

                j++;
                offset = j * size;

                GL.End();
            }

        }
    }
}
