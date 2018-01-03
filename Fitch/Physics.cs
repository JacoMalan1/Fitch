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

            GC.Collect();

            float minX = player.Position.X;
            float maxX = player.Position.X + player.Width;
            float minY = player.Position.Y;
            float maxY = player.Position.Y + player.Height;

            RectangleF pCol = new RectangleF(minX, minY, player.Width, player.Height);

            col = null;

            foreach (Block block in blocks)
            {

                if (Math.Floor(block.ScreenPos.Y) == Math.Floor(player.Position.Y + player.Height) && (block.ScreenPos.X >= minX && block.ScreenPos.X <= maxX))
                {
                    player.isStanding = true;
                }
                else
                {
                    player.isStanding = false;
                }

                if (block.ScreenPos.X >= minX && block.ScreenPos.X <= maxX && block.ScreenPos.Y >= minY && block.ScreenPos.Y <= maxY)
                {
                    RectangleF bCol = new RectangleF(block.ScreenPos.X, block.ScreenPos.Y, block.Size, block.Size);
                    RectangleF colRect = RectangleF.Intersect(bCol, pCol);
                    Rectangle rect = new Rectangle((int)colRect.X, (int)colRect.Y, (int)colRect.Width, (int)colRect.Height);
                    SpriteBatch.DrawRect(rect, Color.Red);

                    if ((colRect.Width > colRect.Height || player.Velocity.X == 0) && (!Block.isCollision(blocks, new RectangleF(minX, minY + colRect.Height, colRect.Width, colRect.Height)) || !Block.isCollision(blocks, new RectangleF(minX, minY - colRect.Height, colRect.Width, colRect.Height))))
                    {
                        if (player.Velocity.Y > 0 && !Block.isCollision(blocks, new RectangleF(minX, minY - colRect.Height, colRect.Width, colRect.Height)))
                        {
                            player.Position -= new Vector2(0, colRect.Height);
                        }
                        else if (!Block.isCollision(blocks, new RectangleF(minX, minY + colRect.Height, colRect.Width, colRect.Height)))
                        {
                            player.Position += new Vector2(0, colRect.Height);
                        }
                        if (!player.isStanding)
                        {
                            player.Velocity = new Vector2(player.Velocity.X, 0);
                        }
                    }
                    else if (!player.isStanding && !Block.isCollision(blocks, new RectangleF(minX + colRect.Width, minY, colRect.Width, colRect.Height)) || !Block.isCollision(blocks, new RectangleF(minX - colRect.Width, minY, colRect.Width, colRect.Height)))
                    {
                        if (player.Velocity.X > 0 && !Block.isCollision(blocks, new RectangleF(minX - colRect.Width, minY, colRect.Width, colRect.Height)))
                        {
                            player.Position -= new Vector2(colRect.Width, 0);
                        }
                        else if (!Block.isCollision(blocks, new RectangleF(minX + colRect.Width, minY, colRect.Width, colRect.Height)))
                        {
                            player.Position += new Vector2(colRect.Width, 0);
                        }

                        else
                        {
                            player.Position -= new Vector2(block.Position.X, player.Position.Y);
                        }

                        if (!player.isStanding)
                        {
                            player.Velocity = new Vector2(0, player.Velocity.Y);
                        }
                    }
                    break;
                }

            }

            VelPrev = player.Position;
            
            player.Position += player.Velocity;

            if (player.isRunning && Math.Abs(player.Velocity.X) > Game.TVELOCITY)
            {

                if (player.Velocity.X > 0)
                {
                    player.Velocity = new Vector2((float)Game.TVELOCITY, player.Velocity.Y);
                }
                else
                {
                    player.Velocity = new Vector2((float)-Game.TVELOCITY, player.Velocity.Y);
                }
            }

            //Gravity
            if (!player.isStanding)
            {
                player.Velocity += new Vector2(0, 0.44f);
            }   
        }
    }
}
