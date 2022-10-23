using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x020000EF RID: 239
	public class DesignatorDropdownGroupDef : Def
	{
		// Token: 0x060006D0 RID: 1744 RVA: 0x00024AE1 File Offset: 0x00022CE1
		public IEnumerable<BuildableDef> BuildablesWithoutDefaultDesignators()
		{
			return from x in DefDatabase<ThingDef>.AllDefs.Concat(DefDatabase<TerrainDef>.AllDefs)
			where x.designatorDropdown == this && !x.canGenerateDefaultDesignator
			select x;
		}

		// Token: 0x0400056D RID: 1389
		public DesignatorDropdownGroupDef.IconSource iconSource;

		// Token: 0x0400056E RID: 1390
		public bool useGridMenu;

		// Token: 0x0400056F RID: 1391
		public bool includeEyeDropperTool;

		// Token: 0x02001CCE RID: 7374
		public enum IconSource : byte
		{
			// Token: 0x040071A1 RID: 29089
			Cost,
			// Token: 0x040071A2 RID: 29090
			Placed
		}
	}
}
