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
using Terraria.Graphics;
using Terraria.IO;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
    internal class UIWorldListItem : UIPanel
    {
        private WorldFileData _data;
        private Texture2D _dividerTexture;
        private Texture2D _innerPanelTexture;
        private UIImage _worldIcon;
        private UIText _buttonLabel;
        private UIText _deleteButtonLabel;
        private Texture2D _buttonCloudActiveTexture;
        private Texture2D _buttonCloudInactiveTexture;
        private Texture2D _buttonFavoriteActiveTexture;
        private Texture2D _buttonFavoriteInactiveTexture;
        private Texture2D _buttonPlayTexture;
        private Texture2D _buttonDeleteTexture;
        private UIImageButton _deleteButton;

        public bool IsFavorite
        {
            get { return _data.IsFavorite; }
        }

        public UIWorldListItem(WorldFileData data)
        {
            BorderColor = new Color(89, 116, 213) * 0.7f;
            _dividerTexture = TextureManager.Load("Images/UI/Divider");
            _innerPanelTexture = TextureManager.Load("Images/UI/InnerPanelBackground");
            _buttonCloudActiveTexture = TextureManager.Load("Images/UI/ButtonCloudActive");
            _buttonCloudInactiveTexture = TextureManager.Load("Images/UI/ButtonCloudInactive");
            _buttonFavoriteActiveTexture = TextureManager.Load("Images/UI/ButtonFavoriteActive");
            _buttonFavoriteInactiveTexture = TextureManager.Load("Images/UI/ButtonFavoriteInactive");
            _buttonPlayTexture = TextureManager.Load("Images/UI/ButtonPlay");
            _buttonDeleteTexture = TextureManager.Load("Images/UI/ButtonDelete");
            Height.Set(96f, 0.0f);
            Width.Set(0.0f, 1f);
            SetPadding(6f);
            _data = data;
            _worldIcon = new UIImage(GetIcon());
            _worldIcon.Left.Set(4f, 0.0f);
            _worldIcon.OnDoubleClick += new UIElement.MouseEvent(PlayGame);
            Append(_worldIcon);
            UIImageButton uiImageButton1 = new UIImageButton(_buttonPlayTexture);
            uiImageButton1.VAlign = 1f;
            uiImageButton1.Left.Set(4f, 0.0f);
            uiImageButton1.OnClick += new UIElement.MouseEvent(PlayGame);
            OnDoubleClick += new UIElement.MouseEvent(PlayGame);
            uiImageButton1.OnMouseOver += new UIElement.MouseEvent(PlayMouseOver);
            uiImageButton1.OnMouseOut += new UIElement.MouseEvent(ButtonMouseOut);
            Append(uiImageButton1);
            UIImageButton uiImageButton2 = new UIImageButton(_data.IsFavorite ? _buttonFavoriteActiveTexture : _buttonFavoriteInactiveTexture);
            uiImageButton2.VAlign = 1f;
            uiImageButton2.Left.Set(28f, 0.0f);
            uiImageButton2.OnClick += new UIElement.MouseEvent(FavoriteButtonClick);
            uiImageButton2.OnMouseOver += new UIElement.MouseEvent(FavoriteMouseOver);
            uiImageButton2.OnMouseOut += new UIElement.MouseEvent(ButtonMouseOut);
            uiImageButton2.SetVisibility(1f, this._data.IsFavorite ? 0.8f : 0.4f);
            Append(uiImageButton2);
            UIImageButton uiImageButton4 = new UIImageButton(_buttonDeleteTexture);
            uiImageButton4.VAlign = 1f;
            uiImageButton4.HAlign = 1f;
            uiImageButton4.OnClick += new UIElement.MouseEvent(DeleteButtonClick);
            uiImageButton4.OnMouseOver += new UIElement.MouseEvent(DeleteMouseOver);
            uiImageButton4.OnMouseOut += new UIElement.MouseEvent(DeleteMouseOut);
            _deleteButton = uiImageButton4;
            if (!_data.IsFavorite)
                Append(uiImageButton4);
            _buttonLabel = new UIText("", 1f, false);
            _buttonLabel.VAlign = 1f;
            _buttonLabel.Left.Set(80f, 0.0f);
            _buttonLabel.Top.Set(-3f, 0.0f);
            Append(_buttonLabel);
            _deleteButtonLabel = new UIText("", 1f, false);
            _deleteButtonLabel.VAlign = 1f;
            _deleteButtonLabel.HAlign = 1f;
            _deleteButtonLabel.Left.Set(-30f, 0.0f);
            _deleteButtonLabel.Top.Set(-3f, 0.0f);
            Append(_deleteButtonLabel);
        }

        private Texture2D GetIcon()
        {
            return TextureManager.Load("Images/UI/Icon" + (_data.IsHardMode ? "Hallow" : "") + (_data.HasCorruption ? "Corruption" : "Crimson"));
        }

        private void FavoriteMouseOver(UIMouseEvent evt, UIElement listeningElement)
        {
            if (_data.IsFavorite)
                _buttonLabel.SetText("Unfavorite");
            else
                _buttonLabel.SetText("Favorite");
        }

        private void PlayMouseOver(UIMouseEvent evt, UIElement listeningElement)
        {
            _buttonLabel.SetText("Play");
        }

        private void DeleteMouseOver(UIMouseEvent evt, UIElement listeningElement)
        {
            _deleteButtonLabel.SetText("Delete");
        }

        private void DeleteMouseOut(UIMouseEvent evt, UIElement listeningElement)
        {
            _deleteButtonLabel.SetText("");
        }

        private void ButtonMouseOut(UIMouseEvent evt, UIElement listeningElement)
        {
            _buttonLabel.SetText("");
        }

        private void DeleteButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            for (int index = 0; index < Main.WorldList.Count; ++index)
            {
                if (Main.WorldList[index] == _data)
                {
                    Main.PlaySound(10, -1, -1, 1);
                    Main.selectedWorld = index;
                    Main.menuMode = 9;
                    break;
                }
            }
        }

        private void PlayGame(UIMouseEvent evt, UIElement listeningElement)
        {
            if (listeningElement != evt.Target)
                return;

            _data.SetAsActive();
            Main.PlaySound(10, -1, -1, 1);
            Main.GetInputText("");
            Main.menuMode = 889;
            if (Main.menuMultiplayer)
                return;
            WorldGen.playWorld();
        }

        private void FavoriteButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            _data.ToggleFavorite();
            ((UIImageButton)evt.Target).SetImage(_data.IsFavorite ? _buttonFavoriteActiveTexture : _buttonFavoriteInactiveTexture);
            ((UIImageButton)evt.Target).SetVisibility(1f, _data.IsFavorite ? 0.8f : 0.4f);
            if (_data.IsFavorite)
            {
                _buttonLabel.SetText("Unfavorite");
                RemoveChild(_deleteButton);
            }
            else
            {
                _buttonLabel.SetText("Favorite");
                Append(_deleteButton);
            }

            UIList uiList = Parent.Parent as UIList;
            if (uiList == null)
                return;
            uiList.UpdateOrder();
        }

        public override int CompareTo(object obj)
        {
            UIWorldListItem uiWorldListItem = obj as UIWorldListItem;
            if (uiWorldListItem == null)
                return base.CompareTo(obj);
            if (IsFavorite && !uiWorldListItem.IsFavorite)
                return -1;
            if (!IsFavorite && uiWorldListItem.IsFavorite)
                return 1;
            return _data.Name.CompareTo(uiWorldListItem._data.Name);
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
            BackgroundColor = new Color(73, 94, 171);
            BorderColor = new Color(89, 116, 213);
        }

        public override void MouseOut(UIMouseEvent evt)
        {
            base.MouseOut(evt);
            BackgroundColor = new Color(63, 82, 151) * 0.7f;
            BorderColor = new Color(89, 116, 213) * 0.7f;
        }

        private void DrawPanel(SpriteBatch spriteBatch, Vector2 position, float width)
        {
            spriteBatch.Draw(_innerPanelTexture, position, new Rectangle?(new Rectangle(0, 0, 8, _innerPanelTexture.Height)), Color.White);
            spriteBatch.Draw(_innerPanelTexture, new Vector2(position.X + 8f, position.Y), new Rectangle?(new Rectangle(8, 0, 8, _innerPanelTexture.Height)),
                Color.White, 0.0f, Vector2.Zero, new Vector2((float)((width - 16.0) / 8.0), 1f), SpriteEffects.None, 0.0f);
            spriteBatch.Draw(_innerPanelTexture, new Vector2((float)(position.X + width - 8.0), position.Y), new Rectangle?(new Rectangle(16, 0, 8, _innerPanelTexture.Height)), Color.White);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            CalculatedStyle innerDimensions = GetInnerDimensions();
            CalculatedStyle dimensions = _worldIcon.GetDimensions();
            float x1 = dimensions.X + dimensions.Width;
            Color color = _data.IsValid ? Color.White : Color.Red;
            Utils.DrawBorderString(spriteBatch, _data.Name, new Vector2(x1 + 6f, dimensions.Y - 2f), color, 1f, 0.0f, 0.0f, -1);
            spriteBatch.Draw(_dividerTexture, new Vector2(x1, innerDimensions.Y + 21f), new Rectangle?(), Color.White, 0.0f, Vector2.Zero,
                new Vector2((float)((GetDimensions().X + GetDimensions().Width - x1) / 8.0), 1f), SpriteEffects.None, 0.0f);
            Vector2 position = new Vector2(x1 + 6f, innerDimensions.Y + 29f);
            float width1 = 80f;
            DrawPanel(spriteBatch, position, width1);
            string text1 = _data.IsExpertMode ? "Expert" : "Normal";
            float num1 = Main.fontMouseText.MeasureString(text1).X;
            float x2 = (float)(width1 * 0.5 - num1 * 0.5);
            Utils.DrawBorderString(spriteBatch, text1, position + new Vector2(x2, 3f), _data.IsExpertMode ? new Color(217, 143, 244) : Color.White, 1f, 0.0f, 0.0f, -1);
            position.X += width1 + 5f;
            float width2 = 140f;
            DrawPanel(spriteBatch, position, width2);
            string text2 = _data.WorldSizeName + " World";
            float num2 = Main.fontMouseText.MeasureString(text2).X;
            float x3 = (float)(width2 * 0.5 - num2 * 0.5);
            Utils.DrawBorderString(spriteBatch, text2, position + new Vector2(x3, 3f), Color.White, 1f, 0.0f, 0.0f, -1);
            position.X += width2 + 5f;
            float width3 = innerDimensions.X + innerDimensions.Width - position.X;
            DrawPanel(spriteBatch, position, width3);
            string text3 = "Created: " + _data.CreationTime.ToString("d MMMM yyyy");
            float num3 = Main.fontMouseText.MeasureString(text3).X;
            float x4 = (float)(width3 * 0.5 - num3 * 0.5);
            Utils.DrawBorderString(spriteBatch, text3, position + new Vector2(x4, 3f), Color.White, 1f, 0.0f, 0.0f, -1);
            position.X += width3 + 5f;
        }
    }
}
