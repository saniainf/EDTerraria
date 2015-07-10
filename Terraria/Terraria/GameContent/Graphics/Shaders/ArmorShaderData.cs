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
using Terraria.DataStructures;
using Terraria.Graphics;

namespace Terraria.Graphics.Shaders
{
    public class ArmorShaderData : ShaderData
    {
        private Vector3 _uColor = Vector3.One;
        private Vector3 _uSecondaryColor = Vector3.One;
        private float _uSaturation = 1f;
        private float _uOpacity = 1f;
        private Ref<Texture2D> _uImage;

        public ArmorShaderData(Effect shader, string passName)
            : base(shader, passName) { }

        public virtual void Apply(Entity entity, DrawData? drawData = null)
        {
            _shader.Parameters["uColor"].SetValue(_uColor);
            _shader.Parameters["uSaturation"].SetValue(_uSaturation);
            _shader.Parameters["uSecondaryColor"].SetValue(_uSecondaryColor);
            _shader.Parameters["uTime"].SetValue(Main.GlobalTime);
            _shader.Parameters["uOpacity"].SetValue(_uOpacity);
            if (drawData.HasValue)
            {
                DrawData drawData1 = drawData.Value;
                _shader.Parameters["uSourceRect"].SetValue(!drawData1.sourceRect.HasValue ? new Vector4(0.0f, 0.0f, (float)drawData1.texture.Width,
                    (float)drawData1.texture.Height) : new Vector4((float)drawData1.sourceRect.Value.X, (float)drawData1.sourceRect.Value.Y,
                        (float)drawData1.sourceRect.Value.Width, (float)drawData1.sourceRect.Value.Height));
                _shader.Parameters["uWorldPosition"].SetValue(Main.screenPosition + drawData1.position);
                _shader.Parameters["uImageSize0"].SetValue(new Vector2((float)drawData1.texture.Width, (float)drawData1.texture.Height));
                _shader.Parameters["uRotation"].SetValue(drawData1.rotation * (drawData1.effect.HasFlag(SpriteEffects.FlipHorizontally) ? -1f : 1f));
                _shader.Parameters["uDirection"].SetValue(drawData1.effect.HasFlag(SpriteEffects.FlipHorizontally) ? -1 : 1);
            }
            else
            {
                _shader.Parameters["uSourceRect"].SetValue(new Vector4(0.0f, 0.0f, 4f, 4f));
                _shader.Parameters["uRotation"].SetValue(0.0f);
            }

            if (_uImage != null)
            {
                Main.graphics.GraphicsDevice.Textures[1] = _uImage.Value;
                _shader.Parameters["uImageSize1"].SetValue(new Vector2((float)_uImage.Value.Width, (float)_uImage.Value.Height));
            }

            if (entity != null)
                _shader.Parameters["uDirection"].SetValue((float)entity.direction);
            Apply();
        }

        public ArmorShaderData UseColor(float r, float g, float b)
        {
            return UseColor(new Vector3(r, g, b));
        }

        public ArmorShaderData UseColor(Color color)
        {
            return UseColor(color.ToVector3());
        }

        public ArmorShaderData UseColor(Vector3 color)
        {
            _uColor = color;
            return this;
        }

        public ArmorShaderData UseImage(string path)
        {
            _uImage = TextureManager.Retrieve(path);
            return this;
        }

        public ArmorShaderData UseOpacity(float alpha)
        {
            _uOpacity = alpha;
            return this;
        }

        public ArmorShaderData UseSecondaryColor(float r, float g, float b)
        {
            return UseSecondaryColor(new Vector3(r, g, b));
        }

        public ArmorShaderData UseSecondaryColor(Color color)
        {
            return UseSecondaryColor(color.ToVector3());
        }

        public ArmorShaderData UseSecondaryColor(Vector3 color)
        {
            _uSecondaryColor = color;
            return this;
        }

        public ArmorShaderData UseSaturation(float saturation)
        {
            _uSaturation = saturation;
            return this;
        }

        public virtual ArmorShaderData GetSecondaryShader(Entity entity)
        {
            return this;
        }
    }
}
