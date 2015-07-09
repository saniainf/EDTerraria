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
using Terraria.Graphics.Shaders;

namespace Terraria.Graphics.Effects
{
    internal class SimpleOverlay : Overlay
    {
        public Vector2 TargetPosition = Vector2.Zero;
        private Ref<Texture2D> _texture;
        private ScreenShaderData _shader;

        public SimpleOverlay(string textureName, ScreenShaderData shader, EffectPriority priority = EffectPriority.VeryLow)
            : base(priority)
        {
            this._texture = TextureManager.Retrieve(textureName == null ? "" : textureName);
            this._shader = shader;
        }

        public SimpleOverlay(string textureName, string shaderName = "Default", EffectPriority priority = EffectPriority.VeryLow)
            : base(priority)
        {
            this._texture = TextureManager.Retrieve(textureName == null ? "" : textureName);
            this._shader = new ScreenShaderData(Main.screenShader, shaderName);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this._shader.UseGlobalOpacity(this.Opacity);
            this._shader.UseTargetPosition(this.TargetPosition);
            this._shader.Apply();
            spriteBatch.Draw(this._texture.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
        }

        internal override void Activate(Vector2 position, params object[] args)
        {
            this.TargetPosition = position;
            this.Mode = OverlayMode.FadeIn;
        }

        internal override void Deactivate(params object[] args)
        {
            this.Mode = OverlayMode.FadeOut;
        }
    }
}
