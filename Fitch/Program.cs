using System;
using System.IO;
using OpenTK;
using IniParser;
using IniParser.Model;

namespace Fitch
{
    class MainClass
    {
        public static IniData data;
        public static void Main(string[] args)
        {
            if (!File.Exists("config.ini"))
            {
                string[] lines = new string[2]
                {
                    "Resolution=1920x1080",
                    "Fullscreen=true"
                };
                File.WriteAllLines("config.ini", lines);
            }

            FileIniDataParser parser = new FileIniDataParser();
            data = parser.ReadFile("config.ini");

            string resolution = data.GetKey("Resolution");

            int pos = 0;
            
            for (int i = 0; i < resolution.Length; i++)
            {

                if (resolution[i] == 'x')
                {
                    pos = i;
                    break;
                }

            }

            string width = resolution.Substring(0, pos);
            string height = resolution.Substring(pos + 1, resolution.Length - pos - 1);

            GameWindow window = new GameWindow(Int32.Parse(width), Int32.Parse(height), new OpenTK.Graphics.GraphicsMode(32, 8, 0, 16));
            
            Game game = new Game(ref window);
            window.Run(1.0f / 60.0f);
        }
    }
}
