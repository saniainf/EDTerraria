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
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
    internal class UIProgressBar : UIElement
    {
        private UIInnerProgressBar _progressBar = new UIInnerProgressBar();
        private float _visualProgress;
        private float _targetProgress;

        public UIProgressBar()
        {
            _progressBar.Height.Precent = 1f;
            _progressBar.Recalculate();
            Append(_progressBar);
        }

        public void SetProgress(float value)
        {
            _targetProgress = value;
            if (value >= _visualProgress)
                return;
            _visualProgress = value;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            _visualProgress = (float)(_visualProgress * 0.949999988079071 + 0.0500000007450581 * _targetProgress);
            _progressBar.Width.Precent = _visualProgress;
            _progressBar.Recalculate();
        }

        private class UIInnerProgressBar : UIElement
        {
            protected override void DrawSelf(SpriteBatch spriteBatch)
            {
                CalculatedStyle dimensions = GetDimensions();
                spriteBatch.Draw(Main.magicPixel, new Vector2(dimensions.X, dimensions.Y), new Rectangle?(), Color.Blue, 0.0f, Vector2.Zero,
                    new Vector2(dimensions.Width, dimensions.Height / 1000f), SpriteEffects.None, 0.0f);
            }
        }
    }
}
