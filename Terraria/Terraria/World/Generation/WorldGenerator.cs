/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using System.Collections.Generic;
using System.Diagnostics;
using Terraria;
using Terraria.GameContent.UI.States;
using Terraria.UI;

namespace Terraria.World.Generation
{
    internal class WorldGenerator
    {
        private List<GenPass> _passes = new List<GenPass>();
        private float _totalLoadWeight;

        public void Append(GenPass pass)
        {
            this._passes.Add(pass);
            this._totalLoadWeight += pass.Weight;
        }

        public void GenerateWorld(GenerationProgress progress = null)
        {
            Stopwatch stopwatch = new Stopwatch();
            float num = 0.0f;
            foreach (GenPass genPass in this._passes)
                num += genPass.Weight;
            if (progress == null)
                progress = new GenerationProgress();
            progress.TotalWeight = num;
            string str = "";
            Main.MenuUI.SetState((UIState)new UIWorldLoad(progress));
            Main.menuMode = 888;
            foreach (GenPass genPass in this._passes)
            {
                stopwatch.Start();
                progress.Start(genPass.Weight);
                genPass.Apply(progress);
                progress.End();
                str = str + "Pass - " + genPass.Name + " : " + stopwatch.Elapsed.TotalMilliseconds.ToString() + ",\n";
                stopwatch.Reset();
            }
        }
    }
}
