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
using System.Collections.Generic;
using Terraria;
using Terraria.World.Generation;

namespace Terraria.GameContent.Generation
{
    internal class ShapeBranch : GenShape
    {
        private Point _offset;
        private List<Point> _endPoints;

        public ShapeBranch()
        {
            _offset = new Point(10, -5);
        }

        public ShapeBranch(Point offset)
        {
            _offset = offset;
        }

        public ShapeBranch(double angle, double distance)
        {
            _offset = new Point((int)(Math.Cos(angle) * distance), (int)(Math.Sin(angle) * distance));
        }

        private bool PerformSegment(Point origin, GenAction action, Point start, Point end, int size)
        {
            size = Math.Max(1, size);
            for (int index1 = -(size >> 1); index1 < size - (size >> 1); ++index1)
            {
                for (int index2 = -(size >> 1); index2 < size - (size >> 1); ++index2)
                {
                    if (!Utils.PlotLine(new Point(start.X + index1, start.Y + index2), end, (Utils.PerLinePoint)((tileX, tileY) =>
                    {
                        if (!UnitApply(action, origin, tileX, tileY))
                            return !_quitOnFail;
                        return true;
                    }), false))
                        return false;
                }
            }
            return true;
        }

        public override bool Perform(Point origin, GenAction action)
        {
            float num1 = new Vector2((float)_offset.X, (float)_offset.Y).Length();
            int size = (int)(num1 / 6.0);
            if (_endPoints != null)
                _endPoints.Add(new Point(origin.X + _offset.X, origin.Y + _offset.Y));
            if (!PerformSegment(origin, action, origin, new Point(origin.X + _offset.X, origin.Y + _offset.Y), size))
                return false;

            int num2 = (int)(num1 / 8.0);
            for (int index = 0; index < num2; ++index)
            {
                float num3 = (float)((index + 1.0) / num2 + 1.0);
                Point point1 = new Point((int)(num3 * _offset.X), (int)(num3 * _offset.Y));
                Vector2 spinningpoint = new Vector2((float)(this._offset.X - point1.X), (float)(_offset.Y - point1.Y));
                spinningpoint = Utils.RotatedBy(spinningpoint, (GenBase._random.NextDouble() * 0.5 + 1.0) * (GenBase._random.Next(2) == 0 ? -1.0 : 1.0), new Vector2()) * 0.75f;

                Point point2 = new Point((int)spinningpoint.X + point1.X, (int)spinningpoint.Y + point1.Y);
                if (_endPoints != null)
                    _endPoints.Add(new Point(point2.X + origin.X, point2.Y + origin.Y));
                if (!PerformSegment(origin, action, new Point(point1.X + origin.X, point1.Y + origin.Y), new Point(point2.X + origin.X, point2.Y + origin.Y), size - 1))
                    return false;
            }
            return true;
        }

        public ShapeBranch OutputEndpoints(List<Point> endpoints)
        {
            _endPoints = endpoints;
            return this;
        }
    }
}
