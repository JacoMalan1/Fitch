using System;
using System.IO;
using OpenTK;
using System.Collections.Generic;

namespace Fitch
{

    public enum TriggerType
    {

        text,
        move,
        rotate

    }

    public class Trigger
    {

        private TriggerType type;
        private bool vertSpesific;
        private int xPos;
        private int yPos;
        private object data;
        private int timer;
        private int index;

        public TriggerType Type { get { return type; } }
        public bool VertSpec { get { return vertSpesific; } }
        public Vector2 Position { get { return new Vector2(xPos, yPos); } }
        public object Data { get { return data; } set { data = value; } }
        public int Timer { get { return timer; } set { timer = value; } }
        public int Index { get { return index; } set { index = value; } }

        /// <summary>
        ///Creates a new Trigger object. 
        /// </summary>
        /// <param name="type">The type of trigger.</param>
        /// <param name="data">The data for the trigger, e.g. some text for text trigger.</param>
        /// <param name="xPos">The X-Position of the trigger.</param>
        /// <param name="vertSpesific">Wether the trigger has a Y-Position</param>
        /// <param name="yPos">The Y-Position for the trigger. Only needed when vertSpecific is true.</param>
        public Trigger(TriggerType type, object data, int xPos, bool vertSpesific, int yPos = 0, int timer = 1000, int index = 0)
        {

            this.type = type;
            this.vertSpesific = vertSpesific;
            this.xPos = xPos;
            this.yPos = yPos;
            this.data = data;
            this.timer = timer;
            this.index = index;

        }

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

        /// <summary>
        /// Loads a level file.
        /// </summary>
        /// <param name="blockSize"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<Block> LoadFromFile(float blockSize, string filePath)
        {
            string triggerFile = "Content/" + filePath + "t";
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

            try
            {
                lines = File.ReadAllLines(triggerFile);

                foreach (string line in lines)
                {

                    pos = line.IndexOf(',');
                    type = line.Substring(0, pos);
                    temp = line.Substring(pos + 1, line.Length - pos - 1);
                    pos = temp.IndexOf(',');
                    string data = temp.Substring(0, pos);
                    temp = temp.Substring(pos + 1, temp.Length - pos - 1);
                    pos = temp.IndexOf(',');
                    xCoord = temp.Substring(0, pos);
                    temp = temp.Substring(pos + 1, temp.Length - pos - 1);
                    pos = temp.IndexOf(',');
                    yCoord = temp.Substring(0, pos);
                    int timer = Int32.Parse(temp.Substring(pos + 1, temp.Length - pos - 1));

                    if (type == "text")
                    {

                        Game.TriggerBuffer.Add(new Trigger(TriggerType.text, data, Int32.Parse(xCoord), false));

                    }
                    else if (type == "move")
                    {

                        Game.TriggerBuffer.Add(new Trigger(TriggerType.move, data, Int32.Parse(xCoord), true, Int32.Parse(yCoord), timer));

                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Error loading " + triggerFile + ", assuming no triggers.");

                //Construct a log string.
                Game.logStrings.Add(DateTime.Now.ToLongTimeString() + ": " + ex.Message + "\nError loading " + triggerFile + ", assuming no triggers.");
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
