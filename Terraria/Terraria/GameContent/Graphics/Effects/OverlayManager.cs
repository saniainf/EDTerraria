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
using System;
using System.Collections.Generic;

namespace Terraria.Graphics.Effects
{
    internal class OverlayManager : EffectManager<Overlay>
    {
        private LinkedList<Overlay>[] _activeOverlays = new LinkedList<Overlay>[Enum.GetNames(typeof(EffectPriority)).Length];
        private const float OPACITY_RATE = 0.05f;

        public OverlayManager()
        {
            for (int index = 0; index < _activeOverlays.Length; ++index)
                _activeOverlays[index] = new LinkedList<Overlay>();
        }

        public override void OnActivate(Overlay overlay, Vector2 position)
        {
            LinkedList<Overlay> linkedList = _activeOverlays[(int)overlay.Priority];
            if (overlay.Mode == OverlayMode.FadeIn || overlay.Mode == OverlayMode.Active)
                return;

            if (overlay.Mode == OverlayMode.FadeOut)
                linkedList.Remove(overlay);
            else
                overlay.Opacity = 0.0f;

            if (linkedList.Count != 0)
            {
                foreach (Overlay overlay1 in linkedList)
                    overlay1.Mode = OverlayMode.FadeOut;
            }
            linkedList.AddLast(overlay);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Overlay overlay1 = null;
            for (int index = 0; index < _activeOverlays.Length; ++index)
            {
                foreach (Overlay overlay2 in _activeOverlays[index])
                {
                    if (overlay2.Mode == OverlayMode.Active)
                        overlay1 = overlay2;
                }
            }

            LinkedListNode<Overlay> next;
            for (int index = 0; index < _activeOverlays.Length; ++index)
            {
                for (LinkedListNode<Overlay> node = _activeOverlays[index].First; node != null; node = next)
                {
                    Overlay overlay2 = node.Value;
                    overlay2.Draw(spriteBatch);
                    next = node.Next;
                    switch (overlay2.Mode)
                    {
                        case OverlayMode.FadeIn:
                            overlay2.Opacity += 0.05f;
                            if (overlay2.Opacity >= 1.0)
                            {
                                overlay2.Opacity = 1f;
                                overlay2.Mode = OverlayMode.Active;
                                break;
                            }
                            break;
                        case OverlayMode.Active:
                            if (overlay1 != null && overlay2 != overlay1)
                            {
                                overlay2.Opacity = Math.Max(0.0f, overlay2.Opacity - 0.05f);
                                break;
                            }
                            overlay2.Opacity = Math.Min(1f, overlay2.Opacity + 0.05f);
                            break;
                        case OverlayMode.FadeOut:
                            overlay2.Opacity -= 0.05f;
                            if (overlay2.Opacity <= 0.0)
                            {
                                overlay2.Opacity = 0.0f;
                                overlay2.Mode = OverlayMode.Inactive;
                                _activeOverlays[index].Remove(node);
                                break;
                            }
                            break;
                    }
                }
            }
        }
    }
}
