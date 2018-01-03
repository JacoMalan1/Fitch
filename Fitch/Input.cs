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

        public static void Initialize(GameWindow game)
        {

            keysDown = new List<Key>();
            keysDownLast = new List<Key>();

            game.KeyDown += Game_KeyDown;
            game.KeyUp += Game_KeyUp;
            
        }

        private static void Game_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            keysDown.Add(e.Key);
        }

        private static void Game_KeyUp(object sender, KeyboardKeyEventArgs e)
		{
			while (keysDown.Contains(e.Key))
			{
				keysDown.Remove(e.Key);
			}
        }

        public static void Update()
        {

            keysDownLast = new List<Key>(keysDown);

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
