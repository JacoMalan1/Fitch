using System;
using OpenTK;

namespace Fitch
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            GameWindow window = new GameWindow(800, 600, new OpenTK.Graphics.GraphicsMode(32, 8, 0, 0));

            Game game = new Game(window);
            window.Run(1.0 / 60.0);
        }
    }
}
