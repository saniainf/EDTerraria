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
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Terraria.DataStructures;

namespace Terraria
{
    public static class Utils
    {
        public static Dictionary<SpriteFont, float[]> charLengths = new Dictionary<SpriteFont, float[]>();
        public const long MaxCoins = 999999999L;
        private const ulong RANDOM_MULTIPLIER = 25214903917UL;
        private const ulong RANDOM_ADD = 11UL;
        private const ulong RANDOM_MASK = 281474976710655UL;

        public static float SmoothStep(float min, float max, float x)
        {
            return MathHelper.Clamp((float)(((double)x - (double)min) / ((double)max - (double)min)), 0.0f, 1f);
        }

        public static Dictionary<string, string> ParseArguements(string[] args)
        {
            string key = (string)null;
            string str1 = "";
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            for (int index = 0; index < args.Length; ++index)
            {
                if (args[index].Length != 0)
                {
                    if ((int)args[index][0] == 45 || (int)args[index][0] == 43)
                    {
                        if (key != null)
                            dictionary.Add(key, str1);
                        key = args[index];
                        str1 = "";
                    }
                    else
                    {
                        if (str1 != "")
                            str1 += " ";
                        str1 += args[index];
                    }
                }
            }

            if (key != null)
                dictionary.Add(key, str1);

            return dictionary;
        }

        public static void Swap<T>(ref T t1, ref T t2)
        {
            T obj = t1;
            t1 = t2;
            t2 = obj;
        }

        public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(max) > 0)
                return max;
            if (value.CompareTo(min) < 0)
                return min;
            return value;
        }

        public static string[] FixArgs(string[] brokenArgs)
        {
            ArrayList arrayList = new ArrayList();
            string str = "";
            for (int index = 0; index < brokenArgs.Length; ++index)
            {
                if (brokenArgs[index].StartsWith("-"))
                {
                    if (str != "")
                    {
                        arrayList.Add((object)str);
                        str = "";
                    }
                    else
                        arrayList.Add((object)"");
                    arrayList.Add((object)brokenArgs[index]);
                }
                else
                {
                    if (str != "")
                        str += " ";
                    str += brokenArgs[index];
                }
            }
            arrayList.Add((object)str);
            string[] strArray = new string[arrayList.Count];
            arrayList.CopyTo((Array)strArray);
            return strArray;
        }

        public static string[] WordwrapString(string text, SpriteFont font, int maxWidth, int maxLines, out int lineAmount)
        {
            string[] strArray1 = new string[maxLines];
            int index1 = 0;
            List<string> list1 = new List<string>((IEnumerable<string>)text.Split('\n'));
            List<string> list2 = new List<string>((IEnumerable<string>)list1[0].Split(' '));
            for (int index2 = 1; index2 < list1.Count; ++index2)
            {
                list2.Add("\n");
                list2.AddRange((IEnumerable<string>)list1[index2].Split(' '));
            }
            bool flag = true;
            while (list2.Count > 0)
            {
                string text1 = list2[0];
                string str1 = " ";
                if (list2.Count == 1)
                    str1 = "";
                if (text1 == "\n")
                {
                    string[] strArray2;
                    int index2;
                    string str2 = (strArray2 = strArray1)[(int)(index2 = index1++)] + text1;
                    strArray2[index2] = str2;
                    if (index1 < maxLines)
                        list2.RemoveAt(0);
                    else
                        break;
                }
                else if (flag)
                {
                    if ((double)font.MeasureString(text1).X > (double)maxWidth)
                    {
                        string str2 = string.Concat((object)text1[0]);
                        int startIndex = 1;
                        while ((double)font.MeasureString(str2 + (object)text1[startIndex] + (string)(object)'-').X <= (double)maxWidth)
                            str2 += (string)(object)text1[startIndex++];
                        string str3 = str2 + (object)'-';
                        strArray1[index1++] = str3 + " ";
                        if (index1 < maxLines)
                        {
                            list2.RemoveAt(0);
                            list2.Insert(0, text1.Substring(startIndex));
                        }
                        else
                            break;
                    }
                    else
                    {
                        string[] strArray2;
                        IntPtr index2;
                        (strArray2 = strArray1)[(int)(index2 = (IntPtr)index1)] = strArray2[(int)index2] + text1 + str1;
                        flag = false;
                        list2.RemoveAt(0);
                    }
                }
                else if ((double)font.MeasureString(strArray1[index1] + text1).X > (double)maxWidth)
                {
                    ++index1;
                    if (index1 < maxLines)
                        flag = true;
                    else
                        break;
                }
                else
                {
                    string[] strArray2;
                    IntPtr index2;
                    (strArray2 = strArray1)[(int)(index2 = (IntPtr)index1)] = strArray2[(int)index2] + text1 + str1;
                    flag = false;
                    list2.RemoveAt(0);
                }
            }
            lineAmount = index1;
            if (lineAmount == maxLines)
                --lineAmount;
            return strArray1;
        }

        public static Rectangle CenteredRectangle(Vector2 center, Vector2 size)
        {
            return new Rectangle((int)((double)center.X - (double)size.X / 2.0), (int)((double)center.Y - (double)size.Y / 2.0), (int)size.X, (int)size.Y);
        }

        public static Vector2 Vector2FromElipse(Vector2 angleVector, Vector2 elipseSizes)
        {
            if (elipseSizes == Vector2.Zero || angleVector == Vector2.Zero)
                return Vector2.Zero;
            angleVector.Normalize();
            Vector2 vector2 = Vector2.One / Vector2.Normalize(elipseSizes);
            angleVector *= vector2;
            angleVector.Normalize();
            return angleVector * elipseSizes / 2f;
        }

        public static bool FloatIntersect(float r1StartX, float r1StartY, float r1Width, float r1Height, float r2StartX, float r2StartY, float r2Width, float r2Height)
        {
            return (double)r1StartX <= (double)r2StartX + (double)r2Width && (double)r1StartY <= (double)r2StartY + (double)r2Height && ((double)r1StartX + (double)r1Width >= (double)r2StartX && (double)r1StartY + (double)r1Height >= (double)r2StartY);
        }

        public static long CoinsCount(out bool overFlowing, Item[] inv, params int[] ignoreSlots)
        {
            List<int> list = new List<int>((IEnumerable<int>)ignoreSlots);
            long num = 0L;
            for (int index = 0; index < inv.Length; ++index)
            {
                if (!list.Contains(index))
                {
                    switch (inv[index].type)
                    {
                        case 71:
                            num += (long)inv[index].stack;
                            break;
                        case 72:
                            num += (long)(inv[index].stack * 100);
                            break;
                        case 73:
                            num += (long)(inv[index].stack * 10000);
                            break;
                        case 74:
                            num += (long)(inv[index].stack * 1000000);
                            break;
                    }
                    if (num >= 999999999L)
                    {
                        overFlowing = true;
                        return 999999999L;
                    }
                }
            }
            overFlowing = false;
            return num;
        }

        public static int[] CoinsSplit(long count)
        {
            int[] numArray = new int[4];
            long num1 = 0L;
            long num2 = 1000000L;
            for (int index = 3; index >= 0; --index)
            {
                numArray[index] = (int)((count - num1) / num2);
                num1 += (long)numArray[index] * num2;
                num2 /= 100L;
            }
            return numArray;
        }

        public static long CoinsCombineStacks(out bool overFlowing, params long[] coinCounts)
        {
            long num1 = 0L;
            foreach (long num2 in coinCounts)
            {
                num1 += num2;
                if (num1 >= 999999999L)
                {
                    overFlowing = true;
                    return 999999999L;
                }
            }
            overFlowing = false;
            return num1;
        }

        public static byte[] ToByteArray(this string str)
        {
            byte[] numArray = new byte[str.Length * 2];
            Buffer.BlockCopy((Array)str.ToCharArray(), 0, (Array)numArray, 0, numArray.Length);
            return numArray;
        }

        public static float NextFloat(this Random r)
        {
            return (float)r.NextDouble();
        }

        public static Rectangle Frame(this Texture2D tex, int horizontalFrames = 1, int verticalFrames = 1, int frameX = 0, int frameY = 0)
        {
            int width = tex.Width / horizontalFrames;
            int height = tex.Height / verticalFrames;
            return new Rectangle(width * frameX, height * frameY, width, height);
        }

        public static Vector2 OriginFlip(this Rectangle rect, Vector2 origin, SpriteEffects effects)
        {
            if (effects.HasFlag((Enum)SpriteEffects.FlipHorizontally))
                origin.X = (float)rect.Width - origin.X;
            if (effects.HasFlag((Enum)SpriteEffects.FlipVertically))
                origin.Y = (float)rect.Height - origin.Y;
            return origin;
        }

        public static Vector2 Size(this Texture2D tex)
        {
            return new Vector2((float)tex.Width, (float)tex.Height);
        }

        public static void WriteRGB(this BinaryWriter bb, Color c)
        {
            bb.Write(c.R);
            bb.Write(c.G);
            bb.Write(c.B);
        }

        public static void WriteVector2(this BinaryWriter bb, Vector2 v)
        {
            bb.Write(v.X);
            bb.Write(v.Y);
        }

        public static Color ReadRGB(this BinaryReader bb)
        {
            return new Color((int)bb.ReadByte(), (int)bb.ReadByte(), (int)bb.ReadByte());
        }

        public static Vector2 ReadVector2(this BinaryReader bb)
        {
            return new Vector2(bb.ReadSingle(), bb.ReadSingle());
        }

        public static Vector2 Left(this Rectangle r)
        {
            return new Vector2((float)r.X, (float)(r.Y + r.Height / 2));
        }

        public static Vector2 Right(this Rectangle r)
        {
            return new Vector2((float)(r.X + r.Width), (float)(r.Y + r.Height / 2));
        }

        public static Vector2 Top(this Rectangle r)
        {
            return new Vector2((float)(r.X + r.Width / 2), (float)r.Y);
        }

        public static Vector2 Bottom(this Rectangle r)
        {
            return new Vector2((float)(r.X + r.Width / 2), (float)(r.Y + r.Height));
        }

        public static Vector2 TopLeft(this Rectangle r)
        {
            return new Vector2((float)r.X, (float)r.Y);
        }

        public static Vector2 TopRight(this Rectangle r)
        {
            return new Vector2((float)(r.X + r.Width), (float)r.Y);
        }

        public static Vector2 BottomLeft(this Rectangle r)
        {
            return new Vector2((float)r.X, (float)(r.Y + r.Height));
        }

        public static Vector2 BottomRight(this Rectangle r)
        {
            return new Vector2((float)(r.X + r.Width), (float)(r.Y + r.Height));
        }

        public static Vector2 Center(this Rectangle r)
        {
            return new Vector2((float)(r.X + r.Width / 2), (float)(r.Y + r.Height / 2));
        }

        public static Vector2 Size(this Rectangle r)
        {
            return new Vector2((float)r.Width, (float)r.Height);
        }

        public static float Distance(this Rectangle r, Vector2 point)
        {
            if (Utils.FloatIntersect((float)r.Left, (float)r.Top, (float)r.Width, (float)r.Height, point.X, point.Y, 0.0f, 0.0f))
                return 0.0f;
            if ((double)point.X >= (double)r.Left && (double)point.X <= (double)r.Right)
            {
                if ((double)point.Y < (double)r.Top)
                    return (float)r.Top - point.Y;
                return point.Y - (float)r.Bottom;
            }
            if ((double)point.Y >= (double)r.Top && (double)point.Y <= (double)r.Bottom)
            {
                if ((double)point.X < (double)r.Left)
                    return (float)r.Left - point.X;
                return point.X - (float)r.Right;
            }
            if ((double)point.X < (double)r.Left)
            {
                if ((double)point.Y < (double)r.Top)
                    return Vector2.Distance(point, Utils.TopLeft(r));
                return Vector2.Distance(point, Utils.BottomLeft(r));
            }
            if ((double)point.Y < (double)r.Top)
                return Vector2.Distance(point, Utils.TopRight(r));
            return Vector2.Distance(point, Utils.BottomRight(r));
        }

        public static float ToRotation(this Vector2 v)
        {
            return (float)Math.Atan2((double)v.Y, (double)v.X);
        }

        public static Vector2 ToRotationVector2(this float f)
        {
            return new Vector2((float)Math.Cos((double)f), (float)Math.Sin((double)f));
        }

        public static Vector2 RotatedBy(this Vector2 spinningpoint, double radians, Vector2 center = default(Vector2))
        {
            float num1 = (float)Math.Cos(radians);
            float num2 = (float)Math.Sin(radians);
            Vector2 vector2_1 = spinningpoint - center;
            Vector2 vector2_2 = center;
            vector2_2.X += (float)((double)vector2_1.X * (double)num1 - (double)vector2_1.Y * (double)num2);
            vector2_2.Y += (float)((double)vector2_1.X * (double)num2 + (double)vector2_1.Y * (double)num1);
            return vector2_2;
        }

        public static Vector2 RotatedByRandom(this Vector2 spinninpoint, double maxRadians)
        {
            return Utils.RotatedBy(spinninpoint, Main.rand.NextDouble() * maxRadians - Main.rand.NextDouble() * maxRadians, new Vector2());
        }

        public static Vector2 Floor(this Vector2 vec)
        {
            vec.X = (float)(int)vec.X;
            vec.Y = (float)(int)vec.Y;
            return vec;
        }

        public static bool HasNaNs(this Vector2 vec)
        {
            if (!float.IsNaN(vec.X))
                return float.IsNaN(vec.Y);
            return true;
        }

        public static bool Between(this Vector2 vec, Vector2 minimum, Vector2 maximum)
        {
            if ((double)vec.X >= (double)minimum.X && (double)vec.X <= (double)maximum.X && (double)vec.Y >= (double)minimum.Y)
                return (double)vec.Y <= (double)maximum.Y;
            return false;
        }

        public static Vector2 ToVector2(this Point p)
        {
            return new Vector2((float)p.X, (float)p.Y);
        }

        public static Point16 ToTileCoordinates16(this Vector2 vec)
        {
            return new Point16((int)vec.X >> 4, (int)vec.Y >> 4);
        }

        public static Point ToTileCoordinates(this Vector2 vec)
        {
            return new Point((int)vec.X >> 4, (int)vec.Y >> 4);
        }

        public static Point ToPoint(this Vector2 v)
        {
            return new Point((int)v.X, (int)v.Y);
        }

        public static Vector2 XY(this Vector4 vec)
        {
            return new Vector2(vec.X, vec.Y);
        }

        public static Vector2 ZW(this Vector4 vec)
        {
            return new Vector2(vec.Z, vec.W);
        }

        public static Vector3 XZW(this Vector4 vec)
        {
            return new Vector3(vec.X, vec.Z, vec.W);
        }

        public static Vector3 YZW(this Vector4 vec)
        {
            return new Vector3(vec.Y, vec.Z, vec.W);
        }

        public static Color MultiplyRGB(this Color firstColor, Color secondColor)
        {
            return new Color((int)(byte)((double)((int)firstColor.R * (int)secondColor.R) / (double)byte.MaxValue), (int)(byte)((double)((int)firstColor.G * (int)secondColor.G) / (double)byte.MaxValue), (int)(byte)((double)((int)firstColor.B * (int)secondColor.B) / (double)byte.MaxValue));
        }

        public static Color MultiplyRGBA(this Color firstColor, Color secondColor)
        {
            return new Color((int)(byte)((double)((int)firstColor.R * (int)secondColor.R) / (double)byte.MaxValue), (int)(byte)((double)((int)firstColor.G * (int)secondColor.G) / (double)byte.MaxValue), (int)(byte)((double)((int)firstColor.B * (int)secondColor.B) / (double)byte.MaxValue), (int)(byte)((double)((int)firstColor.A * (int)secondColor.A) / (double)byte.MaxValue));
        }

        public static string Hex3(this Color color)
        {
            return (color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2")).ToLower();
        }

        public static string Hex4(this Color color)
        {
            return (color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2") + color.A.ToString("X2")).ToLower();
        }

        public static int ToDirectionInt(this bool value)
        {
            return !value ? -1 : 1;
        }

        public static int ToInt(this bool value)
        {
            return !value ? 0 : 1;
        }

        public static float AngleLerp(this float curAngle, float targetAngle, float amount)
        {
            float angle;
            if ((double)targetAngle < (double)curAngle)
            {
                float num = targetAngle + 6.283185f;
                angle = (double)num - (double)curAngle > (double)curAngle - (double)targetAngle ? MathHelper.Lerp(curAngle, targetAngle, amount) : MathHelper.Lerp(curAngle, num, amount);
            }
            else
            {
                if ((double)targetAngle <= (double)curAngle)
                    return curAngle;
                float num = targetAngle - 6.283185f;
                angle = (double)targetAngle - (double)curAngle > (double)curAngle - (double)num ? MathHelper.Lerp(curAngle, num, amount) : MathHelper.Lerp(curAngle, targetAngle, amount);
            }
            return MathHelper.WrapAngle(angle);
        }

        public static float AngleTowards(this float curAngle, float targetAngle, float maxChange)
        {
            curAngle = MathHelper.WrapAngle(curAngle);
            targetAngle = MathHelper.WrapAngle(targetAngle);
            if ((double)curAngle < (double)targetAngle)
            {
                if ((double)targetAngle - (double)curAngle > 3.14159274101257)
                    curAngle += 6.283185f;
            }
            else if ((double)curAngle - (double)targetAngle > 3.14159274101257)
                curAngle -= 6.283185f;
            curAngle += MathHelper.Clamp(targetAngle - curAngle, -maxChange, maxChange);
            return MathHelper.WrapAngle(curAngle);
        }

        public static bool deepCompare(this int[] firstArray, int[] secondArray)
        {
            if (firstArray == null && secondArray == null)
                return true;
            if (firstArray == null || secondArray == null || firstArray.Length != secondArray.Length)
                return false;
            for (int index = 0; index < firstArray.Length; ++index)
            {
                if (firstArray[index] != secondArray[index])
                    return false;
            }
            return true;
        }

        public static bool PlotLine(Point16 p0, Point16 p1, Utils.PerLinePoint plot, bool jump = true)
        {
            return Utils.PlotLine((int)p0.X, (int)p0.Y, (int)p1.X, (int)p1.Y, plot, jump);
        }

        public static bool PlotLine(Point p0, Point p1, Utils.PerLinePoint plot, bool jump = true)
        {
            return Utils.PlotLine(p0.X, p0.Y, p1.X, p1.Y, plot, jump);
        }

        private static bool PlotLine(int x0, int y0, int x1, int y1, Utils.PerLinePoint plot, bool jump = true)
        {
            if (x0 == x1 && y0 == y1)
                return plot(x0, y0);
            bool flag = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (flag)
            {
                Utils.Swap<int>(ref x0, ref y0);
                Utils.Swap<int>(ref x1, ref y1);
            }
            int num1 = Math.Abs(x1 - x0);
            int num2 = Math.Abs(y1 - y0);
            int num3 = num1 / 2;
            int num4 = y0;
            int num5 = x0 < x1 ? 1 : -1;
            int num6 = y0 < y1 ? 1 : -1;
            int num7 = x0;
            while (num7 != x1)
            {
                if (flag)
                {
                    if (!plot(num4, num7))
                        return false;
                }
                else if (!plot(num7, num4))
                    return false;
                num3 -= num2;
                if (num3 < 0)
                {
                    num4 += num6;
                    if (!jump)
                    {
                        if (flag)
                        {
                            if (!plot(num4, num7))
                                return false;
                        }
                        else if (!plot(num7, num4))
                            return false;
                    }
                    num3 += num1;
                }
                num7 += num5;
            }
            return true;
        }

        public static int RandomNext(ref ulong seed, int bits)
        {
            seed = Utils.RandomNextSeed(seed);
            return (int)(seed >> 48 - bits);
        }

        public static ulong RandomNextSeed(ulong seed)
        {
            return (ulong)((long)seed * 25214903917L + 11L & 281474976710655L);
        }

        public static float RandomFloat(ref ulong seed)
        {
            return (float)Utils.RandomNext(ref seed, 24) / 1.677722E+07f;
        }

        public static int RandomInt(ref ulong seed, int max)
        {
            if ((max & -max) == max)
                return (int)((long)max * (long)Utils.RandomNext(ref seed, 31) >> 31);
            int num1;
            int num2;
            do
            {
                num1 = Utils.RandomNext(ref seed, 31);
                num2 = num1 % max;
            }
            while (num1 - num2 + (max - 1) < 0);
            return num2;
        }

        public static int RandomInt(ref ulong seed, int min, int max)
        {
            return Utils.RandomInt(ref seed, max - min) + min;
        }

        public static bool PlotTileLine(Vector2 start, Vector2 end, float width, Utils.PerLinePoint plot)
        {
            float num = width / 2f;
            Vector2 vector2_1 = end - start;
            Vector2 vector2_2 = vector2_1 / vector2_1.Length();
            Vector2 vector2_3 = new Vector2(-vector2_2.Y, vector2_2.X) * num;
            Point point1 = Utils.ToTileCoordinates(start - vector2_3);
            Point point2 = Utils.ToTileCoordinates(start + vector2_3);
            Point point3 = Utils.ToTileCoordinates(start);
            Point point4 = Utils.ToTileCoordinates(end);
            Point lineMinOffset = new Point(point1.X - point3.X, point1.Y - point3.Y);
            Point lineMaxOffset = new Point(point2.X - point3.X, point2.Y - point3.Y);
            return Utils.PlotLine(point3.X, point3.Y, point4.X, point4.Y, (Utils.PerLinePoint)((x, y) => Utils.PlotLine(x + lineMinOffset.X, y + lineMinOffset.Y, x + lineMaxOffset.X, y + lineMaxOffset.Y, plot, false)), true);
        }

        public static bool PlotTileTale(Vector2 start, Vector2 end, float width, Utils.PerLinePoint plot)
        {
            float halfWidth = width / 2f;
            Vector2 vector2_1 = end - start;
            Vector2 vector2_2 = vector2_1 / vector2_1.Length();
            Vector2 perpOffset = new Vector2(-vector2_2.Y, vector2_2.X);
            Point pointStart = Utils.ToTileCoordinates(start);
            Point point1 = Utils.ToTileCoordinates(end);
            int length = 0;
            Utils.PlotLine(pointStart.X, pointStart.Y, point1.X, point1.Y, (Utils.PerLinePoint)((x, y) =>
            {
                ++length;
                return true;
            }), true);
            --length;
            int curLength = 0;
            return Utils.PlotLine(pointStart.X, pointStart.Y, point1.X, point1.Y, (Utils.PerLinePoint)((x, y) =>
            {
                float num = (float)(1.0 - (double)curLength / (double)length);
                ++curLength;
                Point point2 = Utils.ToTileCoordinates(start - perpOffset * halfWidth * num);
                Point point3 = Utils.ToTileCoordinates(start + perpOffset * halfWidth * num);
                Point point4 = new Point(point2.X - pointStart.X, point2.Y - pointStart.Y);
                Point point5 = new Point(point3.X - pointStart.X, point3.Y - pointStart.Y);
                return Utils.PlotLine(x + point4.X, y + point4.Y, x + point5.X, y + point5.Y, plot, false);
            }), true);
        }

        public static int RandomConsecutive(double random, int odds)
        {
            return (int)Math.Log(1.0 - random, 1.0 / (double)odds);
        }

        public static Vector2 RandomVector2(Random random, float min, float max)
        {
            return new Vector2((max - min) * (float)random.NextDouble() + min, (max - min) * (float)random.NextDouble() + min);
        }

        public static T SelectRandom<T>(Random random, params T[] choices)
        {
            return choices[random.Next(choices.Length)];
        }

        public static void DrawBorderStringFourWay(SpriteBatch sb, SpriteFont font, string text, float x, float y, Color textColor, Color borderColor, Vector2 origin, float scale = 1f)
        {
            Color color = borderColor;
            Vector2 zero = Vector2.Zero;
            for (int index = 0; index < 5; ++index)
            {
                switch (index)
                {
                    case 0:
                        zero.X = x - 2f;
                        zero.Y = y;
                        break;
                    case 1:
                        zero.X = x + 2f;
                        zero.Y = y;
                        break;
                    case 2:
                        zero.X = x;
                        zero.Y = y - 2f;
                        break;
                    case 3:
                        zero.X = x;
                        zero.Y = y + 2f;
                        break;
                    default:
                        zero.X = x;
                        zero.Y = y;
                        color = textColor;
                        break;
                }
                sb.DrawString(font, text, zero, color, 0.0f, origin, scale, SpriteEffects.None, 0.0f);
            }
        }

        public static Vector2 DrawBorderString(SpriteBatch sb, string text, Vector2 pos, Color color, float scale = 1f, float anchorx = 0.0f, float anchory = 0.0f, int stringLimit = -1)
        {
            if (stringLimit != -1 && text.Length > stringLimit)
                text.Substring(0, stringLimit);
            SpriteFont spriteFont = Main.fontMouseText;
            for (int index1 = -1; index1 < 2; ++index1)
            {
                for (int index2 = -1; index2 < 2; ++index2)
                    sb.DrawString(spriteFont, text, pos + new Vector2((float)index1, (float)index2), Color.Black, 0.0f, new Vector2(anchorx, anchory) * spriteFont.MeasureString(text), scale, SpriteEffects.None, 0.0f);
            }
            sb.DrawString(spriteFont, text, pos, color, 0.0f, new Vector2(anchorx, anchory) * spriteFont.MeasureString(text), scale, SpriteEffects.None, 0.0f);
            return spriteFont.MeasureString(text) * scale;
        }

        public static Vector2 DrawBorderStringBig(SpriteBatch sb, string text, Vector2 pos, Color color, float scale = 1f, float anchorx = 0.0f, float anchory = 0.0f, int stringLimit = -1)
        {
            if (stringLimit != -1 && text.Length > stringLimit)
                text.Substring(0, stringLimit);
            SpriteFont spriteFont = Main.fontDeathText;
            for (int index1 = -1; index1 < 2; ++index1)
            {
                for (int index2 = -1; index2 < 2; ++index2)
                    sb.DrawString(spriteFont, text, pos + new Vector2((float)index1, (float)index2), Color.Black, 0.0f, new Vector2(anchorx, anchory) * spriteFont.MeasureString(text), scale, SpriteEffects.None, 0.0f);
            }
            sb.DrawString(spriteFont, text, pos, color, 0.0f, new Vector2(anchorx, anchory) * spriteFont.MeasureString(text), scale, SpriteEffects.None, 0.0f);
            return spriteFont.MeasureString(text) * scale;
        }

        public static void DrawInvBG(SpriteBatch sb, Rectangle R, Color c = default(Color))
        {
            Utils.DrawInvBG(sb, R.X, R.Y, R.Width, R.Height, c);
        }

        public static void DrawInvBG(SpriteBatch sb, float x, float y, float w, float h, Color c = default(Color))
        {
            Utils.DrawInvBG(sb, (int)x, (int)y, (int)w, (int)h, c);
        }

        public static void DrawInvBG(SpriteBatch sb, int x, int y, int w, int h, Color c = default(Color))
        {
            if (c == new Color())
                c = new Color(63, 65, 151, (int)byte.MaxValue) * 0.785f;
            Texture2D texture = Main.inventoryBack13Texture;
            if (w < 20)
                w = 20;
            if (h < 20)
                h = 20;
            sb.Draw(texture, new Rectangle(x, y, 10, 10), new Rectangle?(new Rectangle(0, 0, 10, 10)), c);
            sb.Draw(texture, new Rectangle(x + 10, y, w - 20, 10), new Rectangle?(new Rectangle(10, 0, 10, 10)), c);
            sb.Draw(texture, new Rectangle(x + w - 10, y, 10, 10), new Rectangle?(new Rectangle(texture.Width - 10, 0, 10, 10)), c);
            sb.Draw(texture, new Rectangle(x, y + 10, 10, h - 20), new Rectangle?(new Rectangle(0, 10, 10, 10)), c);
            sb.Draw(texture, new Rectangle(x + 10, y + 10, w - 20, h - 20), new Rectangle?(new Rectangle(10, 10, 10, 10)), c);
            sb.Draw(texture, new Rectangle(x + w - 10, y + 10, 10, h - 20), new Rectangle?(new Rectangle(texture.Width - 10, 10, 10, 10)), c);
            sb.Draw(texture, new Rectangle(x, y + h - 10, 10, 10), new Rectangle?(new Rectangle(0, texture.Height - 10, 10, 10)), c);
            sb.Draw(texture, new Rectangle(x + 10, y + h - 10, w - 20, 10), new Rectangle?(new Rectangle(10, texture.Height - 10, 10, 10)), c);
            sb.Draw(texture, new Rectangle(x + w - 10, y + h - 10, 10, 10), new Rectangle?(new Rectangle(texture.Width - 10, texture.Height - 10, 10, 10)), c);
        }

        public static void DrawLaser(SpriteBatch sb, Texture2D tex, Vector2 start, Vector2 end, Vector2 scale, Utils.LaserLineFraming framing)
        {
            Vector2 vector2_1 = start;
            Vector2 vector2_2 = Vector2.Normalize(end - start);
            float distanceLeft1 = (end - start).Length();
            float rotation = Utils.ToRotation(vector2_2) - 1.570796f;
            if (Utils.HasNaNs(vector2_2))
                return;
            float distanceCovered;
            Rectangle frame;
            Vector2 origin;
            Color color;
            framing(0, vector2_1, distanceLeft1, new Rectangle(), out distanceCovered, out frame, out origin, out color);
            sb.Draw(tex, vector2_1, new Rectangle?(frame), color, rotation, Utils.Size(frame) / 2f, scale, SpriteEffects.None, 0.0f);
            float distanceLeft2 = distanceLeft1 - distanceCovered * scale.Y;
            Vector2 vector2_3 = vector2_1 + vector2_2 * ((float)frame.Height - origin.Y) * scale.Y;
            if ((double)distanceLeft2 > 0.0)
            {
                float num = 0.0f;
                while ((double)num + 1.0 < (double)distanceLeft2)
                {
                    framing(1, vector2_3, distanceLeft2 - num, frame, out distanceCovered, out frame, out origin, out color);
                    if ((double)distanceLeft2 - (double)num < (double)frame.Height)
                    {
                        distanceCovered *= (distanceLeft2 - num) / (float)frame.Height;
                        frame.Height = (int)((double)distanceLeft2 - (double)num);
                    }
                    sb.Draw(tex, vector2_3, new Rectangle?(frame), color, rotation, origin, scale, SpriteEffects.None, 0.0f);
                    num += distanceCovered * scale.Y;
                    vector2_3 += vector2_2 * distanceCovered * scale.Y;
                }
            }
            framing(2, vector2_3, distanceLeft2, new Rectangle(), out distanceCovered, out frame, out origin, out color);
            sb.Draw(tex, vector2_3, new Rectangle?(frame), color, rotation, origin, scale, SpriteEffects.None, 0.0f);
        }

        public static void DrawLine(SpriteBatch spriteBatch, Point start, Point end, Color color)
        {
            Utils.DrawLine(spriteBatch, new Vector2((float)(start.X << 4), (float)(start.Y << 4)), new Vector2((float)(end.X << 4), (float)(end.Y << 4)), color);
        }

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color)
        {
            float num1 = Vector2.Distance(start, end);
            Vector2 v = (end - start) / num1;
            Vector2 vector2_1 = start;
            Vector2 vector2_2 = Main.screenPosition;
            float rotation = Utils.ToRotation(v);
            float num2 = 0.0f;
            while ((double)num2 <= (double)num1)
            {
                float num3 = num2 / num1;
                spriteBatch.Draw(Main.blackTileTexture, vector2_1 - vector2_2, new Rectangle?(), new Color(new Vector4(num3, num3, num3, 1f) * color.ToVector4()), rotation, Vector2.Zero, 0.25f, SpriteEffects.None, 0.0f);
                vector2_1 = start + num2 * v;
                num2 += 4f;
            }
        }

        public static void DrawRect(SpriteBatch spriteBatch, Rectangle rect, Color color)
        {
            Utils.DrawRect(spriteBatch, new Point(rect.X, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height), color);
        }

        public static void DrawRect(SpriteBatch spriteBatch, Point start, Point end, Color color)
        {
            Utils.DrawRect(spriteBatch, new Vector2((float)(start.X << 4), (float)(start.Y << 4)), new Vector2((float)((end.X << 4) - 4), (float)((end.Y << 4) - 4)), color);
        }

        public static void DrawRect(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color)
        {
            Utils.DrawLine(spriteBatch, start, new Vector2(start.X, end.Y), color);
            Utils.DrawLine(spriteBatch, start, new Vector2(end.X, start.Y), color);
            Utils.DrawLine(spriteBatch, end, new Vector2(start.X, end.Y), color);
            Utils.DrawLine(spriteBatch, end, new Vector2(end.X, start.Y), color);
        }

        public static void DrawRect(SpriteBatch spriteBatch, Vector2 topLeft, Vector2 topRight, Vector2 bottomRight, Vector2 bottomLeft, Color color)
        {
            Utils.DrawLine(spriteBatch, topLeft, topRight, color);
            Utils.DrawLine(spriteBatch, topRight, bottomRight, color);
            Utils.DrawLine(spriteBatch, bottomRight, bottomLeft, color);
            Utils.DrawLine(spriteBatch, bottomLeft, topLeft, color);
        }

        public static void DrawCursorSingle(SpriteBatch sb, Color color, float rot = float.NaN, float scale = 1f, Vector2 manualPosition = default(Vector2), int cursorSlot = 0, int specialMode = 0)
        {
            bool flag1 = false;
            bool flag2 = true;
            bool flag3 = true;
            Vector2 origin = Vector2.Zero;
            Vector2 position = new Vector2((float)Main.mouseX, (float)Main.mouseY);
            if (manualPosition != Vector2.Zero)
                position = manualPosition;
            if (float.IsNaN(rot))
            {
                rot = 0.0f;
            }
            else
            {
                flag1 = true;
                rot -= 2.356194f;
            }
            if (cursorSlot == 4 || cursorSlot == 5)
            {
                flag2 = false;
                origin = new Vector2(8f);
                if (flag1 && specialMode == 0)
                {
                    float num1 = rot;
                    if ((double)num1 < 0.0)
                        num1 += 6.283185f;
                    for (float num2 = 0.0f; (double)num2 < 4.0; ++num2)
                    {
                        if ((double)Math.Abs(num1 - 1.570796f * num2) <= 0.785398185253143)
                        {
                            rot = 1.570796f * num2;
                            break;
                        }
                    }
                }
            }
            if (Main.ThickMouse && cursorSlot == 0 || cursorSlot == 1)
                Main.DrawThickCursor(cursorSlot == 1);
            if (flag2)
                sb.Draw(Main.cursorTextures[cursorSlot], position + Vector2.One, new Rectangle?(), Utils.MultiplyRGB(color, new Color(0.2f, 0.2f, 0.2f, 0.5f)), rot, origin, scale * 1.1f, SpriteEffects.None, 0.0f);
            if (!flag3)
                return;
            sb.Draw(Main.cursorTextures[cursorSlot], position, new Rectangle?(), color, rot, origin, scale, SpriteEffects.None, 0.0f);
        }

        public delegate bool PerLinePoint(int x, int y);

        public delegate void LaserLineFraming(int stage, Vector2 currentPosition, float distanceLeft, Rectangle lastFrame, out float distanceCovered, out Rectangle frame, out Vector2 origin, out Color color);
    }
}
