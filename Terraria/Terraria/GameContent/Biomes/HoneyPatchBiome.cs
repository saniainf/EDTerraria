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
    internal class HoneyPatchBiome : MicroBiome
    {
        public override bool Place(Point origin, StructureMap structures)
        {
            if (GenBase._tiles[origin.X, origin.Y].active() && WorldGen.SolidTile(origin.X, origin.Y))
                return false;

            Point result;
            if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(80), new Conditions.IsSolid()), out result))
                return false;

            result.Y += 2;
            Ref<int> count = new Ref<int>(0);
            WorldUtils.Gen(result, new Shapes.Circle(8), Actions.Chain(new Modifiers.IsSolid(), new Actions.Scanner(count)));
            if (count.Value < 20 || !structures.CanPlace(new Microsoft.Xna.Framework.Rectangle(result.X - 8, result.Y - 8, 16, 16), 0))
                return false;

            WorldUtils.Gen(result, new Shapes.Circle(8), Actions.Chain(new Modifiers.RadialDither(0.0f, 10f), new Modifiers.IsSolid(), new Actions.SetTile(229, true, true)));
            ShapeData data = new ShapeData();
            WorldUtils.Gen(result, new Shapes.Circle(4, 3), Actions.Chain(new Modifiers.Blotches(2, 0.3), new Modifiers.IsSolid(), new Actions.ClearTile(true),
                new Modifiers.RectangleMask(-6, 6, 0, 3).Output(data), new Actions.SetLiquid(2, byte.MaxValue)));
            WorldUtils.Gen(new Point(result.X, result.Y + 1), new ModShapes.InnerOutline(data, true), Actions.Chain(new Modifiers.IsEmpty(), new Modifiers.RectangleMask(-6, 6, 1, 3),
                new Actions.SetTile(59, true, true)));

            structures.AddStructure(new Microsoft.Xna.Framework.Rectangle(result.X - 8, result.Y - 8, 16, 16), 0);
            return true;
        }
    }
}
