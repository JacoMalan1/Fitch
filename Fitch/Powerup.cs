using OpenTK;

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
        private bool isCollected;

        public PowerupType Type { get { return type; } set { type = value; }}
        public Vector2 Size { get { return size; } set { size = value; }}
        public Vector2 Position { get { return position; } set { position = value; }}
        public Vector2 ScreenPos { get { return screenPos; }}
        public event powerupCollected Collected { add { collected += value; } remove { collected -= value; }}
        public bool IsCollected { get { return isCollected; } set { isCollected = value; } }

        public Powerup(PowerupType type, Vector2 size, Vector2 position, bool isCollected = false)
        {

            this.type = type;
            this.size = size;
            this.position = position;
            this.screenPos = position * size;
            this.collected += OneUpCollected;
            this.isCollected = isCollected;

        }

        public static void OneUpCollected(Powerup powerup)
        {

            Game.player.Lives++;
            Game.powerups.Remove(powerup);
            powerup.isCollected = true;
            Game.collectedPowerups.Add(powerup);

        }

        /// <summary>
        /// [ Deprecated v2.2.0 ]
        /// </summary>
        /// <param name="powerup"></param>
        public static void raiseCollected(Powerup powerup)
        {
            powerup.collected(powerup);
        }

    }
}
