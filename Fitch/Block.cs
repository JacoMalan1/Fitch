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

    public class BlockMove
    {

        private Direction direction;
        private Vector2 to;
        private Vector2 triggerPos;
        private int steps;
        private bool triggered = false;
        private Vector2 screenPos;
        private Vector2 initPos;

        public Direction MoveDirection { get { return direction; } }
        public Vector2 ToPos { get { return to; } }
        public Vector2 TriggerPos { get { return triggerPos; } }
        public int MoveSteps { get { return steps; } set { steps = value; } }
        public bool Triggered { get { return triggered; } set { triggered = value; } }
        public Vector2 ScreenPos { get { return screenPos; } }
        public Vector2 InitPos { get { return initPos; } }

        public BlockMove(Direction direction, Vector2 from, Vector2 triggerPos, Vector2 initPos, int steps = 10, int blockSize = 50)
        {

            this.direction = direction;
            this.to = from;
            this.triggerPos = triggerPos;
            this.steps = steps;
            this.screenPos = from * blockSize;
            this.initPos = initPos;

        }

    }

    public class Block
    {
        private BlockType type;
        private Vector2 position;
        private Vector2 screenPos;
        private float size;
        private bool moveable;
        private BlockMove moveEvent;

        public BlockType Type { get { return type; } }
        public Vector2 Position { get { return position; } set { position = value; } }
        public Vector2 ScreenPos { get { return screenPos; } set { screenPos = value; } }
        public float Size { get { return size; } }
        public bool isSolid { get { if (type == BlockType.Solid) { return true; } else { return false; }}}
        public bool Moveable { get { return moveable; } set { moveable = value; } }
        public BlockMove MoveEvent { get { return moveEvent; } set { MoveEvent = value; } }

        public Block(BlockType type, Vector2 position, float size, bool moveable = false, BlockMove moveEvent = null)
        {

            this.type = type;
            this.position = position;
            this.screenPos = position * size;
            this.size = size;
            this.moveable = moveable;
            this.moveEvent = moveEvent;

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

        /// <summary>
        /// Updates the block triggers.
        /// </summary>
        /// <param name="blocks">List of blocks to update.</param>
        public static void Update(ref List<Block> blocks, ref Player player)
        {

            foreach (Block block in blocks)
            {

                if (!block.Moveable)
                    continue;

                RectangleF pRect = new RectangleF((int)player.Position.X, (int)player.Position.Y, (int)player.Width, (int)player.Height);
                RectangleF blockRect = new RectangleF(block.MoveEvent.ScreenPos.X, block.MoveEvent.ScreenPos.Y, block.Size, block.Size);

                if (!block.MoveEvent.Triggered && pRect.IntersectsWith(blockRect))
                {

                    block.MoveEvent.Triggered = true;

                }
                else if (block.MoveEvent.Triggered && !(block.MoveEvent.MoveSteps <= 0))
                {

                    block.Position = block.MoveEvent.ToPos - ((block.MoveEvent.ToPos - block.Position) * block.MoveEvent.MoveSteps);
                    block.MoveEvent.MoveSteps--;

                    Game.level[(int)block.Position.X, (int)block.Position.Y] = block;
                    Game.level[(int)block.MoveEvent.InitPos.X, (int)block.MoveEvent.InitPos.Y] = null;

                    Game.removePos = block.MoveEvent.InitPos;
                    blocks.RemoveAll(Game.blockSearch);
                    blocks.Add(block);

                }

            }

        }

    }
}
