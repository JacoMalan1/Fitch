using System;
using OpenTK;
using System.Drawing;

namespace Fitch
{
    public enum PowerupType
    {
        UPOne
    }

    public delegate void powerupCollected(Powerup powerup);

    public class Powerup
    {
        private PowerupType type;
        private Vector2 size;
        private Vector2 position;
        private Vector2 screenPos;
        public event powerupCollected collected;

        public PowerupType Type { get { return type; } set { type = value; }}
        public Vector2 Size { get { return size; } set { size = value; }}
        public Vector2 Position { get { return position; } set { position = value; }}
        public Vector2 ScreenPos { get { return screenPos; }}
        public event powerupCollected Collected { add { collected += value; } remove { collected -= value; }}

        public Powerup(PowerupType type, Vector2 size, Vector2 position)
        {

            this.type = type;
            this.size = size;
            this.position = position;
            this.screenPos = position * size;
            this.collected += OneUpCollected;

        }

        public static void OneUpCollected(Powerup powerup)
        {

            Game.player.Lives++;
            Game.powerups.Remove(powerup);

        }

        public static void raiseCollected(Powerup powerup)
        {
            powerup.collected(powerup);
        }

    }
}
