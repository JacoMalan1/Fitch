using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;

namespace Fitch
{
    public class Input
    {

        private static List<Key> keysDown;
        private static List<Key> keysDownLast;

        public static void Initialize(ref GameWindow game)
        {

            keysDown = new List<Key>();
            keysDownLast = new List<Key>();

            game.KeyDown += Game_KeyDown;
            game.KeyUp += Game_KeyUp;
            
        }

        public static void Game_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            keysDown.Add(e.Key);
        }

        public static void Game_KeyUp(object sender, KeyboardKeyEventArgs e)
		{
			while (keysDown.Contains(e.Key))
			{
				keysDown.Remove(e.Key);
			}
        }

        public static void Update()
        {

            keysDownLast = new List<Key>(keysDown);

            if (keysDownLast.Contains(Key.Space))
            {
                Console.WriteLine("input");
            }

        }

        public static bool KeyPress(Key key)
        {
            return (keysDown.Contains(key) && !keysDownLast.Contains(key));
        }

        public static bool KeyDown(Key key)
        {
            return (keysDown.Contains(key));
        }

        public static bool KeyRelease(Key key)
        {
            return (!keysDown.Contains(key) && keysDownLast.Contains(key));
        }

    }
}
