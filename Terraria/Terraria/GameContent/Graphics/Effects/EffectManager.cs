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
            get { return _isLoaded; }
        }

        public T this[string key]
        {
            get
            {
                T obj;
                if (_effects.TryGetValue(key, out obj))
                    return obj;

                return default(T);
            }
            set { Bind(key, value); }
        }

        public void Bind(string name, T effect)
        {
            _effects[name] = effect;
            if (!_isLoaded)
                return;

            effect.Load();
        }

        public void Load()
        {
            if (_isLoaded)
                return;

            _isLoaded = true;
            foreach (T obj in _effects.Values)
                obj.Load();
        }

        public T Activate(string name, Vector2 position = default(Vector2), params object[] args)
        {
            if (!_effects.ContainsKey(name))
                throw new MissingEffectException("Unable to find effect named: " + name + ". Type: " + typeof(T) + ".");
            
            T effect = _effects[name];
            OnActivate(effect, position);
            effect.Activate(position, args);
            return effect;
        }

        public void Deactivate(string name, params object[] args)
        {
            if (!_effects.ContainsKey(name))
                throw new MissingEffectException("Unable to find effect named: " + name + ". Type: " + typeof(T) + ".");
            
            T effect = _effects[name];
            OnDeactivate(effect);
            effect.Deactivate(args);
        }

        public virtual void OnActivate(T effect, Vector2 position) { }

        public virtual void OnDeactivate(T effect) { }
    }
}
