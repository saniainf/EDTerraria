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
using Terraria.World.Generation;

namespace Terraria.GameContent.Biomes
{
    internal class CampsiteBiome : MicroBiome
    {
        public override bool Place(Point origin, StructureMap structures)
        {
            Ref<int> count1 = new Ref<int>(0);
            Ref<int> count2 = new Ref<int>(0);
            WorldUtils.Gen(origin, new Shapes.Circle(10), Actions.Chain(new Actions.Scanner(count2), new Modifiers.IsSolid(), new Actions.Scanner(count1)));
            if (count1.Value < count2.Value - 5)
                return false;

            int radius = GenBase._random.Next(6, 10);
            int num1 = GenBase._random.Next(5);
            if (!structures.CanPlace(new Microsoft.Xna.Framework.Rectangle(origin.X - radius, origin.Y - radius, radius * 2, radius * 2), 0))
                return false;

            ShapeData data = new ShapeData();
            WorldUtils.Gen(origin, new Shapes.Slime(radius), Actions.Chain(new Modifiers.Blotches(num1, num1, num1, 1, 0.3).Output(data), 
                new Modifiers.Offset(0, -2), new Modifiers.OnlyTiles(new ushort[1] { 53 }), new Actions.SetTile(397, true, true), 
                new Modifiers.OnlyWalls(new byte[1]), new Actions.PlaceWall(16, true)));
            WorldUtils.Gen(origin, new ModShapes.All(data), Actions.Chain(new Actions.ClearTile(false), new Actions.SetLiquid(0, 0), 
                new Actions.SetFrames(true), new Modifiers.OnlyWalls(new byte[1]), new Actions.PlaceWall(16, true)));

            Point result;
            if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(10), new Conditions.IsSolid()), out result))
                return false;

            int j = result.Y - 1;
            bool flag = GenBase._random.Next() % 2 == 0;
            if (GenBase._random.Next() % 10 != 0)
            {
                int num2 = GenBase._random.Next(1, 4);
                int num3 = flag ? 4 : -(radius >> 1);
                for (int index1 = 0; index1 < num2; ++index1)
                {
                    int num4 = GenBase._random.Next(1, 3);
                    for (int index2 = 0; index2 < num4; ++index2)
                        WorldGen.PlaceTile(origin.X + num3 - index1, j - index2, 331, false, false, -1, 0);
                }
            }

            int num5 = (radius - 3) * (flag ? -1 : 1);
            if (GenBase._random.Next() % 10 != 0)
                WorldGen.PlaceTile(origin.X + num5, j, 186, false, false, -1, 0);
            if (GenBase._random.Next() % 10 != 0)
            {
                WorldGen.PlaceTile(origin.X, j, 215, true, false, -1, 0);
                if (GenBase._tiles[origin.X, j].active() && GenBase._tiles[origin.X, j].type == 215)
                {
                    GenBase._tiles[origin.X, j].frameY += 36;
                    GenBase._tiles[origin.X - 1, j].frameY += 36;
                    GenBase._tiles[origin.X + 1, j].frameY += 36;
                    GenBase._tiles[origin.X, j - 1].frameY += 36;
                    GenBase._tiles[origin.X - 1, j - 1].frameY += 36;
                    GenBase._tiles[origin.X + 1, j - 1].frameY += 36;
                }
            }

            structures.AddStructure(new Microsoft.Xna.Framework.Rectangle(origin.X - radius, origin.Y - radius, radius * 2, radius * 2), 4);
            return true;
        }
    }
}
