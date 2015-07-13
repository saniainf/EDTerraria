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
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Effects;

namespace Terraria.GameContent.Skies
{
    internal class StardustSky : CustomSky
    {
        private Random _random = new Random();
        private Texture2D _planetTexture;
        private Texture2D _bgTexture;
        private Texture2D[] _starTextures;
        private bool _isActive;
        private StardustSky.Star[] _stars;
        private float _fadeOpacity;

        public override void OnLoad()
        {
            _planetTexture = TextureManager.Load("Images/Misc/StarDustSky/Planet");
            _bgTexture = TextureManager.Load("Images/Misc/StarDustSky/Background");
            _starTextures = new Texture2D[2];
            for (int index = 0; index < _starTextures.Length; ++index)
                _starTextures[index] = TextureManager.Load("Images/Misc/StarDustSky/Star " + index);
        }

        public override void Update()
        {
            if (_isActive)
                _fadeOpacity = Math.Min(1f, 0.01f + _fadeOpacity);
            else
                _fadeOpacity = Math.Max(0.0f, _fadeOpacity - 0.01f);
        }

        public override Color OnTileColor(Color inColor)
        {
            return new Color(Vector4.Lerp(inColor.ToVector4(), Vector4.One, _fadeOpacity * 0.5f));
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (maxDepth >= 3.40282346638529E+38 && minDepth < 3.40282346638529E+38)
            {
                spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * _fadeOpacity);
                spriteBatch.Draw(_bgTexture, new Rectangle(0, Math.Max(0, (int)((Main.worldSurface * 16.0 - Main.screenPosition.Y - 2400.0) * 0.100000001490116)),
                    Main.screenWidth, Main.screenHeight), Color.White * Math.Min(1f, (float)((Main.screenPosition.Y - 800.0) / 1000.0) * _fadeOpacity));
                Vector2 vector2_1 = new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
                Vector2 vector2_2 = 0.01f * (new Vector2((float)Main.maxTilesX * 8f, (float)Main.worldSurface / 2f) - Main.screenPosition);
                spriteBatch.Draw(_planetTexture, vector2_1 + new Vector2(-200f, -200f) + vector2_2, new Rectangle?(), Color.White * 0.9f * _fadeOpacity, 0.0f,
                    new Vector2((float)(_planetTexture.Width >> 1), (float)(_planetTexture.Height >> 1)), 1f, SpriteEffects.None, 1f);
            }

            int num1 = -1;
            int num2 = 0;
            for (int index = 0; index < _stars.Length; ++index)
            {
                float num3 = _stars[index].Depth;
                if (num1 == -1 && num3 < maxDepth)
                    num1 = index;
                if (num3 > minDepth)
                    num2 = index;
                else
                    break;
            }

            if (num1 == -1)
                return;

            float num4 = Math.Min(1f, (float)((Main.screenPosition.Y - 1000.0) / 1000.0));
            Vector2 vector2_3 = Main.screenPosition + new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
            Rectangle rectangle = new Rectangle(-1000, -1000, 4000, 4000);
            for (int index = num1; index < num2; ++index)
            {
                Vector2 vector2_1 = new Vector2(1f / _stars[index].Depth, 1.1f / _stars[index].Depth);
                Vector2 position = (_stars[index].Position - vector2_3) * vector2_1 + vector2_3 - Main.screenPosition;
                if (rectangle.Contains((int)position.X, (int)position.Y))
                {
                    float num3 = (float)Math.Sin(_stars[index].AlphaFrequency * Main.GlobalTime + _stars[index].SinOffset) * _stars[index].AlphaAmplitude + _stars[index].AlphaAmplitude;
                    float num5 = (float)(Math.Sin(_stars[index].AlphaFrequency * Main.GlobalTime * 5.0 + _stars[index].SinOffset) * 0.100000001490116 - 0.100000001490116);
                    float num6 = MathHelper.Clamp(num3, 0.0f, 1f);
                    Texture2D texture = _starTextures[_stars[index].TextureIndex];
                    spriteBatch.Draw(texture, position, new Rectangle?(), Color.White * num4 * num6 * 0.8f * (1f - num5) * _fadeOpacity, 0.0f,
                        new Vector2((float)(texture.Width >> 1), (float)(texture.Height >> 1)), (float)((vector2_1.X * 0.5 + 0.5) * (num6 * 0.300000011920929 + 0.699999988079071)), SpriteEffects.None, 0.0f);
                }
            }
        }

        public override float GetCloudAlpha()
        {
            return (float)((1.0 - _fadeOpacity) * 0.300000011920929 + 0.699999988079071);
        }

        internal override void Activate(Vector2 position, params object[] args)
        {
            _fadeOpacity = 1.0f / 500.0f;
            _isActive = true;
            int num1 = 200;
            int num2 = 10;
            _stars = new Star[num1 * num2];
            int index1 = 0;
            for (int index2 = 0; index2 < num1; ++index2)
            {
                float num3 = (float)index2 / (float)num1;
                for (int index3 = 0; index3 < num2; ++index3)
                {
                    float num4 = (float)index3 / (float)num2;
                    _stars[index1].Position.X = (float)(num3 * Main.maxTilesX * 16.0);
                    _stars[index1].Position.Y = (float)(num4 * (Main.worldSurface * 16.0 + 2000.0) - 1000.0);
                    _stars[index1].Depth = (float)(Utils.NextFloat(_random) * 8.0 + 1.5);
                    _stars[index1].TextureIndex = this._random.Next(_starTextures.Length);
                    _stars[index1].SinOffset = Utils.NextFloat(_random) * 6.28f;
                    _stars[index1].AlphaAmplitude = Utils.NextFloat(_random) * 5f;
                    _stars[index1].AlphaFrequency = Utils.NextFloat(_random) + 1f;
                    ++index1;
                }
            }
            Array.Sort<Star>(_stars, new Comparison<Star>(SortMethod));
        }

        private int SortMethod(Star meteor1, Star meteor2)
        {
            return meteor2.Depth.CompareTo(meteor1.Depth);
        }

        internal override void Deactivate(params object[] args)
        {
            _isActive = false;
        }

        public override void Reset()
        {
            _isActive = false;
        }

        public override bool IsActive()
        {
            if (!_isActive)
                return _fadeOpacity > 1.0 / 1000.0;

            return true;
        }

        private struct Star
        {
            public Vector2 Position;
            public float Depth;
            public int TextureIndex;
            public float SinOffset;
            public float AlphaFrequency;
            public float AlphaAmplitude;
        }
    }
}