using System;
using System.Collections.Generic;
using OpenTK;
using System.Drawing;

namespace Fitch
{
    class Physics
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">Needed to change the players coords.</param>
        /// <param name="blocks">Needed to itterate through all the blocks to check collision.</param>
        /// <param name="world">Needed to check the block size.</param>
        /// <param name="level">Used cuz i'm lazy JK xD.</param>
        public static void updatePhysics(ref Player player, List<Block> blocks, World world, Block[,] level)
        {

            //Get player coords for checking if player is standing.
            int lX = (int)Math.Floor(player.Position.X / world.blockSize);
            int xX = (int)Math.Floor((player.Position.X + player.Width) / world.blockSize);
            int mX = (int)Math.Ceiling((decimal)((lX + xX) / 2));
            int lY = (int)Math.Floor((player.Position.Y + player.Height) / world.blockSize);

            try
            {
                //Check if the player is standing
                if ((level[lX, lY].isSolid) && player.Velocity.Y >= 0)
                {
                    player.isStanding = true;
                    player.isJumping = false;
                   // Console.WriteLine("Standing");
                }
                else if (level[mX, lY].isSolid && player.Velocity.Y >= 0)
                {
                    player.isStanding = true;
                    player.isJumping = false;
                   // Console.WriteLine("Standing");
                }
                else if (level[xX, lY].isSolid && player.Velocity.Y >= 0)
                {
                    player.isStanding = true;
                    player.isJumping = false;
                    //Console.WriteLine("Standing");
                }
                else
                {
                    player.isStanding = false;
                    //Console.WriteLine("Not Standing");
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Game.playerDeath(ref player);
                Console.WriteLine(e.StackTrace);

                Game.logStrings.Add(DateTime.Now.ToLongTimeString() + ": Player went out of range.\n" + e.StackTrace);
            }

            try
            {
                List<Block> checkBlocks = new List<Block>();

                int minX = (int)Math.Floor(player.Position.X / world.blockSize);
                int midX = (int)Math.Floor((player.Position.X + (player.Width / 2)) / world.blockSize);
                int maxX = (int)Math.Floor((player.Position.X + player.Width) / world.blockSize);
                int minY = (int)Math.Floor(player.Position.Y / world.blockSize);
                int maxY = (int)Math.Floor((player.Position.Y + player.Height) / world.blockSize);

                //Only check blocks in the player radius rather than checking every single block.
                if (level[minX, minY].isSolid)
                {
                    checkBlocks.Add(level[minX, minY]);
                }
                if (level[minX, maxY].isSolid)
                {
                    checkBlocks.Add(level[minX, maxY]);
                }
                if (level[maxX, maxY].isSolid)
                {
                    checkBlocks.Add(level[maxX, maxY]);
                }
                if (level[maxX, minY].isSolid)
                {
                    checkBlocks.Add(level[maxX, minY]);
                }
				if (level[midX, minY].isSolid)
				{
					checkBlocks.Add(level[midX, minY]);
				}
                if (level[midX, maxY].isSolid)
				{
                    checkBlocks.Add(level[midX, maxY]);
				}

                foreach (Block block in blocks)
                {
                    if (block.Type == BlockType.Spike)
                        checkBlocks.Add(block);
                }

                foreach (Powerup block in Game.powerups)
                {

					RectangleF bCol = new RectangleF(block.ScreenPos.X, block.ScreenPos.Y, block.Size.X, block.Size.X);
					RectangleF pCol = new RectangleF(player.Position.X, player.Position.Y, player.Width, player.Height);

                    if (pCol.IntersectsWith(bCol))
                        Powerup.raiseCollected(block);
				}

                //Collision
                foreach (Block block in checkBlocks)
                {

                    //Setup bounding boxes
                    RectangleF bCol = new RectangleF(block.ScreenPos.X, block.ScreenPos.Y, block.Size, block.Size);
                    RectangleF pCol = new RectangleF(player.Position.X, player.Position.Y, player.Width, player.Height);

                    int bX = (int)block.Position.X;
                    int bY = (int)block.Position.Y;

                    RectangleF colRect = RectangleF.Intersect(bCol, pCol);

                    //Check wether the player is intersecting with a block.
                    if (pCol.IntersectsWith(bCol))
                    {

                        //BURN BABY, BURN!
                        if (block.Type == BlockType.Spike)
                        {
                            Game.playerDeath(ref player);
                            break;
                        }

                        if (block.Type == BlockType.Goal)
                            continue;

                        double colArea = colRect.Width * colRect.Height;
                        double pArea = player.Width * player.Height;

                        if (((colArea / pArea) * 100) > 60)
                        {
                            Game.playerDeath(ref player);
                            break;
                        }

                        #region Resolve

                        //Resolve Y
                        if ((!level[bX, bY + 1].isSolid || !level[bX, bY - 1].isSolid) && (!(colRect.Height > player.Height / 3)) && block.isSolid)
                        {

                            //If player is moving downwards or standing then we always move up.
                            if (player.Velocity.Y > 0 || player.isStanding)
                            {
                                player.Position -= new Vector2(0, colRect.Height);
                            }
                            else
                            {
                                player.Position += new Vector2(0, colRect.Height);
                                player.Velocity = new Vector2(player.Velocity.X, 0);
                            }

                            //Retain X-velocity if player is standing or jumping.
                            if (player.isStanding || player.isJumping)
                                player.Velocity = new Vector2(player.Velocity.X, 0);
                            else
                                player.Velocity = new Vector2(0, player.Velocity.Y);

                        }

                        //Resolve X
                        else if ((!level[bX + 1, bY].isSolid || !level[bX - 1, bY].isSolid))
                        {

                            //Almost same as for Y resolution.
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

                        #endregion

                    }

                }
            }
            catch (Exception e)
            {

                //Write error to command line.
                Console.WriteLine("Error in update physics.X or Y");
                Console.WriteLine(e.StackTrace);

                Game.logStrings.Add(DateTime.Now.ToLongTimeString() + ": Error in update physics.\n" + e.StackTrace);
            
            }

            //Prevent player from breaking physics :)
            if (player.Position.X < 0)
            {
                player.Position = new Vector2(0, player.Position.Y);
            }
            if (player.Position.Y < 0)
            {
                player.Position = new Vector2(player.Position.X, 0);
            }

            //Velocity
            player.Position += player.Velocity;
            if (!Input.KeyDown(OpenTK.Input.Key.D) && !Input.KeyDown(OpenTK.Input.Key.A))
            {
                player.Velocity = new Vector2(player.Velocity.X / 1.5f, player.Velocity.Y);
            }

            //Gravity
            if (!player.isStanding)
            {
                player.Velocity += new Vector2(0, 0.44f);
            }

            //Setup killplane
            if (player.Position.Y >= world.worldSize.Y * world.blockSize)
            {
                Game.playerDeath(ref player);
            }

        }
    }
}