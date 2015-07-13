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
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
    internal class UIImage : UIElement
    {
        private Texture2D _texture;

        public UIImage(Texture2D texture)
        {
            _texture = texture;
            Width.Set((float)_texture.Width, 0.0f);
            Height.Set((float)_texture.Height, 0.0f);
        }

        public void SetImage(Texture2D texture)
        {
            _texture = texture;
            Width.Set((float)_texture.Width, 0.0f);
            Height.Set((float)_texture.Height, 0.0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            spriteBatch.Draw(_texture, dimensions.Position(), Color.White);
        }
    }
}
