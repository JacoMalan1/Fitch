using System;
using System.IO;
using OpenTK;
using System.Collections.Generic;

namespace Fitch
{

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

        public static List<Block> LoadFromFile(float blockSize, string filePath)
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
                if (line[0] == '#')
                {
                    continue;
                }
                
                if (line.Substring(0, 5) == "solid")
                {

                    int x = Int32.Parse(line.Substring(6, 3));
                    int y = Int32.Parse(line.Substring(10, 3));

                    blocks.Add(new Block(BlockType.Solid, new Vector2(x, y), blockSize));
				}
				else if (line.Substring(0, 5) == "spike")
				{
					int x = Int32.Parse(line.Substring(6, 3));
					int y = Int32.Parse(line.Substring(10, 3));

                    blocks.Add(new Block(BlockType.Spike, new Vector2(x, y), blockSize));
				}
                else if (line.Substring(0, 4) == "goal")
                {
					int x = Int32.Parse(line.Substring(5, 3));
					int y = Int32.Parse(line.Substring(9, 3));

                    blocks.Add(new Block(BlockType.Goal, new Vector2(x, y), blockSize));
                    Game.GoalBlock = new Block(BlockType.Goal, new Vector2(x, y), blockSize);
                }
            }

            return blocks;
        }

        public static Block[,] LoadFromFile(World world, string filePath)
        {

            filePath = "Content/" + filePath;
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            string[] lines = File.ReadAllLines(filePath);

            Block[,] blocks = new Block[(int)world.WorldSize.X, (int)world.WorldSize.Y];

            foreach (string line in lines)
            {

                if (line[0] == '#')
                    continue;

                if (line.Substring(0, 5) == "solid")
                {

                    int x = Int32.Parse(line.Substring(6, 3));
                    int y = Int32.Parse(line.Substring(10, 3));

                    blocks[x, y] = new Block(BlockType.Solid, new Vector2(x, y), world.blockSize);

                }

				else if (line.Substring(0, 5) == "spike")
				{
					int x = Int32.Parse(line.Substring(6, 3));
					int y = Int32.Parse(line.Substring(10, 3));

					blocks[x, y] = new Block(BlockType.Spike, new Vector2(x, y), world.blockSize);
				}

                else if (line.Substring(0, 5) == "start")
                {

					int x = Int32.Parse(line.Substring(6, 3));
					int y = Int32.Parse(line.Substring(10, 3));

                    blocks[x, y] = new Block(BlockType.PlayerStart, new Vector2(x, y), world.blockSize);

                    Game.playerStart = new Block(BlockType.PlayerStart, new Vector2(x, y), world.blockSize);

                }

            }

            return blocks;

        }

    }
}
