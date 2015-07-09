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
    public abstract class AchievementTracker<T> : IAchievementTracker
    {
        protected T _value;
        protected T _maxValue;
        protected string _name;
        private TrackerType _type;

        public T Value
        {
            get { return _value; }
        }

        public T MaxValue
        {
            get { return _maxValue; }
        }

        public abstract void ReportUpdate();
        protected abstract void Load();
        protected void OnComplete() { }

        protected AchievementTracker(TrackerType type)
        {
            _type = type;
        }

        void IAchievementTracker.ReportAs(string name)
        {
            _name = name;
        }

        TrackerType IAchievementTracker.GetTrackerType()
        {
            return _type;
        }

        void IAchievementTracker.Clear()
        {
            SetValue(default(T), true);
        }

        void IAchievementTracker.Load()
        {
            Load();
        }

        public void SetValue(T newValue, bool reportUpdate = true)
        {
            if (newValue.Equals(_value))
                return;

            _value = newValue;
            if (reportUpdate)
                ReportUpdate();
            if (_value.Equals(_maxValue))
                OnComplete();
        }
    }
}
