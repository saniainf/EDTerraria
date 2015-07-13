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
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
    internal class UITextPanel : UIPanel
    {
        private string _text = "";
        private float _textScale = 1f;
        private Vector2 _textSize = Vector2.Zero;
        private bool _isLarge;

        public UITextPanel(string text, float textScale = 1f, bool large = false)
        {
            SetText(text, textScale, large);
        }

        public override void Recalculate()
        {
            SetText(_text, _textScale, _isLarge);
            base.Recalculate();
        }

        public void SetText(string text, float textScale, bool large)
        {
            Vector2 vector2 = new Vector2((large ? Main.fontDeathText : Main.fontMouseText).MeasureString(text).X, large ? 32f : 16f) * textScale;
            _text = text;
            _textScale = textScale;
            _textSize = vector2;
            _isLarge = large;
            MinWidth.Set(vector2.X + PaddingLeft + PaddingRight, 0.0f);
            MinHeight.Set(vector2.Y + PaddingTop + PaddingBottom, 0.0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            CalculatedStyle innerDimensions = GetInnerDimensions();
            Vector2 pos = innerDimensions.Position();
            if (_isLarge)
                pos.Y -= 10f * _textScale;
            else
                pos.Y -= 2f * _textScale;
            pos.X += (float)((innerDimensions.Width - _textSize.X) * 0.5);
            if (_isLarge)
                Utils.DrawBorderStringBig(spriteBatch, _text, pos, Color.White, _textScale, 0.0f, 0.0f, -1);
            else
                Utils.DrawBorderString(spriteBatch, _text, pos, Color.White, _textScale, 0.0f, 0.0f, -1);
        }
    }
}
