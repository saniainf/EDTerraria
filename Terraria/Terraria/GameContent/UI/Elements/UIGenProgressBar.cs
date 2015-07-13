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
    internal class UIGenProgressBar : UIElement
    {
        private Texture2D _texInnerDirt;
        private Texture2D _texOuterCrimson;
        private Texture2D _texOuterCorrupt;
        private Texture2D _texOuterLower;
        private float _visualOverallProgress;
        private float _targetOverallProgress;
        private float _visualCurrentProgress;
        private float _targetCurrentProgress;

        public UIGenProgressBar()
        {
            if (Main.netMode != 2)
            {
                _texInnerDirt = TextureManager.Load("Images/UI/WorldGen/Outer Dirt");
                _texOuterCorrupt = TextureManager.Load("Images/UI/WorldGen/Outer Corrupt");
                _texOuterCrimson = TextureManager.Load("Images/UI/WorldGen/Outer Crimson");
                _texOuterLower = TextureManager.Load("Images/UI/WorldGen/Outer Lower");
            }
            Recalculate();
        }

        public override void Recalculate()
        {
            Width.Precent = 0.0f;
            Height.Precent = 0.0f;
            Width.Pixels = 612f;
            Height.Pixels = 70f;
            base.Recalculate();
        }

        public void SetProgress(float overallProgress, float currentProgress)
        {
            _targetCurrentProgress = currentProgress;
            _targetOverallProgress = overallProgress;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            _visualOverallProgress = _targetOverallProgress;
            _visualCurrentProgress = _targetCurrentProgress;
            CalculatedStyle dimensions = this.GetDimensions();
            int completedWidth1 = (int)(_visualOverallProgress * 504.0);
            int completedWidth2 = (int)(_visualCurrentProgress * 504.0);
            Vector2 vector2 = new Vector2(dimensions.X, dimensions.Y);
            Color filled = new Color();
            filled.PackedValue = WorldGen.crimson ? 4286836223U : 4283888223U;
            DrawFilling2(spriteBatch, vector2 + new Vector2(20f, 40f), 16, completedWidth1, 564, filled, Color.Lerp(filled, Color.Black, 0.5f), new Color(48, 48, 48));
            filled.PackedValue = 4290947159U;
            DrawFilling2(spriteBatch, vector2 + new Vector2(50f, 60f), 8, completedWidth2, 504, filled, Color.Lerp(filled, Color.Black, 0.5f), new Color(33, 33, 33));
            Rectangle r = this.GetDimensions().ToRectangle();
            r.X -= 8;
            spriteBatch.Draw(WorldGen.crimson ? _texOuterCrimson : _texOuterCorrupt, Utils.TopLeft(r), Color.White);
            spriteBatch.Draw(_texOuterLower, Utils.TopLeft(r) + new Vector2(44f, 60f), Color.White);
        }

        private void DrawFilling(SpriteBatch spritebatch, Texture2D tex, Texture2D texShadow, Vector2 topLeft, int completedWidth, int totalWidth, Color separator, Color empty)
        {
            if (completedWidth % 2 != 0)
                --completedWidth;
            Vector2 position = topLeft + (float)completedWidth * Vector2.UnitX;
            int num = completedWidth;
            Rectangle rectangle = Utils.Frame(tex, 1, 1, 0, 0);
            while (num > 0)
            {
                if (rectangle.Width > num)
                {
                    rectangle.X += rectangle.Width - num;
                    rectangle.Width = num;
                }
                spritebatch.Draw(tex, position, new Rectangle?(rectangle), Color.White, 0.0f, new Vector2((float)rectangle.Width, 0.0f), 1f, SpriteEffects.None, 0.0f);
                position.X -= (float)rectangle.Width;
                num -= rectangle.Width;
            }

            if (texShadow != null)
                spritebatch.Draw(texShadow, topLeft, new Rectangle?(new Rectangle(0, 0, completedWidth, texShadow.Height)), Color.White);
            spritebatch.Draw(Main.magicPixel, new Rectangle((int)topLeft.X + completedWidth, (int)topLeft.Y, totalWidth - completedWidth, tex.Height), new Rectangle?(new Rectangle(0, 0, 1, 1)), empty);
            spritebatch.Draw(Main.magicPixel, new Rectangle((int)topLeft.X + completedWidth - 2, (int)topLeft.Y, 2, tex.Height), new Rectangle?(new Rectangle(0, 0, 1, 1)), separator);
        }

        private void DrawFilling2(SpriteBatch spritebatch, Vector2 topLeft, int height, int completedWidth, int totalWidth, Color filled, Color separator, Color empty)
        {
            if (completedWidth % 2 != 0)
                --completedWidth;
            spritebatch.Draw(Main.magicPixel, new Rectangle((int)topLeft.X, (int)topLeft.Y, completedWidth, height), new Rectangle?(new Rectangle(0, 0, 1, 1)), filled);
            spritebatch.Draw(Main.magicPixel, new Rectangle((int)topLeft.X + completedWidth, (int)topLeft.Y, totalWidth - completedWidth, height), new Rectangle?(new Rectangle(0, 0, 1, 1)), empty);
            spritebatch.Draw(Main.magicPixel, new Rectangle((int)topLeft.X + completedWidth - 2, (int)topLeft.Y, 2, height), new Rectangle?(new Rectangle(0, 0, 1, 1)), separator);
        }
    }
}
