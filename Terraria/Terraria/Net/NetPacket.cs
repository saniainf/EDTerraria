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
using Terraria.DataStructures;

namespace Terraria.Net
{
    public struct NetPacket
    {
        public ushort Id;
        public int Length;
        public CachedBuffer Buffer;

        public BinaryWriter Writer
        {
            get
            {
                return this.Buffer.Writer;
            }
        }

        public BinaryReader Reader
        {
            get
            {
                return this.Buffer.Reader;
            }
        }

        public NetPacket(ushort id, int size)
        {
            this.Id = id;
            this.Buffer = BufferPool.Request(size);
            this.Length = size;
        }

        public void Recycle()
        {
            this.Buffer.Recycle();
        }
    }
}
