using System.Collections.Generic;

namespace Terraria
{
	public class NBT
	{
		Dictionary<string, object> tags;

		public NBT()
		{
			tags = new Dictionary<string, object>(128);
		}
	}
}
