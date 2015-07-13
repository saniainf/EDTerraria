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
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
    internal class UIList : UIElement
    {
        protected List<UIElement> _items = new List<UIElement>();
        private UIElement _innerList = new UIInnerList();
        public float ListPadding = 5f;
        protected UIScrollbar _scrollbar;
        private float _innerListHeight;

        public int Count
        {
            get { return _items.Count; }
        }

        public UIList()
        {
            _innerList.OverflowHidden = false;
            _innerList.Width.Set(0.0f, 1f);
            _innerList.Height.Set(0.0f, 1f);
            OverflowHidden = true;
            Append(_innerList);
        }

        public void Goto(ElementSearchMethod searchMethod)
        {
            for (int index = 0; index < _items.Count; ++index)
            {
                if (searchMethod(_items[index]))
                {
                    _scrollbar.ViewPosition = _items[index].Top.Pixels;
                    break;
                }
            }
        }

        public virtual void Add(UIElement item)
        {
            _items.Add(item);
            _innerList.Append(item);
            UpdateOrder();
            _innerList.Recalculate();
        }

        public virtual bool Remove(UIElement item)
        {
            _innerList.RemoveChild(item);
            UpdateOrder();
            return _items.Remove(item);
        }

        public virtual void Clear()
        {
            _innerList.RemoveAllChildren();
            _items.Clear();
        }

        public override void Recalculate()
        {
            base.Recalculate();
            UpdateScrollbar();
        }

        public override void ScrollWheel(UIScrollWheelEvent evt)
        {
            base.ScrollWheel(evt);
            if (_scrollbar == null)
                return;
            _scrollbar.ViewPosition -= (float)evt.ScrollWheelValue;
        }

        public override void RecalculateChildren()
        {
            base.RecalculateChildren();
            float pixels = 0.0f;
            for (int index = 0; index < _items.Count; ++index)
            {
                this._items[index].Top.Set(pixels, 0.0f);
                this._items[index].Recalculate();
                CalculatedStyle dimensions = _items[index].GetDimensions();
                pixels += dimensions.Height + ListPadding;
            }
            _innerListHeight = pixels;
        }

        private void UpdateScrollbar()
        {
            if (_scrollbar == null)
                return;
            _scrollbar.SetView(GetInnerDimensions().Height, _innerListHeight);
        }

        public void SetScrollbar(UIScrollbar scrollbar)
        {
            _scrollbar = scrollbar;
            UpdateScrollbar();
        }

        public void UpdateOrder()
        {
            _items.Sort(new Comparison<UIElement>(SortMethod));
            UpdateScrollbar();
        }

        public int SortMethod(UIElement item1, UIElement item2)
        {
            return item1.CompareTo(item2);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (_scrollbar != null)
                _innerList.Top.Set(-_scrollbar.GetValue(), 0.0f);
            Recalculate();
        }

        public delegate bool ElementSearchMethod(UIElement element);

        private class UIInnerList : UIElement
        {
            public override bool ContainsPoint(Vector2 point)
            {
                return true;
            }

            protected override void DrawChildren(SpriteBatch spriteBatch)
            {
                Vector2 position1 = Parent.GetDimensions().Position();
                Vector2 dimensions1 = new Vector2(Parent.GetDimensions().Width, Parent.GetDimensions().Height);
                foreach (UIElement uiElement in this.Elements)
                {
                    Vector2 position2 = uiElement.GetDimensions().Position();
                    Vector2 dimensions2 = new Vector2(uiElement.GetDimensions().Width, uiElement.GetDimensions().Height);
                    if (Collision.CheckAABBvAABBCollision(position1, dimensions1, position2, dimensions2))
                        uiElement.Draw(spriteBatch);
                }
            }
        }
    }
}
