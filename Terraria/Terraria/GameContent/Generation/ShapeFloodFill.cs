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
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.World.Generation;

namespace Terraria.GameContent.Generation
{
    internal class ShapeFloodFill : GenShape
    {
        private int _maximumActions;

        public ShapeFloodFill(int maximumActions = 100)
        {
            _maximumActions = maximumActions;
        }

        public override bool Perform(Point origin, GenAction action)
        {
            Queue<Point> queue = new Queue<Point>();
            HashSet<Point16> hashSet = new HashSet<Point16>();
            queue.Enqueue(origin);

            int num = _maximumActions;
            while (queue.Count > 0 && num > 0)
            {
                Point point = queue.Dequeue();
                if (!hashSet.Contains(new Point16(point.X, point.Y)) && UnitApply(action, origin, point.X, point.Y))
                {
                    hashSet.Add(new Point16(point));
                    --num;
                    if (point.X + 1 < Main.maxTilesX - 1)
                        queue.Enqueue(new Point(point.X + 1, point.Y));
                    if (point.X - 1 >= 1)
                        queue.Enqueue(new Point(point.X - 1, point.Y));
                    if (point.Y + 1 < Main.maxTilesY - 1)
                        queue.Enqueue(new Point(point.X, point.Y + 1));
                    if (point.Y - 1 >= 1)
                        queue.Enqueue(new Point(point.X, point.Y - 1));
                }
            }

            while (queue.Count > 0)
            {
                Point point = queue.Dequeue();
                if (!hashSet.Contains(new Point16(point.X, point.Y)))
                {
                    queue.Enqueue(point);
                    break;
                }
            }

            return queue.Count == 0;
        }
    }
}
