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
using System.Collections.Generic;

namespace Terraria.Graphics.Effects
{
    internal class SkyManager : EffectManager<CustomSky>
    {
        public static SkyManager Instance = new SkyManager();
        private LinkedList<CustomSky> _activeSkies = new LinkedList<CustomSky>();
        private float _lastDepth;

        public void Reset()
        {
            foreach (CustomSky customSky in _effects.Values)
                customSky.Reset();
            _activeSkies.Clear();
        }

        public void Update()
        {
            LinkedListNode<CustomSky> next;
            for (LinkedListNode<CustomSky> node = _activeSkies.First; node != null; node = next)
            {
                CustomSky customSky = node.Value;
                next = node.Next;
                customSky.Update();
                if (!customSky.IsActive())
                    _activeSkies.Remove(node);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawDepthRange(spriteBatch, float.MinValue, float.MaxValue);
        }

        public void DrawToDepth(SpriteBatch spriteBatch, float minDepth)
        {
            if (_lastDepth <= minDepth)
                return;

            DrawDepthRange(spriteBatch, minDepth, _lastDepth);
            _lastDepth = minDepth;
        }

        public void DrawDepthRange(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            foreach (CustomSky customSky in _activeSkies)
                customSky.Draw(spriteBatch, minDepth, maxDepth);
        }

        public void DrawRemainingDepth(SpriteBatch spriteBatch)
        {
            DrawDepthRange(spriteBatch, float.MinValue, _lastDepth);
            _lastDepth = float.MinValue;
        }

        public void ResetDepthTracker()
        {
            _lastDepth = float.MaxValue;
        }

        public void SetStartingDepth(float depth)
        {
            _lastDepth = depth;
        }

        public override void OnActivate(CustomSky effect, Vector2 position)
        {
            _activeSkies.AddLast(effect);
        }

        public Color ProcessTileColor(Color color)
        {
            foreach (CustomSky customSky in this._activeSkies)
                color = customSky.OnTileColor(color);

            return color;
        }

        public float ProcessCloudAlpha()
        {
            float num = 1f;
            foreach (CustomSky customSky in _activeSkies)
                num *= customSky.GetCloudAlpha();

            return MathHelper.Clamp(num, 0.0f, 1f);
        }
    }
}