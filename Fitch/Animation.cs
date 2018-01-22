using System;

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
        
        public static void Initialize()
        {
            frameCounter = 1;
            animCounter = 0;
        }

        public static void Update(ref Player player)
        {
            frameCounter++;
            if ((player.isRunning || Math.Abs(player.Velocity.X) > 0.1) && !player.isJumping && !player.isDead && player.isStanding && !Game.goal)
            {

                if (frameCounter % 4 == 0)
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

            else
            {

                Game.playerTexture = playerStanding;
                animCounter = 0;

            }

        }

    }
}
