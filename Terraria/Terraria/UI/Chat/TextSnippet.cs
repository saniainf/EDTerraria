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

namespace Terraria.UI.Chat
{
    public class TextSnippet
    {
        public Color Color = Color.White;
        public float Scale = 1f;
        public string Text;
        public string TextOriginal;
        public bool CheckForHover;
        public bool DeleteWhole;

        public TextSnippet(string text = "")
        {
            this.Text = text;
            this.TextOriginal = text;
        }

        public TextSnippet(string text, Color color, float scale = 1f)
        {
            this.Text = text;
            this.TextOriginal = text;
            this.Color = color;
            this.Scale = scale;
        }

        public virtual void Update()
        {
        }

        public virtual void OnHover()
        {
        }

        public virtual void OnClick()
        {
        }

        public virtual Color GetVisibleColor()
        {
            return ChatManager.WaveColor(this.Color);
        }

        public virtual bool UniqueDraw(bool justCheckingString, out Vector2 size, SpriteBatch spriteBatch, Vector2 position = default(Vector2), Color color = default(Color), float scale = 1f)
        {
            size = Vector2.Zero;
            return false;
        }
    }
}
