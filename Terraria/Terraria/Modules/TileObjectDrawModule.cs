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
    public class TileObjectDrawModule
    {
        public int yOffset;
        public bool flipHorizontal;
        public bool flipVertical;
        public int stepDown;

        public TileObjectDrawModule(TileObjectDrawModule copyFrom = null)
        {
            if (copyFrom == null)
            {
                this.yOffset = 0;
                this.flipHorizontal = false;
                this.flipVertical = false;
                this.stepDown = 0;
            }
            else
            {
                this.yOffset = copyFrom.yOffset;
                this.flipHorizontal = copyFrom.flipHorizontal;
                this.flipVertical = copyFrom.flipVertical;
                this.stepDown = copyFrom.stepDown;
            }
        }
    }
}
