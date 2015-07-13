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
using Terraria;
using Terraria.Achievements;
using Terraria.Graphics;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Elements
{
    internal class UIAchievementListItem : UIPanel
    {
        private const int _iconSize = 64;
        private const int _iconSizeWithSpace = 66;
        private const int _iconsPerRow = 8;
        private Achievement _achievement;
        private UIImageFramed _achievementIcon;
        private UIImage _achievementIconBorders;
        private int _iconIndex;
        private Rectangle _iconFrame;
        private Rectangle _iconFrameUnlocked;
        private Rectangle _iconFrameLocked;
        private Texture2D _innerPanelTopTexture;
        private Texture2D _innerPanelBottomTexture;
        private Texture2D _categoryTexture;
        private bool _locked;

        public UIAchievementListItem(Achievement achievement)
        {
            BackgroundColor = new Color(26, 40, 89) * 0.8f;
            BorderColor = new Color(13, 20, 44) * 0.8f;
            _achievement = achievement;
            Height.Set(82f, 0.0f);
            Width.Set(0.0f, 1f);
            PaddingTop = 8f;
            PaddingLeft = 9f;
            int iconIndex = Main.Achievements.GetIconIndex(achievement.Name);
            _iconIndex = iconIndex;
            _iconFrameUnlocked = new Rectangle(iconIndex % 8 * 66, iconIndex / 8 * 66, 64, 64);
            _iconFrameLocked = _iconFrameUnlocked;
            _iconFrameLocked.X += 528;
            _iconFrame = _iconFrameLocked;
            UpdateIconFrame();
            _achievementIcon = new UIImageFramed(TextureManager.Load("Images/UI/Achievements"), _iconFrame);
            Append(_achievementIcon);
            _achievementIconBorders = new UIImage(TextureManager.Load("Images/UI/Achievement_Borders"));
            _achievementIconBorders.Left.Set(-4f, 0.0f);
            _achievementIconBorders.Top.Set(-4f, 0.0f);
            Append(_achievementIconBorders);
            _innerPanelTopTexture = TextureManager.Load("Images/UI/Achievement_InnerPanelTop");
            _innerPanelBottomTexture = TextureManager.Load("Images/UI/Achievement_InnerPanelBottom");
            _categoryTexture = TextureManager.Load("Images/UI/Achievement_Categories");
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            _locked = !_achievement.IsCompleted;
            UpdateIconFrame();
            CalculatedStyle innerDimensions = GetInnerDimensions();
            CalculatedStyle dimensions = _achievementIconBorders.GetDimensions();
            Vector2 vector2 = new Vector2(dimensions.X + dimensions.Width + 7f, innerDimensions.Y);
            Tuple<Decimal, Decimal> trackerValues = GetTrackerValues();
            bool flag = false;
            if ((!(trackerValues.Item1 == new Decimal(0)) || !(trackerValues.Item2 == new Decimal(0))) && _locked)
                flag = true;

            float num = (float)(innerDimensions.Width - dimensions.Width + 1.0);
            Vector2 baseScale1 = new Vector2(0.85f);
            Vector2 baseScale2 = new Vector2(0.92f);
            Vector2 stringSize1 = ChatManager.GetStringSize(Main.fontItemStack, _achievement.Description, baseScale2, num);
            if (stringSize1.Y > 38.0)
                baseScale2.Y *= 38f / stringSize1.Y;

            Color baseColor1 = Color.Lerp(_locked ? Color.Silver : Color.Gold, Color.White, IsMouseHovering ? 0.5f : 0.0f);
            Color baseColor2 = Color.Lerp(_locked ? Color.DarkGray : Color.Silver, Color.White, IsMouseHovering ? 1f : 0.0f);
            Color color1 = this.IsMouseHovering ? Color.White : Color.Gray;
            Vector2 position1 = vector2 - Vector2.UnitY * 2f;
            DrawPanelTop(spriteBatch, position1, num, color1);
            AchievementCategory category = _achievement.Category;
            position1.Y += 2f;
            position1.X += 4f;
            spriteBatch.Draw(_categoryTexture, position1, new Rectangle?(Utils.Frame(_categoryTexture, 4, 2, (int)category, 0)),
                IsMouseHovering ? Color.White : Color.Silver, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.0f);
            position1.X += 4f;
            position1.X += 17f;
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, _achievement.FriendlyName, position1, baseColor1, 0.0f, Vector2.Zero, baseScale1, num, 2f);
            position1.X -= 17f;
            Vector2 position2 = vector2 + Vector2.UnitY * 27f;
            this.DrawPanelBottom(spriteBatch, position2, num, color1);
            position2.X += 8f;
            position2.Y += 4f;
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, _achievement.Description, position2, baseColor2, 0.0f, Vector2.Zero, baseScale2, num - 10f, 2f);
            if (!flag)
                return;

            Vector2 position3 = position1 + Vector2.UnitX * num + Vector2.UnitY;
            string text = (int)trackerValues.Item1 + "/" + (int)trackerValues.Item2;
            Vector2 baseScale3 = new Vector2(0.75f);
            Vector2 stringSize2 = ChatManager.GetStringSize(Main.fontItemStack, text, baseScale3, -1f);
            float progress = (float)(trackerValues.Item1 / trackerValues.Item2);
            float Width = 80f;
            Color color2 = new Color(100, 255, 100);
            if (!IsMouseHovering)
                color2 = Color.Lerp(color2, Color.Black, 0.25f);
            Color BackColor = new Color(255, 255, 255);
            if (!IsMouseHovering)
                BackColor = Color.Lerp(BackColor, Color.Black, 0.25f);
            DrawProgressBar(spriteBatch, progress, position3 - Vector2.UnitX * Width * 0.7f, Width, BackColor, color2, Utils.MultiplyRGBA(color2, new Color(new Vector4(1f, 1f, 1f, 0.5f))));
            position3.X -= Width * 1.4f + stringSize2.X;
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, text, position3, baseColor1, 0.0f, new Vector2(0.0f, 0.0f), baseScale3, 90f, 2f);
        }

        private void UpdateIconFrame()
        {
            _iconFrame = _locked ? _iconFrameLocked : _iconFrameUnlocked;
            if (_achievementIcon == null)
                return;
            _achievementIcon.SetFrame(_iconFrame);
        }

        private void DrawPanelTop(SpriteBatch spriteBatch, Vector2 position, float width, Color color)
        {
            spriteBatch.Draw(_innerPanelTopTexture, position, new Rectangle?(new Rectangle(0, 0, 2, _innerPanelTopTexture.Height)), color);
            spriteBatch.Draw(_innerPanelTopTexture, new Vector2(position.X + 2f, position.Y), new Rectangle?(new Rectangle(2, 0, 2, _innerPanelTopTexture.Height)), color, 0.0f,
                Vector2.Zero, new Vector2((float)((width - 4.0) / 2.0), 1f), SpriteEffects.None, 0.0f);
            spriteBatch.Draw(_innerPanelTopTexture, new Vector2((float)(position.X + width - 2.0), position.Y), new Rectangle?(new Rectangle(4, 0, 2, _innerPanelTopTexture.Height)), color);
        }

        private void DrawPanelBottom(SpriteBatch spriteBatch, Vector2 position, float width, Color color)
        {
            spriteBatch.Draw(_innerPanelBottomTexture, position, new Rectangle?(new Rectangle(0, 0, 6, _innerPanelBottomTexture.Height)), color);
            spriteBatch.Draw(_innerPanelBottomTexture, new Vector2(position.X + 6f, position.Y), new Rectangle?(new Rectangle(6, 0, 7, _innerPanelBottomTexture.Height)), color, 0.0f,
                Vector2.Zero, new Vector2((float)((width - 12.0) / 7.0), 1f), SpriteEffects.None, 0.0f);
            spriteBatch.Draw(_innerPanelBottomTexture, new Vector2((float)(position.X + width - 6.0), position.Y), new Rectangle?(new Rectangle(13, 0, 6, _innerPanelBottomTexture.Height)), color);
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
            BackgroundColor = new Color(46, 60, 119);
            BorderColor = new Color(20, 30, 56);
        }

        public override void MouseOut(UIMouseEvent evt)
        {
            base.MouseOut(evt);
            BackgroundColor = new Color(26, 40, 89) * 0.8f;
            BorderColor = new Color(13, 20, 44) * 0.8f;
        }

        public Achievement GetAchievement()
        {
            return _achievement;
        }

        private Tuple<Decimal, Decimal> GetTrackerValues()
        {
            if (!_achievement.HasTracker)
                return Tuple.Create<Decimal, Decimal>(new Decimal(0), new Decimal(0));

            IAchievementTracker tracker = this._achievement.GetTracker();
            if (tracker.GetTrackerType() == TrackerType.Int)
            {
                AchievementTracker<int> achievementTracker = (AchievementTracker<int>)tracker;
                return Tuple.Create<Decimal, Decimal>((Decimal)achievementTracker.Value, (Decimal)achievementTracker.MaxValue);
            }

            if (tracker.GetTrackerType() != TrackerType.Float)
                return Tuple.Create<Decimal, Decimal>(new Decimal(0), new Decimal(0));

            AchievementTracker<float> achievementTracker1 = (AchievementTracker<float>)tracker;
            return Tuple.Create<Decimal, Decimal>((Decimal)achievementTracker1.Value, (Decimal)achievementTracker1.MaxValue);
        }

        private void DrawProgressBar(SpriteBatch spriteBatch, float progress, Vector2 spot, float Width = 169f, Color BackColor = default(Color), Color FillingColor = default(Color), Color BlipColor = default(Color))
        {
            if (BlipColor == Color.Transparent)
                BlipColor = new Color(255, 165, 0, 127);
            if (FillingColor == Color.Transparent)
                FillingColor = new Color((255, 241, 51);
            if (BackColor == Color.Transparent)
                FillingColor = new Color(255, 255, 255);

            Texture2D texture1 = Main.colorBarTexture;
            Texture2D texture2D = Main.colorBlipTexture;
            Texture2D texture2 = Main.magicPixel;
            float num1 = MathHelper.Clamp(progress, 0.0f, 1f);
            float num2 = Width * 1f;
            float y = 8f;
            float x = num2 / 169f;
            Vector2 vector2 = spot + Vector2.UnitY * y + Vector2.UnitX * 1f;
            spriteBatch.Draw(texture1, spot, new Rectangle?(new Rectangle(5, 0, texture1.Width - 9, texture1.Height)), BackColor, 0.0f, new Vector2(84.5f, 0.0f), new Vector2(x, 1f), SpriteEffects.None, 0.0f);
            spriteBatch.Draw(texture1, spot + new Vector2((float)(-(double)x * 84.5 - 5.0), 0.0f), new Rectangle?(new Rectangle(0, 0, 5, texture1.Height)), BackColor, 0.0f,
                Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(texture1, spot + new Vector2(x * 84.5f, 0.0f), new Rectangle?(new Rectangle(texture1.Width - 4, 0, 4, texture1.Height)), BackColor, 0.0f,
                Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
            Vector2 position = vector2 + Vector2.UnitX * (num1 - 0.5f) * num2;
            --position.X;

            spriteBatch.Draw(texture2, position, new Rectangle?(new Rectangle(0, 0, 1, 1)), FillingColor, 0.0f, new Vector2(1f, 0.5f), new Vector2(num2 * num1, y), SpriteEffects.None, 0.0f);
            if (progress != 0.0)
                spriteBatch.Draw(texture2, position, new Rectangle?(new Rectangle(0, 0, 1, 1)), BlipColor, 0.0f, new Vector2(1f, 0.5f), new Vector2(2f, y), SpriteEffects.None, 0.0f);
            spriteBatch.Draw(texture2, position, new Rectangle?(new Rectangle(0, 0, 1, 1)), Color.Black, 0.0f, new Vector2(0.0f, 0.5f), new Vector2(num2 * (1f - num1), y), SpriteEffects.None, 0.0f);
        }

        public override int CompareTo(object obj)
        {
            UIAchievementListItem achievementListItem = obj as UIAchievementListItem;
            if (achievementListItem == null)
                return 0;
            if (_achievement.IsCompleted && !achievementListItem._achievement.IsCompleted)
                return -1;
            if (!_achievement.IsCompleted && achievementListItem._achievement.IsCompleted)
                return 1;

            return _achievement.Id.CompareTo(achievementListItem._achievement.Id);
        }
    }
}
