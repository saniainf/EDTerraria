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
    internal class UIText : UIElement
    {
        private string _text = "";
        private float _textScale = 1f;
        private Vector2 _textSize = Vector2.Zero;
        private bool _isLarge;

        public UIText(string text, float textScale = 1f, bool large = false)
        {
            this.SetText(text, textScale, large);
        }

        public override void Recalculate()
        {
            this.SetText(this._text, this._textScale, this._isLarge);
            base.Recalculate();
        }

        public void SetText(string text)
        {
            this.SetText(text, this._textScale, this._isLarge);
        }

        public void SetText(string text, float textScale, bool large)
        {
            Vector2 vector2 = new Vector2((large ? Main.fontDeathText : Main.fontMouseText).MeasureString(text).X, large ? 32f : 16f) * textScale;
            this._text = text;
            this._textScale = textScale;
            this._textSize = vector2;
            this._isLarge = large;
            this.MinWidth.Set(vector2.X + this.PaddingLeft + this.PaddingRight, 0.0f);
            this.MinHeight.Set(vector2.Y + this.PaddingTop + this.PaddingBottom, 0.0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            CalculatedStyle innerDimensions = this.GetInnerDimensions();
            Vector2 pos = innerDimensions.Position();
            if (this._isLarge)
                pos.Y -= 10f * this._textScale;
            else
                pos.Y -= 2f * this._textScale;
            pos.X += (float)(((double)innerDimensions.Width - (double)this._textSize.X) * 0.5);
            if (this._isLarge)
                Utils.DrawBorderStringBig(spriteBatch, this._text, pos, Color.White, this._textScale, 0.0f, 0.0f, -1);
            else
                Utils.DrawBorderString(spriteBatch, this._text, pos, Color.White, this._textScale, 0.0f, 0.0f, -1);
        }
    }
}
