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
using Terraria.World.Generation;

namespace Terraria.GameContent.Biomes
{
    internal class ThinIceBiome : MicroBiome
    {
        public override bool Place(Point origin, StructureMap structures)
        {
            Dictionary<ushort, int> resultsOutput = new Dictionary<ushort, int>();
            WorldUtils.Gen(new Point(origin.X - 25, origin.Y - 25), new Shapes.Rectangle(50, 50), new Actions.TileScanner(new ushort[4] { 0, 59, 147, 1 }).Output(resultsOutput));
            int num1 = resultsOutput[0] + resultsOutput[1];
            int num2 = resultsOutput[59];
            int num3 = resultsOutput[147];
            if (num3 <= num2 || num3 <= num1)
                return false;

            int num4 = 0;
            for (int radius = GenBase._random.Next(10, 15); radius > 5; --radius)
            {
                int num5 = GenBase._random.Next(-5, 5);
                WorldUtils.Gen(new Point(origin.X + num5, origin.Y + num4), new Shapes.Circle(radius), Actions.Chain(new Modifiers.Blotches(4, 0.3),
                    new Modifiers.OnlyTiles(new ushort[5] { 147, 161, 224, 0, 1 }), new Actions.SetTile(162, true, true)));
                WorldUtils.Gen(new Point(origin.X + num5, origin.Y + num4), new Shapes.Circle(radius), Actions.Chain(new Modifiers.Blotches(4, 0.3),
                    new Modifiers.HasLiquid(-1, -1), new Actions.SetTile(162, true, true), new Actions.SetLiquid(0, 0)));
                num4 += radius - 2;
            }

            return true;
        }
    }
}
