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
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Dyes
{
    internal class LegacyHairShaderData : HairShaderData
    {
        private LegacyHairShaderData.ColorProcessingMethod _colorProcessor;

        public LegacyHairShaderData()
            : base((Effect)null, (string)null)
        {
            this._shaderDisabled = true;
        }

        public override Color GetColor(Player player, Color lightColor)
        {
            bool lighting = true;
            Color color = this._colorProcessor(player, player.hairColor, ref lighting);
            if (lighting)
                return new Color(color.ToVector4() * lightColor.ToVector4());
            return color;
        }

        public LegacyHairShaderData UseLegacyMethod(LegacyHairShaderData.ColorProcessingMethod colorProcessor)
        {
            this._colorProcessor = colorProcessor;
            return this;
        }

        public delegate Color ColorProcessingMethod(Player player, Color color, ref bool lighting);
    }
}