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
using Terraria.Graphics.Shaders;

namespace Terraria.Graphics.Effects
{
    internal class Filter : GameEffect
    {
        public Vector2 TargetPosition = Vector2.Zero;
        public bool Active;
        private ScreenShaderData _shader;

        public Filter(ScreenShaderData shader, EffectPriority priority = EffectPriority.VeryLow)
        {
            _shader = shader;
            _priority = priority;
        }

        public void Apply()
        {
            _shader.UseGlobalOpacity(Opacity);
            _shader.UseTargetPosition(TargetPosition);
            _shader.Apply();
        }

        public ScreenShaderData GetShader()
        {
            return _shader;
        }

        internal override void Activate(Vector2 position, params object[] args)
        {
            TargetPosition = position;
            Active = true;
        }

        internal override void Deactivate(params object[] args)
        {
            Active = false;
        }

        public bool IsActive()
        {
            return Active;
        }
    }
}
