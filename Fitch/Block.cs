using System.Collections.Generic;
using System.Drawing;
using OpenTK;

namespace Fitch
{
    public enum BlockType
    {
        Solid,
        Spike,
        PlayerStart,
        Goal,
        Air
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
        public bool isSolid { get { if (type == BlockType.Solid) { return true; } else { return false; }}}

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
