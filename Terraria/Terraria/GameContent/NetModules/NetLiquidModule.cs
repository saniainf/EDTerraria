/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Net;

namespace Terraria.GameContent.NetModules
{
    internal class NetLiquidModule : NetModule
    {
        public static NetPacket Serialize(HashSet<int> changes)
        {
            NetPacket packet = NetModule.CreatePacket<NetLiquidModule>(changes.Count * 6 + 2);
            packet.Writer.Write((ushort)changes.Count);
            foreach (int num in changes)
            {
                int index1 = num >> 16 & (int)ushort.MaxValue;
                int index2 = num & (int)ushort.MaxValue;
                packet.Writer.Write(num);
                packet.Writer.Write(Main.tile[index1, index2].liquid);
                packet.Writer.Write(Main.tile[index1, index2].liquidType());
            }
            return packet;
        }

        public override bool Deserialize(BinaryReader reader, int userId)
        {
            int num1 = (int)reader.ReadUInt16();
            for (int index1 = 0; index1 < num1; ++index1)
            {
                int num2 = reader.ReadInt32();
                byte num3 = reader.ReadByte();
                byte num4 = reader.ReadByte();
                int index2 = num2 >> 16 & (int)ushort.MaxValue;
                int index3 = num2 & (int)ushort.MaxValue;
                Tile tile = Main.tile[index2, index3];
                if (tile != null)
                {
                    tile.liquid = num3;
                    tile.liquidType((int)num4);
                }
            }
            return true;
        }
    }
}
