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

namespace Terraria.Graphics.Capture
{
    public class CaptureSettings
    {
        public bool UseScaling = true;
        public bool CaptureEntities = true;
        public CaptureBiome Biome = CaptureBiome.Biomes[0];
        public Rectangle Area;
        public string OutputName;
        public bool CaptureBackground;

        public CaptureSettings()
        {
            DateTime dateTime = DateTime.Now.ToLocalTime();
            OutputName = "Capture " + dateTime.Year.ToString("D4") + "-" + dateTime.Month.ToString("D2") + "-" + dateTime.Day.ToString("D2") + " " + dateTime.Hour.ToString("D2") + "_" + dateTime.Minute.ToString("D2") + "_" + dateTime.Second.ToString("D2");
        }
    }
}
