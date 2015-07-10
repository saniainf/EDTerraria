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
                yOffset = 0;
                flipHorizontal = false;
                flipVertical = false;
                stepDown = 0;
            }
            else
            {
                yOffset = copyFrom.yOffset;
                flipHorizontal = copyFrom.flipHorizontal;
                flipVertical = copyFrom.flipVertical;
                stepDown = copyFrom.stepDown;
            }
        }
    }
}
