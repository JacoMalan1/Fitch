using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Fitch
{
    public class ContentPipe
    {

        /// <summary>
        /// Loads an image file into memory.
        /// </summary>
        /// <param name="filePath">"Content/" automatically appended.</param>
        /// <returns></returns>
        public static Texture2D LoadTexture(string filePath)
        {
            filePath = "Content/" + filePath;
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }

            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            Bitmap bmp = new Bitmap(filePath);

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp.Width, bmp.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);
            bmp.UnlockBits(bmpData);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);

            int width = bmp.Width;
            int height = bmp.Height;
            bmp.Dispose();

            return new Texture2D(id, width, height);
        }
    }
}
