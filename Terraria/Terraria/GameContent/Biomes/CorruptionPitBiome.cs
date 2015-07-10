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
using Terraria;
using Terraria.ID;
using Terraria.World.Generation;

namespace Terraria.GameContent.Biomes
{
    internal class CorruptionPitBiome : MicroBiome
    {
        public static bool[] ValidTiles = TileID.Sets.Factory.CreateBoolSet(1 != 0, 21, 31, 26);

        public override bool Place(Point origin, StructureMap structures)
        {
            if (WorldGen.SolidTile(origin.X, origin.Y) && GenBase._tiles[origin.X, origin.Y].wall == 3)
                return false;

            if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(100), new Conditions.IsSolid()), out origin))
                return false;

            Point result;
            if (!WorldUtils.Find(new Point(origin.X - 4, origin.Y), Searches.Chain(new Searches.Down(5), new Conditions.IsTile(new ushort[1] { 25 }).AreaAnd(8, 1)), out result))
                return false;

            ShapeData data1 = new ShapeData();
            ShapeData shapeData1 = new ShapeData();
            ShapeData shapeData2 = new ShapeData();
            for (int index = 0; index < 6; ++index)
                WorldUtils.Gen(origin, new Shapes.Circle(GenBase._random.Next(10, 12) + index), Actions.Chain(new Modifiers.Offset(0, 5 * index + 5), new Modifiers.Blotches(3, 0.3).Output(data1)));
            for (int index = 0; index < 6; ++index)
                WorldUtils.Gen(origin, new Shapes.Circle(GenBase._random.Next(5, 7) + index), Actions.Chain(new Modifiers.Offset(0, 2 * index + 18), new Modifiers.Blotches(3, 0.3).Output(shapeData1)));
            for (int index = 0; index < 6; ++index)
                WorldUtils.Gen(origin, new Shapes.Circle(GenBase._random.Next(4, 6) + index / 2), Actions.Chain(new Modifiers.Offset(0, (int)(7.5 * (double)index) - 10), new Modifiers.Blotches(3, 0.3).Output(shapeData2)));

            ShapeData data2 = new ShapeData(shapeData1);
            shapeData1.Subtract(shapeData2, origin, origin);
            data2.Subtract(shapeData1, origin, origin);
            Microsoft.Xna.Framework.Rectangle bounds = ShapeData.GetBounds(origin, data1, shapeData2);
            if (!structures.CanPlace(bounds, CorruptionPitBiome.ValidTiles, 2))
                return false;

            WorldUtils.Gen(origin, new ModShapes.All(data1), Actions.Chain(new Actions.SetTile((ushort)25, true, true), new Actions.PlaceWall(3, true)));
            WorldUtils.Gen(origin, new ModShapes.All(shapeData1), new Actions.SetTile(0, true, true));
            WorldUtils.Gen(origin, new ModShapes.All(shapeData2), new Actions.ClearTile(true));
            WorldUtils.Gen(origin, new ModShapes.All(shapeData1), Actions.Chain(new Modifiers.IsTouchingAir(true), new Modifiers.NotTouching(0 != 0, new ushort[1] { 25 }), new Actions.SetTile(23, true, true)));
            WorldUtils.Gen(origin, new ModShapes.All(data2), new Actions.PlaceWall(69, true));

            structures.AddStructure(bounds, 2);
            return true;
        }
    }
}
