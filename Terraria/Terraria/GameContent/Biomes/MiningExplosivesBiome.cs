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
using Terraria.GameContent.Generation;
using Terraria.World.Generation;

namespace Terraria.GameContent.Biomes
{
    internal class MiningExplosivesBiome : MicroBiome
    {
        public override bool Place(Point origin, StructureMap structures)
        {
            if (WorldGen.SolidTile(origin.X, origin.Y))
                return false;

            ushort type = Utils.SelectRandom<ushort>(GenBase._random, WorldGen.goldBar == 19 ? (ushort)8 : (ushort)169, WorldGen.silverBar == 21 ?
                (ushort)9 : (ushort)168, WorldGen.ironBar == 22 ? (ushort)6 : (ushort)167, WorldGen.copperBar == 20 ? (ushort)7 : (ushort)166);

            double num = GenBase._random.NextDouble() * 2.0 - 1.0;
            if (!WorldUtils.Find(origin, Searches.Chain(num > 0.0 ? new Searches.Right(40) : (GenSearch)new Searches.Left(40), new Conditions.IsSolid()), out origin))
                return false;
            if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(80), (GenCondition)new Conditions.IsSolid()), out origin))
                return false;

            ShapeData shapeData = new ShapeData();
            Ref<int> count1 = new Ref<int>(0);
            Ref<int> count2 = new Ref<int>(0);
            WorldUtils.Gen(origin, new ShapeRunner(10f, 20, new Vector2((float)num, 1f)).Output(shapeData), Actions.Chain(new Modifiers.Blotches(2, 0.3),
                new Actions.Scanner(count1), new Modifiers.IsSolid(), new Actions.Scanner(count2)));
            if (count2.Value < count1.Value / 2)
                return false;

            Microsoft.Xna.Framework.Rectangle area = new Microsoft.Xna.Framework.Rectangle(origin.X - 15, origin.Y - 10, 30, 20);
            if (!structures.CanPlace(area, 0))
                return false;

            WorldUtils.Gen(origin, new ModShapes.All(shapeData), new Actions.SetTile(type, true, true));
            WorldUtils.Gen(new Point(origin.X - (int)(num * -5.0), origin.Y - 5), new Shapes.Circle(5), Actions.Chain(new Modifiers.Blotches(2, 0.3), new Actions.ClearTile(true)));
            Point result1;
            Point result2;
            if (((((true ? 1 : 0) & (WorldUtils.Find(new Point(origin.X - (num > 0.0 ? 3 : -3), origin.Y - 3), Searches.Chain(new Searches.Down(10),
                new Conditions.IsSolid()), out result1) ? 1 : 0)) != 0 ? 1 : 0) & (WorldUtils.Find(new Point(origin.X - (num > 0.0 ? -3 : 3), origin.Y - 3),
                Searches.Chain(new Searches.Down(10), new Conditions.IsSolid()), out result2) ? 1 : 0)) == 0)
                return false;

            --result1.Y;
            --result2.Y;
            Tile tile1 = GenBase._tiles[result1.X, result1.Y + 1];
            tile1.slope(0);
            tile1.halfBrick(false);
            for (int index = -1; index <= 1; ++index)
            {
                WorldUtils.ClearTile(result2.X + index, result2.Y, false);
                Tile tile2 = GenBase._tiles[result2.X + index, result2.Y + 1];
                if (!WorldGen.SolidOrSlopedTile(tile2))
                {
                    tile2.ResetToType(1);
                    tile2.active(true);
                }

                tile2.slope(0);
                tile2.halfBrick(false);
                WorldUtils.TileFrame(result2.X + index, result2.Y + 1, true);
            }

            WorldGen.PlaceTile(result1.X, result1.Y, 141, false, false, -1, 0);
            WorldGen.PlaceTile(result2.X, result2.Y, 411, true, true, -1, 0);
            WorldUtils.WireLine(result1, result2);
            structures.AddStructure(area, 5);
            return true;
        }
    }
}
