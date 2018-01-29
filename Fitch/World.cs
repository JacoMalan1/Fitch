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

        /// <summary>
        /// Loads a level file.
        /// </summary>
        /// <param name="blockSize"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<Block> LoadFromFile(float blockSize, string filePath)
        {

            filePath = "Content/" + filePath;
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }

            string[] lines = File.ReadAllLines(filePath);

            List<Block> blocks = new List<Block>();
            string type;
            string xCoord;
            string yCoord;
            int pos;
            string temp;

            Game.powerups.Clear();

            foreach (string line in lines)
            {
                pos = line.IndexOf(',');
                type = line.Substring(0, pos);
                temp = line.Substring(pos + 1, line.Length - pos - 1);
                pos = temp.IndexOf(',');
                xCoord = temp.Substring(0, pos);
                yCoord = temp.Substring(pos + 1, temp.Length - pos - 1);

                if (line[0] == '#')
                {
                    continue;
                }
                
                if (type == "solid")
                {
                    int x = Int32.Parse(xCoord);
                    int y = Int32.Parse(yCoord);

                    blocks.Add(new Block(BlockType.Solid, new Vector2(x, y), blockSize));
				}
				else if (type == "spike")
                {
                    int x = Int32.Parse(xCoord);
                    int y = Int32.Parse(yCoord);

                    blocks.Add(new Block(BlockType.Spike, new Vector2(x, y), blockSize));
				}
                else if (type == "goal")
                {

                    int x = Int32.Parse(xCoord);
                    int y = Int32.Parse(yCoord);

                    blocks.Add(new Block(BlockType.Goal, new Vector2(x, y), blockSize));
                    Game.GoalBlock = new Block(BlockType.Goal, new Vector2(x, y), blockSize);

                }
                else if (type == "oneup")
                {
                    int x = Int32.Parse(xCoord);
                    int y = Int32.Parse(yCoord);

                    Game.powerups.Add(new Powerup(PowerupType.UPOne, new Vector2(blockSize, blockSize), new Vector2(x, y)));
                }
            }

            return blocks;
        }

        /// <summary>
        /// Loads a level file.
        /// </summary>
        /// <param name="world"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Block[,] LoadFromFile(World world, string filePath)
        {

            filePath = "Content/" + filePath;
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            string[] lines = File.ReadAllLines(filePath);

            Block[,] blocks = new Block[(int)world.WorldSize.X, (int)world.WorldSize.Y];

            for (int j = 0; j < (int)world.worldSize.X;j++)
            {
				for (int k = 0; k < (int)world.worldSize.Y; k++)
				{

                    blocks[j, k] = new Block(BlockType.Air, new Vector2(j, k), world.blockSize);

				}
            }

            string type;
            string xCoord;
            string yCoord;
            int pos;
            string temp;

            foreach (string line in lines)
            {

                pos = line.IndexOf(',');
                type = line.Substring(0, pos);
                temp = line.Substring(pos + 1, line.Length - pos - 1);
                pos = temp.IndexOf(',');
                xCoord = temp.Substring(0, pos);
                yCoord = temp.Substring(pos + 1, temp.Length - pos - 1);

                if (line[0] == '#')
                    continue;

                if (type == "solid")
                {

                    int x = Int32.Parse(xCoord);
                    int y = Int32.Parse(yCoord);

                    blocks[x, y] = new Block(BlockType.Solid, new Vector2(x, y), world.blockSize);

                }

				else if (type == "spike")
                {
                    int x = Int32.Parse(xCoord);
                    int y = Int32.Parse(yCoord);

                    blocks[x, y] = new Block(BlockType.Spike, new Vector2(x, y), world.blockSize);
				}

                else if (type == "start")
                {

                    int x = Int32.Parse(xCoord);
                    int y = Int32.Parse(yCoord);

                    blocks[x, y] = new Block(BlockType.PlayerStart, new Vector2(x, y), world.blockSize);

                    Game.playerStart = new Block(BlockType.PlayerStart, new Vector2(x, y), world.blockSize);

                }

            }

            return blocks;

        }

    }
}
