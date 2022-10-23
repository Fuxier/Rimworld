using System;

namespace Verse
{
	// Token: 0x0200025A RID: 602
	public static class SnowUtility
	{
		// Token: 0x0600112A RID: 4394 RVA: 0x000642D3 File Offset: 0x000624D3
		public static SnowCategory GetSnowCategory(float snowDepth)
		{
			if (snowDepth < 0.03f)
			{
				return SnowCategory.None;
			}
			if (snowDepth < 0.25f)
			{
				return SnowCategory.Dusting;
			}
			if (snowDepth < 0.5f)
			{
				return SnowCategory.Thin;
			}
			if (snowDepth < 0.75f)
			{
				return SnowCategory.Medium;
			}
			return SnowCategory.Thick;
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x00064300 File Offset: 0x00062500
		public static string GetDescription(SnowCategory category)
		{
			switch (category)
			{
			case SnowCategory.None:
				return "SnowNone".Translate();
			case SnowCategory.Dusting:
				return "SnowDusting".Translate();
			case SnowCategory.Thin:
				return "SnowThin".Translate();
			case SnowCategory.Medium:
				return "SnowMedium".Translate();
			case SnowCategory.Thick:
				return "SnowThick".Translate();
			default:
				return "Unknown snow";
			}
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x0006437E File Offset: 0x0006257E
		public static int MovementTicksAddOn(SnowCategory category)
		{
			switch (category)
			{
			case SnowCategory.None:
				return 0;
			case SnowCategory.Dusting:
				return 0;
			case SnowCategory.Thin:
				return 4;
			case SnowCategory.Medium:
				return 8;
			case SnowCategory.Thick:
				return 12;
			default:
				return 0;
			}
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x000643A8 File Offset: 0x000625A8
		public static void AddSnowRadial(IntVec3 center, Map map, float radius, float depth)
		{
			int num = GenRadial.NumCellsInRadius(radius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map))
				{
					float lengthHorizontal = (center - intVec).LengthHorizontal;
					float num2 = 1f - lengthHorizontal / radius;
					map.snowGrid.AddDepth(intVec, num2 * depth);
				}
			}
		}
	}
}
