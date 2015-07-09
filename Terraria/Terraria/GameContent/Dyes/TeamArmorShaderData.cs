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
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Dyes
{
    internal class TeamArmorShaderData : ArmorShaderData
    {
        private static bool isInitialized = false;
        private static ArmorShaderData[] dustShaderData;

        public TeamArmorShaderData(Effect shader, string passName)
            : base(shader, passName)
        {
            if (TeamArmorShaderData.isInitialized)
                return;
            TeamArmorShaderData.isInitialized = true;
            TeamArmorShaderData.dustShaderData = new ArmorShaderData[Main.teamColor.Length];
            for (int index = 1; index < Main.teamColor.Length; ++index)
                TeamArmorShaderData.dustShaderData[index] = new ArmorShaderData(shader, passName).UseColor(Main.teamColor[index]);
            TeamArmorShaderData.dustShaderData[0] = new ArmorShaderData(shader, "Default");
        }

        public override void Apply(Entity entity, DrawData? drawData)
        {
            Player player = entity as Player;
            if (player == null || player.team == 0)
            {
                TeamArmorShaderData.dustShaderData[0].Apply((Entity)player, drawData);
            }
            else
            {
                this.UseColor(Main.teamColor[player.team]);
                base.Apply((Entity)player, drawData);
            }
        }

        public override ArmorShaderData GetSecondaryShader(Entity entity)
        {
            Player player = entity as Player;
            return TeamArmorShaderData.dustShaderData[player.team];
        }
    }
}
