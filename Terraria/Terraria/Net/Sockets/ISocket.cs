/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using Terraria.Net;

namespace Terraria.Net.Sockets
{
    public interface ISocket
    {
        void Close();

        bool IsConnected();

        void Connect(RemoteAddress address);

        void AsyncSend(byte[] data, int offset, int size, SocketSendCallback callback, object state = null);

        void AsyncReceive(byte[] data, int offset, int size, SocketReceiveCallback callback, object state = null);

        bool IsDataAvailable();

        bool StartListening(SocketConnectionAccepted callback);

        void StopListening();

        RemoteAddress GetRemoteAddress();
    }
}
