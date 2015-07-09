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

namespace Terraria.Modules
{
    public class AnchorDataModule
    {
        public AnchorData top;
        public AnchorData bottom;
        public AnchorData left;
        public AnchorData right;
        public bool wall;

        public AnchorDataModule(AnchorDataModule copyFrom = null)
        {
            if (copyFrom == null)
            {
                this.top = new AnchorData();
                this.bottom = new AnchorData();
                this.left = new AnchorData();
                this.right = new AnchorData();
                this.wall = false;
            }
            else
            {
                this.top = copyFrom.top;
                this.bottom = copyFrom.bottom;
                this.left = copyFrom.left;
                this.right = copyFrom.right;
                this.wall = copyFrom.wall;
            }
        }
    }
}
