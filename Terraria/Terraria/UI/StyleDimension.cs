/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

namespace Terraria.UI
{
    public struct StyleDimension
    {
        public static StyleDimension Fill = new StyleDimension(0.0f, 1f);
        public static StyleDimension Empty = new StyleDimension(0.0f, 0.0f);
        public float Pixels;
        public float Precent;

        public StyleDimension(float pixels, float precent)
        {
            this.Pixels = pixels;
            this.Precent = precent;
        }

        public void Set(float pixels, float precent)
        {
            this.Pixels = pixels;
            this.Precent = precent;
        }

        public float GetValue(float containerSize)
        {
            return this.Pixels + this.Precent * containerSize;
        }
    }
}
