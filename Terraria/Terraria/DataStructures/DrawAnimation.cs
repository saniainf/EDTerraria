/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Terraria.DataStructures
{
    public class DrawAnimation
    {
        public int Frame;
        public int FrameCount;
        public int TicksPerFrame;
        public int FrameCounter;

        public virtual void Update() { }

        public virtual Rectangle GetFrame(Texture2D texture)
        {
            return Utils.Frame(texture, 1, 1, 0, 0);
        }
    }
}
