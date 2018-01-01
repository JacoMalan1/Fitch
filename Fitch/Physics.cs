using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;

namespace Fitch
{
    class Physics
    {
        public static bool collide = false;
        public static int i = 0;

        public static void updatePhysics(ref Player player, List<Block> blocks, World world)
        {
            
            RectangleF Rect = new RectangleF(player.Position.X, player.Position.Y, player.Width, player.Height);
            int minX = (int)Math.Floor(player.Position.X);
            int maxX = (int)Math.Floor(player.Position.X + player.Width);
            int minY = (int)Math.Floor(player.Position.Y);
            int maxY = (int)Math.Floor(player.Position.Y + player.Height);

            List<Block> checkBlocks = new List<Block>();
            foreach (Block block in blocks)
            {

                if ((block.Position.X * block.Size >= minX && block.Position.X * block.Size <= maxX) && (block.Position.Y * block.Size >= minY && block.Position.Y * block.Size <= maxY))
                {
                    checkBlocks.Add(block);
                }

            }

            Vector2[] directions = new Vector2[3]
            {
                new Vector2(0, -1),
                new Vector2(0, 1),
                new Vector2(-1, 0)

            };

            foreach (Block block in checkBlocks)
            {
                float min = float.MaxValue;

                RectangleF intersect = RectangleF.Intersect(Rect, new RectangleF(block.Position.X * block.Size, block.Position.Y * block.Size, block.Size, block.Size));
                Vector2 minDirection = Vector2.Zero;

                for (int i = 0; i < 3; i++)
                {
                    bool occupied = false;
                    foreach (Block oBlock in blocks)
                    {
                        if (oBlock.Position == block.Position + directions[i])
                        {
                            occupied = true;
                        }
                    }

                    if (!occupied && (intersect.X + intersect.Width) * directions[i].X - block.Position.X * block.Size <= intersect.Width && (intersect.Y + intersect.Height) * directions[i].Y - block.Position.Y * block.Size <= intersect.Height)
                    {
                        min = intersect.Width + intersect.Height;
                        minDirection = new Vector2(directions[i].X, directions[i].Y);
                    }

                    if (min == float.MaxValue)
                    {
                        continue;
                    }

                    player.Position += minDirection * min / world.blockSize;
                    player.Velocity = Vector2.Zero;
                    collide = true;
                    player.Position = new Vector2((float)Math.Round(player.Position.X / world.blockSize) * world.blockSize, (float)Math.Round(player.Position.Y / world.blockSize) * world.blockSize);
                    break;
                }

            }

            player.Position += player.Velocity;

            if (!(player.Velocity.X == 0) && !(player.Velocity.Y == 0))
            {
                if (player.Velocity.X <= 0.0000000000000001)
                {
                    player.Velocity = new Vector2(0, player.Velocity.Y);
                }

                player.Velocity = new Vector2(player.Velocity.X / 1.02f, player.Velocity.Y);

            }

            //Gravity
            player.Velocity += new Vector2(0, 0.44f);
        }
    }
}
