using System;
using System.Collections.Generic;

namespace Terraria.DataStructures
{
	class WeightedList<T>
	{
		struct WeightedListItem
		{
			public T value;
			public float weight;

			public WeightedListItem(T value, float weight)
			{
				this.value = value;
				this.weight = weight;
			}
		}

		ulong seed;
		List<WeightedListItem> items;
		float totalWeight;

		void Constructor(int capacity)
		{
			items = new List<WeightedListItem>(capacity);
			seed = Utils.RandomNextSeed((ulong)DateTime.Now.Millisecond);
		}
		public WeightedList()				{ Constructor(32); }
		public WeightedList(int capacity)	{ Constructor(capacity); }

		public void Add(T Item, float Weight)
		{
			if (Item == null || Weight <= 0f)
				return;

			Remove(Item);
			items.Add(new WeightedListItem(Item, Weight));
			totalWeight += Weight;
		}

		public void Add(WeightedList<T> List, bool Overwrite)
		{
			if (List == null)
				return;

			foreach (var item in List.items)
			{
				if (Overwrite || FindIndex(item.value) == -1)
					Add(item.value, item.weight);
			}
		}

		public void Remove(T Item)
		{
			if (Item == null)
				return;

			int index = FindIndex(Item);
			if (index != -1)
			{
				totalWeight -= items[index].weight;
				items[index] = items[items.Count - 1];
				items.RemoveAt(items.Count - 1);
			}
		}

		int FindIndex(T Item)
		{
			if (Item == null)
				return -1;

			for (int i = 0; i < items.Count; ++i)
			{
				if (Item.Equals(items[i].value))
					return i;
			}
			return -1;
		}

		public bool Contains(T Item)
		{
			return FindIndex(Item) != -1;
		}

		public T GetRandom()
		{
			if (items.Count == 0)
				return default(T); // throw new Exception();

			float weight = totalWeight * Utils.RandomFloat(ref seed);
			foreach (var item in items)
			{
				weight -= item.weight;
				if (weight <= 0)
					return item.value;
			}

			return default(T); // Should never happen
		}

		public WeightedList<T> Copy(float WeightMultiplier = 1f)
		{
			if (WeightMultiplier <= 0f)
				return new WeightedList<T>();

			var ret = new WeightedList<T>(items.Capacity);
			foreach (var item in items)
				ret.Add(item.value, item.weight * WeightMultiplier);

			return ret;
		}
	}
}
