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
using System.Diagnostics;

namespace Terraria.Utilities
{
    public class MultiTimer
    {
        private int ticksBetweenPrint = 100;
        private Stopwatch timer = new Stopwatch();
        private Dictionary<string, double> timerDataMap = new Dictionary<string, double>();
        private int ticksElapsedForPrint;

        public MultiTimer()
        {
        }

        public MultiTimer(int ticksBetweenPrint)
        {
            this.ticksBetweenPrint = ticksBetweenPrint;
        }

        public void Start()
        {
            this.timer.Reset();
            this.timer.Start();
        }

        public void Record(string key)
        {
            double totalMilliseconds = this.timer.Elapsed.TotalMilliseconds;
            if (!this.timerDataMap.ContainsKey(key))
            {
                this.timerDataMap.Add(key, totalMilliseconds);
            }
            else
            {
                Dictionary<string, double> dictionary;
                string index;
                (dictionary = this.timerDataMap)[index = key] = dictionary[index] + totalMilliseconds;
            }
            this.timer.Restart();
        }

        public bool StopAndPrint()
        {
            ++this.ticksElapsedForPrint;
            if (this.ticksElapsedForPrint != this.ticksBetweenPrint)
                return false;
            this.ticksElapsedForPrint = 0;
            Console.WriteLine("Average elapsed time: ");
            double num = 0.0;
            foreach (KeyValuePair<string, double> keyValuePair in this.timerDataMap)
            {
                Console.WriteLine(string.Concat(new object[4]
        {
          (object) keyValuePair.Key,
          (object) " : ",
          (object) (float) (keyValuePair.Value / (double) this.ticksBetweenPrint),
          (object) "ms"
        }));
                num += keyValuePair.Value;
            }
            List<string> list = new List<string>((IEnumerable<string>)this.timerDataMap.Keys);
            for (int index = 0; index < list.Count; ++index)
                this.timerDataMap[list[index]] = 0.0;
            Console.WriteLine("Total : " + (object)(float)(num / (double)this.ticksBetweenPrint) + "ms");
            return true;
        }
    }
}
