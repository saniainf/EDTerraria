/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using Terraria;

namespace Terraria.World.Generation
{
    internal static class Conditions
    {
        public class IsTile : GenCondition
        {
            private ushort[] _types;

            public IsTile(params ushort[] types)
            {
                this._types = types;
            }

            protected override bool CheckValidity(int x, int y)
            {
                if (GenBase._tiles[x, y].active())
                {
                    for (int index = 0; index < this._types.Length; ++index)
                    {
                        if ((int)GenBase._tiles[x, y].type == (int)this._types[index])
                            return true;
                    }
                }
                return false;
            }
        }

        public class Continue : GenCondition
        {
            protected override bool CheckValidity(int x, int y)
            {
                return false;
            }
        }

        public class IsSolid : GenCondition
        {
            protected override bool CheckValidity(int x, int y)
            {
                if (GenBase._tiles[x, y].active())
                    return Main.tileSolid[(int)GenBase._tiles[x, y].type];
                return false;
            }
        }

        public class HasLava : GenCondition
        {
            protected override bool CheckValidity(int x, int y)
            {
                if ((int)GenBase._tiles[x, y].liquid > 0)
                    return (int)GenBase._tiles[x, y].liquidType() == 1;
                return false;
            }
        }
    }
}
