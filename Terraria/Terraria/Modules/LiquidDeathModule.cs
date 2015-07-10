/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

namespace Terraria.Modules
{
    public class LiquidDeathModule
    {
        public bool water;
        public bool lava;

        public LiquidDeathModule(LiquidDeathModule copyFrom = null)
        {
            if (copyFrom == null)
            {
                water = false;
                lava = false;
            }
            else
            {
                water = copyFrom.water;
                lava = copyFrom.lava;
            }
        }
    }
}
