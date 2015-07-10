/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
    internal class CustomFloatCondition : AchievementCondition
    {
        [JsonProperty("Value")]
        private float _value;
        private float _maxValue;

        public float Value
        {
            get { return _value; }
            set
            {
                float num = Utils.Clamp<float>(value, 0f, this._maxValue);
                if (_tracker != null)
                    ((ConditionFloatTracker)_tracker).SetValue(num, true);
                _value = num;
                if (_value == _maxValue)
                    Complete();
            }
        }

        private CustomFloatCondition(string name, float maxVal)
            : base(name)
        {
            _maxValue = maxVal;
            _value = 0f;
        }

        public override void Clear()
        {
            _value = 0f;
            base.Clear();
        }

        public override void Load(JObject state)
        {
            base.Load(state);
            _value = (float)state.GetValue("Value");
            if (this._tracker != null)
                ((ConditionFloatTracker)_tracker).SetValue(_value, false);
        }

        protected override IAchievementTracker CreateAchievementTracker()
        {
            return new ConditionFloatTracker(_maxValue);
        }

        public static AchievementCondition Create(string name, float maxValue)
        {
            return new CustomFloatCondition(name, maxValue);
        }
    }
}