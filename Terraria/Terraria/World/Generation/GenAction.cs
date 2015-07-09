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
    internal abstract class GenAction : GenBase
    {
        private bool _returnFalseOnFailure = true;
        public GenAction NextAction;
        public ShapeData OutputData;

        public abstract bool Apply(Point origin, int x, int y, params object[] args);

        protected bool UnitApply(Point origin, int x, int y, params object[] args)
        {
            if (this.OutputData != null)
                this.OutputData.Add(x - origin.X, y - origin.Y);
            if (this.NextAction != null)
                return this.NextAction.Apply(origin, x, y, args);
            return true;
        }

        public GenAction IgnoreFailures()
        {
            this._returnFalseOnFailure = false;
            return this;
        }

        protected bool Fail()
        {
            return !this._returnFalseOnFailure;
        }

        public GenAction Output(ShapeData data)
        {
            this.OutputData = data;
            return this;
        }
    }
}
