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

namespace Terraria.DataStructures
{
    public struct Point16
    {
        public static Point16 Zero = new Point16(0, 0);
        public static Point16 NegativeOne = new Point16(-1, -1);
        public readonly short X;
        public readonly short Y;

        public Point16(Point point)
        {
            X = (short)point.X;
            Y = (short)point.Y;
        }

        public Point16(int x, int y)
        {
            X = (short)x;
            Y = (short)y;
        }

        public Point16(short x, short y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(Point16 first, Point16 second)
        {
            if ((int)first.X == (int)second.X)
                return (int)first.Y == (int)second.Y;
            return false;
        }

        public static bool operator !=(Point16 first, Point16 second)
        {
            if ((int)first.X == (int)second.X)
                return (int)first.Y != (int)second.Y;
            return true;
        }

        public static Point16 Max(int firstX, int firstY, int secondX, int secondY)
        {
            return new Point16(firstX > secondX ? firstX : secondX, firstY > secondY ? firstY : secondY);
        }

        public Point16 Max(int compareX, int compareY)
        {
            return new Point16((int)X > compareX ? (int)X : compareX, (int)Y > compareY ? (int)Y : compareY);
        }

        public Point16 Max(Point16 compareTo)
        {
            return new Point16((int)X > (int)compareTo.X ? X : compareTo.X, (int)Y > (int)compareTo.Y ? Y : compareTo.Y);
        }

        public override bool Equals(object obj)
        {
            Point16 point16 = (Point16)obj;
            return (int)X == (int)point16.X && (int)Y == (int)point16.Y;
        }

        public override int GetHashCode()
        {
            return (int)X << 16 | (int)(ushort)Y;
        }

        public override string ToString()
        {
            return string.Format("{{{0}, {1}}}", X, Y);
        }
    }
}
