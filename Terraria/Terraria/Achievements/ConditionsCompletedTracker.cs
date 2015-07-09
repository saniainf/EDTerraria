/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using System;
using System.Collections.Generic;

namespace Terraria.Achievements
{
    public class ConditionsCompletedTracker : ConditionIntTracker
    {
        private List<AchievementCondition> _conditions = new List<AchievementCondition>();

        public void AddCondition(AchievementCondition condition)
        {
            ++_maxValue;
            condition.OnComplete += new AchievementCondition.AchievementUpdate(OnConditionCompleted);
            _conditions.Add(condition);
        }

        private void OnConditionCompleted(AchievementCondition condition)
        {
            SetValue(Math.Min(_value + 1, _maxValue), true);
        }

        protected override void Load()
        {
            for (int index = 0; index < _conditions.Count; ++index)
            {
                if (_conditions[index].IsCompleted)
                    ++_value;
            }
        }
    }
}
