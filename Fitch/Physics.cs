using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Fitch
{
    class Physics
    {
        public static void updatePhysics(ref Player player, List<Block> blocks, World world)
        {
            foreach (Block block in blocks)
            {

                if ((block.Position.X == Math.Round((player.Position.X + player.Width / 2) / world.blockSize)) && (block.Position.Y == Math.Round((player.Position.Y + player.Height / 2) / world.blockSize)))
                {
                    
                        player.Velocity /= 2;

                        player.Position = new Vector2(player.Position.X - (player.Position.X % world.blockSize), player.Position.Y - (player.Position.Y % world.blockSize));
                }

            }

            player.Position = new Vector2(player.Position.X + player.Velocity.X, player.Position.Y + player.Velocity.Y);

            if (!(player.Velocity.X == 0) && !(player.Velocity.Y == 0))
            {
                if (player.Velocity.X <= 0.0000000000000001)
                {
                    player.Velocity = new Vector2(0, player.Velocity.Y);
                }

                player.Velocity = new Vector2(player.Velocity.X / 1.02f, player.Velocity.Y);

            }

            //Gravity
            player.Velocity += new Vector2(0, 0.44f);

        }

    }
}
