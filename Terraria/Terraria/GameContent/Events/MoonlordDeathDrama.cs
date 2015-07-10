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
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics;

namespace Terraria.GameContent.Events
{
    public class MoonlordDeathDrama
    {
        private static List<MoonlordPiece> _pieces = new List<MoonlordPiece>();
        private static List<MoonlordExplosion> _explosions = new List<MoonlordExplosion>();
        private static List<Vector2> _lightSources = new List<Vector2>();
        private static float whitening = 0.0f;
        private static float requestedLight = 0.0f;

        public static void Update()
        {
            for (int index = 0; index < _pieces.Count; ++index)
            {
                MoonlordPiece moonlordPiece = _pieces[index];
                moonlordPiece.Update();
                if (moonlordPiece.Dead)
                {
                    _pieces.Remove(moonlordPiece);
                    --index;
                }
            }

            for (int index = 0; index < _explosions.Count; ++index)
            {
                MoonlordExplosion moonlordExplosion = _explosions[index];
                moonlordExplosion.Update();
                if (moonlordExplosion.Dead)
                {
                    _explosions.Remove(moonlordExplosion);
                    --index;
                }
            }

            bool flag = false;
            for (int index = 0; index < _lightSources.Count; ++index)
            {
                if (Main.player[Main.myPlayer].Distance(_lightSources[index]) < 2000.0)
                {
                    flag = true;
                    break;
                }
            }

            _lightSources.Clear();
            if (!flag)
                requestedLight = 0.0f;
            if (requestedLight != whitening)
            {
                if (Math.Abs(requestedLight - whitening) < 0.0199999995529652)
                    whitening = requestedLight;
                else
                    whitening += (float)Math.Sign(requestedLight - whitening) * 0.02f;
            }

            requestedLight = 0.0f;
        }

        public static void DrawPieces(SpriteBatch spriteBatch)
        {
            Rectangle playerScreen = Utils.CenteredRectangle(Main.screenPosition + new Vector2((float)Main.screenWidth, (float)Main.screenHeight) * 0.5f,
                new Vector2((float)(Main.screenWidth + 1000), (float)(Main.screenHeight + 1000)));
            for (int index = 0; index < _pieces.Count; ++index)
            {
                if (_pieces[index].InDrawRange(playerScreen))
                    _pieces[index].Draw(spriteBatch);
            }
        }

        public static void DrawExplosions(SpriteBatch spriteBatch)
        {
            Rectangle playerScreen = Utils.CenteredRectangle(Main.screenPosition + new Vector2((float)Main.screenWidth, (float)Main.screenHeight) * 0.5f,
                new Vector2((float)(Main.screenWidth + 1000), (float)(Main.screenHeight + 1000)));
            for (int index = 0; index < _explosions.Count; ++index)
            {
                if (_explosions[index].InDrawRange(playerScreen))
                    _explosions[index].Draw(spriteBatch);
            }
        }

        public static void DrawWhite(SpriteBatch spriteBatch)
        {
            if (whitening == 0.0)
                return;

            Color color = Color.White * whitening;
            spriteBatch.Draw(Main.magicPixel, new Rectangle(-2, -2, Main.screenWidth + 4, Main.screenHeight + 4), new Rectangle?(new Rectangle(0, 0, 1, 1)), color);
        }

        public static void ThrowPieces(Vector2 MoonlordCoreCenter, int DramaSeed)
        {
            Random r = new Random(DramaSeed);
            Vector2 vector2_1 = Utils.RotatedBy(Vector2.UnitY, Utils.NextFloat(r) * 1.57079637050629 - 0.785398185253143 + 3.14159274101257, new Vector2());
            _pieces.Add(new MoonlordPiece(TextureManager.Load("Images/Misc/MoonExplosion/Spine"), new Vector2(64f, 150f), MoonlordCoreCenter + new Vector2(0.0f, 50f), 
                vector2_1 * 6f, 0.0f, (float)(Utils.NextFloat(r) * 0.100000001490116 - 0.0500000007450581)));
            Vector2 vector2_2 = Utils.RotatedBy(Vector2.UnitY, Utils.NextFloat(r) * 1.57079637050629 - 0.785398185253143 + 3.14159274101257, new Vector2());
            _pieces.Add(new MoonlordPiece(TextureManager.Load("Images/Misc/MoonExplosion/Shoulder"), new Vector2(40f, 120f), MoonlordCoreCenter + new Vector2(50f, -120f), 
                vector2_2 * 10f, 0.0f, (float)(Utils.NextFloat(r) * 0.100000001490116 - 0.0500000007450581)));
            Vector2 vector2_3 = Utils.RotatedBy(Vector2.UnitY, Utils.NextFloat(r) * 1.57079637050629 - 0.785398185253143 + 3.14159274101257, new Vector2());
            _pieces.Add(new MoonlordPiece(TextureManager.Load("Images/Misc/MoonExplosion/Torso"), new Vector2(192f, 252f), MoonlordCoreCenter, vector2_3 * 8f, 0.0f, 
                (float)(Utils.NextFloat(r) * 0.100000001490116 - 0.0500000007450581)));
            Vector2 vector2_4 = Utils.RotatedBy(Vector2.UnitY, Utils.NextFloat(r) * 1.57079637050629 - 0.785398185253143 + 3.14159274101257, new Vector2());
            _pieces.Add(new MoonlordPiece(TextureManager.Load("Images/Misc/MoonExplosion/Head"), new Vector2(138f, 185f), MoonlordCoreCenter - new Vector2(0.0f, 200f), 
                vector2_4 * 12f, 0.0f, (float)(Utils.NextFloat(r) * 0.100000001490116 - 0.0500000007450581)));
        }

        public static void AddExplosion(Vector2 spot)
        {
            _explosions.Add(new MoonlordExplosion(TextureManager.Load("Images/Misc/MoonExplosion/Explosion"), spot, Main.rand.Next(2, 4)));
        }

        public static void RequestLight(float light, Vector2 spot)
        {
            _lightSources.Add(spot);
            if (light > 1.0)
                light = 1f;
            if (requestedLight >= (double)light)
                return;

            requestedLight = light;
        }

        public class MoonlordPiece
        {
            private Texture2D _texture;
            private Vector2 _position;
            private Vector2 _velocity;
            private Vector2 _origin;
            private float _rotation;
            private float _rotationVelocity;

            public bool Dead
            {
                get
                {
                    if (_position.Y <= (Main.maxTilesY * 16) - 480.0 && _position.X >= 480.0)
                        return _position.X >= (Main.maxTilesX * 16) - 480.0;
                    return true;
                }
            }

            public MoonlordPiece(Texture2D pieceTexture, Vector2 textureOrigin, Vector2 centerPos, Vector2 velocity, float rot, float angularVelocity)
            {
                _texture = pieceTexture;
                _origin = textureOrigin;
                _position = centerPos;
                _velocity = velocity;
                _rotation = rot;
                _rotationVelocity = angularVelocity;
            }

            public void Update()
            {
                _velocity.Y += 0.3f;
                _rotation += this._rotationVelocity;
                _rotationVelocity *= 0.99f;
                _position += this._velocity;
            }

            public void Draw(SpriteBatch sp)
            {
                Color light = GetLight();
                sp.Draw(_texture, _position - Main.screenPosition, new Rectangle?(), light, _rotation, _origin, 1f, SpriteEffects.None, 0.0f);
            }

            public bool InDrawRange(Rectangle playerScreen)
            {
                return playerScreen.Contains(Utils.ToPoint(_position));
            }

            public Color GetLight()
            {
                Vector3 zero = Vector3.Zero;
                float num1 = 0.0f;
                int num2 = 5;
                Point point = Utils.ToTileCoordinates(_position);
                for (int x = point.X - num2; x <= point.X + num2; ++x)
                {
                    for (int y = point.Y - num2; y <= point.Y + num2; ++y)
                    {
                        zero += Lighting.GetColor(x, y).ToVector3();
                        ++num1;
                    }
                }

                if (num1 == 0)
                    return Color.White;

                return new Color(zero / num1);
            }
        }

        public class MoonlordExplosion
        {
            private Texture2D _texture;
            private Vector2 _position;
            private Vector2 _origin;
            private Rectangle _frame;
            private int _frameCounter;
            private int _frameSpeed;

            public bool Dead
            {
                get
                {
                    if (_position.Y <= (Main.maxTilesY * 16) - 480.0 && _position.X >= 480.0 &&_position.X < (Main.maxTilesX * 16) - 480.0)
                        return _frameCounter >= _frameSpeed * 7;

                    return true;
                }
            }

            public MoonlordExplosion(Texture2D pieceTexture, Vector2 centerPos, int frameSpeed)
            {
                _texture = pieceTexture;
                _position = centerPos;
                _frameSpeed = frameSpeed;
                _frameCounter = 0;
                _frame = Utils.Frame(this._texture, 1, 7, 0, 0);
                _origin = Utils.Size(this._frame) / 2f;
            }

            public void Update()
            {
                ++_frameCounter;
                _frame = Utils.Frame(_texture, 1, 7, 0, _frameCounter / _frameSpeed);
            }

            public void Draw(SpriteBatch sp)
            {
                Color light = GetLight();
                sp.Draw(_texture, _position - Main.screenPosition, new Rectangle?(_frame), light, 0.0f, _origin, 1f, SpriteEffects.None, 0.0f);
            }

            public bool InDrawRange(Rectangle playerScreen)
            {
                return playerScreen.Contains(Utils.ToPoint(_position));
            }

            public Color GetLight()
            {
                return new Color(255, 255, 255, 127);
            }
        }
    }
}