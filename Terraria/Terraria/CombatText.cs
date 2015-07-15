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
    public class CombatText
    {
        public static readonly Color DamagedFriendly = new Color(255, 80, 90, 255);
        public static readonly Color DamagedFriendlyCrit = new Color(255, 100, 30, 255);
        public static readonly Color DamagedHostile = new Color(255, 160, 80, 255);
        public static readonly Color DamagedHostileCrit = new Color(255, 100, 30, 255);
        public static readonly Color OthersDamagedHostile = DamagedHostile * 0.4f;
        public static readonly Color OthersDamagedHostileCrit = DamagedHostileCrit * 0.4f;
        public static readonly Color HealLife = new Color(100, 255, 100, 255);
        public static readonly Color HealMana = new Color(100, 100, 255, 255);
        public static readonly Color LifeRegen = new Color(255, 60, 70, 255);
        public static readonly Color LifeRegenNegative = new Color(255, 140, 40, 255);
        public int alphaDir = 1;
        public float scale = 1f;
        public Vector2 position;
        public Vector2 velocity;
        public float alpha;
        public string text;
        public float rotation;
        public Color color;
        public bool active;
        public int lifeTime;
        public bool crit;
        public bool dot;

        public static int NewText(Rectangle location, Color color, string text, bool dramatic = false, bool dot = false)
        {
            if (Main.netMode == 2)
                return 100;
            for (int index1 = 0; index1 < 100; ++index1)
            {
                if (!Main.combatText[index1].active)
                {
                    int index2 = 0;
                    if (dramatic)
                        index2 = 1;
                    Vector2 vector2 = Main.fontCombatText[index2].MeasureString(text);
                    Main.combatText[index1].alpha = 1f;
                    Main.combatText[index1].alphaDir = -1;
                    Main.combatText[index1].active = true;
                    Main.combatText[index1].scale = 0.0f;
                    Main.combatText[index1].rotation = 0.0f;
                    Main.combatText[index1].position.X = (float)(location.X + location.Width * 0.5 - vector2.X * 0.5);
                    Main.combatText[index1].position.Y = (float)(location.Y + location.Height * 0.25 - vector2.Y * 0.5);
                    Main.combatText[index1].position.X += Main.rand.Next(-(int)(location.Width * 0.5), (int)(location.Width * 0.5) + 1);
                    Main.combatText[index1].position.Y += Main.rand.Next(-(int)(location.Height * 0.5), (int)(location.Height * 0.5) + 1);
                    Main.combatText[index1].color = color;
                    Main.combatText[index1].text = text;
                    Main.combatText[index1].velocity.Y = -7f;
                    if (Main.player[Main.myPlayer].gravDir == -1.0)
                    {
                        Main.combatText[index1].velocity.Y *= -1f;
                        Main.combatText[index1].position.Y = (float)(location.Y + location.Height * 0.75 + vector2.Y * 0.5);
                    }
                    Main.combatText[index1].lifeTime = 60;
                    Main.combatText[index1].crit = dramatic;
                    Main.combatText[index1].dot = dot;
                    if (dramatic)
                    {
                        Main.combatText[index1].text = text;
                        Main.combatText[index1].lifeTime *= 2;
                        Main.combatText[index1].velocity.Y *= 2f;
                        Main.combatText[index1].velocity.X = Main.rand.Next(-25, 26) * 0.05f;
                        Main.combatText[index1].rotation = Main.combatText[index1].lifeTime / 2 * (1.0f / 500.0f);
                        if (Main.combatText[index1].velocity.X < 0.0)
                            Main.combatText[index1].rotation *= -1f;
                    }
                    if (dot)
                    {
                        Main.combatText[index1].velocity.Y = -4f;
                        Main.combatText[index1].lifeTime = 40;
                    }
                    return index1;
                }
            }
            return 100;
        }

        public static void clearAll()
        {
            for (int index = 0; index < 100; ++index)
                Main.combatText[index].active = false;
        }

        public void Update()
        {
            if (!active)
                return;
            alpha += alphaDir * 0.05f;
            if (alpha <= 0.6)
                alphaDir = 1;
            if (alpha >= 1.0)
            {
                alpha = 1f;
                alphaDir = -1;
            }
            if (dot)
            {
                velocity.Y += 0.15f;
            }
            else
            {
                velocity.Y *= 0.92f;
                if (crit)
                    velocity.Y *= 0.92f;
            }
            velocity.X *= 0.93f;
            position += velocity;
            --lifeTime;
            if (lifeTime <= 0)
            {
                scale -= 0.1f;
                if (scale < 0.1)
                    active = false;
                lifeTime = 0;
                if (!crit)
                    return;
                alphaDir = -1;
                scale += 0.07f;
            }
            else
            {
                if (crit)
                {
                    if (velocity.X < 0.0)
                        rotation += 1.0f / 1000.0f;
                    else
                        rotation -= 1.0f / 1000.0f;
                }
                if (dot)
                {
                    scale += 0.5f;
                    if (scale <= 0.8)
                        return;
                    scale = 0.8f;
                }
                else
                {
                    if (scale < 1.0)
                        scale += 0.1f;
                    if (scale <= 1.0)
                        return;
                    scale = 1f;
                }
            }
        }

        public static void UpdateCombatText()
        {
            for (int index = 0; index < 100; ++index)
            {
                if (Main.combatText[index].active)
                    Main.combatText[index].Update();
            }
        }
    }
}
