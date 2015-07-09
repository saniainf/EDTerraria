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

namespace Terraria.DataStructures
{
    public struct DrillDebugDraw
    {
        public Vector2 point;
        public Color color;

        public DrillDebugDraw(Vector2 p, Color c)
        {
            point = p;
            color = c;
        }
    }
}
