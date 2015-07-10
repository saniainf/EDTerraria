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
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.World.Generation;

namespace Terraria.GameContent.Biomes
{
    internal class EnchantedSwordBiome : MicroBiome
    {
        public override bool Place(Point origin, StructureMap structures)
        {
            Dictionary<ushort, int> resultsOutput = new Dictionary<ushort, int>();
            WorldUtils.Gen(new Point(origin.X - 25, origin.Y - 25), new Shapes.Rectangle(50, 50), new Actions.TileScanner(new ushort[2] { 0, 1 }).Output(resultsOutput));
            if (resultsOutput[0] + resultsOutput[1] < 1250)
                return false;

            Point result1;
            bool flag = WorldUtils.Find(origin, Searches.Chain(new Searches.Up(1000), new Conditions.IsSolid().AreaOr(1, 50).Not()), out result1);
            Point result2;
            if (WorldUtils.Find(origin, Searches.Chain(new Searches.Up(origin.Y - result1.Y), new Conditions.IsTile(new ushort[1] { 53 })), out result2) || !flag)
                return false;

            result1.Y += 50;
            ShapeData data1 = new ShapeData();
            ShapeData shapeData = new ShapeData();
            Point point1 = new Point(origin.X, origin.Y + 20);
            Point point2 = new Point(origin.X, origin.Y + 30);
            float xScale = (float)(0.800000011920929 + Utils.NextFloat(GenBase._random) * 0.5);
            if (!structures.CanPlace(new Microsoft.Xna.Framework.Rectangle(point1.X - (int)(20.0 * xScale), point1.Y - 20, (int)(40.0 * xScale), 40), 0) ||
                !structures.CanPlace(new Microsoft.Xna.Framework.Rectangle(origin.X, result1.Y + 10, 1, origin.Y - result1.Y - 9), 2))
                return false;

            WorldUtils.Gen(point1, new Shapes.Slime(20, xScale, 1f), Actions.Chain(new Modifiers.Blotches(2, 0.4), new Actions.ClearTile(true).Output(data1)));
            WorldUtils.Gen(point2, new Shapes.Mound(14, 14), Actions.Chain(new Modifiers.Blotches(2, 1, 0.8), new Actions.SetTile(0, false, true), new Actions.SetFrames(true).Output(shapeData)));
            data1.Subtract(shapeData, point1, point2);
            WorldUtils.Gen(point1, new ModShapes.InnerOutline(data1, true), Actions.Chain(new Actions.SetTile((ushort)2, false, true), new Actions.SetFrames(true)));
            WorldUtils.Gen(point1, new ModShapes.All(data1), Actions.Chain(new Modifiers.RectangleMask(-40, 40, 0, 40), new Modifiers.IsEmpty(), new Actions.SetLiquid()));
            WorldUtils.Gen(point1, new ModShapes.All(data1), Actions.Chain(new Actions.PlaceWall((byte)68, true), new Modifiers.OnlyTiles(new ushort[1] { 2 }),
                new Modifiers.Offset(0, 1), new ActionVines(3, 5, 52)));
            ShapeData data2 = new ShapeData();
            WorldUtils.Gen(new Point(origin.X, result1.Y + 10), new Shapes.Rectangle(1, origin.Y - result1.Y - 9), Actions.Chain(new Modifiers.Blotches(2, 0.2),
                new Actions.ClearTile(false).Output(data2), new Modifiers.Expand(1), new Modifiers.OnlyTiles(new ushort[1] { 53 }), new Actions.SetTile(397, false, true).Output(data2)));
            WorldUtils.Gen(new Point(origin.X, result1.Y + 10), new ModShapes.All(data2), new Actions.SetFrames(true));
            if (GenBase._random.Next(3) == 0)
                WorldGen.PlaceTile(point2.X, point2.Y - 15, 187, true, false, -1, 17);
            else
                WorldGen.PlaceTile(point2.X, point2.Y - 15, 186, true, false, -1, 15);
            WorldUtils.Gen(point2, new ModShapes.All(shapeData), Actions.Chain(new Modifiers.Offset(0, -1), new Modifiers.OnlyTiles(new ushort[1] { 2 }),
                new Modifiers.Offset(0, -1), new ActionGrass()));

            structures.AddStructure(new Microsoft.Xna.Framework.Rectangle(point1.X - (int)(20.0 * xScale), point1.Y - 20, (int)(40.0 * xScale), 40), 4);
            return true;
        }
    }
}
