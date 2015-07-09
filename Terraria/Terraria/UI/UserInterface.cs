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
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Terraria;

namespace Terraria.UI
{
    public class UserInterface
    {
        public static UserInterface ActiveInstance = new UserInterface();
        private List<UIState> _history = new List<UIState>();
        private const double DOUBLE_CLICK_TIME = 500.0;
        private const double STATE_CHANGE_CLICK_DISABLE_TIME = 200.0;
        private const int MAX_HISTORY_SIZE = 32;
        private const int HISTORY_PRUNE_SIZE = 4;
        public Vector2 MousePosition;
        private bool _wasMouseDown;
        private UIElement _lastElementHover;
        private UIElement _lastElementDown;
        private UIElement _lastElementClicked;
        private double _lastMouseDownTime;
        private int _scrollWheelState;
        private double _clickDisabledTimeRemaining;
        public bool IsVisible;
        private UIState _currentState;

        public UIState CurrentState
        {
            get
            {
                return this._currentState;
            }
        }

        public UserInterface()
        {
            UserInterface.ActiveInstance = this;
        }

        public void Use()
        {
            if (UserInterface.ActiveInstance != this)
            {
                UserInterface.ActiveInstance = this;
                this.Recalculate();
            }
            else
                UserInterface.ActiveInstance = this;
        }

        private void ResetState()
        {
            MouseState state = Mouse.GetState();
            this._scrollWheelState = state.ScrollWheelValue;
            this.MousePosition = new Vector2((float)state.X, (float)state.Y);
            this._wasMouseDown = state.LeftButton == ButtonState.Pressed;
            if (this._lastElementHover != null)
                this._lastElementHover.MouseOut(new UIMouseEvent(this._lastElementHover, this.MousePosition));
            this._lastElementHover = (UIElement)null;
            this._lastElementDown = (UIElement)null;
            this._lastElementClicked = (UIElement)null;
            this._lastMouseDownTime = 0.0;
            this._clickDisabledTimeRemaining = Math.Max(this._clickDisabledTimeRemaining, 200.0);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime time)
        {
            this.Use();
            if (this._currentState == null)
                return;
            MouseState state = Mouse.GetState();
            this.MousePosition = new Vector2((float)state.X, (float)state.Y);
            bool flag1 = state.LeftButton == ButtonState.Pressed && Main.hasFocus;
            UIElement target1 = Main.hasFocus ? this._currentState.GetElementAt(this.MousePosition) : (UIElement)null;
            this._clickDisabledTimeRemaining = Math.Max(0.0, this._clickDisabledTimeRemaining - time.ElapsedGameTime.TotalMilliseconds);
            bool flag2 = this._clickDisabledTimeRemaining > 0.0;
            if (target1 != this._lastElementHover)
            {
                if (this._lastElementHover != null)
                    this._lastElementHover.MouseOut(new UIMouseEvent(this._lastElementHover, this.MousePosition));
                if (target1 != null)
                    target1.MouseOver(new UIMouseEvent(target1, this.MousePosition));
                this._lastElementHover = target1;
            }
            if (flag1 && !this._wasMouseDown && (target1 != null && !flag2))
            {
                this._lastElementDown = target1;
                target1.MouseDown(new UIMouseEvent(target1, this.MousePosition));
                if (this._lastElementClicked == target1 && time.TotalGameTime.TotalMilliseconds - this._lastMouseDownTime < 500.0)
                {
                    target1.DoubleClick(new UIMouseEvent(target1, this.MousePosition));
                    this._lastElementClicked = (UIElement)null;
                }
                this._lastMouseDownTime = time.TotalGameTime.TotalMilliseconds;
            }
            else if (!flag1 && this._wasMouseDown && (this._lastElementDown != null && !flag2))
            {
                UIElement target2 = this._lastElementDown;
                if (target2.ContainsPoint(this.MousePosition))
                {
                    target2.Click(new UIMouseEvent(target2, this.MousePosition));
                    this._lastElementClicked = this._lastElementDown;
                }
                target2.MouseUp(new UIMouseEvent(target2, this.MousePosition));
                this._lastElementDown = (UIElement)null;
            }
            if (state.ScrollWheelValue != this._scrollWheelState)
            {
                if (target1 != null)
                    target1.ScrollWheel(new UIScrollWheelEvent(target1, this.MousePosition, state.ScrollWheelValue - this._scrollWheelState));
                this._scrollWheelState = state.ScrollWheelValue;
            }
            this._wasMouseDown = flag1;
            if (this._currentState == null)
                return;
            this._currentState.Draw(spriteBatch);
        }

        public void SetState(UIState state)
        {
            this.AddToHistory(state);
            if (this._currentState != null)
                this._currentState.Deactivate();
            this._currentState = state;
            this.ResetState();
            if (state == null)
                return;
            state.Activate();
            state.Recalculate();
        }

        public void GoBack()
        {
            if (this._history.Count < 2)
                return;
            UIState state = this._history[this._history.Count - 2];
            this._history.RemoveRange(this._history.Count - 2, 2);
            this.SetState(state);
        }

        private void AddToHistory(UIState state)
        {
            this._history.Add(state);
            if (this._history.Count <= 32)
                return;
            this._history.RemoveRange(0, 4);
        }

        public void Recalculate()
        {
            this._scrollWheelState = Mouse.GetState().ScrollWheelValue;
            if (this._currentState == null)
                return;
            this._currentState.Recalculate();
        }

        public CalculatedStyle GetDimensions()
        {
            return new CalculatedStyle(0.0f, 0.0f, (float)Main.screenWidth, (float)Main.screenHeight);
        }

        internal void RefreshState()
        {
            if (this._currentState != null)
                this._currentState.Deactivate();
            this.ResetState();
            this._currentState.Activate();
            this._currentState.Recalculate();
        }

        public bool IsElementUnderMouse()
        {
            if (this.IsVisible && this._lastElementHover != null)
                return !(this._lastElementHover is UIState);
            return false;
        }
    }
}
