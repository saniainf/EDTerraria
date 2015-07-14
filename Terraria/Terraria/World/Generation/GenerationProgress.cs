/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using Terraria;

namespace Terraria.World.Generation
{
    public class GenerationProgress
    {
        private string _message = "";
        public float CurrentPassWeight = 1f;
        private float _value;
        private float _totalProgress;
        public float TotalWeight;

        public string Message
        {
            get { return string.Format(_message, Value); }
            set { _message = value.Replace("%", "{0:0.0%}"); }
        }

        public float Value
        {
            get { return _value; }
            set { _value = Utils.Clamp<float>(value, 0.0f, 1f); }
        }

        public float TotalProgress
        {
            get
            {
                if (TotalWeight == 0.0)
                    return 0.0f;
                return (Value * CurrentPassWeight + _totalProgress) / TotalWeight;
            }
        }

        public void Set(float value)
        {
            Value = value;
        }

        public void Start(float weight)
        {
            CurrentPassWeight = weight;
            _value = 0.0f;
        }

        public void End()
        {
            _totalProgress += CurrentPassWeight;
        }
    }
}
