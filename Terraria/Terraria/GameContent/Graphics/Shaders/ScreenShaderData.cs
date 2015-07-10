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

namespace Terraria.Graphics.Shaders
{
    public class ScreenShaderData : ShaderData
    {
        private Vector3 _uColor = Vector3.One;
        private Vector3 _uSecondaryColor = Vector3.One;
        private float _uOpacity = 1f;
        private float _globalOpacity = 1f;
        private float _uIntensity = 1f;
        private Vector2 _uTargetPosition = Vector2.One;
        private float _uProgress;

        public ScreenShaderData(string passName)
            : base(Main.screenShader, passName) { }

        public ScreenShaderData(Effect shader, string passName)
            : base(shader, passName) { }

        public override void Apply()
        {
            Vector2 vector2_1 = new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange);
            Vector2 vector2_2 = new Vector2((float)Main.screenWidth, (float)Main.screenHeight);
            _shader.Parameters["uColor"].SetValue(_uColor);
            _shader.Parameters["uOpacity"].SetValue(_uOpacity * _globalOpacity);
            _shader.Parameters["uSecondaryColor"].SetValue(_uSecondaryColor);
            _shader.Parameters["uTime"].SetValue(Main.GlobalTime);
            _shader.Parameters["uScreenResolution"].SetValue(vector2_2);
            _shader.Parameters["uScreenPosition"].SetValue(Main.screenPosition - vector2_1);
            _shader.Parameters["uTargetPosition"].SetValue(_uTargetPosition - vector2_1);
            _shader.Parameters["uIntensity"].SetValue(_uIntensity);
            _shader.Parameters["uProgress"].SetValue(_uProgress);
            base.Apply();
        }

        public ScreenShaderData UseIntensity(float intensity)
        {
            _uIntensity = intensity;
            return this;
        }

        public ScreenShaderData UseColor(float r, float g, float b)
        {
            return UseColor(new Vector3(r, g, b));
        }

        public ScreenShaderData UseProgress(float progress)
        {
            _uProgress = progress;
            return this;
        }

        public ScreenShaderData UseColor(Color color)
        {
            return UseColor(color.ToVector3());
        }

        public ScreenShaderData UseColor(Vector3 color)
        {
            _uColor = color;
            return this;
        }

        public ScreenShaderData UseGlobalOpacity(float opacity)
        {
            _globalOpacity = opacity;
            return this;
        }

        public ScreenShaderData UseTargetPosition(Vector2 position)
        {
            _uTargetPosition = position;
            return this;
        }

        public ScreenShaderData UseSecondaryColor(float r, float g, float b)
        {
            return UseSecondaryColor(new Vector3(r, g, b));
        }

        public ScreenShaderData UseSecondaryColor(Color color)
        {
            return UseSecondaryColor(color.ToVector3());
        }

        public ScreenShaderData UseSecondaryColor(Vector3 color)
        {
            _uSecondaryColor = color;
            return this;
        }

        public ScreenShaderData UseOpacity(float opacity)
        {
            _uOpacity = opacity;
            return this;
        }

        public virtual ScreenShaderData GetSecondaryShader(Player player)
        {
            return this;
        }
    }
}
