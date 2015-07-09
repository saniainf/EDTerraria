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
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.UI.Chat;

namespace Terraria.GameContent.Events
{
    internal class BlameNPCTest
    {
        public static Dictionary<int, int> npcTypes = new Dictionary<int, int>();
        public static List<KeyValuePair<int, int>> mostSeen = new List<KeyValuePair<int, int>>();

        public static void Update(int newEntry)
        {
            if (BlameNPCTest.npcTypes.ContainsKey(newEntry))
            {
                Dictionary<int, int> dictionary;
                int index;
                (dictionary = BlameNPCTest.npcTypes)[index = newEntry] = dictionary[index] + 1;
            }
            else
                BlameNPCTest.npcTypes[newEntry] = 1;
            BlameNPCTest.mostSeen = Enumerable.ToList<KeyValuePair<int, int>>((IEnumerable<KeyValuePair<int, int>>)BlameNPCTest.npcTypes);
            BlameNPCTest.mostSeen.Sort((Comparison<KeyValuePair<int, int>>)((x, y) => x.Value.CompareTo(y.Value)));
        }

        public static void Draw(SpriteBatch sb)
        {
            if (Main.netDiag || Main.showFrameRate)
                return;
            for (int index = 0; index < BlameNPCTest.mostSeen.Count; ++index)
            {
                int num1 = 200 + index % 13 * 100;
                int num2 = 200 + index / 13 * 30;
                ChatManager.DrawColorCodedString(sb, Main.fontItemStack, string.Concat(new object[4]
        {
          (object) BlameNPCTest.mostSeen[index].Key,
          (object) " (",
          (object) BlameNPCTest.mostSeen[index].Value,
          (object) ")"
        }), new Vector2((float)num1, (float)num2), Color.White, 0.0f, Vector2.Zero, Vector2.One, -1f, 0 != 0);
            }
        }
    }
}
