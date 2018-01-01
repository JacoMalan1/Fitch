using System;
using System.IO;
using OpenTK;
using System.Collections.Generic;

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

        public static List<Block> LoadFromFile(World world, string filePath)
        {

            filePath = "Content/" + filePath;
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }

            string[] lines = File.ReadAllLines(filePath);

            List<Block> blocks = new List<Block>();

            foreach (string line in lines)
            {
                if (line.Substring(0, 5) == "solid")
                {

                    int x = Int32.Parse(line.Substring(6, 3));
                    int y = Int32.Parse(line.Substring(10, 3));

                    blocks.Add(new Block(BlockType.Solid, new Vector2(x, y), world.blockSize));
                }
            }

            return blocks;
        }
    }
}
