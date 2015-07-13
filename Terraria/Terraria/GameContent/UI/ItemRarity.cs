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
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace Terraria.GameContent.UI
{
    public class ItemRarity
    {
        private static Dictionary<int, Color> rarities = new Dictionary<int, Color>();

        public static void Initialize()
        {
            rarities.Clear();
            rarities.Add(-11, Colors.RarityAmber);
            rarities.Add(-1, Colors.RarityTrash);
            rarities.Add(1, Colors.RarityBlue);
            rarities.Add(2, Colors.RarityGreen);
            rarities.Add(3, Colors.RarityOrange);
            rarities.Add(4, Colors.RarityRed);
            rarities.Add(5, Colors.RarityPink);
            rarities.Add(6, Colors.RarityPurple);
            rarities.Add(7, Colors.RarityLime);
            rarities.Add(8, Colors.RarityYellow);
            rarities.Add(9, Colors.RarityCyan);
        }

        public static Color GetColor(int rarity)
        {
            Color color = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
            if (rarities.ContainsKey(rarity))
                return rarities[rarity];
            return color;
        }
    }
}
