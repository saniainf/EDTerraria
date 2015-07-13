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
using Terraria;

namespace Terraria.GameContent.UI
{
    public class WorldUIAnchor
    {
        public Vector2 pos = Vector2.Zero;
        public Vector2 size = Vector2.Zero;
        public AnchorType type;
        public Entity entity;

        public WorldUIAnchor()
        {
            type = AnchorType.None;
        }

        public WorldUIAnchor(Entity anchor)
        {
            type = AnchorType.Entity;
            entity = anchor;
        }

        public WorldUIAnchor(Vector2 anchor)
        {
            type = AnchorType.Pos;
            pos = anchor;
        }

        public WorldUIAnchor(int topLeftX, int topLeftY, int width, int height)
        {
            type = AnchorType.Tile;
            pos = new Vector2((float)topLeftX + (float)width / 2f, (float)topLeftY + (float)height / 2f) * 16f;
            size = new Vector2((float)width, (float)height) * 16f;
        }

        public bool InRange(Vector2 target, float tileRangeX, float tileRangeY)
        {
            switch (this.type)
            {
                case AnchorType.Entity:
                    if (Math.Abs(target.X - entity.Center.X) <= tileRangeX * 16.0 + entity.width / 2.0)
                        return Math.Abs(target.Y - entity.Center.Y) <= tileRangeY * 16.0 + entity.height / 2.0;
                    return false;
                case AnchorType.Tile:
                    if (Math.Abs(target.X - pos.X) <= tileRangeX * 16.0 + size.X / 2.0)
                        return Math.Abs(target.Y - pos.Y) <= tileRangeY * 16.0 + size.Y / 2.0;
                    return false;
                case AnchorType.Pos:
                    if (Math.Abs(target.X - pos.X) <= tileRangeX * 16.0)
                        return Math.Abs(target.Y - pos.Y) <= tileRangeY * 16.0;
                    return false;
                default:
                    return true;
            }
        }

        public enum AnchorType
        {
            Entity,
            Tile,
            Pos,
            None,
        }
    }
}
