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
using System;

namespace Terraria
{
    public class Rain
    {
        public Vector2 position;
        public Vector2 velocity;
        public float scale;
        public float rotation;
        public int alpha;
        public bool active;
        public byte type;

        public static void MakeRain()
        {
            if ((double)Main.screenPosition.Y > Main.worldSurface * 16.0 || Main.gameMenu)
                return;
            float num1 = (float)Main.screenWidth / 1920f * 25f * (float)(0.25 + 1.0 * (double)Main.cloudAlpha);
            for (int index = 0; (double)index < (double)num1; ++index)
            {
                int num2 = 600;
                if ((double)Main.player[Main.myPlayer].velocity.Y < 0.0)
                    num2 += (int)((double)Math.Abs(Main.player[Main.myPlayer].velocity.Y) * 30.0);
                Vector2 Position;
                Position.X = (float)Main.rand.Next((int)Main.screenPosition.X - num2, (int)Main.screenPosition.X + Main.screenWidth + num2);
                Position.Y = Main.screenPosition.Y - (float)Main.rand.Next(20, 100);
                Position.X -= (float)((double)Main.windSpeed * 15.0 * 40.0);
                Position.X += Main.player[Main.myPlayer].velocity.X * 40f;
                if ((double)Position.X < 0.0)
                    Position.X = 0.0f;
                if ((double)Position.X > (double)((Main.maxTilesX - 1) * 16))
                    Position.X = (float)((Main.maxTilesX - 1) * 16);
                int i = (int)Position.X / 16;
                int j = (int)Position.Y / 16;
                if (i < 0)
                    i = 0;
                if (i > Main.maxTilesX - 1)
                    i = Main.maxTilesX - 1;
                if (Main.gameMenu || !WorldGen.SolidTile(i, j) && (int)Main.tile[i, j].wall <= 0)
                {
                    Vector2 Velocity = new Vector2(Main.windSpeed * 12f, 14f);
                    Rain.NewRain(Position, Velocity);
                }
            }
        }

        public void Update()
        {
            this.position += this.velocity;
            if (!Collision.SolidCollision(this.position, 2, 2) && (double)this.position.Y <= (double)Main.screenPosition.Y + (double)Main.screenHeight + 100.0 && !Collision.WetCollision(this.position, 2, 2))
                return;
            this.active = false;
            if ((double)Main.rand.Next(100) >= (double)Main.gfxQuality * 100.0)
                return;
            int Type = 154;
            if ((int)this.type == 3 || (int)this.type == 4 || (int)this.type == 5)
                Type = 218;
            int index = Dust.NewDust(this.position - this.velocity, 2, 2, Type, 0.0f, 0.0f, 0, new Color(), 1f);
            Main.dust[index].position.X -= 2f;
            Main.dust[index].alpha = 38;
            Main.dust[index].velocity *= 0.1f;
            Main.dust[index].velocity += -this.velocity * 0.025f;
            Main.dust[index].scale = 0.75f;
        }

        public static int NewRain(Vector2 Position, Vector2 Velocity)
        {
            int index1 = -1;
            int num1 = (int)((double)Main.maxRain * (double)Main.cloudAlpha);
            if (num1 > Main.maxRain)
                num1 = Main.maxRain;
            float num2 = (float)((1.0 + (double)Main.gfxQuality) / 2.0);
            if ((double)num2 < 0.9)
                num1 = (int)((double)num1 * (double)num2);
            float num3 = (float)(800 - Main.snowTiles);
            if ((double)num3 < 0.0)
                num3 = 0.0f;
            float num4 = num3 / 800f;
            int num5 = (int)((double)num1 * (double)num4);
            for (int index2 = 0; index2 < num5; ++index2)
            {
                if (!Main.rain[index2].active)
                {
                    index1 = index2;
                    break;
                }
            }
            if (index1 == -1)
                return Main.maxRain;
            Rain rain = Main.rain[index1];
            rain.active = true;
            rain.position = Position;
            rain.scale = (float)(1.0 + (double)Main.rand.Next(-20, 21) * 0.00999999977648258);
            rain.velocity = Velocity * rain.scale;
            rain.rotation = (float)Math.Atan2((double)rain.velocity.X, -(double)rain.velocity.Y);
            rain.type = (byte)Main.rand.Next(3);
            if (Main.bloodMoon)
                rain.type += (byte)3;
            return index1;
        }
    }
}
