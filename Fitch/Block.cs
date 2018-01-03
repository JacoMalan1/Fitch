using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;

namespace Fitch
{
    public enum BlockType
    {
        Air,
        Solid
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
                float minX = rect.X;
                float maxX = rect.X + rect.Width;
                float minY = rect.Y;
                float maxY = rect.Y + rect.Height;

                Vector2[] vertices = new Vector2[4]
                {
                    new Vector2(block.ScreenPos.X, block.ScreenPos.Y),
                    new Vector2(block.ScreenPos.X + block.Size, block.ScreenPos.Y),
                    new Vector2(block.ScreenPos.X, block.ScreenPos.Y + block.Size),
                    new Vector2(block.ScreenPos.X + block.Size, block.ScreenPos.Y + block.Size)
                };

                foreach (Vector2 v in vertices)
                {

                    if ((v.X >= minX && v.X <= maxX) && (v.Y >= minY && v.Y <= maxY))
                    {
                        collision = true;
                    }

                }
            }

            return collision;
        }

    }
}
