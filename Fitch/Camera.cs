using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Fitch
{
    class Camera
    {

        public static Matrix4 ApplyTransform(ref Player player, GameWindow window)
        {

            Matrix4 transform = Matrix4.Identity;
            transform = Matrix4.Mult(transform, Matrix4.CreateTranslation((-player.Position.X + window.Width / 2) - player.Width, (-player.Position.Y + window.Height / 2) - player.Height, 0));

            GL.MultMatrix(ref transform);

            return transform;

        }

    }
}
