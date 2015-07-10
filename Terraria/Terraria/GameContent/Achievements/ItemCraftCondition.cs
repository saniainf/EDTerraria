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
using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
    public class ItemCraftCondition : AchievementCondition
    {
        private static Dictionary<short, List<ItemCraftCondition>> _listeners = new Dictionary<short, List<ItemCraftCondition>>();
        private static bool _isListenerHooked = false;
        private const string Identifier = "ITEM_PICKUP";
        private short[] _itemIds;

        private ItemCraftCondition(short itemId)
            : base("ITEM_PICKUP_" + itemId)
        {
            _itemIds = new short[1] { itemId };
            ListenForCraft(this);
        }

        private ItemCraftCondition(short[] itemIds)
            : base("ITEM_PICKUP_" + itemIds[0])
        {
            _itemIds = itemIds;
            ListenForCraft(this);
        }

        private static void ListenForCraft(ItemCraftCondition condition)
        {
            if (!_isListenerHooked)
            {
                AchievementsHelper.OnItemCraft += new AchievementsHelper.ItemCraftEvent(ItemCraftListener);
                _isListenerHooked = true;
            }

            for (int index = 0; index < condition._itemIds.Length; ++index)
            {
                if (!_listeners.ContainsKey(condition._itemIds[index]))
                    _listeners[condition._itemIds[index]] = new List<ItemCraftCondition>();
                _listeners[condition._itemIds[index]].Add(condition);
            }
        }

        private static void ItemCraftListener(short itemId, int count)
        {
            if (!_listeners.ContainsKey(itemId))
                return;
            foreach (AchievementCondition achievementCondition in _listeners[itemId])
                achievementCondition.Complete();
        }

        public static AchievementCondition Create(params short[] items)
        {
            return (AchievementCondition)new ItemCraftCondition(items);
        }

        public static AchievementCondition Create(short item)
        {
            return (AchievementCondition)new ItemCraftCondition(item);
        }

        public static AchievementCondition[] CreateMany(params short[] items)
        {
            AchievementCondition[] achievementConditionArray = new AchievementCondition[items.Length];
            for (int index = 0; index < items.Length; ++index)
                achievementConditionArray[index] = (AchievementCondition)new ItemCraftCondition(items[index]);
            return achievementConditionArray;
        }
    }
}
