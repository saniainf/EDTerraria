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
using Terraria;
using Terraria.Achievements;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Chat
{
    internal class AchievementTagHandler : ITagHandler
    {
        TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
        {
            Achievement achievement = Main.Achievements.GetAchievement(text);
            if (achievement == null)
                return new TextSnippet(text);

            return new AchievementTagHandler.AchievementSnippet(achievement);
        }

        public static string GenerateTag(Achievement achievement)
        {
            return "[a:" + achievement.Name + "]";
        }

        private class AchievementSnippet : TextSnippet
        {
            private Achievement _achievement;

            public AchievementSnippet(Achievement achievement)
                : base(achievement.FriendlyName, Color.LightBlue, 1f)
            {
                CheckForHover = true;
                _achievement = achievement;
            }

            public override void OnClick()
            {
                IngameOptions.Close();
                AchievementsUI.OpenAndGoto(_achievement);
            }
        }
    }
}
