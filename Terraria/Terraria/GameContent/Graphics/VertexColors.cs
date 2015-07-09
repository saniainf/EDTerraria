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

namespace Terraria.Graphics
{
    public struct VertexColors
    {
        public Color TopLeftColor;
        public Color TopRightColor;
        public Color BottomLeftColor;
        public Color BottomRightColor;

        public VertexColors(Color color)
        {
            this.TopLeftColor = color;
            this.TopRightColor = color;
            this.BottomRightColor = color;
            this.BottomLeftColor = color;
        }

        public VertexColors(Color topLeft, Color topRight, Color bottomRight, Color bottomLeft)
        {
            this.TopLeftColor = topLeft;
            this.TopRightColor = topRight;
            this.BottomLeftColor = bottomLeft;
            this.BottomRightColor = bottomRight;
        }
    }
}
