/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using Microsoft.Xna.Framework;

namespace Terraria
{
    public class Star
    {
        public Vector2 position;
        public float scale;
        public float rotation;
        public int type;
        public float twinkle;
        public float twinkleSpeed;
        public float rotationSpeed;

        public static void SpawnStars()
        {
            Main.numStars = Main.rand.Next(65, 130);
            Main.numStars = 130;
            for (int index = 0; index < Main.numStars; ++index)
            {
                Main.star[index] = new Star();
                Main.star[index].position.X = Main.rand.Next(-12, Main.screenWidth + 1);
                Main.star[index].position.Y = Main.rand.Next(-12, Main.screenHeight);
                Main.star[index].rotation = Main.rand.Next(628) * 0.01f;
                Main.star[index].scale = Main.rand.Next(50, 120) * 0.01f;
                Main.star[index].type = Main.rand.Next(0, 5);
                Main.star[index].twinkle = Main.rand.Next(101) * 0.01f;
                Main.star[index].twinkleSpeed = Main.rand.Next(40, 100) * 0.0001f;
                if (Main.rand.Next(2) == 0)
                    Main.star[index].twinkleSpeed *= -1f;
                Main.star[index].rotationSpeed = Main.rand.Next(10, 40) * 0.0001f;
                if (Main.rand.Next(2) == 0)
                    Main.star[index].rotationSpeed *= -1f;
            }
        }

        public static void UpdateStars()
        {
            for (int index = 0; index < Main.numStars; ++index)
            {
                Main.star[index].twinkle += Main.star[index].twinkleSpeed;
                if (Main.star[index].twinkle > 1.0)
                {
                    Main.star[index].twinkle = 1f;
                    Main.star[index].twinkleSpeed *= -1f;
                }
                else if (Main.star[index].twinkle < 0.5)
                {
                    Main.star[index].twinkle = 0.5f;
                    Main.star[index].twinkleSpeed *= -1f;
                }
                Main.star[index].rotation += Main.star[index].rotationSpeed;
                if (Main.star[index].rotation > 6.28)
                    Main.star[index].rotation -= 6.28f;
                if (Main.star[index].rotation < 0.0)
                    Main.star[index].rotation += 6.28f;
            }
        }
    }
}
