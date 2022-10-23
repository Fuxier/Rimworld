using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000430 RID: 1072
	public static class DebugActionCategories
	{
		// Token: 0x06001F8A RID: 8074 RVA: 0x000BB868 File Offset: 0x000B9A68
		static DebugActionCategories()
		{
			DebugActionCategories.categoryOrders.Add("Incidents", 100);
			DebugActionCategories.categoryOrders.Add("Quests", 200);
			DebugActionCategories.categoryOrders.Add("Quests (old)", 250);
			DebugActionCategories.categoryOrders.Add("Translation", 300);
			DebugActionCategories.categoryOrders.Add("General", 400);
			DebugActionCategories.categoryOrders.Add("Pawns", 500);
			DebugActionCategories.categoryOrders.Add("Spawning", 600);
			DebugActionCategories.categoryOrders.Add("Ideoligion", 700);
			DebugActionCategories.categoryOrders.Add("Map", 800);
			DebugActionCategories.categoryOrders.Add("Autotests", 900);
			DebugActionCategories.categoryOrders.Add("Mods", 100);
			DebugActionCategories.categoryOrders.Add("More debug actions", 1000);
			DebugActionCategories.categoryOrders.Add("Humanlike", 1100);
			DebugActionCategories.categoryOrders.Add("Animal", 1200);
			DebugActionCategories.categoryOrders.Add("Insect", 1300);
			DebugActionCategories.categoryOrders.Add("Mechanoid", 1400);
			DebugActionCategories.categoryOrders.Add("Other", 1500);
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x000BB9D0 File Offset: 0x000B9BD0
		public static int GetOrderFor(string category)
		{
			int result;
			if (!category.NullOrEmpty() && DebugActionCategories.categoryOrders.TryGetValue(category, out result))
			{
				return result;
			}
			return int.MaxValue;
		}

		// Token: 0x04001575 RID: 5493
		public const string Incidents = "Incidents";

		// Token: 0x04001576 RID: 5494
		public const string Quests = "Quests";

		// Token: 0x04001577 RID: 5495
		public const string QuestsOld = "Quests (old)";

		// Token: 0x04001578 RID: 5496
		public const string Translation = "Translation";

		// Token: 0x04001579 RID: 5497
		public const string General = "General";

		// Token: 0x0400157A RID: 5498
		public const string Pawns = "Pawns";

		// Token: 0x0400157B RID: 5499
		public const string Spawning = "Spawning";

		// Token: 0x0400157C RID: 5500
		public const string Ideoligion = "Ideoligion";

		// Token: 0x0400157D RID: 5501
		public const string MapManagement = "Map";

		// Token: 0x0400157E RID: 5502
		public const string Autotests = "Autotests";

		// Token: 0x0400157F RID: 5503
		public const string Mods = "Mods";

		// Token: 0x04001580 RID: 5504
		public const string More = "More debug actions";

		// Token: 0x04001581 RID: 5505
		public const string Humanlike = "Humanlike";

		// Token: 0x04001582 RID: 5506
		public const string Animal = "Animal";

		// Token: 0x04001583 RID: 5507
		public const string Insect = "Insect";

		// Token: 0x04001584 RID: 5508
		public const string Mechanoid = "Mechanoid";

		// Token: 0x04001585 RID: 5509
		public const string Other = "Other";

		// Token: 0x04001586 RID: 5510
		public static readonly Dictionary<string, int> categoryOrders = new Dictionary<string, int>();
	}
}
