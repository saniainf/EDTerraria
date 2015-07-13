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
    internal class VortexSky : CustomSky
    {
        private Random _random = new Random();
        private Texture2D _planetTexture;
        private Texture2D _bgTexture;
        private Texture2D _boltTexture;
        private Texture2D _flashTexture;
        private bool _isActive;
        private int _ticksUntilNextBolt;
        private float _fadeOpacity;
        private VortexSky.Bolt[] _bolts;

        public override void OnLoad()
        {
            _planetTexture = TextureManager.Load("Images/Misc/VortexSky/Planet");
            _bgTexture = TextureManager.Load("Images/Misc/VortexSky/Background");
            _boltTexture = TextureManager.Load("Images/Misc/VortexSky/Bolt");
            _flashTexture = TextureManager.Load("Images/Misc/VortexSky/Flash");
        }

        public override void Update()
        {
            _fadeOpacity = !_isActive ? Math.Max(0.0f, _fadeOpacity - 0.01f) : Math.Min(1f, 0.01f + _fadeOpacity);
            if (_ticksUntilNextBolt <= 0)
            {
                _ticksUntilNextBolt = _random.Next(1, 5);
                int index = 0;
                while (_bolts[index].IsAlive && index != _bolts.Length - 1)
                    ++index;

                _bolts[index].IsAlive = true;
                _bolts[index].Position.X = (float)(Utils.NextFloat(_random) * (Main.maxTilesX * 16.0 + 4000.0) - 2000.0);
                _bolts[index].Position.Y = Utils.NextFloat(_random) * 500f;
                _bolts[index].Depth = (float)(Utils.NextFloat(_random) * 8.0 + 2.0);
                _bolts[index].Life = 30;
            }
            --_ticksUntilNextBolt;
            for (int index = 0; index < _bolts.Length; ++index)
            {
                if (_bolts[index].IsAlive)
                {
                    --_bolts[index].Life;
                    if (_bolts[index].Life <= 0)
                        _bolts[index].IsAlive = false;
                }
            }
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
                    Main.screenWidth, Main.screenHeight), Color.White * Math.Min(1f, (float)((Main.screenPosition.Y - 800.0) / 1000.0)) * _fadeOpacity);
                Vector2 vector2_1 = new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
                Vector2 vector2_2 = 0.01f * (new Vector2((float)Main.maxTilesX * 8f, (float)Main.worldSurface / 2f) - Main.screenPosition);
                spriteBatch.Draw(_planetTexture, vector2_1 + new Vector2(-200f, -200f) + vector2_2, new Rectangle?(), Color.White * 0.9f * _fadeOpacity, 0.0f,
                    new Vector2((float)(_planetTexture.Width >> 1), (float)(_planetTexture.Height >> 1)), 1f, SpriteEffects.None, 1f);
            }

            float num1 = Math.Min(1f, (float)((Main.screenPosition.Y - 1000.0) / 1000.0));
            Vector2 vector2_3 = Main.screenPosition + new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
            Rectangle rectangle = new Rectangle(-1000, -1000, 4000, 4000);
            for (int index = 0; index < _bolts.Length; ++index)
            {
                if (this._bolts[index].IsAlive && _bolts[index].Depth > minDepth && _bolts[index].Depth < maxDepth)
                {
                    Vector2 vector2_1 = new Vector2(1f / _bolts[index].Depth, 0.9f / _bolts[index].Depth);
                    Vector2 position = (_bolts[index].Position - vector2_3) * vector2_1 + vector2_3 - Main.screenPosition;
                    if (rectangle.Contains((int)position.X, (int)position.Y))
                    {
                        Texture2D texture = _boltTexture;
                        int num2 = _bolts[index].Life;
                        if (num2 > 26 && num2 % 2 == 0)
                            texture = _flashTexture;
                        float num3 = (float)num2 / 30f;
                        spriteBatch.Draw(texture, position, new Rectangle?(), Color.White * num1 * num3 * _fadeOpacity, 0.0f, Vector2.Zero, vector2_1.X * 5f, SpriteEffects.None, 0.0f);
                    }
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
            _bolts = new VortexSky.Bolt[500];
            for (int index = 0; index < _bolts.Length; ++index)
                _bolts[index].IsAlive = false;
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
            if (!this._isActive)
                return _fadeOpacity > 1.0 / 1000.0;

            return true;
        }

        private struct Bolt
        {
            public Vector2 Position;
            public float Depth;
            public int Life;
            public bool IsAlive;
        }
    }
}
