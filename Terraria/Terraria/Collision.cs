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
using System.Collections.Generic;

namespace Terraria
{
    public class Collision
    {
        public static bool stair = false;
        public static bool stairFall = false;
        public static bool honey = false;
        public static bool sloping = false;
        public static bool landMine = false;
        public static bool up = false;
        public static bool down = false;
        public static float Epsilon = 2.718282f;

        public static Vector2[] CheckLinevLine(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
        {
            if (a1.Equals(a2) && b1.Equals(b2))
            {
                if (!a1.Equals(b1))
                    return new Vector2[0];
                return new Vector2[1]
        {
          a1
        };
            }
            if (b1.Equals(b2))
            {
                if (!Collision.PointOnLine(b1, a1, a2))
                    return new Vector2[0];
                return new Vector2[1]
        {
          b1
        };
            }
            if (a1.Equals(a2))
            {
                if (!Collision.PointOnLine(a1, b1, b2))
                    return new Vector2[0];
                return new Vector2[1]
        {
          a1
        };
            }
            float num1 = (float)(((double)b2.X - (double)b1.X) * ((double)a1.Y - (double)b1.Y) - ((double)b2.Y - (double)b1.Y) * ((double)a1.X - (double)b1.X));
            float num2 = (float)(((double)a2.X - (double)a1.X) * ((double)a1.Y - (double)b1.Y) - ((double)a2.Y - (double)a1.Y) * ((double)a1.X - (double)b1.X));
            float num3 = (float)(((double)b2.Y - (double)b1.Y) * ((double)a2.X - (double)a1.X) - ((double)b2.X - (double)b1.X) * ((double)a2.Y - (double)a1.Y));
            if (-(double)Collision.Epsilon >= (double)num3 || (double)num3 >= (double)Collision.Epsilon)
            {
                float num4 = num1 / num3;
                float num5 = num2 / num3;
                if (0.0 > (double)num4 || (double)num4 > 1.0 || (0.0 > (double)num5 || (double)num5 > 1.0))
                    return new Vector2[0];
                return new Vector2[1]
        {
          new Vector2(a1.X + num4 * (a2.X - a1.X), a1.Y + num4 * (a2.Y - a1.Y))
        };
            }
            if ((-(double)Collision.Epsilon >= (double)num1 || (double)num1 >= (double)Collision.Epsilon) && (-(double)Collision.Epsilon >= (double)num2 || (double)num2 >= (double)Collision.Epsilon))
                return new Vector2[0];
            if (a1.Equals(a2))
                return Collision.OneDimensionalIntersection(b1, b2, a1, a2);
            return Collision.OneDimensionalIntersection(a1, a2, b1, b2);
        }

        private static double DistFromSeg(Vector2 p, Vector2 q0, Vector2 q1, double radius, ref float u)
        {
            double num1 = (double)q1.X - (double)q0.X;
            double num2 = (double)q1.Y - (double)q0.Y;
            double num3 = (double)q0.X - (double)p.X;
            double num4 = (double)q0.Y - (double)p.Y;
            double num5 = Math.Sqrt(num1 * num1 + num2 * num2);
            if (num5 < (double)Collision.Epsilon)
                throw new Exception("Expected line segment, not point.");
            return Math.Abs(num1 * num4 - num3 * num2) / num5;
        }

        private static bool PointOnLine(Vector2 p, Vector2 a1, Vector2 a2)
        {
            float u = 0.0f;
            return Collision.DistFromSeg(p, a1, a2, (double)Collision.Epsilon, ref u) < (double)Collision.Epsilon;
        }

        private static Vector2[] OneDimensionalIntersection(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
        {
            float num1 = a2.X - a1.X;
            float num2 = a2.Y - a1.Y;
            float relativePoint1;
            float relativePoint2;
            if ((double)Math.Abs(num1) > (double)Math.Abs(num2))
            {
                relativePoint1 = (b1.X - a1.X) / num1;
                relativePoint2 = (b2.X - a1.X) / num1;
            }
            else
            {
                relativePoint1 = (b1.Y - a1.Y) / num2;
                relativePoint2 = (b2.Y - a1.Y) / num2;
            }
            List<Vector2> list = new List<Vector2>();
            foreach (float num3 in Collision.FindOverlapPoints(relativePoint1, relativePoint2))
            {
                float x = (float)((double)a2.X * (double)num3 + (double)a1.X * (1.0 - (double)num3));
                float y = (float)((double)a2.Y * (double)num3 + (double)a1.Y * (1.0 - (double)num3));
                list.Add(new Vector2(x, y));
            }
            return list.ToArray();
        }

        private static float[] FindOverlapPoints(float relativePoint1, float relativePoint2)
        {
            float val2_1 = Math.Min(relativePoint1, relativePoint2);
            float val2_2 = Math.Max(relativePoint1, relativePoint2);
            float num1 = Math.Max(0.0f, val2_1);
            float num2 = Math.Min(1f, val2_2);
            if ((double)num1 > (double)num2)
                return new float[0];
            if ((double)num1 == (double)num2)
                return new float[1]
        {
          num1
        };
            return new float[2]
      {
        num1,
        num2
      };
        }

        public static bool CheckAABBvAABBCollision(Vector2 position1, Vector2 dimensions1, Vector2 position2, Vector2 dimensions2)
        {
            if ((double)position1.X < (double)position2.X + (double)dimensions2.X && (double)position1.Y < (double)position2.Y + (double)dimensions2.Y && (double)position1.X + (double)dimensions1.X > (double)position2.X)
                return (double)position1.Y + (double)dimensions1.Y > (double)position2.Y;
            return false;
        }

        public static bool CheckAABBvLineCollision(Vector2 objectPosition, Vector2 objectDimensions, Vector2 lineStart, Vector2 lineEnd, float lineWidth, ref float collisionPoint)
        {
            float y = lineWidth * 0.5f;
            Vector2 position2 = lineStart;
            Vector2 dimensions2 = lineEnd - lineStart;
            if ((double)dimensions2.X > 0.0)
            {
                dimensions2.X += lineWidth;
                position2.X -= y;
            }
            else
            {
                position2.X += dimensions2.X - y;
                dimensions2.X = -dimensions2.X + lineWidth;
            }
            if ((double)dimensions2.Y > 0.0)
            {
                dimensions2.Y += lineWidth;
                position2.Y -= y;
            }
            else
            {
                position2.Y += dimensions2.Y - y;
                dimensions2.Y = -dimensions2.Y + lineWidth;
            }
            if (!Collision.CheckAABBvAABBCollision(objectPosition, objectDimensions, position2, dimensions2))
                return false;
            Vector2 spinningpoint1 = objectPosition - lineStart;
            Vector2 spinningpoint2 = spinningpoint1 + objectDimensions;
            Vector2 spinningpoint3 = new Vector2(spinningpoint1.X, spinningpoint2.Y);
            Vector2 spinningpoint4 = new Vector2(spinningpoint2.X, spinningpoint1.Y);
            Vector2 vector2_1 = lineEnd - lineStart;
            float x = vector2_1.Length();
            float num1 = (float)Math.Atan2((double)vector2_1.Y, (double)vector2_1.X);
            Vector2[] vector2Array = new Vector2[4]
      {
        Utils.RotatedBy(spinningpoint1, -(double) num1, new Vector2()),
        Utils.RotatedBy(spinningpoint4, -(double) num1, new Vector2()),
        Utils.RotatedBy(spinningpoint2, -(double) num1, new Vector2()),
        Utils.RotatedBy(spinningpoint3, -(double) num1, new Vector2())
      };
            collisionPoint = x;
            bool flag = false;
            for (int index = 0; index < vector2Array.Length; ++index)
            {
                if ((double)Math.Abs(vector2Array[index].Y) < (double)y && (double)vector2Array[index].X < (double)collisionPoint && (double)vector2Array[index].X >= 0.0)
                {
                    collisionPoint = vector2Array[index].X;
                    flag = true;
                }
            }
            Vector2 vector2_2 = new Vector2(0.0f, y);
            Vector2 vector2_3 = new Vector2(x, y);
            Vector2 vector2_4 = new Vector2(0.0f, -y);
            Vector2 vector2_5 = new Vector2(x, -y);
            for (int index1 = 0; index1 < vector2Array.Length; ++index1)
            {
                int index2 = (index1 + 1) % vector2Array.Length;
                Vector2 vector2_6 = vector2_3 - vector2_2;
                Vector2 vector2_7 = vector2Array[index2] - vector2Array[index1];
                float num2 = (float)((double)vector2_6.X * (double)vector2_7.Y - (double)vector2_6.Y * (double)vector2_7.X);
                if ((double)num2 != 0.0)
                {
                    Vector2 vector2_8 = vector2Array[index1] - vector2_2;
                    float num3 = (float)((double)vector2_8.X * (double)vector2_7.Y - (double)vector2_8.Y * (double)vector2_7.X) / num2;
                    if ((double)num3 >= 0.0 && (double)num3 <= 1.0)
                    {
                        float num4 = (float)((double)vector2_8.X * (double)vector2_6.Y - (double)vector2_8.Y * (double)vector2_6.X) / num2;
                        if ((double)num4 >= 0.0 && (double)num4 <= 1.0)
                        {
                            flag = true;
                            collisionPoint = Math.Min(collisionPoint, vector2_2.X + num3 * vector2_6.X);
                        }
                    }
                }
                vector2_6 = vector2_5 - vector2_4;
                float num5 = (float)((double)vector2_6.X * (double)vector2_7.Y - (double)vector2_6.Y * (double)vector2_7.X);
                if ((double)num5 != 0.0)
                {
                    Vector2 vector2_8 = vector2Array[index1] - vector2_4;
                    float num3 = (float)((double)vector2_8.X * (double)vector2_7.Y - (double)vector2_8.Y * (double)vector2_7.X) / num5;
                    if ((double)num3 >= 0.0 && (double)num3 <= 1.0)
                    {
                        float num4 = (float)((double)vector2_8.X * (double)vector2_6.Y - (double)vector2_8.Y * (double)vector2_6.X) / num5;
                        if ((double)num4 >= 0.0 && (double)num4 <= 1.0)
                        {
                            flag = true;
                            collisionPoint = Math.Min(collisionPoint, vector2_4.X + num3 * vector2_6.X);
                        }
                    }
                }
            }
            return flag;
        }

        public static bool CanHit(Vector2 Position1, int Width1, int Height1, Vector2 Position2, int Width2, int Height2)
        {
            int index1 = (int)(((double)Position1.X + (double)(Width1 / 2)) / 16.0);
            int index2 = (int)(((double)Position1.Y + (double)(Height1 / 2)) / 16.0);
            int num1 = (int)(((double)Position2.X + (double)(Width2 / 2)) / 16.0);
            int num2 = (int)(((double)Position2.Y + (double)(Height2 / 2)) / 16.0);
            if (index1 <= 1)
                index1 = 1;
            if (index1 >= Main.maxTilesX)
                index1 = Main.maxTilesX - 1;
            if (num1 <= 1)
                num1 = 1;
            if (num1 >= Main.maxTilesX)
                num1 = Main.maxTilesX - 1;
            if (index2 <= 1)
                index2 = 1;
            if (index2 >= Main.maxTilesY)
                index2 = Main.maxTilesY - 1;
            if (num2 <= 1)
                num2 = 1;
            if (num2 >= Main.maxTilesY)
                num2 = Main.maxTilesY - 1;
            try
            {
                do
                {
                    int num3 = Math.Abs(index1 - num1);
                    int num4 = Math.Abs(index2 - num2);
                    if (index1 == num1 && index2 == num2)
                        return true;
                    if (num3 > num4)
                    {
                        if (index1 < num1)
                            ++index1;
                        else
                            --index1;
                        if (Main.tile[index1, index2 - 1] == null || Main.tile[index1, index2 + 1] == null || !Main.tile[index1, index2 - 1].inActive() && Main.tile[index1, index2 - 1].active() && (Main.tileSolid[(int)Main.tile[index1, index2 - 1].type] && !Main.tileSolidTop[(int)Main.tile[index1, index2 - 1].type]) && ((int)Main.tile[index1, index2 - 1].slope() == 0 && !Main.tile[index1, index2 - 1].halfBrick() && (!Main.tile[index1, index2 + 1].inActive() && Main.tile[index1, index2 + 1].active())) && (Main.tileSolid[(int)Main.tile[index1, index2 + 1].type] && !Main.tileSolidTop[(int)Main.tile[index1, index2 + 1].type] && ((int)Main.tile[index1, index2 + 1].slope() == 0 && !Main.tile[index1, index2 + 1].halfBrick())))
                            return false;
                    }
                    else
                    {
                        if (index2 < num2)
                            ++index2;
                        else
                            --index2;
                        if (Main.tile[index1 - 1, index2] == null || Main.tile[index1 + 1, index2] == null || !Main.tile[index1 - 1, index2].inActive() && Main.tile[index1 - 1, index2].active() && (Main.tileSolid[(int)Main.tile[index1 - 1, index2].type] && !Main.tileSolidTop[(int)Main.tile[index1 - 1, index2].type]) && ((int)Main.tile[index1 - 1, index2].slope() == 0 && !Main.tile[index1 - 1, index2].halfBrick() && (!Main.tile[index1 + 1, index2].inActive() && Main.tile[index1 + 1, index2].active())) && (Main.tileSolid[(int)Main.tile[index1 + 1, index2].type] && !Main.tileSolidTop[(int)Main.tile[index1 + 1, index2].type] && ((int)Main.tile[index1 + 1, index2].slope() == 0 && !Main.tile[index1 + 1, index2].halfBrick())))
                            return false;
                    }
                }
                while (Main.tile[index1, index2] != null && (Main.tile[index1, index2].inActive() || !Main.tile[index1, index2].active() || (!Main.tileSolid[(int)Main.tile[index1, index2].type] || Main.tileSolidTop[(int)Main.tile[index1, index2].type])));
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool CanHitWithCheck(Vector2 Position1, int Width1, int Height1, Vector2 Position2, int Width2, int Height2, Utils.PerLinePoint check)
        {
            int x = (int)(((double)Position1.X + (double)(Width1 / 2)) / 16.0);
            int y = (int)(((double)Position1.Y + (double)(Height1 / 2)) / 16.0);
            int num1 = (int)(((double)Position2.X + (double)(Width2 / 2)) / 16.0);
            int num2 = (int)(((double)Position2.Y + (double)(Height2 / 2)) / 16.0);
            if (x <= 1)
                x = 1;
            if (x >= Main.maxTilesX)
                x = Main.maxTilesX - 1;
            if (num1 <= 1)
                num1 = 1;
            if (num1 >= Main.maxTilesX)
                num1 = Main.maxTilesX - 1;
            if (y <= 1)
                y = 1;
            if (y >= Main.maxTilesY)
                y = Main.maxTilesY - 1;
            if (num2 <= 1)
                num2 = 1;
            if (num2 >= Main.maxTilesY)
                num2 = Main.maxTilesY - 1;
            try
            {
                do
                {
                    int num3 = Math.Abs(x - num1);
                    int num4 = Math.Abs(y - num2);
                    if (x == num1 && y == num2)
                        return true;
                    if (num3 > num4)
                    {
                        if (x < num1)
                            ++x;
                        else
                            --x;
                        if (Main.tile[x, y - 1] == null || Main.tile[x, y + 1] == null || !Main.tile[x, y - 1].inActive() && Main.tile[x, y - 1].active() && (Main.tileSolid[(int)Main.tile[x, y - 1].type] && !Main.tileSolidTop[(int)Main.tile[x, y - 1].type]) && ((int)Main.tile[x, y - 1].slope() == 0 && !Main.tile[x, y - 1].halfBrick() && (!Main.tile[x, y + 1].inActive() && Main.tile[x, y + 1].active())) && (Main.tileSolid[(int)Main.tile[x, y + 1].type] && !Main.tileSolidTop[(int)Main.tile[x, y + 1].type] && ((int)Main.tile[x, y + 1].slope() == 0 && !Main.tile[x, y + 1].halfBrick())))
                            return false;
                    }
                    else
                    {
                        if (y < num2)
                            ++y;
                        else
                            --y;
                        if (Main.tile[x - 1, y] == null || Main.tile[x + 1, y] == null || !Main.tile[x - 1, y].inActive() && Main.tile[x - 1, y].active() && (Main.tileSolid[(int)Main.tile[x - 1, y].type] && !Main.tileSolidTop[(int)Main.tile[x - 1, y].type]) && ((int)Main.tile[x - 1, y].slope() == 0 && !Main.tile[x - 1, y].halfBrick() && (!Main.tile[x + 1, y].inActive() && Main.tile[x + 1, y].active())) && (Main.tileSolid[(int)Main.tile[x + 1, y].type] && !Main.tileSolidTop[(int)Main.tile[x + 1, y].type] && ((int)Main.tile[x + 1, y].slope() == 0 && !Main.tile[x + 1, y].halfBrick())))
                            return false;
                    }
                }
                while (Main.tile[x, y] != null && (Main.tile[x, y].inActive() || !Main.tile[x, y].active() || (!Main.tileSolid[(int)Main.tile[x, y].type] || Main.tileSolidTop[(int)Main.tile[x, y].type])) && check(x, y));
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool CanHitLine(Vector2 Position1, int Width1, int Height1, Vector2 Position2, int Width2, int Height2)
        {
            int index1 = (int)(((double)Position1.X + (double)(Width1 / 2)) / 16.0);
            int index2 = (int)(((double)Position1.Y + (double)(Height1 / 2)) / 16.0);
            int num1 = (int)(((double)Position2.X + (double)(Width2 / 2)) / 16.0);
            int num2 = (int)(((double)Position2.Y + (double)(Height2 / 2)) / 16.0);
            if (index1 <= 1)
                index1 = 1;
            if (index1 >= Main.maxTilesX)
                index1 = Main.maxTilesX - 1;
            if (num1 <= 1)
                num1 = 1;
            if (num1 >= Main.maxTilesX)
                num1 = Main.maxTilesX - 1;
            if (index2 <= 1)
                index2 = 1;
            if (index2 >= Main.maxTilesY)
                index2 = Main.maxTilesY - 1;
            if (num2 <= 1)
                num2 = 1;
            if (num2 >= Main.maxTilesY)
                num2 = Main.maxTilesY - 1;
            float num3 = (float)Math.Abs(index1 - num1);
            float num4 = (float)Math.Abs(index2 - num2);
            if ((double)num3 == 0.0 && (double)num4 == 0.0)
                return true;
            float num5 = 1f;
            float num6 = 1f;
            if ((double)num3 == 0.0 || (double)num4 == 0.0)
            {
                if ((double)num3 == 0.0)
                    num5 = 0.0f;
                if ((double)num4 == 0.0)
                    num6 = 0.0f;
            }
            else if ((double)num3 > (double)num4)
                num5 = num3 / num4;
            else
                num6 = num4 / num3;
            float num7 = 0.0f;
            float num8 = 0.0f;
            int num9 = 1;
            if (index2 < num2)
                num9 = 2;
            int num10 = (int)num3;
            int num11 = (int)num4;
            int num12 = Math.Sign(num1 - index1);
            int num13 = Math.Sign(num2 - index2);
            bool flag1 = false;
            bool flag2 = false;
            try
            {
                do
                {
                    if (num9 == 2)
                    {
                        float num14 = num7 + num5;
                        int num15 = (int)num14;
                        num7 = num14 % 1f;
                        for (int index3 = 0; index3 < num15; ++index3)
                        {
                            if (Main.tile[index1, index2 - 1] == null || Main.tile[index1, index2] == null || Main.tile[index1, index2 + 1] == null)
                                return false;
                            Tile tile1 = Main.tile[index1, index2 - 1];
                            Tile tile2 = Main.tile[index1, index2 + 1];
                            Tile tile3 = Main.tile[index1, index2];
                            if (!tile1.inActive() && tile1.active() && (Main.tileSolid[(int)tile1.type] && !Main.tileSolidTop[(int)tile1.type]) || !tile2.inActive() && tile2.active() && (Main.tileSolid[(int)tile2.type] && !Main.tileSolidTop[(int)tile2.type]) || !tile3.inActive() && tile3.active() && (Main.tileSolid[(int)tile3.type] && !Main.tileSolidTop[(int)tile3.type]))
                                return false;
                            if (num10 == 0 && num11 == 0)
                            {
                                flag1 = true;
                                break;
                            }
                            index1 += num12;
                            --num10;
                            if (num10 == 0 && num11 == 0 && num15 == 1)
                                flag2 = true;
                        }
                        if (num11 != 0)
                            num9 = 1;
                    }
                    else if (num9 == 1)
                    {
                        float num14 = num8 + num6;
                        int num15 = (int)num14;
                        num8 = num14 % 1f;
                        for (int index3 = 0; index3 < num15; ++index3)
                        {
                            if (Main.tile[index1 - 1, index2] == null || Main.tile[index1, index2] == null || Main.tile[index1 + 1, index2] == null)
                                return false;
                            Tile tile1 = Main.tile[index1 - 1, index2];
                            Tile tile2 = Main.tile[index1 + 1, index2];
                            Tile tile3 = Main.tile[index1, index2];
                            if (!tile1.inActive() && tile1.active() && (Main.tileSolid[(int)tile1.type] && !Main.tileSolidTop[(int)tile1.type]) || !tile2.inActive() && tile2.active() && (Main.tileSolid[(int)tile2.type] && !Main.tileSolidTop[(int)tile2.type]) || !tile3.inActive() && tile3.active() && (Main.tileSolid[(int)tile3.type] && !Main.tileSolidTop[(int)tile3.type]))
                                return false;
                            if (num10 == 0 && num11 == 0)
                            {
                                flag1 = true;
                                break;
                            }
                            index2 += num13;
                            --num11;
                            if (num10 == 0 && num11 == 0 && num15 == 1)
                                flag2 = true;
                        }
                        if (num10 != 0)
                            num9 = 2;
                    }
                    if (Main.tile[index1, index2] == null)
                        return false;
                    Tile tile = Main.tile[index1, index2];
                    if (!tile.inActive() && tile.active() && (Main.tileSolid[(int)tile.type] && !Main.tileSolidTop[(int)tile.type]))
                        return false;
                }
                while (!flag1 && !flag2);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool TupleHitLine(int x1, int y1, int x2, int y2, int ignoreX, int ignoreY, List<Tuple<int, int>> ignoreTargets, out Tuple<int, int> col)
        {
            int num1 = x1;
            int num2 = y1;
            int num3 = x2;
            int num4 = y2;
            int index1 = Utils.Clamp<int>(num1, 1, Main.maxTilesX - 1);
            int num5 = Utils.Clamp<int>(num3, 1, Main.maxTilesX - 1);
            int index2 = Utils.Clamp<int>(num2, 1, Main.maxTilesY - 1);
            int num6 = Utils.Clamp<int>(num4, 1, Main.maxTilesY - 1);
            float num7 = (float)Math.Abs(index1 - num5);
            float num8 = (float)Math.Abs(index2 - num6);
            if ((double)num7 == 0.0 && (double)num8 == 0.0)
            {
                col = new Tuple<int, int>(index1, index2);
                return true;
            }
            float num9 = 1f;
            float num10 = 1f;
            if ((double)num7 == 0.0 || (double)num8 == 0.0)
            {
                if ((double)num7 == 0.0)
                    num9 = 0.0f;
                if ((double)num8 == 0.0)
                    num10 = 0.0f;
            }
            else if ((double)num7 > (double)num8)
                num9 = num7 / num8;
            else
                num10 = num8 / num7;
            float num11 = 0.0f;
            float num12 = 0.0f;
            int num13 = 1;
            if (index2 < num6)
                num13 = 2;
            int num14 = (int)num7;
            int num15 = (int)num8;
            int num16 = Math.Sign(num5 - index1);
            int num17 = Math.Sign(num6 - index2);
            bool flag1 = false;
            bool flag2 = false;
            try
            {
                do
                {
                    if (num13 == 2)
                    {
                        float num18 = num11 + num9;
                        int num19 = (int)num18;
                        num11 = num18 % 1f;
                        for (int index3 = 0; index3 < num19; ++index3)
                        {
                            if (Main.tile[index1, index2 - 1] == null)
                            {
                                col = new Tuple<int, int>(index1, index2 - 1);
                                return false;
                            }
                            if (Main.tile[index1, index2 + 1] == null)
                            {
                                col = new Tuple<int, int>(index1, index2 + 1);
                                return false;
                            }
                            Tile tile1 = Main.tile[index1, index2 - 1];
                            Tile tile2 = Main.tile[index1, index2 + 1];
                            Tile tile3 = Main.tile[index1, index2];
                            if (!ignoreTargets.Contains(new Tuple<int, int>(index1, index2)) && !ignoreTargets.Contains(new Tuple<int, int>(index1, index2 - 1)) && !ignoreTargets.Contains(new Tuple<int, int>(index1, index2 + 1)))
                            {
                                if (ignoreY != -1 && num17 < 0 && (!tile1.inActive() && tile1.active()) && (Main.tileSolid[(int)tile1.type] && !Main.tileSolidTop[(int)tile1.type]))
                                {
                                    col = new Tuple<int, int>(index1, index2 - 1);
                                    return true;
                                }
                                if (ignoreY != 1 && num17 > 0 && (!tile2.inActive() && tile2.active()) && (Main.tileSolid[(int)tile2.type] && !Main.tileSolidTop[(int)tile2.type]))
                                {
                                    col = new Tuple<int, int>(index1, index2 + 1);
                                    return true;
                                }
                                if (!tile3.inActive() && tile3.active() && (Main.tileSolid[(int)tile3.type] && !Main.tileSolidTop[(int)tile3.type]))
                                {
                                    col = new Tuple<int, int>(index1, index2);
                                    return true;
                                }
                            }
                            if (num14 == 0 && num15 == 0)
                            {
                                flag1 = true;
                                break;
                            }
                            index1 += num16;
                            --num14;
                            if (num14 == 0 && num15 == 0 && num19 == 1)
                                flag2 = true;
                        }
                        if (num15 != 0)
                            num13 = 1;
                    }
                    else if (num13 == 1)
                    {
                        float num18 = num12 + num10;
                        int num19 = (int)num18;
                        num12 = num18 % 1f;
                        for (int index3 = 0; index3 < num19; ++index3)
                        {
                            if (Main.tile[index1 - 1, index2] == null)
                            {
                                col = new Tuple<int, int>(index1 - 1, index2);
                                return false;
                            }
                            if (Main.tile[index1 + 1, index2] == null)
                            {
                                col = new Tuple<int, int>(index1 + 1, index2);
                                return false;
                            }
                            Tile tile1 = Main.tile[index1 - 1, index2];
                            Tile tile2 = Main.tile[index1 + 1, index2];
                            Tile tile3 = Main.tile[index1, index2];
                            if (!ignoreTargets.Contains(new Tuple<int, int>(index1, index2)) && !ignoreTargets.Contains(new Tuple<int, int>(index1 - 1, index2)) && !ignoreTargets.Contains(new Tuple<int, int>(index1 + 1, index2)))
                            {
                                if (ignoreX != -1 && num16 < 0 && (!tile1.inActive() && tile1.active()) && (Main.tileSolid[(int)tile1.type] && !Main.tileSolidTop[(int)tile1.type]))
                                {
                                    col = new Tuple<int, int>(index1 - 1, index2);
                                    return true;
                                }
                                if (ignoreX != 1 && num16 > 0 && (!tile2.inActive() && tile2.active()) && (Main.tileSolid[(int)tile2.type] && !Main.tileSolidTop[(int)tile2.type]))
                                {
                                    col = new Tuple<int, int>(index1 + 1, index2);
                                    return true;
                                }
                                if (!tile3.inActive() && tile3.active() && (Main.tileSolid[(int)tile3.type] && !Main.tileSolidTop[(int)tile3.type]))
                                {
                                    col = new Tuple<int, int>(index1, index2);
                                    return true;
                                }
                            }
                            if (num14 == 0 && num15 == 0)
                            {
                                flag1 = true;
                                break;
                            }
                            index2 += num17;
                            --num15;
                            if (num14 == 0 && num15 == 0 && num19 == 1)
                                flag2 = true;
                        }
                        if (num14 != 0)
                            num13 = 2;
                    }
                    if (Main.tile[index1, index2] == null)
                    {
                        col = new Tuple<int, int>(index1, index2);
                        return false;
                    }
                    Tile tile = Main.tile[index1, index2];
                    if (!ignoreTargets.Contains(new Tuple<int, int>(index1, index2)) && !tile.inActive() && (tile.active() && Main.tileSolid[(int)tile.type]) && !Main.tileSolidTop[(int)tile.type])
                    {
                        col = new Tuple<int, int>(index1, index2);
                        return true;
                    }
                }
                while (!flag1 && !flag2);
                col = new Tuple<int, int>(index1, index2);
                return true;
            }
            catch
            {
                col = new Tuple<int, int>(x1, y1);
                return false;
            }
        }

        public static Tuple<int, int> TupleHitLineWall(int x1, int y1, int x2, int y2)
        {
            int x = x1;
            int y = y1;
            int num1 = x2;
            int num2 = y2;
            if (x <= 1)
                x = 1;
            if (x >= Main.maxTilesX)
                x = Main.maxTilesX - 1;
            if (num1 <= 1)
                num1 = 1;
            if (num1 >= Main.maxTilesX)
                num1 = Main.maxTilesX - 1;
            if (y <= 1)
                y = 1;
            if (y >= Main.maxTilesY)
                y = Main.maxTilesY - 1;
            if (num2 <= 1)
                num2 = 1;
            if (num2 >= Main.maxTilesY)
                num2 = Main.maxTilesY - 1;
            float num3 = (float)Math.Abs(x - num1);
            float num4 = (float)Math.Abs(y - num2);
            if ((double)num3 == 0.0 && (double)num4 == 0.0)
                return new Tuple<int, int>(x, y);
            float num5 = 1f;
            float num6 = 1f;
            if ((double)num3 == 0.0 || (double)num4 == 0.0)
            {
                if ((double)num3 == 0.0)
                    num5 = 0.0f;
                if ((double)num4 == 0.0)
                    num6 = 0.0f;
            }
            else if ((double)num3 > (double)num4)
                num5 = num3 / num4;
            else
                num6 = num4 / num3;
            float num7 = 0.0f;
            float num8 = 0.0f;
            int num9 = 1;
            if (y < num2)
                num9 = 2;
            int num10 = (int)num3;
            int num11 = (int)num4;
            int num12 = Math.Sign(num1 - x);
            int num13 = Math.Sign(num2 - y);
            bool flag1 = false;
            bool flag2 = false;
            try
            {
                do
                {
                    if (num9 == 2)
                    {
                        float num14 = num7 + num5;
                        int num15 = (int)num14;
                        num7 = num14 % 1f;
                        for (int index = 0; index < num15; ++index)
                        {
                            Tile tile = Main.tile[x, y];
                            if (Collision.HitWallSubstep(x, y))
                                return new Tuple<int, int>(x, y);
                            if (num10 == 0 && num11 == 0)
                            {
                                flag1 = true;
                                break;
                            }
                            x += num12;
                            --num10;
                            if (num10 == 0 && num11 == 0 && num15 == 1)
                                flag2 = true;
                        }
                        if (num11 != 0)
                            num9 = 1;
                    }
                    else if (num9 == 1)
                    {
                        float num14 = num8 + num6;
                        int num15 = (int)num14;
                        num8 = num14 % 1f;
                        for (int index = 0; index < num15; ++index)
                        {
                            Tile tile = Main.tile[x, y];
                            if (Collision.HitWallSubstep(x, y))
                                return new Tuple<int, int>(x, y);
                            if (num10 == 0 && num11 == 0)
                            {
                                flag1 = true;
                                break;
                            }
                            y += num13;
                            --num11;
                            if (num10 == 0 && num11 == 0 && num15 == 1)
                                flag2 = true;
                        }
                        if (num10 != 0)
                            num9 = 2;
                    }
                    if (Main.tile[x, y] == null)
                        return new Tuple<int, int>(-1, -1);
                    Tile tile1 = Main.tile[x, y];
                    if (Collision.HitWallSubstep(x, y))
                        return new Tuple<int, int>(x, y);
                }
                while (!flag1 && !flag2);
                return new Tuple<int, int>(x, y);
            }
            catch
            {
                return new Tuple<int, int>(-1, -1);
            }
        }

        public static bool HitWallSubstep(int x, int y)
        {
            if ((int)Main.tile[x, y].wall == 0)
                return false;
            bool flag1 = false;
            if (Main.wallHouse[(int)Main.tile[x, y].wall])
                flag1 = true;
            if (!flag1)
            {
                for (int index1 = -1; index1 < 2; ++index1)
                {
                    for (int index2 = -1; index2 < 2; ++index2)
                    {
                        if ((index1 != 0 || index2 != 0) && (int)Main.tile[x + index1, y + index2].wall == 0)
                            flag1 = true;
                    }
                }
            }
            if (Main.tile[x, y].active() && flag1)
            {
                bool flag2 = true;
                for (int index1 = -1; index1 < 2; ++index1)
                {
                    for (int index2 = -1; index2 < 2; ++index2)
                    {
                        if (index1 != 0 || index2 != 0)
                        {
                            Tile tile = Main.tile[x + index1, y + index2];
                            if (!tile.active() || !Main.tileSolid[(int)tile.type] || Main.tileSolidTop[(int)tile.type])
                                flag2 = false;
                        }
                    }
                }
                if (flag2)
                    flag1 = false;
            }
            return flag1;
        }

        public static bool EmptyTile(int i, int j, bool ignoreTiles = false)
        {
            Rectangle rectangle = new Rectangle(i * 16, j * 16, 16, 16);
            if (Main.tile[i, j].active() && !ignoreTiles)
                return false;
            for (int index = 0; index < (int)byte.MaxValue; ++index)
            {
                if (Main.player[index].active && rectangle.Intersects(new Rectangle((int)Main.player[index].position.X, (int)Main.player[index].position.Y, Main.player[index].width, Main.player[index].height)))
                    return false;
            }
            for (int index = 0; index < 200; ++index)
            {
                if (Main.npc[index].active && rectangle.Intersects(new Rectangle((int)Main.npc[index].position.X, (int)Main.npc[index].position.Y, Main.npc[index].width, Main.npc[index].height)))
                    return false;
            }
            return true;
        }

        public static bool DrownCollision(Vector2 Position, int Width, int Height, float gravDir = -1f)
        {
            Vector2 vector2_1 = new Vector2(Position.X + (float)(Width / 2), Position.Y + (float)(Height / 2));
            int num1 = 10;
            int num2 = 12;
            if (num1 > Width)
                num1 = Width;
            if (num2 > Height)
                num2 = Height;
            vector2_1 = new Vector2(vector2_1.X - (float)(num1 / 2), Position.Y - 2f);
            if ((double)gravDir == -1.0)
                vector2_1.Y += (float)(Height / 2 - 6);
            int num3 = (int)((double)Position.X / 16.0) - 1;
            int num4 = (int)(((double)Position.X + (double)Width) / 16.0) + 2;
            int num5 = (int)((double)Position.Y / 16.0) - 1;
            int num6 = (int)(((double)Position.Y + (double)Height) / 16.0) + 2;
            int num7 = Utils.Clamp<int>(num3, 0, Main.maxTilesX - 1);
            int num8 = Utils.Clamp<int>(num4, 0, Main.maxTilesX - 1);
            int num9 = Utils.Clamp<int>(num5, 0, Main.maxTilesY - 1);
            int num10 = Utils.Clamp<int>(num6, 0, Main.maxTilesY - 1);
            for (int index1 = num7; index1 < num8; ++index1)
            {
                for (int index2 = num9; index2 < num10; ++index2)
                {
                    if (Main.tile[index1, index2] != null && (int)Main.tile[index1, index2].liquid > 0 && !Main.tile[index1, index2].lava())
                    {
                        Vector2 vector2_2;
                        vector2_2.X = (float)(index1 * 16);
                        vector2_2.Y = (float)(index2 * 16);
                        int num11 = 16;
                        float num12 = (float)(256 - (int)Main.tile[index1, index2].liquid) / 32f;
                        vector2_2.Y += num12 * 2f;
                        int num13 = num11 - (int)((double)num12 * 2.0);
                        if ((double)vector2_1.X + (double)num1 > (double)vector2_2.X && (double)vector2_1.X < (double)vector2_2.X + 16.0 && ((double)vector2_1.Y + (double)num2 > (double)vector2_2.Y && (double)vector2_1.Y < (double)vector2_2.Y + (double)num13))
                            return true;
                    }
                }
            }
            return false;
        }

        public static bool WetCollision(Vector2 Position, int Width, int Height)
        {
            Collision.honey = false;
            Vector2 vector2_1 = new Vector2(Position.X + (float)(Width / 2), Position.Y + (float)(Height / 2));
            int num1 = 10;
            int num2 = Height / 2;
            if (num1 > Width)
                num1 = Width;
            if (num2 > Height)
                num2 = Height;
            vector2_1 = new Vector2(vector2_1.X - (float)(num1 / 2), vector2_1.Y - (float)(num2 / 2));
            int num3 = (int)((double)Position.X / 16.0) - 1;
            int num4 = (int)(((double)Position.X + (double)Width) / 16.0) + 2;
            int num5 = (int)((double)Position.Y / 16.0) - 1;
            int num6 = (int)(((double)Position.Y + (double)Height) / 16.0) + 2;
            int num7 = Utils.Clamp<int>(num3, 0, Main.maxTilesX - 1);
            int num8 = Utils.Clamp<int>(num4, 0, Main.maxTilesX - 1);
            int num9 = Utils.Clamp<int>(num5, 0, Main.maxTilesY - 1);
            int num10 = Utils.Clamp<int>(num6, 0, Main.maxTilesY - 1);
            Vector2 vector2_2;
            for (int index1 = num7; index1 < num8; ++index1)
            {
                for (int index2 = num9; index2 < num10; ++index2)
                {
                    if (Main.tile[index1, index2] != null)
                    {
                        if ((int)Main.tile[index1, index2].liquid > 0)
                        {
                            vector2_2.X = (float)(index1 * 16);
                            vector2_2.Y = (float)(index2 * 16);
                            int num11 = 16;
                            float num12 = (float)(256 - (int)Main.tile[index1, index2].liquid) / 32f;
                            vector2_2.Y += num12 * 2f;
                            int num13 = num11 - (int)((double)num12 * 2.0);
                            if ((double)vector2_1.X + (double)num1 > (double)vector2_2.X && (double)vector2_1.X < (double)vector2_2.X + 16.0 && ((double)vector2_1.Y + (double)num2 > (double)vector2_2.Y && (double)vector2_1.Y < (double)vector2_2.Y + (double)num13))
                            {
                                if (Main.tile[index1, index2].honey())
                                    Collision.honey = true;
                                return true;
                            }
                        }
                        else if (Main.tile[index1, index2].active() && (int)Main.tile[index1, index2].slope() != 0 && (index2 > 0 && Main.tile[index1, index2 - 1] != null) && (int)Main.tile[index1, index2 - 1].liquid > 0)
                        {
                            vector2_2.X = (float)(index1 * 16);
                            vector2_2.Y = (float)(index2 * 16);
                            int num11 = 16;
                            if ((double)vector2_1.X + (double)num1 > (double)vector2_2.X && (double)vector2_1.X < (double)vector2_2.X + 16.0 && ((double)vector2_1.Y + (double)num2 > (double)vector2_2.Y && (double)vector2_1.Y < (double)vector2_2.Y + (double)num11))
                            {
                                if (Main.tile[index1, index2 - 1].honey())
                                    Collision.honey = true;
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static bool LavaCollision(Vector2 Position, int Width, int Height)
        {
            int num1 = Height;
            int num2 = (int)((double)Position.X / 16.0) - 1;
            int num3 = (int)(((double)Position.X + (double)Width) / 16.0) + 2;
            int num4 = (int)((double)Position.Y / 16.0) - 1;
            int num5 = (int)(((double)Position.Y + (double)Height) / 16.0) + 2;
            int num6 = Utils.Clamp<int>(num2, 0, Main.maxTilesX - 1);
            int num7 = Utils.Clamp<int>(num3, 0, Main.maxTilesX - 1);
            int num8 = Utils.Clamp<int>(num4, 0, Main.maxTilesY - 1);
            int num9 = Utils.Clamp<int>(num5, 0, Main.maxTilesY - 1);
            for (int index1 = num6; index1 < num7; ++index1)
            {
                for (int index2 = num8; index2 < num9; ++index2)
                {
                    if (Main.tile[index1, index2] != null && (int)Main.tile[index1, index2].liquid > 0 && Main.tile[index1, index2].lava())
                    {
                        Vector2 vector2;
                        vector2.X = (float)(index1 * 16);
                        vector2.Y = (float)(index2 * 16);
                        int num10 = 16;
                        float num11 = (float)(256 - (int)Main.tile[index1, index2].liquid) / 32f;
                        vector2.Y += num11 * 2f;
                        int num12 = num10 - (int)((double)num11 * 2.0);
                        if ((double)Position.X + (double)Width > (double)vector2.X && (double)Position.X < (double)vector2.X + 16.0 && ((double)Position.Y + (double)num1 > (double)vector2.Y && (double)Position.Y < (double)vector2.Y + (double)num12))
                            return true;
                    }
                }
            }
            return false;
        }

        public static Vector4 WalkDownSlope(Vector2 Position, Vector2 Velocity, int Width, int Height, float gravity = 0.0f)
        {
            if ((double)Velocity.Y != (double)gravity)
                return new Vector4(Position, Velocity.X, Velocity.Y);
            Vector2 vector2_1 = Position;
            int num1 = (int)((double)vector2_1.X / 16.0);
            int num2 = (int)(((double)vector2_1.X + (double)Width) / 16.0);
            int num3 = (int)(((double)Position.Y + (double)Height + 4.0) / 16.0);
            int num4 = Utils.Clamp<int>(num1, 0, Main.maxTilesX - 1);
            int num5 = Utils.Clamp<int>(num2, 0, Main.maxTilesX - 1);
            int num6 = Utils.Clamp<int>(num3, 0, Main.maxTilesY - 3);
            float num7 = (float)((num6 + 3) * 16);
            int index1 = -1;
            int index2 = -1;
            int num8 = 1;
            if ((double)Velocity.X < 0.0)
                num8 = 2;
            for (int index3 = num4; index3 <= num5; ++index3)
            {
                for (int index4 = num6; index4 <= num6 + 1; ++index4)
                {
                    if (Main.tile[index3, index4] == null)
                        Main.tile[index3, index4] = new Tile();
                    if (Main.tile[index3, index4].nactive() && (Main.tileSolid[(int)Main.tile[index3, index4].type] || Main.tileSolidTop[(int)Main.tile[index3, index4].type]))
                    {
                        int num9 = index4 * 16;
                        if (Main.tile[index3, index4].halfBrick())
                            num9 += 8;
                        if (new Rectangle(index3 * 16, index4 * 16 - 17, 16, 16).Intersects(new Rectangle((int)Position.X, (int)Position.Y, Width, Height)) && (double)num9 <= (double)num7)
                        {
                            if ((double)num7 == (double)num9)
                            {
                                if ((int)Main.tile[index3, index4].slope() != 0)
                                {
                                    if (index1 != -1 && index2 != -1 && (Main.tile[index1, index2] != null && (int)Main.tile[index1, index2].slope() != 0))
                                    {
                                        if ((int)Main.tile[index3, index4].slope() == num8)
                                        {
                                            num7 = (float)num9;
                                            index1 = index3;
                                            index2 = index4;
                                        }
                                    }
                                    else
                                    {
                                        num7 = (float)num9;
                                        index1 = index3;
                                        index2 = index4;
                                    }
                                }
                            }
                            else
                            {
                                num7 = (float)num9;
                                index1 = index3;
                                index2 = index4;
                            }
                        }
                    }
                }
            }
            int index5 = index1;
            int index6 = index2;
            if (index1 != -1 && index2 != -1 && (Main.tile[index5, index6] != null && (int)Main.tile[index5, index6].slope() > 0))
            {
                int num9 = (int)Main.tile[index5, index6].slope();
                Vector2 vector2_2;
                vector2_2.X = (float)(index5 * 16);
                vector2_2.Y = (float)(index6 * 16);
                if (num9 == 2)
                {
                    float num10 = (float)((double)vector2_2.X + 16.0 - ((double)Position.X + (double)Width));
                    if ((double)Position.Y + (double)Height >= (double)vector2_2.Y + (double)num10 && (double)Velocity.X < 0.0)
                        Velocity.Y += Math.Abs(Velocity.X);
                }
                else if (num9 == 1)
                {
                    float num10 = Position.X - vector2_2.X;
                    if ((double)Position.Y + (double)Height >= (double)vector2_2.Y + (double)num10 && (double)Velocity.X > 0.0)
                        Velocity.Y += Math.Abs(Velocity.X);
                }
            }
            return new Vector4(Position, Velocity.X, Velocity.Y);
        }

        public static Vector4 SlopeCollision_Yor(Vector2 Position, Vector2 Velocity, int Width, int Height, float gravity = 0.0f, bool fall = false)
        {
            Collision.stair = false;
            Collision.stairFall = false;
            bool[] flagArray = new bool[5];
            float num1 = Position.Y;
            float num2 = Position.Y;
            Collision.sloping = false;
            Vector2 vector2_1 = Position;
            Vector2 vector2_2 = Position;
            Vector2 vector2_3 = Velocity;
            int num3 = (int)((double)Position.X / 16.0) - 1;
            int num4 = (int)(((double)Position.X + (double)Width) / 16.0) + 2;
            int num5 = (int)((double)Position.Y / 16.0) - 1;
            int num6 = (int)(((double)Position.Y + (double)Height) / 16.0) + 2;
            int num7 = Utils.Clamp<int>(num3, 0, Main.maxTilesX - 1);
            int num8 = Utils.Clamp<int>(num4, 0, Main.maxTilesX - 1);
            int num9 = Utils.Clamp<int>(num5, 0, Main.maxTilesY - 1);
            int num10 = Utils.Clamp<int>(num6, 0, Main.maxTilesY - 1);
            int num11 = 1;
            int num12 = 1;
            int num13 = num8;
            int num14 = num10;
            if ((double)Velocity.X < 0.0)
            {
                num11 = -1;
                num13 = num7;
            }
            if ((double)Velocity.Y < 0.0)
            {
                num12 = -1;
                num14 = num9;
            }
            int num15 = (int)((double)Position.X + (double)(Width / 2)) / 16;
            int num16 = (int)((double)Position.Y + (double)(Height / 2)) / 16;
            int index1 = num15;
            while (index1 != num13)
            {
                int index2 = num16;
                while (index2 != num14)
                {
                    if (Main.tile[index1, index2] != null && Main.tile[index1, index2].active() && !Main.tile[index1, index2].inActive() && (Main.tileSolid[(int)Main.tile[index1, index2].type] || Main.tileSolidTop[(int)Main.tile[index1, index2].type] && (int)Main.tile[index1, index2].frameY == 0))
                    {
                        Vector2 vector2_4;
                        vector2_4.X = (float)(index1 * 16);
                        vector2_4.Y = (float)(index2 * 16);
                        int num17 = 16;
                        if (Main.tile[index1, index2].halfBrick())
                        {
                            vector2_4.Y += 8f;
                            num17 -= 8;
                        }
                        if ((double)Position.X + (double)Width > (double)vector2_4.X && (double)Position.X < (double)vector2_4.X + 16.0 && ((double)Position.Y + (double)Height > (double)vector2_4.Y && (double)Position.Y < (double)vector2_4.Y + (double)num17))
                        {
                            bool flag1 = true;
                            if ((int)Main.tile[index1, index2].slope() > 0)
                            {
                                if ((int)Main.tile[index1, index2].slope() > 2)
                                {
                                    if ((int)Main.tile[index1, index2].slope() == 3 && (double)vector2_1.Y + (double)Math.Abs(Velocity.X) + 1.0 >= (double)vector2_4.Y && (double)vector2_1.X >= (double)vector2_4.X)
                                        flag1 = true;
                                    if ((int)Main.tile[index1, index2].slope() == 4 && (double)vector2_1.Y + (double)Math.Abs(Velocity.X) + 1.0 >= (double)vector2_4.Y && (double)vector2_1.X + (double)Width <= (double)vector2_4.X + 16.0)
                                        flag1 = true;
                                }
                                else
                                {
                                    if ((int)Main.tile[index1, index2].slope() == 1 && (double)vector2_1.Y + (double)Height - (double)Math.Abs(Velocity.X) - 1.0 <= (double)vector2_4.Y + (double)num17 && (double)vector2_1.X >= (double)vector2_4.X)
                                        flag1 = true;
                                    if ((int)Main.tile[index1, index2].slope() == 2 && (double)vector2_1.Y + (double)Height - (double)Math.Abs(Velocity.X) - 1.0 <= (double)vector2_4.Y + (double)num17 && (double)vector2_1.X + (double)Width <= (double)vector2_4.X + 16.0)
                                        flag1 = true;
                                }
                            }
                            if ((int)Main.tile[index1, index2].type == 19)
                            {
                                if ((double)Velocity.Y < 0.0)
                                    flag1 = false;
                                if ((double)Position.Y + (double)Height < (double)(index2 * 16) || (double)Position.Y + (double)Height - (1.0 + (double)Math.Abs(Velocity.X)) > (double)(index2 * 16 + 16))
                                    flag1 = false;
                            }
                            if (flag1)
                            {
                                bool flag2 = false;
                                if (fall && (int)Main.tile[index1, index2].type == 19)
                                    flag2 = true;
                                int index3 = (int)Main.tile[index1, index2].slope();
                                vector2_4.X = (float)(index1 * 16);
                                vector2_4.Y = (float)(index2 * 16);
                                if ((double)Position.X + (double)Width > (double)vector2_4.X && (double)Position.X < (double)vector2_4.X + 16.0 && ((double)Position.Y + (double)Height > (double)vector2_4.Y && (double)Position.Y < (double)vector2_4.Y + 16.0))
                                {
                                    float num18 = 0.0f;
                                    if (index3 == 3 || index3 == 4)
                                    {
                                        if (index3 == 3)
                                            num18 = Position.X - vector2_4.X;
                                        if (index3 == 4)
                                            num18 = (float)((double)vector2_4.X + 16.0 - ((double)Position.X + (double)Width));
                                        if ((double)num18 >= 0.0)
                                        {
                                            if ((double)Position.Y <= (double)vector2_4.Y + 16.0 - (double)num18)
                                            {
                                                float num19 = vector2_4.Y + 16f - vector2_1.Y - num18;
                                                if ((double)Position.Y + (double)num19 > (double)num2)
                                                {
                                                    vector2_2.Y += num19;
                                                    num2 = vector2_2.Y;
                                                    if ((double)vector2_3.Y < 0.0100999996066093)
                                                        vector2_3.Y = 0.0101f;
                                                    flagArray[index3] = true;
                                                }
                                            }
                                        }
                                        else if ((double)Position.Y > (double)vector2_4.Y)
                                        {
                                            float num19 = vector2_4.Y + 16f;
                                            if ((double)vector2_2.Y < (double)num19)
                                            {
                                                vector2_2.Y = num19;
                                                if ((double)vector2_3.Y < 0.0100999996066093)
                                                    vector2_3.Y = 0.0101f;
                                            }
                                        }
                                    }
                                    if (index3 == 1 || index3 == 2)
                                    {
                                        if (index3 == 1)
                                            num18 = Position.X - vector2_4.X;
                                        if (index3 == 2)
                                            num18 = (float)((double)vector2_4.X + 16.0 - ((double)Position.X + (double)Width));
                                        if ((double)num18 >= 0.0)
                                        {
                                            if ((double)Position.Y + (double)Height >= (double)vector2_4.Y + (double)num18)
                                            {
                                                float num19 = vector2_4.Y - (vector2_1.Y + (float)Height) + num18;
                                                if ((double)Position.Y + (double)num19 < (double)num1)
                                                {
                                                    if (flag2)
                                                    {
                                                        Collision.stairFall = true;
                                                    }
                                                    else
                                                    {
                                                        Collision.stair = (int)Main.tile[index1, index2].type == 19;
                                                        vector2_2.Y += num19;
                                                        num1 = vector2_2.Y;
                                                        if ((double)vector2_3.Y > 0.0)
                                                            vector2_3.Y = 0.0f;
                                                        flagArray[index3] = true;
                                                    }
                                                }
                                            }
                                        }
                                        else if ((int)Main.tile[index1, index2].type == 19 && (double)Position.Y + (double)Height - 4.0 - (double)Math.Abs(Velocity.X) > (double)vector2_4.Y)
                                        {
                                            if (flag2)
                                                Collision.stairFall = true;
                                        }
                                        else
                                        {
                                            float num19 = vector2_4.Y - (float)Height;
                                            if ((double)vector2_2.Y > (double)num19)
                                            {
                                                if (flag2)
                                                {
                                                    Collision.stairFall = true;
                                                }
                                                else
                                                {
                                                    Collision.stair = (int)Main.tile[index1, index2].type == 19;
                                                    vector2_2.Y = num19;
                                                    if ((double)vector2_3.Y > 0.0)
                                                        vector2_3.Y = 0.0f;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    index2 += num12;
                }
                index1 += num11;
            }
            Vector2 Position1 = Position;
            Vector2 Velocity1 = vector2_2 - Position;
            Vector2 vector2_5 = Collision.TileCollision(Position1, Velocity1, Width, Height, false, false, 1);
            if ((double)vector2_5.Y > (double)Velocity1.Y)
            {
                float num17 = Velocity1.Y - vector2_5.Y;
                vector2_2.Y = Position.Y + vector2_5.Y;
                if (flagArray[1])
                    vector2_2.X = Position.X - num17;
                if (flagArray[2])
                    vector2_2.X = Position.X + num17;
                vector2_3.X = 0.0f;
                vector2_3.Y = 0.0f;
                Collision.up = false;
            }
            else if ((double)vector2_5.Y < (double)Velocity1.Y)
            {
                float num17 = vector2_5.Y - Velocity1.Y;
                vector2_2.Y = Position.Y + vector2_5.Y;
                if (flagArray[3])
                    vector2_2.X = Position.X - num17;
                if (flagArray[4])
                    vector2_2.X = Position.X + num17;
                vector2_3.X = 0.0f;
                vector2_3.Y = 0.0f;
            }
            return new Vector4(vector2_2, vector2_3.X, vector2_3.Y);
        }

        public static Vector4 SlopeCollision(Vector2 Position, Vector2 Velocity, int Width, int Height, float gravity = 0.0f, bool fall = false)
        {
            Collision.stair = false;
            Collision.stairFall = false;
            bool[] flagArray = new bool[5];
            float num1 = Position.Y;
            float num2 = Position.Y;
            Collision.sloping = false;
            Vector2 vector2_1 = Position;
            Vector2 vector2_2 = Position;
            Vector2 vector2_3 = Velocity;
            int num3 = (int)((double)Position.X / 16.0) - 1;
            int num4 = (int)(((double)Position.X + (double)Width) / 16.0) + 2;
            int num5 = (int)((double)Position.Y / 16.0) - 1;
            int num6 = (int)(((double)Position.Y + (double)Height) / 16.0) + 2;
            int num7 = Utils.Clamp<int>(num3, 0, Main.maxTilesX - 1);
            int num8 = Utils.Clamp<int>(num4, 0, Main.maxTilesX - 1);
            int num9 = Utils.Clamp<int>(num5, 0, Main.maxTilesY - 1);
            int num10 = Utils.Clamp<int>(num6, 0, Main.maxTilesY - 1);
            for (int index1 = num7; index1 < num8; ++index1)
            {
                for (int index2 = num9; index2 < num10; ++index2)
                {
                    if (Main.tile[index1, index2] != null && Main.tile[index1, index2].active() && !Main.tile[index1, index2].inActive() && (Main.tileSolid[(int)Main.tile[index1, index2].type] || Main.tileSolidTop[(int)Main.tile[index1, index2].type] && (int)Main.tile[index1, index2].frameY == 0))
                    {
                        Vector2 vector2_4;
                        vector2_4.X = (float)(index1 * 16);
                        vector2_4.Y = (float)(index2 * 16);
                        int num11 = 16;
                        if (Main.tile[index1, index2].halfBrick())
                        {
                            vector2_4.Y += 8f;
                            num11 -= 8;
                        }
                        if ((double)Position.X + (double)Width > (double)vector2_4.X && (double)Position.X < (double)vector2_4.X + 16.0 && ((double)Position.Y + (double)Height > (double)vector2_4.Y && (double)Position.Y < (double)vector2_4.Y + (double)num11))
                        {
                            bool flag1 = true;
                            if ((int)Main.tile[index1, index2].slope() > 0)
                            {
                                if ((int)Main.tile[index1, index2].slope() > 2)
                                {
                                    if ((int)Main.tile[index1, index2].slope() == 3 && (double)vector2_1.Y + (double)Math.Abs(Velocity.X) + 1.0 >= (double)vector2_4.Y && (double)vector2_1.X >= (double)vector2_4.X)
                                        flag1 = true;
                                    if ((int)Main.tile[index1, index2].slope() == 4 && (double)vector2_1.Y + (double)Math.Abs(Velocity.X) + 1.0 >= (double)vector2_4.Y && (double)vector2_1.X + (double)Width <= (double)vector2_4.X + 16.0)
                                        flag1 = true;
                                }
                                else
                                {
                                    if ((int)Main.tile[index1, index2].slope() == 1 && (double)vector2_1.Y + (double)Height - (double)Math.Abs(Velocity.X) - 1.0 <= (double)vector2_4.Y + (double)num11 && (double)vector2_1.X >= (double)vector2_4.X)
                                        flag1 = true;
                                    if ((int)Main.tile[index1, index2].slope() == 2 && (double)vector2_1.Y + (double)Height - (double)Math.Abs(Velocity.X) - 1.0 <= (double)vector2_4.Y + (double)num11 && (double)vector2_1.X + (double)Width <= (double)vector2_4.X + 16.0)
                                        flag1 = true;
                                }
                            }
                            if ((int)Main.tile[index1, index2].type == 19)
                            {
                                if ((double)Velocity.Y < 0.0)
                                    flag1 = false;
                                if ((double)Position.Y + (double)Height < (double)(index2 * 16) || (double)Position.Y + (double)Height - (1.0 + (double)Math.Abs(Velocity.X)) > (double)(index2 * 16 + 16))
                                    flag1 = false;
                            }
                            if (flag1)
                            {
                                bool flag2 = false;
                                if (fall && (int)Main.tile[index1, index2].type == 19)
                                    flag2 = true;
                                int index3 = (int)Main.tile[index1, index2].slope();
                                vector2_4.X = (float)(index1 * 16);
                                vector2_4.Y = (float)(index2 * 16);
                                if ((double)Position.X + (double)Width > (double)vector2_4.X && (double)Position.X < (double)vector2_4.X + 16.0 && ((double)Position.Y + (double)Height > (double)vector2_4.Y && (double)Position.Y < (double)vector2_4.Y + 16.0))
                                {
                                    float num12 = 0.0f;
                                    if (index3 == 3 || index3 == 4)
                                    {
                                        if (index3 == 3)
                                            num12 = Position.X - vector2_4.X;
                                        if (index3 == 4)
                                            num12 = (float)((double)vector2_4.X + 16.0 - ((double)Position.X + (double)Width));
                                        if ((double)num12 >= 0.0)
                                        {
                                            if ((double)Position.Y <= (double)vector2_4.Y + 16.0 - (double)num12)
                                            {
                                                float num13 = vector2_4.Y + 16f - vector2_1.Y - num12;
                                                if ((double)Position.Y + (double)num13 > (double)num2)
                                                {
                                                    vector2_2.Y = Position.Y + num13;
                                                    num2 = vector2_2.Y;
                                                    if ((double)vector2_3.Y < 0.0100999996066093)
                                                        vector2_3.Y = 0.0101f;
                                                    flagArray[index3] = true;
                                                }
                                            }
                                        }
                                        else if ((double)Position.Y > (double)vector2_4.Y)
                                        {
                                            float num13 = vector2_4.Y + 16f;
                                            if ((double)vector2_2.Y < (double)num13)
                                            {
                                                vector2_2.Y = num13;
                                                if ((double)vector2_3.Y < 0.0100999996066093)
                                                    vector2_3.Y = 0.0101f;
                                            }
                                        }
                                    }
                                    if (index3 == 1 || index3 == 2)
                                    {
                                        if (index3 == 1)
                                            num12 = Position.X - vector2_4.X;
                                        if (index3 == 2)
                                            num12 = (float)((double)vector2_4.X + 16.0 - ((double)Position.X + (double)Width));
                                        if ((double)num12 >= 0.0)
                                        {
                                            if ((double)Position.Y + (double)Height >= (double)vector2_4.Y + (double)num12)
                                            {
                                                float num13 = vector2_4.Y - (vector2_1.Y + (float)Height) + num12;
                                                if ((double)Position.Y + (double)num13 < (double)num1)
                                                {
                                                    if (flag2)
                                                    {
                                                        Collision.stairFall = true;
                                                    }
                                                    else
                                                    {
                                                        Collision.stair = (int)Main.tile[index1, index2].type == 19;
                                                        vector2_2.Y = Position.Y + num13;
                                                        num1 = vector2_2.Y;
                                                        if ((double)vector2_3.Y > 0.0)
                                                            vector2_3.Y = 0.0f;
                                                        flagArray[index3] = true;
                                                    }
                                                }
                                            }
                                        }
                                        else if ((int)Main.tile[index1, index2].type == 19 && (double)Position.Y + (double)Height - 4.0 - (double)Math.Abs(Velocity.X) > (double)vector2_4.Y)
                                        {
                                            if (flag2)
                                                Collision.stairFall = true;
                                        }
                                        else
                                        {
                                            float num13 = vector2_4.Y - (float)Height;
                                            if ((double)vector2_2.Y > (double)num13)
                                            {
                                                if (flag2)
                                                {
                                                    Collision.stairFall = true;
                                                }
                                                else
                                                {
                                                    Collision.stair = (int)Main.tile[index1, index2].type == 19;
                                                    vector2_2.Y = num13;
                                                    if ((double)vector2_3.Y > 0.0)
                                                        vector2_3.Y = 0.0f;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Vector2 Position1 = Position;
            Vector2 Velocity1 = vector2_2 - Position;
            Vector2 vector2_5 = Collision.TileCollision(Position1, Velocity1, Width, Height, false, false, 1);
            if ((double)vector2_5.Y > (double)Velocity1.Y)
            {
                float num11 = Velocity1.Y - vector2_5.Y;
                vector2_2.Y = Position.Y + vector2_5.Y;
                if (flagArray[1])
                    vector2_2.X = Position.X - num11;
                if (flagArray[2])
                    vector2_2.X = Position.X + num11;
                vector2_3.X = 0.0f;
                vector2_3.Y = 0.0f;
                Collision.up = false;
            }
            else if ((double)vector2_5.Y < (double)Velocity1.Y)
            {
                float num11 = vector2_5.Y - Velocity1.Y;
                vector2_2.Y = Position.Y + vector2_5.Y;
                if (flagArray[3])
                    vector2_2.X = Position.X - num11;
                if (flagArray[4])
                    vector2_2.X = Position.X + num11;
                vector2_3.X = 0.0f;
                vector2_3.Y = 0.0f;
            }
            return new Vector4(vector2_2, vector2_3.X, vector2_3.Y);
        }

        public static Vector2 noSlopeCollision(Vector2 Position, Vector2 Velocity, int Width, int Height, bool fallThrough = false, bool fall2 = false)
        {
            Collision.up = false;
            Collision.down = false;
            Vector2 vector2_1 = Velocity;
            Vector2 vector2_2 = Velocity;
            Vector2 vector2_3 = Position + Velocity;
            Vector2 vector2_4 = Position;
            int num1 = (int)((double)Position.X / 16.0) - 1;
            int num2 = (int)(((double)Position.X + (double)Width) / 16.0) + 2;
            int num3 = (int)((double)Position.Y / 16.0) - 1;
            int num4 = (int)(((double)Position.Y + (double)Height) / 16.0) + 2;
            int num5 = -1;
            int num6 = -1;
            int num7 = -1;
            int num8 = -1;
            int num9 = Utils.Clamp<int>(num1, 0, Main.maxTilesX - 1);
            int num10 = Utils.Clamp<int>(num2, 0, Main.maxTilesX - 1);
            int num11 = Utils.Clamp<int>(num3, 0, Main.maxTilesY - 1);
            int num12 = Utils.Clamp<int>(num4, 0, Main.maxTilesY - 1);
            float num13 = (float)((num12 + 3) * 16);
            for (int index1 = num9; index1 < num10; ++index1)
            {
                for (int index2 = num11; index2 < num12; ++index2)
                {
                    if (Main.tile[index1, index2] != null && Main.tile[index1, index2].active() && (Main.tileSolid[(int)Main.tile[index1, index2].type] || Main.tileSolidTop[(int)Main.tile[index1, index2].type] && (int)Main.tile[index1, index2].frameY == 0))
                    {
                        Vector2 vector2_5;
                        vector2_5.X = (float)(index1 * 16);
                        vector2_5.Y = (float)(index2 * 16);
                        int num14 = 16;
                        if (Main.tile[index1, index2].halfBrick())
                        {
                            vector2_5.Y += 8f;
                            num14 -= 8;
                        }
                        if ((double)vector2_3.X + (double)Width > (double)vector2_5.X && (double)vector2_3.X < (double)vector2_5.X + 16.0 && ((double)vector2_3.Y + (double)Height > (double)vector2_5.Y && (double)vector2_3.Y < (double)vector2_5.Y + (double)num14))
                        {
                            if ((double)vector2_4.Y + (double)Height <= (double)vector2_5.Y)
                            {
                                Collision.down = true;
                                if ((!Main.tileSolidTop[(int)Main.tile[index1, index2].type] || !fallThrough || (double)Velocity.Y > 1.0 && !fall2) && (double)num13 > (double)vector2_5.Y)
                                {
                                    num7 = index1;
                                    num8 = index2;
                                    if (num14 < 16)
                                        ++num8;
                                    if (num7 != num5)
                                    {
                                        vector2_1.Y = vector2_5.Y - (vector2_4.Y + (float)Height);
                                        num13 = vector2_5.Y;
                                    }
                                }
                            }
                            else if ((double)vector2_4.X + (double)Width <= (double)vector2_5.X && !Main.tileSolidTop[(int)Main.tile[index1, index2].type])
                            {
                                num5 = index1;
                                num6 = index2;
                                if (num6 != num8)
                                    vector2_1.X = vector2_5.X - (vector2_4.X + (float)Width);
                                if (num7 == num5)
                                    vector2_1.Y = vector2_2.Y;
                            }
                            else if ((double)vector2_4.X >= (double)vector2_5.X + 16.0 && !Main.tileSolidTop[(int)Main.tile[index1, index2].type])
                            {
                                num5 = index1;
                                num6 = index2;
                                if (num6 != num8)
                                    vector2_1.X = vector2_5.X + 16f - vector2_4.X;
                                if (num7 == num5)
                                    vector2_1.Y = vector2_2.Y;
                            }
                            else if ((double)vector2_4.Y >= (double)vector2_5.Y + (double)num14 && !Main.tileSolidTop[(int)Main.tile[index1, index2].type])
                            {
                                Collision.up = true;
                                num7 = index1;
                                num8 = index2;
                                vector2_1.Y = (float)((double)vector2_5.Y + (double)num14 - (double)vector2_4.Y + 0.00999999977648258);
                                if (num8 == num6)
                                    vector2_1.X = vector2_2.X;
                            }
                        }
                    }
                }
            }
            return vector2_1;
        }

        public static Vector2 TileCollision(Vector2 Position, Vector2 Velocity, int Width, int Height, bool fallThrough = false, bool fall2 = false, int gravDir = 1)
        {
            Collision.up = false;
            Collision.down = false;
            Vector2 vector2_1 = Velocity;
            Vector2 vector2_2 = Velocity;
            Vector2 vector2_3 = Position + Velocity;
            Vector2 vector2_4 = Position;
            int num1 = (int)((double)Position.X / 16.0) - 1;
            int num2 = (int)(((double)Position.X + (double)Width) / 16.0) + 2;
            int num3 = (int)((double)Position.Y / 16.0) - 1;
            int num4 = (int)(((double)Position.Y + (double)Height) / 16.0) + 2;
            int num5 = -1;
            int num6 = -1;
            int num7 = -1;
            int num8 = -1;
            int num9 = Utils.Clamp<int>(num1, 0, Main.maxTilesX - 1);
            int num10 = Utils.Clamp<int>(num2, 0, Main.maxTilesX - 1);
            int num11 = Utils.Clamp<int>(num3, 0, Main.maxTilesY - 1);
            int num12 = Utils.Clamp<int>(num4, 0, Main.maxTilesY - 1);
            float num13 = (float)((num12 + 3) * 16);
            for (int index1 = num9; index1 < num10; ++index1)
            {
                for (int index2 = num11; index2 < num12; ++index2)
                {
                    if (Main.tile[index1, index2] != null && Main.tile[index1, index2].active() && !Main.tile[index1, index2].inActive() && (Main.tileSolid[(int)Main.tile[index1, index2].type] || Main.tileSolidTop[(int)Main.tile[index1, index2].type] && (int)Main.tile[index1, index2].frameY == 0))
                    {
                        Vector2 vector2_5;
                        vector2_5.X = (float)(index1 * 16);
                        vector2_5.Y = (float)(index2 * 16);
                        int num14 = 16;
                        if (Main.tile[index1, index2].halfBrick())
                        {
                            vector2_5.Y += 8f;
                            num14 -= 8;
                        }
                        if ((double)vector2_3.X + (double)Width > (double)vector2_5.X && (double)vector2_3.X < (double)vector2_5.X + 16.0 && ((double)vector2_3.Y + (double)Height > (double)vector2_5.Y && (double)vector2_3.Y < (double)vector2_5.Y + (double)num14))
                        {
                            bool flag1 = false;
                            bool flag2 = false;
                            if ((int)Main.tile[index1, index2].slope() > 2)
                            {
                                if ((int)Main.tile[index1, index2].slope() == 3 && (double)vector2_4.Y + (double)Math.Abs(Velocity.X) >= (double)vector2_5.Y && (double)vector2_4.X >= (double)vector2_5.X)
                                    flag2 = true;
                                if ((int)Main.tile[index1, index2].slope() == 4 && (double)vector2_4.Y + (double)Math.Abs(Velocity.X) >= (double)vector2_5.Y && (double)vector2_4.X + (double)Width <= (double)vector2_5.X + 16.0)
                                    flag2 = true;
                            }
                            else if ((int)Main.tile[index1, index2].slope() > 0)
                            {
                                flag1 = true;
                                if ((int)Main.tile[index1, index2].slope() == 1 && (double)vector2_4.Y + (double)Height - (double)Math.Abs(Velocity.X) <= (double)vector2_5.Y + (double)num14 && (double)vector2_4.X >= (double)vector2_5.X)
                                    flag2 = true;
                                if ((int)Main.tile[index1, index2].slope() == 2 && (double)vector2_4.Y + (double)Height - (double)Math.Abs(Velocity.X) <= (double)vector2_5.Y + (double)num14 && (double)vector2_4.X + (double)Width <= (double)vector2_5.X + 16.0)
                                    flag2 = true;
                            }
                            if (!flag2)
                            {
                                if ((double)vector2_4.Y + (double)Height <= (double)vector2_5.Y)
                                {
                                    Collision.down = true;
                                    if ((!Main.tileSolidTop[(int)Main.tile[index1, index2].type] || !fallThrough || (double)Velocity.Y > 1.0 && !fall2) && (double)num13 > (double)vector2_5.Y)
                                    {
                                        num7 = index1;
                                        num8 = index2;
                                        if (num14 < 16)
                                            ++num8;
                                        if (num7 != num5 && !flag1)
                                        {
                                            vector2_1.Y = (float)((double)vector2_5.Y - ((double)vector2_4.Y + (double)Height) + (gravDir == -1 ? -0.00999999977648258 : 0.0));
                                            num13 = vector2_5.Y;
                                        }
                                    }
                                }
                                else if ((double)vector2_4.X + (double)Width <= (double)vector2_5.X && !Main.tileSolidTop[(int)Main.tile[index1, index2].type])
                                {
                                    if (Main.tile[index1 - 1, index2] == null)
                                        Main.tile[index1 - 1, index2] = new Tile();
                                    if ((int)Main.tile[index1 - 1, index2].slope() != 2 && (int)Main.tile[index1 - 1, index2].slope() != 4)
                                    {
                                        num5 = index1;
                                        num6 = index2;
                                        if (num6 != num8)
                                            vector2_1.X = vector2_5.X - (vector2_4.X + (float)Width);
                                        if (num7 == num5)
                                            vector2_1.Y = vector2_2.Y;
                                    }
                                }
                                else if ((double)vector2_4.X >= (double)vector2_5.X + 16.0 && !Main.tileSolidTop[(int)Main.tile[index1, index2].type])
                                {
                                    if (Main.tile[index1 + 1, index2] == null)
                                        Main.tile[index1 + 1, index2] = new Tile();
                                    if ((int)Main.tile[index1 + 1, index2].slope() != 1 && (int)Main.tile[index1 + 1, index2].slope() != 3)
                                    {
                                        num5 = index1;
                                        num6 = index2;
                                        if (num6 != num8)
                                            vector2_1.X = vector2_5.X + 16f - vector2_4.X;
                                        if (num7 == num5)
                                            vector2_1.Y = vector2_2.Y;
                                    }
                                }
                                else if ((double)vector2_4.Y >= (double)vector2_5.Y + (double)num14 && !Main.tileSolidTop[(int)Main.tile[index1, index2].type])
                                {
                                    Collision.up = true;
                                    num7 = index1;
                                    num8 = index2;
                                    vector2_1.Y = (float)((double)vector2_5.Y + (double)num14 - (double)vector2_4.Y + (gravDir == 1 ? 0.00999999977648258 : 0.0));
                                    if (num8 == num6)
                                        vector2_1.X = vector2_2.X;
                                }
                            }
                        }
                    }
                }
            }
            return vector2_1;
        }

        public static bool IsClearSpotHack(Vector2 position, float testMagnitude, int Width, int Height, bool fallThrough = false, bool fall2 = false, int gravDir = 1, bool checkCardinals = true, bool checkSlopes = false)
        {
            if (checkCardinals)
            {
                Vector2 Velocity1 = Vector2.UnitX * testMagnitude;
                if (Collision.TileCollision(position - Velocity1, Velocity1, Width, Height, fallThrough, fall2, gravDir) != Velocity1)
                    return false;
                Vector2 Velocity2 = -Vector2.UnitX * testMagnitude;
                if (Collision.TileCollision(position - Velocity2, Velocity2, Width, Height, fallThrough, fall2, gravDir) != Velocity2)
                    return false;
                Vector2 Velocity3 = Vector2.UnitY * testMagnitude;
                if (Collision.TileCollision(position - Velocity3, Velocity3, Width, Height, fallThrough, fall2, gravDir) != Velocity3)
                    return false;
                Vector2 Velocity4 = -Vector2.UnitY * testMagnitude;
                if (Collision.TileCollision(position - Velocity4, Velocity4, Width, Height, fallThrough, fall2, gravDir) != Velocity4)
                    return false;
            }
            if (checkSlopes)
            {
                Vector2 Velocity1 = Vector2.UnitX * testMagnitude;
                Vector4 vector4 = new Vector4(position, testMagnitude, 0.0f);
                if (Collision.SlopeCollision(position, Velocity1, Width, Height, (float)gravDir, fallThrough) != vector4)
                    return false;
                Vector2 Velocity2 = -Vector2.UnitX * testMagnitude;
                vector4 = new Vector4(position, -testMagnitude, 0.0f);
                if (Collision.SlopeCollision(position, Velocity2, Width, Height, (float)gravDir, fallThrough) != vector4)
                    return false;
                Vector2 Velocity3 = Vector2.UnitY * testMagnitude;
                vector4 = new Vector4(position, 0.0f, testMagnitude);
                if (Collision.SlopeCollision(position, Velocity3, Width, Height, (float)gravDir, fallThrough) != vector4)
                    return false;
                Vector2 Velocity4 = -Vector2.UnitY * testMagnitude;
                vector4 = new Vector4(position, 0.0f, -testMagnitude);
                if (Collision.SlopeCollision(position, Velocity4, Width, Height, (float)gravDir, fallThrough) != vector4)
                    return false;
            }
            return true;
        }

        public static List<Point> FindCollisionTile(int Direction, Vector2 position, float testMagnitude, int Width, int Height, bool fallThrough = false, bool fall2 = false, int gravDir = 1, bool checkCardinals = true, bool checkSlopes = false)
        {
            List<Point> list = new List<Point>();
            switch (Direction)
            {
                case 0:
                case 1:
                    Vector2 Velocity1 = Direction == 0 ? Vector2.UnitX * testMagnitude : -Vector2.UnitX * testMagnitude;
                    Vector4 vec1 = new Vector4(position, Velocity1.X, Velocity1.Y);
                    int x = (int)((double)position.X + (Direction == 0 ? (double)Width : 0.0)) / 16;
                    float num1 = Math.Min((float)(16.0 - (double)position.Y % 16.0), (float)Height);
                    float num2 = num1;
                    if (checkCardinals && Collision.TileCollision(position - Velocity1, Velocity1, Width, (int)num1, fallThrough, fall2, gravDir) != Velocity1)
                        list.Add(new Point(x, (int)position.Y / 16));
                    else if (checkSlopes && Utils.XZW(Collision.SlopeCollision(position, Velocity1, Width, (int)num1, (float)gravDir, fallThrough)) != Utils.XZW(vec1))
                        list.Add(new Point(x, (int)position.Y / 16));
                    while ((double)num2 + 16.0 <= (double)(Height - 16))
                    {
                        if (checkCardinals && Collision.TileCollision(position - Velocity1 + Vector2.UnitY * num2, Velocity1, Width, 16, fallThrough, fall2, gravDir) != Velocity1)
                            list.Add(new Point(x, (int)((double)position.Y + (double)num2) / 16));
                        else if (checkSlopes && Utils.XZW(Collision.SlopeCollision(position + Vector2.UnitY * num2, Velocity1, Width, 16, (float)gravDir, fallThrough)) != Utils.XZW(vec1))
                            list.Add(new Point(x, (int)((double)position.Y + (double)num2) / 16));
                        num2 += 16f;
                    }
                    int Height1 = Height - (int)num2;
                    if (checkCardinals && Collision.TileCollision(position - Velocity1 + Vector2.UnitY * num2, Velocity1, Width, Height1, fallThrough, fall2, gravDir) != Velocity1)
                    {
                        list.Add(new Point(x, (int)((double)position.Y + (double)num2) / 16));
                        break;
                    }
                    if (checkSlopes && Utils.XZW(Collision.SlopeCollision(position + Vector2.UnitY * num2, Velocity1, Width, Height1, (float)gravDir, fallThrough)) != Utils.XZW(vec1))
                    {
                        list.Add(new Point(x, (int)((double)position.Y + (double)num2) / 16));
                        break;
                    }
                    break;
                case 2:
                case 3:
                    Vector2 Velocity2 = Direction == 2 ? Vector2.UnitY * testMagnitude : -Vector2.UnitY * testMagnitude;
                    Vector4 vec2 = new Vector4(position, Velocity2.X, Velocity2.Y);
                    int y = (int)((double)position.Y + (Direction == 2 ? (double)Height : 0.0)) / 16;
                    float num3 = Math.Min((float)(16.0 - (double)position.X % 16.0), (float)Width);
                    float num4 = num3;
                    if (checkCardinals && Collision.TileCollision(position - Velocity2, Velocity2, (int)num3, Height, fallThrough, fall2, gravDir) != Velocity2)
                        list.Add(new Point((int)position.X / 16, y));
                    else if (checkSlopes && Utils.YZW(Collision.SlopeCollision(position, Velocity2, (int)num3, Height, (float)gravDir, fallThrough)) != Utils.YZW(vec2))
                        list.Add(new Point((int)position.X / 16, y));
                    while ((double)num4 + 16.0 <= (double)(Width - 16))
                    {
                        if (checkCardinals && Collision.TileCollision(position - Velocity2 + Vector2.UnitX * num4, Velocity2, 16, Height, fallThrough, fall2, gravDir) != Velocity2)
                            list.Add(new Point((int)((double)position.X + (double)num4) / 16, y));
                        else if (checkSlopes && Utils.YZW(Collision.SlopeCollision(position + Vector2.UnitX * num4, Velocity2, 16, Height, (float)gravDir, fallThrough)) != Utils.YZW(vec2))
                            list.Add(new Point((int)((double)position.X + (double)num4) / 16, y));
                        num4 += 16f;
                    }
                    int Width1 = Width - (int)num4;
                    if (checkCardinals && Collision.TileCollision(position - Velocity2 + Vector2.UnitX * num4, Velocity2, Width1, Height, fallThrough, fall2, gravDir) != Velocity2)
                    {
                        list.Add(new Point((int)((double)position.X + (double)num4) / 16, y));
                        break;
                    }
                    if (checkSlopes && Utils.YZW(Collision.SlopeCollision(position + Vector2.UnitX * num4, Velocity2, Width1, Height, (float)gravDir, fallThrough)) != Utils.YZW(vec2))
                    {
                        list.Add(new Point((int)((double)position.X + (double)num4) / 16, y));
                        break;
                    }
                    break;
            }
            return list;
        }

        public static bool FindCollisionDirection(out int Direction, Vector2 position, int Width, int Height, bool fallThrough = false, bool fall2 = false, int gravDir = 1)
        {
            Vector2 Velocity1 = Vector2.UnitX * 16f;
            if (Collision.TileCollision(position - Velocity1, Velocity1, Width, Height, fallThrough, fall2, gravDir) != Velocity1)
            {
                Direction = 0;
                return true;
            }
            Vector2 Velocity2 = -Vector2.UnitX * 16f;
            if (Collision.TileCollision(position - Velocity2, Velocity2, Width, Height, fallThrough, fall2, gravDir) != Velocity2)
            {
                Direction = 1;
                return true;
            }
            Vector2 Velocity3 = Vector2.UnitY * 16f;
            if (Collision.TileCollision(position - Velocity3, Velocity3, Width, Height, fallThrough, fall2, gravDir) != Velocity3)
            {
                Direction = 2;
                return true;
            }
            Vector2 Velocity4 = -Vector2.UnitY * 16f;
            if (Collision.TileCollision(position - Velocity4, Velocity4, Width, Height, fallThrough, fall2, gravDir) != Velocity4)
            {
                Direction = 3;
                return true;
            }
            Direction = -1;
            return false;
        }

        public static bool SolidCollision(Vector2 Position, int Width, int Height)
        {
            int num1 = (int)((double)Position.X / 16.0) - 1;
            int num2 = (int)(((double)Position.X + (double)Width) / 16.0) + 2;
            int num3 = (int)((double)Position.Y / 16.0) - 1;
            int num4 = (int)(((double)Position.Y + (double)Height) / 16.0) + 2;
            int num5 = Utils.Clamp<int>(num1, 0, Main.maxTilesX - 1);
            int num6 = Utils.Clamp<int>(num2, 0, Main.maxTilesX - 1);
            int num7 = Utils.Clamp<int>(num3, 0, Main.maxTilesY - 1);
            int num8 = Utils.Clamp<int>(num4, 0, Main.maxTilesY - 1);
            for (int index1 = num5; index1 < num6; ++index1)
            {
                for (int index2 = num7; index2 < num8; ++index2)
                {
                    if (Main.tile[index1, index2] != null && !Main.tile[index1, index2].inActive() && (Main.tile[index1, index2].active() && Main.tileSolid[(int)Main.tile[index1, index2].type]) && !Main.tileSolidTop[(int)Main.tile[index1, index2].type])
                    {
                        Vector2 vector2;
                        vector2.X = (float)(index1 * 16);
                        vector2.Y = (float)(index2 * 16);
                        int num9 = 16;
                        if (Main.tile[index1, index2].halfBrick())
                        {
                            vector2.Y += 8f;
                            num9 -= 8;
                        }
                        if ((double)Position.X + (double)Width > (double)vector2.X && (double)Position.X < (double)vector2.X + 16.0 && ((double)Position.Y + (double)Height > (double)vector2.Y && (double)Position.Y < (double)vector2.Y + (double)num9))
                            return true;
                    }
                }
            }
            return false;
        }

        public static Vector2 WaterCollision(Vector2 Position, Vector2 Velocity, int Width, int Height, bool fallThrough = false, bool fall2 = false, bool lavaWalk = true)
        {
            Vector2 vector2_1 = Velocity;
            Vector2 vector2_2 = Position + Velocity;
            Vector2 vector2_3 = Position;
            int num1 = (int)((double)Position.X / 16.0) - 1;
            int num2 = (int)(((double)Position.X + (double)Width) / 16.0) + 2;
            int num3 = (int)((double)Position.Y / 16.0) - 1;
            int num4 = (int)(((double)Position.Y + (double)Height) / 16.0) + 2;
            int num5 = Utils.Clamp<int>(num1, 0, Main.maxTilesX - 1);
            int num6 = Utils.Clamp<int>(num2, 0, Main.maxTilesX - 1);
            int num7 = Utils.Clamp<int>(num3, 0, Main.maxTilesY - 1);
            int num8 = Utils.Clamp<int>(num4, 0, Main.maxTilesY - 1);
            for (int index1 = num5; index1 < num6; ++index1)
            {
                for (int index2 = num7; index2 < num8; ++index2)
                {
                    if (Main.tile[index1, index2] != null && (int)Main.tile[index1, index2].liquid > 0 && (int)Main.tile[index1, index2 - 1].liquid == 0 && (!Main.tile[index1, index2].lava() || lavaWalk))
                    {
                        int num9 = (int)Main.tile[index1, index2].liquid / 32 * 2 + 2;
                        Vector2 vector2_4;
                        vector2_4.X = (float)(index1 * 16);
                        vector2_4.Y = (float)(index2 * 16 + 16 - num9);
                        if ((double)vector2_2.X + (double)Width > (double)vector2_4.X && (double)vector2_2.X < (double)vector2_4.X + 16.0 && ((double)vector2_2.Y + (double)Height > (double)vector2_4.Y && (double)vector2_2.Y < (double)vector2_4.Y + (double)num9) && ((double)vector2_3.Y + (double)Height <= (double)vector2_4.Y && !fallThrough))
                            vector2_1.Y = vector2_4.Y - (vector2_3.Y + (float)Height);
                    }
                }
            }
            return vector2_1;
        }

        public static Vector2 AnyCollision(Vector2 Position, Vector2 Velocity, int Width, int Height)
        {
            Vector2 vector2_1 = Velocity;
            Vector2 vector2_2 = Velocity;
            Vector2 vector2_3 = Position + Velocity;
            Vector2 vector2_4 = Position;
            int num1 = (int)((double)Position.X / 16.0) - 1;
            int num2 = (int)(((double)Position.X + (double)Width) / 16.0) + 2;
            int num3 = (int)((double)Position.Y / 16.0) - 1;
            int num4 = (int)(((double)Position.Y + (double)Height) / 16.0) + 2;
            int num5 = -1;
            int num6 = -1;
            int num7 = -1;
            int num8 = -1;
            if (num1 < 0)
                num1 = 0;
            if (num2 > Main.maxTilesX)
                num2 = Main.maxTilesX;
            if (num3 < 0)
                num3 = 0;
            if (num4 > Main.maxTilesY)
                num4 = Main.maxTilesY;
            for (int index1 = num1; index1 < num2; ++index1)
            {
                for (int index2 = num3; index2 < num4; ++index2)
                {
                    if (Main.tile[index1, index2] != null && Main.tile[index1, index2].active() && !Main.tile[index1, index2].inActive())
                    {
                        Vector2 vector2_5;
                        vector2_5.X = (float)(index1 * 16);
                        vector2_5.Y = (float)(index2 * 16);
                        int num9 = 16;
                        if (Main.tile[index1, index2].halfBrick())
                        {
                            vector2_5.Y += 8f;
                            num9 -= 8;
                        }
                        if ((double)vector2_3.X + (double)Width > (double)vector2_5.X && (double)vector2_3.X < (double)vector2_5.X + 16.0 && ((double)vector2_3.Y + (double)Height > (double)vector2_5.Y && (double)vector2_3.Y < (double)vector2_5.Y + (double)num9))
                        {
                            if ((double)vector2_4.Y + (double)Height <= (double)vector2_5.Y)
                            {
                                num7 = index1;
                                num8 = index2;
                                if (num7 != num5)
                                    vector2_1.Y = vector2_5.Y - (vector2_4.Y + (float)Height);
                            }
                            else if ((double)vector2_4.X + (double)Width <= (double)vector2_5.X && !Main.tileSolidTop[(int)Main.tile[index1, index2].type])
                            {
                                num5 = index1;
                                num6 = index2;
                                if (num6 != num8)
                                    vector2_1.X = vector2_5.X - (vector2_4.X + (float)Width);
                                if (num7 == num5)
                                    vector2_1.Y = vector2_2.Y;
                            }
                            else if ((double)vector2_4.X >= (double)vector2_5.X + 16.0 && !Main.tileSolidTop[(int)Main.tile[index1, index2].type])
                            {
                                num5 = index1;
                                num6 = index2;
                                if (num6 != num8)
                                    vector2_1.X = vector2_5.X + 16f - vector2_4.X;
                                if (num7 == num5)
                                    vector2_1.Y = vector2_2.Y;
                            }
                            else if ((double)vector2_4.Y >= (double)vector2_5.Y + (double)num9 && !Main.tileSolidTop[(int)Main.tile[index1, index2].type])
                            {
                                num7 = index1;
                                num8 = index2;
                                vector2_1.Y = (float)((double)vector2_5.Y + (double)num9 - (double)vector2_4.Y + 0.00999999977648258);
                                if (num8 == num6)
                                    vector2_1.X = vector2_2.X + 0.01f;
                            }
                        }
                    }
                }
            }
            return vector2_1;
        }

        public static void HitTiles(Vector2 Position, Vector2 Velocity, int Width, int Height)
        {
            Vector2 vector2_1 = Position + Velocity;
            int num1 = (int)((double)Position.X / 16.0) - 1;
            int num2 = (int)(((double)Position.X + (double)Width) / 16.0) + 2;
            int num3 = (int)((double)Position.Y / 16.0) - 1;
            int num4 = (int)(((double)Position.Y + (double)Height) / 16.0) + 2;
            if (num1 < 0)
                num1 = 0;
            if (num2 > Main.maxTilesX)
                num2 = Main.maxTilesX;
            if (num3 < 0)
                num3 = 0;
            if (num4 > Main.maxTilesY)
                num4 = Main.maxTilesY;
            for (int i = num1; i < num2; ++i)
            {
                for (int j = num3; j < num4; ++j)
                {
                    if (Main.tile[i, j] != null && !Main.tile[i, j].inActive() && Main.tile[i, j].active() && (Main.tileSolid[(int)Main.tile[i, j].type] || Main.tileSolidTop[(int)Main.tile[i, j].type] && (int)Main.tile[i, j].frameY == 0))
                    {
                        Vector2 vector2_2;
                        vector2_2.X = (float)(i * 16);
                        vector2_2.Y = (float)(j * 16);
                        int num5 = 16;
                        if (Main.tile[i, j].halfBrick())
                        {
                            vector2_2.Y += 8f;
                            num5 -= 8;
                        }
                        if ((double)vector2_1.X + (double)Width >= (double)vector2_2.X && (double)vector2_1.X <= (double)vector2_2.X + 16.0 && ((double)vector2_1.Y + (double)Height >= (double)vector2_2.Y && (double)vector2_1.Y <= (double)vector2_2.Y + (double)num5))
                            WorldGen.KillTile(i, j, true, true, false);
                    }
                }
            }
        }

        public static Vector2 HurtTiles(Vector2 Position, Vector2 Velocity, int Width, int Height, bool fireImmune = false)
        {
            Vector2 vector2_1 = Position;
            int num1 = (int)((double)Position.X / 16.0) - 1;
            int num2 = (int)(((double)Position.X + (double)Width) / 16.0) + 2;
            int num3 = (int)((double)Position.Y / 16.0) - 1;
            int num4 = (int)(((double)Position.Y + (double)Height) / 16.0) + 2;
            if (num1 < 0)
                num1 = 0;
            if (num2 > Main.maxTilesX)
                num2 = Main.maxTilesX;
            if (num3 < 0)
                num3 = 0;
            if (num4 > Main.maxTilesY)
                num4 = Main.maxTilesY;
            for (int i = num1; i < num2; ++i)
            {
                for (int j = num3; j < num4; ++j)
                {
                    if (Main.tile[i, j] != null && (int)Main.tile[i, j].slope() == 0 && (!Main.tile[i, j].inActive() && Main.tile[i, j].active()) && ((int)Main.tile[i, j].type == 32 || (int)Main.tile[i, j].type == 37 || ((int)Main.tile[i, j].type == 48 || (int)Main.tile[i, j].type == 232) || ((int)Main.tile[i, j].type == 53 || (int)Main.tile[i, j].type == 57 || ((int)Main.tile[i, j].type == 58 || (int)Main.tile[i, j].type == 69)) || ((int)Main.tile[i, j].type == 76 || (int)Main.tile[i, j].type == 112 || ((int)Main.tile[i, j].type == 116 || (int)Main.tile[i, j].type == 123) || ((int)Main.tile[i, j].type == 224 || (int)Main.tile[i, j].type == 234 || (int)Main.tile[i, j].type == 352))))
                    {
                        Vector2 vector2_2;
                        vector2_2.X = (float)(i * 16);
                        vector2_2.Y = (float)(j * 16);
                        int num5 = 0;
                        int num6 = (int)Main.tile[i, j].type;
                        int num7 = 16;
                        if (Main.tile[i, j].halfBrick())
                        {
                            vector2_2.Y += 8f;
                            num7 -= 8;
                        }
                        if (num6 == 32 || num6 == 69 || (num6 == 80 || num6 == 352) || num6 == 80 && Main.expertMode)
                        {
                            if ((double)vector2_1.X + (double)Width > (double)vector2_2.X && (double)vector2_1.X < (double)vector2_2.X + 16.0 && ((double)vector2_1.Y + (double)Height > (double)vector2_2.Y && (double)vector2_1.Y < (double)vector2_2.Y + (double)num7 + 11.0 / 1000.0))
                            {
                                int num8 = 1;
                                if ((double)vector2_1.X + (double)(Width / 2) < (double)vector2_2.X + 8.0)
                                    num8 = -1;
                                int num9 = 10;
                                if (num6 == 69)
                                    num9 = 17;
                                else if (num6 == 80)
                                    num9 = 6;
                                if (num6 == 32 || num6 == 69 || num6 == 352)
                                {
                                    WorldGen.KillTile(i, j, false, false, false);
                                    if (Main.netMode == 1 && !Main.tile[i, j].active() && Main.netMode == 1)
                                        NetMessage.SendData(17, -1, -1, "", 4, (float)i, (float)j, 0.0f, 0, 0, 0);
                                }
                                return new Vector2((float)num8, (float)num9);
                            }
                        }
                        else if (num6 == 53 || num6 == 112 || (num6 == 116 || num6 == 123) || (num6 == 224 || num6 == 234))
                        {
                            if ((double)vector2_1.X + (double)Width - 2.0 >= (double)vector2_2.X && (double)vector2_1.X + 2.0 <= (double)vector2_2.X + 16.0 && ((double)vector2_1.Y + (double)Height - 2.0 >= (double)vector2_2.Y && (double)vector2_1.Y + 2.0 <= (double)vector2_2.Y + (double)num7))
                            {
                                int num8 = 1;
                                if ((double)vector2_1.X + (double)(Width / 2) < (double)vector2_2.X + 8.0)
                                    num8 = -1;
                                int num9 = 15;
                                return new Vector2((float)num8, (float)num9);
                            }
                        }
                        else if ((double)vector2_1.X + (double)Width >= (double)vector2_2.X && (double)vector2_1.X <= (double)vector2_2.X + 16.0 && ((double)vector2_1.Y + (double)Height >= (double)vector2_2.Y && (double)vector2_1.Y <= (double)vector2_2.Y + (double)num7 + 11.0 / 1000.0))
                        {
                            int num8 = 1;
                            if ((double)vector2_1.X + (double)(Width / 2) < (double)vector2_2.X + 8.0)
                                num8 = -1;
                            if (!fireImmune && (num6 == 37 || num6 == 58 || num6 == 76))
                                num5 = 20;
                            if (num6 == 48)
                                num5 = 40;
                            if (num6 == 232)
                                num5 = 60;
                            return new Vector2((float)num8, (float)num5);
                        }
                    }
                }
            }
            return new Vector2();
        }

        public static bool SwitchTiles(Vector2 Position, int Width, int Height, Vector2 oldPosition, int objType)
        {
            int num1 = (int)((double)Position.X / 16.0) - 1;
            int num2 = (int)(((double)Position.X + (double)Width) / 16.0) + 2;
            int num3 = (int)((double)Position.Y / 16.0) - 1;
            int num4 = (int)(((double)Position.Y + (double)Height) / 16.0) + 2;
            if (num1 < 0)
                num1 = 0;
            if (num2 > Main.maxTilesX)
                num2 = Main.maxTilesX;
            if (num3 < 0)
                num3 = 0;
            if (num4 > Main.maxTilesY)
                num4 = Main.maxTilesY;
            for (int index = num1; index < num2; ++index)
            {
                for (int j = num3; j < num4; ++j)
                {
                    if (Main.tile[index, j] != null && Main.tile[index, j].active() && ((int)Main.tile[index, j].type == 135 || (int)Main.tile[index, j].type == 210))
                    {
                        Vector2 vector2;
                        vector2.X = (float)(index * 16);
                        vector2.Y = (float)(j * 16 + 12);
                        if ((double)Position.X + (double)Width > (double)vector2.X && (double)Position.X < (double)vector2.X + 16.0 && ((double)Position.Y + (double)Height > (double)vector2.Y && (double)Position.Y < (double)vector2.Y + 4.01))
                        {
                            if ((int)Main.tile[index, j].type == 210)
                                WorldGen.ExplodeMine(index, j);
                            else if ((double)oldPosition.X + (double)Width <= (double)vector2.X || (double)oldPosition.X >= (double)vector2.X + 16.0 || ((double)oldPosition.Y + (double)Height <= (double)vector2.Y || (double)oldPosition.Y >= (double)vector2.Y + 16.01))
                            {
                                int num5 = (int)Main.tile[index, j].frameY / 18;
                                bool flag = true;
                                if ((num5 == 4 || num5 == 2 || (num5 == 3 || num5 == 6)) && objType != 1)
                                    flag = false;
                                if (num5 == 5 && objType == 1)
                                    flag = false;
                                if (flag)
                                {
                                    Wiring.HitSwitch(index, j);
                                    NetMessage.SendData(59, -1, -1, "", index, (float)j, 0.0f, 0.0f, 0, 0, 0);
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static Vector2 StickyTiles(Vector2 Position, Vector2 Velocity, int Width, int Height)
        {
            Vector2 vector2_1 = Position;
            int num1 = (int)((double)Position.X / 16.0) - 1;
            int num2 = (int)(((double)Position.X + (double)Width) / 16.0) + 2;
            int num3 = (int)((double)Position.Y / 16.0) - 1;
            int num4 = (int)(((double)Position.Y + (double)Height) / 16.0) + 2;
            if (num1 < 0)
                num1 = 0;
            if (num2 > Main.maxTilesX)
                num2 = Main.maxTilesX;
            if (num3 < 0)
                num3 = 0;
            if (num4 > Main.maxTilesY)
                num4 = Main.maxTilesY;
            Vector2 vector2_2;
            for (int index1 = num1; index1 < num2; ++index1)
            {
                for (int index2 = num3; index2 < num4; ++index2)
                {
                    if (Main.tile[index1, index2] != null && Main.tile[index1, index2].active() && !Main.tile[index1, index2].inActive())
                    {
                        if ((int)Main.tile[index1, index2].type == 51)
                        {
                            int num5 = 0;
                            vector2_2.X = (float)(index1 * 16);
                            vector2_2.Y = (float)(index2 * 16);
                            if ((double)vector2_1.X + (double)Width > (double)vector2_2.X - (double)num5 && (double)vector2_1.X < (double)vector2_2.X + 16.0 + (double)num5 && ((double)vector2_1.Y + (double)Height > (double)vector2_2.Y && (double)vector2_1.Y < (double)vector2_2.Y + 16.01))
                            {
                                if ((int)Main.tile[index1, index2].type == 51 && (double)Math.Abs(Velocity.X) + (double)Math.Abs(Velocity.Y) > 0.7 && Main.rand.Next(30) == 0)
                                    Dust.NewDust(new Vector2((float)(index1 * 16), (float)(index2 * 16)), 16, 16, 30, 0.0f, 0.0f, 0, new Color(), 1f);
                                return new Vector2((float)index1, (float)index2);
                            }
                        }
                        else if ((int)Main.tile[index1, index2].type == 229 && (int)Main.tile[index1, index2].slope() == 0)
                        {
                            int num5 = 1;
                            vector2_2.X = (float)(index1 * 16);
                            vector2_2.Y = (float)(index2 * 16);
                            float num6 = 16.01f;
                            if (Main.tile[index1, index2].halfBrick())
                            {
                                vector2_2.Y += 8f;
                                num6 -= 8f;
                            }
                            if ((double)vector2_1.X + (double)Width > (double)vector2_2.X - (double)num5 && (double)vector2_1.X < (double)vector2_2.X + 16.0 + (double)num5 && ((double)vector2_1.Y + (double)Height > (double)vector2_2.Y && (double)vector2_1.Y < (double)vector2_2.Y + (double)num6))
                            {
                                if ((int)Main.tile[index1, index2].type == 51 && (double)Math.Abs(Velocity.X) + (double)Math.Abs(Velocity.Y) > 0.7 && Main.rand.Next(30) == 0)
                                    Dust.NewDust(new Vector2((float)(index1 * 16), (float)(index2 * 16)), 16, 16, 30, 0.0f, 0.0f, 0, new Color(), 1f);
                                return new Vector2((float)index1, (float)index2);
                            }
                        }
                    }
                }
            }
            return new Vector2(-1f, -1f);
        }

        public static bool SolidTilesVersatile(int startX, int endX, int startY, int endY)
        {
            if (startX > endX)
                Utils.Swap<int>(ref startX, ref endX);
            if (startY > endY)
                Utils.Swap<int>(ref startY, ref endY);
            return Collision.SolidTiles(startX, endX, startY, endY);
        }

        public static bool SolidTiles(int startX, int endX, int startY, int endY)
        {
            if (startX < 0 || endX >= Main.maxTilesX || (startY < 0 || endY >= Main.maxTilesY))
                return true;
            for (int index1 = startX; index1 < endX + 1; ++index1)
            {
                for (int index2 = startY; index2 < endY + 1; ++index2)
                {
                    if (Main.tile[index1, index2] == null)
                        return false;
                    if (Main.tile[index1, index2].active() && !Main.tile[index1, index2].inActive() && (Main.tileSolid[(int)Main.tile[index1, index2].type] && !Main.tileSolidTop[(int)Main.tile[index1, index2].type]))
                        return true;
                }
            }
            return false;
        }

        public static void StepDown(ref Vector2 position, ref Vector2 velocity, int width, int height, ref float stepSpeed, ref float gfxOffY, int gravDir = 1, bool waterWalk = false)
        {
            Vector2 vector2 = position;
            vector2.X += velocity.X;
            vector2.Y = (float)Math.Floor(((double)vector2.Y + (double)height) / 16.0) * 16f - (float)height;
            bool flag = false;
            int num1 = (int)((double)vector2.X / 16.0);
            int num2 = (int)(((double)vector2.X + (double)width) / 16.0);
            int num3 = (int)(((double)vector2.Y + (double)height + 4.0) / 16.0);
            int num4 = height / 16 + (height % 16 == 0 ? 0 : 1);
            float num5 = (float)((num3 + num4) * 16);
            float num6 = (float)((double)Main.bottomWorld / 16.0 - 42.0);
            for (int x = num1; x <= num2; ++x)
            {
                for (int y = num3; y <= num3 + 1; ++y)
                {
                    if (WorldGen.InWorld(x, y, 1))
                    {
                        if (Main.tile[x, y] == null)
                            Main.tile[x, y] = new Tile();
                        if (Main.tile[x, y - 1] == null)
                            Main.tile[x, y - 1] = new Tile();
                        if (Main.tile[x, y].topSlope())
                            flag = true;
                        if (waterWalk && (int)Main.tile[x, y].liquid > 0 && (int)Main.tile[x, y - 1].liquid == 0)
                        {
                            int num7 = (int)Main.tile[x, y].liquid / 32 * 2 + 2;
                            int num8 = y * 16 + 16 - num7;
                            if (new Rectangle(x * 16, y * 16 - 17, 16, 16).Intersects(new Rectangle((int)position.X, (int)position.Y, width, height)) && (double)num8 < (double)num5)
                                num5 = (float)num8;
                        }
                        if ((double)y >= (double)num6 || Main.tile[x, y].nactive() && (Main.tileSolid[(int)Main.tile[x, y].type] || Main.tileSolidTop[(int)Main.tile[x, y].type]))
                        {
                            int num7 = y * 16;
                            if (Main.tile[x, y].halfBrick())
                                num7 += 8;
                            if (Utils.FloatIntersect((float)(x * 16), (float)(y * 16 - 17), 16f, 16f, position.X, position.Y, (float)width, (float)height) && (double)num7 < (double)num5)
                                num5 = (float)num7;
                        }
                    }
                }
            }
            float num9 = num5 - (position.Y + (float)height);
            if ((double)num9 <= 7.0 || (double)num9 >= 17.0 || flag)
                return;
            stepSpeed = 1.5f;
            if ((double)num9 > 9.0)
                stepSpeed = 2.5f;
            gfxOffY += position.Y + (float)height - num5;
            position.Y = num5 - (float)height;
        }

        public static void StepUp(ref Vector2 position, ref Vector2 velocity, int width, int height, ref float stepSpeed, ref float gfxOffY, int gravDir = 1, bool holdsMatching = false, int specialChecksMode = 0)
        {
            int num1 = 0;
            if ((double)velocity.X < 0.0)
                num1 = -1;
            if ((double)velocity.X > 0.0)
                num1 = 1;
            Vector2 vector2 = position;
            vector2.X += velocity.X;
            int x = (int)(((double)vector2.X + (double)(width / 2) + (double)((width / 2 + 1) * num1)) / 16.0);
            int index1 = (int)(((double)vector2.Y + 0.1) / 16.0);
            if (gravDir == 1)
                index1 = (int)(((double)vector2.Y + (double)height - 1.0) / 16.0);
            int num2 = height / 16 + (height % 16 == 0 ? 0 : 1);
            bool flag1 = true;
            bool flag2 = true;
            if (Main.tile[x, index1] == null)
                return;
            for (int index2 = 1; index2 < num2 + 2; ++index2)
            {
                if (!WorldGen.InWorld(x, index1 - index2 * gravDir, 0) || Main.tile[x, index1 - index2 * gravDir] == null)
                    return;
            }
            if (!WorldGen.InWorld(x - num1, index1 - num2 * gravDir, 0) || Main.tile[x - num1, index1 - num2 * gravDir] == null)
                return;
            for (int index2 = 2; index2 < num2 + 1; ++index2)
            {
                if (!WorldGen.InWorld(x, index1 - index2 * gravDir, 0) || Main.tile[x, index1 - index2 * gravDir] == null)
                    return;
                Tile tile = Main.tile[x, index1 - index2 * gravDir];
                flag1 = flag1 && (!tile.nactive() || !Main.tileSolid[(int)tile.type] || Main.tileSolidTop[(int)tile.type]);
            }
            Tile tile1 = Main.tile[x - num1, index1 - num2 * gravDir];
            bool flag3 = flag2 && (!tile1.nactive() || !Main.tileSolid[(int)tile1.type] || Main.tileSolidTop[(int)tile1.type]);
            bool flag4 = true;
            bool flag5 = true;
            bool flag6 = true;
            bool flag7;
            bool flag8;
            if (gravDir == 1)
            {
                if (Main.tile[x, index1 - gravDir] == null || Main.tile[x, index1 - (num2 + 1) * gravDir] == null)
                    return;
                Tile tile2 = Main.tile[x, index1 - gravDir];
                Tile tile3 = Main.tile[x, index1 - (num2 + 1) * gravDir];
                flag7 = flag4 && (!tile2.nactive() || !Main.tileSolid[(int)tile2.type] || Main.tileSolidTop[(int)tile2.type] || ((int)tile2.slope() == 1 && (double)position.X + (double)(width / 2) > (double)(x * 16) || (int)tile2.slope() == 2 && (double)position.X + (double)(width / 2) < (double)(x * 16 + 16)) || tile2.halfBrick() && (!tile3.nactive() || !Main.tileSolid[(int)tile3.type] || Main.tileSolidTop[(int)tile3.type]));
                Tile tile4 = Main.tile[x, index1];
                Tile tile5 = Main.tile[x, index1 - 1];
                if (specialChecksMode == 1)
                    flag6 = (int)tile4.type != 16 && (int)tile4.type != 18 && (int)tile4.type != 134;
                flag8 = ((!flag5 ? (false ? 1 : 0) : (!tile4.nactive() || tile4.topSlope() && ((int)tile4.slope() != 1 || (double)position.X + (double)(width / 2) >= (double)(x * 16)) && ((int)tile4.slope() != 2 || (double)position.X + (double)(width / 2) <= (double)(x * 16 + 16)) || tile4.topSlope() && (double)position.Y + (double)height <= (double)(index1 * 16) || (!Main.tileSolid[(int)tile4.type] || Main.tileSolidTop[(int)tile4.type]) && (!holdsMatching || (!Main.tileSolidTop[(int)tile4.type] || (int)tile4.frameY != 0) && (int)tile4.type != 19 || (Main.tileSolid[(int)tile5.type] && tile5.nactive() || !flag6)) ? (!tile5.halfBrick() ? (false ? 1 : 0) : (tile5.nactive() ? 1 : 0)) : (true ? 1 : 0))) & (!Main.tileSolidTop[(int)tile4.type] ? 1 : (!Main.tileSolidTop[(int)tile5.type] ? 1 : 0))) != 0;
            }
            else
            {
                Tile tile2 = Main.tile[x, index1 - gravDir];
                Tile tile3 = Main.tile[x, index1 - (num2 + 1) * gravDir];
                flag7 = flag4 && (!tile2.nactive() || !Main.tileSolid[(int)tile2.type] || (Main.tileSolidTop[(int)tile2.type] || (int)tile2.slope() != 0) || tile2.halfBrick() && (!tile3.nactive() || !Main.tileSolid[(int)tile3.type] || Main.tileSolidTop[(int)tile3.type]));
                Tile tile4 = Main.tile[x, index1];
                Tile tile5 = Main.tile[x, index1 + 1];
                flag8 = flag5 && (tile4.nactive() && (Main.tileSolid[(int)tile4.type] && !Main.tileSolidTop[(int)tile4.type] || holdsMatching && Main.tileSolidTop[(int)tile4.type] && (int)tile4.frameY == 0 && (!Main.tileSolid[(int)tile5.type] || !tile5.nactive())) || tile5.halfBrick() && tile5.nactive());
            }
            if ((double)(x * 16) >= (double)vector2.X + (double)width || (double)(x * 16 + 16) <= (double)vector2.X)
                return;
            if (gravDir == 1)
            {
                if (!flag8 || !flag7 || (!flag1 || !flag3))
                    return;
                float num3 = (float)(index1 * 16);
                if (Main.tile[x, index1 - 1].halfBrick())
                    num3 -= 8f;
                else if (Main.tile[x, index1].halfBrick())
                    num3 += 8f;
                if ((double)num3 >= (double)vector2.Y + (double)height)
                    return;
                float num4 = vector2.Y + (float)height - num3;
                if ((double)num4 > 16.1)
                    return;
                gfxOffY += position.Y + (float)height - num3;
                position.Y = num3 - (float)height;
                if ((double)num4 < 9.0)
                    stepSpeed = 1f;
                else
                    stepSpeed = 2f;
            }
            else
            {
                if (!flag8 || !flag7 || (!flag1 || !flag3) || Main.tile[x, index1].bottomSlope())
                    return;
                float num3 = (float)(index1 * 16 + 16);
                if ((double)num3 <= (double)vector2.Y)
                    return;
                float num4 = num3 - vector2.Y;
                if ((double)num4 > 16.1)
                    return;
                gfxOffY -= num3 - position.Y;
                position.Y = num3;
                velocity.Y = 0.0f;
                if ((double)num4 < 9.0)
                    stepSpeed = 1f;
                else
                    stepSpeed = 2f;
            }
        }

        public static bool InTileBounds(int x, int y, int lx, int ly, int hx, int hy)
        {
            return x >= lx && x <= hx && (y >= ly && y <= hy);
        }

        public static float GetTileRotation(Vector2 position)
        {
            float num1 = position.Y % 16f;
            int index1 = (int)((double)position.X / 16.0);
            int index2 = (int)((double)position.Y / 16.0);
            Tile tile = Main.tile[index1, index2];
            bool flag = false;
            for (int index3 = 2; index3 >= 0; --index3)
            {
                if (tile.active())
                {
                    if (Main.tileSolid[(int)tile.type])
                    {
                        int num2 = tile.blockType();
                        if ((int)tile.type == 19)
                        {
                            int num3 = (int)tile.frameX / 18;
                            if ((num3 >= 0 && num3 <= 7 || num3 >= 12 && num3 <= 16) && ((double)num1 == 0.0 || flag))
                                return 0.0f;
                            switch (num3)
                            {
                                case 8:
                                case 19:
                                case 21:
                                case 23:
                                    return -0.7853982f;
                                case 10:
                                case 20:
                                case 22:
                                case 24:
                                    return 0.7853982f;
                                case 25:
                                case 26:
                                    if (flag)
                                        return 0.0f;
                                    if (num2 == 2)
                                        return 0.7853982f;
                                    if (num2 == 3)
                                        return -0.7853982f;
                                    break;
                            }
                        }
                        else
                        {
                            if (num2 == 1)
                                return 0.0f;
                            if (num2 == 2)
                                return 0.7853982f;
                            return num2 == 3 ? -0.7853982f : 0.0f;
                        }
                    }
                    else if (Main.tileSolidTop[(int)tile.type] && (int)tile.frameY == 0 && flag)
                        return 0.0f;
                }
                ++index2;
                tile = Main.tile[index1, index2];
                flag = true;
            }
            return 0.0f;
        }
    }
}
