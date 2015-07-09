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

namespace Terraria.Enums
{
    [Flags]
    public enum AnchorType
    {
        None = 0,
        SolidTile = 1,
        SolidWithTop = 2,
        Table = 4,
        SolidSide = 8,
        Tree = 16,
        AlternateTile = 32,
        EmptyTile = 64,
        SolidBottom = 128
    }
}
