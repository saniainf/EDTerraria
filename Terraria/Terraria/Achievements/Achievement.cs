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
using System.Collections.Generic;

namespace Terraria.Achievements
{
    [JsonObject]
    public class Achievement
    {
        private static int _totalAchievements;
        public readonly int Id = _totalAchievements++;
        [JsonProperty("Conditions")]
        private Dictionary<string, AchievementCondition> _conditions = new Dictionary<string, AchievementCondition>();
        public readonly string Name;
        public readonly string FriendlyName;
        public readonly string Description;
        private AchievementCategory _category;
        private IAchievementTracker _tracker;
        private int _completedCount;

        public delegate void AchievementCompleted(Achievement achievement);
        public event AchievementCompleted OnCompleted;

        public AchievementCategory Category
        {
            get { return _category; }
        }

        public bool HasTracker
        {
            get { return _tracker != null; }
        }

        public bool IsCompleted
        {
            get { return _completedCount == _conditions.Count; }
        }

        public Achievement(string name, string friendlyName, string description)
        {
            Name = name;
            FriendlyName = friendlyName;
            Description = description;
        }

        public IAchievementTracker GetTracker()
        {
            return _tracker;
        }

        public void ClearProgress()
        {
            _completedCount = 0;
            foreach (KeyValuePair<string, AchievementCondition> keyValuePair in _conditions)
                keyValuePair.Value.Clear();

            if (_tracker != null)
                _tracker.Clear();
        }

        public void Load(Dictionary<string, JObject> conditions)
        {
            using (Dictionary<string, JObject>.Enumerator enumerator = conditions.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    KeyValuePair<string, JObject> current = enumerator.Current;
                    AchievementCondition achievementCondition;
                    if (_conditions.TryGetValue(current.Key, out achievementCondition))
                    {
                        achievementCondition.Load(current.Value);
                        if (achievementCondition.IsCompleted)
                            ++_completedCount;
                    }
                }
            }

            if (_tracker != null)
                _tracker.Load();
        }

        public void AddCondition(AchievementCondition condition)
        {
            _conditions[condition.Name] = condition;
            condition.OnComplete += new AchievementCondition.AchievementUpdate(OnConditionComplete);
        }

        private void OnConditionComplete(AchievementCondition condition)
        {
            ++_completedCount;
            if (_completedCount != _conditions.Count)
                return;

            if (OnCompleted != null)
                OnCompleted(this);
        }

        private void UseTracker(IAchievementTracker tracker)
        {
            tracker.ReportAs("STAT_" + Name);
            _tracker = tracker;
        }

        public void UseTrackerFromCondition(string conditionName)
        {
            UseTracker(GetConditionTracker(conditionName));
        }

        public void UseConditionsCompletedTracker()
        {
            ConditionsCompletedTracker completedTracker = new ConditionsCompletedTracker();
            foreach (KeyValuePair<string, AchievementCondition> keyValuePair in _conditions)
                completedTracker.AddCondition(keyValuePair.Value);
            UseTracker((IAchievementTracker)completedTracker);
        }

        public void UseConditionsCompletedTracker(params string[] conditions)
        {
            ConditionsCompletedTracker completedTracker = new ConditionsCompletedTracker();
            for (int index1 = 0; index1 < conditions.Length; ++index1)
            {
                string index2 = conditions[index1];
                completedTracker.AddCondition(_conditions[index2]);
            }
            UseTracker((IAchievementTracker)completedTracker);
        }

        public void ClearTracker()
        {
            _tracker = (IAchievementTracker)null;
        }

        private IAchievementTracker GetConditionTracker(string name)
        {
            return _conditions[name].GetAchievementTracker();
        }

        public void AddConditions(params AchievementCondition[] conditions)
        {
            for (int index = 0; index < conditions.Length; ++index)
                AddCondition(conditions[index]);
        }

        public AchievementCondition GetCondition(string conditionName)
        {
            AchievementCondition achievementCondition;
            if (_conditions.TryGetValue(conditionName, out achievementCondition))
                return achievementCondition;

            return null;
        }

        public void SetCategory(AchievementCategory category)
        {
            _category = category;
        }
    }
}
