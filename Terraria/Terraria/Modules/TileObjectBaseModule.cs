/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using Terraria.DataStructures;
using Terraria.Enums;

namespace Terraria.Modules
{
    public class TileObjectBaseModule
    {
        public int width;
        public int height;
        public Point16 origin;
        public TileObjectDirection direction;
        public int randomRange;
        public bool flattenAnchors;

        public TileObjectBaseModule(TileObjectBaseModule copyFrom = null)
        {
            if (copyFrom == null)
            {
                this.width = 1;
                this.height = 1;
                this.origin = Point16.Zero;
                this.direction = TileObjectDirection.None;
                this.randomRange = 0;
                this.flattenAnchors = false;
            }
            else
            {
                this.width = copyFrom.width;
                this.height = copyFrom.height;
                this.origin = copyFrom.origin;
                this.direction = copyFrom.direction;
                this.randomRange = copyFrom.randomRange;
                this.flattenAnchors = copyFrom.flattenAnchors;
            }
        }
    }
}
