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
    }
}
