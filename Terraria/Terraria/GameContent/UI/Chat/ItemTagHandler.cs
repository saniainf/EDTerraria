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
using Terraria.GameContent.UI;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Chat
{
    internal class ItemTagHandler : ITagHandler
    {
        TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
        {
            Item obj = new Item();
            int result1;
            if (int.TryParse(text, out result1))
                obj.netDefaults(result1);
            else
                obj.SetDefaults(text);
            if (obj.type <= 0)
                return new TextSnippet(text);
            obj.stack = 1;
            if (options != null)
            {
                string[] strArray = options.Split(',');
                for (int index = 0; index < strArray.Length; ++index)
                {
                    if (strArray[index].Length != 0)
                    {
                        switch (strArray[index][0])
                        {
                            case 'p':
                                int result2;
                                if (int.TryParse(strArray[index].Substring(1), out result2))
                                {
                                    obj.Prefix((int)(byte)Utils.Clamp<int>(result2, 0, 84));
                                    continue;
                                }
                                continue;
                            case 's':
                            case 'x':
                                int result3;
                                if (int.TryParse(strArray[index].Substring(1), out result3))
                                {
                                    obj.stack = Utils.Clamp<int>(result3, 1, obj.maxStack);
                                    continue;
                                }
                                continue;
                            default:
                                continue;
                        }
                    }
                }
            }
            string str = "";
            if (obj.stack > 1)
                str = " (" + (object)obj.stack + ")";
            ItemTagHandler.ItemSnippet itemSnippet = new ItemTagHandler.ItemSnippet(obj);
            itemSnippet.Text = "[" + obj.AffixName() + str + "]";
            itemSnippet.CheckForHover = true;
            itemSnippet.DeleteWhole = true;
            return (TextSnippet)itemSnippet;
        }

        public static string GenerateTag(Item I)
        {
            string str = "[i";
            if ((int)I.prefix != 0)
                str = str + (object)"/p" + (string)(object)I.prefix;
            if (I.stack != 1)
                str = str + (object)"/s" + (string)(object)I.stack;
            return string.Concat(new object[4]
      {
        (object) str,
        (object) ":",
        (object) I.netID,
        (object) "]"
      });
        }

        private class ItemSnippet : TextSnippet
        {
            private Item _item;

            public ItemSnippet(Item item)
                : base("")
            {
                this._item = item;
                this.Color = ItemRarity.GetColor(item.rare);
            }

            public override void OnHover()
            {
                Main.toolTip = this._item.Clone();
                Main.instance.MouseText(this._item.name, this._item.rare, (byte)0);
            }

            public override bool UniqueDraw(bool justCheckingString, out Vector2 size, SpriteBatch spriteBatch, Vector2 position = default(Vector2), Color color = default(Color), float scale = 1f)
            {
                float num1 = 1f;
                float num2 = 1f;
                if (Main.netMode != 2 && !Main.dedServ)
                {
                    Texture2D texture2D = Main.itemTexture[this._item.type];
                    Rectangle rectangle = Main.itemAnimations[this._item.type] == null ? Utils.Frame(texture2D, 1, 1, 0, 0) : Main.itemAnimations[this._item.type].GetFrame(texture2D);
                    if (rectangle.Height > 32)
                        num2 = 32f / (float)rectangle.Height;
                }
                float num3 = num2 * scale;
                float num4 = num1 * num3;
                if ((double)num4 > 0.75)
                    num4 = 0.75f;
                if (!justCheckingString && color != Color.Black)
                {
                    float num5 = Main.inventoryScale;
                    Main.inventoryScale = scale * num4;
                    ItemSlot.Draw(spriteBatch, ref this._item, 14, position - new Vector2(10f) * scale * num4, Color.White);
                    Main.inventoryScale = num5;
                }
                size = new Vector2(32f) * scale * num4;
                return true;
            }
        }
    }
}
