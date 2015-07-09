/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using System.IO;
using Terraria;
using Terraria.DataStructures;

namespace Terraria.GameContent.Tile_Entities
{
    internal class TEItemFrame : TileEntity
    {
        public Item item;

        public TEItemFrame()
        {
            this.item = new Item();
        }

        public static int Place(int x, int y)
        {
            TEItemFrame teItemFrame = new TEItemFrame();
            teItemFrame.Position = new Point16(x, y);
            teItemFrame.ID = TileEntity.AssignNewID();
            teItemFrame.type = (byte)1;
            TileEntity.ByID[teItemFrame.ID] = (TileEntity)teItemFrame;
            TileEntity.ByPosition[teItemFrame.Position] = (TileEntity)teItemFrame;
            return teItemFrame.ID;
        }

        public static int Hook_AfterPlacement(int x, int y, int type = 21, int style = 0, int direction = 1)
        {
            if (Main.netMode != 1)
                return TEItemFrame.Place(x, y);
            NetMessage.SendTileSquare(Main.myPlayer, x, y, 2);
            NetMessage.SendData(87, -1, -1, "", x, (float)y, 1f, 0.0f, 0, 0, 0);
            return -1;
        }

        public static void Kill(int x, int y)
        {
            TileEntity tileEntity;
            if (!TileEntity.ByPosition.TryGetValue(new Point16(x, y), out tileEntity) || (int)tileEntity.type != 1)
                return;
            TileEntity.ByID.Remove(tileEntity.ID);
            TileEntity.ByPosition.Remove(new Point16(x, y));
        }

        public static int Find(int x, int y)
        {
            TileEntity tileEntity;
            if (TileEntity.ByPosition.TryGetValue(new Point16(x, y), out tileEntity) && (int)tileEntity.type == 1)
                return tileEntity.ID;
            return -1;
        }

        public static bool ValidTile(int x, int y)
        {
            return Main.tile[x, y].active() && (int)Main.tile[x, y].type == 395 && ((int)Main.tile[x, y].frameY == 0 && (int)Main.tile[x, y].frameX % 36 == 0);
        }

        public override void WriteExtraData(BinaryWriter writer)
        {
            writer.Write((short)this.item.netID);
            writer.Write(this.item.prefix);
            writer.Write((short)this.item.stack);
        }

        public override void ReadExtraData(BinaryReader reader)
        {
            this.item = new Item();
            this.item.netDefaults((int)reader.ReadInt16());
            this.item.Prefix((int)reader.ReadByte());
            this.item.stack = (int)reader.ReadInt16();
        }

        public override string ToString()
        {
            return (string)(object)this.Position.X + (object)"x  " + (string)(object)this.Position.Y + "y item: " + this.item.ToString();
        }

        public void DropItem()
        {
            if (Main.netMode != 1)
                Item.NewItem((int)this.Position.X * 16, (int)this.Position.Y * 16, 32, 32, this.item.netID, 1, false, (int)this.item.prefix, false);
            this.item = new Item();
        }

        public static void TryPlacing(int x, int y, int netid, int prefix, int stack)
        {
            int index = TEItemFrame.Find(x, y);
            if (index == -1)
            {
                int number = Item.NewItem(x * 16, y * 16, 32, 32, 1, 1, false, 0, false);
                Main.item[number].netDefaults(netid);
                Main.item[number].Prefix(prefix);
                Main.item[number].stack = stack;
                NetMessage.SendData(21, -1, -1, "", number, 0.0f, 0.0f, 0.0f, 0, 0, 0);
            }
            else
            {
                TEItemFrame teItemFrame = (TEItemFrame)TileEntity.ByID[index];
                if (teItemFrame.item.stack > 0)
                    teItemFrame.DropItem();
                teItemFrame.item = new Item();
                teItemFrame.item.netDefaults(netid);
                teItemFrame.item.Prefix(prefix);
                teItemFrame.item.stack = stack;
                NetMessage.SendData(86, -1, -1, "", teItemFrame.ID, (float)x, (float)y, 0.0f, 0, 0, 0);
            }
        }
    }
}
