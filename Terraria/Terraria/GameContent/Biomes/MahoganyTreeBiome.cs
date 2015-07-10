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
using Terraria.World.Generation;

namespace Terraria.GameContent.Biomes
{
    internal class MahoganyTreeBiome : MicroBiome
    {
        public override bool Place(Point origin, StructureMap structures)
        {
            Point result1, result2;
            if (!WorldUtils.Find(new Point(origin.X - 3, origin.Y), Searches.Chain(new Searches.Down(200), new Conditions.IsSolid().AreaAnd(6, 1)), out result1))
                return false;

            if (!WorldUtils.Find(new Point(result1.X, result1.Y - 5), Searches.Chain(new Searches.Up(120), new Conditions.IsSolid().AreaOr(6, 1)), out result2) ||
                result1.Y - 5 - result2.Y > 60 || (result1.Y - result2.Y < 30 || !structures.CanPlace(new Microsoft.Xna.Framework.Rectangle(result1.X - 30, result1.Y - 60, 60, 90), 0)))
                return false;

            Dictionary<ushort, int> resultsOutput = new Dictionary<ushort, int>();
            WorldUtils.Gen(new Point(result1.X - 25, result1.Y - 25), new Shapes.Rectangle(50, 50), new Actions.TileScanner(new ushort[4] { 0, 59, 147, 1 }).Output(resultsOutput));
            int num1 = resultsOutput[0] + resultsOutput[1];
            int num2 = resultsOutput[59];
            if (resultsOutput[147] > num2 || num1 > num2 || num2 < 50)
                return false;

            int num3 = (result1.Y - result2.Y - 9) / 5;
            int num4 = num3 * 5;
            int num5 = 0;
            double num6 = GenBase._random.NextDouble() + 1.0;
            double num7 = GenBase._random.NextDouble() + 2.0;
            if (GenBase._random.Next(2) == 0)
                num7 = -num7;

            for (int index = 0; index < num3; ++index)
            {
                int num8 = (int)(Math.Sin((index + 1) / 12.0 * num6 * 3.14159274101257) * num7);
                int num9 = num8 < num5 ? num8 - num5 : 0;
                WorldUtils.Gen(new Point(result1.X + num5 + num9, result1.Y - (index + 1) * 5), new Shapes.Rectangle(6 + Math.Abs(num8 - num5), 7),
                    Actions.Chain(new Actions.RemoveWall(), new Actions.SetTile(383, false, true), new Actions.SetFrames(false)));
                WorldUtils.Gen(new Point(result1.X + num5 + num9 + 2, result1.Y - (index + 1) * 5), new Shapes.Rectangle(2 + Math.Abs(num8 - num5), 5),
                    Actions.Chain(new Actions.ClearTile(true), new Actions.PlaceWall(78, true)));
                WorldUtils.Gen(new Point(result1.X + num5 + 2, result1.Y - index * 5), new Shapes.Rectangle(2, 2), Actions.Chain(new Actions.ClearTile(true), new Actions.PlaceWall(78, true)));
                num5 = num8;
            }

            int num10 = 6;
            if (num7 < 0.0)
                num10 = 0;
            List<Point> endpoints = new List<Point>();
            for (int index = 0; index < 2; ++index)
            {
                double num8 = ((double)index + 1.0) / 3.0;
                int num9 = num10 + (int)(Math.Sin((double)num3 * num8 / 12.0 * num6 * 3.14159274101257) * num7);
                double angle = GenBase._random.NextDouble() * 0.785398185253143 - 0.785398185253143 - 0.200000002980232;
                if (num10 == 0)
                    angle -= 1.57079637050629;

                WorldUtils.Gen(new Point(result1.X + num9, result1.Y - (int)((num3 * 5) * num8)), new ShapeBranch(angle, GenBase._random.Next(12, 16)).OutputEndpoints(endpoints),
                    Actions.Chain(new Actions.SetTile(383, false, true), new Actions.SetFrames(true)));
                num10 = 6 - num10;
            }

            int num11 = (int)(Math.Sin(num3 / 12.0 * num6 * 3.14159274101257) * num7);
            WorldUtils.Gen(new Point(result1.X + 6 + num11, result1.Y - num4), new ShapeBranch(-0.685398185253143, GenBase._random.Next(16, 22)).OutputEndpoints(endpoints),
                Actions.Chain(new Actions.SetTile(383, false, true), new Actions.SetFrames(true)));
            WorldUtils.Gen(new Point(result1.X + num11, result1.Y - num4), new ShapeBranch(-2.45619455575943, GenBase._random.Next(16, 22)).OutputEndpoints(endpoints),
                Actions.Chain(new Actions.SetTile(383, false, true), new Actions.SetFrames(true)));

            foreach (Point origin1 in endpoints)
            {
                Shapes.Circle circle = new Shapes.Circle(4);
                GenAction action = Actions.Chain(new Modifiers.Blotches(4, 2, 0.3), new Modifiers.SkipTiles(new ushort[1] { 383 }), new Modifiers.SkipWalls(new byte[1] { 78 }),
                    new Actions.SetTile(384, false, true), new Actions.SetFrames(true));
                WorldUtils.Gen(origin1, circle, action);
            }

            for (int index = 0; index < 4; ++index)
            {
                float angle = (float)(index / 3.0 * 2.0 + 0.570749998092651);
                WorldUtils.Gen(result1, new ShapeRoot(angle, (float)GenBase._random.Next(40, 60), 4f, 1f), new Actions.SetTile(383, true, true));
            }

            WorldGen.AddBuriedChest(result1.X + 3, result1.Y - 1, GenBase._random.Next(4) == 0 ? 0 : WorldGen.GetNextJungleChestItem(), false, 10);
            structures.AddStructure(new Microsoft.Xna.Framework.Rectangle(result1.X - 30, result1.Y - 30, 60, 60), 0);
            return true;
        }
    }
}
