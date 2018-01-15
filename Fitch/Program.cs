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
        public static void Main()
        {
            string screenWwidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width.ToString();
            string screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height.ToString();

            if (!File.Exists("config.ini"))
            {
                string[] lines = new string[3]
                {
                    "Resolution=" + screenWwidth + "x" + screenHeight,
                    "Fullscreen=true",
                    "FSAA_Samples=8"
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

            GameWindow window = new GameWindow(Int32.Parse(width), Int32.Parse(height), new OpenTK.Graphics.GraphicsMode(32, 8, 0, Int32.Parse(data.GetKey("FSAA_Samples"))));

            window.Title = "Fitch";

            Game game = new Game(ref window);
            window.Run(1.0f / 60.0f);
        }
    }
}
