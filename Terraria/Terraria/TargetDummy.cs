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

namespace Terraria
{
    public class TargetDummy
    {
        public static TargetDummy[] dummies = new TargetDummy[1000];
        public const int MaxDummies = 1000;
        public short x;
        public short y;
        public int npc;
        public int whoAmI;

        public TargetDummy(int x, int y)
        {
            this.x = (short)x;
            this.y = (short)y;
            this.npc = -1;
        }

        public static void UpdateDummies()
        {
            Dictionary<int, Rectangle> dictionary = new Dictionary<int, Rectangle>();
            bool flag1 = false;
            Rectangle rectangle = new Rectangle(0, 0, 32, 48);
            rectangle.Inflate(1600, 1600);
            int num1 = rectangle.X;
            int num2 = rectangle.Y;
            for (int index1 = 0; index1 < 1000; ++index1)
            {
                if (TargetDummy.dummies[index1] != null)
                {
                    TargetDummy.dummies[index1].whoAmI = index1;
                    if (TargetDummy.dummies[index1].npc != -1)
                    {
                        if (!Main.npc[TargetDummy.dummies[index1].npc].active || Main.npc[TargetDummy.dummies[index1].npc].type != 488 || ((double)Main.npc[TargetDummy.dummies[index1].npc].ai[0] != (double)TargetDummy.dummies[index1].x || (double)Main.npc[TargetDummy.dummies[index1].npc].ai[1] != (double)TargetDummy.dummies[index1].y))
                            TargetDummy.dummies[index1].Deactivate();
                    }
                    else
                    {
                        if (!flag1)
                        {
                            for (int index2 = 0; index2 < (int)byte.MaxValue; ++index2)
                            {
                                if (Main.player[index2].active)
                                    dictionary[index2] = Main.player[index2].getRect();
                            }
                            flag1 = true;
                        }
                        rectangle.X = (int)TargetDummy.dummies[index1].x * 16 + num1;
                        rectangle.Y = (int)TargetDummy.dummies[index1].y * 16 + num2;
                        bool flag2 = false;
                        foreach (KeyValuePair<int, Rectangle> keyValuePair in dictionary)
                        {
                            if (keyValuePair.Value.Intersects(rectangle))
                            {
                                flag2 = true;
                                break;
                            }
                        }
                        if (flag2)
                            TargetDummy.dummies[index1].Activate();
                    }
                }
            }
        }

        public static int Find(int x, int y)
        {
            for (int index = 0; index < 1000; ++index)
            {
                if (TargetDummy.dummies[index] != null && (int)TargetDummy.dummies[index].x == x && (int)TargetDummy.dummies[index].y == y)
                    return index;
            }
            return -1;
        }

        public static int Place(int x, int y)
        {
            int index1 = -1;
            for (int index2 = 0; index2 < 1000; ++index2)
            {
                if (TargetDummy.dummies[index2] == null)
                {
                    index1 = index2;
                    break;
                }
            }
            if (index1 == -1)
                return index1;
            TargetDummy.dummies[index1] = new TargetDummy(x, y);
            return index1;
        }

        public static void Kill(int x, int y)
        {
            for (int index = 0; index < 1000; ++index)
            {
                TargetDummy targetDummy = TargetDummy.dummies[index];
                if (targetDummy != null && (int)targetDummy.x == x && (int)targetDummy.y == y)
                    TargetDummy.dummies[index] = (TargetDummy)null;
            }
        }

        public static int Hook_AfterPlacement(int x, int y, int type = 21, int style = 0, int direction = 1)
        {
            if (Main.netMode != 1)
                return TargetDummy.Place(x - 1, y - 2);
            NetMessage.SendTileSquare(Main.myPlayer, x - 1, y - 1, 3);
            NetMessage.SendData(87, -1, -1, "", x - 1, (float)(y - 2), 0.0f, 0.0f, 0, 0, 0);
            return -1;
        }

        public void Activate()
        {
            int index = NPC.NewNPC((int)this.x * 16 + 16, (int)this.y * 16 + 48, 488, 100, 0.0f, 0.0f, 0.0f, 0.0f, (int)byte.MaxValue);
            Main.npc[index].ai[0] = (float)this.x;
            Main.npc[index].ai[1] = (float)this.y;
            Main.npc[index].netUpdate = true;
            this.npc = index;
            if (Main.netMode == 1)
                return;
            NetMessage.SendData(86, -1, -1, "", this.whoAmI, (float)this.x, (float)this.y, 0.0f, 0, 0, 0);
        }

        public void Deactivate()
        {
            if (this.npc != -1)
                Main.npc[this.npc].active = false;
            this.npc = -1;
            if (Main.netMode == 1)
                return;
            NetMessage.SendData(86, -1, -1, "", this.whoAmI, (float)this.x, (float)this.y, 0.0f, 0, 0, 0);
        }

        public override string ToString()
        {
            return (string)(object)this.x + (object)"x  " + (string)(object)this.y + "y npc: " + (string)(object)this.npc;
        }
    }
}
