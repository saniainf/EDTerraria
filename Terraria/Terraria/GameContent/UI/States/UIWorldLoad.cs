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
            _progressBar.Top.Pixels = 370f;
            _progressBar.HAlign = 0.5f;
            _progressBar.VAlign = 0.0f;
            _progressBar.Recalculate();
            _progressMessage.CopyStyle(_progressBar);
            _progressMessage.Top.Pixels -= 70f;
            _progressMessage.Recalculate();
            _progress = progress;
            Append(_progressBar);
            Append(_progressMessage);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            _progressBar.SetProgress(_progress.TotalProgress, _progress.Value);
            _progressMessage.Text = _progress.Message;
        }

        public string GetStatusText()
        {
            return string.Format("{0:0.0%} - " + _progress.Message + " - {1:0.0%}", _progress.TotalProgress, _progress.Value);
        }
    }
}
