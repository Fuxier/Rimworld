using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020000DF RID: 223
	public class RecipeWorkerCounter_MakeStoneBlocks : RecipeWorkerCounter
	{
		// Token: 0x0600067B RID: 1659 RVA: 0x00002662 File Offset: 0x00000862
		public override bool CanCountProducts(Bill_Production bill)
		{
			return true;
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x00023334 File Offset: 0x00021534
		public override int CountProducts(Bill_Production bill)
		{
			int num = 0;
			List<ThingDef> childThingDefs = ThingCategoryDefOf.StoneBlocks.childThingDefs;
			for (int i = 0; i < childThingDefs.Count; i++)
			{
				num += bill.Map.resourceCounter.GetCount(childThingDefs[i]);
			}
			return num;
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0002337A File Offset: 0x0002157A
		public override string ProductsDescription(Bill_Production bill)
		{
			return ThingCategoryDefOf.StoneBlocks.label;
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00023388 File Offset: 0x00021588
		public override bool CanPossiblyStoreInStockpile(Bill_Production bill, Zone_Stockpile stockpile)
		{
			foreach (ThingDef thingDef in bill.ingredientFilter.AllowedThingDefs)
			{
				if (!thingDef.butcherProducts.NullOrEmpty<ThingDefCountClass>())
				{
					ThingDef thingDef2 = thingDef.butcherProducts[0].thingDef;
					if (!stockpile.GetStoreSettings().AllowedToAccept(thingDef2))
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
