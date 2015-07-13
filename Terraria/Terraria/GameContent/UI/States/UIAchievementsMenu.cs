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
using Terraria.Achievements;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.States
{
    public class UIAchievementsMenu : UIState
    {
        private List<UIAchievementListItem> _achievementElements = new List<UIAchievementListItem>();
        private List<UIToggleImage> _categoryButtons = new List<UIToggleImage>();
        private UIList _achievementsList;
        private UIElement _outerContainer;

        public override void OnInitialize()
        {
            UIElement element1 = new UIElement();
            element1.Width.Set(0.0f, 0.8f);
            element1.MaxWidth.Set(800f, 0.0f);
            element1.MinWidth.Set(600f, 0.0f);
            element1.Top.Set(220f, 0.0f);
            element1.Height.Set(-220f, 1f);
            element1.HAlign = 0.5f;
            _outerContainer = element1;
            Append(element1);
            UIPanel uiPanel = new UIPanel();
            uiPanel.Width.Set(0.0f, 1f);
            uiPanel.Height.Set(-110f, 1f);
            uiPanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
            uiPanel.PaddingTop = 0.0f;
            element1.Append(uiPanel);
            _achievementsList = new UIList();
            _achievementsList.Width.Set(-25f, 1f);
            _achievementsList.Height.Set(-50f, 1f);
            _achievementsList.Top.Set(50f, 0.0f);
            _achievementsList.ListPadding = 5f;
            uiPanel.Append(_achievementsList);
            UITextPanel uiTextPanel1 = new UITextPanel("Achievements", 1f, true);
            uiTextPanel1.HAlign = 0.5f;
            uiTextPanel1.Top.Set(-33f, 0.0f);
            uiTextPanel1.SetPadding(13f);
            uiTextPanel1.BackgroundColor = new Color(73, 94, 171);
            element1.Append(uiTextPanel1);
            UITextPanel uiTextPanel2 = new UITextPanel("Back", 0.7f, true);
            uiTextPanel2.Width.Set(-10f, 0.5f);
            uiTextPanel2.Height.Set(50f, 0.0f);
            uiTextPanel2.VAlign = 1f;
            uiTextPanel2.HAlign = 0.5f;
            uiTextPanel2.Top.Set(-45f, 0.0f);
            uiTextPanel2.OnMouseOver += new UIElement.MouseEvent(FadedMouseOver);
            uiTextPanel2.OnMouseOut += new UIElement.MouseEvent(FadedMouseOut);
            uiTextPanel2.OnClick += new UIElement.MouseEvent(GoBackClick);
            element1.Append(uiTextPanel2);
            List<Achievement> achievementsList = Main.Achievements.CreateAchievementsList();
            for (int index = 0; index < achievementsList.Count; ++index)
            {
                UIAchievementListItem achievementListItem = new UIAchievementListItem(achievementsList[index]);
                _achievementsList.Add((UIElement)achievementListItem);
                _achievementElements.Add(achievementListItem);
            }

            UIScrollbar scrollbar = new UIScrollbar();
            scrollbar.SetView(100f, 1000f);
            scrollbar.Height.Set(-50f, 1f);
            scrollbar.Top.Set(50f, 0.0f);
            scrollbar.HAlign = 1f;
            uiPanel.Append((UIElement)scrollbar);
            _achievementsList.SetScrollbar(scrollbar);
            UIElement element2 = new UIElement();
            element2.Width.Set(0.0f, 1f);
            element2.Height.Set(32f, 0.0f);
            element2.Top.Set(10f, 0.0f);
            Texture2D texture = TextureManager.Load("Images/UI/Achievement_Categories");
            for (int index = 0; index < 4; ++index)
            {
                UIToggleImage uiToggleImage = new UIToggleImage(texture, 32, 32, new Point(34 * index, 0), new Point(34 * index, 34));
                uiToggleImage.Left.Set((float)(index * 36 + 8), 0.0f);
                uiToggleImage.SetState(true);
                uiToggleImage.OnClick += new UIElement.MouseEvent(FilterList);
                _categoryButtons.Add(uiToggleImage);
                element2.Append((UIElement)uiToggleImage);
            }
            uiPanel.Append(element2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            for (int index = 0; index < _categoryButtons.Count; ++index)
            {
                if (_categoryButtons[index].IsMouseHovering)
                {
                    string text;
                    switch (index)
                    {
                        case -1:
                            text = "None";
                            break;
                        case 0:
                            text = "Slayer";
                            break;
                        case 1:
                            text = "Collector";
                            break;
                        case 2:
                            text = "Explorer";
                            break;
                        case 3:
                            text = "Challenger";
                            break;
                        default:
                            text = "None";
                            break;
                    }

                    float num = Main.fontMouseText.MeasureString(text).X;
                    Vector2 vector2 = new Vector2((float)Main.mouseX, (float)Main.mouseY) + new Vector2(16f);
                    if (vector2.Y > (Main.screenHeight - 30))
                        vector2.Y = (float)(Main.screenHeight - 30);
                    if (vector2.X > Main.screenWidth - num)
                        vector2.X = (float)(Main.screenWidth - 460);
                    Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, text, vector2.X, vector2.Y, new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor,
                        (int)Main.mouseTextColor, (int)Main.mouseTextColor), Color.Black, Vector2.Zero, 1f);
                    break;
                }
            }
        }

        public void GotoAchievement(Achievement achievement)
        {
            _achievementsList.Goto((UIList.ElementSearchMethod)(element =>
            {
                UIAchievementListItem achievementListItem = element as UIAchievementListItem;
                if (achievementListItem == null)
                    return false;
                return achievementListItem.GetAchievement() == achievement;
            }));
        }

        private void GoBackClick(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.menuMode = 0;
            AchievementsUI.Close();
        }

        private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(12, -1, -1, 1);
            ((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
        }

        private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
        {
            ((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
        }

        private void FilterList(UIMouseEvent evt, UIElement listeningElement)
        {
            _achievementsList.Clear();
            foreach (UIAchievementListItem achievementListItem in _achievementElements)
            {
                if (_categoryButtons[(int)achievementListItem.GetAchievement().Category].IsOn)
                    _achievementsList.Add((UIElement)achievementListItem);
            }
            Recalculate();
        }

        public override void OnActivate()
        {
            if (Main.gameMenu)
            {
                _outerContainer.Top.Set(220f, 0.0f);
                _outerContainer.Height.Set(-220f, 1f);
            }
            else
            {
                _outerContainer.Top.Set(120f, 0.0f);
                _outerContainer.Height.Set(-120f, 1f);
            }
            _achievementsList.UpdateOrder();
        }
    }
}
