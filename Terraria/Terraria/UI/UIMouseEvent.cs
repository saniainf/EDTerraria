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

namespace Terraria.UI
{
    public class UIMouseEvent : UIEvent
    {
        public readonly Vector2 MousePosition;

        public UIMouseEvent(UIElement target, Vector2 mousePosition)
            : base(target)
        {
            MousePosition = mousePosition;
        }
    }
}
