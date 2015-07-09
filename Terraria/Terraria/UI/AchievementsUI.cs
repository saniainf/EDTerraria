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
using Terraria;
using Terraria.Achievements;

namespace Terraria.UI
{
    public class AchievementsUI
    {
        public static void Open()
        {
            Main.playerInventory = false;
            Main.editChest = false;
            Main.npcChatText = "";
            Main.achievementsWindow = true;
            Main.InGameUI.SetState((UIState)Main.AchievementsMenu);
        }

        public static void OpenAndGoto(Achievement achievement)
        {
            AchievementsUI.Open();
            Main.AchievementsMenu.GotoAchievement(achievement);
        }

        public static void Close()
        {
            Main.achievementsWindow = false;
            Main.PlaySound(11, -1, -1, 1);
            if (!Main.gameMenu)
                Main.playerInventory = true;
            Main.InGameUI.SetState((UIState)null);
        }

        public static void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!Main.gameMenu && Main.player[Main.myPlayer].dead && !Main.player[Main.myPlayer].ghost)
            {
                AchievementsUI.Close();
                Main.playerInventory = false;
            }
            else
            {
                if (Main.gameMenu)
                    return;
                Main.mouseText = false;
                Main.instance.GUIBarsDraw();
                if (!Main.achievementsWindow)
                    Main.InGameUI.SetState((UIState)null);
                Main.instance.DrawMouseOver();
                Main.DrawThickCursor(false);
                spriteBatch.Draw(Main.cursorTextures[0], new Vector2((float)(Main.mouseX + 1), (float)(Main.mouseY + 1)), new Rectangle?(), new Color((int)((double)Main.cursorColor.R * 0.200000002980232), (int)((double)Main.cursorColor.G * 0.200000002980232), (int)((double)Main.cursorColor.B * 0.200000002980232), (int)((double)Main.cursorColor.A * 0.5)), 0.0f, new Vector2(), Main.cursorScale * 1.1f, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(Main.cursorTextures[0], new Vector2((float)Main.mouseX, (float)Main.mouseY), new Rectangle?(), Main.cursorColor, 0.0f, new Vector2(), Main.cursorScale, SpriteEffects.None, 0.0f);
            }
        }

        public static void MouseOver()
        {
            if (!Main.achievementsWindow || !Main.InGameUI.IsElementUnderMouse())
                return;
            Main.mouseText = true;
        }
    }
}
