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
using System;
using System.Collections.Generic;

namespace Terraria.Graphics.Effects
{
    internal class FilterManager : EffectManager<Filter>
    {
        private LinkedList<Filter> _activeFilters = new LinkedList<Filter>();
        private const float OPACITY_RATE = 0.05f;

        public override void OnActivate(Filter effect, Vector2 position)
        {
            if (_activeFilters.Contains(effect))
            {
                if (effect.Active)
                    return;

                _activeFilters.Remove(effect);
            }
            else
                effect.Opacity = 0.0f;

            if (_activeFilters.Count == 0)
                _activeFilters.AddLast(effect);
            else
            {
                for (LinkedListNode<Filter> node = _activeFilters.First; node != null; node = node.Next)
                {
                    Filter filter = node.Value;
                    if (effect.Priority < filter.Priority)
                    {
                        _activeFilters.AddBefore(node, effect);
                        return;
                    }
                }
                _activeFilters.AddLast(effect);
            }
        }

        public void Apply()
        {
            LinkedListNode<Filter> node = _activeFilters.First;
            int num = 0;
            int count = _activeFilters.Count;
            LinkedListNode<Filter> linkedListNode;
            for (; node != null; node = linkedListNode)
            {
                ++num;
                Filter filter = node.Value;
                linkedListNode = node.Next;
                if (filter.Opacity > 0.0 || num == count)
                {
                    filter.Apply();
                    if (num == count && filter.Active)
                        filter.Opacity = Math.Min(filter.Opacity + 0.05f, 1f);
                    else
                        filter.Opacity = Math.Max(filter.Opacity - 0.05f, 0.0f);
                    linkedListNode = null;
                }

                if (!filter.Active && filter.Opacity == 0.0)
                    _activeFilters.Remove(node);
            }
        }

        public bool HasActiveFilter()
        {
            return _activeFilters.Count != 0;
        }
    }
}
