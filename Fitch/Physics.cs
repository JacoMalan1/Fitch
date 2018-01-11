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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">Needed to change the players coords.</param>
        /// <param name="blocks">Needed to itterate through all the blocks to check collision.</param>
        /// <param name="world">Needed to check the block size.</param>
        /// <param name="level">Used cuz i'm lazy JK xD.</param>
        public static void updatePhysics(ref Player player, List<Block> blocks, World world, Block[,] level)
        {

            int lX = (int)Math.Floor(player.Position.X / world.blockSize);
            int xX = (int)Math.Floor((player.Position.X + player.Width) / world.blockSize);
            int lY = (int)Math.Floor((player.Position.Y + player.Height) / world.blockSize);

            try
            {

                //Check if the player is standing
                if (!(level[lX, lY] == null) || !(level[xX, lY] == null) && player.Velocity.Y >= 0)
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
                Console.WriteLine(e.StackTrace);
            }

            try
            {
                //Collision
                foreach (Block block in blocks)
                {

                    RectangleF bCol = new RectangleF(block.ScreenPos.X, block.ScreenPos.Y, block.Size, block.Size);
                    RectangleF pCol = new RectangleF(player.Position.X, player.Position.Y, player.Width, player.Height);

                    int bX = (int)block.Position.X;
                    int bY = (int)block.Position.Y;

                    RectangleF colRect = RectangleF.Intersect(bCol, pCol);

                    if (pCol.IntersectsWith(bCol))
                    {
                        if (block.Type == BlockType.Spike)
                        {
                            Game.playerDeath(ref player, Game.playerStart);
                            break;
                        }

                        //Resolve Y
                        if (level[bX, bY + 1] == null || level[bX, bY - 1] == null)
                        {

                            if (player.Velocity.Y > 0 || player.isStanding)
                            {
                                player.Position -= new Vector2(0, colRect.Height);
                            }
                            else
                            {
                                player.Position += new Vector2(0, colRect.Height);
                                player.Velocity = new Vector2(player.Velocity.X, 0);
                            }

                            if (player.isStanding || player.isJumping)
                                player.Velocity = new Vector2(player.Velocity.X, 0);
                            else
                                player.Velocity = new Vector2(0, player.Velocity.Y);

                        }
                        //Resolve X
                        else if (level[bX + 1, bY] == null || level[bX - 1, bY] == null)
                        {

                            if (player.Velocity.X > 0)
                            {
                                player.Position -= new Vector2(colRect.Width, 0);
                            }
                            else
                            {
                                player.Position += new Vector2(colRect.Width, 0);
                            }

                            player.Velocity = new Vector2(0, player.Velocity.Y);

                        }

                        break;

                    }

                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("Error in update physics.X or Y");
                Console.WriteLine(e.StackTrace);
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
                player.Velocity = new Vector2(player.Velocity.X / 1.2f, player.Velocity.Y);
            }

            //Gravity
            if (!player.isStanding)
            {
                player.Velocity += new Vector2(0, 0.44f);
            }

            if (player.Position.Y >= world.worldSize.Y * world.blockSize)
            {
                Game.playerDeath(ref player, Game.playerStart);
            }

        }
    }
}
