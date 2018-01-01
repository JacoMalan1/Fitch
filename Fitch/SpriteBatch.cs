using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Fitch
{
    public class SpriteBatch
    {
        public static void DrawSquare(Texture2D texture, Vector2 position, float size)
        {

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
                GL.Vertex2(vertices[i].X * size, vertices[i].Y * size);

            }
            GL.End();
        }

        public static void DrawBlock(BlockType type, Vector2 position, float size)
		{

			Vector3[] vertices = new Vector3[4]
			{
                new Vector3(0, 0, 0.5f),
                new Vector3(1, 0, 0.5f),
                new Vector3(0, 1, 0.5f),
                new Vector3(1, 1, 0.5f)
			};

			int[] indices = new int[6]
			{
				0, 1, 3, 0, 2, 3
			};

            Texture2D texture;
            switch (type)
            {
                
                case BlockType.Air:
                    texture = ContentPipe.LoadTexture("air.png");
                    break;

                case BlockType.Solid:
                    texture = ContentPipe.LoadTexture("solid.png");
                    break;

                default:
                    texture = ContentPipe.LoadTexture("missing.png");
                    break;
            }

            GL.BindTexture(TextureTarget.Texture2D, texture.ID);

			GL.Begin(PrimitiveType.Triangles);
			foreach (int i in indices)
			{

				GL.TexCoord2(vertices[i].X, vertices[i].Y);
                GL.Vertex3(vertices[i].X * size, vertices[i].Y * size, vertices[i].Z);

			}
			GL.End();
		}
    }
}
