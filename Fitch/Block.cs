using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;

namespace Fitch
{
    public enum BlockType
    {
        Air,
        Solid,
        Spike,
        PlayerStart
    }
    public class Collision
    {

        private Block block;
        private Player player;
        private static Vector2 searchPos;

        public Block Block { get { return block; } }
        public Player Player { get { return player; } }

        public Collision(Block block, ref Player player)
        {

            this.block = block;
            this.player = player;

        }

        private static bool blockSearch(Block block)
        {

            if (block.Position == searchPos)
            {
                searchPos = new Vector2(int.MaxValue, int.MaxValue);
                return true;
            }
            else
            {
                return false;
            }

        }

        public static void CorrectPlayer(List<Block> blocks, Block block, ref Player player)
        {



        }

    }



    public class Block
    {
        private BlockType type;
        private Vector2 position;
        private Vector2 screenPos;
        private float size;

        public BlockType Type { get { return type; } }
        public Vector2 Position { get { return position; } }
        public Vector2 ScreenPos { get { return screenPos; } }
        public float Size { get { return size; } }

        public Block(BlockType type, Vector2 position, float size)
        {

            this.type = type;
            this.position = position;
            this.screenPos = position * size;
            this.size = size;

        }

        public static bool isCollision(List<Block> blocks, RectangleF rect)
        {

            bool collision = false;

            foreach (Block block in blocks)
            {
                RectangleF bCol = new RectangleF(block.ScreenPos.X, block.ScreenPos.Y, block.ScreenPos.X + block.Size, block.ScreenPos.Y + block.Size);

                if (bCol.IntersectsWith(rect)) 
                {
                    collision = true;
                    break;
                }

            }

            return collision;
                
        }

    }
}
