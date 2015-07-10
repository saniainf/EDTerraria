/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using System.Collections.Generic;
using Terraria;

namespace Terraria.Modules
{
    public class TileObjectSubTilesModule
    {
        public List<TileObjectData> data;

        public TileObjectSubTilesModule(TileObjectSubTilesModule copyFrom = null, List<TileObjectData> newData = null)
        {
            if (copyFrom == null)
                data = null;
            else if (copyFrom.data == null)
                data = null;
            else
            {
                data = new List<TileObjectData>(copyFrom.data.Count);
                for (int index = 0; index < data.Count; ++index)
                    data.Add(new TileObjectData(copyFrom.data[index]));
            }
        }
    }
}
