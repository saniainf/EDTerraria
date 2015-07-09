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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Terraria.Achievements
{
    [JsonObject]
    public abstract class AchievementCondition
    {
        public delegate void AchievementUpdate(AchievementCondition condition);
        public readonly string Name;
        protected IAchievementTracker _tracker;
        [JsonProperty("Completed")]
        private bool _isCompleted;
        public event AchievementUpdate OnComplete;

        public bool IsCompleted
        {
            get { return _isCompleted; }
        }

        protected AchievementCondition(string name)
        {
            Name = name;
        }

        public virtual void Load(JObject state)
        {
            _isCompleted = (bool)state.GetValue("Completed");
        }

        public virtual void Clear()
        {
            _isCompleted = false;
        }

        public virtual void Complete()
        {
            if (_isCompleted)
                return;
            _isCompleted = true;
            if (OnComplete != null)
                OnComplete(this);
        }

        protected virtual IAchievementTracker CreateAchievementTracker()
        {
            return null;
        }

        public IAchievementTracker GetAchievementTracker()
        {
            if (_tracker == null)
                _tracker = CreateAchievementTracker();

            return _tracker;
        }
    }
}
