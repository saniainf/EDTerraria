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
using Terraria.Graphics.Effects;

namespace Terraria.GameContent.Skies
{
    internal class MartianSky : CustomSky
    {
        private Random _random = new Random();
        private Ufo[] _ufos;
        private int _maxUfos;
        private bool _active;
        private bool _leaving;
        private int _activeUfos;

        public override void Update()
        {
            if (Main.gamePaused || !Main.hasFocus)
                return;

            int index1 = _activeUfos;
            for (int index2 = 0; index2 < _ufos.Length; ++index2)
            {
                Ufo ufo = _ufos[index2];
                if (ufo.IsActive)
                {
                    ++ufo.Frame;
                    if (!ufo.Update())
                    {
                        if (!_leaving)
                        {
                            ufo.AssignNewBehavior();
                        }
                        else
                        {
                            ufo.IsActive = false;
                            --index1;
                        }
                    }
                }
                _ufos[index2] = ufo;
            }

            if (!_leaving && index1 != _maxUfos)
            {
                _ufos[index1].IsActive = true;
                _ufos[index1++].AssignNewBehavior();
            }
            _active = !_leaving || index1 != 0;
            _activeUfos = index1;
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (Main.screenPosition.Y > 10000.0)
                return;

            int num1 = -1;
            int num2 = 0;
            for (int index = 0; index < _ufos.Length; ++index)
            {
                float num3 = _ufos[index].Depth;
                if (num1 == -1 && num3 < maxDepth)
                    num1 = index;
                if (num3 > minDepth)
                    num2 = index;
                else
                    break;
            }

            if (num1 == -1)
                return;

            Color color = new Color(Main.bgColor.ToVector4() * 0.9f + new Vector4(0.1f));
            Vector2 vector2_1 = Main.screenPosition + new Vector2((Main.screenWidth >> 1), (Main.screenHeight >> 1));
            Rectangle rectangle = new Rectangle(-1000, -1000, 4000, 4000);
            for (int index = num1; index < num2; ++index)
            {
                Vector2 vector2_2 = new Vector2(1f / _ufos[index].Depth, 0.9f / _ufos[index].Depth);
                Vector2 position = _ufos[index].Position;
                position = (position - vector2_1) * vector2_2 + vector2_1 - Main.screenPosition;
                if (_ufos[index].IsActive && rectangle.Contains((int)position.X, (int)position.Y))
                {
                    spriteBatch.Draw(_ufos[index].Texture, position, new Rectangle?(_ufos[index].GetSourceRectangle()), color * _ufos[index].Opacity, _ufos[index].Rotation,
                        Vector2.Zero, vector2_2.X * 5f * _ufos[index].Scale, SpriteEffects.None, 0.0f);
                    if (_ufos[index].GlowTexture != null)
                        spriteBatch.Draw(_ufos[index].GlowTexture, position, new Rectangle?(_ufos[index].GetSourceRectangle()), Color.White * _ufos[index].Opacity,
                            _ufos[index].Rotation, Vector2.Zero, vector2_2.X * 5f * _ufos[index].Scale, SpriteEffects.None, 0.0f);
                }
            }
        }

        private void GenerateUfos()
        {
            _maxUfos = (int)(256.0 * (Main.maxTilesX / 4200f));
            _ufos = new Ufo[_maxUfos];
            int num1 = _maxUfos >> 4;
            for (int index = 0; index < num1; ++index)
            {
                double num2 = index / num1;
                _ufos[index] = new Ufo(Main.extraTexture[5], (float)(Main.rand.NextDouble() * 4.0 + 6.59999990463257));
                _ufos[index].GlowTexture = Main.glowMaskTexture[90];
            }

            for (int index = num1; index < this._ufos.Length; ++index)
            {
                double num2 = (index - num1) / (_ufos.Length - num1);
                _ufos[index] = new Ufo(Main.extraTexture[6], (float)(Main.rand.NextDouble() * 5.0 + 1.60000002384186));
                _ufos[index].Scale = 0.5f;
                _ufos[index].GlowTexture = Main.glowMaskTexture[91];
            }
        }

        internal override void Activate(Vector2 position, params object[] args)
        {
            _activeUfos = 0;
            GenerateUfos();
            Array.Sort<Ufo>(_ufos, (Comparison<Ufo>)((ufo1, ufo2) => ufo2.Depth.CompareTo(ufo1.Depth)));
            _active = true;
            _leaving = false;
        }

        internal override void Deactivate(params object[] args)
        {
            _leaving = true;
        }

        public override bool IsActive()
        {
            return _active;
        }

        public override void Reset()
        {
            _active = false;
        }

        private abstract class IUfoController
        {
            public abstract void InitializeUfo(ref Ufo ufo);

            public abstract bool Update(ref Ufo ufo);
        }

        private class ZipBehavior : IUfoController
        {
            private Vector2 _speed;
            private int _ticks;
            private int _maxTicks;

            public override void InitializeUfo(ref Ufo ufo)
            {
                ufo.Position.X = (float)Ufo.Random.NextDouble() * (Main.maxTilesX << 4);
                ufo.Position.Y = (float)(Ufo.Random.NextDouble() * 5000.0);
                ufo.Opacity = 0.0f;
                float num1 = (float)(Ufo.Random.NextDouble() * 5.0 + 10.0);
                double num2 = Ufo.Random.NextDouble() * 0.600000023841858 - 0.300000011920929;
                ufo.Rotation = (float)num2;
                if (Ufo.Random.Next(2) == 0)
                    num2 += 3.14159274101257;
                _speed = new Vector2((float)Math.Cos(num2) * num1, (float)Math.Sin(num2) * num1);
                _ticks = 0;
                _maxTicks = Ufo.Random.Next(400, 500);
            }

            public override bool Update(ref Ufo ufo)
            {
                if (_ticks < 10)
                    ufo.Opacity += 0.1f;
                else if (_ticks > _maxTicks - 10)
                    ufo.Opacity -= 0.1f;
                ufo.Position += _speed;
                if (_ticks == _maxTicks)
                    return false;
                ++_ticks;
                return true;
            }
        }

        private class HoverBehavior : IUfoController
        {
            private int _ticks;
            private int _maxTicks;

            public override void InitializeUfo(ref Ufo ufo)
            {
                ufo.Position.X = (float)Ufo.Random.NextDouble() * (float)(Main.maxTilesX << 4);
                ufo.Position.Y = (float)(Ufo.Random.NextDouble() * 5000.0);
                ufo.Opacity = 0.0f;
                ufo.Rotation = 0.0f;
                _ticks = 0;
                _maxTicks = Ufo.Random.Next(120, 240);
            }

            public override bool Update(ref Ufo ufo)
            {
                if (_ticks < 10)
                    ufo.Opacity += 0.1f;
                else if (_ticks > _maxTicks - 10)
                    ufo.Opacity -= 0.1f;
                if (_ticks == _maxTicks)
                    return false;
                ++_ticks;
                return true;
            }
        }

        private struct Ufo
        {
            public static Random Random = new Random();
            private const int MAX_FRAMES = 3;
            private const int FRAME_RATE = 4;
            private int _frame;
            private Texture2D _texture;
            private IUfoController _controller;
            public Texture2D GlowTexture;
            public Vector2 Position;
            public int FrameHeight;
            public int FrameWidth;
            public float Depth;
            public float Scale;
            public float Opacity;
            public bool IsActive;
            public float Rotation;

            public int Frame
            {
                get { return _frame; }
                set { _frame = value % 12; }
            }

            public Texture2D Texture
            {
                get { return _texture; }
                set
                {
                    _texture = value;
                    FrameWidth = value.Width;
                    FrameHeight = value.Height / 3;
                }
            }

            public IUfoController Controller
            {
                get { return _controller; }
                set
                {
                    _controller = value;
                    value.InitializeUfo(ref this);
                }
            }

            public Ufo(Texture2D texture, float depth = 1f)
            {
                _frame = 0;
                Position = Vector2.Zero;
                _texture = texture;
                Depth = depth;
                Scale = 1f;
                FrameWidth = texture.Width;
                FrameHeight = texture.Height / 3;
                GlowTexture = null;
                Opacity = 0.0f;
                Rotation = 0.0f;
                IsActive = false;
                _controller = null;
            }

            public Rectangle GetSourceRectangle()
            {
                return new Rectangle(0, _frame / 4 * FrameHeight, FrameWidth, FrameHeight);
            }

            public bool Update()
            {
                return Controller.Update(ref this);
            }

            public void AssignNewBehavior()
            {
                switch (Random.Next(2))
                {
                    case 0:
                        Controller = new ZipBehavior();
                        break;
                    case 1:
                        Controller = new HoverBehavior();
                        break;
                }
            }
        }
    }
}
