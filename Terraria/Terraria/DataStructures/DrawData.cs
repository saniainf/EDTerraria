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

namespace Terraria.DataStructures
{
    public struct DrawData
    {
        public static Rectangle? nullRectangle = new Rectangle?();
        public Texture2D texture;
        public Vector2 position;
        public Rectangle destinationRectangle;
        public Rectangle? sourceRect;
        public Color color;
        public float rotation;
        public Vector2 origin;
        public Vector2 scale;
        public SpriteEffects effect;
        public int shader;
        public bool ignorePlayerRotation;
        public readonly bool useDestinationRectangle;

        public DrawData(Texture2D tex, Vector2 pos, Color c)
        {
            texture = tex;
            position = pos;
            color = c;
            destinationRectangle = new Rectangle();
            sourceRect = DrawData.nullRectangle;
            rotation = 0.0f;
            origin = Vector2.Zero;
            scale = Vector2.One;
            effect = SpriteEffects.None;
            shader = 0;
            ignorePlayerRotation = false;
            useDestinationRectangle = false;
        }

        public DrawData(Texture2D tex, Vector2 pos, Rectangle? sRect, Color c)
        {
            texture = tex;
            position = pos;
            color = c;
            destinationRectangle = new Rectangle();
            sourceRect = sRect;
            rotation = 0.0f;
            origin = Vector2.Zero;
            scale = Vector2.One;
            effect = SpriteEffects.None;
            shader = 0;
            ignorePlayerRotation = false;
            useDestinationRectangle = false;
        }

        public DrawData(Texture2D tex, Vector2 pos, Rectangle? sRect, Color c, float rotat, Vector2 _origin, float _scale, SpriteEffects eff, int inactiveLayerDepth)
        {
            texture = tex;
            position = pos;
            sourceRect = sRect;
            color = c;
            rotation = rotat;
            origin = _origin;
            scale = new Vector2(_scale, _scale);
            effect = eff;
            destinationRectangle = new Rectangle();
            shader = 0;
            ignorePlayerRotation = false;
            useDestinationRectangle = false;
        }

        public DrawData(Texture2D tex, Vector2 pos, Rectangle? sRect, Color c, float rotat, Vector2 _origin, Vector2 _scale, SpriteEffects spriteEffect, int inactiveLayerDepth)
        {
            texture = tex;
            position = pos;
            sourceRect = sRect;
            color = c;
            rotation = rotat;
            origin = _origin;
            scale = _scale;
            effect = spriteEffect;
            destinationRectangle = new Rectangle();
            shader = 0;
            ignorePlayerRotation = false;
            useDestinationRectangle = false;
        }

        public DrawData(Texture2D tex, Rectangle desRectangle, Color c)
        {
            texture = tex;
            destinationRectangle = desRectangle;
            color = c;
            position = Vector2.Zero;
            sourceRect = DrawData.nullRectangle;
            rotation = 0.0f;
            origin = Vector2.Zero;
            scale = Vector2.One;
            effect = SpriteEffects.None;
            shader = 0;
            ignorePlayerRotation = false;
            useDestinationRectangle = false;
        }

        public DrawData(Texture2D tex, Rectangle desRectangle, Rectangle? sRect, Color c)
        {
            texture = tex;
            destinationRectangle = desRectangle;
            color = c;
            position = Vector2.Zero;
            sourceRect = sRect;
            rotation = 0.0f;
            origin = Vector2.Zero;
            scale = Vector2.One;
            effect = SpriteEffects.None;
            shader = 0;
            ignorePlayerRotation = false;
            useDestinationRectangle = false;
        }

        public DrawData(Texture2D tex, Rectangle desRectangle, Rectangle? sRect, Color c, float rotat, Vector2 _origin, SpriteEffects _effect, int inactiveLayerDepth)
        {
            texture = tex;
            destinationRectangle = desRectangle;
            sourceRect = sRect;
            color = c;
            rotation = rotat;
            origin = _origin;
            effect = _effect;
            position = Vector2.Zero;
            scale = Vector2.One;
            shader = 0;
            ignorePlayerRotation = false;
            useDestinationRectangle = false;
        }

        public void Draw(SpriteBatch sb)
        {
            if (useDestinationRectangle)
                sb.Draw(texture, destinationRectangle, sourceRect, color, rotation, origin, effect, 0.0f);
            else
                sb.Draw(texture, position, sourceRect, color, rotation, origin, scale, effect, 0.0f);
        }
    }
}
