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
            lock (LegacyNetBufferPool.bufferLock)
            {
                if (size <= 32)
                {
                    if (LegacyNetBufferPool.SmallBufferQueue.Count == 0)
                        return new byte[32];
                    return LegacyNetBufferPool.SmallBufferQueue.Dequeue();
                }
                if (size <= 256)
                {
                    if (LegacyNetBufferPool.MediumBufferQueue.Count == 0)
                        return new byte[256];
                    return LegacyNetBufferPool.MediumBufferQueue.Dequeue();
                }
                if (size > 16384)
                    return new byte[size];
                if (LegacyNetBufferPool.LargeBufferQueue.Count == 0)
                    return new byte[16384];
                return LegacyNetBufferPool.LargeBufferQueue.Dequeue();
            }
        }

        public static byte[] RequestBuffer(byte[] data, int offset, int size)
        {
            byte[] numArray = LegacyNetBufferPool.RequestBuffer(size);
            Buffer.BlockCopy((Array)data, offset, (Array)numArray, 0, size);
            return numArray;
        }

        public static void ReturnBuffer(byte[] buffer)
        {
            int length = buffer.Length;
            lock (LegacyNetBufferPool.bufferLock)
            {
                if (length <= 32)
                    LegacyNetBufferPool.SmallBufferQueue.Enqueue(buffer);
                else if (length <= 256)
                {
                    LegacyNetBufferPool.MediumBufferQueue.Enqueue(buffer);
                }
                else
                {
                    if (length > 16384)
                        return;
                    LegacyNetBufferPool.LargeBufferQueue.Enqueue(buffer);
                }
            }
        }

        public static void PrintBufferSizes()
        {
            lock (LegacyNetBufferPool.bufferLock)
            {
                Console.WriteLine("SmallBufferQueue.Count: " + (object)LegacyNetBufferPool.SmallBufferQueue.Count);
                Console.WriteLine("MediumBufferQueue.Count: " + (object)LegacyNetBufferPool.MediumBufferQueue.Count);
                Console.WriteLine("LargeBufferQueue.Count: " + (object)LegacyNetBufferPool.LargeBufferQueue.Count);
                Console.WriteLine("");
            }
        }
    }
}
