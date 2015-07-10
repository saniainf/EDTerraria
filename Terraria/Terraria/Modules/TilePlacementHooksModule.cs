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
                check = new PlacementHook();
                postPlaceEveryone = new PlacementHook();
                postPlaceMyPlayer = new PlacementHook();
                placeOverride = new PlacementHook();
            }
            else
            {
                check = copyFrom.check;
                postPlaceEveryone = copyFrom.postPlaceEveryone;
                postPlaceMyPlayer = copyFrom.postPlaceMyPlayer;
                placeOverride = copyFrom.placeOverride;
            }
        }
    }
}
