using System;
using System.IO;

namespace Fitch
{
    public struct SaveData
    {

        private int levelNum;
        private int lives;

        public int LevelNum { get { return levelNum; } set { levelNum = value; } }
        public int Lives { get { return lives; } set { lives = value; } }

        public SaveData(int levelNum, int lives)
        {

            this.levelNum = levelNum;
            this.lives = lives;

        }

    }

    public class Save
    {

        private string fileName;
        private SaveData data;

        public string FileName { get { return fileName; } set { fileName = value; } }
        public SaveData Data { get { return data; } set { data = value; } }

        public Save(string fileName, SaveData data)
        {

            this.fileName = fileName;
            this.data = data;

        }

        /// <summary>
        /// Writes a save to disk.
        /// </summary>
        /// <param name="save"></param>
        public static void Write(Save save)
        {

            try
            {
                File.WriteAllText(save.fileName, save.Data.LevelNum + "," + save.Data.Lives);
            }
            catch (IOException e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }

        }

        /// <summary>
        /// Loads a file to a save.
        /// </summary>
        /// <param name="fileName">The file for the save to be loaded.</param>
        /// <returns></returns>
        public static Save Load(string fileName)
        {
            
            string line = File.ReadAllText(fileName);
            int pos = line.IndexOf(',');

            int levelNum = Int32.Parse(line.Substring(0, pos));
            string slives = line.Substring(pos + 1, line.Length - pos - 1);
            int lives = Int32.Parse(slives);
            
            return new Save(fileName, new SaveData(levelNum, lives));

        }

    }
}
