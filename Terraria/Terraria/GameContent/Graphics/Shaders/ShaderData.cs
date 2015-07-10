/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics.Shaders
{
    public class ShaderData
    {
        protected Effect _shader;
        protected string _passName;
        protected EffectPass _effectPass;

        public ShaderData(Effect shader, string passName)
        {
            _passName = passName;
            _shader = shader;
            if (shader == null || passName == null)
                return;

            _effectPass = shader.CurrentTechnique.Passes[passName];
        }

        public void SwapProgram(string passName)
        {
            _passName = passName;
            if (passName == null)
                return;

            _effectPass = _shader.CurrentTechnique.Passes[passName];
        }

        public virtual void Apply()
        {
            _effectPass.Apply();
        }
    }
}
