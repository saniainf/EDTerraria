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

namespace Terraria.Net
{
    public abstract class NetModule
    {
        protected const int HEADER_SIZE = 5;
        public ushort Id;

        public abstract bool Deserialize(BinaryReader reader, int userId);

        protected static NetPacket CreatePacket<T>(int size) where T : NetModule
        {
            ushort id = NetManager.Instance.GetId<T>();
            NetPacket netPacket = new NetPacket(id, size + 5);
            netPacket.Writer.Write((ushort)(size + 5));
            netPacket.Writer.Write((byte)82);
            netPacket.Writer.Write(id);
            return netPacket;
        }
    }
}
