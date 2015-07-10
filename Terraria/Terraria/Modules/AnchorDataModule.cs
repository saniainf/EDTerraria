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
                top = new AnchorData();
                bottom = new AnchorData();
                left = new AnchorData();
                right = new AnchorData();
                wall = false;
            }
            else
            {
                top = copyFrom.top;
                bottom = copyFrom.bottom;
                left = copyFrom.left;
                right = copyFrom.right;
                wall = copyFrom.wall;
            }
        }
    }
}
