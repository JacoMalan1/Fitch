using System;
using OpenTK;

namespace Fitch
{
    public class Player
    {
        private Vector2 position;
        private float width;
        private float height;

        public Vector2 Position { get { return position; }}
        public float Width { get { return width; }}
        public float Height { get { return height; }}

        public Player(Vector2 position, float width, float height)
        {

            this.position = position;
            this.height = height;
            this.width = width;

        }
    }
}
