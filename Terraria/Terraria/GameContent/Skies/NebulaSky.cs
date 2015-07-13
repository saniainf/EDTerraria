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
    internal class NebulaSky : CustomSky
    {
        private Random _random = new Random();
        private LightPillar[] _pillars;
        private Texture2D _planetTexture;
        private Texture2D _bgTexture;
        private Texture2D _beamTexture;
        private Texture2D[] _rockTextures;
        private bool _isActive;
        private float _fadeOpacity;

        public override void OnLoad()
        {
            _planetTexture = TextureManager.Load("Images/Misc/NebulaSky/Planet");
            _bgTexture = TextureManager.Load("Images/Misc/NebulaSky/Background");
            _beamTexture = TextureManager.Load("Images/Misc/NebulaSky/Beam");
            _rockTextures = new Texture2D[3];
            for (int index = 0; index < _rockTextures.Length; ++index)
                _rockTextures[index] = TextureManager.Load("Images/Misc/NebulaSky/Rock_" + index);
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
            for (int index = 0; index < _pillars.Length; ++index)
            {
                float num3 = _pillars[index].Depth;
                if (num1 == -1 && num3 < maxDepth)
                    num1 = index;
                if (num3 > minDepth)
                    num2 = index;
                else
                    break;
            }

            if (num1 == -1)
                return;

            Vector2 vector2_3 = Main.screenPosition + new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
            Rectangle rectangle = new Rectangle(-1000, -1000, 4000, 4000);
            float num4 = Math.Min(1f, (float)((Main.screenPosition.Y - 1000.0) / 1000.0));
            for (int index1 = num1; index1 < num2; ++index1)
            {
                Vector2 vector2_1 = new Vector2(1f / _pillars[index1].Depth, 0.9f / _pillars[index1].Depth);
                Vector2 position = (_pillars[index1].Position - vector2_3) * vector2_1 + vector2_3 - Main.screenPosition;
                if (rectangle.Contains((int)position.X, (int)position.Y))
                {
                    float num3 = vector2_1.X * 450f;
                    spriteBatch.Draw(_beamTexture, position, new Rectangle?(), Color.White * 0.2f * num4 * _fadeOpacity, 0.0f, Vector2.Zero, new Vector2(num3 / 70f, num3 / 45f), SpriteEffects.None, 0.0f);
                    int index2 = 0;
                    float num5 = 0.0f;
                    while (num5 <= 1.0)
                    {
                        float num6 = (float)(1.0 - (num5 + Main.GlobalTime * 0.0199999995529652 + Math.Sin(index1)) % 1.0);
                        spriteBatch.Draw(this._rockTextures[index2], position + new Vector2((float)(Math.Sin(num5 * 1582.0) * (num3 * 0.5) + num3 * 0.5), num6 * 2000f),
                            new Rectangle?(), Color.White * num6 * num4 * _fadeOpacity, num6 * 20f, new Vector2((float)(_rockTextures[index2].Width >> 1),
                                (float)(_rockTextures[index2].Height >> 1)), 0.9f, SpriteEffects.None, 0.0f);
                        index2 = (index2 + 1) % _rockTextures.Length;
                        num5 += 0.03f;
                    }
                }
            }
        }

        public override float GetCloudAlpha()
        {
            return (float)((1.0 - this._fadeOpacity) * 0.300000011920929 + 0.699999988079071);
        }

        internal override void Activate(Vector2 position, params object[] args)
        {
            _fadeOpacity = 1.0f / 500.0f;
            _isActive = true;
            _pillars = new LightPillar[40];
            for (int index = 0; index < this._pillars.Length; ++index)
            {
                _pillars[index].Position.X = (float)(index / _pillars.Length * ((double)Main.maxTilesX * 16.0 + 20000.0) + Utils.NextFloat(_random) * 40.0 - 20.0 - 20000.0);
                _pillars[index].Position.Y = (float)(Utils.NextFloat(_random) * 200.0 - 2000.0);
                _pillars[index].Depth = (float)(Utils.NextFloat(_random) * 8.0 + 7.0);
            }

            Array.Sort<LightPillar>(_pillars, new Comparison<LightPillar>(SortMethod));
        }

        private int SortMethod(LightPillar pillar1, LightPillar pillar2)
        {
            return pillar2.Depth.CompareTo(pillar1.Depth);
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

        private struct LightPillar
        {
            public Vector2 Position;
            public float Depth;
        }
    }
}
