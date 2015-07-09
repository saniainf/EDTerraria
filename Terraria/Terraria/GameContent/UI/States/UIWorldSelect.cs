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
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.IO;
using Terraria.UI;

namespace Terraria.GameContent.UI.States
{
    internal class UIWorldSelect : UIState
    {
        private UIList _worldList;

        public override void OnInitialize()
        {
            UIElement element = new UIElement();
            element.Width.Set(0.0f, 0.8f);
            element.MaxWidth.Set(600f, 0.0f);
            element.Top.Set(220f, 0.0f);
            element.Height.Set(-220f, 1f);
            element.HAlign = 0.5f;
            UIPanel uiPanel = new UIPanel();
            uiPanel.Width.Set(0.0f, 1f);
            uiPanel.Height.Set(-110f, 1f);
            uiPanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
            element.Append((UIElement)uiPanel);
            this._worldList = new UIList();
            this._worldList.Width.Set(-25f, 1f);
            this._worldList.Height.Set(0.0f, 1f);
            this._worldList.ListPadding = 5f;
            uiPanel.Append((UIElement)this._worldList);
            UIScrollbar scrollbar = new UIScrollbar();
            scrollbar.SetView(100f, 1000f);
            scrollbar.Height.Set(0.0f, 1f);
            scrollbar.HAlign = 1f;
            uiPanel.Append((UIElement)scrollbar);
            this._worldList.SetScrollbar(scrollbar);
            UITextPanel uiTextPanel1 = new UITextPanel("Select World", 0.8f, true);
            uiTextPanel1.HAlign = 0.5f;
            uiTextPanel1.Top.Set(-35f, 0.0f);
            uiTextPanel1.SetPadding(15f);
            uiTextPanel1.BackgroundColor = new Color(73, 94, 171);
            element.Append((UIElement)uiTextPanel1);
            UITextPanel uiTextPanel2 = new UITextPanel("Back", 0.7f, true);
            uiTextPanel2.Width.Set(-10f, 0.5f);
            uiTextPanel2.Height.Set(50f, 0.0f);
            uiTextPanel2.VAlign = 1f;
            uiTextPanel2.Top.Set(-45f, 0.0f);
            uiTextPanel2.OnMouseOver += new UIElement.MouseEvent(this.FadedMouseOver);
            uiTextPanel2.OnMouseOut += new UIElement.MouseEvent(this.FadedMouseOut);
            uiTextPanel2.OnClick += new UIElement.MouseEvent(this.GoBackClick);
            element.Append((UIElement)uiTextPanel2);
            UITextPanel uiTextPanel3 = new UITextPanel("New", 0.7f, true);
            uiTextPanel3.CopyStyle((UIElement)uiTextPanel2);
            uiTextPanel3.HAlign = 1f;
            uiTextPanel3.OnMouseOver += new UIElement.MouseEvent(this.FadedMouseOver);
            uiTextPanel3.OnMouseOut += new UIElement.MouseEvent(this.FadedMouseOut);
            uiTextPanel3.OnClick += new UIElement.MouseEvent(this.NewWorldClick);
            element.Append((UIElement)uiTextPanel3);
            this.Append(element);
        }

        private void NewWorldClick(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(10, -1, -1, 1);
            Main.menuMode = 16;
            Main.newWorldName = Lang.gen[57] + " " + (Main.WorldList.Count + 1);
        }

        private void GoBackClick(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(11, -1, -1, 1);
            Main.menuMode = Main.menuMultiplayer ? 12 : 1;
        }

        private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(12, -1, -1, 1);
            ((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
        }

        private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
        {
            ((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.7f;
        }

        public override void OnActivate()
        {
            Main.LoadWorlds();
            this._worldList.Clear();
            foreach (WorldFileData data in Main.WorldList)
                this._worldList.Add((UIElement)new UIWorldListItem(data));
        }
    }
}
