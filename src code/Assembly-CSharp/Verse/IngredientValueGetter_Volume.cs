using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000127 RID: 295
	public class IngredientValueGetter_Volume : IngredientValueGetter
	{
		// Token: 0x060007A3 RID: 1955 RVA: 0x000272F0 File Offset: 0x000254F0
		public override float ValuePerUnitOf(ThingDef t)
		{
			if (t.IsStuff)
			{
				return t.VolumePerUnit;
			}
			return 1f;
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x00027308 File Offset: 0x00025508
		public override string BillRequirementsDescription(RecipeDef r, IngredientCount ing)
		{
			if (!ing.filter.AllowedThingDefs.Any((ThingDef td) => td.smallVolume) || ing.filter.AllowedThingDefs.Any((ThingDef td) => td.smallVolume && !r.GetPremultipliedSmallIngredients().Contains(td)))
			{
				return ing.GetBaseCount() + "x " + ing.filter.Summary;
			}
			return ing.GetBaseCount() * 10f + "x " + ing.filter.Summary;
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x000273B8 File Offset: 0x000255B8
		public override string ExtraDescriptionLine(RecipeDef r)
		{
			Func<ThingDef, bool> <>9__1;
			if (r.ingredients.Any(delegate(IngredientCount ing)
			{
				IEnumerable<ThingDef> allowedThingDefs = ing.filter.AllowedThingDefs;
				Func<ThingDef, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((ThingDef td) => td.smallVolume && !r.GetPremultipliedSmallIngredients().Contains(td)));
				}
				return allowedThingDefs.Any(predicate);
			}))
			{
				return "BillRequiresMayVary".Translate(10.ToStringCached());
			}
			return null;
		}
	}
}
