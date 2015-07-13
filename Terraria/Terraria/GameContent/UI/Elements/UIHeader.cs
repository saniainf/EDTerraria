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
            get { return _text; }
            set
            {
                if (!(_text != value))
                    return;
                _text = value;
                Vector2 vector2 = Main.fontDeathText.MeasureString(Text);
                Width.Pixels = vector2.X;
                Height.Pixels = vector2.Y;
                Width.Precent = 0.0f;
                Height.Precent = 0.0f;
                Recalculate();
            }
        }

        public UIHeader()
        {
            Text = "";
        }

        public UIHeader(string text)
        {
            Text = text;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            spriteBatch.DrawString(Main.fontDeathText, Text, new Vector2(dimensions.X, dimensions.Y), Color.White);
        }
    }
}
