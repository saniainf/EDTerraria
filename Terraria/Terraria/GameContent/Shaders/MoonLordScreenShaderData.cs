/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using Terraria;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Shaders
{
    internal class MoonLordScreenShaderData : ScreenShaderData
    {
        private int _moonLordIndex = -1;

        public MoonLordScreenShaderData(string passName)
            : base(passName) { }

        private void UpdateMoonLordIndex()
        {
            if (_moonLordIndex >= 0 && Main.npc[_moonLordIndex].active && Main.npc[_moonLordIndex].type == 398)
                return;

            int num = -1;
            for (int index = 0; index < Main.npc.Length; ++index)
            {
                if (Main.npc[index].active && Main.npc[index].type == 398)
                {
                    num = index;
                    break;
                }
            }
            _moonLordIndex = num;
        }

        public override void Apply()
        {
            UpdateMoonLordIndex();
            if (_moonLordIndex != -1)
                UseTargetPosition(Main.npc[_moonLordIndex].Center);
            base.Apply();
        }
    }
}
