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

namespace Terraria.Graphics.Shaders
{
    internal class GameShaders
    {
        public static ArmorShaderDataSet Armor = new ArmorShaderDataSet();
        public static HairShaderDataSet Hair = new HairShaderDataSet();
        public static Dictionary<string, MiscShaderData> Misc = new Dictionary<string, MiscShaderData>();
    }
}
