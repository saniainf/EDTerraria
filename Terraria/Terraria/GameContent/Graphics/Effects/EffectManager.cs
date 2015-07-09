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
using System.Collections.Generic;

namespace Terraria.Graphics.Effects
{
    internal abstract class EffectManager<T> where T : GameEffect
    {
        protected Dictionary<string, T> _effects = new Dictionary<string, T>();
        protected bool _isLoaded;

        public bool IsLoaded
        {
            get
            {
                return this._isLoaded;
            }
        }

        public T this[string key]
        {
            get
            {
                T obj;
                if (this._effects.TryGetValue(key, out obj))
                    return obj;
                return default(T);
            }
            set
            {
                this.Bind(key, value);
            }
        }

        public void Bind(string name, T effect)
        {
            this._effects[name] = effect;
            if (!this._isLoaded)
                return;
            effect.Load();
        }

        public void Load()
        {
            if (this._isLoaded)
                return;
            this._isLoaded = true;
            foreach (T obj in this._effects.Values)
                obj.Load();
        }

        public T Activate(string name, Vector2 position = default(Vector2), params object[] args)
        {
            if (!this._effects.ContainsKey(name))
                throw new MissingEffectException("Unable to find effect named: " + (object)name + ". Type: " + (string)(object)typeof(T) + ".");
            T effect = this._effects[name];
            this.OnActivate(effect, position);
            effect.Activate(position, args);
            return effect;
        }

        public void Deactivate(string name, params object[] args)
        {
            if (!this._effects.ContainsKey(name))
                throw new MissingEffectException("Unable to find effect named: " + (object)name + ". Type: " + (string)(object)typeof(T) + ".");
            T effect = this._effects[name];
            this.OnDeactivate(effect);
            effect.Deactivate(args);
        }

        public virtual void OnActivate(T effect, Vector2 position)
        {
        }

        public virtual void OnDeactivate(T effect)
        {
        }
    }
}
