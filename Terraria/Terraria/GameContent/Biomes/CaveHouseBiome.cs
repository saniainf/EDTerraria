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
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.World.Generation;

namespace Terraria.GameContent.Biomes
{
    internal class CaveHouseBiome : MicroBiome
    {
        private static readonly bool[] _blacklistedTiles = TileID.Sets.Factory.CreateBoolSet(1 != 0, 225, 41, 43, 44, 226, 203, 112, 25, 151);
        private static readonly ushort[] scanTileIds = { 0, 59, 147, 1, 161, 53, 396, 397, 368, 367, 60, 70 };
        private const int VERTICAL_EXIT_WIDTH = 3;
        private int _sharpenerCount;
        private int _extractinatorCount;

        private Microsoft.Xna.Framework.Rectangle GetRoom(Point origin)
        {
            Point result1, result2, result3, result4;
            bool flag1 = WorldUtils.Find(origin, Searches.Chain(new Searches.Left(25), new Conditions.IsSolid()), out result1);
            bool flag2 = WorldUtils.Find(origin, Searches.Chain(new Searches.Right(25), new Conditions.IsSolid()), out result2);
            if (!flag1)
                result1 = new Point(origin.X - 25, origin.Y);
            if (!flag2)
                result2 = new Point(origin.X + 25, origin.Y);

            Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle(origin.X, origin.Y, 0, 0);
            if (origin.X - result1.X > result2.X - origin.X)
            {
                rectangle.X = result1.X;
                rectangle.Width = Utils.Clamp<int>(result2.X - result1.X, 15, 30);
            }
            else
            {
                rectangle.Width = Utils.Clamp<int>(result2.X - result1.X, 15, 30);
                rectangle.X = result2.X - rectangle.Width;
            }

            bool flag3 = WorldUtils.Find(result1, Searches.Chain(new Searches.Up(10), new Conditions.IsSolid()), out result3);
            bool flag4 = WorldUtils.Find(result2, Searches.Chain(new Searches.Up(10), new Conditions.IsSolid()), out result4);
            if (!flag3)
                result3 = new Point(origin.X, origin.Y - 10);
            if (!flag4)
                result4 = new Point(origin.X, origin.Y - 10);

            rectangle.Height = Utils.Clamp<int>(Math.Max(origin.Y - result3.Y, origin.Y - result4.Y), 8, 12);
            rectangle.Y -= rectangle.Height;
            return rectangle;
        }

        private float RoomSolidPrecentage(Microsoft.Xna.Framework.Rectangle room)
        {
            float num = (float)(room.Width * room.Height);
            Ref<int> count = new Ref<int>(0);
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Modifiers.IsSolid(), new Actions.Count(count)));
            return (float)count.Value / num;
        }

        private bool FindVerticalExit(Microsoft.Xna.Framework.Rectangle wall, bool isUp, out int exitX)
        {
            Point result;
            bool flag = WorldUtils.Find(new Point(wall.X + wall.Width - 3, wall.Y + (isUp ? -5 : 0)), Searches.Chain(new Searches.Left(wall.Width - 3),
                new Conditions.IsSolid().Not().AreaOr(3, 5)), out result);

            exitX = result.X;
            return flag;
        }

        private bool FindSideExit(Microsoft.Xna.Framework.Rectangle wall, bool isLeft, out int exitY)
        {
            Point result;
            bool flag = WorldUtils.Find(new Point(wall.X + (isLeft ? -4 : 0), wall.Y + wall.Height - 3), Searches.Chain(new Searches.Up(wall.Height - 3),
                new Conditions.IsSolid().Not().AreaOr(4, 3)), out result);

            exitY = result.Y;
            return flag;
        }

        private int SortBiomeResults(Tuple<CaveHouseBiome.BuildData, int> item1, Tuple<CaveHouseBiome.BuildData, int> item2)
        {
            return item2.Item2.CompareTo(item1.Item2);
        }

        public override bool Place(Point origin, StructureMap structures)
        {
            Point result1;
            if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(200), new Conditions.IsSolid()), out result1) || result1 == origin)
                return false;

            Microsoft.Xna.Framework.Rectangle room1 = this.GetRoom(result1);
            Microsoft.Xna.Framework.Rectangle room2 = this.GetRoom(new Point(room1.Center.X, room1.Y + 1));
            Microsoft.Xna.Framework.Rectangle room3 = this.GetRoom(new Point(room1.Center.X, room1.Y + room1.Height + 10));
            room3.Y = room1.Y + room1.Height - 1;
            float num1 = this.RoomSolidPrecentage(room2);
            float num2 = this.RoomSolidPrecentage(room3);
            room1.Y += 3;
            room2.Y += 3;
            room3.Y += 3;
            List<Microsoft.Xna.Framework.Rectangle> list1 = new List<Microsoft.Xna.Framework.Rectangle>();
            if ((double)Utils.NextFloat(GenBase._random) > (double)num1 + 0.200000002980232)
                list1.Add(room2);
            else
                room2 = room1;
            list1.Add(room1);

            if ((double)Utils.NextFloat(GenBase._random) > (double)num2 + 0.200000002980232)
                list1.Add(room3);
            else
                room3 = room1;

            Dictionary<ushort, int> resultsOutput = new Dictionary<ushort, int>();
            foreach (Microsoft.Xna.Framework.Rectangle rectangle in list1)
                WorldUtils.Gen(new Point(rectangle.X - 5, rectangle.Y - 5), new Shapes.Rectangle(rectangle.Width + 10, rectangle.Height + 10),
                    new Actions.TileScanner(scanTileIds).Output(resultsOutput));

            List<Tuple<BuildData, int>> list2 = new List<Tuple<BuildData, int>>();
            list2.Add(Tuple.Create<BuildData, int>(BuildData.Default, resultsOutput[0] + resultsOutput[1]));
            list2.Add(Tuple.Create<BuildData, int>(BuildData.Jungle, resultsOutput[59] + resultsOutput[60] * 10));
            list2.Add(Tuple.Create<BuildData, int>(BuildData.Mushroom, resultsOutput[59] + resultsOutput[70] * 10));
            list2.Add(Tuple.Create<BuildData, int>(BuildData.Snow, resultsOutput[147] + resultsOutput[161]));
            list2.Add(Tuple.Create<BuildData, int>(BuildData.Desert, resultsOutput[397] + resultsOutput[396] + resultsOutput[53]));
            list2.Add(Tuple.Create<BuildData, int>(BuildData.Granite, resultsOutput[368]));
            list2.Add(Tuple.Create<BuildData, int>(BuildData.Marble, resultsOutput[367]));
            list2.Sort(new Comparison<Tuple<BuildData, int>>(this.SortBiomeResults));

            BuildData buildData = list2[0].Item1;
            foreach (Microsoft.Xna.Framework.Rectangle area in list1)
            {
                if (buildData != BuildData.Granite)
                {
                    Point result2;
                    if (WorldUtils.Find(new Point(area.X - 2, area.Y - 2), Searches.Chain(new Searches.Rectangle(area.Width + 4, area.Height + 4).RequireAll(false), new Conditions.HasLava()), out result2))
                        return false;
                }

                if (!structures.CanPlace(area, _blacklistedTiles, 5))
                    return false;
            }

            int val1_1 = room1.X;
            int val1_2 = room1.X + room1.Width - 1;
            List<Microsoft.Xna.Framework.Rectangle> list3 = new List<Microsoft.Xna.Framework.Rectangle>();
            foreach (Microsoft.Xna.Framework.Rectangle rectangle in list1)
            {
                val1_1 = Math.Min(val1_1, rectangle.X);
                val1_2 = Math.Max(val1_2, rectangle.X + rectangle.Width - 1);
            }

            int num3 = 6;
            while (num3 > 4 && (val1_2 - val1_1) % num3 != 0)
                --num3;

            int x = val1_1;
            while (x <= val1_2)
            {
                for (int index1 = 0; index1 < list1.Count; ++index1)
                {
                    Microsoft.Xna.Framework.Rectangle rectangle = list1[index1];
                    if (x >= rectangle.X && x < rectangle.X + rectangle.Width)
                    {
                        int y = rectangle.Y + rectangle.Height;
                        int num4 = 50;
                        for (int index2 = index1 + 1; index2 < list1.Count; ++index2)
                        {
                            if (x >= list1[index2].X && x < list1[index2].X + list1[index2].Width)
                                num4 = Math.Min(num4, list1[index2].Y - y);
                        }

                        if (num4 > 0)
                        {
                            Point result2;
                            bool flag = WorldUtils.Find(new Point(x, y), Searches.Chain(new Searches.Down(num4), new Conditions.IsSolid()), out result2);
                            if (num4 < 50)
                            {
                                flag = true;
                                result2 = new Point(x, y + num4);
                            }
                            if (flag)
                                list3.Add(new Microsoft.Xna.Framework.Rectangle(x, y, 1, result2.Y - y));
                        }
                    }
                }
                x += num3;
            }

            List<Point> list4 = new List<Point>();
            foreach (Microsoft.Xna.Framework.Rectangle rectangle in list1)
            {
                int exitY;
                if (FindSideExit(new Microsoft.Xna.Framework.Rectangle(rectangle.X + rectangle.Width, rectangle.Y + 1, 1, rectangle.Height - 2), false, out exitY))
                    list4.Add(new Point(rectangle.X + rectangle.Width - 1, exitY));
                if (FindSideExit(new Microsoft.Xna.Framework.Rectangle(rectangle.X, rectangle.Y + 1, 1, rectangle.Height - 2), true, out exitY))
                    list4.Add(new Point(rectangle.X, exitY));
            }

            List<Tuple<Point, Point>> list5 = new List<Tuple<Point, Point>>();
            for (int index = 1; index < list1.Count; ++index)
            {
                Microsoft.Xna.Framework.Rectangle rectangle1 = list1[index];
                Microsoft.Xna.Framework.Rectangle rectangle2 = list1[index - 1];
                if (rectangle2.X - rectangle1.X > rectangle1.X + rectangle1.Width - (rectangle2.X + rectangle2.Width))
                    list5.Add(new Tuple<Point, Point>(new Point(rectangle1.X + rectangle1.Width - 1, rectangle1.Y + 1),
                        new Point(rectangle1.X + rectangle1.Width - rectangle1.Height + 1, rectangle1.Y + rectangle1.Height - 1)));
                else
                    list5.Add(new Tuple<Point, Point>(new Point(rectangle1.X, rectangle1.Y + 1), new Point(rectangle1.X + rectangle1.Height - 1, rectangle1.Y + rectangle1.Height - 1)));
            }

            List<Point> list6 = new List<Point>();
            int exitX;
            if (this.FindVerticalExit(new Microsoft.Xna.Framework.Rectangle(room2.X + 2, room2.Y, room2.Width - 4, 1), true, out exitX))
                list6.Add(new Point(exitX, room2.Y));
            if (this.FindVerticalExit(new Microsoft.Xna.Framework.Rectangle(room3.X + 2, room3.Y + room3.Height - 1, room3.Width - 4, 1), false, out exitX))
                list6.Add(new Point(exitX, room3.Y + room3.Height - 1));

            foreach (Microsoft.Xna.Framework.Rectangle area in list1)
            {
                WorldUtils.Gen(new Point(area.X, area.Y), new Shapes.Rectangle(area.Width, area.Height),
                    Actions.Chain(new Actions.SetTile(buildData.Tile, false, true), new Actions.SetFrames(true)));
                WorldUtils.Gen(new Point(area.X + 1, area.Y + 1), new Shapes.Rectangle(area.Width - 2, area.Height - 2),
                    Actions.Chain(new Actions.ClearTile(true), new Actions.PlaceWall(buildData.Wall, true)));
                structures.AddStructure(area, 8);
            }

            foreach (Tuple<Point, Point> tuple in list5)
            {
                Point origin1 = tuple.Item1;
                Point point = tuple.Item2;
                int num4 = point.X > origin1.X ? 1 : -1;
                ShapeData data = new ShapeData();
                for (int y = 0; y < point.Y - origin1.Y; ++y)
                    data.Add(num4 * (y + 1), y);
                WorldUtils.Gen(origin1, new ModShapes.All(data), Actions.Chain(new Actions.PlaceTile(19, buildData.PlatformStyle),
                    new Actions.SetSlope(num4 == 1 ? 1 : 2), new Actions.SetFrames(true)));
                WorldUtils.Gen(new Point(origin1.X + (num4 == 1 ? 1 : -4), origin1.Y - 1), new Shapes.Rectangle(4, 1), Actions.Chain(new Actions.Clear(),
                    new Actions.PlaceWall(buildData.Wall, true), new Actions.PlaceTile(19, buildData.PlatformStyle), new Actions.SetFrames(true)));
            }

            foreach (Point origin1 in list4)
            {
                WorldUtils.Gen(origin1, new Shapes.Rectangle(1, 3), new Actions.ClearTile(true));
                WorldGen.PlaceTile(origin1.X, origin1.Y, 10, true, true, -1, buildData.DoorStyle);
            }

            foreach (Point origin1 in list6)
            {
                Shapes.Rectangle rectangle = new Shapes.Rectangle(3, 1);
                GenAction action = Actions.Chain(new Actions.ClearMetadata(), new Actions.PlaceTile((ushort)19, buildData.PlatformStyle), new Actions.SetFrames(true));
                WorldUtils.Gen(origin1, (GenShape)rectangle, action);
            }

            foreach (Microsoft.Xna.Framework.Rectangle rectangle in list3)
            {
                if (rectangle.Height > 1 && (int)GenBase._tiles[rectangle.X, rectangle.Y - 1].type != 19)
                {
                    WorldUtils.Gen(new Point(rectangle.X, rectangle.Y), new Shapes.Rectangle(rectangle.Width, rectangle.Height),
                        Actions.Chain((GenAction)new Actions.SetTile(124, false, true), new Actions.SetFrames(true)));
                    Tile tile = GenBase._tiles[rectangle.X, rectangle.Y + rectangle.Height];
                    tile.slope((byte)0);
                    tile.halfBrick(false);
                }
            }

            Point[] pointArray = new Point[7] { new Point(14, buildData.TableStyle), new Point(16, 0), new Point(18, buildData.WorkbenchStyle), new Point(86, 0),
                new Point(87, buildData.PianoStyle), new Point(94, 0), new Point(101, buildData.BookcaseStyle) };
            foreach (Microsoft.Xna.Framework.Rectangle rectangle in list1)
            {
                int num4 = rectangle.Width / 8;
                int num5 = rectangle.Width / (num4 + 1);
                int num6 = GenBase._random.Next(2);
                for (int index1 = 0; index1 < num4; ++index1)
                {
                    int num7 = (index1 + 1) * num5 + rectangle.X;
                    switch (index1 + num6 % 2)
                    {
                        case 0:
                            int num8 = rectangle.Y + Math.Min(rectangle.Height / 2, rectangle.Height - 5);
                            Vector2 vector2 = WorldGen.randHousePicture();
                            int type = (int)vector2.X;
                            int style = (int)vector2.Y;
                            if (!WorldGen.nearPicture(num7, num8))
                            {
                                WorldGen.PlaceTile(num7, num8, type, true, false, -1, style);
                                break;
                            }
                            break;
                        case 1:
                            int j = rectangle.Y + 1;
                            WorldGen.PlaceTile(num7, j, 34, true, false, -1, GenBase._random.Next(6));
                            for (int index2 = -1; index2 < 2; ++index2)
                            {
                                for (int index3 = 0; index3 < 3; ++index3)
                                    GenBase._tiles[index2 + num7, index3 + j].frameX += (short)54;
                            }
                            break;
                    }
                }

                int num9 = rectangle.Width / 8 + 3;
                WorldGen.SetupStatueList();
                for (; num9 > 0; --num9)
                {
                    int num7 = GenBase._random.Next(rectangle.Width - 3) + 1 + rectangle.X;
                    int num8 = rectangle.Y + rectangle.Height - 2;
                    switch (GenBase._random.Next(4))
                    {
                        case 0:
                            WorldGen.PlaceSmallPile(num7, num8, GenBase._random.Next(31, 34), 1, (ushort)185);
                            break;
                        case 1:
                            WorldGen.PlaceTile(num7, num8, 186, true, false, -1, GenBase._random.Next(22, 26));
                            break;
                        case 2:
                            int index = GenBase._random.Next(2, WorldGen.statueList.Length);
                            WorldGen.PlaceTile(num7, num8, (int)WorldGen.statueList[index].X, true, false, -1, (int)WorldGen.statueList[index].Y);
                            if (WorldGen.StatuesWithTraps.Contains(index))
                            {
                                WorldGen.PlaceStatueTrap(num7, num8);
                                break;
                            }
                            break;
                        case 3:
                            Point point = Utils.SelectRandom<Point>(GenBase._random, pointArray);
                            WorldGen.PlaceTile(num7, num8, point.X, true, false, -1, point.Y);
                            break;
                    }
                }
            }

            foreach (Microsoft.Xna.Framework.Rectangle room4 in list1)
                buildData.ProcessRoom(room4);

            bool flag1 = false;
            foreach (Microsoft.Xna.Framework.Rectangle rectangle in list1)
            {
                int j = rectangle.Height - 1 + rectangle.Y;
                int Style = j > (int)Main.worldSurface ? buildData.ChestStyle : 0;
                int num4 = 0;
                while (num4 < 10 && !(flag1 = WorldGen.AddBuriedChest(GenBase._random.Next(2, rectangle.Width - 2) + rectangle.X, j, 0, false, Style)))
                    ++num4;
                if (!flag1)
                {
                    int i = rectangle.X + 2;
                    while (i <= rectangle.X + rectangle.Width - 2 && !(flag1 = WorldGen.AddBuriedChest(i, j, 0, false, Style)))
                        ++i;
                    if (flag1)
                        break;
                }
                else
                    break;
            }

            if (!flag1)
            {
                foreach (Microsoft.Xna.Framework.Rectangle rectangle in list1)
                {
                    int j = rectangle.Y - 1;
                    int Style = j > (int)Main.worldSurface ? buildData.ChestStyle : 0;
                    int num4 = 0;
                    while (num4 < 10 && !(flag1 = WorldGen.AddBuriedChest(GenBase._random.Next(2, rectangle.Width - 2) + rectangle.X, j, 0, false, Style)))
                        ++num4;
                    if (!flag1)
                    {
                        int i = rectangle.X + 2;
                        while (i <= rectangle.X + rectangle.Width - 2 && !(flag1 = WorldGen.AddBuriedChest(i, j, 0, false, Style)))
                            ++i;
                        if (flag1)
                            break;
                    }
                    else
                        break;
                }
            }

            if (!flag1)
            {
                for (int index = 0; index < 1000; ++index)
                {
                    int i = GenBase._random.Next(list1[0].X - 30, list1[0].X + 30);
                    int j = GenBase._random.Next(list1[0].Y - 30, list1[0].Y + 30);
                    int Style = j > (int)Main.worldSurface ? buildData.ChestStyle : 0;
                    if (WorldGen.AddBuriedChest(i, j, 0, false, Style))
                        break;
                }
            }

            if (buildData == BuildData.Jungle && _sharpenerCount < GenBase._random.Next(2, 5))
            {
                bool flag2 = false;
                foreach (Microsoft.Xna.Framework.Rectangle rectangle in list1)
                {
                    int j = rectangle.Height - 2 + rectangle.Y;
                    for (int index = 0; index < 10; ++index)
                    {
                        int i = GenBase._random.Next(2, rectangle.Width - 2) + rectangle.X;
                        WorldGen.PlaceTile(i, j, 377, true, true, -1, 0);
                        if (flag2 = GenBase._tiles[i, j].active() && (int)GenBase._tiles[i, j].type == 377)
                            break;
                    }
                    if (!flag2)
                    {
                        int i = rectangle.X + 2;
                        while (i <= rectangle.X + rectangle.Width - 2 && !(flag2 = WorldGen.PlaceTile(i, j, 377, true, true, -1, 0)))
                            ++i;
                        if (flag2)
                            break;
                    }
                    else
                        break;
                }

                if (flag2)
                    ++_sharpenerCount;
            }

            if (buildData == BuildData.Desert && _extractinatorCount < GenBase._random.Next(2, 5))
            {
                bool flag2 = false;
                foreach (Microsoft.Xna.Framework.Rectangle rectangle in list1)
                {
                    int j = rectangle.Height - 2 + rectangle.Y;
                    for (int index = 0; index < 10; ++index)
                    {
                        int i = GenBase._random.Next(2, rectangle.Width - 2) + rectangle.X;
                        WorldGen.PlaceTile(i, j, 219, true, true, -1, 0);
                        if (flag2 = GenBase._tiles[i, j].active() && (int)GenBase._tiles[i, j].type == 219)
                            break;
                    }
                    if (!flag2)
                    {
                        int i = rectangle.X + 2;
                        while (i <= rectangle.X + rectangle.Width - 2 && !(flag2 = WorldGen.PlaceTile(i, j, 219, true, true, -1, 0)))
                            ++i;
                        if (flag2)
                            break;
                    }
                    else
                        break;
                }
                if (flag2)
                    ++_extractinatorCount;
            }

            return true;
        }

        public override void Reset()
        {
            _sharpenerCount = 0;
            _extractinatorCount = 0;
        }

        internal static void AgeDefaultRoom(Microsoft.Xna.Framework.Rectangle room)
        {
            for (int index = 0; index < room.Width * room.Height / 16; ++index)
                WorldUtils.Gen(new Point(GenBase._random.Next(1, room.Width - 1) + room.X, GenBase._random.Next(1, room.Height - 1) + room.Y), new Shapes.Rectangle(2, 2),
                    Actions.Chain(new Modifiers.Dither(0.5), new Modifiers.Blotches(2, 2.0), new Modifiers.IsEmpty(), new Actions.SetTile(51, true, true)));
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Modifiers.Dither(0.850000023841858),
                new Modifiers.Blotches(2, 0.3), new Modifiers.OnlyWalls(new byte[1] { BuildData.Default.Wall }),
                (double)room.Y > Main.worldSurface ? new Actions.ClearWall(true) : (GenAction)new Actions.PlaceWall(2, true)));
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height),
                Actions.Chain(new Modifiers.Dither(0.949999988079071), new Modifiers.OnlyTiles(new ushort[3] { 30, 321, 158 }), new Actions.ClearTile(true)));
        }

        internal static void AgeSnowRoom(Microsoft.Xna.Framework.Rectangle room)
        {
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Modifiers.Dither(0.600000023841858),
                new Modifiers.Blotches(2, 0.600000023841858),
                new Modifiers.OnlyTiles(new ushort[1] { BuildData.Snow.Tile }),
                new Actions.SetTile(161, true, true), new Modifiers.Dither(0.8), new Actions.SetTile(147, true, true)));
            WorldUtils.Gen(new Point(room.X + 1, room.Y), new Shapes.Rectangle(room.Width - 2, 1), Actions.Chain(new Modifiers.Dither(0.5), new Modifiers.OnlyTiles(new ushort[1] { 161 }),
                new Modifiers.Offset(0, 1), new ActionStalagtite()));
            WorldUtils.Gen(new Point(room.X + 1, room.Y + room.Height - 1), new Shapes.Rectangle(room.Width - 2, 1),
                Actions.Chain(new Modifiers.Dither(0.5), new Modifiers.OnlyTiles(new ushort[1] { 161 }), new Modifiers.Offset(0, 1), new ActionStalagtite()));
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Modifiers.Dither(0.850000023841858),
                new Modifiers.Blotches(2, 0.8), (double)room.Y > Main.worldSurface ? new Actions.ClearWall(true) : (GenAction)new Actions.PlaceWall(40, true)));
        }

        internal static void AgeDesertRoom(Microsoft.Xna.Framework.Rectangle room)
        {
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height),
                Actions.Chain(new Modifiers.Dither(0.800000011920929), new Modifiers.Blotches(2, 0.200000002980232),
                new Modifiers.OnlyTiles(new ushort[1] { BuildData.Desert.Tile }), (GenAction)new Actions.SetTile(396, true, true),
                new Modifiers.Dither(0.5), new Actions.SetTile(397, true, true)));
            WorldUtils.Gen(new Point(room.X + 1, room.Y), new Shapes.Rectangle(room.Width - 2, 1),
                Actions.Chain(new Modifiers.Dither(0.5), new Modifiers.OnlyTiles(new ushort[2] { 397, 396 }), new Modifiers.Offset(0, 1), new ActionStalagtite()));
            WorldUtils.Gen(new Point(room.X + 1, room.Y + room.Height - 1), new Shapes.Rectangle(room.Width - 2, 1),
                Actions.Chain(new Modifiers.Dither(0.5), new Modifiers.OnlyTiles(new ushort[2] { 397, 396 }), new Modifiers.Offset(0, 1), new ActionStalagtite()));
            WorldUtils.Gen(new Point(room.X, room.Y), (GenShape)new Shapes.Rectangle(room.Width, room.Height),
                Actions.Chain(new Modifiers.Dither(0.800000011920929), new Modifiers.Blotches(2, 0.3),
                new Modifiers.OnlyWalls(new byte[1] { BuildData.Desert.Wall }), new Actions.PlaceWall((byte)216, true)));
        }

        internal static void AgeGraniteRoom(Microsoft.Xna.Framework.Rectangle room)
        {
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height),
                Actions.Chain(new Modifiers.Dither(0.600000023841858), new Modifiers.Blotches(2, 0.600000023841858),
                new Modifiers.OnlyTiles(new ushort[1] { BuildData.Granite.Tile }), new Actions.SetTile((ushort)368, true, true)));
            WorldUtils.Gen(new Point(room.X + 1, room.Y), new Shapes.Rectangle(room.Width - 2, 1), Actions.Chain(new Modifiers.Dither(0.800000011920929),
                new Modifiers.OnlyTiles(new ushort[1] { 368 }), new Modifiers.Offset(0, 1), new ActionStalagtite()));
            WorldUtils.Gen(new Point(room.X + 1, room.Y + room.Height - 1), new Shapes.Rectangle(room.Width - 2, 1),
                Actions.Chain(new Modifiers.Dither(0.800000011920929), new Modifiers.OnlyTiles(new ushort[1] { 368 }), new Modifiers.Offset(0, 1), new ActionStalagtite()));
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Modifiers.Dither(0.850000023841858),
                new Modifiers.Blotches(2, 0.3), new Actions.PlaceWall(180, true)));
        }

        internal static void AgeMarbleRoom(Microsoft.Xna.Framework.Rectangle room)
        {
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Modifiers.Dither(0.600000023841858),
                new Modifiers.Blotches(2, 0.600000023841858), new Modifiers.OnlyTiles(new ushort[1] { BuildData.Marble.Tile }), new Actions.SetTile(367, true, true)));
            WorldUtils.Gen(new Point(room.X + 1, room.Y), new Shapes.Rectangle(room.Width - 2, 1), Actions.Chain(new Modifiers.Dither(0.800000011920929),
                new Modifiers.OnlyTiles(new ushort[1] { 367 }), new Modifiers.Offset(0, 1), new ActionStalagtite()));
            WorldUtils.Gen(new Point(room.X + 1, room.Y + room.Height - 1), new Shapes.Rectangle(room.Width - 2, 1), Actions.Chain(new Modifiers.Dither(0.800000011920929),
                new Modifiers.OnlyTiles(new ushort[1] { 367 }), new Modifiers.Offset(0, 1), new ActionStalagtite()));
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Modifiers.Dither(0.850000023841858),
                new Modifiers.Blotches(2, 0.3), new Actions.PlaceWall(178, true)));
        }

        internal static void AgeMushroomRoom(Microsoft.Xna.Framework.Rectangle room)
        {
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Modifiers.Dither(0.699999988079071),
                new Modifiers.Blotches(2, 0.5), new Modifiers.OnlyTiles(new ushort[1] { BuildData.Mushroom.Tile }), new Actions.SetTile(70, true, true)));
            WorldUtils.Gen(new Point(room.X + 1, room.Y), new Shapes.Rectangle(room.Width - 2, 1), Actions.Chain(new Modifiers.Dither(0.600000023841858),
                new Modifiers.OnlyTiles(new ushort[1] { 70 }), new Modifiers.Offset(0, -1), new Actions.SetTile(71, false, true)));
            WorldUtils.Gen(new Point(room.X + 1, room.Y + room.Height - 1), new Shapes.Rectangle(room.Width - 2, 1), Actions.Chain(new Modifiers.Dither(0.600000023841858),
                new Modifiers.OnlyTiles(new ushort[1] { 70 }), new Modifiers.Offset(0, -1), new Actions.SetTile(71, false, true)));
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Modifiers.Dither(0.850000023841858),
                new Modifiers.Blotches(2, 0.3), new Actions.ClearWall(false)));
        }

        internal static void AgeJungleRoom(Microsoft.Xna.Framework.Rectangle room)
        {
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height),
                Actions.Chain(new Modifiers.Dither(0.600000023841858), new Modifiers.Blotches(2, 0.600000023841858),
                new Modifiers.OnlyTiles(new ushort[1] { BuildData.Jungle.Tile }), new Actions.SetTile(60, true, true),
                new Modifiers.Dither(0.800000011920929), new Actions.SetTile(59, true, true)));
            WorldUtils.Gen(new Point(room.X + 1, room.Y), new Shapes.Rectangle(room.Width - 2, 1),
                Actions.Chain(new Modifiers.Dither(0.5), new Modifiers.OnlyTiles(new ushort[1] { 60 }), new Modifiers.Offset(0, 1), new ActionVines(3, room.Height, 62)));
            WorldUtils.Gen(new Point(room.X + 1, room.Y + room.Height - 1), new Shapes.Rectangle(room.Width - 2, 1), Actions.Chain(new Modifiers.Dither(0.5),
                new Modifiers.OnlyTiles(new ushort[1] { 60 }), new Modifiers.Offset(0, 1), new ActionVines(3, room.Height, 62)));
            WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height),
                Actions.Chain(new Modifiers.Dither(0.850000023841858), new Modifiers.Blotches(2, 0.3), new Actions.PlaceWall((byte)64, true)));
        }

        private class BuildData
        {
            public static BuildData Snow = CreateSnowData();
            public static BuildData Jungle = CreateJungleData();
            public static BuildData Default = CreateDefaultData();
            public static BuildData Granite = CreateGraniteData();
            public static BuildData Marble = CreateMarbleData();
            public static BuildData Mushroom = CreateMushroomData();
            public static BuildData Desert = CreateDesertData();
            public ushort Tile;
            public byte Wall;
            public int PlatformStyle;
            public int DoorStyle;
            public int TableStyle;
            public int WorkbenchStyle;
            public int PianoStyle;
            public int BookcaseStyle;
            public int ChairStyle;
            public int ChestStyle;

            public delegate void ProcessRoomMethod(Microsoft.Xna.Framework.Rectangle room);
            public ProcessRoomMethod ProcessRoom;

            public static BuildData CreateSnowData()
            {
                return new BuildData()
                {
                    Tile = (ushort)321,
                    Wall = (byte)149,
                    DoorStyle = 30,
                    PlatformStyle = 19,
                    TableStyle = 28,
                    WorkbenchStyle = 23,
                    PianoStyle = 23,
                    BookcaseStyle = 25,
                    ChairStyle = 30,
                    ChestStyle = 11,
                    ProcessRoom = new ProcessRoomMethod(CaveHouseBiome.AgeSnowRoom)
                };
            }

            public static BuildData CreateDesertData()
            {
                return new BuildData()
                {
                    Tile = (ushort)396,
                    Wall = (byte)187,
                    PlatformStyle = 0,
                    DoorStyle = 0,
                    TableStyle = 0,
                    WorkbenchStyle = 0,
                    PianoStyle = 0,
                    BookcaseStyle = 0,
                    ChairStyle = 0,
                    ChestStyle = 1,
                    ProcessRoom = new ProcessRoomMethod(AgeDesertRoom)
                };
            }

            public static BuildData CreateJungleData()
            {
                return new BuildData()
                {
                    Tile = (ushort)158,
                    Wall = (byte)42,
                    PlatformStyle = 2,
                    DoorStyle = 2,
                    TableStyle = 2,
                    WorkbenchStyle = 2,
                    PianoStyle = 2,
                    BookcaseStyle = 12,
                    ChairStyle = 3,
                    ChestStyle = 8,
                    ProcessRoom = new ProcessRoomMethod(AgeJungleRoom)
                };
            }

            public static BuildData CreateGraniteData()
            {
                return new BuildData()
                {
                    Tile = (ushort)369,
                    Wall = (byte)181,
                    PlatformStyle = 28,
                    DoorStyle = 34,
                    TableStyle = 33,
                    WorkbenchStyle = 29,
                    PianoStyle = 28,
                    BookcaseStyle = 30,
                    ChairStyle = 34,
                    ChestStyle = 50,
                    ProcessRoom = new ProcessRoomMethod(AgeGraniteRoom)
                };
            }

            public static BuildData CreateMarbleData()
            {
                return new BuildData()
                {
                    Tile = (ushort)357,
                    Wall = (byte)179,
                    PlatformStyle = 29,
                    DoorStyle = 35,
                    TableStyle = 34,
                    WorkbenchStyle = 30,
                    PianoStyle = 29,
                    BookcaseStyle = 31,
                    ChairStyle = 35,
                    ChestStyle = 51,
                    ProcessRoom = new ProcessRoomMethod(AgeMarbleRoom)
                };
            }

            public static BuildData CreateMushroomData()
            {
                return new BuildData()
                {
                    Tile = (ushort)190,
                    Wall = (byte)74,
                    PlatformStyle = 18,
                    DoorStyle = 6,
                    TableStyle = 27,
                    WorkbenchStyle = 7,
                    PianoStyle = 22,
                    BookcaseStyle = 24,
                    ChairStyle = 9,
                    ChestStyle = 32,
                    ProcessRoom = new ProcessRoomMethod(AgeMushroomRoom)
                };
            }

            public static BuildData CreateDefaultData()
            {
                return new BuildData()
                {
                    Tile = (ushort)30,
                    Wall = (byte)27,
                    PlatformStyle = 0,
                    DoorStyle = 0,
                    TableStyle = 0,
                    WorkbenchStyle = 0,
                    PianoStyle = 0,
                    BookcaseStyle = 0,
                    ChairStyle = 0,
                    ChestStyle = 1,
                    ProcessRoom = new ProcessRoomMethod(AgeDefaultRoom)
                };
            }
        }
    }
}
