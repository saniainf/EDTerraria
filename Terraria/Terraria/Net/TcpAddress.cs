/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using System.Net;

namespace Terraria.Net
{
    public class TcpAddress : RemoteAddress
    {
        public IPAddress Address;
        public int Port;

        public TcpAddress(IPAddress address, int port)
        {
            this.Type = AddressType.Tcp;
            this.Address = address;
            this.Port = port;
        }

        public override string GetIdentifier()
        {
            return this.Address.ToString();
        }

        public override bool IsLocalHost()
        {
            return this.Address.Equals((object)IPAddress.Loopback);
        }

        public override string ToString()
        {
            return new IPEndPoint(this.Address, this.Port).ToString();
        }

        public override string GetFriendlyName()
        {
            return this.ToString();
        }
    }
}
