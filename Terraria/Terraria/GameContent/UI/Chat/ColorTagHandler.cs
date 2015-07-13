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
using System;
using System.Globalization;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Chat
{
    internal class ColorTagHandler : ITagHandler
    {
        TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
        {
            TextSnippet textSnippet = new TextSnippet(text);
            int result;
            if (!int.TryParse(options, NumberStyles.AllowHexSpecifier, (IFormatProvider)CultureInfo.InvariantCulture, out result))
                return textSnippet;

            textSnippet.Color = new Color(result >> 16 & 255, result >> 8 & 255, result & 255);
            return textSnippet;
        }
    }
}
