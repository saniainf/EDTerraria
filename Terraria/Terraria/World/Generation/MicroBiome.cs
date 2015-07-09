/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

namespace Terraria.World.Generation
{
    public abstract class MicroBiome : GenStructure
    {
        public virtual void Reset()
        {
        }

        public static void ResetAll()
        {
            foreach (MicroBiome microBiome in BiomeCollection.Biomes)
                microBiome.Reset();
        }
    }
}
