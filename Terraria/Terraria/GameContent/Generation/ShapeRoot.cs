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
using System;
using Terraria;
using Terraria.World.Generation;

namespace Terraria.GameContent.Generation
{
    internal class ShapeRoot : GenShape
    {
        private float _angle;
        private float _startingSize;
        private float _endingSize;
        private float _distance;

        public ShapeRoot(float angle, float distance = 10f, float startingSize = 4f, float endingSize = 1f)
        {
            this._angle = angle;
            this._distance = distance;
            this._startingSize = startingSize;
            this._endingSize = endingSize;
        }

        private bool DoRoot(Point origin, GenAction action, float angle, float distance, float startingSize)
        {
            float num1 = (float)origin.X;
            float num2 = (float)origin.Y;
            for (float num3 = 0.0f; (double)num3 < (double)distance * 0.850000023841858; ++num3)
            {
                float amount = num3 / distance;
                float num4 = MathHelper.Lerp(startingSize, this._endingSize, amount);
                num1 += (float)Math.Cos((double)angle);
                num2 += (float)Math.Sin((double)angle);
                angle += (float)((double)Utils.NextFloat(GenBase._random) - 0.5 + (double)Utils.NextFloat(GenBase._random) * ((double)this._angle - 1.57079637050629) * 0.100000001490116 * (1.0 - (double)amount));
                angle = (float)((double)angle * 0.400000005960464 + 0.449999988079071 * (double)MathHelper.Clamp(angle, this._angle - (float)(2.0 * (1.0 - 0.5 * (double)amount)), this._angle + (float)(2.0 * (1.0 - 0.5 * (double)amount))) + (double)MathHelper.Lerp(this._angle, 1.570796f, amount) * 0.150000005960464);
                for (int index1 = 0; index1 < (int)num4; ++index1)
                {
                    for (int index2 = 0; index2 < (int)num4; ++index2)
                    {
                        if (!this.UnitApply(action, origin, (int)num1 + index1, (int)num2 + index2) && this._quitOnFail)
                            return false;
                    }
                }
            }
            return true;
        }

        public override bool Perform(Point origin, GenAction action)
        {
            return this.DoRoot(origin, action, this._angle, this._distance, this._startingSize);
        }
    }
}
