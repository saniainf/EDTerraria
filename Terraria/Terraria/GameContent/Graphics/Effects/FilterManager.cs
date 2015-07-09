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
            if (this._activeFilters.Contains(effect))
            {
                if (effect.Active)
                    return;
                this._activeFilters.Remove(effect);
            }
            else
                effect.Opacity = 0.0f;
            if (this._activeFilters.Count == 0)
            {
                this._activeFilters.AddLast(effect);
            }
            else
            {
                for (LinkedListNode<Filter> node = this._activeFilters.First; node != null; node = node.Next)
                {
                    Filter filter = node.Value;
                    if (effect.Priority < filter.Priority)
                    {
                        this._activeFilters.AddBefore(node, effect);
                        return;
                    }
                }
                this._activeFilters.AddLast(effect);
            }
        }

        public void Apply()
        {
            LinkedListNode<Filter> node = this._activeFilters.First;
            int num = 0;
            int count = this._activeFilters.Count;
            LinkedListNode<Filter> linkedListNode;
            for (; node != null; node = linkedListNode)
            {
                ++num;
                Filter filter = node.Value;
                linkedListNode = node.Next;
                if ((double)filter.Opacity > 0.0 || num == count)
                {
                    filter.Apply();
                    if (num == count && filter.Active)
                        filter.Opacity = Math.Min(filter.Opacity + 0.05f, 1f);
                    else
                        filter.Opacity = Math.Max(filter.Opacity - 0.05f, 0.0f);
                    linkedListNode = (LinkedListNode<Filter>)null;
                }
                if (!filter.Active && (double)filter.Opacity == 0.0)
                    this._activeFilters.Remove(node);
            }
        }

        public bool HasActiveFilter()
        {
            return this._activeFilters.Count != 0;
        }
    }
}
