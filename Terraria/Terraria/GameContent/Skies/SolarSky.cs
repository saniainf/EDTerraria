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
    internal class SolarSky : CustomSky
    {
        private Random _random = new Random();
        private Texture2D _planetTexture;
        private Texture2D _bgTexture;
        private Texture2D _meteorTexture;
        private bool _isActive;
        private SolarSky.Meteor[] _meteors;
        private float _fadeOpacity;

        public override void OnLoad()
        {
            _planetTexture = TextureManager.Load("Images/Misc/SolarSky/Planet");
            _bgTexture = TextureManager.Load("Images/Misc/SolarSky/Background");
            _meteorTexture = TextureManager.Load("Images/Misc/SolarSky/Meteor");
        }

        public override void Update()
        {
            _fadeOpacity = !_isActive ? Math.Max(0.0f, _fadeOpacity - 0.01f) : Math.Min(1f, 0.01f + _fadeOpacity);
            float num = 20f;
            for (int index = 0; index < _meteors.Length; ++index)
            {
                _meteors[index].Position.X -= num;
                _meteors[index].Position.Y += num;
                if (_meteors[index].Position.Y > Main.worldSurface * 16.0)
                {
                    _meteors[index].Position.X = _meteors[index].StartX;
                    _meteors[index].Position.Y = -10000f;
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
                    Main.screenWidth, Main.screenHeight), Color.White * Math.Min(1f, (float)((Main.screenPosition.Y - 800.0) / 1000.0) * _fadeOpacity));
                Vector2 vector2_1 = new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
                Vector2 vector2_2 = 0.01f * (new Vector2((float)Main.maxTilesX * 8f, (float)Main.worldSurface / 2f) - Main.screenPosition);
                spriteBatch.Draw(_planetTexture, vector2_1 + new Vector2(-200f, -200f) + vector2_2, new Rectangle?(), Color.White * 0.9f * _fadeOpacity, 0.0f,
                    new Vector2((float)(_planetTexture.Width >> 1), (float)(_planetTexture.Height >> 1)), 1f, SpriteEffects.None, 1f);
            }

            int num1 = -1;
            int num2 = 0;
            for (int index = 0; index < _meteors.Length; ++index)
            {
                float num3 = _meteors[index].Depth;
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
                Vector2 vector2_1 = new Vector2(1f / _meteors[index].Depth, 0.9f / _meteors[index].Depth);
                Vector2 position = (_meteors[index].Position - vector2_3) * vector2_1 + vector2_3 - Main.screenPosition;
                int num3 = _meteors[index].FrameCounter / 3;
                _meteors[index].FrameCounter = (_meteors[index].FrameCounter + 1) % 12;
                if (rectangle.Contains((int)position.X, (int)position.Y))
                    spriteBatch.Draw(_meteorTexture, position, new Rectangle?(new Rectangle(0, num3 * (_meteorTexture.Height / 4), _meteorTexture.Width, _meteorTexture.Height / 4)),
                        Color.White * num4 * _fadeOpacity, 0.0f, Vector2.Zero, vector2_1.X * 5f * _meteors[index].Scale, SpriteEffects.None, 0.0f);
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
            _meteors = new Meteor[150];

            for (int index = 0; index < _meteors.Length; ++index)
            {
                float num = (float)index / (float)_meteors.Length;
                _meteors[index].Position.X = (float)(num * (Main.maxTilesX * 16.0) + Utils.NextFloat(_random) * 40.0 - 20.0);
                _meteors[index].Position.Y = (float)(Utils.NextFloat(_random) * -(Main.worldSurface * 16.0 + 10000.0) - 10000.0);
                _meteors[index].Depth = _random.Next(3) == 0 ? (float)(Utils.NextFloat(_random) * 5.0 + 4.80000019073486) : (float)(Utils.NextFloat(_random) * 3.0 + 1.79999995231628);
                _meteors[index].FrameCounter = _random.Next(12);
                _meteors[index].Scale = (float)(Utils.NextFloat(_random) * 0.5 + 1.0);
                _meteors[index].StartX = _meteors[index].Position.X;
            }
            Array.Sort<Meteor>(_meteors, new Comparison<Meteor>(SortMethod));
        }

        private int SortMethod(Meteor meteor1, Meteor meteor2)
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

        private struct Meteor
        {
            public Vector2 Position;
            public float Depth;
            public int FrameCounter;
            public float Scale;
            public float StartX;
        }
    }
}
