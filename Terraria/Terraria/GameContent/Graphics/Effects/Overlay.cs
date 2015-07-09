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

namespace Terraria.Graphics.Effects
{
    internal abstract class Overlay : GameEffect
    {
        public OverlayMode Mode = OverlayMode.Inactive;

        public Overlay(EffectPriority priority)
        {
            this._priority = priority;
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
