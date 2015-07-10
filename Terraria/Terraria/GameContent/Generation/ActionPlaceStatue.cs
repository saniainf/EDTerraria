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
using Terraria.DataStructures;
using Terraria.World.Generation;

namespace Terraria.GameContent.Generation
{
    internal class ActionPlaceStatue : GenAction
    {
        private int _statueIndex;

        public ActionPlaceStatue(int index = -1)
        {
            _statueIndex = index;
        }

        public override bool Apply(Point origin, int x, int y, params object[] args)
        {
            Point16 point16 = _statueIndex != -1 ? WorldGen.statueList[_statueIndex] : WorldGen.statueList[GenBase._random.Next(2, WorldGen.statueList.Length)];
            WorldGen.PlaceTile(x, y, (int)point16.X, true, false, -1, (int)point16.Y);
            return UnitApply(origin, x, y, args);
        }
    }
}