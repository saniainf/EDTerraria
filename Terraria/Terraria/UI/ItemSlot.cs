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
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.UI.Chat;
using Terraria.ID;
using Terraria.UI.Chat;

namespace Terraria.UI
{
    public class ItemSlot
    {
        private static Item[] singleSlotArray = new Item[1];
        private static bool[] canFavoriteAt = new bool[23];
        private static int dyeSlotCount = 0;
        private static int accSlotCount = 0;

        static ItemSlot()
        {
            ItemSlot.canFavoriteAt[0] = true;
            ItemSlot.canFavoriteAt[1] = true;
            ItemSlot.canFavoriteAt[2] = true;
        }

        public static void Handle(ref Item inv, int context = 0)
        {
            ItemSlot.singleSlotArray[0] = inv;
            ItemSlot.Handle(ItemSlot.singleSlotArray, context, 0);
            inv = ItemSlot.singleSlotArray[0];
            Recipe.FindRecipes();
        }

        public static void Handle(Item[] inv, int context = 0, int slot = 0)
        {
            ItemSlot.OverrideHover(inv, context, slot);
            if (Main.mouseLeftRelease && Main.mouseLeft)
            {
                ItemSlot.LeftClick(inv, context, slot);
                Recipe.FindRecipes();
            }
            else
                ItemSlot.RightClick(inv, context, slot);
            ItemSlot.MouseHover(inv, context, slot);
        }

        public static void OverrideHover(Item[] inv, int context = 0, int slot = 0)
        {
            Item obj = inv[slot];
            if (Main.keyState.IsKeyDown(Keys.LeftShift) && obj.type > 0 && (obj.stack > 0 && !inv[slot].favorited))
            {
                switch (context)
                {
                    case 0:
                    case 1:
                    case 2:
                        if (Main.npcShop > 0 && !obj.favorited)
                        {
                            Main.cursorOverride = 10;
                            break;
                        }
                        if (Main.player[Main.myPlayer].chest != -1)
                        {
                            if (ChestUI.TryPlacingInChest(obj, true))
                            {
                                Main.cursorOverride = 9;
                                break;
                            }
                            break;
                        }
                        Main.cursorOverride = 6;
                        break;
                    case 3:
                    case 4:
                        if (Main.player[Main.myPlayer].ItemSpace(obj))
                        {
                            Main.cursorOverride = 8;
                            break;
                        }
                        break;
                    case 5:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                    case 16:
                    case 17:
                    case 18:
                    case 19:
                    case 20:
                        if (Main.player[Main.myPlayer].ItemSpace(inv[slot]))
                        {
                            Main.cursorOverride = 7;
                            break;
                        }
                        break;
                }
            }
            if (!Main.keyState.IsKeyDown(Keys.LeftAlt) || !ItemSlot.canFavoriteAt[context])
                return;
            if (obj.type > 0 && obj.stack > 0 && Main.chatMode)
            {
                Main.cursorOverride = 2;
            }
            else
            {
                if (obj.type <= 0 || obj.stack <= 0)
                    return;
                Main.cursorOverride = 3;
            }
        }

        private static bool OverrideLeftClick(Item[] inv, int context = 0, int slot = 0)
        {
            Item I = inv[slot];
            if (Main.cursorOverride == 2)
            {
                if (ChatManager.AddChatText(Main.fontMouseText, ItemTagHandler.GenerateTag(I), Vector2.One))
                    Main.PlaySound(12, -1, -1, 1);
                return true;
            }
            if (Main.cursorOverride == 3)
            {
                if (!ItemSlot.canFavoriteAt[context])
                    return false;
                I.favorited = !I.favorited;
                Main.PlaySound(12, -1, -1, 1);
                return true;
            }
            if (Main.cursorOverride == 7)
            {
                inv[slot] = Main.player[Main.myPlayer].GetItem(Main.myPlayer, inv[slot], false, true);
                Main.PlaySound(12, -1, -1, 1);
                return true;
            }
            if (Main.cursorOverride == 8)
            {
                inv[slot] = Main.player[Main.myPlayer].GetItem(Main.myPlayer, inv[slot], false, true);
                if (Main.player[Main.myPlayer].chest > -1)
                    NetMessage.SendData(32, -1, -1, "", Main.player[Main.myPlayer].chest, (float)slot, 0.0f, 0.0f, 0, 0, 0);
                return true;
            }
            if (Main.cursorOverride != 9)
                return false;
            ChestUI.TryPlacingInChest(inv[slot], false);
            return true;
        }

        public static void LeftClick(ref Item inv, int context = 0)
        {
            ItemSlot.singleSlotArray[0] = inv;
            ItemSlot.LeftClick(ItemSlot.singleSlotArray, context, 0);
            inv = ItemSlot.singleSlotArray[0];
        }

        public static void LeftClick(Item[] inv, int context = 0, int slot = 0)
        {
            if (ItemSlot.OverrideLeftClick(inv, context, slot))
                return;
            inv[slot].newAndShiny = false;
            Player player = Main.player[Main.myPlayer];
            bool flag = false;
            switch (context)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                    flag = player.chest == -1;
                    break;
            }
            if (Main.keyState.IsKeyDown(Keys.LeftShift) && flag)
            {
                if (inv[slot].type <= 0)
                    return;
                if (Main.npcShop > 0 && !inv[slot].favorited)
                {
                    Chest chest = Main.instance.shop[Main.npcShop];
                    if (inv[slot].type >= 71 && inv[slot].type <= 74)
                        return;
                    if (player.SellItem(inv[slot].value, inv[slot].stack))
                    {
                        chest.AddShop(inv[slot]);
                        inv[slot].SetDefaults(0, false);
                        Main.PlaySound(18, -1, -1, 1);
                        Recipe.FindRecipes();
                    }
                    else
                    {
                        if (inv[slot].value != 0)
                            return;
                        chest.AddShop(inv[slot]);
                        inv[slot].SetDefaults(0, false);
                        Main.PlaySound(7, -1, -1, 1);
                        Recipe.FindRecipes();
                    }
                }
                else
                {
                    if (inv[slot].favorited || ItemSlot.Options.DisableLeftShiftTrashCan)
                        return;
                    Main.PlaySound(7, -1, -1, 1);
                    player.trashItem = inv[slot].Clone();
                    inv[slot].SetDefaults(0, false);
                    if (context == 3 && Main.netMode == 1)
                        NetMessage.SendData(32, -1, -1, "", player.chest, (float)slot, 0.0f, 0.0f, 0, 0, 0);
                    Recipe.FindRecipes();
                }
            }
            else
            {
                if (player.selectedItem == slot && player.itemAnimation > 0 || player.itemTime != 0)
                    return;
                switch (ItemSlot.PickItemMovementAction(inv, context, slot, Main.mouseItem))
                {
                    case 0:
                        if (context == 6 && Main.mouseItem.type != 0)
                            inv[slot].SetDefaults(0, false);
                        Utils.Swap<Item>(ref inv[slot], ref Main.mouseItem);
                        if (inv[slot].stack > 0)
                        {
                            switch (context)
                            {
                                case 0:
                                    AchievementsHelper.NotifyItemPickup(player, inv[slot]);
                                    break;
                                case 8:
                                case 9:
                                case 10:
                                case 11:
                                case 12:
                                case 16:
                                case 17:
                                    AchievementsHelper.HandleOnEquip(player, inv[slot], context);
                                    break;
                            }
                        }
                        if (inv[slot].type == 0 || inv[slot].stack < 1)
                            inv[slot] = new Item();
                        if (Main.mouseItem.IsTheSameAs(inv[slot]))
                        {
                            Utils.Swap<bool>(ref inv[slot].favorited, ref Main.mouseItem.favorited);
                            if (inv[slot].stack != inv[slot].maxStack && Main.mouseItem.stack != Main.mouseItem.maxStack)
                            {
                                if (Main.mouseItem.stack + inv[slot].stack <= Main.mouseItem.maxStack)
                                {
                                    inv[slot].stack += Main.mouseItem.stack;
                                    Main.mouseItem.stack = 0;
                                }
                                else
                                {
                                    int num = Main.mouseItem.maxStack - inv[slot].stack;
                                    inv[slot].stack += num;
                                    Main.mouseItem.stack -= num;
                                }
                            }
                        }
                        if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
                            Main.mouseItem = new Item();
                        if (Main.mouseItem.type > 0 || inv[slot].type > 0)
                        {
                            Recipe.FindRecipes();
                            Main.PlaySound(7, -1, -1, 1);
                        }
                        if (context == 3 && Main.netMode == 1)
                        {
                            NetMessage.SendData(32, -1, -1, "", player.chest, (float)slot, 0.0f, 0.0f, 0, 0, 0);
                            break;
                        }
                        break;
                    case 1:
                        if (Main.mouseItem.stack == 1 && Main.mouseItem.type > 0 && (inv[slot].type > 0 && inv[slot].IsNotTheSameAs(Main.mouseItem)))
                        {
                            Utils.Swap<Item>(ref inv[slot], ref Main.mouseItem);
                            Main.PlaySound(7, -1, -1, 1);
                            if (inv[slot].stack > 0)
                            {
                                switch (context)
                                {
                                    case 0:
                                        AchievementsHelper.NotifyItemPickup(player, inv[slot]);
                                        break;
                                    case 8:
                                    case 9:
                                    case 10:
                                    case 11:
                                    case 12:
                                    case 16:
                                    case 17:
                                        AchievementsHelper.HandleOnEquip(player, inv[slot], context);
                                        break;
                                }
                            }
                            else
                                break;
                        }
                        else
                        {
                            if (Main.mouseItem.type == 0 && inv[slot].type > 0)
                            {
                                Utils.Swap<Item>(ref inv[slot], ref Main.mouseItem);
                                if (inv[slot].type == 0 || inv[slot].stack < 1)
                                    inv[slot] = new Item();
                                if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
                                    Main.mouseItem = new Item();
                                if (Main.mouseItem.type > 0 || inv[slot].type > 0)
                                {
                                    Recipe.FindRecipes();
                                    Main.PlaySound(7, -1, -1, 1);
                                    break;
                                }
                                break;
                            }
                            if (Main.mouseItem.type > 0 && inv[slot].type == 0)
                            {
                                if (Main.mouseItem.stack == 1)
                                {
                                    Utils.Swap<Item>(ref inv[slot], ref Main.mouseItem);
                                    if (inv[slot].type == 0 || inv[slot].stack < 1)
                                        inv[slot] = new Item();
                                    if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
                                        Main.mouseItem = new Item();
                                    if (Main.mouseItem.type > 0 || inv[slot].type > 0)
                                    {
                                        Recipe.FindRecipes();
                                        Main.PlaySound(7, -1, -1, 1);
                                    }
                                }
                                else
                                {
                                    --Main.mouseItem.stack;
                                    inv[slot].SetDefaults(Main.mouseItem.type, false);
                                    Recipe.FindRecipes();
                                    Main.PlaySound(7, -1, -1, 1);
                                }
                                if (inv[slot].stack > 0)
                                {
                                    switch (context)
                                    {
                                        case 0:
                                            AchievementsHelper.NotifyItemPickup(player, inv[slot]);
                                            break;
                                        case 8:
                                        case 9:
                                        case 10:
                                        case 11:
                                        case 12:
                                        case 16:
                                        case 17:
                                            AchievementsHelper.HandleOnEquip(player, inv[slot], context);
                                            break;
                                    }
                                }
                                else
                                    break;
                            }
                            else
                                break;
                        }
                        break;
                    case 2:
                        if (Main.mouseItem.stack == 1 && (int)Main.mouseItem.dye > 0 && (inv[slot].type > 0 && inv[slot].type != Main.mouseItem.type))
                        {
                            Utils.Swap<Item>(ref inv[slot], ref Main.mouseItem);
                            Main.PlaySound(7, -1, -1, 1);
                            if (inv[slot].stack > 0)
                            {
                                switch (context)
                                {
                                    case 0:
                                        AchievementsHelper.NotifyItemPickup(player, inv[slot]);
                                        break;
                                    case 8:
                                    case 9:
                                    case 10:
                                    case 11:
                                    case 12:
                                    case 16:
                                    case 17:
                                        AchievementsHelper.HandleOnEquip(player, inv[slot], context);
                                        break;
                                }
                            }
                            else
                                break;
                        }
                        else
                        {
                            if (Main.mouseItem.type == 0 && inv[slot].type > 0)
                            {
                                Utils.Swap<Item>(ref inv[slot], ref Main.mouseItem);
                                if (inv[slot].type == 0 || inv[slot].stack < 1)
                                    inv[slot] = new Item();
                                if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
                                    Main.mouseItem = new Item();
                                if (Main.mouseItem.type > 0 || inv[slot].type > 0)
                                {
                                    Recipe.FindRecipes();
                                    Main.PlaySound(7, -1, -1, 1);
                                    break;
                                }
                                break;
                            }
                            if ((int)Main.mouseItem.dye > 0 && inv[slot].type == 0)
                            {
                                if (Main.mouseItem.stack == 1)
                                {
                                    Utils.Swap<Item>(ref inv[slot], ref Main.mouseItem);
                                    if (inv[slot].type == 0 || inv[slot].stack < 1)
                                        inv[slot] = new Item();
                                    if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
                                        Main.mouseItem = new Item();
                                    if (Main.mouseItem.type > 0 || inv[slot].type > 0)
                                    {
                                        Recipe.FindRecipes();
                                        Main.PlaySound(7, -1, -1, 1);
                                    }
                                }
                                else
                                {
                                    --Main.mouseItem.stack;
                                    inv[slot].SetDefaults(Main.mouseItem.type, false);
                                    Recipe.FindRecipes();
                                    Main.PlaySound(7, -1, -1, 1);
                                }
                                if (inv[slot].stack > 0)
                                {
                                    switch (context)
                                    {
                                        case 0:
                                            AchievementsHelper.NotifyItemPickup(player, inv[slot]);
                                            break;
                                        case 8:
                                        case 9:
                                        case 10:
                                        case 11:
                                        case 12:
                                        case 16:
                                        case 17:
                                            AchievementsHelper.HandleOnEquip(player, inv[slot], context);
                                            break;
                                    }
                                }
                                else
                                    break;
                            }
                            else
                                break;
                        }
                        break;
                    case 3:
                        Main.mouseItem.netDefaults(inv[slot].netID);
                        if (inv[slot].buyOnce)
                            Main.mouseItem.Prefix((int)inv[slot].prefix);
                        else
                            Main.mouseItem.Prefix(-1);
                        Main.mouseItem.position = player.Center - new Vector2((float)Main.mouseItem.width, (float)Main.mouseItem.headSlot) / 2f;
                        ItemText.NewText(Main.mouseItem, Main.mouseItem.stack, false, false);
                        if (inv[slot].buyOnce && --inv[slot].stack <= 0)
                            inv[slot].SetDefaults(0, false);
                        if (inv[slot].value > 0)
                        {
                            Main.PlaySound(18, -1, -1, 1);
                            break;
                        }
                        Main.PlaySound(7, -1, -1, 1);
                        break;
                    case 4:
                        Chest chest = Main.instance.shop[Main.npcShop];
                        if (player.SellItem(Main.mouseItem.value, Main.mouseItem.stack))
                        {
                            chest.AddShop(Main.mouseItem);
                            Main.mouseItem.SetDefaults(0, false);
                            Main.PlaySound(18, -1, -1, 1);
                        }
                        else if (Main.mouseItem.value == 0)
                        {
                            chest.AddShop(Main.mouseItem);
                            Main.mouseItem.SetDefaults(0, false);
                            Main.PlaySound(7, -1, -1, 1);
                        }
                        Recipe.FindRecipes();
                        break;
                }
                switch (context)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 5:
                        break;
                    default:
                        inv[slot].favorited = false;
                        break;
                }
            }
        }

        public static int PickItemMovementAction(Item[] inv, int context, int slot, Item checkItem)
        {
            Player player = Main.player[Main.myPlayer];
            int num = -1;
            if (context == 0)
                num = 0;
            else if (context == 1)
            {
                if (checkItem.type == 0 || checkItem.type == 71 || (checkItem.type == 72 || checkItem.type == 73) || checkItem.type == 74)
                    num = 0;
            }
            else if (context == 2)
            {
                if ((checkItem.type == 0 || checkItem.ammo > 0 || checkItem.bait > 0) && !checkItem.notAmmo || checkItem.type == 530)
                    num = 0;
            }
            else if (context == 3)
                num = 0;
            else if (context == 4)
                num = 0;
            else if (context == 5)
            {
                if (checkItem.Prefix(-3) || checkItem.type == 0)
                    num = 0;
            }
            else if (context == 6)
                num = 0;
            else if (context == 7)
            {
                if (checkItem.material || checkItem.type == 0)
                    num = 0;
            }
            else if (context == 8)
            {
                if (checkItem.type == 0 || checkItem.headSlot > -1 && slot == 0 || (checkItem.bodySlot > -1 && slot == 1 || checkItem.legSlot > -1 && slot == 2))
                    num = 1;
            }
            else if (context == 9)
            {
                if (checkItem.type == 0 || checkItem.headSlot > -1 && slot == 10 || (checkItem.bodySlot > -1 && slot == 11 || checkItem.legSlot > -1 && slot == 12))
                    num = 1;
            }
            else if (context == 10)
            {
                if (checkItem.type == 0 || checkItem.accessory && !ItemSlot.AccCheck(checkItem, slot))
                    num = 1;
            }
            else if (context == 11)
            {
                if (checkItem.type == 0 || checkItem.accessory && !ItemSlot.AccCheck(checkItem, slot))
                    num = 1;
            }
            else if (context == 12)
                num = 2;
            else if (context == 15)
            {
                if (checkItem.type == 0 && inv[slot].type > 0)
                {
                    if (player.BuyItem(inv[slot].value))
                        num = 3;
                }
                else if (inv[slot].type == 0 && checkItem.type > 0 && (checkItem.type < 71 || checkItem.type > 74))
                    num = 4;
            }
            else if (context == 16)
            {
                if (checkItem.type == 0 || Main.projHook[checkItem.shoot])
                    num = 1;
            }
            else if (context == 17)
            {
                if (checkItem.type == 0 || checkItem.mountType != -1 && !MountID.Sets.Cart[checkItem.mountType])
                    num = 1;
            }
            else if (context == 19)
            {
                if (checkItem.type == 0 || checkItem.buffType > 0 && Main.vanityPet[checkItem.buffType] && !Main.lightPet[checkItem.buffType])
                    num = 1;
            }
            else if (context == 18)
            {
                if (checkItem.type == 0 || checkItem.mountType != -1 && MountID.Sets.Cart[checkItem.mountType])
                    num = 1;
            }
            else if (context == 20 && (checkItem.type == 0 || checkItem.buffType > 0 && Main.lightPet[checkItem.buffType]))
                num = 1;
            return num;
        }

        public static void RightClick(ref Item inv, int context = 0)
        {
            ItemSlot.singleSlotArray[0] = inv;
            ItemSlot.RightClick(ItemSlot.singleSlotArray, context, 0);
            inv = ItemSlot.singleSlotArray[0];
        }

        public static void RightClick(Item[] inv, int context = 0, int slot = 0)
        {
            Player player = Main.player[Main.myPlayer];
            inv[slot].newAndShiny = false;
            if (player.itemAnimation > 0)
                return;
            bool flag1 = false;
            if (context == 0)
            {
                flag1 = true;
                if (Main.mouseRight && inv[slot].type >= 3318 && inv[slot].type <= 3332)
                {
                    if (Main.mouseRightRelease)
                    {
                        player.OpenBossBag(inv[slot].type);
                        --inv[slot].stack;
                        if (inv[slot].stack == 0)
                            inv[slot].SetDefaults(0, false);
                        Main.PlaySound(7, -1, -1, 1);
                        Main.stackSplit = 30;
                        Main.mouseRightRelease = false;
                        Recipe.FindRecipes();
                    }
                }
                else if (Main.mouseRight && (inv[slot].type >= 2334 && inv[slot].type <= 2336 || inv[slot].type >= 3203 && inv[slot].type <= 3208))
                {
                    if (Main.mouseRightRelease)
                    {
                        player.openCrate(inv[slot].type);
                        --inv[slot].stack;
                        if (inv[slot].stack == 0)
                            inv[slot].SetDefaults(0, false);
                        Main.PlaySound(7, -1, -1, 1);
                        Main.stackSplit = 30;
                        Main.mouseRightRelease = false;
                        Recipe.FindRecipes();
                    }
                }
                else if (Main.mouseRight && inv[slot].type == 3093)
                {
                    if (Main.mouseRightRelease)
                    {
                        player.openHerbBag();
                        --inv[slot].stack;
                        if (inv[slot].stack == 0)
                            inv[slot].SetDefaults(0, false);
                        Main.PlaySound(7, -1, -1, 1);
                        Main.stackSplit = 30;
                        Main.mouseRightRelease = false;
                        Recipe.FindRecipes();
                    }
                }
                else if (Main.mouseRight && inv[slot].type == 1774)
                {
                    if (Main.mouseRightRelease)
                    {
                        --inv[slot].stack;
                        if (inv[slot].stack == 0)
                            inv[slot].SetDefaults(0, false);
                        Main.PlaySound(7, -1, -1, 1);
                        Main.stackSplit = 30;
                        Main.mouseRightRelease = false;
                        player.openGoodieBag();
                        Recipe.FindRecipes();
                    }
                }
                else if (Main.mouseRight && inv[slot].type == 3085)
                {
                    if (Main.mouseRightRelease && player.consumeItem(327))
                    {
                        --inv[slot].stack;
                        if (inv[slot].stack == 0)
                            inv[slot].SetDefaults(0, false);
                        Main.PlaySound(7, -1, -1, 1);
                        Main.stackSplit = 30;
                        Main.mouseRightRelease = false;
                        player.openLockBox();
                        Recipe.FindRecipes();
                    }
                }
                else if (Main.mouseRight && inv[slot].type == 1869)
                {
                    if (Main.mouseRightRelease)
                    {
                        --inv[slot].stack;
                        if (inv[slot].stack == 0)
                            inv[slot].SetDefaults(0, false);
                        Main.PlaySound(7, -1, -1, 1);
                        Main.stackSplit = 30;
                        Main.mouseRightRelease = false;
                        player.openPresent();
                        Recipe.FindRecipes();
                    }
                }
                else if (Main.mouseRight && Main.mouseRightRelease && (inv[slot].type == 599 || inv[slot].type == 600 || inv[slot].type == 601))
                {
                    Main.PlaySound(7, -1, -1, 1);
                    Main.stackSplit = 30;
                    Main.mouseRightRelease = false;
                    int num = Main.rand.Next(14);
                    if (num == 0 && Main.hardMode)
                        inv[slot].SetDefaults(602, false);
                    else if (num <= 7)
                    {
                        inv[slot].SetDefaults(586, false);
                        inv[slot].stack = Main.rand.Next(20, 50);
                    }
                    else
                    {
                        inv[slot].SetDefaults(591, false);
                        inv[slot].stack = Main.rand.Next(20, 50);
                    }
                    Recipe.FindRecipes();
                }
                else
                    flag1 = false;
            }
            else if (context == 9 || context == 11)
            {
                flag1 = true;
                if (Main.mouseRight && Main.mouseRightRelease && (inv[slot].type > 0 && inv[slot].stack > 0 || inv[slot - 10].type > 0 && inv[slot - 10].stack > 0))
                {
                    bool flag2 = true;
                    if (flag2 && context == 11 && (int)inv[slot].wingSlot > 0)
                    {
                        for (int index = 3; index < 10; ++index)
                        {
                            if ((int)inv[index].wingSlot > 0 && index != slot - 10)
                                flag2 = false;
                        }
                    }
                    if (flag2)
                    {
                        Utils.Swap<Item>(ref inv[slot], ref inv[slot - 10]);
                        Main.PlaySound(7, -1, -1, 1);
                        Recipe.FindRecipes();
                        if (inv[slot].stack > 0)
                        {
                            switch (context)
                            {
                                case 0:
                                    AchievementsHelper.NotifyItemPickup(player, inv[slot]);
                                    break;
                                case 8:
                                case 9:
                                case 10:
                                case 11:
                                case 12:
                                case 16:
                                case 17:
                                    AchievementsHelper.HandleOnEquip(player, inv[slot], context);
                                    break;
                            }
                        }
                    }
                }
            }
            else if (context == 12)
            {
                flag1 = true;
                if (Main.mouseRight && Main.mouseRightRelease && (Main.mouseItem.stack < Main.mouseItem.maxStack && Main.mouseItem.type > 0) && (inv[slot].type > 0 && Main.mouseItem.type == inv[slot].type))
                {
                    ++Main.mouseItem.stack;
                    inv[slot].SetDefaults(0, false);
                    Main.PlaySound(7, -1, -1, 1);
                }
            }
            else if (context == 15)
            {
                flag1 = true;
                Chest chest = Main.instance.shop[Main.npcShop];
                if (Main.stackSplit <= 1 && Main.mouseRight && inv[slot].type > 0 && (Main.mouseItem.IsTheSameAs(inv[slot]) || Main.mouseItem.type == 0))
                {
                    int num = Main.superFastStack + 1;
                    for (int index = 0; index < num; ++index)
                    {
                        if ((Main.mouseItem.stack < Main.mouseItem.maxStack || Main.mouseItem.type == 0) && (player.BuyItem(inv[slot].value) && inv[slot].stack > 0))
                        {
                            if (index == 0)
                                Main.PlaySound(18, -1, -1, 1);
                            if (Main.mouseItem.type == 0)
                            {
                                Main.mouseItem.netDefaults(inv[slot].netID);
                                if ((int)inv[slot].prefix != 0)
                                    Main.mouseItem.Prefix((int)inv[slot].prefix);
                                Main.mouseItem.stack = 0;
                            }
                            ++Main.mouseItem.stack;
                            Main.stackSplit = Main.stackSplit != 0 ? Main.stackDelay : 15;
                            if (inv[slot].buyOnce && --inv[slot].stack <= 0)
                                inv[slot].SetDefaults(0, false);
                        }
                    }
                }
            }
            if (flag1)
                return;
            if ((context == 0 || context == 4 || context == 3) && (Main.mouseRight && Main.mouseRightRelease && inv[slot].maxStack == 1))
            {
                if ((int)inv[slot].dye > 0)
                {
                    bool success;
                    inv[slot] = ItemSlot.DyeSwap(inv[slot], out success);
                    if (success)
                    {
                        Main.EquipPageSelected = 0;
                        AchievementsHelper.HandleOnEquip(player, inv[slot], 12);
                    }
                }
                else if (Main.projHook[inv[slot].shoot])
                {
                    bool success;
                    inv[slot] = ItemSlot.EquipSwap(inv[slot], player.miscEquips, 4, out success);
                    if (success)
                    {
                        Main.EquipPageSelected = 2;
                        AchievementsHelper.HandleOnEquip(player, inv[slot], 16);
                    }
                }
                else if (inv[slot].mountType != -1 && !MountID.Sets.Cart[inv[slot].mountType])
                {
                    bool success;
                    inv[slot] = ItemSlot.EquipSwap(inv[slot], player.miscEquips, 3, out success);
                    if (success)
                    {
                        Main.EquipPageSelected = 2;
                        AchievementsHelper.HandleOnEquip(player, inv[slot], 17);
                    }
                }
                else if (inv[slot].mountType != -1 && MountID.Sets.Cart[inv[slot].mountType])
                {
                    bool success;
                    inv[slot] = ItemSlot.EquipSwap(inv[slot], player.miscEquips, 2, out success);
                    if (success)
                        Main.EquipPageSelected = 2;
                }
                else if (inv[slot].buffType > 0 && Main.lightPet[inv[slot].buffType])
                {
                    bool success;
                    inv[slot] = ItemSlot.EquipSwap(inv[slot], player.miscEquips, 1, out success);
                    if (success)
                        Main.EquipPageSelected = 2;
                }
                else if (inv[slot].buffType > 0 && Main.vanityPet[inv[slot].buffType])
                {
                    bool success;
                    inv[slot] = ItemSlot.EquipSwap(inv[slot], player.miscEquips, 0, out success);
                    if (success)
                        Main.EquipPageSelected = 2;
                }
                else
                {
                    bool success;
                    inv[slot] = ItemSlot.ArmorSwap(inv[slot], out success);
                    if (success)
                    {
                        Main.EquipPageSelected = 0;
                        AchievementsHelper.HandleOnEquip(player, inv[slot], 8);
                    }
                }
                Recipe.FindRecipes();
                if (context != 3 || Main.netMode != 1)
                    return;
                NetMessage.SendData(32, -1, -1, "", player.chest, (float)slot, 0.0f, 0.0f, 0, 0, 0);
            }
            else
            {
                if (Main.stackSplit > 1 || !Main.mouseRight)
                    return;
                bool flag2 = true;
                if (context == 0 && inv[slot].maxStack <= 1)
                    flag2 = false;
                if (context == 3 && inv[slot].maxStack <= 1)
                    flag2 = false;
                if (context == 4 && inv[slot].maxStack <= 1)
                    flag2 = false;
                if (!flag2 || !Main.mouseItem.IsTheSameAs(inv[slot]) && Main.mouseItem.type != 0 || Main.mouseItem.stack >= Main.mouseItem.maxStack && Main.mouseItem.type != 0)
                    return;
                if (Main.mouseItem.type == 0)
                {
                    Main.mouseItem = inv[slot].Clone();
                    Main.mouseItem.stack = 0;
                    Main.mouseItem.favorited = inv[slot].favorited && inv[slot].maxStack == 1;
                }
                ++Main.mouseItem.stack;
                --inv[slot].stack;
                if (inv[slot].stack <= 0)
                    inv[slot] = new Item();
                Recipe.FindRecipes();
                Main.soundInstanceMenuTick.Stop();
                Main.soundInstanceMenuTick = Main.soundMenuTick.CreateInstance();
                Main.PlaySound(12, -1, -1, 1);
                Main.stackSplit = Main.stackSplit != 0 ? Main.stackDelay : 15;
                if (context != 3 || Main.netMode != 1)
                    return;
                NetMessage.SendData(32, -1, -1, "", player.chest, (float)slot, 0.0f, 0.0f, 0, 0, 0);
            }
        }

        public static void Draw(SpriteBatch spriteBatch, ref Item inv, int context, Vector2 position, Color lightColor = default(Color))
        {
            ItemSlot.singleSlotArray[0] = inv;
            ItemSlot.Draw(spriteBatch, ItemSlot.singleSlotArray, context, 0, position, lightColor);
            inv = ItemSlot.singleSlotArray[0];
        }

        public static void Draw(SpriteBatch spriteBatch, Item[] inv, int context, int slot, Vector2 position, Color lightColor = default(Color))
        {
            Player player = Main.player[Main.myPlayer];
            Item obj = inv[slot];
            float num1 = Main.inventoryScale;
            Color color1 = Color.White;
            if (lightColor != Color.Transparent)
                color1 = lightColor;
            Texture2D texture2D1 = Main.inventoryBackTexture;
            Color color2 = Main.inventoryBack;
            bool flag = false;
            if (obj.type > 0 && obj.stack > 0 && (obj.favorited && context != 13) && (context != 21 && context != 22))
                texture2D1 = Main.inventoryBack10Texture;
            else if (obj.type > 0 && obj.stack > 0 && (ItemSlot.Options.HighlightNewItems && obj.newAndShiny) && (context != 13 && context != 21 && context != 22))
            {
                texture2D1 = Main.inventoryBack15Texture;
                float num2 = (float)((double)((float)Main.mouseTextColor / (float)byte.MaxValue) * 0.200000002980232 + 0.800000011920929);
                color2 = Utils.MultiplyRGBA(color2, new Color(num2, num2, num2));
            }
            else if (context == 0 && slot < 10)
                texture2D1 = Main.inventoryBack9Texture;
            else if (context == 10 || context == 8 || (context == 16 || context == 17) || (context == 19 || context == 18 || context == 20))
                texture2D1 = Main.inventoryBack3Texture;
            else if (context == 11 || context == 9)
                texture2D1 = Main.inventoryBack8Texture;
            else if (context == 12)
                texture2D1 = Main.inventoryBack12Texture;
            else if (context == 3)
                texture2D1 = Main.inventoryBack5Texture;
            else if (context == 4)
                texture2D1 = Main.inventoryBack2Texture;
            else if (context == 7 || context == 5)
                texture2D1 = Main.inventoryBack4Texture;
            else if (context == 6)
                texture2D1 = Main.inventoryBack7Texture;
            else if (context == 13)
            {
                byte num2 = (byte)200;
                if (slot == Main.player[Main.myPlayer].selectedItem)
                {
                    texture2D1 = Main.inventoryBack14Texture;
                    num2 = byte.MaxValue;
                }
                color2 = new Color((int)num2, (int)num2, (int)num2, (int)num2);
            }
            else if (context == 14 || context == 21)
                flag = true;
            else if (context == 15)
                texture2D1 = Main.inventoryBack6Texture;
            else if (context == 22)
                texture2D1 = Main.inventoryBack4Texture;
            if (!flag)
                spriteBatch.Draw(texture2D1, position, new Rectangle?(), color2, 0.0f, new Vector2(), num1, SpriteEffects.None, 0.0f);
            int num3 = -1;
            switch (context)
            {
                case 8:
                    if (slot == 0)
                        num3 = 0;
                    if (slot == 1)
                        num3 = 6;
                    if (slot == 2)
                    {
                        num3 = 12;
                        break;
                    }
                    break;
                case 9:
                    if (slot == 10)
                        num3 = 3;
                    if (slot == 11)
                        num3 = 9;
                    if (slot == 12)
                    {
                        num3 = 15;
                        break;
                    }
                    break;
                case 10:
                    num3 = 11;
                    break;
                case 11:
                    num3 = 2;
                    break;
                case 12:
                    num3 = 1;
                    break;
                case 16:
                    num3 = 4;
                    break;
                case 17:
                    num3 = 13;
                    break;
                case 18:
                    num3 = 7;
                    break;
                case 19:
                    num3 = 10;
                    break;
                case 20:
                    num3 = 17;
                    break;
            }
            if ((obj.type <= 0 || obj.stack <= 0) && num3 != -1)
            {
                Texture2D texture2D2 = Main.extraTexture[54];
                Rectangle r = Utils.Frame(texture2D2, 3, 6, num3 % 3, num3 / 3);
                r.Width -= 2;
                r.Height -= 2;
                spriteBatch.Draw(texture2D2, position + Utils.Size(texture2D1) / 2f * num1, new Rectangle?(r), Color.White * 0.35f, 0.0f, Utils.Size(r) / 2f, num1, SpriteEffects.None, 0.0f);
            }
            if (obj.type > 0 && obj.stack > 0)
            {
                Texture2D texture2D2 = Main.itemTexture[obj.type];
                Rectangle r = Main.itemAnimations[obj.type] == null ? Utils.Frame(texture2D2, 1, 1, 0, 0) : Main.itemAnimations[obj.type].GetFrame(texture2D2);
                Color currentColor = color1;
                float scale1 = 1f;
                ItemSlot.GetItemLight(ref currentColor, ref scale1, obj, false);
                float num2 = 1f;
                if (r.Width > 32 || r.Height > 32)
                    num2 = r.Width <= r.Height ? 32f / (float)r.Height : 32f / (float)r.Width;
                float scale2 = num2 * num1;
                Vector2 position1 = position + Utils.Size(texture2D1) * num1 / 2f - Utils.Size(r) * scale2 / 2f;
                Vector2 origin = Utils.Size(r) * (float)((double)scale1 / 2.0 - 0.5);
                spriteBatch.Draw(texture2D2, position1, new Rectangle?(r), obj.GetAlpha(currentColor), 0.0f, origin, scale2 * scale1, SpriteEffects.None, 0.0f);
                if (obj.color != Color.Transparent)
                    spriteBatch.Draw(texture2D2, position1, new Rectangle?(r), obj.GetColor(color1), 0.0f, origin, scale2 * scale1, SpriteEffects.None, 0.0f);
                if (obj.stack > 1)
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, obj.stack.ToString(), position + new Vector2(10f, 26f) * num1, color1, 0.0f, Vector2.Zero, new Vector2(num1), -1f, num1);
                int num4 = -1;
                if (context == 13)
                {
                    if (obj.useAmmo > 0)
                    {
                        int num5 = obj.useAmmo;
                        num4 = 0;
                        for (int index = 0; index < 58; ++index)
                        {
                            if (inv[index].ammo == num5)
                                num4 += inv[index].stack;
                        }
                    }
                    if (obj.fishingPole > 0)
                    {
                        num4 = 0;
                        for (int index = 0; index < 58; ++index)
                        {
                            if (inv[index].bait > 0)
                                num4 += inv[index].stack;
                        }
                    }
                    if (obj.tileWand > 0)
                    {
                        int num5 = obj.tileWand;
                        num4 = 0;
                        for (int index = 0; index < 58; ++index)
                        {
                            if (inv[index].type == num5)
                                num4 += inv[index].stack;
                        }
                    }
                    if (obj.type == 509 || obj.type == 851 || obj.type == 850)
                    {
                        num4 = 0;
                        for (int index = 0; index < 58; ++index)
                        {
                            if (inv[index].type == 530)
                                num4 += inv[index].stack;
                        }
                    }
                }
                if (num4 != -1)
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, num4.ToString(), position + new Vector2(8f, 30f) * num1, color1, 0.0f, Vector2.Zero, new Vector2(num1 * 0.8f), -1f, num1);
                if (context == 13)
                {
                    string text = string.Concat((object)(slot + 1));
                    if (text == "10")
                        text = "0";
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, text, position + new Vector2(8f, 4f) * num1, color1, 0.0f, Vector2.Zero, new Vector2(num1), -1f, num1);
                }
                if (context == 13 && obj.potion)
                {
                    Vector2 position2 = position + Utils.Size(texture2D1) * num1 / 2f - Utils.Size(Main.cdTexture) * num1 / 2f;
                    Color color3 = obj.GetAlpha(color1) * ((float)player.potionDelay / (float)player.potionDelayTime);
                    spriteBatch.Draw(Main.cdTexture, position2, new Rectangle?(), color3, 0.0f, new Vector2(), scale2, SpriteEffects.None, 0.0f);
                }
                if ((context == 10 || context == 18) && (obj.expertOnly && !Main.expertMode))
                {
                    Vector2 position2 = position + Utils.Size(texture2D1) * num1 / 2f - Utils.Size(Main.cdTexture) * num1 / 2f;
                    Color white = Color.White;
                    spriteBatch.Draw(Main.cdTexture, position2, new Rectangle?(), white, 0.0f, new Vector2(), scale2, SpriteEffects.None, 0.0f);
                }
            }
            else if (context == 6)
            {
                Texture2D texture2D2 = Main.trashTexture;
                Vector2 position1 = position + Utils.Size(texture2D1) * num1 / 2f - Utils.Size(texture2D2) * num1 / 2f;
                spriteBatch.Draw(texture2D2, position1, new Rectangle?(), new Color(100, 100, 100, 100), 0.0f, new Vector2(), num1, SpriteEffects.None, 0.0f);
            }
            if (context != 0 || slot >= 10)
                return;
            float num6 = num1;
            string text1 = string.Concat((object)(slot + 1));
            if (text1 == "10")
                text1 = "0";
            Color baseColor = Main.inventoryBack;
            int num7 = 0;
            if (Main.player[Main.myPlayer].selectedItem == slot)
            {
                num7 -= 3;
                baseColor.R = byte.MaxValue;
                baseColor.B = (byte)0;
                baseColor.G = (byte)210;
                baseColor.A = (byte)100;
                float num2 = num6 * 1.4f;
            }
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, text1, position + new Vector2(6f, (float)(4 + num7)) * num1, baseColor, 0.0f, Vector2.Zero, new Vector2(num1), -1f, num1);
        }

        public static void MouseHover(ref Item inv, int context = 0)
        {
            ItemSlot.singleSlotArray[0] = inv;
            ItemSlot.MouseHover(ItemSlot.singleSlotArray, context, 0);
            inv = ItemSlot.singleSlotArray[0];
        }

        public static void MouseHover(Item[] inv, int context = 0, int slot = 0)
        {
            if (context == 6 && Main.hoverItemName == null)
                Main.hoverItemName = Lang.inter[3];
            if (inv[slot].type > 0 && inv[slot].stack > 0)
            {
                Main.hoverItemName = inv[slot].name;
                if (inv[slot].stack > 1)
                    Main.hoverItemName = string.Concat(new object[4]
          {
            (object) Main.hoverItemName,
            (object) " (",
            (object) inv[slot].stack,
            (object) ")"
          });
                Main.toolTip = inv[slot].Clone();
                if (context == 8 && slot <= 2)
                    Main.toolTip.wornArmor = true;
                if (context == 11 || context == 9)
                    Main.toolTip.social = true;
                if (context != 15)
                    return;
                Main.toolTip.buy = true;
            }
            else
            {
                if (context == 10 || context == 11)
                    Main.hoverItemName = Lang.inter[9];
                if (context == 11)
                    Main.hoverItemName = Lang.inter[11] + " " + Main.hoverItemName;
                if (context == 8 || context == 9)
                {
                    if (slot == 0 || slot == 10)
                        Main.hoverItemName = Lang.inter[12];
                    if (slot == 1 || slot == 11)
                        Main.hoverItemName = Lang.inter[13];
                    if (slot == 2 || slot == 12)
                        Main.hoverItemName = Lang.inter[14];
                    if (slot >= 10)
                        Main.hoverItemName = Lang.inter[11] + " " + Main.hoverItemName;
                }
                if (context == 12)
                    Main.hoverItemName = Lang.inter[57];
                if (context == 16)
                    Main.hoverItemName = Lang.inter[90];
                if (context == 17)
                    Main.hoverItemName = Lang.inter[91];
                if (context == 19)
                    Main.hoverItemName = Lang.inter[92];
                if (context == 18)
                    Main.hoverItemName = Lang.inter[93];
                if (context != 20)
                    return;
                Main.hoverItemName = Lang.inter[94];
            }
        }

        private static bool AccCheck(Item item, int slot)
        {
            Player player = Main.player[Main.myPlayer];
            if (slot != -1 && (player.armor[slot].IsTheSameAs(item) || (int)player.armor[slot].wingSlot > 0 && (int)item.wingSlot > 0))
                return false;
            for (int index = 0; index < player.armor.Length; ++index)
            {
                if (slot < 10 && index < 10 && ((int)item.wingSlot > 0 && (int)player.armor[index].wingSlot > 0 || slot >= 10 && index >= 10 && ((int)item.wingSlot > 0 && (int)player.armor[index].wingSlot > 0)) || item.IsTheSameAs(player.armor[index]))
                    return true;
            }
            return false;
        }

        private static Item DyeSwap(Item item, out bool success)
        {
            success = false;
            if ((int)item.dye <= 0)
                return item;
            Player player = Main.player[Main.myPlayer];
            for (int index = 0; index < 10; ++index)
            {
                if (player.dye[index].type == 0)
                {
                    ItemSlot.dyeSlotCount = index;
                    break;
                }
            }
            if (ItemSlot.dyeSlotCount >= 10)
                ItemSlot.dyeSlotCount = 0;
            if (ItemSlot.dyeSlotCount < 0)
                ItemSlot.dyeSlotCount = 9;
            Item obj = player.dye[ItemSlot.dyeSlotCount].Clone();
            player.dye[ItemSlot.dyeSlotCount] = item.Clone();
            ++ItemSlot.dyeSlotCount;
            if (ItemSlot.dyeSlotCount >= 10)
                ItemSlot.accSlotCount = 0;
            Main.PlaySound(7, -1, -1, 1);
            Recipe.FindRecipes();
            success = true;
            return obj;
        }

        private static Item ArmorSwap(Item item, out bool success)
        {
            success = false;
            if (item.headSlot == -1 && item.bodySlot == -1 && (item.legSlot == -1 && !item.accessory))
                return item;
            Player player = Main.player[Main.myPlayer];
            int index1 = !item.vanity || item.accessory ? 0 : 10;
            item.favorited = false;
            Item obj = item;
            if (item.headSlot != -1)
            {
                obj = player.armor[index1].Clone();
                player.armor[index1] = item.Clone();
            }
            else if (item.bodySlot != -1)
            {
                obj = player.armor[index1 + 1].Clone();
                player.armor[index1 + 1] = item.Clone();
            }
            else if (item.legSlot != -1)
            {
                obj = player.armor[index1 + 2].Clone();
                player.armor[index1 + 2] = item.Clone();
            }
            else if (item.accessory)
            {
                int num = 5 + Main.player[Main.myPlayer].extraAccessorySlots;
                for (int index2 = 3; index2 < 3 + num; ++index2)
                {
                    if (player.armor[index2].type == 0)
                    {
                        ItemSlot.accSlotCount = index2 - 3;
                        break;
                    }
                }
                for (int index2 = 0; index2 < player.armor.Length; ++index2)
                {
                    if (item.IsTheSameAs(player.armor[index2]))
                        ItemSlot.accSlotCount = index2 - 3;
                    if (index2 < 10 && (int)item.wingSlot > 0 && (int)player.armor[index2].wingSlot > 0)
                        ItemSlot.accSlotCount = index2 - 3;
                }
                if (ItemSlot.accSlotCount >= num)
                    ItemSlot.accSlotCount = 0;
                if (ItemSlot.accSlotCount < 0)
                    ItemSlot.accSlotCount = num - 1;
                int index3 = 3 + ItemSlot.accSlotCount;
                for (int index2 = 0; index2 < player.armor.Length; ++index2)
                {
                    if (item.IsTheSameAs(player.armor[index2]))
                        index3 = index2;
                }
                obj = player.armor[index3].Clone();
                player.armor[index3] = item.Clone();
                ++ItemSlot.accSlotCount;
                if (ItemSlot.accSlotCount >= num)
                    ItemSlot.accSlotCount = 0;
            }
            Main.PlaySound(7, -1, -1, 1);
            Recipe.FindRecipes();
            success = true;
            return obj;
        }

        private static Item EquipSwap(Item item, Item[] inv, int slot, out bool success)
        {
            success = false;
            Player player = Main.player[Main.myPlayer];
            item.favorited = false;
            Item obj = inv[slot].Clone();
            inv[slot] = item.Clone();
            Main.PlaySound(7, -1, -1, 1);
            Recipe.FindRecipes();
            success = true;
            return obj;
        }

        public static void EquipPage(Item item)
        {
            Main.EquipPage = -1;
            if (Main.projHook[item.shoot])
                Main.EquipPage = 2;
            else if (item.mountType != -1)
                Main.EquipPage = 2;
            else if ((int)item.dye > 0 && Main.EquipPageSelected == 1)
            {
                Main.EquipPage = 0;
            }
            else
            {
                if (item.legSlot == -1 && item.headSlot == -1 && (item.bodySlot == -1 && !item.accessory))
                    return;
                Main.EquipPage = 0;
            }
        }

        public static void DrawMoney(SpriteBatch sb, string text, float shopx, float shopy, int[] coinsArray, bool horizontal = false)
        {
            Utils.DrawBorderStringFourWay(sb, Main.fontMouseText, text, shopx, shopy + 40f, Color.White * ((float)Main.mouseTextColor / (float)byte.MaxValue), Color.Black, Vector2.Zero, 1f);
            if (horizontal)
            {
                for (int index = 0; index < 4; ++index)
                {
                    if (index == 0)
                    {
                        int num = coinsArray[3 - index];
                    }
                    Vector2 position = new Vector2((float)((double)shopx + (double)ChatManager.GetStringSize(Main.fontMouseText, text, Vector2.One, -1f).X + (double)(24 * index) + 45.0), shopy + 50f);
                    sb.Draw(Main.itemTexture[74 - index], position, new Rectangle?(), Color.White, 0.0f, Utils.Size(Main.itemTexture[74 - index]) / 2f, 1f, SpriteEffects.None, 0.0f);
                    Utils.DrawBorderStringFourWay(sb, Main.fontItemStack, coinsArray[3 - index].ToString(), position.X - 11f, position.Y, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
                }
            }
            else
            {
                for (int index = 0; index < 4; ++index)
                {
                    int num = index != 0 || coinsArray[3 - index] <= 99 ? 0 : -6;
                    sb.Draw(Main.itemTexture[74 - index], new Vector2(shopx + 11f + (float)(24 * index), shopy + 75f), new Rectangle?(), Color.White, 0.0f, Utils.Size(Main.itemTexture[74 - index]) / 2f, 1f, SpriteEffects.None, 0.0f);
                    Utils.DrawBorderStringFourWay(sb, Main.fontItemStack, coinsArray[3 - index].ToString(), shopx + (float)(24 * index) + (float)num, shopy + 75f, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
                }
            }
        }

        public static void DrawSavings(SpriteBatch sb, float shopx, float shopy, bool horizontal = false)
        {
            Player player = Main.player[Main.myPlayer];
            bool overFlowing;
            long num1 = Utils.CoinsCount(out overFlowing, player.bank.item);
            long num2 = Utils.CoinsCount(out overFlowing, player.bank2.item);
            long count = Utils.CoinsCombineStacks(out overFlowing, num1, num2);
            if (count <= 0L)
                return;
            if (num2 > 0L)
                sb.Draw(Main.itemTexture[346], Utils.CenteredRectangle(new Vector2(shopx + 80f, shopy + 50f), Utils.Size(Main.itemTexture[346]) * 0.65f), new Rectangle?(), Color.White);
            if (num1 > 0L)
                sb.Draw(Main.itemTexture[87], Utils.CenteredRectangle(new Vector2(shopx + 70f, shopy + 60f), Utils.Size(Main.itemTexture[87]) * 0.65f), new Rectangle?(), Color.White);
            ItemSlot.DrawMoney(sb, Lang.inter[66], shopx, shopy, Utils.CoinsSplit(count), horizontal);
        }

        public static void GetItemLight(ref Color currentColor, Item item, bool outInTheWorld = false)
        {
            float scale = 1f;
            ItemSlot.GetItemLight(ref currentColor, ref scale, item, outInTheWorld);
        }

        public static void GetItemLight(ref Color currentColor, int type, bool outInTheWorld = false)
        {
            float scale = 1f;
            ItemSlot.GetItemLight(ref currentColor, ref scale, type, outInTheWorld);
        }

        public static void GetItemLight(ref Color currentColor, ref float scale, Item item, bool outInTheWorld = false)
        {
            ItemSlot.GetItemLight(ref currentColor, ref scale, item.type, outInTheWorld);
        }

        public static Color GetItemLight(ref Color currentColor, ref float scale, int type, bool outInTheWorld = false)
        {
            if (type < 0 || type > 3601)
                return currentColor;
            if (type == 662 || type == 663)
            {
                currentColor.R = (byte)Main.DiscoR;
                currentColor.G = (byte)Main.DiscoG;
                currentColor.B = (byte)Main.DiscoB;
                currentColor.A = byte.MaxValue;
            }
            else if (ItemID.Sets.ItemIconPulse[type])
            {
                scale = Main.essScale;
                currentColor.R = (byte)((double)currentColor.R * (double)scale);
                currentColor.G = (byte)((double)currentColor.G * (double)scale);
                currentColor.B = (byte)((double)currentColor.B * (double)scale);
                currentColor.A = (byte)((double)currentColor.A * (double)scale);
            }
            else if (type == 58 || type == 184)
            {
                scale = (float)((double)Main.essScale * 0.25 + 0.75);
                currentColor.R = (byte)((double)currentColor.R * (double)scale);
                currentColor.G = (byte)((double)currentColor.G * (double)scale);
                currentColor.B = (byte)((double)currentColor.B * (double)scale);
                currentColor.A = (byte)((double)currentColor.A * (double)scale);
            }
            return currentColor;
        }

        public class Options
        {
            public static bool DisableLeftShiftTrashCan = false;
            public static bool HighlightNewItems = true;
        }

        public class Context
        {
            public const int InventoryItem = 0;
            public const int InventoryCoin = 1;
            public const int InventoryAmmo = 2;
            public const int ChestItem = 3;
            public const int BankItem = 4;
            public const int PrefixItem = 5;
            public const int TrashItem = 6;
            public const int GuideItem = 7;
            public const int EquipArmor = 8;
            public const int EquipArmorVanity = 9;
            public const int EquipAccessory = 10;
            public const int EquipAccessoryVanity = 11;
            public const int EquipDye = 12;
            public const int HotbarItem = 13;
            public const int ChatItem = 14;
            public const int ShopItem = 15;
            public const int EquipGrapple = 16;
            public const int EquipMount = 17;
            public const int EquipMinecart = 18;
            public const int EquipPet = 19;
            public const int EquipLight = 20;
            public const int MouseItem = 21;
            public const int CraftingMaterial = 22;
            public const int Count = 23;
        }
    }
}
