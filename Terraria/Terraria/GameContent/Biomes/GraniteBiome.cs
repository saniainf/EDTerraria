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
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.World.Generation;

namespace Terraria.GameContent.Biomes
{
    internal class GraniteBiome : MicroBiome
    {
        private static Magma[,] _sourceMagmaMap = new Magma[200, 200];
        private static Magma[,] _targetMagmaMap = new Magma[200, 200];
        private const int MAX_MAGMA_ITERATIONS = 300;

        public override bool Place(Point origin, StructureMap structures)
        {
            if (GenBase._tiles[origin.X, origin.Y].active())
                return false;

            int length1 = _sourceMagmaMap.GetLength(0);
            int length2 = _sourceMagmaMap.GetLength(1);
            int index1 = length1 / 2;
            int index2 = length2 / 2;
            origin.X -= index1;
            origin.Y -= index2;
            for (int index3 = 0; index3 < length1; ++index3)
            {
                for (int index4 = 0; index4 < length2; ++index4)
                {
                    int i = index3 + origin.X;
                    int j = index4 + origin.Y;
                    _sourceMagmaMap[index3, index4] = Magma.CreateEmpty(WorldGen.SolidTile(i, j) ? 4f : 1f);
                    _targetMagmaMap[index3, index4] = _sourceMagmaMap[index3, index4];
                }
            }

            int max1 = index1;
            int min1 = index1;
            int max2 = index2;
            int min2 = index2;
            for (int index3 = 0; index3 < 300; ++index3)
            {
                for (int index4 = max1; index4 <= min1; ++index4)
                {
                    for (int index5 = max2; index5 <= min2; ++index5)
                    {
                        Magma magma1 = _sourceMagmaMap[index4, index5];
                        if (magma1.IsActive)
                        {
                            float num1 = 0.0f;
                            Vector2 zero = Vector2.Zero;
                            for (int index6 = -1; index6 <= 1; ++index6)
                            {
                                for (int index7 = -1; index7 <= 1; ++index7)
                                {
                                    if (index6 != 0 || index7 != 0)
                                    {
                                        Vector2 vector2 = new Vector2((float)index6, (float)index7);
                                        vector2.Normalize();
                                        Magma magma2 = _sourceMagmaMap[index4 + index6, index5 + index7];
                                        if ((double)magma1.Pressure > 0.00999999977648258 && !magma2.IsActive)
                                        {
                                            if (index6 == -1)
                                                max1 = Utils.Clamp<int>(index4 + index6, 1, max1);
                                            else
                                                min1 = Utils.Clamp<int>(index4 + index6, min1, length1 - 2);
                                            if (index7 == -1)
                                                max2 = Utils.Clamp<int>(index5 + index7, 1, max2);
                                            else
                                                min2 = Utils.Clamp<int>(index5 + index7, min2, length2 - 2);
                                            _targetMagmaMap[index4 + index6, index5 + index7] = magma2.ToFlow();
                                        }

                                        float num2 = magma2.Pressure;
                                        num1 += num2;
                                        zero += num2 * vector2;
                                    }
                                }
                            }

                            float num3 = num1 / 8f;
                            if ((double)num3 > (double)magma1.Resistance)
                            {
                                float num2 = zero.Length() / 8f;
                                float pressure = Math.Max(0.0f, (float)(Math.Max(num3 - num2 - magma1.Pressure, 0.0f) + (double)num2 + (double)magma1.Pressure * 0.875) - magma1.Resistance);
                                _targetMagmaMap[index4, index5] = Magma.CreateFlow(pressure, Math.Max(0.0f, magma1.Resistance - pressure * 0.02f));
                            }
                        }
                    }
                }

                if (index3 < 2)
                    _targetMagmaMap[index1, index2] = Magma.CreateFlow(25f, 0.0f);
                Utils.Swap<Magma[,]>(ref _sourceMagmaMap, ref _targetMagmaMap);
            }

            bool flag1 = origin.Y + index2 > WorldGen.lavaLine - 30;
            bool flag2 = false;
            for (int index3 = -50; index3 < 50 && !flag2; ++index3)
            {
                for (int index4 = -50; index4 < 50 && !flag2; ++index4)
                {
                    if (GenBase._tiles[origin.X + index1 + index3, origin.Y + index2 + index4].active())
                    {
                        switch (GenBase._tiles[origin.X + index1 + index3, origin.Y + index2 + index4].type)
                        {
                            case 147:
                            case 161:
                            case 162:
                            case 163:
                            case 200:
                                flag1 = false;
                                flag2 = true;
                                continue;
                            default:
                                continue;
                        }
                    }
                }
            }

            for (int index3 = max1; index3 <= min1; ++index3)
            {
                for (int index4 = max2; index4 <= min2; ++index4)
                {
                    Magma magma = _sourceMagmaMap[index3, index4];
                    if (magma.IsActive)
                    {
                        Tile tile = GenBase._tiles[origin.X + index3, origin.Y + index4];
                        if (Math.Max(1f - Math.Max(0.0f, (float)(Math.Sin((origin.Y + index4) * 0.400000005960464) * 0.699999988079071 + 1.20000004768372) *
                            (float)(0.200000002980232 + 0.5 / Math.Sqrt(Math.Max(0.0f, magma.Pressure - magma.Resistance)))), magma.Pressure / 15f) > 0.349999994039536 +
                            (WorldGen.SolidTile(origin.X + index3, origin.Y + index4) ? 0.0 : 0.5))
                        {
                            if (TileID.Sets.Ore[(int)tile.type])
                                tile.ResetToType(tile.type);
                            else
                                tile.ResetToType((ushort)368);
                            tile.wall = (byte)180;
                        }
                        else if ((double)magma.Resistance < 0.00999999977648258)
                        {
                            WorldUtils.ClearTile(origin.X + index3, origin.Y + index4, false);
                            tile.wall = (byte)180;
                        }

                        if ((int)tile.liquid > 0 && flag1)
                            tile.liquidType(1);
                    }
                }
            }

            List<Point16> list = new List<Point16>();
            for (int index3 = max1; index3 <= min1; ++index3)
            {
                for (int index4 = max2; index4 <= min2; ++index4)
                {
                    if (GraniteBiome._sourceMagmaMap[index3, index4].IsActive)
                    {
                        int num1 = 0;
                        int num2 = index3 + origin.X;
                        int num3 = index4 + origin.Y;
                        if (WorldGen.SolidTile(num2, num3))
                        {
                            for (int index5 = -1; index5 <= 1; ++index5)
                            {
                                for (int index6 = -1; index6 <= 1; ++index6)
                                {
                                    if (WorldGen.SolidTile(num2 + index5, num3 + index6))
                                        ++num1;
                                }
                            }
                            if (num1 < 3)
                                list.Add(new Point16(num2, num3));
                        }
                    }
                }
            }
            foreach (Point16 point16 in list)
            {
                int x = (int)point16.X;
                int y = (int)point16.Y;
                WorldUtils.ClearTile(x, y, true);
                GenBase._tiles[x, y].wall = (byte)180;
            }
            list.Clear();
            for (int index3 = max1; index3 <= min1; ++index3)
            {
                for (int index4 = max2; index4 <= min2; ++index4)
                {
                    Magma magma = _sourceMagmaMap[index3, index4];
                    int index5 = index3 + origin.X;
                    int index6 = index4 + origin.Y;
                    if (magma.IsActive)
                    {
                        WorldUtils.TileFrame(index5, index6, false);
                        WorldGen.SquareWallFrame(index5, index6, true);
                        if (GenBase._random.Next(8) == 0 && GenBase._tiles[index5, index6].active())
                        {
                            if (!GenBase._tiles[index5, index6 + 1].active())
                                WorldGen.PlaceTight(index5, index6 + 1, (ushort)165, false);
                            if (!GenBase._tiles[index5, index6 - 1].active())
                                WorldGen.PlaceTight(index5, index6 - 1, (ushort)165, false);
                        }
                        if (GenBase._random.Next(2) == 0)
                            Tile.SmoothSlope(index5, index6, true);
                    }
                }
            }
            return true;
        }

        private struct Magma
        {
            public readonly float Pressure;
            public readonly float Resistance;
            public readonly bool IsActive;

            private Magma(float pressure, float resistance, bool active)
            {
                Pressure = pressure;
                Resistance = resistance;
                IsActive = active;
            }

            public Magma ToFlow()
            {
                return new Magma(Pressure, Resistance, true);
            }

            public static Magma CreateFlow(float pressure, float resistance = 0.0f)
            {
                return new Magma(pressure, resistance, true);
            }

            public static Magma CreateEmpty(float resistance = 0.0f)
            {
                return new Magma(0.0f, resistance, false);
            }
        }
    }
}
