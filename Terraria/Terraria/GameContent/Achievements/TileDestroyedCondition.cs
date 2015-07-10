/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using System.Collections.Generic;
using Terraria;
using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
    internal class TileDestroyedCondition : AchievementCondition
    {
        private static Dictionary<ushort, List<TileDestroyedCondition>> _listeners = new Dictionary<ushort, List<TileDestroyedCondition>>();
        private static bool _isListenerHooked = false;
        private const string Identifier = "TILE_DESTROYED";
        private ushort[] _tileIds;

        private TileDestroyedCondition(ushort[] tileIds)
            : base("TILE_DESTROYED_" + tileIds[0])
        {
            _tileIds = tileIds;
            ListenForDestruction(this);
        }

        private static void ListenForDestruction(TileDestroyedCondition condition)
        {
            if (!_isListenerHooked)
            {
                AchievementsHelper.OnTileDestroyed += new AchievementsHelper.TileDestroyedEvent(TileDestroyedListener);
                _isListenerHooked = true;
            }

            for (int index = 0; index < condition._tileIds.Length; ++index)
            {
                if (!_listeners.ContainsKey(condition._tileIds[index]))
                    _listeners[condition._tileIds[index]] = new List<TileDestroyedCondition>();
                _listeners[condition._tileIds[index]].Add(condition);
            }
        }

        private static void TileDestroyedListener(Player player, ushort tileId)
        {
            if (player.whoAmI != Main.myPlayer || !_listeners.ContainsKey(tileId))
                return;

            foreach (AchievementCondition achievementCondition in _listeners[tileId])
                achievementCondition.Complete();
        }

        public static AchievementCondition Create(params ushort[] tileIds)
        {
            return (AchievementCondition)new TileDestroyedCondition(tileIds);
        }
    }
}
