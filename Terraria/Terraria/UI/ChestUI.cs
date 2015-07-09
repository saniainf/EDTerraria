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
using System.Collections.Generic;
using Terraria;
using Terraria.UI.Chat;

namespace Terraria.UI
{
    public class ChestUI
    {
        public static float[] ButtonScale = new float[6];
        public static bool[] ButtonHovered = new bool[6];
        public const float buttonScaleMinimum = 0.75f;
        public const float buttonScaleMaximum = 1f;

        public static void UpdateHover(int ID, bool hovering)
        {
            if (hovering)
            {
                if (!ChestUI.ButtonHovered[ID])
                    Main.PlaySound(12, -1, -1, 1);
                ChestUI.ButtonHovered[ID] = true;
                ChestUI.ButtonScale[ID] += 0.05f;
                if ((double)ChestUI.ButtonScale[ID] <= 1.0)
                    return;
                ChestUI.ButtonScale[ID] = 1f;
            }
            else
            {
                ChestUI.ButtonHovered[ID] = false;
                ChestUI.ButtonScale[ID] -= 0.05f;
                if ((double)ChestUI.ButtonScale[ID] >= 0.75)
                    return;
                ChestUI.ButtonScale[ID] = 0.75f;
            }
        }

        public static void Draw(SpriteBatch spritebatch)
        {
            if (Main.player[Main.myPlayer].chest != -1 && !Main.recBigList)
            {
                Main.inventoryScale = 0.755f;
                if (Utils.FloatIntersect((float)Main.mouseX, (float)Main.mouseY, 0.0f, 0.0f, 73f, (float)Main.instance.invBottom, 560f * Main.inventoryScale, 224f * Main.inventoryScale))
                    Main.player[Main.myPlayer].mouseInterface = true;
                ChestUI.DrawName(spritebatch);
                ChestUI.DrawButtons(spritebatch);
                ChestUI.DrawSlots(spritebatch);
            }
            else
            {
                for (int index = 0; index < 6; ++index)
                {
                    ChestUI.ButtonScale[index] = 0.75f;
                    ChestUI.ButtonHovered[index] = false;
                }
            }
        }

        private static void DrawName(SpriteBatch spritebatch)
        {
            Player player = Main.player[Main.myPlayer];
            string text = string.Empty;
            if (Main.editChest)
            {
                text = Main.npcChatText;
                ++Main.instance.textBlinkerCount;
                if (Main.instance.textBlinkerCount >= 20)
                {
                    Main.instance.textBlinkerState = Main.instance.textBlinkerState != 0 ? 0 : 1;
                    Main.instance.textBlinkerCount = 0;
                }
                if (Main.instance.textBlinkerState == 1)
                    text += "|";
            }
            else if (player.chest > -1)
            {
                if (Main.chest[player.chest] == null)
                    Main.chest[player.chest] = new Chest(false);
                Chest chest = Main.chest[player.chest];
                if (chest.name != "")
                    text = chest.name;
                else if ((int)Main.tile[player.chestX, player.chestY].type == 21)
                    text = Lang.chestType[(int)Main.tile[player.chestX, player.chestY].frameX / 36];
                else if ((int)Main.tile[player.chestX, player.chestY].type == 88)
                    text = Lang.dresserType[(int)Main.tile[player.chestX, player.chestY].frameX / 54];
            }
            else if (player.chest == -2)
                text = Lang.inter[32];
            else if (player.chest == -3)
                text = Lang.inter[33];
            Color color = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
            Color baseColor = Color.White * (float)(1.0 - ((double)byte.MaxValue - (double)Main.mouseTextColor) / (double)byte.MaxValue * 0.5);
            baseColor.A = byte.MaxValue;
            int lineAmount;
            Utils.WordwrapString(text, Main.fontMouseText, 200, 1, out lineAmount);
            ++lineAmount;
            for (int index = 0; index < lineAmount; ++index)
                ChatManager.DrawColorCodedStringWithShadow(spritebatch, Main.fontMouseText, text, new Vector2(504f, (float)(Main.instance.invBottom + index * 26)), baseColor, 0.0f, Vector2.Zero, Vector2.One, -1f, 1.5f);
        }

        private static void DrawButtons(SpriteBatch spritebatch)
        {
            for (int ID = 0; ID < 6; ++ID)
                ChestUI.DrawButton(spritebatch, ID, 506, Main.instance.invBottom + 40);
        }

        private static void DrawButton(SpriteBatch spriteBatch, int ID, int X, int Y)
        {
            Player player = Main.player[Main.myPlayer];
            if (ID == 4 && player.chest < -1 || ID == 5 && !Main.editChest)
            {
                ChestUI.UpdateHover(ID, false);
            }
            else
            {
                Y += ID * 26;
                float num = ChestUI.ButtonScale[ID];
                string text = "";
                switch (ID)
                {
                    case 0:
                        text = Lang.inter[29];
                        break;
                    case 1:
                        text = Lang.inter[30];
                        break;
                    case 2:
                        text = Lang.inter[31];
                        break;
                    case 3:
                        text = Lang.inter[82];
                        break;
                    case 4:
                        text = Lang.inter[Main.editChest ? 47 : 61];
                        break;
                    case 5:
                        text = Lang.inter[63];
                        break;
                }
                Vector2 vector2 = Main.fontMouseText.MeasureString(text);
                Color color = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor) * num;
                Color baseColor = Color.White * 0.97f * (float)(1.0 - ((double)byte.MaxValue - (double)Main.mouseTextColor) / (double)byte.MaxValue * 0.5);
                baseColor.A = byte.MaxValue;
                X += (int)((double)vector2.X * (double)num / 2.0);
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, text, new Vector2((float)X, (float)Y), baseColor, 0.0f, vector2 / 2f, new Vector2(num), -1f, 1.5f);
                vector2 *= num;
                if (!Utils.FloatIntersect((float)Main.mouseX, (float)Main.mouseY, 0.0f, 0.0f, (float)X - vector2.X / 2f, (float)Y - vector2.Y / 2f, vector2.X, vector2.Y))
                {
                    ChestUI.UpdateHover(ID, false);
                }
                else
                {
                    ChestUI.UpdateHover(ID, true);
                    player.mouseInterface = true;
                    if (!Main.mouseLeft || !Main.mouseLeftRelease)
                        return;
                    switch (ID)
                    {
                        case 0:
                            ChestUI.LootAll();
                            break;
                        case 1:
                            ChestUI.DepositAll();
                            break;
                        case 2:
                            ChestUI.QuickStack();
                            break;
                        case 3:
                            ChestUI.Restock();
                            break;
                        case 4:
                            ChestUI.RenameChest();
                            break;
                        case 5:
                            ChestUI.RenameChestCancel();
                            break;
                    }
                    Recipe.FindRecipes();
                }
            }
        }

        private static void DrawSlots(SpriteBatch spriteBatch)
        {
            Player player = Main.player[Main.myPlayer];
            int context = 0;
            Item[] inv = (Item[])null;
            if (player.chest > -1)
            {
                context = 3;
                inv = Main.chest[player.chest].item;
            }
            if (player.chest == -2)
            {
                context = 4;
                inv = player.bank.item;
            }
            if (player.chest == -3)
            {
                context = 4;
                inv = player.bank2.item;
            }
            Main.inventoryScale = 0.755f;
            if (Utils.FloatIntersect((float)Main.mouseX, (float)Main.mouseY, 0.0f, 0.0f, 73f, (float)Main.instance.invBottom, 560f * Main.inventoryScale, 224f * Main.inventoryScale))
                player.mouseInterface = true;
            for (int index1 = 0; index1 < 10; ++index1)
            {
                for (int index2 = 0; index2 < 4; ++index2)
                {
                    int num1 = (int)(73.0 + (double)(index1 * 56) * (double)Main.inventoryScale);
                    int num2 = (int)((double)Main.instance.invBottom + (double)(index2 * 56) * (double)Main.inventoryScale);
                    int slot = index1 + index2 * 10;
                    Color color = new Color(100, 100, 100, 100);
                    if (Utils.FloatIntersect((float)Main.mouseX, (float)Main.mouseY, 0.0f, 0.0f, (float)num1, (float)num2, (float)Main.inventoryBackTexture.Width * Main.inventoryScale, (float)Main.inventoryBackTexture.Height * Main.inventoryScale))
                    {
                        player.mouseInterface = true;
                        ItemSlot.Handle(inv, context, slot);
                    }
                    ItemSlot.Draw(spriteBatch, inv, context, slot, new Vector2((float)num1, (float)num2), new Color());
                }
            }
        }

        public static void LootAll()
        {
            Player player = Main.player[Main.myPlayer];
            if (player.chest > -1)
            {
                Chest chest = Main.chest[player.chest];
                for (int index = 0; index < 40; ++index)
                {
                    if (chest.item[index].type > 0)
                    {
                        chest.item[index].position = player.Center;
                        chest.item[index] = player.GetItem(Main.myPlayer, chest.item[index], false, false);
                        if (Main.netMode == 1)
                            NetMessage.SendData(32, -1, -1, "", player.chest, (float)index, 0.0f, 0.0f, 0, 0, 0);
                    }
                }
            }
            else if (player.chest == -3)
            {
                for (int index = 0; index < 40; ++index)
                {
                    if (player.bank2.item[index].type > 0)
                    {
                        player.bank2.item[index].position = player.Center;
                        player.bank2.item[index] = player.GetItem(Main.myPlayer, player.bank2.item[index], false, false);
                    }
                }
            }
            else
            {
                for (int index = 0; index < 40; ++index)
                {
                    if (player.bank.item[index].type > 0)
                    {
                        player.bank.item[index].position = player.Center;
                        player.bank.item[index] = player.GetItem(Main.myPlayer, player.bank.item[index], false, false);
                    }
                }
            }
        }

        public static void DepositAll()
        {
            Player player = Main.player[Main.myPlayer];
            if (player.chest > -1)
                ChestUI.MoveCoins(player.inventory, Main.chest[player.chest].item);
            else if (player.chest == -3)
                ChestUI.MoveCoins(player.inventory, player.bank2.item);
            else
                ChestUI.MoveCoins(player.inventory, player.bank.item);
            for (int index1 = 49; index1 >= 10; --index1)
            {
                if (player.inventory[index1].stack > 0 && player.inventory[index1].type > 0 && !player.inventory[index1].favorited)
                {
                    if (player.inventory[index1].maxStack > 1)
                    {
                        for (int index2 = 0; index2 < 40; ++index2)
                        {
                            if (player.chest > -1)
                            {
                                Chest chest = Main.chest[player.chest];
                                if (chest.item[index2].stack < chest.item[index2].maxStack && player.inventory[index1].IsTheSameAs(chest.item[index2]))
                                {
                                    int num = player.inventory[index1].stack;
                                    if (player.inventory[index1].stack + chest.item[index2].stack > chest.item[index2].maxStack)
                                        num = chest.item[index2].maxStack - chest.item[index2].stack;
                                    player.inventory[index1].stack -= num;
                                    chest.item[index2].stack += num;
                                    Main.PlaySound(7, -1, -1, 1);
                                    if (player.inventory[index1].stack <= 0)
                                    {
                                        player.inventory[index1].SetDefaults(0, false);
                                        if (Main.netMode == 1)
                                        {
                                            NetMessage.SendData(32, -1, -1, "", player.chest, (float)index2, 0.0f, 0.0f, 0, 0, 0);
                                            break;
                                        }
                                        break;
                                    }
                                    if (chest.item[index2].type == 0)
                                    {
                                        chest.item[index2] = player.inventory[index1].Clone();
                                        player.inventory[index1].SetDefaults(0, false);
                                    }
                                    if (Main.netMode == 1)
                                        NetMessage.SendData(32, -1, -1, "", player.chest, (float)index2, 0.0f, 0.0f, 0, 0, 0);
                                }
                            }
                            else if (player.chest == -3)
                            {
                                if (player.bank2.item[index2].stack < player.bank2.item[index2].maxStack && player.inventory[index1].IsTheSameAs(player.bank2.item[index2]))
                                {
                                    int num = player.inventory[index1].stack;
                                    if (player.inventory[index1].stack + player.bank2.item[index2].stack > player.bank2.item[index2].maxStack)
                                        num = player.bank2.item[index2].maxStack - player.bank2.item[index2].stack;
                                    player.inventory[index1].stack -= num;
                                    player.bank2.item[index2].stack += num;
                                    Main.PlaySound(7, -1, -1, 1);
                                    if (player.inventory[index1].stack <= 0)
                                    {
                                        player.inventory[index1].SetDefaults(0, false);
                                        break;
                                    }
                                    if (player.bank2.item[index2].type == 0)
                                    {
                                        player.bank2.item[index2] = player.inventory[index1].Clone();
                                        player.inventory[index1].SetDefaults(0, false);
                                    }
                                }
                            }
                            else if (player.bank.item[index2].stack < player.bank.item[index2].maxStack && player.inventory[index1].IsTheSameAs(player.bank.item[index2]))
                            {
                                int num = player.inventory[index1].stack;
                                if (player.inventory[index1].stack + player.bank.item[index2].stack > player.bank.item[index2].maxStack)
                                    num = player.bank.item[index2].maxStack - player.bank.item[index2].stack;
                                player.inventory[index1].stack -= num;
                                player.bank.item[index2].stack += num;
                                Main.PlaySound(7, -1, -1, 1);
                                if (player.inventory[index1].stack <= 0)
                                {
                                    player.inventory[index1].SetDefaults(0, false);
                                    break;
                                }
                                if (player.bank.item[index2].type == 0)
                                {
                                    player.bank.item[index2] = player.inventory[index1].Clone();
                                    player.inventory[index1].SetDefaults(0, false);
                                }
                            }
                        }
                    }
                    if (player.inventory[index1].stack > 0)
                    {
                        if (player.chest > -1)
                        {
                            for (int index2 = 0; index2 < 40; ++index2)
                            {
                                if (Main.chest[player.chest].item[index2].stack == 0)
                                {
                                    Main.PlaySound(7, -1, -1, 1);
                                    Main.chest[player.chest].item[index2] = player.inventory[index1].Clone();
                                    player.inventory[index1].SetDefaults(0, false);
                                    if (Main.netMode == 1)
                                    {
                                        NetMessage.SendData(32, -1, -1, "", player.chest, (float)index2, 0.0f, 0.0f, 0, 0, 0);
                                        break;
                                    }
                                    break;
                                }
                            }
                        }
                        else if (player.chest == -3)
                        {
                            for (int index2 = 0; index2 < 40; ++index2)
                            {
                                if (player.bank2.item[index2].stack == 0)
                                {
                                    Main.PlaySound(7, -1, -1, 1);
                                    player.bank2.item[index2] = player.inventory[index1].Clone();
                                    player.inventory[index1].SetDefaults(0, false);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int index2 = 0; index2 < 40; ++index2)
                            {
                                if (player.bank.item[index2].stack == 0)
                                {
                                    Main.PlaySound(7, -1, -1, 1);
                                    player.bank.item[index2] = player.inventory[index1].Clone();
                                    player.inventory[index1].SetDefaults(0, false);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void QuickStack()
        {
            Player player = Main.player[Main.myPlayer];
            if (player.chest == -3)
                ChestUI.MoveCoins(player.inventory, player.bank2.item);
            else if (player.chest == -2)
                ChestUI.MoveCoins(player.inventory, player.bank.item);
            Item[] objArray1 = player.inventory;
            Item[] objArray2 = player.bank.item;
            if (player.chest > -1)
                objArray2 = Main.chest[player.chest].item;
            else if (player.chest == -2)
                objArray2 = player.bank.item;
            else if (player.chest == -3)
                objArray2 = player.bank2.item;
            List<int> list1 = new List<int>();
            List<int> list2 = new List<int>();
            List<int> list3 = new List<int>();
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            List<int> list4 = new List<int>();
            bool[] flagArray = new bool[objArray2.Length];
            for (int index = 0; index < 40; ++index)
            {
                if (objArray2[index].type > 0 && objArray2[index].stack > 0 && objArray2[index].maxStack > 1 && (objArray2[index].type < 71 || objArray2[index].type > 74))
                {
                    list2.Add(index);
                    list1.Add(objArray2[index].netID);
                }
                if (objArray2[index].type == 0 || objArray2[index].stack <= 0)
                    list3.Add(index);
            }
            int num1 = 50;
            if (player.chest <= -2)
                num1 += 4;
            for (int key = 0; key < num1; ++key)
            {
                if (list1.Contains(objArray1[key].netID) && !objArray1[key].favorited)
                    dictionary.Add(key, objArray1[key].netID);
            }
            for (int index1 = 0; index1 < list2.Count; ++index1)
            {
                int index2 = list2[index1];
                int num2 = objArray2[index2].netID;
                foreach (KeyValuePair<int, int> keyValuePair in dictionary)
                {
                    if (keyValuePair.Value == num2 && objArray1[keyValuePair.Key].netID == num2)
                    {
                        int num3 = objArray1[keyValuePair.Key].stack;
                        int num4 = objArray2[index2].maxStack - objArray2[index2].stack;
                        if (num4 != 0)
                        {
                            if (num3 > num4)
                                num3 = num4;
                            Main.PlaySound(7, -1, -1, 1);
                            objArray2[index2].stack += num3;
                            objArray1[keyValuePair.Key].stack -= num3;
                            if (objArray1[keyValuePair.Key].stack == 0)
                                objArray1[keyValuePair.Key].SetDefaults(0, false);
                            flagArray[index2] = true;
                        }
                        else
                            break;
                    }
                }
            }
            foreach (KeyValuePair<int, int> keyValuePair in dictionary)
            {
                if (objArray1[keyValuePair.Key].stack == 0)
                    list4.Add(keyValuePair.Key);
            }
            foreach (int key in list4)
                dictionary.Remove(key);
            for (int index1 = 0; index1 < list3.Count; ++index1)
            {
                int index2 = list3[index1];
                bool flag = true;
                int num2 = objArray2[index2].netID;
                foreach (KeyValuePair<int, int> keyValuePair in dictionary)
                {
                    if (keyValuePair.Value == num2 && objArray1[keyValuePair.Key].netID == num2 || flag && objArray1[keyValuePair.Key].stack > 0)
                    {
                        Main.PlaySound(7, -1, -1, 1);
                        if (flag)
                        {
                            num2 = keyValuePair.Value;
                            objArray2[index2] = objArray1[keyValuePair.Key];
                            objArray1[keyValuePair.Key] = new Item();
                        }
                        else
                        {
                            int num3 = objArray1[keyValuePair.Key].stack;
                            int num4 = objArray2[index2].maxStack - objArray2[index2].stack;
                            if (num4 != 0)
                            {
                                if (num3 > num4)
                                    num3 = num4;
                                objArray2[index2].stack += num3;
                                objArray1[keyValuePair.Key].stack -= num3;
                                if (objArray1[keyValuePair.Key].stack == 0)
                                    objArray1[keyValuePair.Key] = new Item();
                            }
                            else
                                break;
                        }
                        flagArray[index2] = true;
                        flag = false;
                    }
                }
            }
            if (Main.netMode == 1 && player.chest >= 0)
            {
                for (int index = 0; index < flagArray.Length; ++index)
                    NetMessage.SendData(32, -1, -1, "", player.chest, (float)index, 0.0f, 0.0f, 0, 0, 0);
            }
            list1.Clear();
            list2.Clear();
            list3.Clear();
            dictionary.Clear();
            list4.Clear();
        }

        public static void RenameChest()
        {
            Player player = Main.player[Main.myPlayer];
            if (!Main.editChest)
            {
                Main.npcChatText = Main.chest[player.chest].name;
                if ((int)Main.tile[player.chestX, player.chestY].type == 21)
                    Main.defaultChestName = Lang.chestType[(int)Main.tile[player.chestX, player.chestY].frameX / 36];
                if ((int)Main.tile[player.chestX, player.chestY].type == 88)
                    Main.defaultChestName = Lang.dresserType[(int)Main.tile[player.chestX, player.chestY].frameX / 54];
                if (Main.npcChatText == "")
                    Main.npcChatText = Main.defaultChestName;
                Main.editChest = true;
                Main.clrInput();
            }
            else
            {
                Main.PlaySound(12, -1, -1, 1);
                Main.editChest = false;
                int index = player.chest;
                if (Main.npcChatText == Main.defaultChestName)
                    Main.npcChatText = "";
                if (!(Main.chest[index].name != Main.npcChatText))
                    return;
                Main.chest[index].name = Main.npcChatText;
                if (Main.netMode != 1)
                    return;
                player.editedChestName = true;
            }
        }

        public static void RenameChestCancel()
        {
            Main.PlaySound(12, -1, -1, 1);
            Main.editChest = false;
            Main.npcChatText = string.Empty;
        }

        public static void Restock()
        {
            Player player = Main.player[Main.myPlayer];
            Item[] inv = player.inventory;
            Item[] objArray = player.bank.item;
            if (player.chest > -1)
                objArray = Main.chest[player.chest].item;
            else if (player.chest == -2)
                objArray = player.bank.item;
            else if (player.chest == -3)
                objArray = player.bank2.item;
            HashSet<int> hashSet = new HashSet<int>();
            List<int> list1 = new List<int>();
            List<int> list2 = new List<int>();
            for (int index = 57; index >= 0; --index)
            {
                if ((index < 50 || index >= 54) && (inv[index].type < 71 || inv[index].type > 74))
                {
                    if (inv[index].stack > 0 && inv[index].maxStack > 1 && (int)inv[index].prefix == 0)
                    {
                        hashSet.Add(inv[index].netID);
                        if (inv[index].stack < inv[index].maxStack)
                            list1.Add(index);
                    }
                    else if (inv[index].stack == 0 || inv[index].netID == 0 || inv[index].type == 0)
                        list2.Add(index);
                }
            }
            bool flag1 = false;
            for (int index1 = 0; index1 < objArray.Length; ++index1)
            {
                if (objArray[index1].stack >= 1 && (int)objArray[index1].prefix == 0 && hashSet.Contains(objArray[index1].netID))
                {
                    bool flag2 = false;
                    for (int index2 = 0; index2 < list1.Count; ++index2)
                    {
                        int slot = list1[index2];
                        int context = 0;
                        if (slot >= 50)
                            context = 2;
                        if (inv[slot].netID == objArray[index1].netID && ItemSlot.PickItemMovementAction(inv, context, slot, objArray[index1]) != -1)
                        {
                            int num = objArray[index1].stack;
                            if (inv[slot].maxStack - inv[slot].stack < num)
                                num = inv[slot].maxStack - inv[slot].stack;
                            inv[slot].stack += num;
                            objArray[index1].stack -= num;
                            flag1 = true;
                            if (inv[slot].stack == inv[slot].maxStack)
                            {
                                if (Main.netMode == 1 && Main.player[Main.myPlayer].chest > -1)
                                    NetMessage.SendData(32, -1, -1, "", Main.player[Main.myPlayer].chest, (float)index1, 0.0f, 0.0f, 0, 0, 0);
                                list1.RemoveAt(index2);
                                --index2;
                            }
                            if (objArray[index1].stack == 0)
                            {
                                objArray[index1] = new Item();
                                flag2 = true;
                                if (Main.netMode == 1 && Main.player[Main.myPlayer].chest > -1)
                                {
                                    NetMessage.SendData(32, -1, -1, "", Main.player[Main.myPlayer].chest, (float)index1, 0.0f, 0.0f, 0, 0, 0);
                                    break;
                                }
                                break;
                            }
                        }
                    }
                    if (!flag2 && list2.Count > 0 && objArray[index1].ammo != 0)
                    {
                        for (int index2 = 0; index2 < list2.Count; ++index2)
                        {
                            int context = 0;
                            if (list2[index2] >= 50)
                                context = 2;
                            if (ItemSlot.PickItemMovementAction(inv, context, list2[index2], objArray[index1]) != -1)
                            {
                                Utils.Swap<Item>(ref inv[list2[index2]], ref objArray[index1]);
                                list1.Add(list2[index2]);
                                list2.RemoveAt(index2);
                                flag1 = true;
                                break;
                            }
                        }
                    }
                }
            }
            if (!flag1)
                return;
            Main.PlaySound(7, -1, -1, 1);
        }

        public static void MoveCoins(Item[] pInv, Item[] cInv)
        {
            int[] numArray1 = new int[4];
            List<int> list1 = new List<int>();
            List<int> list2 = new List<int>();
            bool flag = false;
            int[] numArray2 = new int[40];
            for (int index = 0; index < cInv.Length; ++index)
            {
                numArray2[index] = -1;
                if (cInv[index].stack < 1 || cInv[index].type < 1)
                {
                    list2.Add(index);
                    cInv[index] = new Item();
                }
                if (cInv[index] != null && cInv[index].stack > 0)
                {
                    int num = 0;
                    if (cInv[index].type == 71)
                        num = 1;
                    if (cInv[index].type == 72)
                        num = 2;
                    if (cInv[index].type == 73)
                        num = 3;
                    if (cInv[index].type == 74)
                        num = 4;
                    numArray2[index] = num - 1;
                    if (num > 0)
                    {
                        numArray1[num - 1] += cInv[index].stack;
                        list2.Add(index);
                        cInv[index] = new Item();
                        flag = true;
                    }
                }
            }
            if (!flag)
                return;
            Main.PlaySound(7, -1, -1, 1);
            for (int index = 0; index < pInv.Length; ++index)
            {
                if (index != 58 && pInv[index] != null && pInv[index].stack > 0)
                {
                    int num = 0;
                    if (pInv[index].type == 71)
                        num = 1;
                    if (pInv[index].type == 72)
                        num = 2;
                    if (pInv[index].type == 73)
                        num = 3;
                    if (pInv[index].type == 74)
                        num = 4;
                    if (num > 0)
                    {
                        numArray1[num - 1] += pInv[index].stack;
                        list1.Add(index);
                        pInv[index] = new Item();
                    }
                }
            }
            for (int index = 0; index < 3; ++index)
            {
                while (numArray1[index] >= 100)
                {
                    numArray1[index] -= 100;
                    ++numArray1[index + 1];
                }
            }
            for (int index1 = 0; index1 < 40; ++index1)
            {
                if (numArray2[index1] >= 0 && cInv[index1].type == 0)
                {
                    int index2 = index1;
                    int index3 = numArray2[index1];
                    if (numArray1[index3] > 0)
                    {
                        cInv[index2].SetDefaults(71 + index3, false);
                        cInv[index2].stack = numArray1[index3];
                        if (cInv[index2].stack > cInv[index2].maxStack)
                            cInv[index2].stack = cInv[index2].maxStack;
                        numArray1[index3] -= cInv[index2].stack;
                        numArray2[index1] = -1;
                    }
                    if (Main.netMode == 1 && Main.player[Main.myPlayer].chest > -1)
                        NetMessage.SendData(32, -1, -1, "", Main.player[Main.myPlayer].chest, (float)index2, 0.0f, 0.0f, 0, 0, 0);
                    list2.Remove(index2);
                }
            }
            for (int index1 = 0; index1 < 40; ++index1)
            {
                if (numArray2[index1] >= 0 && cInv[index1].type == 0)
                {
                    int index2 = index1;
                    int index3 = 3;
                    while (index3 >= 0)
                    {
                        if (numArray1[index3] > 0)
                        {
                            cInv[index2].SetDefaults(71 + index3, false);
                            cInv[index2].stack = numArray1[index3];
                            if (cInv[index2].stack > cInv[index2].maxStack)
                                cInv[index2].stack = cInv[index2].maxStack;
                            numArray1[index3] -= cInv[index2].stack;
                            numArray2[index1] = -1;
                            break;
                        }
                        if (numArray1[index3] == 0)
                            --index3;
                    }
                    if (Main.netMode == 1 && Main.player[Main.myPlayer].chest > -1)
                        NetMessage.SendData(32, -1, -1, "", Main.player[Main.myPlayer].chest, (float)index2, 0.0f, 0.0f, 0, 0, 0);
                    list2.Remove(index2);
                }
            }
            while (list2.Count > 0)
            {
                int index1 = list2[0];
                int index2 = 3;
                while (index2 >= 0)
                {
                    if (numArray1[index2] > 0)
                    {
                        cInv[index1].SetDefaults(71 + index2, false);
                        cInv[index1].stack = numArray1[index2];
                        if (cInv[index1].stack > cInv[index1].maxStack)
                            cInv[index1].stack = cInv[index1].maxStack;
                        numArray1[index2] -= cInv[index1].stack;
                        break;
                    }
                    if (numArray1[index2] == 0)
                        --index2;
                }
                if (Main.netMode == 1 && Main.player[Main.myPlayer].chest > -1)
                    NetMessage.SendData(32, -1, -1, "", Main.player[Main.myPlayer].chest, (float)list2[0], 0.0f, 0.0f, 0, 0, 0);
                list2.RemoveAt(0);
            }
            int index4 = 3;
            while (index4 >= 0 && list1.Count > 0)
            {
                int index1 = list1[0];
                if (numArray1[index4] > 0)
                {
                    pInv[index1].SetDefaults(71 + index4, false);
                    pInv[index1].stack = numArray1[index4];
                    if (pInv[index1].stack > pInv[index1].maxStack)
                        pInv[index1].stack = pInv[index1].maxStack;
                    numArray1[index4] -= pInv[index1].stack;
                }
                if (numArray1[index4] == 0)
                    --index4;
                list1.RemoveAt(0);
            }
        }

        public static bool TryPlacingInChest(Item I, bool justCheck)
        {
            bool flag1 = false;
            Player player = Main.player[Main.myPlayer];
            Item[] objArray = player.bank.item;
            if (player.chest > -1)
            {
                objArray = Main.chest[player.chest].item;
                flag1 = Main.netMode == 1;
            }
            else if (player.chest == -2)
                objArray = player.bank.item;
            else if (player.chest == -3)
                objArray = player.bank2.item;
            bool flag2 = false;
            if (I.maxStack > 1)
            {
                for (int index = 0; index < 40; ++index)
                {
                    if (objArray[index].stack < objArray[index].maxStack && I.IsTheSameAs(objArray[index]))
                    {
                        int num = I.stack;
                        if (I.stack + objArray[index].stack > objArray[index].maxStack)
                            num = objArray[index].maxStack - objArray[index].stack;
                        if (justCheck)
                        {
                            flag2 = flag2 || num > 0;
                            break;
                        }
                        I.stack -= num;
                        objArray[index].stack += num;
                        Main.PlaySound(7, -1, -1, 1);
                        if (I.stack <= 0)
                        {
                            I.SetDefaults(0, false);
                            if (flag1)
                            {
                                NetMessage.SendData(32, -1, -1, "", player.chest, (float)index, 0.0f, 0.0f, 0, 0, 0);
                                break;
                            }
                            break;
                        }
                        if (objArray[index].type == 0)
                        {
                            objArray[index] = I.Clone();
                            I.SetDefaults(0, false);
                        }
                        if (flag1)
                            NetMessage.SendData(32, -1, -1, "", player.chest, (float)index, 0.0f, 0.0f, 0, 0, 0);
                    }
                }
            }
            if (I.stack > 0)
            {
                for (int index = 0; index < 40; ++index)
                {
                    if (objArray[index].stack == 0)
                    {
                        if (justCheck)
                        {
                            flag2 = true;
                            break;
                        }
                        Main.PlaySound(7, -1, -1, 1);
                        objArray[index] = I.Clone();
                        I.SetDefaults(0, false);
                        if (flag1)
                        {
                            NetMessage.SendData(32, -1, -1, "", player.chest, (float)index, 0.0f, 0.0f, 0, 0, 0);
                            break;
                        }
                        break;
                    }
                }
            }
            return flag2;
        }

        public static bool TryPlacingInPlayer(int slot, bool justCheck)
        {
            bool flag1 = false;
            Player player = Main.player[Main.myPlayer];
            Item[] objArray1 = player.inventory;
            Item[] objArray2 = player.bank.item;
            if (player.chest > -1)
            {
                objArray2 = Main.chest[player.chest].item;
                flag1 = Main.netMode == 1;
            }
            else if (player.chest == -2)
                objArray2 = player.bank.item;
            else if (player.chest == -3)
                objArray2 = player.bank2.item;
            Item obj = objArray2[slot];
            bool flag2 = false;
            if (obj.maxStack > 1)
            {
                for (int index = 49; index >= 0; --index)
                {
                    if (objArray1[index].stack < objArray1[index].maxStack && obj.IsTheSameAs(objArray1[index]))
                    {
                        int num = obj.stack;
                        if (obj.stack + objArray1[index].stack > objArray1[index].maxStack)
                            num = objArray1[index].maxStack - objArray1[index].stack;
                        if (justCheck)
                        {
                            flag2 = flag2 || num > 0;
                            break;
                        }
                        obj.stack -= num;
                        objArray1[index].stack += num;
                        Main.PlaySound(7, -1, -1, 1);
                        if (obj.stack <= 0)
                        {
                            obj.SetDefaults(0, false);
                            if (flag1)
                            {
                                NetMessage.SendData(32, -1, -1, "", player.chest, (float)index, 0.0f, 0.0f, 0, 0, 0);
                                break;
                            }
                            break;
                        }
                        if (objArray1[index].type == 0)
                        {
                            objArray1[index] = obj.Clone();
                            obj.SetDefaults(0, false);
                        }
                        if (flag1)
                            NetMessage.SendData(32, -1, -1, "", player.chest, (float)index, 0.0f, 0.0f, 0, 0, 0);
                    }
                }
            }
            if (obj.stack > 0)
            {
                for (int index = 49; index >= 0; --index)
                {
                    if (objArray1[index].stack == 0)
                    {
                        if (justCheck)
                        {
                            flag2 = true;
                            break;
                        }
                        Main.PlaySound(7, -1, -1, 1);
                        objArray1[index] = obj.Clone();
                        obj.SetDefaults(0, false);
                        if (flag1)
                        {
                            NetMessage.SendData(32, -1, -1, "", player.chest, (float)index, 0.0f, 0.0f, 0, 0, 0);
                            break;
                        }
                        break;
                    }
                }
            }
            return flag2;
        }

        public class ButtonID
        {
            public const int LootAll = 0;
            public const int DepositAll = 1;
            public const int QuickStack = 2;
            public const int Restock = 3;
            public const int RenameChest = 4;
            public const int RenameChestCancel = 5;
            public const int Count = 6;
        }
    }
}
