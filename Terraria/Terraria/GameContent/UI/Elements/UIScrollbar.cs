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
using Terraria.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
    internal class UIScrollbar : UIElement
    {
        private float _viewSize = 1f;
        private float _maxViewSize = 20f;
        private float _viewPosition;
        private bool _isDragging;
        private bool _isHoveringOverHandle;
        private float _dragYOffset;
        private Texture2D _texture;
        private Texture2D _innerTexture;

        public float ViewPosition
        {
            get { return _viewPosition; }
            set { _viewPosition = MathHelper.Clamp(value, 0.0f, _maxViewSize - _viewSize); }
        }

        public UIScrollbar()
        {
            Width.Set(20f, 0.0f);
            MaxWidth.Set(20f, 0.0f);
            _texture = TextureManager.Load("Images/UI/Scrollbar");
            _innerTexture = TextureManager.Load("Images/UI/ScrollbarInner");
            PaddingTop = 5f;
            PaddingBottom = 5f;
        }

        public void SetView(float viewSize, float maxViewSize)
        {
            viewSize = MathHelper.Clamp(viewSize, 0.0f, maxViewSize);
            _viewPosition = MathHelper.Clamp(_viewPosition, 0.0f, maxViewSize - viewSize);
            _viewSize = viewSize;
            _maxViewSize = maxViewSize;
        }

        public float GetValue()
        {
            return _viewPosition;
        }

        private Rectangle GetHandleRectangle()
        {
            CalculatedStyle innerDimensions = GetInnerDimensions();
            if (_maxViewSize == 0.0 && _viewSize == 0.0)
            {
                _viewSize = 1f;
                _maxViewSize = 1f;
            }

            return new Rectangle((int)innerDimensions.X, (int)(innerDimensions.Y + innerDimensions.Height * (_viewPosition / _maxViewSize)) - 3, 20,
                (int)(innerDimensions.Height * (_viewSize / _maxViewSize)) + 7);
        }

        private void DrawBar(SpriteBatch spriteBatch, Texture2D texture, Rectangle dimensions, Color color)
        {
            spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y - 6, dimensions.Width, 6), new Rectangle?(new Rectangle(0, 0, texture.Width, 6)), color);
            spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y, dimensions.Width, dimensions.Height), new Rectangle?(new Rectangle(0, 9, texture.Width, 2)), color);
            spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y + dimensions.Height, dimensions.Width, 6), new Rectangle?(new Rectangle(0, texture.Height - 6, texture.Width, 6)), color);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            CalculatedStyle innerDimensions = GetInnerDimensions();
            if (_isDragging)
                _viewPosition = MathHelper.Clamp((UserInterface.ActiveInstance.MousePosition.Y - innerDimensions.Y - _dragYOffset) / innerDimensions.Height * _maxViewSize, 0.0f,
                    _maxViewSize - _viewSize);
            Rectangle handleRectangle = GetHandleRectangle();
            Vector2 vector2 = UserInterface.ActiveInstance.MousePosition;
            bool flag = _isHoveringOverHandle;
            _isHoveringOverHandle = handleRectangle.Contains(new Point((int)vector2.X, (int)vector2.Y));
            if (!flag && _isHoveringOverHandle && Main.hasFocus)
                Main.PlaySound(12, -1, -1, 1);
            DrawBar(spriteBatch, _texture, dimensions.ToRectangle(), Color.White);
            DrawBar(spriteBatch, _innerTexture, handleRectangle, Color.White * (_isDragging || _isHoveringOverHandle ? 1f : 0.85f));
        }

        public override void MouseDown(UIMouseEvent evt)
        {
            base.MouseDown(evt);
            if (evt.Target != this)
                return;
            Rectangle handleRectangle = GetHandleRectangle();
            if (handleRectangle.Contains(new Point((int)evt.MousePosition.X, (int)evt.MousePosition.Y)))
            {
                _isDragging = true;
                _dragYOffset = evt.MousePosition.Y - (float)handleRectangle.Y;
            }
            else
            {
                CalculatedStyle innerDimensions = GetInnerDimensions();
                _viewPosition = MathHelper.Clamp((UserInterface.ActiveInstance.MousePosition.Y - innerDimensions.Y - (float)(handleRectangle.Height >> 1)) / innerDimensions.Height * _maxViewSize, 0.0f,
                    _maxViewSize - _viewSize);
            }
        }

        public override void MouseUp(UIMouseEvent evt)
        {
            base.MouseUp(evt);
            _isDragging = false;
        }
    }
}
