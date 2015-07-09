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
            get
            {
                return this._value;
            }
            set
            {
                float num = Utils.Clamp<float>(value, 0f, this._maxValue);
                if (this._tracker != null)
                {
                    ((ConditionFloatTracker)this._tracker).SetValue(num, true);
                }
                this._value = num;
                if (this._value == this._maxValue)
                {
                    this.Complete();
                }
            }
        }
        private CustomFloatCondition(string name, float maxValue)
            : base(name)
        {
            this._maxValue = maxValue;
            this._value = 0f;
        }
        public override void Clear()
        {
            this._value = 0f;
            base.Clear();
        }
        public override void Load(JObject state)
        {
            base.Load(state);
            this._value = (float)state.GetValue("Value");
            if (this._tracker != null)
            {
                ((ConditionFloatTracker)this._tracker).SetValue(this._value, false);
            }
        }
        protected override IAchievementTracker CreateAchievementTracker()
        {
            return new ConditionFloatTracker(this._maxValue);
        }
        public static AchievementCondition Create(string name, float maxValue)
        {
            return new CustomFloatCondition(name, maxValue);
        }
    }
}