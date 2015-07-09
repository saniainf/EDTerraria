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
            this._passName = passName;
            this._shader = shader;
            if (shader == null || passName == null)
                return;
            this._effectPass = shader.CurrentTechnique.Passes[passName];
        }

        public void SwapProgram(string passName)
        {
            this._passName = passName;
            if (passName == null)
                return;
            this._effectPass = this._shader.CurrentTechnique.Passes[passName];
        }

        public virtual void Apply()
        {
            this._effectPass.Apply();
        }
    }
}
