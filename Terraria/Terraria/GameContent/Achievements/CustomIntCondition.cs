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
    internal class CustomIntCondition : AchievementCondition
    {
        [JsonProperty("Value")]
        private int _value;
        private int _maxValue;
        public int Value
        {
            get
            {
                return this._value;
            }
            set
            {
                int num = Utils.Clamp<int>(value, 0, this._maxValue);
                if (this._tracker != null)
                {
                    ((ConditionIntTracker)this._tracker).SetValue(num, true);
                }
                this._value = num;
                if (this._value == this._maxValue)
                {
                    this.Complete();
                }
            }
        }
        private CustomIntCondition(string name, int maxValue)
            : base(name)
        {
            this._maxValue = maxValue;
            this._value = 0;
        }
        public override void Clear()
        {
            this._value = 0;
            base.Clear();
        }
        public override void Load(JObject state)
        {
            base.Load(state);
            this._value = (int)state.GetValue("Value");
            if (this._tracker != null)
            {
                ((ConditionIntTracker)this._tracker).SetValue(this._value, false);
            }
        }
        protected override IAchievementTracker CreateAchievementTracker()
        {
            return new ConditionIntTracker(this._maxValue);
        }
        public static AchievementCondition Create(string name, int maxValue)
        {
            return new CustomIntCondition(name, maxValue);
        }
    }
}