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
using Terraria.Graphics;
using Terraria.IO;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
    internal class UICharacterListItem : UIPanel
    {
        private PlayerFileData playerFileData;
        private Texture2D _dividerTexture;
        private Texture2D _innerPanelTexture;
        private UICharacter _playerPanel;
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
            get { return playerFileData.IsFavorite; }
        }

        public UICharacterListItem(PlayerFileData data)
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
            playerFileData = data;
            _playerPanel = new UICharacter(data.Player);
            _playerPanel.Left.Set(4f, 0.0f);
            _playerPanel.OnDoubleClick += new UIElement.MouseEvent(PlayGame);
            OnDoubleClick += new UIElement.MouseEvent(PlayGame);
            Append(_playerPanel);
            UIImageButton uiImageButton1 = new UIImageButton(_buttonPlayTexture);
            uiImageButton1.VAlign = 1f;
            uiImageButton1.Left.Set(4f, 0.0f);
            uiImageButton1.OnClick += new UIElement.MouseEvent(PlayGame);
            uiImageButton1.OnMouseOver += new UIElement.MouseEvent(PlayMouseOver);
            uiImageButton1.OnMouseOut += new UIElement.MouseEvent(ButtonMouseOut);
            Append(uiImageButton1);
            UIImageButton uiImageButton2 = new UIImageButton(playerFileData.IsFavorite ? _buttonFavoriteActiveTexture : _buttonFavoriteInactiveTexture);
            uiImageButton2.VAlign = 1f;
            uiImageButton2.Left.Set(28f, 0.0f);
            uiImageButton2.OnClick += new UIElement.MouseEvent(FavoriteButtonClick);
            uiImageButton2.OnMouseOver += new UIElement.MouseEvent(FavoriteMouseOver);
            uiImageButton2.OnMouseOut += new UIElement.MouseEvent(ButtonMouseOut);
            uiImageButton2.SetVisibility(1f, playerFileData.IsFavorite ? 0.8f : 0.4f);
            Append(uiImageButton2);
            UIImageButton uiImageButton4 = new UIImageButton(_buttonDeleteTexture);
            uiImageButton4.VAlign = 1f;
            uiImageButton4.HAlign = 1f;
            uiImageButton4.OnClick += new UIElement.MouseEvent(DeleteButtonClick);
            uiImageButton4.OnMouseOver += new UIElement.MouseEvent(DeleteMouseOver);
            uiImageButton4.OnMouseOut += new UIElement.MouseEvent(DeleteMouseOut);
            _deleteButton = uiImageButton4;
            if (!playerFileData.IsFavorite)
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

        private void FavoriteMouseOver(UIMouseEvent evt, UIElement listeningElement)
        {
            if (playerFileData.IsFavorite)
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
            for (int index = 0; index < Main.PlayerList.Count; ++index)
            {
                if (Main.PlayerList[index] == playerFileData)
                {
                    Main.PlaySound(10, -1, -1, 1);
                    Main.selectedPlayer = index;
                    Main.menuMode = 5;
                    break;
                }
            }
        }

        private void PlayGame(UIMouseEvent evt, UIElement listeningElement)
        {
            if (listeningElement != evt.Target || playerFileData.Player.loadStatus != 0)
                return;

            Main.SelectPlayer(playerFileData);
        }

        private void FavoriteButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            playerFileData.ToggleFavorite();
            ((UIImageButton)evt.Target).SetImage(playerFileData.IsFavorite ? _buttonFavoriteActiveTexture : _buttonFavoriteInactiveTexture);
            ((UIImageButton)evt.Target).SetVisibility(1f, playerFileData.IsFavorite ? 0.8f : 0.4f);
            if (playerFileData.IsFavorite)
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
            UICharacterListItem characterListItem = obj as UICharacterListItem;
            if (characterListItem == null)
                return base.CompareTo(obj);
            if (IsFavorite && !characterListItem.IsFavorite)
                return -1;
            if (!IsFavorite && characterListItem.IsFavorite)
                return 1;
            return playerFileData.Name.CompareTo(characterListItem.playerFileData.Name);
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
            CalculatedStyle dimensions = _playerPanel.GetDimensions();
            float x = dimensions.X + dimensions.Width;
            Utils.DrawBorderString(spriteBatch, playerFileData.Name, new Vector2(x + 6f, dimensions.Y - 2f), Color.White, 1f, 0.0f, 0.0f, -1);
            spriteBatch.Draw(_dividerTexture, new Vector2(x, innerDimensions.Y + 21f), new Rectangle?(), Color.White, 0.0f, Vector2.Zero,
                new Vector2((float)((GetDimensions().X + GetDimensions().Width - x) / 8.0), 1f), SpriteEffects.None, 0.0f);
            Vector2 vector2 = new Vector2(x + 6f, innerDimensions.Y + 29f);
            float width1 = 200f;
            Vector2 position1 = vector2;
            this.DrawPanel(spriteBatch, position1, width1);
            spriteBatch.Draw(Main.heartTexture, position1 + new Vector2(5f, 2f), Color.White);
            position1.X += 10f + (float)Main.heartTexture.Width;
            Utils.DrawBorderString(spriteBatch, playerFileData.Player.statLifeMax + " HP", position1 + new Vector2(0.0f, 3f), Color.White, 1f, 0.0f, 0.0f, -1);
            position1.X += 65f;
            spriteBatch.Draw(Main.manaTexture, position1 + new Vector2(5f, 2f), Color.White);
            position1.X += 10f + (float)Main.manaTexture.Width;
            Utils.DrawBorderString(spriteBatch, playerFileData.Player.statManaMax + " MP", position1 + new Vector2(0.0f, 3f), Color.White, 1f, 0.0f, 0.0f, -1);
            vector2.X += width1 + 5f;
            Vector2 position2 = vector2;
            float width2 = 110f;
            DrawPanel(spriteBatch, position2, width2);
            string text1 = "";
            Color color = Color.White;
            switch (playerFileData.Player.difficulty)
            {
                case 0:
                    text1 = "Softcore";
                    break;
                case 1:
                    text1 = "Mediumcore";
                    color = Main.mcColor;
                    break;
                case 2:
                    text1 = "Hardcore";
                    color = Main.hcColor;
                    break;
            }
            Vector2 pos1 = position2 + new Vector2((float)(width2 * 0.5 - Main.fontMouseText.MeasureString(text1).X * 0.5), 3f);
            Utils.DrawBorderString(spriteBatch, text1, pos1, color, 1f, 0.0f, 0.0f, -1);
            vector2.X += width2 + 5f;
            Vector2 position3 = vector2;
            float width3 = innerDimensions.X + innerDimensions.Width - position3.X;
            DrawPanel(spriteBatch, position3, width3);
            TimeSpan playTime = playerFileData.GetPlayTime();
            int num = playTime.Days * 24 + playTime.Hours;
            string text2 = (num < 10 ? "0" : "") + num + playTime.ToString("\\:mm\\:ss");
            Vector2 pos2 = position3 + new Vector2((float)(width3 * 0.5 - Main.fontMouseText.MeasureString(text2).X * 0.5), 3f);
            Utils.DrawBorderString(spriteBatch, text2, pos2, Color.White, 1f, 0.0f, 0.0f, -1);
        }
    }
}
