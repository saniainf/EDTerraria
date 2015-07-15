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
using Terraria.DataStructures;
using Terraria.GameContent.Achievements;

namespace Terraria
{
    public class Chest
    {
        public static int[] chestTypeToIcon = new int[52];
        public static int[] chestItemSpawn = new int[52];
        public static int[] dresserTypeToIcon = new int[28];
        public static int[] dresserItemSpawn = new int[28];
        public const int maxChestTypes = 52;
        public const int maxDresserTypes = 28;
        public const int maxItems = 40;
        public const int MaxNameLength = 20;
        public Item[] item;
        public int x;
        public int y;
        public bool bankChest;
        public string name;
        public int frameCounter;
        public int frame;

        public Chest(bool bank = false)
        {
            item = new Item[40];
            bankChest = bank;
            name = string.Empty;
        }

        public override string ToString()
        {
            int num = 0;
            for (int index = 0; index < item.Length; ++index)
            {
                if (item[index].stack > 0)
                    ++num;
            }
            return string.Format("{{X: {0}, Y: {1}, Count: {2}}}", x, y, num);
        }

        public static void Initialize()
        {
            chestTypeToIcon[0] = chestItemSpawn[0] = 48;
            chestTypeToIcon[1] = chestItemSpawn[1] = 306;
            chestTypeToIcon[2] = 327;
            chestItemSpawn[2] = 306;
            chestTypeToIcon[3] = chestItemSpawn[3] = 328;
            chestTypeToIcon[4] = 329;
            chestItemSpawn[4] = 328;
            chestTypeToIcon[5] = chestItemSpawn[5] = 343;
            chestTypeToIcon[6] = chestItemSpawn[6] = 348;
            chestTypeToIcon[7] = chestItemSpawn[7] = 625;
            chestTypeToIcon[8] = chestItemSpawn[8] = 626;
            chestTypeToIcon[9] = chestItemSpawn[9] = 627;
            chestTypeToIcon[10] = chestItemSpawn[10] = 680;
            chestTypeToIcon[11] = chestItemSpawn[11] = 681;
            chestTypeToIcon[12] = chestItemSpawn[12] = 831;
            chestTypeToIcon[13] = chestItemSpawn[13] = 838;
            chestTypeToIcon[14] = chestItemSpawn[14] = 914;
            chestTypeToIcon[15] = chestItemSpawn[15] = 952;
            chestTypeToIcon[16] = chestItemSpawn[16] = 1142;
            chestTypeToIcon[17] = chestItemSpawn[17] = 1298;
            chestTypeToIcon[18] = chestItemSpawn[18] = 1528;
            chestTypeToIcon[19] = chestItemSpawn[19] = 1529;
            chestTypeToIcon[20] = chestItemSpawn[20] = 1530;
            chestTypeToIcon[21] = chestItemSpawn[21] = 1531;
            chestTypeToIcon[22] = chestItemSpawn[22] = 1532;
            chestTypeToIcon[23] = 1533;
            chestItemSpawn[23] = 1528;
            chestTypeToIcon[24] = 1534;
            chestItemSpawn[24] = 1529;
            chestTypeToIcon[25] = 1535;
            chestItemSpawn[25] = 1530;
            chestTypeToIcon[26] = 1536;
            chestItemSpawn[26] = 1531;
            chestTypeToIcon[27] = 1537;
            chestItemSpawn[27] = 1532;
            chestTypeToIcon[28] = chestItemSpawn[28] = 2230;
            chestTypeToIcon[29] = chestItemSpawn[29] = 2249;
            chestTypeToIcon[30] = chestItemSpawn[30] = 2250;
            chestTypeToIcon[31] = chestItemSpawn[31] = 2526;
            chestTypeToIcon[32] = chestItemSpawn[32] = 2544;
            chestTypeToIcon[33] = chestItemSpawn[33] = 2559;
            chestTypeToIcon[34] = chestItemSpawn[34] = 2574;
            chestTypeToIcon[35] = chestItemSpawn[35] = 2612;
            chestTypeToIcon[36] = 327;
            chestItemSpawn[36] = 2612;
            chestTypeToIcon[37] = chestItemSpawn[37] = 2613;
            chestTypeToIcon[38] = 327;
            chestItemSpawn[38] = 2613;
            chestTypeToIcon[39] = chestItemSpawn[39] = 2614;
            chestTypeToIcon[40] = 327;
            chestItemSpawn[40] = 2614;
            chestTypeToIcon[41] = chestItemSpawn[41] = 2615;
            chestTypeToIcon[42] = chestItemSpawn[42] = 2616;
            chestTypeToIcon[43] = chestItemSpawn[43] = 2617;
            chestTypeToIcon[44] = chestItemSpawn[44] = 2618;
            chestTypeToIcon[45] = chestItemSpawn[45] = 2619;
            chestTypeToIcon[46] = chestItemSpawn[46] = 2620;
            chestTypeToIcon[47] = chestItemSpawn[47] = 2748;
            chestTypeToIcon[48] = chestItemSpawn[48] = 2814;
            chestTypeToIcon[49] = chestItemSpawn[49] = 3180;
            chestTypeToIcon[50] = chestItemSpawn[50] = 3125;
            chestTypeToIcon[51] = chestItemSpawn[51] = 3181;
            dresserTypeToIcon[0] = dresserItemSpawn[0] = 334;
            dresserTypeToIcon[1] = dresserItemSpawn[1] = 647;
            dresserTypeToIcon[2] = dresserItemSpawn[2] = 648;
            dresserTypeToIcon[3] = dresserItemSpawn[3] = 649;
            dresserTypeToIcon[4] = dresserItemSpawn[4] = 918;
            dresserTypeToIcon[5] = dresserItemSpawn[5] = 2386;
            dresserTypeToIcon[6] = dresserItemSpawn[6] = 2387;
            dresserTypeToIcon[7] = dresserItemSpawn[7] = 2388;
            dresserTypeToIcon[8] = dresserItemSpawn[8] = 2389;
            dresserTypeToIcon[9] = dresserItemSpawn[9] = 2390;
            dresserTypeToIcon[10] = dresserItemSpawn[10] = 2391;
            dresserTypeToIcon[11] = dresserItemSpawn[11] = 2392;
            dresserTypeToIcon[12] = dresserItemSpawn[12] = 2393;
            dresserTypeToIcon[13] = dresserItemSpawn[13] = 2394;
            dresserTypeToIcon[14] = dresserItemSpawn[14] = 2395;
            dresserTypeToIcon[15] = dresserItemSpawn[15] = 2396;
            dresserTypeToIcon[16] = dresserItemSpawn[16] = 2529;
            dresserTypeToIcon[17] = dresserItemSpawn[17] = 2545;
            dresserTypeToIcon[18] = dresserItemSpawn[18] = 2562;
            dresserTypeToIcon[19] = dresserItemSpawn[19] = 2577;
            dresserTypeToIcon[20] = dresserItemSpawn[20] = 2637;
            dresserTypeToIcon[21] = dresserItemSpawn[21] = 2638;
            dresserTypeToIcon[22] = dresserItemSpawn[22] = 2639;
            dresserTypeToIcon[23] = dresserItemSpawn[23] = 2640;
            dresserTypeToIcon[24] = dresserItemSpawn[24] = 2816;
            dresserTypeToIcon[25] = dresserItemSpawn[25] = 3132;
            dresserTypeToIcon[26] = dresserItemSpawn[26] = 3134;
            dresserTypeToIcon[27] = dresserItemSpawn[27] = 3133;
        }

        private static bool IsPlayerInChest(int i)
        {
            for (int index = 0; index < 255; ++index)
            {
                if (Main.player[index].chest == i)
                    return true;
            }
            return false;
        }

        public static bool isLocked(int x, int y)
        {
            return Main.tile[x, y] == null || Main.tile[x, y].frameX >= 72 && Main.tile[x, y].frameX <= 106 || (Main.tile[x, y].frameX >= 144 && Main.tile[x, y].frameX <= 178 || Main.tile[x, y].frameX >= 828 && Main.tile[x, y].frameX <= 1006) || (Main.tile[x, y].frameX >= 1296 && Main.tile[x, y].frameX <= 1330 || Main.tile[x, y].frameX >= 1368 && Main.tile[x, y].frameX <= 1402 || Main.tile[x, y].frameX >= 1440 && Main.tile[x, y].frameX <= 1474);
        }

        public static void ServerPlaceItem(int plr, int slot)
        {
            Main.player[plr].inventory[slot] = Chest.PutItemInNearbyChest(Main.player[plr].inventory[slot], Main.player[plr].Center);
            NetMessage.SendData(5, -1, -1, "", plr, slot, Main.player[plr].inventory[slot].prefix, 0.0f, 0, 0, 0);
        }

        public static Item PutItemInNearbyChest(Item item, Vector2 position)
        {
            if (Main.netMode == 1)
                return item;
            for (int i = 0; i < 1000; ++i)
            {
                bool flag1 = false;
                bool flag2 = false;
                if (Main.chest[i] != null && !IsPlayerInChest(i) && !isLocked(Main.chest[i].x, Main.chest[i].y) && (new Vector2((Main.chest[i].x * 16 + 16), (Main.chest[i].y * 16 + 16)) - position).Length() < 200.0)
                {
                    for (int index = 0; index < Main.chest[i].item.Length; ++index)
                    {
                        if (Main.chest[i].item[index].type > 0 && Main.chest[i].item[index].stack > 0)
                        {
                            if (item.IsTheSameAs(Main.chest[i].item[index]))
                            {
                                flag1 = true;
                                int num = Main.chest[i].item[index].maxStack - Main.chest[i].item[index].stack;
                                if (num > 0)
                                {
                                    if (num > item.stack)
                                        num = item.stack;
                                    item.stack -= num;
                                    Main.chest[i].item[index].stack += num;
                                    if (item.stack <= 0)
                                    {
                                        item.SetDefaults(0, false);
                                        return item;
                                    }
                                }
                            }
                        }
                        else
                            flag2 = true;
                    }
                    if (flag1 && flag2 && item.stack > 0)
                    {
                        for (int index = 0; index < Main.chest[i].item.Length; ++index)
                        {
                            if (Main.chest[i].item[index].type == 0 || Main.chest[i].item[index].stack == 0)
                            {
                                Main.chest[i].item[index] = item.Clone();
                                item.SetDefaults(0, false);
                                return item;
                            }
                        }
                    }
                }
            }
            return item;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public static bool Unlock(int X, int Y)
        {
            if (Main.tile[X, Y] == null)
                return false;
            short num;
            int Type;
            switch (Main.tile[X, Y].frameX / 36)
            {
                case 2:
                    num = 36;
                    Type = 11;
                    AchievementsHelper.NotifyProgressionEvent(19);
                    break;
                case 4:
                    num = 36;
                    Type = 11;
                    break;
                case 23:
                case 24:
                case 25:
                case 26:
                case 27:
                    if (!NPC.downedPlantBoss)
                        return false;
                    num = 180;
                    Type = 11;
                    AchievementsHelper.NotifyProgressionEvent(20);
                    break;
                case 36:
                case 38:
                case 40:
                    num = 36;
                    Type = 11;
                    break;
                default:
                    return false;
            }
            Main.PlaySound(22, X * 16, Y * 16, 1);
            for (int index1 = X; index1 <= X + 1; ++index1)
            {
                for (int index2 = Y; index2 <= Y + 1; ++index2)
                {
                    Main.tile[index1, index2].frameX -= num;
                    for (int index3 = 0; index3 < 4; ++index3)
                        Dust.NewDust(new Vector2((index1 * 16), (index2 * 16)), 16, 16, Type, 0.0f, 0.0f, 0, new Color(), 1f);
                }
            }
            return true;
        }

        public static int UsingChest(int i)
        {
            if (Main.chest[i] != null)
            {
                for (int index = 0; index < 255; ++index)
                {
                    if (Main.player[index].active && Main.player[index].chest == i)
                        return index;
                }
            }
            return -1;
        }

        public static int FindChest(int X, int Y)
        {
            for (int index = 0; index < 1000; ++index)
            {
                if (Main.chest[index] != null && Main.chest[index].x == X && Main.chest[index].y == Y)
                    return index;
            }
            return -1;
        }

        public static int FindEmptyChest(int x, int y, int type = 21, int style = 0, int direction = 1)
        {
            int num = -1;
            for (int index = 0; index < 1000; ++index)
            {
                Chest chest = Main.chest[index];
                if (chest != null)
                {
                    if (chest.x == x && chest.y == y)
                        return -1;
                }
                else if (num == -1)
                    num = index;
            }
            return num;
        }

        public static bool NearOtherChests(int x, int y)
        {
            for (int i = x - 25; i < x + 25; ++i)
            {
                for (int j = y - 8; j < y + 8; ++j)
                {
                    Tile tileSafely = Framing.GetTileSafely(i, j);
                    if (tileSafely.active() && tileSafely.type == 21)
                        return true;
                }
            }
            return false;
        }

        public static int AfterPlacement_Hook(int x, int y, int type = 21, int style = 0, int direction = 1)
        {
            Point16 baseCoords = new Point16(x, y);
            TileObjectData.OriginToTopLeft(type, style, ref baseCoords);
            int emptyChest = FindEmptyChest(baseCoords.X, baseCoords.Y, 21, 0, 1);
            if (emptyChest == -1)
                return -1;
            if (Main.netMode != 1)
            {
                Chest chest = new Chest(false);
                chest.x = baseCoords.X;
                chest.y = baseCoords.Y;
                for (int index = 0; index < 40; ++index)
                    chest.item[index] = new Item();
                Main.chest[emptyChest] = chest;
            }
            else if (type == 21)
                NetMessage.SendData(34, -1, -1, "", 0, (float)x, (float)y, (float)style, 0, 0, 0);
            else
                NetMessage.SendData(34, -1, -1, "", 2, (float)x, (float)y, (float)style, 0, 0, 0);
            return emptyChest;
        }

        public static int CreateChest(int X, int Y, int id = -1)
        {
            int index1 = id;
            if (index1 == -1)
            {
                index1 = Chest.FindEmptyChest(X, Y, 21, 0, 1);
                if (index1 == -1)
                    return -1;
                if (Main.netMode == 1)
                    return index1;
            }
            Main.chest[index1] = new Chest(false);
            Main.chest[index1].x = X;
            Main.chest[index1].y = Y;
            for (int index2 = 0; index2 < 40; ++index2)
                Main.chest[index1].item[index2] = new Item();
            return index1;
        }

        public static bool CanDestroyChest(int X, int Y)
        {
            for (int index1 = 0; index1 < 1000; ++index1)
            {
                Chest chest = Main.chest[index1];
                if (chest != null && chest.x == X && chest.y == Y)
                {
                    for (int index2 = 0; index2 < 40; ++index2)
                    {
                        if (chest.item[index2] != null && chest.item[index2].type > 0 && chest.item[index2].stack > 0)
                            return false;
                    }
                    return true;
                }
            }
            return true;
        }

        public static bool DestroyChest(int X, int Y)
        {
            for (int index1 = 0; index1 < 1000; ++index1)
            {
                Chest chest = Main.chest[index1];
                if (chest != null && chest.x == X && chest.y == Y)
                {
                    for (int index2 = 0; index2 < 40; ++index2)
                    {
                        if (chest.item[index2] != null && chest.item[index2].type > 0 && chest.item[index2].stack > 0)
                            return false;
                    }
                    Main.chest[index1] = null;
                    if (Main.player[Main.myPlayer].chest == index1)
                        Main.player[Main.myPlayer].chest = -1;
                    return true;
                }
            }
            return true;
        }

        public static void DestroyChestDirect(int X, int Y, int id)
        {
            if (id < 0)
                return;
            if (id >= Main.chest.Length)
                return;
            try
            {
                Chest chest = Main.chest[id];
                if (chest == null || chest.x != X || chest.y != Y)
                    return;
                Main.chest[id] = null;
                if (Main.player[Main.myPlayer].chest != id)
                    return;
                Main.player[Main.myPlayer].chest = -1;
            }
            catch
            {
            }
        }

        public void AddShop(Item newItem)
        {
            for (int index = 0; index < 39; ++index)
            {
                if (item[index] == null || item[index].type == 0)
                {
                    item[index] = newItem.Clone();
                    item[index].favorited = false;
                    item[index].buyOnce = true;
                    if (item[index].value <= 0)
                        break;
                    item[index].value = item[index].value / 5;
                    if (item[index].value >= 1)
                        break;
                    item[index].value = 1;
                    break;
                }
            }
        }

        public static void SetupTravelShop()
        {
            for (int index = 0; index < 40; ++index)
                Main.travelShop[index] = 0;
            int num1 = Main.rand.Next(4, 7);
            if (Main.rand.Next(4) == 0)
                ++num1;
            if (Main.rand.Next(8) == 0)
                ++num1;
            if (Main.rand.Next(16) == 0)
                ++num1;
            if (Main.rand.Next(32) == 0)
                ++num1;
            if (Main.expertMode && Main.rand.Next(2) == 0)
                ++num1;
            int index1 = 0;
            int num2 = 0;
            int[] numArray = new int[6]
      {
        100,
        200,
        300,
        400,
        500,
        600
      };
            while (num2 < num1)
            {
                int num3 = 0;
                if (Main.rand.Next(numArray[4]) == 0)
                    num3 = 3309;
                if (Main.rand.Next(numArray[3]) == 0)
                    num3 = 3314;
                if (Main.rand.Next(numArray[5]) == 0)
                    num3 = 1987;
                if (Main.rand.Next(numArray[4]) == 0 && Main.hardMode)
                    num3 = 2270;
                if (Main.rand.Next(numArray[4]) == 0)
                    num3 = 2278;
                if (Main.rand.Next(numArray[4]) == 0)
                    num3 = 2271;
                if (Main.rand.Next(numArray[3]) == 0 && Main.hardMode && NPC.downedPlantBoss)
                    num3 = 2223;
                if (Main.rand.Next(numArray[3]) == 0)
                    num3 = 2272;
                if (Main.rand.Next(numArray[3]) == 0)
                    num3 = 2219;
                if (Main.rand.Next(numArray[3]) == 0)
                    num3 = 2276;
                if (Main.rand.Next(numArray[3]) == 0)
                    num3 = 2284;
                if (Main.rand.Next(numArray[3]) == 0)
                    num3 = 2285;
                if (Main.rand.Next(numArray[3]) == 0)
                    num3 = 2286;
                if (Main.rand.Next(numArray[3]) == 0)
                    num3 = 2287;
                if (Main.rand.Next(numArray[3]) == 0)
                    num3 = 2296;
                if (Main.rand.Next(numArray[2]) == 0 && WorldGen.shadowOrbSmashed)
                    num3 = 2269;
                if (Main.rand.Next(numArray[2]) == 0)
                    num3 = 2177;
                if (Main.rand.Next(numArray[2]) == 0)
                    num3 = 1988;
                if (Main.rand.Next(numArray[2]) == 0)
                    num3 = 2275;
                if (Main.rand.Next(numArray[2]) == 0)
                    num3 = 2279;
                if (Main.rand.Next(numArray[2]) == 0)
                    num3 = 2277;
                if (Main.rand.Next(numArray[2]) == 0 && NPC.downedBoss1)
                    num3 = 3262;
                if (Main.rand.Next(numArray[2]) == 0 && NPC.downedMechBossAny)
                    num3 = 3284;
                if (Main.rand.Next(numArray[2]) == 0 && Main.hardMode && NPC.downedMoonlord)
                    num3 = 3596;
                if (Main.rand.Next(numArray[2]) == 0 && Main.hardMode && NPC.downedMartians)
                    num3 = 2865;
                if (Main.rand.Next(numArray[2]) == 0 && Main.hardMode && NPC.downedMartians)
                    num3 = 2866;
                if (Main.rand.Next(numArray[2]) == 0 && Main.hardMode && NPC.downedMartians)
                    num3 = 2867;
                if (Main.rand.Next(numArray[2]) == 0 && Main.xMas)
                    num3 = 3055;
                if (Main.rand.Next(numArray[2]) == 0 && Main.xMas)
                    num3 = 3056;
                if (Main.rand.Next(numArray[2]) == 0 && Main.xMas)
                    num3 = 3057;
                if (Main.rand.Next(numArray[2]) == 0 && Main.xMas)
                    num3 = 3058;
                if (Main.rand.Next(numArray[2]) == 0 && Main.xMas)
                    num3 = 3059;
                if (Main.rand.Next(numArray[1]) == 0)
                    num3 = 2214;
                if (Main.rand.Next(numArray[1]) == 0)
                    num3 = 2215;
                if (Main.rand.Next(numArray[1]) == 0)
                    num3 = 2216;
                if (Main.rand.Next(numArray[1]) == 0)
                    num3 = 2217;
                if (Main.rand.Next(numArray[1]) == 0)
                    num3 = 2273;
                if (Main.rand.Next(numArray[1]) == 0)
                    num3 = 2274;
                if (Main.rand.Next(numArray[0]) == 0)
                    num3 = 2266;
                if (Main.rand.Next(numArray[0]) == 0)
                    num3 = 2267;
                if (Main.rand.Next(numArray[0]) == 0)
                    num3 = 2268;
                if (Main.rand.Next(numArray[0]) == 0)
                    num3 = 2281 + Main.rand.Next(3);
                if (Main.rand.Next(numArray[0]) == 0)
                    num3 = 2258;
                if (Main.rand.Next(numArray[0]) == 0)
                    num3 = 2242;
                if (Main.rand.Next(numArray[0]) == 0)
                    num3 = 2260;
                if (Main.rand.Next(numArray[0]) == 0)
                    num3 = 3119;
                if (Main.rand.Next(numArray[0]) == 0)
                    num3 = 3118;
                if (Main.rand.Next(numArray[0]) == 0)
                    num3 = 3099;
                if (num3 != 0)
                {
                    for (int index2 = 0; index2 < 40; ++index2)
                    {
                        if (Main.travelShop[index2] == num3)
                        {
                            num3 = 0;
                            break;
                        }
                    }
                }
                if (num3 != 0)
                {
                    ++num2;
                    Main.travelShop[index1] = num3;
                    ++index1;
                    if (num3 == 2260)
                    {
                        Main.travelShop[index1] = 2261;
                        int index2 = index1 + 1;
                        Main.travelShop[index2] = 2262;
                        index1 = index2 + 1;
                    }
                }
            }
        }

        public void SetupShop(int type)
        {
            for (int index = 0; index < 40; ++index)
                item[index] = new Item();
            int index1 = 0;
            if (type == 1)
            {
                item[index1].SetDefaults("Mining Helmet");
                int index2 = index1 + 1;
                item[index2].SetDefaults("Piggy Bank");
                int index3 = index2 + 1;
                item[index3].SetDefaults("Iron Anvil");
                int index4 = index3 + 1;
                item[index4].SetDefaults(1991, false);
                int index5 = index4 + 1;
                item[index5].SetDefaults("Copper Pickaxe");
                int index6 = index5 + 1;
                item[index6].SetDefaults("Copper Axe");
                int index7 = index6 + 1;
                item[index7].SetDefaults("Torch");
                int index8 = index7 + 1;
                item[index8].SetDefaults("Lesser Healing Potion");
                int index9 = index8 + 1;
                item[index9].SetDefaults("Lesser Mana Potion");
                int index10 = index9 + 1;
                item[index10].SetDefaults("Wooden Arrow");
                int index11 = index10 + 1;
                item[index11].SetDefaults("Shuriken");
                int index12 = index11 + 1;
                item[index12].SetDefaults("Rope");
                int index13 = index12 + 1;
                if (Main.player[Main.myPlayer].ZoneSnow)
                {
                    item[index13].SetDefaults(967, false);
                    ++index13;
                }
                if (Main.bloodMoon)
                {
                    item[index13].SetDefaults("Throwing Knife");
                    ++index13;
                }
                if (!Main.dayTime)
                {
                    item[index13].SetDefaults("Glowstick");
                    ++index13;
                }
                if (NPC.downedBoss3)
                {
                    item[index13].SetDefaults("Safe");
                    ++index13;
                }
                if (Main.hardMode)
                {
                    item[index13].SetDefaults(488, false);
                    ++index13;
                }
                for (int index14 = 0; index14 < 58; ++index14)
                {
                    if (Main.player[Main.myPlayer].inventory[index14].type == 930)
                    {
                        item[index13].SetDefaults(931, false);
                        int index15 = index13 + 1;
                        item[index15].SetDefaults(1614, false);
                        index13 = index15 + 1;
                        break;
                    }
                }
                item[index13].SetDefaults(1786, false);
                index1 = index13 + 1;
                if (Main.hardMode)
                {
                    item[index1].SetDefaults(1348, false);
                    ++index1;
                }
                if (Main.player[Main.myPlayer].HasItem(3107))
                {
                    item[index1].SetDefaults(3108, false);
                    ++index1;
                }
                if (Main.halloween)
                {
                    Item[] objArray1 = item;
                    int index14 = index1;
                    int num1 = 1;
                    int num2 = index14 + num1;
                    objArray1[index14].SetDefaults(3242, false);
                    Item[] objArray2 = item;
                    int index15 = num2;
                    int num3 = 1;
                    int num4 = index15 + num3;
                    objArray2[index15].SetDefaults(3243, false);
                    Item[] objArray3 = item;
                    int index16 = num4;
                    int num5 = 1;
                    index1 = index16 + num5;
                    objArray3[index16].SetDefaults(3244, false);
                }
            }
            else if (type == 2)
            {
                item[index1].SetDefaults("Musket Ball");
                int index2 = index1 + 1;
                if (Main.bloodMoon || Main.hardMode)
                {
                    item[index2].SetDefaults("Silver Bullet");
                    ++index2;
                }
                if (NPC.downedBoss2 && !Main.dayTime || Main.hardMode)
                {
                    item[index2].SetDefaults(47, false);
                    ++index2;
                }
                item[index2].SetDefaults("Flintlock Pistol");
                int index3 = index2 + 1;
                item[index3].SetDefaults("Minishark");
                index1 = index3 + 1;
                if (!Main.dayTime)
                {
                    item[index1].SetDefaults(324, false);
                    ++index1;
                }
                if (Main.hardMode)
                {
                    item[index1].SetDefaults(534, false);
                    ++index1;
                }
                if (Main.hardMode)
                {
                    item[index1].SetDefaults(1432, false);
                    ++index1;
                }
                if (Main.player[Main.myPlayer].HasItem(1258))
                {
                    item[index1].SetDefaults(1261, false);
                    ++index1;
                }
                if (Main.player[Main.myPlayer].HasItem(1835))
                {
                    item[index1].SetDefaults(1836, false);
                    ++index1;
                }
                if (Main.player[Main.myPlayer].HasItem(3107))
                {
                    item[index1].SetDefaults(3108, false);
                    ++index1;
                }
                if (Main.player[Main.myPlayer].HasItem(1782))
                {
                    item[index1].SetDefaults(1783, false);
                    ++index1;
                }
                if (Main.player[Main.myPlayer].HasItem(1784))
                {
                    item[index1].SetDefaults(1785, false);
                    ++index1;
                }
                if (Main.halloween)
                {
                    item[index1].SetDefaults(1736, false);
                    int index4 = index1 + 1;
                    item[index4].SetDefaults(1737, false);
                    int index5 = index4 + 1;
                    item[index5].SetDefaults(1738, false);
                    index1 = index5 + 1;
                }
            }
            else if (type == 3)
            {
                int index2;
                if (Main.bloodMoon)
                {
                    if (WorldGen.crimson)
                    {
                        item[index1].SetDefaults(2171, false);
                        index2 = index1 + 1;
                    }
                    else
                    {
                        item[index1].SetDefaults(67, false);
                        int index3 = index1 + 1;
                        item[index3].SetDefaults(59, false);
                        index2 = index3 + 1;
                    }
                }
                else
                {
                    item[index1].SetDefaults("Purification Powder");
                    int index3 = index1 + 1;
                    item[index3].SetDefaults("Grass Seeds");
                    int index4 = index3 + 1;
                    item[index4].SetDefaults("Sunflower");
                    index2 = index4 + 1;
                }
                item[index2].SetDefaults("Acorn");
                int index5 = index2 + 1;
                item[index5].SetDefaults(114, false);
                int index6 = index5 + 1;
                item[index6].SetDefaults(1828, false);
                int index7 = index6 + 1;
                item[index7].SetDefaults(745, false);
                int index8 = index7 + 1;
                item[index8].SetDefaults(747, false);
                index1 = index8 + 1;
                if (Main.hardMode)
                {
                    item[index1].SetDefaults(746, false);
                    ++index1;
                }
                if (Main.hardMode)
                {
                    item[index1].SetDefaults(369, false);
                    ++index1;
                }
                if (Main.shroomTiles > 50)
                {
                    item[index1].SetDefaults(194, false);
                    ++index1;
                }
                if (Main.halloween)
                {
                    item[index1].SetDefaults(1853, false);
                    int index3 = index1 + 1;
                    item[index3].SetDefaults(1854, false);
                    index1 = index3 + 1;
                }
                if (NPC.downedSlimeKing)
                {
                    item[index1].SetDefaults(3215, false);
                    ++index1;
                }
                if (NPC.downedQueenBee)
                {
                    item[index1].SetDefaults(3216, false);
                    ++index1;
                }
                if (NPC.downedBoss1)
                {
                    item[index1].SetDefaults(3219, false);
                    ++index1;
                }
                if (NPC.downedBoss2)
                {
                    if (WorldGen.crimson)
                    {
                        item[index1].SetDefaults(3218, false);
                        ++index1;
                    }
                    else
                    {
                        item[index1].SetDefaults(3217, false);
                        ++index1;
                    }
                }
                if (NPC.downedBoss3)
                {
                    item[index1].SetDefaults(3220, false);
                    int index3 = index1 + 1;
                    item[index3].SetDefaults(3221, false);
                    index1 = index3 + 1;
                }
                if (Main.hardMode)
                {
                    item[index1].SetDefaults(3222, false);
                    ++index1;
                }
            }
            else if (type == 4)
            {
                item[index1].SetDefaults("Grenade");
                int index2 = index1 + 1;
                item[index2].SetDefaults("Bomb");
                int index3 = index2 + 1;
                item[index3].SetDefaults("Dynamite");
                index1 = index3 + 1;
                if (Main.hardMode)
                {
                    item[index1].SetDefaults("Hellfire Arrow");
                    ++index1;
                }
                if (Main.hardMode && NPC.downedPlantBoss && NPC.downedPirates)
                {
                    item[index1].SetDefaults(937, false);
                    ++index1;
                }
                if (Main.hardMode)
                {
                    item[index1].SetDefaults(1347, false);
                    ++index1;
                }
            }
            else if (type == 5)
            {
                item[index1].SetDefaults(254, false);
                int index2 = index1 + 1;
                item[index2].SetDefaults(981, false);
                int index3 = index2 + 1;
                if (Main.dayTime)
                {
                    item[index3].SetDefaults(242, false);
                    ++index3;
                }
                if (Main.moonPhase == 0)
                {
                    item[index3].SetDefaults(245, false);
                    int index4 = index3 + 1;
                    item[index4].SetDefaults(246, false);
                    index3 = index4 + 1;
                    if (!Main.dayTime)
                    {
                        Item[] objArray1 = item;
                        int index5 = index3;
                        int num1 = 1;
                        int num2 = index5 + num1;
                        objArray1[index5].SetDefaults(1288, false);
                        Item[] objArray2 = item;
                        int index6 = num2;
                        int num3 = 1;
                        index3 = index6 + num3;
                        objArray2[index6].SetDefaults(1289, false);
                    }
                }
                else if (Main.moonPhase == 1)
                {
                    item[index3].SetDefaults(325, false);
                    int index4 = index3 + 1;
                    item[index4].SetDefaults(326, false);
                    index3 = index4 + 1;
                }
                item[index3].SetDefaults(269, false);
                int index7 = index3 + 1;
                item[index7].SetDefaults(270, false);
                int index8 = index7 + 1;
                item[index8].SetDefaults(271, false);
                index1 = index8 + 1;
                if (NPC.downedClown)
                {
                    item[index1].SetDefaults(503, false);
                    int index4 = index1 + 1;
                    item[index4].SetDefaults(504, false);
                    int index5 = index4 + 1;
                    item[index5].SetDefaults(505, false);
                    index1 = index5 + 1;
                }
                if (Main.bloodMoon)
                {
                    item[index1].SetDefaults(322, false);
                    ++index1;
                    if (!Main.dayTime)
                    {
                        Item[] objArray1 = item;
                        int index4 = index1;
                        int num1 = 1;
                        int num2 = index4 + num1;
                        objArray1[index4].SetDefaults(3362, false);
                        Item[] objArray2 = item;
                        int index5 = num2;
                        int num3 = 1;
                        index1 = index5 + num3;
                        objArray2[index5].SetDefaults(3363, false);
                    }
                }
                if (NPC.downedAncientCultist)
                {
                    if (Main.dayTime)
                    {
                        Item[] objArray1 = item;
                        int index4 = index1;
                        int num1 = 1;
                        int num2 = index4 + num1;
                        objArray1[index4].SetDefaults(2856, false);
                        Item[] objArray2 = item;
                        int index5 = num2;
                        int num3 = 1;
                        index1 = index5 + num3;
                        objArray2[index5].SetDefaults(2858, false);
                    }
                    else
                    {
                        Item[] objArray1 = item;
                        int index4 = index1;
                        int num1 = 1;
                        int num2 = index4 + num1;
                        objArray1[index4].SetDefaults(2857, false);
                        Item[] objArray2 = item;
                        int index5 = num2;
                        int num3 = 1;
                        index1 = index5 + num3;
                        objArray2[index5].SetDefaults(2859, false);
                    }
                }
                if (NPC.AnyNPCs(441))
                {
                    Item[] objArray1 = item;
                    int index4 = index1;
                    int num1 = 1;
                    int num2 = index4 + num1;
                    objArray1[index4].SetDefaults(3242, false);
                    Item[] objArray2 = item;
                    int index5 = num2;
                    int num3 = 1;
                    int num4 = index5 + num3;
                    objArray2[index5].SetDefaults(3243, false);
                    Item[] objArray3 = item;
                    int index6 = num4;
                    int num5 = 1;
                    index1 = index6 + num5;
                    objArray3[index6].SetDefaults(3244, false);
                }
                if (Main.player[Main.myPlayer].ZoneSnow)
                {
                    item[index1].SetDefaults(1429, false);
                    ++index1;
                }
                if (Main.halloween)
                {
                    item[index1].SetDefaults(1740, false);
                    ++index1;
                }
                if (Main.hardMode)
                {
                    if (Main.moonPhase == 2)
                    {
                        item[index1].SetDefaults(869, false);
                        ++index1;
                    }
                    if (Main.moonPhase == 4)
                    {
                        item[index1].SetDefaults(864, false);
                        int index4 = index1 + 1;
                        item[index4].SetDefaults(865, false);
                        index1 = index4 + 1;
                    }
                    if (Main.moonPhase == 6)
                    {
                        item[index1].SetDefaults(873, false);
                        int index4 = index1 + 1;
                        item[index4].SetDefaults(874, false);
                        int index5 = index4 + 1;
                        item[index5].SetDefaults(875, false);
                        index1 = index5 + 1;
                    }
                }
                if (NPC.downedFrost)
                {
                    item[index1].SetDefaults(1275, false);
                    int index4 = index1 + 1;
                    item[index4].SetDefaults(1276, false);
                    index1 = index4 + 1;
                }
                if (Main.halloween)
                {
                    Item[] objArray1 = item;
                    int index4 = index1;
                    int num1 = 1;
                    int num2 = index4 + num1;
                    objArray1[index4].SetDefaults(3246, false);
                    Item[] objArray2 = item;
                    int index5 = num2;
                    int num3 = 1;
                    index1 = index5 + num3;
                    objArray2[index5].SetDefaults(3247, false);
                }
            }
            else if (type == 6)
            {
                item[index1].SetDefaults(128, false);
                int index2 = index1 + 1;
                item[index2].SetDefaults(486, false);
                int index3 = index2 + 1;
                item[index3].SetDefaults(398, false);
                int index4 = index3 + 1;
                item[index4].SetDefaults(84, false);
                int index5 = index4 + 1;
                item[index5].SetDefaults(407, false);
                int index6 = index5 + 1;
                item[index6].SetDefaults(161, false);
                index1 = index6 + 1;
            }
            else if (type == 7)
            {
                item[index1].SetDefaults(487, false);
                int index2 = index1 + 1;
                item[index2].SetDefaults(496, false);
                int index3 = index2 + 1;
                item[index3].SetDefaults(500, false);
                int index4 = index3 + 1;
                item[index4].SetDefaults(507, false);
                int index5 = index4 + 1;
                item[index5].SetDefaults(508, false);
                int index6 = index5 + 1;
                item[index6].SetDefaults(531, false);
                int index7 = index6 + 1;
                item[index7].SetDefaults(576, false);
                int index8 = index7 + 1;
                item[index8].SetDefaults(3186, false);
                index1 = index8 + 1;
                if (Main.halloween)
                {
                    item[index1].SetDefaults(1739, false);
                    ++index1;
                }
            }
            else if (type == 8)
            {
                item[index1].SetDefaults(509, false);
                int index2 = index1 + 1;
                item[index2].SetDefaults(850, false);
                int index3 = index2 + 1;
                item[index3].SetDefaults(851, false);
                int index4 = index3 + 1;
                item[index4].SetDefaults(510, false);
                int index5 = index4 + 1;
                item[index5].SetDefaults(530, false);
                int index6 = index5 + 1;
                item[index6].SetDefaults(513, false);
                int index7 = index6 + 1;
                item[index7].SetDefaults(538, false);
                int index8 = index7 + 1;
                item[index8].SetDefaults(529, false);
                int index9 = index8 + 1;
                item[index9].SetDefaults(541, false);
                int index10 = index9 + 1;
                item[index10].SetDefaults(542, false);
                int index11 = index10 + 1;
                item[index11].SetDefaults(543, false);
                int index12 = index11 + 1;
                item[index12].SetDefaults(852, false);
                int index13 = index12 + 1;
                item[index13].SetDefaults(853, false);
                int index14 = index13 + 1;
                item[index14].SetDefaults(2739, false);
                int index15 = index14 + 1;
                item[index15].SetDefaults(849, false);
                int num1 = index15 + 1;
                Item[] objArray = item;
                int index16 = num1;
                int num2 = 1;
                index1 = index16 + num2;
                objArray[index16].SetDefaults(2799, false);
                if (NPC.AnyNPCs(369) && Main.hardMode && Main.moonPhase == 3)
                {
                    item[index1].SetDefaults(2295, false);
                    ++index1;
                }
            }
            else if (type == 9)
            {
                item[index1].SetDefaults(588, false);
                int index2 = index1 + 1;
                item[index2].SetDefaults(589, false);
                int index3 = index2 + 1;
                item[index3].SetDefaults(590, false);
                int index4 = index3 + 1;
                item[index4].SetDefaults(597, false);
                int index5 = index4 + 1;
                item[index5].SetDefaults(598, false);
                int index6 = index5 + 1;
                item[index6].SetDefaults(596, false);
                index1 = index6 + 1;
                for (int Type = 1873; Type < 1906; ++Type)
                {
                    item[index1].SetDefaults(Type, false);
                    ++index1;
                }
            }
            else if (type == 10)
            {
                if (NPC.downedMechBossAny)
                {
                    item[index1].SetDefaults(756, false);
                    int index2 = index1 + 1;
                    item[index2].SetDefaults(787, false);
                    index1 = index2 + 1;
                }
                item[index1].SetDefaults(868, false);
                int index3 = index1 + 1;
                if (NPC.downedPlantBoss)
                {
                    item[index3].SetDefaults(1551, false);
                    ++index3;
                }
                item[index3].SetDefaults(1181, false);
                int index4 = index3 + 1;
                item[index4].SetDefaults(783, false);
                index1 = index4 + 1;
            }
            else if (type == 11)
            {
                item[index1].SetDefaults(779, false);
                int index2 = index1 + 1;
                int index3;
                if (Main.moonPhase >= 4)
                {
                    item[index2].SetDefaults(748, false);
                    index3 = index2 + 1;
                }
                else
                {
                    item[index2].SetDefaults(839, false);
                    int index4 = index2 + 1;
                    item[index4].SetDefaults(840, false);
                    int index5 = index4 + 1;
                    item[index5].SetDefaults(841, false);
                    index3 = index5 + 1;
                }
                if (NPC.downedGolemBoss)
                {
                    item[index3].SetDefaults(948, false);
                    ++index3;
                }
                item[index3].SetDefaults(995, false);
                int index6 = index3 + 1;
                if (NPC.downedBoss1 && NPC.downedBoss2 && NPC.downedBoss3)
                {
                    item[index6].SetDefaults(2203, false);
                    ++index6;
                }
                if (WorldGen.crimson)
                {
                    item[index6].SetDefaults(2193, false);
                    ++index6;
                }
                item[index6].SetDefaults(1263, false);
                int index7 = index6 + 1;
                if (Main.eclipse || Main.bloodMoon)
                {
                    if (WorldGen.crimson)
                    {
                        item[index7].SetDefaults(784, false);
                        index1 = index7 + 1;
                    }
                    else
                    {
                        item[index7].SetDefaults(782, false);
                        index1 = index7 + 1;
                    }
                }
                else if (Main.player[Main.myPlayer].ZoneHoly)
                {
                    item[index7].SetDefaults(781, false);
                    index1 = index7 + 1;
                }
                else
                {
                    item[index7].SetDefaults(780, false);
                    index1 = index7 + 1;
                }
                if (Main.hardMode)
                {
                    item[index1].SetDefaults(1344, false);
                    ++index1;
                }
                if (Main.halloween)
                {
                    item[index1].SetDefaults(1742, false);
                    ++index1;
                }
            }
            else if (type == 12)
            {
                item[index1].SetDefaults(1037, false);
                int index2 = index1 + 1;
                item[index2].SetDefaults(2874, false);
                int index3 = index2 + 1;
                item[index3].SetDefaults(1120, false);
                index1 = index3 + 1;
                if (Main.netMode == 1)
                {
                    item[index1].SetDefaults(1969, false);
                    ++index1;
                }
                if (Main.halloween)
                {
                    item[index1].SetDefaults(3248, false);
                    int index4 = index1 + 1;
                    item[index4].SetDefaults(1741, false);
                    index1 = index4 + 1;
                }
                if (Main.moonPhase == 0)
                {
                    item[index1].SetDefaults(2871, false);
                    int index4 = index1 + 1;
                    item[index4].SetDefaults(2872, false);
                    index1 = index4 + 1;
                }
            }
            else if (type == 13)
            {
                item[index1].SetDefaults(859, false);
                int index2 = index1 + 1;
                item[index2].SetDefaults(1000, false);
                int index3 = index2 + 1;
                item[index3].SetDefaults(1168, false);
                int index4 = index3 + 1;
                item[index4].SetDefaults(1449, false);
                int index5 = index4 + 1;
                item[index5].SetDefaults(1345, false);
                int index6 = index5 + 1;
                item[index6].SetDefaults(1450, false);
                int num1 = index6 + 1;
                Item[] objArray1 = item;
                int index7 = num1;
                int num2 = 1;
                int num3 = index7 + num2;
                objArray1[index7].SetDefaults(3253, false);
                Item[] objArray2 = item;
                int index8 = num3;
                int num4 = 1;
                int num5 = index8 + num4;
                objArray2[index8].SetDefaults(2700, false);
                Item[] objArray3 = item;
                int index9 = num5;
                int num6 = 1;
                index1 = index9 + num6;
                objArray3[index9].SetDefaults(2738, false);
                if (Main.player[Main.myPlayer].HasItem(3548))
                {
                    item[index1].SetDefaults(3548, false);
                    ++index1;
                }
                if (NPC.AnyNPCs(229))
                    item[index1++].SetDefaults(3369, false);
                if (Main.hardMode)
                {
                    item[index1].SetDefaults(3214, false);
                    int index10 = index1 + 1;
                    item[index10].SetDefaults(2868, false);
                    int index11 = index10 + 1;
                    item[index11].SetDefaults(970, false);
                    int index12 = index11 + 1;
                    item[index12].SetDefaults(971, false);
                    int index13 = index12 + 1;
                    item[index13].SetDefaults(972, false);
                    int index14 = index13 + 1;
                    item[index14].SetDefaults(973, false);
                    index1 = index14 + 1;
                }
            }
            else if (type == 14)
            {
                item[index1].SetDefaults(771, false);
                ++index1;
                if (Main.bloodMoon)
                {
                    item[index1].SetDefaults(772, false);
                    ++index1;
                }
                if (!Main.dayTime || Main.eclipse)
                {
                    item[index1].SetDefaults(773, false);
                    ++index1;
                }
                if (Main.eclipse)
                {
                    item[index1].SetDefaults(774, false);
                    ++index1;
                }
                if (Main.hardMode)
                {
                    item[index1].SetDefaults(760, false);
                    ++index1;
                }
                if (Main.hardMode)
                {
                    item[index1].SetDefaults(1346, false);
                    ++index1;
                }
                if (Main.halloween)
                {
                    item[index1].SetDefaults(1743, false);
                    int index2 = index1 + 1;
                    item[index2].SetDefaults(1744, false);
                    int index3 = index2 + 1;
                    item[index3].SetDefaults(1745, false);
                    index1 = index3 + 1;
                }
                if (NPC.downedMartians)
                {
                    Item[] objArray1 = item;
                    int index2 = index1;
                    int num1 = 1;
                    int num2 = index2 + num1;
                    objArray1[index2].SetDefaults(2862, false);
                    Item[] objArray2 = item;
                    int index3 = num2;
                    int num3 = 1;
                    index1 = index3 + num3;
                    objArray2[index3].SetDefaults(3109, false);
                }
            }
            else if (type == 15)
            {
                item[index1].SetDefaults(1071, false);
                int index2 = index1 + 1;
                item[index2].SetDefaults(1072, false);
                int index3 = index2 + 1;
                item[index3].SetDefaults(1100, false);
                int index4 = index3 + 1;
                for (int Type = 1073; Type <= 1084; ++Type)
                {
                    item[index4].SetDefaults(Type, false);
                    ++index4;
                }
                item[index4].SetDefaults(1097, false);
                int index5 = index4 + 1;
                item[index5].SetDefaults(1099, false);
                int index6 = index5 + 1;
                item[index6].SetDefaults(1098, false);
                int index7 = index6 + 1;
                item[index7].SetDefaults(1966, false);
                int index8 = index7 + 1;
                if (Main.hardMode)
                {
                    item[index8].SetDefaults(1967, false);
                    int index9 = index8 + 1;
                    item[index9].SetDefaults(1968, false);
                    index8 = index9 + 1;
                }
                item[index8].SetDefaults(1490, false);
                int index10 = index8 + 1;
                if (Main.moonPhase <= 1)
                {
                    item[index10].SetDefaults(1481, false);
                    index1 = index10 + 1;
                }
                else if (Main.moonPhase <= 3)
                {
                    item[index10].SetDefaults(1482, false);
                    index1 = index10 + 1;
                }
                else if (Main.moonPhase <= 5)
                {
                    item[index10].SetDefaults(1483, false);
                    index1 = index10 + 1;
                }
                else
                {
                    item[index10].SetDefaults(1484, false);
                    index1 = index10 + 1;
                }
                if (Main.player[Main.myPlayer].ZoneCrimson)
                {
                    item[index1].SetDefaults(1492, false);
                    ++index1;
                }
                if (Main.player[Main.myPlayer].ZoneCorrupt)
                {
                    item[index1].SetDefaults(1488, false);
                    ++index1;
                }
                if (Main.player[Main.myPlayer].ZoneHoly)
                {
                    item[index1].SetDefaults(1489, false);
                    ++index1;
                }
                if (Main.player[Main.myPlayer].ZoneJungle)
                {
                    item[index1].SetDefaults(1486, false);
                    ++index1;
                }
                if (Main.player[Main.myPlayer].ZoneSnow)
                {
                    item[index1].SetDefaults(1487, false);
                    ++index1;
                }
                if (Main.sandTiles > 1000)
                {
                    item[index1].SetDefaults(1491, false);
                    ++index1;
                }
                if (Main.bloodMoon)
                {
                    item[index1].SetDefaults(1493, false);
                    ++index1;
                }
                if ((double)Main.player[Main.myPlayer].position.Y / 16.0 < Main.worldSurface * 0.349999994039536)
                {
                    item[index1].SetDefaults(1485, false);
                    ++index1;
                }
                if ((double)Main.player[Main.myPlayer].position.Y / 16.0 < Main.worldSurface * 0.349999994039536 && Main.hardMode)
                {
                    item[index1].SetDefaults(1494, false);
                    ++index1;
                }
                if (Main.xMas)
                {
                    for (int Type = 1948; Type <= 1957; ++Type)
                    {
                        item[index1].SetDefaults(Type, false);
                        ++index1;
                    }
                }
                for (int Type = 2158; Type <= 2160; ++Type)
                {
                    if (index1 < 39)
                        item[index1].SetDefaults(Type, false);
                    ++index1;
                }
                for (int Type = 2008; Type <= 2014; ++Type)
                {
                    if (index1 < 39)
                        item[index1].SetDefaults(Type, false);
                    ++index1;
                }
            }
            else if (type == 16)
            {
                item[index1].SetDefaults(1430, false);
                int index2 = index1 + 1;
                item[index2].SetDefaults(986, false);
                int index3 = index2 + 1;
                if (NPC.AnyNPCs(108))
                    item[index3++].SetDefaults(2999, false);
                if (Main.hardMode && NPC.downedPlantBoss)
                {
                    if (Main.player[Main.myPlayer].HasItem(1157))
                    {
                        item[index3].SetDefaults(1159, false);
                        int index4 = index3 + 1;
                        item[index4].SetDefaults(1160, false);
                        int index5 = index4 + 1;
                        item[index5].SetDefaults(1161, false);
                        index3 = index5 + 1;
                        if (!Main.dayTime)
                        {
                            item[index3].SetDefaults(1158, false);
                            ++index3;
                        }
                        if (Main.player[Main.myPlayer].ZoneJungle)
                        {
                            item[index3].SetDefaults(1167, false);
                            ++index3;
                        }
                    }
                    item[index3].SetDefaults(1339, false);
                    ++index3;
                }
                if (Main.hardMode && Main.player[Main.myPlayer].ZoneJungle)
                {
                    item[index3].SetDefaults(1171, false);
                    ++index3;
                    if (!Main.dayTime)
                    {
                        item[index3].SetDefaults(1162, false);
                        ++index3;
                    }
                }
                item[index3].SetDefaults(909, false);
                int index6 = index3 + 1;
                item[index6].SetDefaults(910, false);
                int index7 = index6 + 1;
                item[index7].SetDefaults(940, false);
                int index8 = index7 + 1;
                item[index8].SetDefaults(941, false);
                int index9 = index8 + 1;
                item[index9].SetDefaults(942, false);
                int index10 = index9 + 1;
                item[index10].SetDefaults(943, false);
                int index11 = index10 + 1;
                item[index11].SetDefaults(944, false);
                int index12 = index11 + 1;
                item[index12].SetDefaults(945, false);
                index1 = index12 + 1;
                if (Main.player[Main.myPlayer].HasItem(1835))
                {
                    item[index1].SetDefaults(1836, false);
                    ++index1;
                }
                if (Main.player[Main.myPlayer].HasItem(1258))
                {
                    item[index1].SetDefaults(1261, false);
                    ++index1;
                }
                if (Main.halloween)
                {
                    item[index1].SetDefaults(1791, false);
                    ++index1;
                }
            }
            else if (type == 17)
            {
                item[index1].SetDefaults(928, false);
                int index2 = index1 + 1;
                item[index2].SetDefaults(929, false);
                int index3 = index2 + 1;
                item[index3].SetDefaults(876, false);
                int index4 = index3 + 1;
                item[index4].SetDefaults(877, false);
                int index5 = index4 + 1;
                item[index5].SetDefaults(878, false);
                int index6 = index5 + 1;
                item[index6].SetDefaults(2434, false);
                index1 = index6 + 1;
                int num = (int)((Main.screenPosition.X + (Main.screenWidth / 2)) / 16.0);
                if (Main.screenPosition.Y / 16.0 < Main.worldSurface + 10.0 && (num < 380 || num > Main.maxTilesX - 380))
                {
                    item[index1].SetDefaults(1180, false);
                    ++index1;
                }
                if (Main.hardMode && NPC.downedMechBossAny && NPC.AnyNPCs(208))
                {
                    item[index1].SetDefaults(1337, false);
                    ++index1;
                }
            }
            else if (type == 18)
            {
                item[index1].SetDefaults(1990, false);
                int index2 = index1 + 1;
                item[index2].SetDefaults(1979, false);
                index1 = index2 + 1;
                if (Main.player[Main.myPlayer].statLifeMax >= 400)
                {
                    item[index1].SetDefaults(1977, false);
                    ++index1;
                }
                if (Main.player[Main.myPlayer].statManaMax >= 200)
                {
                    item[index1].SetDefaults(1978, false);
                    ++index1;
                }
                long num = 0L;
                for (int index3 = 0; index3 < 54; ++index3)
                {
                    if (Main.player[Main.myPlayer].inventory[index3].type == 71)
                        num += Main.player[Main.myPlayer].inventory[index3].stack;
                    if (Main.player[Main.myPlayer].inventory[index3].type == 72)
                        num += (Main.player[Main.myPlayer].inventory[index3].stack * 100);
                    if (Main.player[Main.myPlayer].inventory[index3].type == 73)
                        num += (Main.player[Main.myPlayer].inventory[index3].stack * 10000);
                    if (Main.player[Main.myPlayer].inventory[index3].type == 74)
                        num += (Main.player[Main.myPlayer].inventory[index3].stack * 1000000);
                }
                if (num >= 1000000L)
                {
                    item[index1].SetDefaults(1980, false);
                    ++index1;
                }
                if (Main.moonPhase % 2 == 0 && Main.dayTime || Main.moonPhase % 2 == 1 && !Main.dayTime)
                {
                    item[index1].SetDefaults(1981, false);
                    ++index1;
                }
                if (Main.player[Main.myPlayer].team != 0)
                {
                    item[index1].SetDefaults(1982, false);
                    ++index1;
                }
                if (Main.hardMode)
                {
                    item[index1].SetDefaults(1983, false);
                    ++index1;
                }
                if (NPC.AnyNPCs(208))
                {
                    item[index1].SetDefaults(1984, false);
                    ++index1;
                }
                if (Main.hardMode && NPC.downedMechBoss1 && (NPC.downedMechBoss2 && NPC.downedMechBoss3))
                {
                    item[index1].SetDefaults(1985, false);
                    ++index1;
                }
                if (Main.hardMode && NPC.downedMechBossAny)
                {
                    item[index1].SetDefaults(1986, false);
                    ++index1;
                }
                if (Main.hardMode && NPC.downedMartians)
                {
                    item[index1].SetDefaults(2863, false);
                    int index3 = index1 + 1;
                    item[index3].SetDefaults(3259, false);
                    index1 = index3 + 1;
                }
            }
            else if (type == 19)
            {
                for (int index2 = 0; index2 < 40; ++index2)
                {
                    if (Main.travelShop[index2] != 0)
                    {
                        item[index1].netDefaults(Main.travelShop[index2]);
                        ++index1;
                    }
                }
            }
            else if (type == 20)
            {
                if (Main.moonPhase % 2 == 0)
                    item[index1].SetDefaults(3001, false);
                else
                    item[index1].SetDefaults(28, false);
                int index2 = index1 + 1;
                if (!Main.dayTime || Main.moonPhase == 0)
                    item[index2].SetDefaults(3002, false);
                else
                    item[index2].SetDefaults(282, false);
                int index3 = index2 + 1;
                if (Main.time % 60.0 * 60.0 * 6.0 <= 10800.0)
                    item[index3].SetDefaults(3004, false);
                else
                    item[index3].SetDefaults(8, false);
                int index4 = index3 + 1;
                if (Main.moonPhase == 0 || Main.moonPhase == 1 || (Main.moonPhase == 4 || Main.moonPhase == 5))
                    item[index4].SetDefaults(3003, false);
                else
                    item[index4].SetDefaults(40, false);
                int index5 = index4 + 1;
                if (Main.moonPhase % 4 == 0)
                    item[index5].SetDefaults(3310, false);
                else if (Main.moonPhase % 4 == 1)
                    item[index5].SetDefaults(3313, false);
                else if (Main.moonPhase % 4 == 2)
                    item[index5].SetDefaults(3312, false);
                else
                    item[index5].SetDefaults(3311, false);
                int index6 = index5 + 1;
                item[index6].SetDefaults(166, false);
                int index7 = index6 + 1;
                item[index7].SetDefaults(965, false);
                index1 = index7 + 1;
                if (Main.hardMode)
                {
                    if (Main.moonPhase < 4)
                        item[index1].SetDefaults(3316, false);
                    else
                        item[index1].SetDefaults(3315, false);
                    int index8 = index1 + 1;
                    item[index8].SetDefaults(3334, false);
                    index1 = index8 + 1;
                    if (Main.bloodMoon)
                    {
                        item[index1].SetDefaults(3258, false);
                        ++index1;
                    }
                }
                if (Main.moonPhase == 0 && !Main.dayTime)
                {
                    item[index1].SetDefaults(3043, false);
                    ++index1;
                }
            }
            if (!Main.player[Main.myPlayer].discount)
                return;
            for (int index2 = 0; index2 < index1; ++index2)
                item[index2].value = (int)(item[index2].value * 0.800000011920929);
        }

        public static void UpdateChestFrames()
        {
            bool[] flagArray = new bool[1000];
            for (int index = 0; index < 255; ++index)
            {
                if (Main.player[index].active && Main.player[index].chest >= 0 && Main.player[index].chest < 1000)
                    flagArray[Main.player[index].chest] = true;
            }
            for (int index = 0; index < 1000; ++index)
            {
                Chest chest = Main.chest[index];
                if (chest != null)
                {
                    if (flagArray[index])
                        ++chest.frameCounter;
                    else
                        --chest.frameCounter;
                    if (chest.frameCounter < 0)
                        chest.frameCounter = 0;
                    if (chest.frameCounter > 10)
                        chest.frameCounter = 10;
                    chest.frame = chest.frameCounter != 0 ? (chest.frameCounter != 10 ? 1 : 2) : 0;
                }
            }
        }
    }
}
