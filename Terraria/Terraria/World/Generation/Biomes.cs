/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using Microsoft.Xna.Framework;
using System;

namespace Terraria.World.Generation
{
    public static class Biomes<T> where T : MicroBiome, new()
    {
        private static T _microBiome = Biomes<T>.CreateInstance();

        public static bool Place(int x, int y, StructureMap structures)
        {
            return Biomes<T>._microBiome.Place(new Point(x, y), structures);
        }

        public static bool Place(Point origin, StructureMap structures)
        {
            return Biomes<T>._microBiome.Place(origin, structures);
        }

        public static T Get()
        {
            return Biomes<T>._microBiome;
        }

        private static T CreateInstance()
        {
            T instance = Activator.CreateInstance<T>();
            BiomeCollection.Biomes.Add((MicroBiome)instance);
            return instance;
        }
    }
}
