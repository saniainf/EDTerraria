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
    internal class UIImageButton : UIElement
    {
        private float _visibilityActive = 1f;
        private float _visibilityInactive = 0.4f;
        private Texture2D _texture;

        public UIImageButton(Texture2D texture)
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
            spriteBatch.Draw(_texture, dimensions.Position(), Color.White * (IsMouseHovering ? _visibilityActive : _visibilityInactive));
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
            Main.PlaySound(12, -1, -1, 1);
        }

        public void SetVisibility(float whenActive, float whenInactive)
        {
            _visibilityActive = MathHelper.Clamp(whenActive, 0.0f, 1f);
            _visibilityInactive = MathHelper.Clamp(whenInactive, 0.0f, 1f);
        }
    }
}