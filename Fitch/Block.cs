using System;
using OpenTK;

namespace Fitch
{
    public class Block
    {
        private BlockType type;
        private Vector2 position;
        private float size;

        public BlockType Type { get { return type; }}
        public Vector2 Position { get { return position; }}
        public float Size { get { return size; }}

        public Block(BlockType type, Vector2 position, float size)
        {

            this.type = type;
            this.position = position;
            this.size = size;

        }
    }
}
