using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000128 RID: 296
	public class IngredientValueGetter_Nutrition : IngredientValueGetter
	{
		// Token: 0x060007A7 RID: 1959 RVA: 0x00027415 File Offset: 0x00025615
		public override float ValuePerUnitOf(ThingDef t)
		{
			if (!t.IsNutritionGivingIngestible)
			{
				return 0f;
			}
			return t.GetStatValueAbstract(StatDefOf.Nutrition, null);
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x00027434 File Offset: 0x00025634
		public override string BillRequirementsDescription(RecipeDef r, IngredientCount ing)
		{
			return ing.GetBaseCount() + "x " + "BillNutrition".Translate() + " (" + ing.filter.Summary + ")";
		}
	}
}
