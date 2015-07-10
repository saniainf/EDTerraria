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
using Terraria;
using Terraria.ID;
using Terraria.World.Generation;

namespace Terraria.GameContent.Biomes
{
    internal class MarbleBiome : MicroBiome
    {
        private const int SCALE = 3;
        private Slab[,] _slabs;

        private void SmoothSlope(int x, int y)
        {
            Slab slab = _slabs[x, y];
            if (!slab.IsSolid)
                return;

            switch ((_slabs[x, y - 1].IsSolid ? 1 : 0) << 3 | (_slabs[x, y + 1].IsSolid ? 1 : 0) << 2 | (_slabs[x - 1, y].IsSolid ? 1 : 0) << 1 | (_slabs[x + 1, y].IsSolid ? 1 : 0))
            {
                case 4:
                    _slabs[x, y] = slab.WithState(new SlabState(SlabStates.HalfBrick));
                    break;
                case 5:
                    _slabs[x, y] = slab.WithState(new SlabState(SlabStates.BottomRightFilled));
                    break;
                case 6:
                    _slabs[x, y] = slab.WithState(new SlabState(SlabStates.BottomLeftFilled));
                    break;
                case 9:
                    _slabs[x, y] = slab.WithState(new SlabState(SlabStates.TopRightFilled));
                    break;
                case 10:
                    _slabs[x, y] = slab.WithState(new SlabState(SlabStates.TopLeftFilled));
                    break;
                default:
                    _slabs[x, y] = slab.WithState(new SlabState(SlabStates.Solid));
                    break;
            }
        }

        private void PlaceSlab(Slab slab, int originX, int originY, int scale)
        {
            for (int x = 0; x < scale; ++x)
            {
                for (int y = 0; y < scale; ++y)
                {
                    Tile tile = GenBase._tiles[originX + x, originY + y];
                    if (TileID.Sets.Ore[tile.type])
                        tile.ResetToType(tile.type);
                    else
                        tile.ResetToType(367);

                    bool active = slab.State(x, y, scale);
                    tile.active(active);
                    if (slab.HasWall)
                        tile.wall = 178;

                    WorldUtils.TileFrame(originX + x, originY + y, true);
                    WorldGen.SquareWallFrame(originX + x, originY + y, true);
                    Tile.SmoothSlope(originX + x, originY + y, true);
                    if (WorldGen.SolidTile(originX + x, originY + y - 1) && GenBase._random.Next(4) == 0)
                        WorldGen.PlaceTight(originX + x, originY + y, 165, false);
                    if (WorldGen.SolidTile(originX + x, originY + y) && GenBase._random.Next(4) == 0)
                        WorldGen.PlaceTight(originX + x, originY + y - 1, 165, false);
                }
            }
        }

        private bool IsGroupSolid(int x, int y, int scale)
        {
            int num = 0;
            for (int index1 = 0; index1 < scale; ++index1)
            {
                for (int index2 = 0; index2 < scale; ++index2)
                {
                    if (WorldGen.SolidOrSlopedTile(x + index1, y + index2))
                        ++num;
                }
            }

            return num > scale / 4 * 3;
        }

        public override bool Place(Point origin, StructureMap structures)
        {
            if (_slabs == null)
                _slabs = new Slab[56, 26];

            int num1 = GenBase._random.Next(80, 150) / 3;
            int num2 = GenBase._random.Next(40, 60) / 3;
            int num3 = (num2 * 3 - GenBase._random.Next(20, 30)) / 3;
            origin.X -= num1 * 3 / 2;
            origin.Y -= num2 * 3 / 2;
            for (int index1 = -1; index1 < num1 + 1; ++index1)
            {
                float num4 = (float)((index1 - num1 / 2) / num1 + 0.5);
                int num5 = (int)((0.5 - (double)Math.Abs(num4 - 0.5f)) * 5.0) - 2;
                for (int index2 = -1; index2 < num2 + 1; ++index2)
                {
                    bool hasWall = true;
                    bool flag1 = false;
                    bool flag2 = IsGroupSolid(index1 * 3 + origin.X, index2 * 3 + origin.Y, 3);
                    int num6 = Math.Abs(index2 - num2 / 2) - num3 / 4 + num5;
                    if (num6 > 3)
                    {
                        flag1 = flag2;
                        hasWall = false;
                    }
                    else if (num6 > 0)
                    {
                        flag1 = index2 - num2 / 2 > 0 || flag2;
                        hasWall = index2 - num2 / 2 < 0 || num6 <= 2;
                    }
                    else if (num6 == 0)
                        flag1 = GenBase._random.Next(2) == 0 && (index2 - num2 / 2 > 0 || flag2);

                    if ((double)Math.Abs(num4 - 0.5f) > 0.349999994039536 + (double)Utils.NextFloat(GenBase._random) * 0.100000001490116 && !flag2)
                    {
                        hasWall = false;
                        flag1 = false;
                    }

                    _slabs[index1 + 1, index2 + 1] = Slab.Create(flag1 ? new SlabState(SlabStates.Solid) : new SlabState(SlabStates.Empty), hasWall);
                }
            }

            for (int index1 = 0; index1 < num1; ++index1)
            {
                for (int index2 = 0; index2 < num2; ++index2)
                    SmoothSlope(index1 + 1, index2 + 1);
            }

            int num7 = num1 / 2;
            int val1 = num2 / 2;
            int num8 = (val1 + 1) * (val1 + 1);
            float num9 = (float)(Utils.NextFloat(GenBase._random) * 2.0 - 1.0);
            float num10 = (float)(Utils.NextFloat(GenBase._random) * 2.0 - 1.0);
            float num11 = (float)(Utils.NextFloat(GenBase._random) * 2.0 - 1.0);
            float num12 = 0.0f;
            for (int index1 = 0; index1 <= num1; ++index1)
            {
                float num4 = (float)val1 / (float)num7 * (float)(index1 - num7);
                int num5 = Math.Min(val1, (int)Math.Sqrt(Math.Max(0.0f, (float)num8 - num4 * num4)));
                if (index1 < num1 / 2)
                    num12 += MathHelper.Lerp(num9, num10, (float)index1 / (float)(num1 / 2));
                else
                    num12 += MathHelper.Lerp(num10, num11, (float)(index1 / (num1 / 2) - 1.0));

                for (int index2 = val1 - num5; index2 <= val1 + num5; ++index2)
                    PlaceSlab(_slabs[index1 + 1, index2 + 1], index1 * 3 + origin.X, index2 * 3 + origin.Y + (int)num12, 3);
            }

            return true;
        }

        private delegate bool SlabState(int x, int y, int scale);

        private class SlabStates
        {
            public static bool Empty(int x, int y, int scale)
            {
                return false;
            }

            public static bool Solid(int x, int y, int scale)
            {
                return true;
            }

            public static bool HalfBrick(int x, int y, int scale)
            {
                return y >= scale / 2;
            }

            public static bool BottomRightFilled(int x, int y, int scale)
            {
                return x >= scale - y;
            }

            public static bool BottomLeftFilled(int x, int y, int scale)
            {
                return x < y;
            }

            public static bool TopRightFilled(int x, int y, int scale)
            {
                return x > y;
            }

            public static bool TopLeftFilled(int x, int y, int scale)
            {
                return x < scale - y;
            }
        }

        private struct Slab
        {
            public readonly SlabState State;
            public readonly bool HasWall;

            public bool IsSolid
            {
                get { return State != new SlabState(SlabStates.Empty); }
            }

            private Slab(SlabState state, bool hasWall)
            {
                State = state;
                HasWall = hasWall;
            }

            public Slab WithState(SlabState state)
            {
                return new Slab(state, HasWall);
            }

            public static Slab Create(SlabState state, bool hasWall)
            {
                return new Slab(state, hasWall);
            }
        }
    }
}
