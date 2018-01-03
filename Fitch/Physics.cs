using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;
using FarseerPhysics;

namespace Fitch
{
    class Physics
    {
        public static bool collide = false;
        public static Collision col = null;
        public static Vector2 VelPrev = Vector2.Zero;

        public static void updatePhysics(ref Player player, List<Block> blocks, World world)
        {

            float minX = player.Position.X;
            float maxX = player.Position.X + player.Width;
            float minY = player.Position.Y;
            float maxY = player.Position.Y + player.Height;

            RectangleF pCol = new RectangleF(minX, minY, player.Width, player.Height);

            col = null;

            foreach (Block block in blocks)
            {

                if (block.ScreenPos.X >= minX && block.ScreenPos.X <= maxX && block.ScreenPos.Y >= minY && block.ScreenPos.Y <= maxY)
                {
                    RectangleF bCol = new RectangleF(block.ScreenPos.X, block.ScreenPos.Y, block.Size, block.Size);
                    RectangleF colRect = RectangleF.Intersect(bCol, pCol);
                    Rectangle rect = new Rectangle((int)colRect.X, (int)colRect.Y, (int)colRect.Width, (int)colRect.Height);
                    SpriteBatch.DrawRect(rect, Color.Red);
                    player.Position = VelPrev;
                    player.Velocity = Vector2.Zero;
                }

            }

            VelPrev = player.Position;

            player.Position += player.Velocity;

            if (!(Math.Abs(player.Velocity.X) <= 0))
            {

                player.Velocity = new Vector2(player.Velocity.X / 1.0000001f, player.Velocity.Y);

            }

            //Gravity
            player.Velocity += new Vector2(0, 0.44f);
        }
    }
}
