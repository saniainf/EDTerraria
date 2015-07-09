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
                this.data = (List<TileObjectData>)null;
            else if (copyFrom.data == null)
            {
                this.data = (List<TileObjectData>)null;
            }
            else
            {
                this.data = new List<TileObjectData>(copyFrom.data.Count);
                for (int index = 0; index < this.data.Count; ++index)
                    this.data.Add(new TileObjectData(copyFrom.data[index]));
            }
        }
    }
}
