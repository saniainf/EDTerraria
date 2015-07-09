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

namespace Terraria.DataStructures
{
    public struct PlacementHook
    {
        public static PlacementHook Empty = new PlacementHook((Func<int, int, int, int, int, int>)null, 0, 0, false);
        public const int Response_AllInvalid = 0;
        public Func<int, int, int, int, int, int> hook;
        public int badReturn;
        public int badResponse;
        public bool processedCoordinates;

        public PlacementHook(Func<int, int, int, int, int, int> _hook, int _badReturn, int _badResponse, bool _processedCoordinates)
        {
            hook = _hook;
            badResponse = _badResponse;
            badReturn = _badReturn;
            processedCoordinates = _processedCoordinates;
        }

        public static bool operator ==(PlacementHook first, PlacementHook second)
        {
            if (first.hook == second.hook && first.badResponse == second.badResponse && first.badReturn == second.badReturn)
                return first.processedCoordinates == second.processedCoordinates;
            return false;
        }

        public static bool operator !=(PlacementHook first, PlacementHook second)
        {
            if (!(first.hook != second.hook) && first.badResponse == second.badResponse && first.badReturn == second.badReturn)
                return first.processedCoordinates != second.processedCoordinates;
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is PlacementHook)
                return this == (PlacementHook)obj;
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
