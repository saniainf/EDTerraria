/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

namespace Terraria
{
    public class Sign
    {
        public const int maxSigns = 1000;
        public int x;
        public int y;
        public string text;

        public static void KillSign(int x, int y)
        {
            for (int index = 0; index < 1000; ++index)
            {
                if (Main.sign[index] != null && Main.sign[index].x == x && Main.sign[index].y == y)
                    Main.sign[index] = (Sign)null;
            }
        }

        public static int ReadSign(int i, int j, bool CreateIfMissing = true)
        {
            int num1 = (int)Main.tile[i, j].frameX / 18;
            int num2 = (int)Main.tile[i, j].frameY / 18;
            int num3 = num1 % 2;
            int x = i - num3;
            int y = j - num2;
            if (!Main.tileSign[(int)Main.tile[x, y].type])
            {
                Sign.KillSign(x, y);
                return -1;
            }
            int num4 = -1;
            for (int index = 0; index < 1000; ++index)
            {
                if (Main.sign[index] != null && Main.sign[index].x == x && Main.sign[index].y == y)
                {
                    num4 = index;
                    break;
                }
            }
            if (num4 < 0 && CreateIfMissing)
            {
                for (int index = 0; index < 1000; ++index)
                {
                    if (Main.sign[index] == null)
                    {
                        num4 = index;
                        Main.sign[index] = new Sign();
                        Main.sign[index].x = x;
                        Main.sign[index].y = y;
                        Main.sign[index].text = "";
                        break;
                    }
                }
            }
            return num4;
        }

        public static void TextSign(int i, string text)
        {
            if (Main.tile[Main.sign[i].x, Main.sign[i].y] == null || !Main.tile[Main.sign[i].x, Main.sign[i].y].active() || (int)Main.tile[Main.sign[i].x, Main.sign[i].y].type != 55 && (int)Main.tile[Main.sign[i].x, Main.sign[i].y].type != 85)
                Main.sign[i] = (Sign)null;
            else
                Main.sign[i].text = text;
        }

        public override string ToString()
        {
            return "x" + (object)this.x + "\ty" + (string)(object)this.y + "\t" + this.text;
        }
    }
}
