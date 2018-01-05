using System;
using System.Collections.Generic;
using OpenTK;
using System.Drawing;

namespace Fitch
{
    class Physics
    {
        public static bool collide = false;
        public static Collision col = null;

        public static void updatePhysics(ref Player player, List<Block> blocks, World world, Block[,] level)
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

                int lY = (int)Math.Floor((player.Position.Y + player.Height) / world.blockSize);
                int lX = (int)Math.Floor(player.Position.X / world.blockSize);

                lY *= 2;
                lX *= 2;

                try
                {
                    if (!(level[lX, lY] == null))
                    {
                        player.isStanding = true;
                        player.isJumping = false;
                    }
                    else
                    {
                        player.isStanding = false;
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }

                RectangleF bCol = new RectangleF(block.ScreenPos.X, block.ScreenPos.Y, block.Size, block.Size);
                if (pCol.IntersectsWith(bCol))
                {
                    RectangleF colRect = RectangleF.Intersect(bCol, pCol);
                    Rectangle rect = new Rectangle((int)colRect.X, (int)colRect.Y, (int)colRect.Width, (int)colRect.Height);
                    SpriteBatch.DrawRect(rect, Color.Red);

                    if (colRect.Width >= colRect.Height && (!Block.isCollision(blocks, new RectangleF(minX, minY + colRect.Height, colRect.Width, colRect.Height)) || !Block.isCollision(blocks, new RectangleF(minX, minY - colRect.Height, colRect.Width, colRect.Height))))
                    {
                        if (player.Velocity.Y > 0 && !Block.isCollision(blocks, new RectangleF(minX, minY - colRect.Height, colRect.Width, colRect.Height)))
                        {
                            player.Position -= new Vector2(0, colRect.Height);
                        }
                        else if (!Block.isCollision(blocks, new RectangleF(minX, minY + colRect.Height, colRect.Width, colRect.Height)))
                        {
                            player.Position += new Vector2(0, colRect.Height);
                        }
 
                        player.Velocity = new Vector2(player.Velocity.X, 0);
                    }
                    else if (!Block.isCollision(blocks, new RectangleF(minX + colRect.Width, minY, colRect.Width, colRect.Height)) || !Block.isCollision(blocks, new RectangleF(minX - colRect.Width, minY, colRect.Width, colRect.Height)))
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

                        player.Velocity = new Vector2(0, player.Velocity.Y);
                    }

					RectangleF newCol = new RectangleF(player.Position.X, player.Position.Y, player.Position.X + player.Width, player.Position.Y + player.Height);

					if (Block.isCollision(blocks, newCol))
					{
						player.Position = new Vector2(block.Position.X + block.Size, player.Position.Y);
					}

                    break;
                }

            }

            if (player.Position.X < 0)
            {
                player.Position = new Vector2(0, player.Position.Y);
            }
            if (player.Position.Y < 0)
            {
                player.Position = new Vector2(player.Position.X, 0);
            }

            player.Position += player.Velocity;

            if (!Input.KeyDown(OpenTK.Input.Key.D) && !Input.KeyDown(OpenTK.Input.Key.A))
            {
                player.Velocity = new Vector2(player.Velocity.X / 1.001f, player.Velocity.Y);
            }

            //Gravity
            if (!player.isStanding)
            {
                player.Velocity += new Vector2(0, 0.44f);
            }
        }

        public static void updatePhysics(ref Player player, Block[,] level, World world)
        {

            GC.Collect();

            int minX = (int)Math.Floor((player.Position.X - player.Size.X / 2f) / world.blockSize);
            int minY = (int)Math.Floor((player.Position.Y - player.Size.Y / 2f) / world.blockSize);
            int maxX = (int)Math.Ceiling((player.Position.X + player.Size.X / 2f) / world.blockSize);
            int maxY = (int)Math.Ceiling((player.Position.Y + player.Size.Y / 2f) / world.blockSize);

            RectangleF colRec = new RectangleF(minX, minY, player.Width, player.Height);

            col = null;

            List<Block> checkBlocks = new List<Block>();

            try
            {
                for (int x = minX; x <= maxX; x++)
                {
                    for (int y = minY; y <= maxY; y++)
                    {

                        RectangleF blockRec = new RectangleF(x * world.blockSize, y * world.blockSize, world.blockSize, world.blockSize);
                        if (level[x, y].Type == BlockType.Solid && colRec.IntersectsWith(blockRec))
                        {
                            float[] depths = new float[4]
                            {
                            blockRec.Right - colRec.Left, //PosX
							blockRec.Bottom - colRec.Top, //PosY
							colRec.Right - blockRec.Left, //NegX
							colRec.Bottom - blockRec.Top  //NegY
                            };

                            if (level[x + 1, y].Type == BlockType.Solid)
                                depths[0] = -1;
                            if (level[x, y + 1].Type == BlockType.Solid)
                                depths[1] = -1;
                            if (level[x - 1, y].Type == BlockType.Solid)
                                depths[2] = -1;
                            if (level[x, y - 1].Type == BlockType.Solid)
                                depths[3] = -1;

                            float min = float.MaxValue;
                            Vector2 minDirection = Vector2.Zero;
                            for (int i = 0; i < 4; i++)
                            {
                                if (depths[i] >= 0 && depths[i] < min)
                                {
                                    min = depths[i];
                                    switch (i)
                                    {
                                        case 0:
                                            minDirection = new Vector2(1, 0);
                                            break;
                                        case 1:
                                            minDirection = new Vector2(0, 1);
                                            break;
                                        case 2:
                                            minDirection = new Vector2(-1, 0);
                                            break;
                                        default:
                                            minDirection = new Vector2(0, -1);
                                            break;
                                    }
                                }
                            }

                            if (minDirection == Vector2.Zero)
                            {
                                //Console.WriteLine("player shouldn't really happen...");
                            }
                            else
                            {
                                player.Position += minDirection * min;
                                if (player.Velocity.X * minDirection.X < 0)
                                    player.Velocity = new Vector2(0, player.Velocity.Y);
                                if (player.Velocity.Y * minDirection.Y < 0)
                                    player.Velocity = new Vector2(player.Velocity.X, 0);

                                if (minDirection == new Vector2(0, -1))
                                    player.isStanding = true;
                            }

                        }
                    }
                }

                    //collision

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
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Oops!!");
            }

        }
    }
}
