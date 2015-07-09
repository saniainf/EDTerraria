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
using System.IO;
using Terraria.Net.Sockets;

namespace Terraria
{
    public class RemoteServer
    {
        public ISocket Socket = (ISocket)new TcpSocket();
        public bool IsActive;
        public int State;
        public int TimeOutTimer;
        public bool IsReading;
        public byte[] ReadBuffer;
        public string StatusText;
        public int StatusCount;
        public int StatusMax;

        public void ClientWriteCallBack(object state)
        {
            --NetMessage.buffer[256].spamCount;
        }

        public void ClientReadCallBack(object state, int length)
        {
            try
            {
                if (!Netplay.disconnect)
                {
                    int streamLength = length;
                    if (streamLength == 0)
                    {
                        Netplay.disconnect = true;
                        Main.statusText = "Lost connection";
                    }
                    else if (Main.ignoreErrors)
                    {
                        try
                        {
                            NetMessage.RecieveBytes(this.ReadBuffer, streamLength, 256);
                        }
                        catch
                        {
                        }
                    }
                    else
                        NetMessage.RecieveBytes(this.ReadBuffer, streamLength, 256);
                }
                this.IsReading = false;
            }
            catch (Exception ex)
            {
                try
                {
                    using (StreamWriter streamWriter = new StreamWriter("client-crashlog.txt", true))
                    {
                        streamWriter.WriteLine((object)DateTime.Now);
                        streamWriter.WriteLine((object)ex);
                        streamWriter.WriteLine("");
                    }
                }
                catch
                {
                }
                Netplay.disconnect = true;
            }
        }
    }
}
