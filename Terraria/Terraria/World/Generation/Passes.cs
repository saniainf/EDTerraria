/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using Terraria;

namespace Terraria.World.Generation
{
    internal static class Passes
    {
        public class Clear : GenPass
        {
            public Clear()
                : base("clear", 1f)
            {
            }

            public override void Apply(GenerationProgress progress)
            {
                for (int index1 = 0; index1 < GenBase._worldWidth; ++index1)
                {
                    for (int index2 = 0; index2 < GenBase._worldHeight; ++index2)
                    {
                        if (GenBase._tiles[index1, index2] == null)
                            GenBase._tiles[index1, index2] = new Tile();
                        else
                            GenBase._tiles[index1, index2].ClearEverything();
                    }
                }
            }
        }

        public class ScatterCustom : GenPass
        {
            private GenBase.CustomPerUnitAction _perUnit;
            private int _count;

            public ScatterCustom(string name, float loadWeight, int count, GenBase.CustomPerUnitAction perUnit = null)
                : base(name, loadWeight)
            {
                this._perUnit = perUnit;
                this._count = count;
            }

            public void SetCustomAction(GenBase.CustomPerUnitAction perUnit)
            {
                this._perUnit = perUnit;
            }

            public override void Apply(GenerationProgress progress)
            {
                int num = this._count;
                while (num > 0)
                {
                    if (this._perUnit(GenBase._random.Next(1, GenBase._worldWidth), GenBase._random.Next(1, GenBase._worldHeight), new object[0]))
                        --num;
                }
            }
        }
    }
}
