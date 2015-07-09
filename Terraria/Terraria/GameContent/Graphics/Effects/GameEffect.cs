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

namespace Terraria.Graphics.Effects
{
    internal abstract class GameEffect
    {
        public float Opacity;
        protected bool _isLoaded;
        protected EffectPriority _priority;

        public bool IsLoaded
        {
            get
            {
                return this._isLoaded;
            }
        }

        public EffectPriority Priority
        {
            get
            {
                return this._priority;
            }
        }

        public void Load()
        {
            if (this._isLoaded)
                return;
            this._isLoaded = true;
            this.OnLoad();
        }

        public virtual void OnLoad() { }

        internal abstract void Activate(Vector2 position, params object[] args);

        internal abstract void Deactivate(params object[] args);
    }
}
