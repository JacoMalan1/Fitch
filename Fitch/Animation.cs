﻿using System;

namespace Fitch
{
    class Animation
    {

        public static int frameCounter;
        public static int animCounter;
        public static Texture2D playerStanding = ContentPipe.LoadTexture("player.png");

        public static Texture2D[] runningAnimFrames = new Texture2D[4]
        {
            ContentPipe.LoadTexture("penguin_walk01.png"),
            ContentPipe.LoadTexture("penguin_walk02.png"),
            ContentPipe.LoadTexture("penguin_walk03.png"),
            ContentPipe.LoadTexture("penguin_walk04.png")
        };
        public static Texture2D[] jumpingAnimFrames = new Texture2D[3]
        {
            ContentPipe.LoadTexture("penguin_jump01.png"),
            ContentPipe.LoadTexture("penguin_jump02.png"),
            ContentPipe.LoadTexture("penguin_jump03.png")
        };
        public static Texture2D[] deathAnimFrames = new Texture2D[4]
        {
            ContentPipe.LoadTexture("penguin_die01.png"),
            ContentPipe.LoadTexture("penguin_die02.png"),
            ContentPipe.LoadTexture("penguin_die03.png"),
            ContentPipe.LoadTexture("penguin_die04.png")
        };
        public static Texture2D[] slideAnimFrames = new Texture2D[2]
        {
            ContentPipe.LoadTexture("penguin_slide01.png"),
            ContentPipe.LoadTexture("penguin_slide02.png")
        };
        
        /// <summary>
        /// Initialize everything for animating the player.
        /// </summary>
        public static void Initialize()
        {
            frameCounter = 1;
            animCounter = 0;
        }

        /// <summary>
        /// Check the state of the player and apply an animation frame as needed.
        /// </summary>
        /// <param name="player"></param>
        public static void Update(ref Player player)
        {
            frameCounter++;
            if (!player.isSliding && (player.isRunning || Math.Abs(player.Velocity.X) > 0.1) && !player.isJumping && !player.isDead && player.isStanding && !Game.goal)
            {

                if (frameCounter % Math.Ceiling(Game.TVELOCITY / Math.Abs(player.Velocity.X)) == 0)
                    animCounter++;
                if (animCounter > 3)
                    animCounter = 0;

                Game.playerTexture = runningAnimFrames[animCounter];

            }

            else if (player.isJumping && !player.isStanding && !player.isDead)
            {

                if (frameCounter % 4 == 0)
                    animCounter++;
                if (animCounter > 2)
                    animCounter = 2;

                Game.playerTexture = jumpingAnimFrames[animCounter];

            }

            else if (player.isDead)
            {

                if (frameCounter % 4 == 0)
                    animCounter++;
                if (animCounter > 3)
                    animCounter = 3;

                Game.playerTexture = deathAnimFrames[animCounter];

            }

            else if (!player.isStanding && !player.isJumping && player.Velocity.Y > 0)
            {

                Game.playerTexture = jumpingAnimFrames[2];

            }

            else if (player.isSliding && player.isStanding && !player.isJumping && !(player.Velocity.X == 0))
            {

                if (frameCounter % 20 == 0)
                    animCounter++;
                if (animCounter > 1)
                    animCounter = 1;

                Game.playerTexture = slideAnimFrames[animCounter];

            }

            else
            {

                Game.playerTexture = playerStanding;
                animCounter = 0;

            }

        }

    }
}
