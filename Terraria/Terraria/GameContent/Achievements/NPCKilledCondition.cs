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
    internal class NPCKilledCondition : AchievementCondition
    {
        private static Dictionary<short, List<NPCKilledCondition>> _listeners = new Dictionary<short, List<NPCKilledCondition>>();
        private static bool _isListenerHooked = false;
        private const string Identifier = "NPC_KILLED";
        private short[] _npcIds;

        private NPCKilledCondition(short npcId)
            : base("NPC_KILLED_" + npcId)
        {
            _npcIds = new short[1] { npcId };
            ListenForPickup(this);
        }

        private NPCKilledCondition(short[] npcIds)
            : base("NPC_KILLED_" + npcIds[0])
        {
            _npcIds = npcIds;
            ListenForPickup(this);
        }

        private static void ListenForPickup(NPCKilledCondition condition)
        {
            if (!_isListenerHooked)
            {
                AchievementsHelper.OnNPCKilled += new AchievementsHelper.NPCKilledEvent(NPCKilledListener);
                _isListenerHooked = true;
            }

            for (int index = 0; index < condition._npcIds.Length; ++index)
            {
                if (!_listeners.ContainsKey(condition._npcIds[index]))
                    _listeners[condition._npcIds[index]] = new List<NPCKilledCondition>();
                _listeners[condition._npcIds[index]].Add(condition);
            }
        }

        private static void NPCKilledListener(Player player, short npcId)
        {
            if (player.whoAmI != Main.myPlayer || !_listeners.ContainsKey(npcId))
                return;

            foreach (AchievementCondition achievementCondition in _listeners[npcId])
                achievementCondition.Complete();
        }

        public static AchievementCondition Create(params short[] npcIds)
        {
            return (AchievementCondition)new NPCKilledCondition(npcIds);
        }

        public static AchievementCondition Create(short npcId)
        {
            return (AchievementCondition)new NPCKilledCondition(npcId);
        }

        public static AchievementCondition[] CreateMany(params short[] npcs)
        {
            AchievementCondition[] achievementConditionArray = new AchievementCondition[npcs.Length];
            for (int index = 0; index < npcs.Length; ++index)
                achievementConditionArray[index] = (AchievementCondition)new NPCKilledCondition(npcs[index]);
            return achievementConditionArray;
        }
    }
}
