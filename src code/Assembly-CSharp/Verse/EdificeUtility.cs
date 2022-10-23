using System;

namespace Verse
{
	// Token: 0x020001E3 RID: 483
	public static class EdificeUtility
	{
		// Token: 0x06000D88 RID: 3464 RVA: 0x0004B3AC File Offset: 0x000495AC
		public static bool IsEdifice(this BuildableDef def)
		{
			ThingDef thingDef = def as ThingDef;
			return thingDef != null && thingDef.category == ThingCategory.Building && thingDef.building.isEdifice;
		}
	}
}
