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
    internal class SlimeSky : CustomSky
    {
        private Random _random = new Random();
        private Texture2D[] _textures;
        private SlimeSky.Slime[] _slimes;
        private int _slimesRemaining;
        private bool _isActive;
        private bool _isLeaving;

        public override void OnLoad()
        {
            _textures = new Texture2D[4];
            for (int index = 0; index < 4; ++index)
                _textures[index] = TextureManager.Load("Images/Misc/Sky_Slime_" + (index + 1));
            GenerateSlimes();
        }

        private void GenerateSlimes()
        {
            _slimes = new Slime[Main.maxTilesY / 6];
            for (int index = 0; index < _slimes.Length; ++index)
            {
                int maxValue = (int)(Main.screenPosition.Y * 0.7 - Main.screenHeight);
                int minValue = (int)(maxValue - Main.worldSurface * 16.0);
                _slimes[index].Position = new Vector2((float)(_random.Next(0, Main.maxTilesX) * 16), (float)_random.Next(minValue, maxValue));
                _slimes[index].Speed = (float)(5.0 + 3.0 * _random.NextDouble());
                _slimes[index].Depth = (float)(index / _slimes.Length * 1.75 + 1.60000002384186);
                _slimes[index].Texture = _textures[_random.Next(2)];

                if (_random.Next(60) == 0)
                {
                    _slimes[index].Texture = _textures[3];
                    _slimes[index].Speed = (float)(6.0 + 3.0 * _random.NextDouble());
                    _slimes[index].Depth += 0.5f;
                }
                else if (_random.Next(30) == 0)
                {
                    _slimes[index].Texture = _textures[2];
                    _slimes[index].Speed = (float)(6.0 + 2.0 * _random.NextDouble());
                }
                _slimes[index].Active = true;
            }
            _slimesRemaining = _slimes.Length;
        }

        public override void Update()
        {
            if (Main.gamePaused || !Main.hasFocus)
                return;

            for (int index = 0; index < _slimes.Length; ++index)
            {
                if (_slimes[index].Active)
                {
                    ++_slimes[index].Frame;
                    _slimes[index].Position.Y += _slimes[index].Speed;
                    if (_slimes[index].Position.Y > Main.worldSurface * 16.0)
                    {
                        if (!_isLeaving)
                        {
                            _slimes[index].Depth = (float)(index / _slimes.Length * 1.75 + 1.60000002384186);
                            _slimes[index].Position = new Vector2((float)(_random.Next(0, Main.maxTilesX) * 16), -100f);
                            _slimes[index].Texture = _textures[_random.Next(2)];
                            _slimes[index].Speed = (float)(5.0 + 3.0 * _random.NextDouble());
                            if (_random.Next(60) == 0)
                            {
                                _slimes[index].Texture = _textures[3];
                                _slimes[index].Speed = (float)(6.0 + 3.0 * _random.NextDouble());
                                _slimes[index].Depth += 0.5f;
                            }
                            else if (_random.Next(30) == 0)
                            {
                                _slimes[index].Texture = _textures[2];
                                _slimes[index].Speed = (float)(6.0 + 2.0 * _random.NextDouble());
                            }
                        }
                        else
                        {
                            _slimes[index].Active = false;
                            --_slimesRemaining;
                        }
                    }
                }
            }

            if (_slimesRemaining != 0)
                return;

            _isActive = false;
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (Main.screenPosition.Y > 10000.0 || Main.gameMenu)
                return;

            int num1 = -1;
            int num2 = 0;
            for (int index = 0; index < _slimes.Length; ++index)
            {
                float num3 = _slimes[index].Depth;
                if (num1 == -1 && num3 < maxDepth)
                    num1 = index;
                if (num3 > minDepth)
                    num2 = index;
                else
                    break;
            }

            if (num1 == -1)
                return;

            Vector2 vector2_1 = Main.screenPosition + new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
            Rectangle rectangle = new Rectangle(-1000, -1000, 4000, 4000);
            for (int index = num1; index < num2; ++index)
            {
                if (_slimes[index].Active)
                {
                    Color color = new Color(Main.bgColor.ToVector4() * 0.9f + new Vector4(0.1f)) * 0.8f;
                    float num3 = 1f;
                    if (_slimes[index].Depth > 3.0)
                        num3 = 0.6f;
                    else if (_slimes[index].Depth > 2.5)
                        num3 = 0.7f;
                    else if (_slimes[index].Depth > 2.0)
                        num3 = 0.8f;
                    else if (_slimes[index].Depth > 1.5)
                        num3 = 0.9f;

                    float num4 = num3 * 0.8f;
                    color = new Color((int)(color.R * num4), (int)(color.G * num4), (int)(color.B * num4), (int)(color.A * num4));
                    Vector2 vector2_2 = new Vector2(1f / _slimes[index].Depth, 0.9f / _slimes[index].Depth);
                    Vector2 position = _slimes[index].Position;
                    position = (position - vector2_1) * vector2_2 + vector2_1 - Main.screenPosition;
                    position.X = (float)((position.X + 500.0) % 4000.0);
                    if (position.X < 0.0)
                        position.X += 4000f;
                    position.X -= 500f;
                    if (rectangle.Contains((int)position.X, (int)position.Y))
                        spriteBatch.Draw(_slimes[index].Texture, position, new Rectangle?(_slimes[index].GetSourceRectangle()), color, 0.0f, Vector2.Zero, vector2_2.X * 2f, SpriteEffects.None, 0.0f);
                }
            }
        }

        internal override void Activate(Vector2 position, params object[] args)
        {
            GenerateSlimes();
            _isActive = true;
            _isLeaving = false;
        }

        internal override void Deactivate(params object[] args)
        {
            _isLeaving = true;
        }

        public override void Reset()
        {
            _isActive = false;
        }

        public override bool IsActive()
        {
            return _isActive;
        }

        private struct Slime
        {
            private const int MAX_FRAMES = 4;
            private const int FRAME_RATE = 6;
            private Texture2D _texture;
            public Vector2 Position;
            public float Depth;
            public int FrameHeight;
            public int FrameWidth;
            public float Speed;
            public bool Active;
            private int _frame;

            public Texture2D Texture
            {
                get { return _texture; }
                set
                {
                    _texture = value;
                    FrameWidth = value.Width;
                    FrameHeight = value.Height / 4;
                }
            }

            public int Frame
            {
                get { return _frame; }
                set { _frame = value % 24; }
            }

            public Rectangle GetSourceRectangle()
            {
                return new Rectangle(0, _frame / 6 * FrameHeight, FrameWidth, FrameHeight);
            }
        }
    }
}
