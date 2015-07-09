/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using Microsoft.Xna.Framework;

namespace Terraria.World.Generation
{
    internal abstract class GenSearch : GenBase
    {
        public static Point NOT_FOUND = new Point(int.MaxValue, int.MaxValue);
        private bool _requireAll = true;
        private GenCondition[] _conditions;

        public GenSearch Conditions(params GenCondition[] conditions)
        {
            this._conditions = conditions;
            return this;
        }

        public abstract Point Find(Point origin);

        protected bool Check(int x, int y)
        {
            for (int index = 0; index < this._conditions.Length; ++index)
            {
                if (this._requireAll ^ this._conditions[index].IsValid(x, y))
                    return !this._requireAll;
            }
            return this._requireAll;
        }

        public GenSearch RequireAll(bool mode)
        {
            this._requireAll = mode;
            return this;
        }
    }
}
