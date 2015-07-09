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
    internal class ActionStalagtite : GenAction
    {
        public override bool Apply(Point origin, int x, int y, params object[] args)
        {
            WorldGen.PlaceTight(x, y, (ushort)165, false);
            return this.UnitApply(origin, x, y, args);
        }
    }
}
