using System;

namespace Verse
{
	// Token: 0x020000D4 RID: 212
	public static class DietCategoryUtility
	{
		// Token: 0x06000636 RID: 1590 RVA: 0x00021F3C File Offset: 0x0002013C
		public static string ToStringHuman(this DietCategory diet)
		{
			switch (diet)
			{
			case DietCategory.NeverEats:
				return "DietCategory_NeverEats".Translate();
			case DietCategory.Herbivorous:
				return "DietCategory_Herbivorous".Translate();
			case DietCategory.Dendrovorous:
				return "DietCategory_Dendrovorous".Translate();
			case DietCategory.Ovivorous:
				return "DietCategory_Ovivorous".Translate();
			case DietCategory.Omnivorous:
				return "DietCategory_Omnivorous".Translate();
			case DietCategory.Carnivorous:
				return "DietCategory_Carnivorous".Translate();
			default:
				return "error";
			}
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00021FD0 File Offset: 0x000201D0
		public static string ToStringHumanShort(this DietCategory diet)
		{
			switch (diet)
			{
			case DietCategory.NeverEats:
				return "DietCategory_NeverEats_Short".Translate();
			case DietCategory.Herbivorous:
				return "DietCategory_Herbivorous_Short".Translate();
			case DietCategory.Dendrovorous:
				return "DietCategory_Dendrovorous_Short".Translate();
			case DietCategory.Ovivorous:
				return "DietCategory_Ovivorous_Short".Translate();
			case DietCategory.Omnivorous:
				return "DietCategory_Omnivorous_Short".Translate();
			case DietCategory.Carnivorous:
				return "DietCategory_Carnivorous_Short".Translate();
			default:
				return "error";
			}
		}
	}
}
