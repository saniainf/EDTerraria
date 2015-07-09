/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
    internal class CustomFlagCondition : AchievementCondition
    {
        private CustomFlagCondition(string name)
            : base(name) { }

        public static AchievementCondition Create(string name)
        {
            return (AchievementCondition)new CustomFlagCondition(name);
        }
    }
}
