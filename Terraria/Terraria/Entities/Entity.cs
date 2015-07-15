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
using System;

namespace Terraria
{
    public abstract class Entity
    {
        public string name = "";
        public int direction = 1;
        public int whoAmI;
        public bool active;
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 oldPosition;
        public Vector2 oldVelocity;
        public int oldDirection;
        public int width;
        public int height;
        public bool wet;
        public bool honeyWet;
        public byte wetCount;
        public bool lavaWet;

        public Vector2 Center
        {
            get {return new Vector2(this.position.X + (width / 2), position.Y + (height / 2));}
            set {position = new Vector2(value.X - (width / 2), value.Y - (height / 2));}
        }

        public Vector2 Left
        {
            get {return new Vector2(position.X, position.Y + (height / 2));}
            set {position = new Vector2(value.X, value.Y - (height / 2));}
        }

        public Vector2 Right
        {
            get {return new Vector2(position.X + width, position.Y + (height / 2));}
            set {position = new Vector2(value.X - width, value.Y - (height / 2));}
        }

        public Vector2 Top
        {
            get {return new Vector2(position.X + (width / 2), position.Y);}
            set {position = new Vector2(value.X - (width / 2), value.Y);}
        }

        public Vector2 Bottom
        {
            get {return new Vector2(position.X + (width / 2), position.Y + height);}
            set {position = new Vector2(value.X - (width / 2), value.Y - height);}
        }

        public Vector2 Size
        {
            get {return new Vector2(width, height);}
            set
            {
                width = (int)value.X;
                height = (int)value.Y;
            }
        }

        public Rectangle Hitbox
        {
            get {return new Rectangle((int)position.X, (int)position.Y, width, height);}
            set
            {
                position = new Vector2(value.X, value.Y);
                width = value.Width;
                height = value.Height;
            }
        }

        public float AngleTo(Vector2 Destination)
        {
            return (float)Math.Atan2(Destination.Y - (double)Center.Y, Destination.X - (double)Center.X);
        }

        public float AngleFrom(Vector2 Source)
        {
            return (float)Math.Atan2(Center.Y - (double)Source.Y, Center.X - (double)Source.X);
        }

        public float Distance(Vector2 Other)
        {
            return Vector2.Distance(Center, Other);
        }

        public float DistanceSQ(Vector2 Other)
        {
            return Vector2.DistanceSquared(Center, Other);
        }

        public Vector2 DirectionTo(Vector2 Destination)
        {
            return Vector2.Normalize(Destination - Center);
        }

        public Vector2 DirectionFrom(Vector2 Source)
        {
            return Vector2.Normalize(Center - Source);
        }

        public bool WithinRange(Vector2 Target, float MaxRange)
        {
            return Vector2.DistanceSquared(Center, Target) <= MaxRange * (double)MaxRange;
        }
    }
}
