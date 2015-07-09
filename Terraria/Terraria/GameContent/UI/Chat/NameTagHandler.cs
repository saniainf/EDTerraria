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
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Chat
{
    internal class NameTagHandler : ITagHandler
    {
        TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
        {
            return new TextSnippet("<" + text.Replace("\\[", "[").Replace("\\]", "]") + ">", baseColor, 1f);
        }

        public static string GenerateTag(string name)
        {
            return "[n:" + name.Replace("[", "\\[").Replace("]", "\\]") + "]";
        }
    }
}
