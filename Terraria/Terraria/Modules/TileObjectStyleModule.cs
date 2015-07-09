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
    public class TileObjectStyleModule
    {
        public int style;
        public bool horizontal;
        public int styleWrapLimit;
        public int styleMultiplier;

        public TileObjectStyleModule(TileObjectStyleModule copyFrom = null)
        {
            if (copyFrom == null)
            {
                this.style = 0;
                this.horizontal = false;
                this.styleWrapLimit = 0;
                this.styleMultiplier = 1;
            }
            else
            {
                this.style = copyFrom.style;
                this.horizontal = copyFrom.horizontal;
                this.styleWrapLimit = copyFrom.styleWrapLimit;
                this.styleMultiplier = copyFrom.styleMultiplier;
            }
        }
    }
}
