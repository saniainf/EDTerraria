/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

namespace Terraria.Achievements
{
    public class ConditionFloatTracker : AchievementTracker<float>
    {
        public ConditionFloatTracker(float maxValue)
            : base(TrackerType.Float)
        {
            _maxValue = maxValue;
        }

        public ConditionFloatTracker()
            : base(TrackerType.Float) { }

        public override void ReportUpdate() { }

        protected override void Load() { }
    }
}
