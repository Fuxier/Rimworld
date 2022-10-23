using System;

namespace Verse
{
	// Token: 0x02000126 RID: 294
	public abstract class IngredientValueGetter
	{
		// Token: 0x0600079F RID: 1951
		public abstract float ValuePerUnitOf(ThingDef t);

		// Token: 0x060007A0 RID: 1952
		public abstract string BillRequirementsDescription(RecipeDef r, IngredientCount ing);

		// Token: 0x060007A1 RID: 1953 RVA: 0x000029B0 File Offset: 0x00000BB0
		public virtual string ExtraDescriptionLine(RecipeDef r)
		{
			return null;
		}
	}
}
