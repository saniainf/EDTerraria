/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.World.Generation;

namespace Terraria.GameContent.UI.States
{
    internal class UIWorldLoad : UIState
    {
        private UIGenProgressBar _progressBar = new UIGenProgressBar();
        private UIHeader _progressMessage = new UIHeader();
        private GenerationProgress _progress;

        public UIWorldLoad(GenerationProgress progress)
        {
            this._progressBar.Top.Pixels = 370f;
            this._progressBar.HAlign = 0.5f;
            this._progressBar.VAlign = 0.0f;
            this._progressBar.Recalculate();
            this._progressMessage.CopyStyle((UIElement)this._progressBar);
            this._progressMessage.Top.Pixels -= 70f;
            this._progressMessage.Recalculate();
            this._progress = progress;
            this.Append((UIElement)this._progressBar);
            this.Append((UIElement)this._progressMessage);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            this._progressBar.SetProgress(this._progress.TotalProgress, this._progress.Value);
            this._progressMessage.Text = this._progress.Message;
        }

        public string GetStatusText()
        {
            return string.Format("{0:0.0%} - " + this._progress.Message + " - {1:0.0%}", (object)this._progress.TotalProgress, (object)this._progress.Value);
        }
    }
}
