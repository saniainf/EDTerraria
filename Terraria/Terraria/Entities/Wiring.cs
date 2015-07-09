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
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria
{
    public static class Wiring
    {
        private const int MaxPump = 20;
        private const int MaxMech = 1000;
        public static bool running;
        private static Dictionary<Point16, bool> _wireSkip;
        private static DoubleStack<Point16> _wireList;
        private static Dictionary<Point16, byte> _toProcess;
        private static Vector2[] _teleport;
        private static int[] _inPumpX;
        private static int[] _inPumpY;
        private static int _numInPump;
        private static int[] _outPumpX;
        private static int[] _outPumpY;
        private static int _numOutPump;
        private static int[] _mechX;
        private static int[] _mechY;
        private static int _numMechs;
        private static int[] _mechTime;

        public static void Initialize()
        {
            Wiring._wireSkip = new Dictionary<Point16, bool>();
            Wiring._wireList = new DoubleStack<Point16>(1024, 0);
            Wiring._toProcess = new Dictionary<Point16, byte>();
            Wiring._inPumpX = new int[20];
            Wiring._inPumpY = new int[20];
            Wiring._outPumpX = new int[20];
            Wiring._outPumpY = new int[20];
            Wiring._teleport = new Vector2[2];
            Wiring._mechX = new int[1000];
            Wiring._mechY = new int[1000];
            Wiring._mechTime = new int[1000];
        }

        public static void SkipWire(int x, int y)
        {
            Wiring._wireSkip[new Point16(x, y)] = true;
        }

        public static void SkipWire(Point16 point)
        {
            Wiring._wireSkip[point] = true;
        }

        public static void UpdateMech()
        {
            for (int index1 = Wiring._numMechs - 1; index1 >= 0; --index1)
            {
                --Wiring._mechTime[index1];
                if (Main.tile[Wiring._mechX[index1], Wiring._mechY[index1]].active() && (int)Main.tile[Wiring._mechX[index1], Wiring._mechY[index1]].type == 144)
                {
                    if ((int)Main.tile[Wiring._mechX[index1], Wiring._mechY[index1]].frameY == 0)
                    {
                        Wiring._mechTime[index1] = 0;
                    }
                    else
                    {
                        int num = (int)Main.tile[Wiring._mechX[index1], Wiring._mechY[index1]].frameX / 18;
                        switch (num)
                        {
                            case 0:
                                num = 60;
                                break;
                            case 1:
                                num = 180;
                                break;
                            case 2:
                                num = 300;
                                break;
                        }
                        if (Math.IEEERemainder((double)Wiring._mechTime[index1], (double)num) == 0.0)
                        {
                            Wiring._mechTime[index1] = 18000;
                            Wiring.TripWire(Wiring._mechX[index1], Wiring._mechY[index1], 1, 1);
                        }
                    }
                }
                if (Wiring._mechTime[index1] <= 0)
                {
                    if (Main.tile[Wiring._mechX[index1], Wiring._mechY[index1]].active() && (int)Main.tile[Wiring._mechX[index1], Wiring._mechY[index1]].type == 144)
                    {
                        Main.tile[Wiring._mechX[index1], Wiring._mechY[index1]].frameY = (short)0;
                        NetMessage.SendTileSquare(-1, Wiring._mechX[index1], Wiring._mechY[index1], 1);
                    }
                    if (Main.tile[Wiring._mechX[index1], Wiring._mechY[index1]].active() && (int)Main.tile[Wiring._mechX[index1], Wiring._mechY[index1]].type == 411)
                    {
                        Tile tile = Main.tile[Wiring._mechX[index1], Wiring._mechY[index1]];
                        int num1 = (int)tile.frameX % 36 / 18;
                        int num2 = (int)tile.frameY % 36 / 18;
                        int tileX = Wiring._mechX[index1] - num1;
                        int tileY = Wiring._mechY[index1] - num2;
                        int num3 = 36;
                        if ((int)Main.tile[tileX, tileY].frameX >= 36)
                            num3 = -36;
                        for (int index2 = tileX; index2 < tileX + 2; ++index2)
                        {
                            for (int index3 = tileY; index3 < tileY + 2; ++index3)
                                Main.tile[index2, index3].frameX = (short)((int)Main.tile[index2, index3].frameX + num3);
                        }
                        NetMessage.SendTileSquare(-1, tileX, tileY, 2);
                    }
                    for (int index2 = index1; index2 < Wiring._numMechs; ++index2)
                    {
                        Wiring._mechX[index2] = Wiring._mechX[index2 + 1];
                        Wiring._mechY[index2] = Wiring._mechY[index2 + 1];
                        Wiring._mechTime[index2] = Wiring._mechTime[index2 + 1];
                    }
                    --Wiring._numMechs;
                }
            }
        }

        public static void HitSwitch(int i, int j)
        {
            if (Main.tile[i, j] == null)
                return;
            if ((int)Main.tile[i, j].type == 135 || (int)Main.tile[i, j].type == 314)
            {
                Main.PlaySound(28, i * 16, j * 16, 0);
                Wiring.TripWire(i, j, 1, 1);
            }
            else if ((int)Main.tile[i, j].type == 136)
            {
                Main.tile[i, j].frameY = (int)Main.tile[i, j].frameY != 0 ? (short)0 : (short)18;
                Main.PlaySound(28, i * 16, j * 16, 0);
                Wiring.TripWire(i, j, 1, 1);
            }
            else if ((int)Main.tile[i, j].type == 144)
            {
                if ((int)Main.tile[i, j].frameY == 0)
                {
                    Main.tile[i, j].frameY = (short)18;
                    if (Main.netMode != 1)
                        Wiring.CheckMech(i, j, 18000);
                }
                else
                    Main.tile[i, j].frameY = (short)0;
                Main.PlaySound(28, i * 16, j * 16, 0);
            }
            else
            {
                if ((int)Main.tile[i, j].type != 132 && (int)Main.tile[i, j].type != 411)
                    return;
                short num1 = (short)36;
                int num2 = (int)Main.tile[i, j].frameX / 18 * -1;
                int num3 = (int)Main.tile[i, j].frameY / 18 * -1;
                int num4 = num2 % 4;
                if (num4 < -1)
                {
                    num4 += 2;
                    num1 = (short)-36;
                }
                int index1 = num4 + i;
                int index2 = num3 + j;
                if (Main.netMode != 1 && (int)Main.tile[index1, index2].type == 411)
                    Wiring.CheckMech(index1, index2, 60);
                for (int index3 = index1; index3 < index1 + 2; ++index3)
                {
                    for (int index4 = index2; index4 < index2 + 2; ++index4)
                    {
                        if ((int)Main.tile[index3, index4].type == 132 || (int)Main.tile[index3, index4].type == 411)
                            Main.tile[index3, index4].frameX += num1;
                    }
                }
                WorldGen.TileFrame(index1, index2, false, false);
                Main.PlaySound(28, i * 16, j * 16, 0);
                Wiring.TripWire(index1, index2, 2, 2);
            }
        }

        private static bool CheckMech(int i, int j, int time)
        {
            for (int index = 0; index < Wiring._numMechs; ++index)
            {
                if (Wiring._mechX[index] == i && Wiring._mechY[index] == j)
                    return false;
            }
            if (Wiring._numMechs >= 999)
                return false;
            Wiring._mechX[Wiring._numMechs] = i;
            Wiring._mechY[Wiring._numMechs] = j;
            Wiring._mechTime[Wiring._numMechs] = time;
            ++Wiring._numMechs;
            return true;
        }

        private static void XferWater()
        {
            for (int index1 = 0; index1 < Wiring._numInPump; ++index1)
            {
                int i1 = Wiring._inPumpX[index1];
                int j1 = Wiring._inPumpY[index1];
                int num1 = (int)Main.tile[i1, j1].liquid;
                if (num1 > 0)
                {
                    bool lava = Main.tile[i1, j1].lava();
                    bool honey = Main.tile[i1, j1].honey();
                    for (int index2 = 0; index2 < Wiring._numOutPump; ++index2)
                    {
                        int i2 = Wiring._outPumpX[index2];
                        int j2 = Wiring._outPumpY[index2];
                        int num2 = (int)Main.tile[i2, j2].liquid;
                        if (num2 < (int)byte.MaxValue)
                        {
                            bool flag1 = Main.tile[i2, j2].lava();
                            bool flag2 = Main.tile[i2, j2].honey();
                            if (num2 == 0)
                            {
                                flag1 = lava;
                                flag2 = honey;
                            }
                            if (lava == flag1 && honey == flag2)
                            {
                                int num3 = num1;
                                if (num3 + num2 > (int)byte.MaxValue)
                                    num3 = (int)byte.MaxValue - num2;
                                Main.tile[i2, j2].liquid += (byte)num3;
                                Main.tile[i1, j1].liquid -= (byte)num3;
                                num1 = (int)Main.tile[i1, j1].liquid;
                                Main.tile[i2, j2].lava(lava);
                                Main.tile[i2, j2].honey(honey);
                                WorldGen.SquareTileFrame(i2, j2, true);
                                if ((int)Main.tile[i1, j1].liquid == 0)
                                {
                                    Main.tile[i1, j1].lava(false);
                                    WorldGen.SquareTileFrame(i1, j1, true);
                                    break;
                                }
                            }
                        }
                    }
                    WorldGen.SquareTileFrame(i1, j1, true);
                }
            }
        }

        private static void TripWire(int left, int top, int width, int height)
        {
            if (Main.netMode == 1)
                return;
            Wiring.running = true;
            if (Wiring._wireList.Count != 0)
                Wiring._wireList.Clear(true);
            Point16 back;
            for (int X = left; X < left + width; ++X)
            {
                for (int Y = top; Y < top + height; ++Y)
                {
                    back = new Point16(X, Y);
                    Tile tile = Main.tile[X, Y];
                    if (tile != null && tile.wire())
                        Wiring._wireList.PushBack(back);
                }
            }
            Vector2[] vector2Array = new Vector2[6];
            Wiring._teleport[0].X = -1f;
            Wiring._teleport[0].Y = -1f;
            Wiring._teleport[1].X = -1f;
            Wiring._teleport[1].Y = -1f;
            if (Wiring._wireList.Count > 0)
            {
                Wiring._numInPump = 0;
                Wiring._numOutPump = 0;
                Wiring.HitWire(Wiring._wireList, 1);
                if (Wiring._numInPump > 0 && Wiring._numOutPump > 0)
                    Wiring.XferWater();
            }
            for (int X = left; X < left + width; ++X)
            {
                for (int Y = top; Y < top + height; ++Y)
                {
                    back = new Point16(X, Y);
                    Tile tile = Main.tile[X, Y];
                    if (tile != null && tile.wire2())
                        Wiring._wireList.PushBack(back);
                }
            }
            vector2Array[0] = Wiring._teleport[0];
            vector2Array[1] = Wiring._teleport[1];
            Wiring._teleport[0].X = -1f;
            Wiring._teleport[0].Y = -1f;
            Wiring._teleport[1].X = -1f;
            Wiring._teleport[1].Y = -1f;
            if (Wiring._wireList.Count > 0)
            {
                Wiring._numInPump = 0;
                Wiring._numOutPump = 0;
                Wiring.HitWire(Wiring._wireList, 2);
                if (Wiring._numInPump > 0 && Wiring._numOutPump > 0)
                    Wiring.XferWater();
            }
            vector2Array[2] = Wiring._teleport[0];
            vector2Array[3] = Wiring._teleport[1];
            Wiring._teleport[0].X = -1f;
            Wiring._teleport[0].Y = -1f;
            Wiring._teleport[1].X = -1f;
            Wiring._teleport[1].Y = -1f;
            for (int X = left; X < left + width; ++X)
            {
                for (int Y = top; Y < top + height; ++Y)
                {
                    back = new Point16(X, Y);
                    Tile tile = Main.tile[X, Y];
                    if (tile != null && tile.wire3())
                        Wiring._wireList.PushBack(back);
                }
            }
            if (Wiring._wireList.Count > 0)
            {
                Wiring._numInPump = 0;
                Wiring._numOutPump = 0;
                Wiring.HitWire(Wiring._wireList, 3);
                if (Wiring._numInPump > 0 && Wiring._numOutPump > 0)
                    Wiring.XferWater();
            }
            vector2Array[4] = Wiring._teleport[0];
            vector2Array[5] = Wiring._teleport[1];
            int index = 0;
            while (index < 5)
            {
                Wiring._teleport[0] = vector2Array[index];
                Wiring._teleport[1] = vector2Array[index + 1];
                if ((double)Wiring._teleport[0].X >= 0.0 && (double)Wiring._teleport[1].X >= 0.0)
                    Wiring.Teleport();
                index += 2;
            }
        }

        private static void HitWire(DoubleStack<Point16> next, int wireType)
        {
            for (int index = 0; index < next.Count; ++index)
            {
                Point16 point16 = next.PopFront();
                Wiring.SkipWire(point16);
                Wiring._toProcess.Add(point16, (byte)4);
                next.PushBack(point16);
            }
            while (next.Count > 0)
            {
                Point16 key = next.PopFront();
                int i = (int)key.X;
                int j = (int)key.Y;
                if (!Wiring._wireSkip.ContainsKey(key))
                    Wiring.HitWireSingle(i, j);
                for (int index1 = 0; index1 < 4; ++index1)
                {
                    int X;
                    int Y;
                    switch (index1)
                    {
                        case 0:
                            X = i;
                            Y = j + 1;
                            break;
                        case 1:
                            X = i;
                            Y = j - 1;
                            break;
                        case 2:
                            X = i + 1;
                            Y = j;
                            break;
                        case 3:
                            X = i - 1;
                            Y = j;
                            break;
                        default:
                            X = i;
                            Y = j + 1;
                            break;
                    }
                    if (X >= 2 && X < Main.maxTilesX - 2 && (Y >= 2 && Y < Main.maxTilesY - 2))
                    {
                        Tile tile = Main.tile[X, Y];
                        if (tile != null)
                        {
                            bool flag;
                            switch (wireType)
                            {
                                case 1:
                                    flag = tile.wire();
                                    break;
                                case 2:
                                    flag = tile.wire2();
                                    break;
                                case 3:
                                    flag = tile.wire3();
                                    break;
                                default:
                                    flag = false;
                                    break;
                            }
                            if (flag)
                            {
                                Point16 index2 = new Point16(X, Y);
                                byte num;
                                if (Wiring._toProcess.TryGetValue(index2, out num))
                                {
                                    --num;
                                    if ((int)num == 0)
                                        Wiring._toProcess.Remove(index2);
                                    else
                                        Wiring._toProcess[index2] = num;
                                }
                                else
                                {
                                    next.PushBack(index2);
                                    Wiring._toProcess.Add(index2, (byte)3);
                                }
                            }
                        }
                    }
                }
            }
            Wiring._wireSkip.Clear();
            Wiring._toProcess.Clear();
            Wiring.running = false;
        }

        private static void HitWireSingle(int i, int j)
        {
            Tile tile1 = Main.tile[i, j];
            int num1 = (int)tile1.type;
            if (tile1.active() && num1 >= (int)byte.MaxValue && num1 <= 268)
            {
                if (num1 >= 262)
                    tile1.type -= (ushort)7;
                else
                    tile1.type += (ushort)7;
                NetMessage.SendTileSquare(-1, i, j, 1);
            }
            if (tile1.actuator() && (num1 != 226 || (double)j <= Main.worldSurface || NPC.downedPlantBoss))
            {
                if (tile1.inActive())
                    Wiring.ReActive(i, j);
                else
                    Wiring.DeActive(i, j);
            }
            if (!tile1.active())
                return;
            if (num1 == 144)
            {
                Wiring.HitSwitch(i, j);
                WorldGen.SquareTileFrame(i, j, true);
                NetMessage.SendTileSquare(-1, i, j, 1);
            }
            else if (num1 == 406)
            {
                int num2 = (int)tile1.frameX % 54 / 18;
                int num3 = (int)tile1.frameY % 54 / 18;
                int index1 = i - num2;
                int index2 = j - num3;
                int num4 = 54;
                if ((int)Main.tile[index1, index2].frameY >= 108)
                    num4 = -108;
                for (int x = index1; x < index1 + 3; ++x)
                {
                    for (int y = index2; y < index2 + 3; ++y)
                    {
                        Wiring.SkipWire(x, y);
                        Main.tile[x, y].frameY = (short)((int)Main.tile[x, y].frameY + num4);
                    }
                }
                NetMessage.SendTileSquare(-1, index1 + 1, index2 + 1, 3);
            }
            else if (num1 == 411)
            {
                int num2 = (int)tile1.frameX % 36 / 18;
                int num3 = (int)tile1.frameY % 36 / 18;
                int tileX = i - num2;
                int tileY = j - num3;
                int num4 = 36;
                if ((int)Main.tile[tileX, tileY].frameX >= 36)
                    num4 = -36;
                for (int x = tileX; x < tileX + 2; ++x)
                {
                    for (int y = tileY; y < tileY + 2; ++y)
                    {
                        Wiring.SkipWire(x, y);
                        Main.tile[x, y].frameX = (short)((int)Main.tile[x, y].frameX + num4);
                    }
                }
                NetMessage.SendTileSquare(-1, tileX, tileY, 2);
            }
            else if (num1 == 405)
            {
                int num2 = (int)tile1.frameX % 54 / 18;
                int num3 = (int)tile1.frameY % 36 / 18;
                int index1 = i - num2;
                int index2 = j - num3;
                int num4 = 54;
                if ((int)Main.tile[index1, index2].frameX >= 54)
                    num4 = -54;
                for (int x = index1; x < index1 + 3; ++x)
                {
                    for (int y = index2; y < index2 + 2; ++y)
                    {
                        Wiring.SkipWire(x, y);
                        Main.tile[x, y].frameX = (short)((int)Main.tile[x, y].frameX + num4);
                    }
                }
                NetMessage.SendTileSquare(-1, index1 + 1, index2 + 1, 3);
            }
            else if (num1 == 215)
            {
                int num2 = (int)tile1.frameX % 54 / 18;
                int num3 = (int)tile1.frameY % 36 / 18;
                int index1 = i - num2;
                int index2 = j - num3;
                int num4 = 36;
                if ((int)Main.tile[index1, index2].frameY >= 36)
                    num4 = -36;
                for (int x = index1; x < index1 + 3; ++x)
                {
                    for (int y = index2; y < index2 + 2; ++y)
                    {
                        Wiring.SkipWire(x, y);
                        Main.tile[x, y].frameY = (short)((int)Main.tile[x, y].frameY + num4);
                    }
                }
                NetMessage.SendTileSquare(-1, index1 + 1, index2 + 1, 3);
            }
            else if (num1 == 130)
            {
                if (Main.tile[i, j - 1] != null && Main.tile[i, j - 1].active() && ((int)Main.tile[i, j - 1].type == 21 || (int)Main.tile[i, j - 1].type == 88))
                    return;
                tile1.type = (ushort)131;
                WorldGen.SquareTileFrame(i, j, true);
                NetMessage.SendTileSquare(-1, i, j, 1);
            }
            else if (num1 == 131)
            {
                tile1.type = (ushort)130;
                WorldGen.SquareTileFrame(i, j, true);
                NetMessage.SendTileSquare(-1, i, j, 1);
            }
            else if (num1 == 387 || num1 == 386)
            {
                bool flag = num1 == 387;
                int num2 = Utils.ToInt(WorldGen.ShiftTrapdoor(i, j, true, -1));
                if (num2 == 0)
                    num2 = -Utils.ToInt(WorldGen.ShiftTrapdoor(i, j, false, -1));
                if (num2 == 0)
                    return;
                NetMessage.SendData(19, -1, -1, "", 2 + Utils.ToInt(flag), (float)i, (float)j, (float)num2, 0, 0, 0);
            }
            else if (num1 == 389 || num1 == 388)
            {
                bool closing = num1 == 389;
                WorldGen.ShiftTallGate(i, j, closing);
                NetMessage.SendData(19, -1, -1, "", 4 + Utils.ToInt(closing), (float)i, (float)j, 0.0f, 0, 0, 0);
            }
            else if (num1 == 11)
            {
                if (!WorldGen.CloseDoor(i, j, true))
                    return;
                NetMessage.SendData(19, -1, -1, "", 1, (float)i, (float)j, 0.0f, 0, 0, 0);
            }
            else if (num1 == 10)
            {
                int direction = 1;
                if (Main.rand.Next(2) == 0)
                    direction = -1;
                if (!WorldGen.OpenDoor(i, j, direction))
                {
                    if (!WorldGen.OpenDoor(i, j, -direction))
                        return;
                    NetMessage.SendData(19, -1, -1, "", 0, (float)i, (float)j, (float)-direction, 0, 0, 0);
                }
                else
                    NetMessage.SendData(19, -1, -1, "", 0, (float)i, (float)j, (float)direction, 0, 0, 0);
            }
            else if (num1 == 216)
            {
                WorldGen.LaunchRocket(i, j);
                Wiring.SkipWire(i, j);
            }
            else if (num1 == 335)
            {
                int num2 = j - (int)tile1.frameY / 18;
                int num3 = i - (int)tile1.frameX / 18;
                Wiring.SkipWire(num3, num2);
                Wiring.SkipWire(num3, num2 + 1);
                Wiring.SkipWire(num3 + 1, num2);
                Wiring.SkipWire(num3 + 1, num2 + 1);
                if (!Wiring.CheckMech(num3, num2, 30))
                    return;
                WorldGen.LaunchRocketSmall(num3, num2);
            }
            else if (num1 == 338)
            {
                int num2 = j - (int)tile1.frameY / 18;
                int num3 = i - (int)tile1.frameX / 18;
                Wiring.SkipWire(num3, num2);
                Wiring.SkipWire(num3, num2 + 1);
                if (!Wiring.CheckMech(num3, num2, 30))
                    return;
                bool flag = false;
                for (int index = 0; index < 1000; ++index)
                {
                    if (Main.projectile[index].active && Main.projectile[index].aiStyle == 73 && ((double)Main.projectile[index].ai[0] == (double)num3 && (double)Main.projectile[index].ai[1] == (double)num2))
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                    return;
                Projectile.NewProjectile((float)(num3 * 16 + 8), (float)(num2 * 16 + 2), 0.0f, 0.0f, 419 + Main.rand.Next(4), 0, 0.0f, Main.myPlayer, (float)num3, (float)num2);
            }
            else if (num1 == 235)
            {
                int num2 = i - (int)tile1.frameX / 18;
                if ((int)tile1.wall == 87 && (double)j > Main.worldSurface && !NPC.downedPlantBoss)
                    return;
                if ((double)Wiring._teleport[0].X == -1.0)
                {
                    Wiring._teleport[0].X = (float)num2;
                    Wiring._teleport[0].Y = (float)j;
                    if (!tile1.halfBrick())
                        return;
                    Wiring._teleport[0].Y += 0.5f;
                }
                else
                {
                    if ((double)Wiring._teleport[0].X == (double)num2 && (double)Wiring._teleport[0].Y == (double)j)
                        return;
                    Wiring._teleport[1].X = (float)num2;
                    Wiring._teleport[1].Y = (float)j;
                    if (!tile1.halfBrick())
                        return;
                    Wiring._teleport[1].Y += 0.5f;
                }
            }
            else if (num1 == 4)
            {
                if ((int)tile1.frameX < 66)
                    tile1.frameX += (short)66;
                else
                    tile1.frameX -= (short)66;
                NetMessage.SendTileSquare(-1, i, j, 1);
            }
            else if (num1 == 149)
            {
                if ((int)tile1.frameX < 54)
                    tile1.frameX += (short)54;
                else
                    tile1.frameX -= (short)54;
                NetMessage.SendTileSquare(-1, i, j, 1);
            }
            else if (num1 == 244)
            {
                int num2 = (int)tile1.frameX / 18;
                while (num2 >= 3)
                    num2 -= 3;
                int num3 = (int)tile1.frameY / 18;
                while (num3 >= 3)
                    num3 -= 3;
                int index1 = i - num2;
                int index2 = j - num3;
                int num4 = 54;
                if ((int)Main.tile[index1, index2].frameX >= 54)
                    num4 = -54;
                for (int x = index1; x < index1 + 3; ++x)
                {
                    for (int y = index2; y < index2 + 2; ++y)
                    {
                        Wiring.SkipWire(x, y);
                        Main.tile[x, y].frameX = (short)((int)Main.tile[x, y].frameX + num4);
                    }
                }
                NetMessage.SendTileSquare(-1, index1 + 1, index2 + 1, 3);
            }
            else if (num1 == 42)
            {
                int num2 = (int)tile1.frameY / 18;
                while (num2 >= 2)
                    num2 -= 2;
                int y = j - num2;
                short num3 = (short)18;
                if ((int)tile1.frameX > 0)
                    num3 = (short)-18;
                Main.tile[i, y].frameX += num3;
                Main.tile[i, y + 1].frameX += num3;
                Wiring.SkipWire(i, y);
                Wiring.SkipWire(i, y + 1);
                NetMessage.SendTileSquare(-1, i, j, 2);
            }
            else if (num1 == 93)
            {
                int num2 = (int)tile1.frameY / 18;
                while (num2 >= 3)
                    num2 -= 3;
                int y = j - num2;
                short num3 = (short)18;
                if ((int)tile1.frameX > 0)
                    num3 = (short)-18;
                Main.tile[i, y].frameX += num3;
                Main.tile[i, y + 1].frameX += num3;
                Main.tile[i, y + 2].frameX += num3;
                Wiring.SkipWire(i, y);
                Wiring.SkipWire(i, y + 1);
                Wiring.SkipWire(i, y + 2);
                NetMessage.SendTileSquare(-1, i, y + 1, 3);
            }
            else if (num1 == 126 || num1 == 95 || (num1 == 100 || num1 == 173))
            {
                int num2 = (int)tile1.frameY / 18;
                while (num2 >= 2)
                    num2 -= 2;
                int index1 = j - num2;
                int num3 = (int)tile1.frameX / 18;
                if (num3 > 1)
                    num3 -= 2;
                int index2 = i - num3;
                short num4 = (short)36;
                if ((int)Main.tile[index2, index1].frameX > 0)
                    num4 = (short)-36;
                Main.tile[index2, index1].frameX += num4;
                Main.tile[index2, index1 + 1].frameX += num4;
                Main.tile[index2 + 1, index1].frameX += num4;
                Main.tile[index2 + 1, index1 + 1].frameX += num4;
                Wiring.SkipWire(index2, index1);
                Wiring.SkipWire(index2 + 1, index1);
                Wiring.SkipWire(index2, index1 + 1);
                Wiring.SkipWire(index2 + 1, index1 + 1);
                NetMessage.SendTileSquare(-1, index2, index1, 3);
            }
            else if (num1 == 34)
            {
                int num2 = (int)tile1.frameY / 18;
                while (num2 >= 3)
                    num2 -= 3;
                int index1 = j - num2;
                int num3 = (int)tile1.frameX / 18;
                if (num3 > 2)
                    num3 -= 3;
                int index2 = i - num3;
                short num4 = (short)54;
                if ((int)Main.tile[index2, index1].frameX > 0)
                    num4 = (short)-54;
                for (int x = index2; x < index2 + 3; ++x)
                {
                    for (int y = index1; y < index1 + 3; ++y)
                    {
                        Main.tile[x, y].frameX += num4;
                        Wiring.SkipWire(x, y);
                    }
                }
                NetMessage.SendTileSquare(-1, index2 + 1, index1 + 1, 3);
            }
            else if (num1 == 314)
            {
                if (!Wiring.CheckMech(i, j, 5))
                    return;
                Minecart.FlipSwitchTrack(i, j);
            }
            else if (num1 == 33 || num1 == 174)
            {
                short num2 = (short)18;
                if ((int)tile1.frameX > 0)
                    num2 = (short)-18;
                tile1.frameX += num2;
                NetMessage.SendTileSquare(-1, i, j, 3);
            }
            else if (num1 == 92)
            {
                int num2 = j - (int)tile1.frameY / 18;
                short num3 = (short)18;
                if ((int)tile1.frameX > 0)
                    num3 = (short)-18;
                for (int y = num2; y < num2 + 6; ++y)
                {
                    Main.tile[i, y].frameX += num3;
                    Wiring.SkipWire(i, y);
                }
                NetMessage.SendTileSquare(-1, i, num2 + 3, 7);
            }
            else if (num1 == 137)
            {
                int num2 = (int)tile1.frameY / 18;
                Vector2 vector2 = Vector2.Zero;
                float SpeedX = 0.0f;
                float SpeedY = 0.0f;
                int Type = 0;
                int Damage = 0;
                switch (num2)
                {
                    case 0:
                        if (Wiring.CheckMech(i, j, 200))
                        {
                            int num3 = -1;
                            if ((int)tile1.frameX != 0)
                                num3 = 1;
                            SpeedX = (float)(12 * num3);
                            Damage = 20;
                            Type = 98;
                            vector2 = new Vector2((float)(i * 16 + 8), (float)(j * 16 + 7));
                            vector2.X += (float)(10 * num3);
                            vector2.Y += 2f;
                            break;
                        }
                        break;
                    case 1:
                        if (Wiring.CheckMech(i, j, 200))
                        {
                            int num3 = -1;
                            if ((int)tile1.frameX != 0)
                                num3 = 1;
                            SpeedX = (float)(12 * num3);
                            Damage = 40;
                            Type = 184;
                            vector2 = new Vector2((float)(i * 16 + 8), (float)(j * 16 + 7));
                            vector2.X += (float)(10 * num3);
                            vector2.Y += 2f;
                            break;
                        }
                        break;
                    case 2:
                        if (Wiring.CheckMech(i, j, 200))
                        {
                            int num3 = -1;
                            if ((int)tile1.frameX != 0)
                                num3 = 1;
                            SpeedX = (float)(5 * num3);
                            Damage = 40;
                            Type = 187;
                            vector2 = new Vector2((float)(i * 16 + 8), (float)(j * 16 + 7));
                            vector2.X += (float)(10 * num3);
                            vector2.Y += 2f;
                            break;
                        }
                        break;
                    case 3:
                        if (Wiring.CheckMech(i, j, 300))
                        {
                            Type = 185;
                            int num3 = 200;
                            for (int index = 0; index < 1000; ++index)
                            {
                                if (Main.projectile[index].active && Main.projectile[index].type == Type)
                                {
                                    float num4 = (new Vector2((float)(i * 16 + 8), (float)(j * 18 + 8)) - Main.projectile[index].Center).Length();
                                    if ((double)num4 < 50.0)
                                        num3 -= 50;
                                    else if ((double)num4 < 100.0)
                                        num3 -= 15;
                                    else if ((double)num4 < 200.0)
                                        num3 -= 10;
                                    else if ((double)num4 < 300.0)
                                        num3 -= 8;
                                    else if ((double)num4 < 400.0)
                                        num3 -= 6;
                                    else if ((double)num4 < 500.0)
                                        num3 -= 5;
                                    else if ((double)num4 < 700.0)
                                        num3 -= 4;
                                    else if ((double)num4 < 900.0)
                                        num3 -= 3;
                                    else if ((double)num4 < 1200.0)
                                        num3 -= 2;
                                    else
                                        --num3;
                                }
                            }
                            if (num3 > 0)
                            {
                                SpeedX = (float)Main.rand.Next(-20, 21) * 0.05f;
                                SpeedY = (float)(4.0 + (double)Main.rand.Next(0, 21) * 0.0500000007450581);
                                Damage = 40;
                                vector2 = new Vector2((float)(i * 16 + 8), (float)(j * 16 + 16));
                                vector2.Y += 6f;
                                Projectile.NewProjectile((float)(int)vector2.X, (float)(int)vector2.Y, SpeedX, SpeedY, Type, Damage, 2f, Main.myPlayer, 0.0f, 0.0f);
                                break;
                            }
                            break;
                        }
                        break;
                    case 4:
                        if (Wiring.CheckMech(i, j, 90))
                        {
                            SpeedX = 0.0f;
                            SpeedY = 8f;
                            Damage = 60;
                            Type = 186;
                            vector2 = new Vector2((float)(i * 16 + 8), (float)(j * 16 + 16));
                            vector2.Y += 10f;
                            break;
                        }
                        break;
                }
                if (Type == 0)
                    return;
                Projectile.NewProjectile((float)(int)vector2.X, (float)(int)vector2.Y, SpeedX, SpeedY, Type, Damage, 2f, Main.myPlayer, 0.0f, 0.0f);
            }
            else if (num1 == 139 || num1 == 35)
                WorldGen.SwitchMB(i, j);
            else if (num1 == 207)
                WorldGen.SwitchFountain(i, j);
            else if (num1 == 410)
                WorldGen.SwitchMonolith(i, j);
            else if (num1 == 141)
            {
                WorldGen.KillTile(i, j, false, false, true);
                NetMessage.SendTileSquare(-1, i, j, 1);
                Projectile.NewProjectile((float)(i * 16 + 8), (float)(j * 16 + 8), 0.0f, 0.0f, 108, 500, 10f, Main.myPlayer, 0.0f, 0.0f);
            }
            else if (num1 == 210)
                WorldGen.ExplodeMine(i, j);
            else if (num1 == 142 || num1 == 143)
            {
                int y = j - (int)tile1.frameY / 18;
                int num2 = (int)tile1.frameX / 18;
                if (num2 > 1)
                    num2 -= 2;
                int x = i - num2;
                Wiring.SkipWire(x, y);
                Wiring.SkipWire(x, y + 1);
                Wiring.SkipWire(x + 1, y);
                Wiring.SkipWire(x + 1, y + 1);
                if (num1 == 142)
                {
                    for (int index = 0; index < 4 && Wiring._numInPump < 19; ++index)
                    {
                        int num3;
                        int num4;
                        if (index == 0)
                        {
                            num3 = x;
                            num4 = y + 1;
                        }
                        else if (index == 1)
                        {
                            num3 = x + 1;
                            num4 = y + 1;
                        }
                        else if (index == 2)
                        {
                            num3 = x;
                            num4 = y;
                        }
                        else
                        {
                            num3 = x + 1;
                            num4 = y;
                        }
                        Wiring._inPumpX[Wiring._numInPump] = num3;
                        Wiring._inPumpY[Wiring._numInPump] = num4;
                        ++Wiring._numInPump;
                    }
                }
                else
                {
                    for (int index = 0; index < 4 && Wiring._numOutPump < 19; ++index)
                    {
                        int num3;
                        int num4;
                        if (index == 0)
                        {
                            num3 = x;
                            num4 = y + 1;
                        }
                        else if (index == 1)
                        {
                            num3 = x + 1;
                            num4 = y + 1;
                        }
                        else if (index == 2)
                        {
                            num3 = x;
                            num4 = y;
                        }
                        else
                        {
                            num3 = x + 1;
                            num4 = y;
                        }
                        Wiring._outPumpX[Wiring._numOutPump] = num3;
                        Wiring._outPumpY[Wiring._numOutPump] = num4;
                        ++Wiring._numOutPump;
                    }
                }
            }
            else if (num1 == 105)
            {
                int num2 = j - (int)tile1.frameY / 18;
                int num3 = (int)tile1.frameX / 18;
                int num4 = 0;
                while (num3 >= 2)
                {
                    num3 -= 2;
                    ++num4;
                }
                int num5 = i - num3;
                Wiring.SkipWire(num5, num2);
                Wiring.SkipWire(num5, num2 + 1);
                Wiring.SkipWire(num5, num2 + 2);
                Wiring.SkipWire(num5 + 1, num2);
                Wiring.SkipWire(num5 + 1, num2 + 1);
                Wiring.SkipWire(num5 + 1, num2 + 2);
                int X = num5 * 16 + 16;
                int Y = (num2 + 3) * 16;
                int index1 = -1;
                if (num4 == 4)
                {
                    if (Wiring.CheckMech(num5, num2, 30) && NPC.MechSpawn((float)X, (float)Y, 1))
                        index1 = NPC.NewNPC(X, Y - 12, 1, 0, 0.0f, 0.0f, 0.0f, 0.0f, (int)byte.MaxValue);
                }
                else if (num4 == 7)
                {
                    if (Wiring.CheckMech(num5, num2, 30) && NPC.MechSpawn((float)X, (float)Y, 49))
                        index1 = NPC.NewNPC(X - 4, Y - 6, 49, 0, 0.0f, 0.0f, 0.0f, 0.0f, (int)byte.MaxValue);
                }
                else if (num4 == 8)
                {
                    if (Wiring.CheckMech(num5, num2, 30) && NPC.MechSpawn((float)X, (float)Y, 55))
                        index1 = NPC.NewNPC(X, Y - 12, 55, 0, 0.0f, 0.0f, 0.0f, 0.0f, (int)byte.MaxValue);
                }
                else if (num4 == 9)
                {
                    if (Wiring.CheckMech(num5, num2, 30) && NPC.MechSpawn((float)X, (float)Y, 46))
                        index1 = NPC.NewNPC(X, Y - 12, 46, 0, 0.0f, 0.0f, 0.0f, 0.0f, (int)byte.MaxValue);
                }
                else if (num4 == 10)
                {
                    if (Wiring.CheckMech(num5, num2, 30) && NPC.MechSpawn((float)X, (float)Y, 21))
                        index1 = NPC.NewNPC(X, Y, 21, 0, 0.0f, 0.0f, 0.0f, 0.0f, (int)byte.MaxValue);
                }
                else if (num4 == 18)
                {
                    if (Wiring.CheckMech(num5, num2, 30) && NPC.MechSpawn((float)X, (float)Y, 67))
                        index1 = NPC.NewNPC(X, Y - 12, 67, 0, 0.0f, 0.0f, 0.0f, 0.0f, (int)byte.MaxValue);
                }
                else if (num4 == 23)
                {
                    if (Wiring.CheckMech(num5, num2, 30) && NPC.MechSpawn((float)X, (float)Y, 63))
                        index1 = NPC.NewNPC(X, Y - 12, 63, 0, 0.0f, 0.0f, 0.0f, 0.0f, (int)byte.MaxValue);
                }
                else if (num4 == 27)
                {
                    if (Wiring.CheckMech(num5, num2, 30) && NPC.MechSpawn((float)X, (float)Y, 85))
                        index1 = NPC.NewNPC(X - 9, Y, 85, 0, 0.0f, 0.0f, 0.0f, 0.0f, (int)byte.MaxValue);
                }
                else if (num4 == 28)
                {
                    if (Wiring.CheckMech(num5, num2, 30) && NPC.MechSpawn((float)X, (float)Y, 74))
                        index1 = NPC.NewNPC(X, Y - 12, 74, 0, 0.0f, 0.0f, 0.0f, 0.0f, (int)byte.MaxValue);
                }
                else if (num4 == 34)
                {
                    for (int index2 = 0; index2 < 2; ++index2)
                    {
                        for (int index3 = 0; index3 < 3; ++index3)
                        {
                            Tile tile2 = Main.tile[num5 + index2, num2 + index3];
                            tile2.type = (ushort)349;
                            tile2.frameX = (short)(index2 * 18 + 216);
                            tile2.frameY = (short)(index3 * 18);
                        }
                    }
                    Animation.NewTemporaryAnimation(0, (ushort)349, num5, num2);
                    if (Main.netMode == 2)
                        NetMessage.SendTileRange(-1, num5, num2, 2, 3);
                }
                else if (num4 == 42)
                {
                    if (Wiring.CheckMech(num5, num2, 30) && NPC.MechSpawn((float)X, (float)Y, 58))
                        index1 = NPC.NewNPC(X, Y - 12, 58, 0, 0.0f, 0.0f, 0.0f, 0.0f, (int)byte.MaxValue);
                }
                else if (num4 == 37)
                {
                    if (Wiring.CheckMech(num5, num2, 600) && Item.MechSpawn((float)X, (float)Y, 58) && (Item.MechSpawn((float)X, (float)Y, 1734) && Item.MechSpawn((float)X, (float)Y, 1867)))
                        Item.NewItem(X, Y - 16, 0, 0, 58, 1, false, 0, false);
                }
                else if (num4 == 50)
                {
                    if (Wiring.CheckMech(num5, num2, 30) && NPC.MechSpawn((float)X, (float)Y, 65) && !Collision.SolidTiles(num5 - 2, num5 + 3, num2, num2 + 2))
                        index1 = NPC.NewNPC(X, Y - 12, 65, 0, 0.0f, 0.0f, 0.0f, 0.0f, (int)byte.MaxValue);
                }
                else if (num4 == 2)
                {
                    if (Wiring.CheckMech(num5, num2, 600) && Item.MechSpawn((float)X, (float)Y, 184) && (Item.MechSpawn((float)X, (float)Y, 1735) && Item.MechSpawn((float)X, (float)Y, 1868)))
                        Item.NewItem(X, Y - 16, 0, 0, 184, 1, false, 0, false);
                }
                else if (num4 == 17)
                {
                    if (Wiring.CheckMech(num5, num2, 600) && Item.MechSpawn((float)X, (float)Y, 166))
                        Item.NewItem(X, Y - 20, 0, 0, 166, 1, false, 0, false);
                }
                else if (num4 == 40)
                {
                    if (Wiring.CheckMech(num5, num2, 300))
                    {
                        int[] numArray = new int[10];
                        int maxValue = 0;
                        for (int index2 = 0; index2 < 200; ++index2)
                        {
                            if (Main.npc[index2].active && (Main.npc[index2].type == 17 || Main.npc[index2].type == 19 || (Main.npc[index2].type == 22 || Main.npc[index2].type == 38) || (Main.npc[index2].type == 54 || Main.npc[index2].type == 107 || (Main.npc[index2].type == 108 || Main.npc[index2].type == 142)) || (Main.npc[index2].type == 160 || Main.npc[index2].type == 207 || (Main.npc[index2].type == 209 || Main.npc[index2].type == 227) || (Main.npc[index2].type == 228 || Main.npc[index2].type == 229 || (Main.npc[index2].type == 358 || Main.npc[index2].type == 369)))))
                            {
                                numArray[maxValue] = index2;
                                ++maxValue;
                                if (maxValue >= 9)
                                    break;
                            }
                        }
                        if (maxValue > 0)
                        {
                            int number = numArray[Main.rand.Next(maxValue)];
                            Main.npc[number].position.X = (float)(X - Main.npc[number].width / 2);
                            Main.npc[number].position.Y = (float)(Y - Main.npc[number].height - 1);
                            NetMessage.SendData(23, -1, -1, "", number, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        }
                    }
                }
                else if (num4 == 41 && Wiring.CheckMech(num5, num2, 300))
                {
                    int[] numArray = new int[10];
                    int maxValue = 0;
                    for (int index2 = 0; index2 < 200; ++index2)
                    {
                        if (Main.npc[index2].active && (Main.npc[index2].type == 18 || Main.npc[index2].type == 20 || (Main.npc[index2].type == 124 || Main.npc[index2].type == 178) || (Main.npc[index2].type == 208 || Main.npc[index2].type == 353)))
                        {
                            numArray[maxValue] = index2;
                            ++maxValue;
                            if (maxValue >= 9)
                                break;
                        }
                    }
                    if (maxValue > 0)
                    {
                        int number = numArray[Main.rand.Next(maxValue)];
                        Main.npc[number].position.X = (float)(X - Main.npc[number].width / 2);
                        Main.npc[number].position.Y = (float)(Y - Main.npc[number].height - 1);
                        NetMessage.SendData(23, -1, -1, "", number, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                    }
                }
                if (index1 < 0)
                    return;
                Main.npc[index1].value = 0.0f;
                Main.npc[index1].npcSlots = 0.0f;
            }
            else
            {
                if (num1 != 349)
                    return;
                int index1 = j - (int)tile1.frameY / 18;
                int num2 = (int)tile1.frameX / 18;
                while (num2 >= 2)
                    num2 -= 2;
                int index2 = i - num2;
                Wiring.SkipWire(index2, index1);
                Wiring.SkipWire(index2, index1 + 1);
                Wiring.SkipWire(index2, index1 + 2);
                Wiring.SkipWire(index2 + 1, index1);
                Wiring.SkipWire(index2 + 1, index1 + 1);
                Wiring.SkipWire(index2 + 1, index1 + 2);
                short num3 = (int)Main.tile[index2, index1].frameX != 0 ? (short)-216 : (short)216;
                for (int index3 = 0; index3 < 2; ++index3)
                {
                    for (int index4 = 0; index4 < 3; ++index4)
                        Main.tile[index2 + index3, index1 + index4].frameX += num3;
                }
                if (Main.netMode == 2)
                    NetMessage.SendTileRange(-1, index2, index1, 2, 3);
                Animation.NewTemporaryAnimation((int)num3 > 0 ? 0 : 1, (ushort)349, index2, index1);
            }
        }

        private static void Teleport()
        {
            if ((double)Wiring._teleport[0].X < (double)Wiring._teleport[1].X + 3.0 && (double)Wiring._teleport[0].X > (double)Wiring._teleport[1].X - 3.0 && ((double)Wiring._teleport[0].Y > (double)Wiring._teleport[1].Y - 3.0 && (double)Wiring._teleport[0].Y < (double)Wiring._teleport[1].Y))
                return;
            Rectangle[] rectangleArray = new Rectangle[2];
            rectangleArray[0].X = (int)((double)Wiring._teleport[0].X * 16.0);
            rectangleArray[0].Width = 48;
            rectangleArray[0].Height = 48;
            rectangleArray[0].Y = (int)((double)Wiring._teleport[0].Y * 16.0 - (double)rectangleArray[0].Height);
            rectangleArray[1].X = (int)((double)Wiring._teleport[1].X * 16.0);
            rectangleArray[1].Width = 48;
            rectangleArray[1].Height = 48;
            rectangleArray[1].Y = (int)((double)Wiring._teleport[1].Y * 16.0 - (double)rectangleArray[1].Height);
            for (int index1 = 0; index1 < 2; ++index1)
            {
                Vector2 vector2_1 = new Vector2((float)(rectangleArray[1].X - rectangleArray[0].X), (float)(rectangleArray[1].Y - rectangleArray[0].Y));
                if (index1 == 1)
                    vector2_1 = new Vector2((float)(rectangleArray[0].X - rectangleArray[1].X), (float)(rectangleArray[0].Y - rectangleArray[1].Y));
                for (int playerIndex = 0; playerIndex < (int)byte.MaxValue; ++playerIndex)
                {
                    if (Main.player[playerIndex].active && !Main.player[playerIndex].dead && (!Main.player[playerIndex].teleporting && rectangleArray[index1].Intersects(Main.player[playerIndex].getRect())))
                    {
                        Vector2 vector2_2 = Main.player[playerIndex].position + vector2_1;
                        Main.player[playerIndex].teleporting = true;
                        if (Main.netMode == 2)
                            RemoteClient.CheckSection(playerIndex, vector2_2, 1);
                        Main.player[playerIndex].Teleport(vector2_2, 0, 0);
                        if (Main.netMode == 2)
                            NetMessage.SendData(65, -1, -1, "", 0, (float)playerIndex, vector2_2.X, vector2_2.Y, 0, 0, 0);
                    }
                }
                for (int index2 = 0; index2 < 200; ++index2)
                {
                    if (Main.npc[index2].active && !Main.npc[index2].teleporting && (Main.npc[index2].lifeMax > 5 && !Main.npc[index2].boss) && (!Main.npc[index2].noTileCollide && rectangleArray[index1].Intersects(Main.npc[index2].getRect())))
                    {
                        Main.npc[index2].teleporting = true;
                        Main.npc[index2].Teleport(Main.npc[index2].position + vector2_1, 0, 0);
                    }
                }
            }
            for (int index = 0; index < (int)byte.MaxValue; ++index)
                Main.player[index].teleporting = false;
            for (int index = 0; index < 200; ++index)
                Main.npc[index].teleporting = false;
        }

        private static void DeActive(int i, int j)
        {
            if (!Main.tile[i, j].active())
                return;
            bool flag = Main.tileSolid[(int)Main.tile[i, j].type] && !TileID.Sets.NotReallySolid[(int)Main.tile[i, j].type];
            switch (Main.tile[i, j].type)
            {
                case (ushort)314:
                case (ushort)386:
                case (ushort)387:
                case (ushort)388:
                case (ushort)389:
                    flag = false;
                    break;
            }
            if (!flag || Main.tile[i, j - 1].active() && ((int)Main.tile[i, j - 1].type == 5 || (int)Main.tile[i, j - 1].type == 21 || ((int)Main.tile[i, j - 1].type == 26 || (int)Main.tile[i, j - 1].type == 77) || (int)Main.tile[i, j - 1].type == 72))
                return;
            Main.tile[i, j].inActive(true);
            WorldGen.SquareTileFrame(i, j, false);
            if (Main.netMode == 1)
                return;
            NetMessage.SendTileSquare(-1, i, j, 1);
        }

        private static void ReActive(int i, int j)
        {
            Main.tile[i, j].inActive(false);
            WorldGen.SquareTileFrame(i, j, false);
            if (Main.netMode == 1)
                return;
            NetMessage.SendTileSquare(-1, i, j, 1);
        }
    }
}
