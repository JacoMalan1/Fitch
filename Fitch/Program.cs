using System;
using System.IO;
using OpenTK;
using IniParser;
using IniParser.Model;
using System.Windows.Forms;

namespace Fitch
{
    class MainClass
    {
        
        public static IniData data;

        [STAThread]
        public static void Main()
        {
            string screenWwidth = Screen.PrimaryScreen.Bounds.Width.ToString();
            string screenHeight = Screen.PrimaryScreen.Bounds.Height.ToString();

            if (!File.Exists("config.ini"))
            {
                string[] lines = new string[5]
                {
                    "Resolution=" + screenWwidth + "x" + screenHeight,
                    "Fullscreen=true",
                    "FSAA_Samples=8",
                    "VSync=true",
                    "Sound=on"
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

            if (File.Exists(Game.save.FileName))
                Game.save = Save.Load(Game.save.FileName);
            else
                Save.Write(Game.save);
                

            string width = resolution.Substring(0, pos);
            string height = resolution.Substring(pos + 1, resolution.Length - pos - 1);

            GameWindow window = new GameWindow(Int32.Parse(width), Int32.Parse(height), new OpenTK.Graphics.GraphicsMode(32, 8, 0, Int32.Parse(data.GetKey("FSAA_Samples"))));

            try
            {
                string[] liness = File.ReadAllLines("LICENCE_short.txt");

                //Display licence
                foreach (string l in liness)
                {
                    Console.WriteLine(l);
                }

                System.Threading.Thread.Sleep(2000);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            window.Title = "Fitch";

            Game game = new Game(ref window);
            window.Run(60, 200);
        }
    }
}
