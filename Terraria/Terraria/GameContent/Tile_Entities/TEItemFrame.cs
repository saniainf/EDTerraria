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
            item = new Item();
        }

        public static int Place(int x, int y)
        {
            TEItemFrame teItemFrame = new TEItemFrame();
            teItemFrame.Position = new Point16(x, y);
            teItemFrame.ID = TileEntity.AssignNewID();
            teItemFrame.type = 1;
            TileEntity.ByID[teItemFrame.ID] = teItemFrame;
            TileEntity.ByPosition[teItemFrame.Position] = teItemFrame;
            return teItemFrame.ID;
        }

        public static int Hook_AfterPlacement(int x, int y, int type = 21, int style = 0, int direction = 1)
        {
            if (Main.netMode != 1)
                return Place(x, y);
            NetMessage.SendTileSquare(Main.myPlayer, x, y, 2);
            NetMessage.SendData(87, -1, -1, "", x, (float)y, 1f);
            return -1;
        }

        public static void Kill(int x, int y)
        {
            TileEntity tileEntity;
            if (!ByPosition.TryGetValue(new Point16(x, y), out tileEntity) || tileEntity.type != 1)
                return;

            ByID.Remove(tileEntity.ID);
            ByPosition.Remove(new Point16(x, y));
        }

        public static int Find(int x, int y)
        {
            TileEntity tileEntity;
            if (ByPosition.TryGetValue(new Point16(x, y), out tileEntity) && tileEntity.type == 1)
                return tileEntity.ID;

            return -1;
        }

        public static bool ValidTile(int x, int y)
        {
            return Main.tile[x, y].active() && Main.tile[x, y].type == 395 && (Main.tile[x, y].frameY == 0 && Main.tile[x, y].frameX % 36 == 0);
        }

        public override void WriteExtraData(BinaryWriter writer)
        {
            writer.Write((short)item.netID);
            writer.Write(item.prefix);
            writer.Write((short)item.stack);
        }

        public override void ReadExtraData(BinaryReader reader)
        {
            item = new Item();
            item.netDefaults((int)reader.ReadInt16());
            item.Prefix((int)reader.ReadByte());
            item.stack = (int)reader.ReadInt16();
        }

        public override string ToString()
        {
            return Position.X + "x  " + Position.Y + "y item: " + item.ToString();
        }

        public void DropItem()
        {
            if (Main.netMode != 1)
                Item.NewItem((int)Position.X * 16, (int)Position.Y * 16, 32, 32, item.netID, 1, false, (int)item.prefix, false);
            item = new Item();
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
