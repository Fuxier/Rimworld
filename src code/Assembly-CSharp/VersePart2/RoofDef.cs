using System;

namespace Verse
{
	// Token: 0x0200012D RID: 301
	public class RoofDef : Def
	{
		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x00028218 File Offset: 0x00026418
		public bool VanishOnCollapse
		{
			get
			{
				return !this.isThickRoof;
			}
		}

		// Token: 0x040007E4 RID: 2020
		public bool isNatural;

		// Token: 0x040007E5 RID: 2021
		public bool isThickRoof;

		// Token: 0x040007E6 RID: 2022
		public ThingDef collapseLeavingThingDef;

		// Token: 0x040007E7 RID: 2023
		public ThingDef filthLeaving;

		// Token: 0x040007E8 RID: 2024
		public SoundDef soundPunchThrough;
	}
}
