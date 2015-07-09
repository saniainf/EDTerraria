/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using Terraria.Enums;

namespace Terraria.Modules
{
    public class LiquidPlacementModule
    {
        public LiquidPlacement water;
        public LiquidPlacement lava;

        public LiquidPlacementModule(LiquidPlacementModule copyFrom = null)
        {
            if (copyFrom == null)
            {
                this.water = LiquidPlacement.Allowed;
                this.lava = LiquidPlacement.Allowed;
            }
            else
            {
                this.water = copyFrom.water;
                this.lava = copyFrom.lava;
            }
        }
    }
}
