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
using System.Collections.Generic;

namespace Terraria.Net
{
    public class LegacyNetBufferPool
    {
        private static object bufferLock = new object();
        private static Queue<byte[]> SmallBufferQueue = new Queue<byte[]>();
        private static Queue<byte[]> MediumBufferQueue = new Queue<byte[]>();
        private static Queue<byte[]> LargeBufferQueue = new Queue<byte[]>();
        private const int SMALL_BUFFER_SIZE = 32;
        private const int MEDIUM_BUFFER_SIZE = 256;
        private const int LARGE_BUFFER_SIZE = 16384;

        public static byte[] RequestBuffer(int size)
        {
            lock (bufferLock)
            {
                if (size <= 32)
                {
                    if (SmallBufferQueue.Count == 0)
                        return new byte[32];
                    return SmallBufferQueue.Dequeue();
                }
                if (size <= 256)
                {
                    if (MediumBufferQueue.Count == 0)
                        return new byte[256];
                    return MediumBufferQueue.Dequeue();
                }
                if (size > 16384)
                    return new byte[size];
                if (LargeBufferQueue.Count == 0)
                    return new byte[16384];
                return LargeBufferQueue.Dequeue();
            }
        }

        public static byte[] RequestBuffer(byte[] data, int offset, int size)
        {
            byte[] numArray = RequestBuffer(size);
            Buffer.BlockCopy(data, offset, numArray, 0, size);
            return numArray;
        }

        public static void ReturnBuffer(byte[] buffer)
        {
            int length = buffer.Length;
            lock (bufferLock)
            {
                if (length <= 32)
                    SmallBufferQueue.Enqueue(buffer);
                else if (length <= 256)
                {
                    MediumBufferQueue.Enqueue(buffer);
                }
                else
                {
                    if (length > 16384)
                        return;
                    LargeBufferQueue.Enqueue(buffer);
                }
            }
        }

        public static void PrintBufferSizes()
        {
            lock (bufferLock)
            {
                Console.WriteLine("SmallBufferQueue.Count: " + SmallBufferQueue.Count);
                Console.WriteLine("MediumBufferQueue.Count: " + MediumBufferQueue.Count);
                Console.WriteLine("LargeBufferQueue.Count: " + LargeBufferQueue.Count);
                Console.WriteLine("");
            }
        }
    }
}
