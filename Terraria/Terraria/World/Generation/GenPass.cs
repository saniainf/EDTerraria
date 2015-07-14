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
    internal abstract class GenPass : GenBase
    {
        public string Name;
        public float Weight;

        public GenPass(string name, float loadWeight)
        {
            Name = name;
            Weight = loadWeight;
        }

        public abstract void Apply(GenerationProgress progress);
    }
}
