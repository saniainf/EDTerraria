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

namespace Terraria
{
	public class k_TileBlock
	{
		public ushort protoID;
		public byte variant;
		public byte parentOffset;
		public byte slope;
		public byte animFrame;
		public byte flags;
		public byte colourID;

		public k_TileBlock() { }

		public void SetParentOffset(byte x, byte y)
		{
			y <<= 4;
			parentOffset = (byte)(x + y);
		}
		public Point GetParentOffset()
		{
			return new Point(parentOffset & 15, parentOffset & 240);
		}

		public k_TileBlock Copy()
		{
			return new k_TileBlock();
		}
	}

	public class k_TileEntity : k_TileBlock
	{
		public NBT nbt;

		public k_TileEntity() { }
	}

	public enum k_WireFlags : byte
	{
		WIRE_NONE		= 0x0,

		WIRE_RED		= 0x1,
		WIRE_GREEN		= 0x2,
		WIRE_BLUE		= 0x4,

		WIRE_RG			= WIRE_RED | WIRE_GREEN,
		WIRE_RB			= WIRE_RED | WIRE_BLUE,
		WIRE_GB			= WIRE_GREEN | WIRE_BLUE,
		WIRE_RGB		= WIRE_RED | WIRE_GREEN | WIRE_BLUE,

		WIRE_ACTUATOR	= 0x8,
		/*
		WIRE_BRIDGE_RG	= 0x10,
		WIRE_BRIDGE_RB	= 0x20,
		WIRE_BRIDGE_GB	= 0x40,
		WIRE_BRIDGE_RGB	= 0x80,
		//*/
	}

    public class Tile
    {
		//*
		public k_TileBlock k_block;

		public ushort k_wall_protoID;
		public byte k_wall_variant;
		public byte k_wall_colourID;

		public byte k_liquid_protoID;
		public byte k_liquid_amount;

		public k_WireFlags k_wireFlags;
		
		public void k_Constructor(Tile copy)
		{
			if (copy == null)
			{
				k_block = null;
				k_wall_protoID = 0;
				k_wall_variant = 0;
				k_wall_colourID = 0;
				k_liquid_protoID = 0;
				k_liquid_amount = 0;
				k_wireFlags = 0;
			}
			else
			{
				k_block = copy.k_block.Copy();
				k_wall_protoID = copy.k_wall_protoID;
				k_wall_variant = copy.k_wall_variant;
				k_wall_colourID = copy.k_wall_colourID;
				k_liquid_protoID = copy.k_liquid_protoID;
				k_liquid_amount = copy.k_liquid_amount;
				k_wireFlags = copy.k_wireFlags;
			}
		}

		public void k_SetBlock(ushort proto, byte variant = 0, byte flags = 0)
		{
			k_block = new k_TileBlock();
			k_block.protoID = proto;
			k_block.variant = variant;
			k_block.flags = flags;
		}
		public void k_KillBlock() { k_SetBlock(0); }

		public void k_SetWall(ushort proto, byte variant = 0)
		{
			k_wall_protoID = proto;
			k_wall_variant = variant;
			k_wall_colourID = 0;
		}
		public void k_KillWall() { k_SetWall(0); }

		public void k_AddLiquid(ushort proto, byte amount){ }
		public byte k_RemoveLiquid(byte amount)
		{
			byte amt = amount;
			if (amt > k_liquid_amount)
				amt = k_liquid_amount;

			k_liquid_amount -= amt;
			if (k_liquid_amount <= 0)
				k_liquid_protoID = 0;

			return amt;
		}

		public void k_SetWireFlags(k_WireFlags flags, bool state)
		{
			if (state)
				k_wireFlags |= flags;
			else
				k_wireFlags &= ~flags;
		}
		public bool k_HasWireFlags(k_WireFlags flags)
		{
			return (k_wireFlags & flags) == flags;
		}
		public bool k_HasWireFlagsAny(k_WireFlags flags)
		{
			return (k_wireFlags & flags) != 0;
		}
		public void k_KillWireFlags()
		{
			k_wireFlags = k_WireFlags.WIRE_NONE;
		}

		public static k_WireFlags k_HACK_GetNetworkWireType(int type)
		{
			switch (type)
			{
				case 5:
				case 6:
					return k_WireFlags.WIRE_RED;
				case 8:
				case 9:
					return k_WireFlags.WIRE_ACTUATOR;
				case 10:
				case 11:
					return k_WireFlags.WIRE_GREEN;
				case 12:
				case 13:
					return k_WireFlags.WIRE_BLUE;
			}

			return k_WireFlags.WIRE_NONE;
		}
		public static int k_HACK_GetNetworkWirePlaceType(k_WireFlags flag)
		{
			switch (flag)
			{
				case k_WireFlags.WIRE_RED:
					return 5;
				case k_WireFlags.WIRE_ACTUATOR:
					return 8;
				case k_WireFlags.WIRE_GREEN:
					return 10;
				case k_WireFlags.WIRE_BLUE:
					return 12;
			}
			return -1;
		}
		public static int k_HACK_GetNetworkWireKillType(k_WireFlags flag)
		{
			switch (flag)
			{
				case k_WireFlags.WIRE_RED:
					return 6;
				case k_WireFlags.WIRE_ACTUATOR:
					return 9;
				case k_WireFlags.WIRE_GREEN:
					return 11;
				case k_WireFlags.WIRE_BLUE:
					return 13;
			}
			return -1;
		}
		//*/
        public const int Type_Solid = 0;
        public const int Type_Halfbrick = 1;
        public const int Type_SlopeDownRight = 2;
        public const int Type_SlopeDownLeft = 3;
        public const int Type_SlopeUpRight = 4;
        public const int Type_SlopeUpLeft = 5;
        public const int Liquid_Water = 0;
        public const int Liquid_Lava = 1;
        public const int Liquid_Honey = 2;
        public ushort type;
        public byte wall;
        public byte liquid;
        public short sTileHeader;
        public byte bTileHeader;
        public byte bTileHeader2;
        public byte bTileHeader3;
        public short frameX;
        public short frameY;

        public int collisionType
        {
            get
            {
                if (!this.active())
                    return 0;
                if (this.halfBrick())
                    return 2;
                if ((int)this.slope() > 0)
                    return 2 + (int)this.slope();
                return Main.tileSolid[(int)this.type] && !Main.tileSolidTop[(int)this.type] ? 1 : -1;
            }
        }

		private void Constructor(Tile copy = null)
		{
			if (copy == null)
			{
				type = (ushort)0;
				wall = (byte)0;
				liquid = (byte)0;
				sTileHeader = (short)0;
				bTileHeader = (byte)0;
				bTileHeader2 = (byte)0;
				bTileHeader3 = (byte)0;
				frameX = (short)0;
				frameY = (short)0;
				k_wireFlags = k_WireFlags.WIRE_NONE;
			}
			else
			{
				type = copy.type;
				wall = copy.wall;
				liquid = copy.liquid;
				sTileHeader = copy.sTileHeader;
				bTileHeader = copy.bTileHeader;
				bTileHeader2 = copy.bTileHeader2;
				bTileHeader3 = copy.bTileHeader3;
				frameX = copy.frameX;
				frameY = copy.frameY;
				k_wireFlags = copy.k_wireFlags;
			}
		}

        public Tile()
        {
			Constructor();
		}

        public Tile(Tile copy)
        {
			Constructor(copy);
        }

        public object Clone()
        {
			return new Tile(this);
        }

        public void ClearEverything()
        {
			Constructor();
        }

        public void ClearTile()
        {
            this.slope((byte)0);
            this.halfBrick(false);
            this.active(false);
        }

        public void CopyFrom(Tile from)
        {
			Constructor(from);
        }

        public bool isTheSameAs(Tile compTile)
        {
            if (compTile == null || (int)this.sTileHeader != (int)compTile.sTileHeader || this.active() && ((int)this.type != (int)compTile.type || Main.tileFrameImportant[(int)this.type] && ((int)this.frameX != (int)compTile.frameX || (int)this.frameY != (int)compTile.frameY)) || ((int)this.wall != (int)compTile.wall || (int)this.liquid != (int)compTile.liquid))
                return false;
			if (k_wireFlags != compTile.k_wireFlags)
				return false;
			if ((int)compTile.liquid == 0)
            {
                if ((int)this.wallColor() != (int)compTile.wallColor())
                    return false;
            }
            else if ((int)this.bTileHeader != (int)compTile.bTileHeader)
                return false;
            return true;
        }

        public int wallFrameX()
        {
            return ((int)this.bTileHeader2 & 15) * 36;
        }

        public void wallFrameX(int wallFrameX)
        {
            this.bTileHeader2 = (byte)((int)this.bTileHeader2 & 240 | wallFrameX / 36 & 15);
        }

        public int wallFrameY()
        {
            return ((int)this.bTileHeader3 & 7) * 36;
        }

        public void wallFrameY(int wallFrameY)
        {
            this.bTileHeader3 = (byte)((int)this.bTileHeader3 & 248 | wallFrameY / 36 & 7);
        }

        public byte frameNumber()
        {
            return (byte)(((int)this.bTileHeader2 & 48) >> 4);
        }

        public void frameNumber(byte frameNumber)
        {
            this.bTileHeader2 = (byte)((int)this.bTileHeader2 & 207 | ((int)frameNumber & 3) << 4);
        }

        public byte wallFrameNumber()
        {
            return (byte)(((int)this.bTileHeader2 & 192) >> 6);
        }

        public void wallFrameNumber(byte wallFrameNumber)
        {
            this.bTileHeader2 = (byte)((int)this.bTileHeader2 & 63 | ((int)wallFrameNumber & 3) << 6);
        }

        public bool topSlope()
        {
            byte num = this.slope();
            if ((int)num != 1)
                return (int)num == 2;
            return true;
        }

        public bool bottomSlope()
        {
            byte num = this.slope();
            if ((int)num != 3)
                return (int)num == 4;
            return true;
        }

        public bool leftSlope()
        {
            byte num = this.slope();
            if ((int)num != 2)
                return (int)num == 4;
            return true;
        }

        public bool rightSlope()
        {
            byte num = this.slope();
            if ((int)num != 1)
                return (int)num == 3;
            return true;
        }

        public byte slope()
        {
            return (byte)(((int)this.sTileHeader & 28672) >> 12);
        }

        public bool HasSameSlope(Tile tile)
        {
            return ((int)this.sTileHeader & 29696) == ((int)tile.sTileHeader & 29696);
        }

        public void slope(byte slope)
        {
            this.sTileHeader = (short)((int)this.sTileHeader & 36863 | ((int)slope & 7) << 12);
        }

        public int blockType()
        {
            if (this.halfBrick())
                return 1;
            int num = (int)this.slope();
            if (num > 0)
                ++num;
            return num;
        }

        public byte color()
        {
            return (byte)((uint)this.sTileHeader & 31U);
        }

        public void color(byte color)
        {
            if ((int)color > 30)
                color = (byte)30;
            this.sTileHeader = (short)((int)this.sTileHeader & 65504 | (int)color);
        }

        public byte wallColor()
        {
            return (byte)((uint)this.bTileHeader & 31U);
        }

        public void wallColor(byte wallColor)
        {
            if ((int)wallColor > 30)
                wallColor = (byte)30;
            this.bTileHeader = (byte)((uint)this.bTileHeader & 224U | (uint)wallColor);
        }

        public bool lava()
        {
            return ((int)this.bTileHeader & 32) == 32;
        }

        public void lava(bool lava)
        {
            if (lava)
                this.bTileHeader = (byte)((int)this.bTileHeader & 159 | 32);
            else
                this.bTileHeader = (byte)((uint)this.bTileHeader & 223U);
        }

        public bool honey()
        {
            return ((int)this.bTileHeader & 64) == 64;
        }

        public void honey(bool honey)
        {
            if (honey)
                this.bTileHeader = (byte)((int)this.bTileHeader & 159 | 64);
            else
                this.bTileHeader = (byte)((uint)this.bTileHeader & 191U);
        }

        public void liquidType(int liquidType)
        {
            if (liquidType == 0)
                this.bTileHeader = (byte)((uint)this.bTileHeader & 159U);
            else if (liquidType == 1)
            {
                this.lava(true);
            }
            else
            {
                if (liquidType != 2)
                    return;
                this.honey(true);
            }
        }

        public byte liquidType()
        {
            return (byte)(((int)this.bTileHeader & 96) >> 5);
        }

        public bool checkingLiquid()
        {
            return ((int)this.bTileHeader3 & 8) == 8;
        }

        public void checkingLiquid(bool checkingLiquid)
        {
            if (checkingLiquid)
                this.bTileHeader3 |= (byte)8;
            else
                this.bTileHeader3 = (byte)((uint)this.bTileHeader3 & 247U);
        }

        public bool skipLiquid()
        {
            return ((int)this.bTileHeader3 & 16) == 16;
        }

        public void skipLiquid(bool skipLiquid)
        {
            if (skipLiquid)
                this.bTileHeader3 |= (byte)16;
            else
                this.bTileHeader3 = (byte)((uint)this.bTileHeader3 & 239U);
        }

        public bool halfBrick()
        {
            return ((int)this.sTileHeader & 1024) == 1024;
        }

        public void halfBrick(bool halfBrick)
        {
            if (halfBrick)
                this.sTileHeader |= (short)1024;
            else
                this.sTileHeader = (short)((int)this.sTileHeader & 64511);
        }

        public bool nactive()
        {
            return ((int)this.sTileHeader & 96) == 32;
        }

        public bool inActive()
        {
            return ((int)this.sTileHeader & 64) == 64;
        }

        public void inActive(bool inActive)
        {
            if (inActive)
                this.sTileHeader |= (short)64;
            else
                this.sTileHeader = (short)((int)this.sTileHeader & 65471);
        }

        public bool active()
        {
            return ((int)this.sTileHeader & 32) == 32;
        }

        public void active(bool active)
        {
            if (active)
                this.sTileHeader |= (short)32;
            else
                this.sTileHeader = (short)((int)this.sTileHeader & 65503);
        }

        public void ResetToType(ushort type)
        {
			Constructor();
			this.type = type;
			sTileHeader = 32;
        }

        public Color actColor(Color oldColor)
        {
            if (!this.inActive())
                return oldColor;
            double num = 0.4;
            return new Color((int)(byte)(num * (double)oldColor.R), (int)(byte)(num * (double)oldColor.G), (int)(byte)(num * (double)oldColor.B), (int)oldColor.A);
        }

        public static void SmoothSlope(int x, int y, bool applyToNeighbors = true)
        {
            if (applyToNeighbors)
            {
                Tile.SmoothSlope(x + 1, y, false);
                Tile.SmoothSlope(x - 1, y, false);
                Tile.SmoothSlope(x, y + 1, false);
                Tile.SmoothSlope(x, y - 1, false);
            }
            Tile tile = Main.tile[x, y];
            if (!WorldGen.SolidOrSlopedTile(x, y))
                return;
            bool flag1 = !WorldGen.TileEmpty(x, y - 1);
            bool flag2 = !WorldGen.SolidOrSlopedTile(x, y - 1) && flag1;
            bool flag3 = WorldGen.SolidOrSlopedTile(x, y + 1);
            bool flag4 = WorldGen.SolidOrSlopedTile(x - 1, y);
            bool flag5 = WorldGen.SolidOrSlopedTile(x + 1, y);
            switch ((flag1 ? 1 : 0) << 3 | (flag3 ? 1 : 0) << 2 | (flag4 ? 1 : 0) << 1 | (flag5 ? 1 : 0))
            {
                case 4:
                    tile.slope((byte)0);
                    tile.halfBrick(true);
                    break;
                case 5:
                    tile.halfBrick(false);
                    tile.slope((byte)2);
                    break;
                case 6:
                    tile.halfBrick(false);
                    tile.slope((byte)1);
                    break;
                case 9:
                    if (flag2)
                        break;
                    tile.halfBrick(false);
                    tile.slope((byte)4);
                    break;
                case 10:
                    if (flag2)
                        break;
                    tile.halfBrick(false);
                    tile.slope((byte)3);
                    break;
                default:
                    tile.halfBrick(false);
                    tile.slope((byte)0);
                    break;
            }
        }

        internal void ClearMetadata()
        {
			var type = this.type;
			var wall = this.wall;
			Constructor();
			this.type = type;
			this.wall = wall;
        }
    }
}
