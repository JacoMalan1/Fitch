using System;
namespace Fitch
{
    public class World
    {
        public enum BlockType
        {
            Air,
            Solid
        }

        private float BlockSize;

        public float blockSize { get { return BlockSize; }}

        public World(float blocksize)
        {

            this.BlockSize = blocksize;

        }
    }
}
