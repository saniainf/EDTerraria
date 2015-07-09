/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using System;

namespace Terraria
{
    public class HitTile
    {
        private static int lastCrack = -1;
        internal const int UNUSED = 0;
        internal const int TILE = 1;
        internal const int WALL = 2;
        internal const int MAX_HITTILES = 20;
        internal const int TIMETOLIVE = 60;
        private static Random rand;
        public HitTile.HitTileObject[] data;
        private int[] order;
        private int bufferLocation;

        public HitTile()
        {
            HitTile.rand = new Random();
            this.data = new HitTile.HitTileObject[21];
            this.order = new int[21];
            for (int index = 0; index <= 20; ++index)
            {
                this.data[index] = new HitTile.HitTileObject();
                this.order[index] = index;
            }
            this.bufferLocation = 0;
        }

        public int HitObject(int x, int y, int hitType)
        {
            for (int index1 = 0; index1 <= 20; ++index1)
            {
                int index2 = this.order[index1];
                HitTile.HitTileObject hitTileObject = this.data[index2];
                if (hitTileObject.type == hitType)
                {
                    if (hitTileObject.X == x && hitTileObject.Y == y)
                        return index2;
                }
                else if (index1 != 0 && hitTileObject.type == 0)
                    break;
            }
            HitTile.HitTileObject hitTileObject1 = this.data[this.bufferLocation];
            hitTileObject1.X = x;
            hitTileObject1.Y = y;
            hitTileObject1.type = hitType;
            return this.bufferLocation;
        }

        public void UpdatePosition(int tileId, int x, int y)
        {
            if (tileId < 0 || tileId > 20)
                return;
            HitTile.HitTileObject hitTileObject = this.data[tileId];
            hitTileObject.X = x;
            hitTileObject.Y = y;
        }

        public int AddDamage(int tileId, int damageAmount, bool updateAmount = true)
        {
            if (tileId < 0 || tileId > 20 || tileId == this.bufferLocation && damageAmount == 0)
                return 0;
            HitTile.HitTileObject hitTileObject = this.data[tileId];
            if (!updateAmount)
                return hitTileObject.damage + damageAmount;
            hitTileObject.damage += damageAmount;
            hitTileObject.timeToLive = 60;
            if (tileId == this.bufferLocation)
            {
                this.bufferLocation = this.order[20];
                this.data[this.bufferLocation].Clear();
                for (int index = 20; index > 0; --index)
                    this.order[index] = this.order[index - 1];
                this.order[0] = this.bufferLocation;
            }
            else
            {
                int index = 0;
                while (index <= 20 && this.order[index] != tileId)
                    ++index;
                for (; index > 1; --index)
                {
                    int num = this.order[index - 1];
                    this.order[index - 1] = this.order[index];
                    this.order[index] = num;
                }
                this.order[1] = tileId;
            }
            return hitTileObject.damage;
        }

        public void Clear(int tileId)
        {
            if (tileId < 0 || tileId > 20)
                return;
            this.data[tileId].Clear();
            int index = 0;
            while (index < 20 && this.order[index] != tileId)
                ++index;
            for (; index < 20; ++index)
                this.order[index] = this.order[index + 1];
            this.order[20] = tileId;
        }

        public void Prune()
        {
            bool flag = false;
            for (int index = 0; index <= 20; ++index)
            {
                HitTile.HitTileObject hitTileObject = this.data[index];
                if (hitTileObject.type != 0)
                {
                    Tile tile = Main.tile[hitTileObject.X, hitTileObject.Y];
                    if (hitTileObject.timeToLive <= 1)
                    {
                        hitTileObject.Clear();
                        flag = true;
                    }
                    else
                    {
                        --hitTileObject.timeToLive;
                        if ((double)hitTileObject.timeToLive < 12.0)
                            hitTileObject.damage -= 10;
                        else if ((double)hitTileObject.timeToLive < 24.0)
                            hitTileObject.damage -= 7;
                        else if ((double)hitTileObject.timeToLive < 36.0)
                            hitTileObject.damage -= 5;
                        else if ((double)hitTileObject.timeToLive < 48.0)
                            hitTileObject.damage -= 2;
                        if (hitTileObject.damage < 0)
                        {
                            hitTileObject.Clear();
                            flag = true;
                        }
                        else if (hitTileObject.type == 1)
                        {
                            if (!tile.active())
                            {
                                hitTileObject.Clear();
                                flag = true;
                            }
                        }
                        else if ((int)tile.wall == 0)
                        {
                            hitTileObject.Clear();
                            flag = true;
                        }
                    }
                }
            }
            if (!flag)
                return;
            int num1 = 1;
            while (flag)
            {
                flag = false;
                for (int index = num1; index < 20; ++index)
                {
                    if (this.data[this.order[index]].type == 0 && this.data[this.order[index + 1]].type != 0)
                    {
                        int num2 = this.order[index];
                        this.order[index] = this.order[index + 1];
                        this.order[index + 1] = num2;
                        flag = true;
                    }
                }
            }
        }

        public class HitTileObject
        {
            public int X;
            public int Y;
            public int damage;
            public int type;
            public int timeToLive;
            public int crackStyle;

            public HitTileObject()
            {
                this.Clear();
            }

            public void Clear()
            {
                this.X = 0;
                this.Y = 0;
                this.damage = 0;
                this.type = 0;
                this.timeToLive = 0;
                if (HitTile.rand == null)
                    HitTile.rand = new Random((int)DateTime.Now.Ticks);
                this.crackStyle = HitTile.rand.Next(4);
                while (this.crackStyle == HitTile.lastCrack)
                    this.crackStyle = HitTile.rand.Next(4);
                HitTile.lastCrack = this.crackStyle;
            }
        }
    }
}
