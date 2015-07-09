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
            this._texture = texture;
            this.Width.Set((float)this._texture.Width, 0.0f);
            this.Height.Set((float)this._texture.Height, 0.0f);
        }

        public void SetImage(Texture2D texture)
        {
            this._texture = texture;
            this.Width.Set((float)this._texture.Width, 0.0f);
            this.Height.Set((float)this._texture.Height, 0.0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = this.GetDimensions();
            spriteBatch.Draw(this._texture, dimensions.Position(), Color.White * (this.IsMouseHovering ? this._visibilityActive : this._visibilityInactive));
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
            Main.PlaySound(12, -1, -1, 1);
        }

        public void SetVisibility(float whenActive, float whenInactive)
        {
            this._visibilityActive = MathHelper.Clamp(whenActive, 0.0f, 1f);
            this._visibilityInactive = MathHelper.Clamp(whenInactive, 0.0f, 1f);
        }
    }
}