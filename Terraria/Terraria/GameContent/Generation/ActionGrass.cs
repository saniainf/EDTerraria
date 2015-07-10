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

namespace Terraria.GameContent.Generation
{
    internal class ActionGrass : GenAction
    {
        public override bool Apply(Point origin, int x, int y, params object[] args)
        {
            if (GenBase._tiles[x, y].active() || GenBase._tiles[x, y - 1].active())
                return false;

            WorldGen.PlaceTile(x, y, (int)Utils.SelectRandom<ushort>(GenBase._random, 3, 73), 1 != 0, 0 != 0, -1, 0);
            return this.UnitApply(origin, x, y, args);
        }
    }
}
