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
    public class TilePlacementHooksModule
    {
        public PlacementHook check;
        public PlacementHook postPlaceEveryone;
        public PlacementHook postPlaceMyPlayer;
        public PlacementHook placeOverride;

        public TilePlacementHooksModule(TilePlacementHooksModule copyFrom = null)
        {
            if (copyFrom == null)
            {
                this.check = new PlacementHook();
                this.postPlaceEveryone = new PlacementHook();
                this.postPlaceMyPlayer = new PlacementHook();
                this.placeOverride = new PlacementHook();
            }
            else
            {
                this.check = copyFrom.check;
                this.postPlaceEveryone = copyFrom.postPlaceEveryone;
                this.postPlaceMyPlayer = copyFrom.postPlaceMyPlayer;
                this.placeOverride = copyFrom.placeOverride;
            }
        }
    }
}
