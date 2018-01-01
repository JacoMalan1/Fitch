using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Fitch
{
    public class SpriteBatch
    {
        public static Texture2D textureSolid = ContentPipe.LoadTexture("solid.jpg");
        public static Texture2D textureAir;
        public static Texture2D missing;
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

        public static void DrawPlayer(Player player)
        {

            GL.Color3(Color.White);
            Texture2D texture = ContentPipe.LoadTexture("player.png");
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

                GL.TexCoord2(vertices[i].X, vertices[i].Y);
                GL.Vertex2(vertices[i].X * player.Width + player.Position.X, vertices[i].Y * player.Height  + player.Position.Y);

            }
            GL.End();

        }

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
                
                case BlockType.Air:
                    texture = textureAir;
                    break;

                case BlockType.Solid:
                    texture = textureSolid ;
                    break;

                default:
                    texture = textureAir;
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
    }
}
