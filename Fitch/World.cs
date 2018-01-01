using System;
using OpenTK;
namespace Fitch
{
	public enum BlockType
	{
		Air,
		Solid
	}

    public class World
    {
        private float BlockSize;
        private Vector2 WorldSize;

        public Vector2 worldSize { get { return WorldSize; }}

        public float blockSize { get { return BlockSize; }}

        public World(float blocksize, Vector2 worldsize)
        {

            this.BlockSize = blocksize;
            this.WorldSize = worldsize;

        }
    }
}
