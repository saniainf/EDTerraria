/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Concurrent;
using System.Threading;
using Terraria;

namespace Terraria.Graphics
{
    internal static class TextureManager
    {
        private static ConcurrentDictionary<string, Texture2D> _textures = new ConcurrentDictionary<string, Texture2D>();
        private static ConcurrentQueue<LoadPair> _loadQueue = new ConcurrentQueue<LoadPair>();
        private static readonly object _loadThreadLock = new object();
        public static Texture2D BlankTexture;

        public static void Initialize()
        {
            BlankTexture = new Texture2D(Main.graphics.GraphicsDevice, 4, 4);
            ThreadPool.QueueUserWorkItem(new WaitCallback(Run));
        }

        public static Texture2D Load(string name)
        {
            if (_textures.ContainsKey(name))
                return _textures[name];

            Texture2D texture2D = BlankTexture;
            if (name != "")
            {
                if (name != null)
                {
                    try
                    {
                        texture2D = Main.instance.Content.Load<Texture2D>(name);
                    }
                    catch
                    {
                        texture2D = BlankTexture;
                    }
                }
            }

            _textures[name] = texture2D;
            return texture2D;
        }

        public static Ref<Texture2D> Retrieve(string name)
        {
            return new Ref<Texture2D>(Load(name));
        }

        public static void Run(object context)
        {
            bool looping = true;
            Main.instance.Exiting += (EventHandler<EventArgs>)((obj, args) =>
            {
                looping = false;
                if (!Monitor.TryEnter(_loadThreadLock))
                    return;

                Monitor.Pulse(_loadThreadLock);
                Monitor.Exit(_loadThreadLock);
            });

            Monitor.Enter(_loadThreadLock);
            while (looping)
            {
                if (_loadQueue.Count != 0)
                {
                    LoadPair result;
                    if (_loadQueue.TryDequeue(out result))
                        result.TextureRef.Value = Load(result.Path);
                }
                else
                    Monitor.Wait(_loadThreadLock);
            }
            Monitor.Exit(_loadThreadLock);
        }

        private struct LoadPair
        {
            public string Path;
            public Ref<Texture2D> TextureRef;

            public LoadPair(string path, Ref<Texture2D> textureRef)
            {
                Path = path;
                TextureRef = textureRef;
            }
        }
    }
}
