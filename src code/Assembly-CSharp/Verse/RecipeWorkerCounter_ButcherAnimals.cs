using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020000E0 RID: 224
	public class RecipeWorkerCounter_ButcherAnimals : RecipeWorkerCounter
	{
		// Token: 0x06000680 RID: 1664 RVA: 0x00002662 File Offset: 0x00000862
		public override bool CanCountProducts(Bill_Production bill)
		{
			return true;
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00023410 File Offset: 0x00021610
		public override int CountProducts(Bill_Production bill)
		{
			int num = 0;
			List<ThingDef> childThingDefs = ThingCategoryDefOf.MeatRaw.childThingDefs;
			for (int i = 0; i < childThingDefs.Count; i++)
			{
				num += bill.Map.resourceCounter.GetCount(childThingDefs[i]);
			}
			return num;
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x00023456 File Offset: 0x00021656
		public override string ProductsDescription(Bill_Production bill)
		{
			return ThingCategoryDefOf.MeatRaw.label;
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x00023464 File Offset: 0x00021664
		public override bool CanPossiblyStoreInStockpile(Bill_Production bill, Zone_Stockpile stockpile)
		{
			foreach (ThingDef thingDef in bill.ingredientFilter.AllowedThingDefs)
			{
				if (thingDef.ingestible != null && thingDef.ingestible.sourceDef != null)
				{
					RaceProperties race = thingDef.ingestible.sourceDef.race;
					if (race != null && race.meatDef != null && !stockpile.GetStoreSettings().AllowedToAccept(race.meatDef))
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
