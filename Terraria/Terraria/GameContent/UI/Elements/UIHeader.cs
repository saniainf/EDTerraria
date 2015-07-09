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
    internal class UIHeader : UIElement
    {
        private string _text;

        public string Text
        {
            get
            {
                return this._text;
            }
            set
            {
                if (!(this._text != value))
                    return;
                this._text = value;
                Vector2 vector2 = Main.fontDeathText.MeasureString(this.Text);
                this.Width.Pixels = vector2.X;
                this.Height.Pixels = vector2.Y;
                this.Width.Precent = 0.0f;
                this.Height.Precent = 0.0f;
                this.Recalculate();
            }
        }

        public UIHeader()
        {
            this.Text = "";
        }

        public UIHeader(string text)
        {
            this.Text = text;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = this.GetDimensions();
            spriteBatch.DrawString(Main.fontDeathText, this.Text, new Vector2(dimensions.X, dimensions.Y), Color.White);
        }
    }
}
