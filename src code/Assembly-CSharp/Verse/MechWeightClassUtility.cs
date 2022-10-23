using System;

namespace Verse
{
	// Token: 0x020000D5 RID: 213
	public static class MechWeightClassUtility
	{
		// Token: 0x06000638 RID: 1592 RVA: 0x00022064 File Offset: 0x00020264
		public static string ToStringHuman(this MechWeightClass wc)
		{
			switch (wc)
			{
			case MechWeightClass.Light:
				return "MechWeightClass_Light".Translate();
			case MechWeightClass.Medium:
				return "MechWeightClass_Medium".Translate();
			case MechWeightClass.Heavy:
				return "MechWeightClass_Heavy".Translate();
			case MechWeightClass.UltraHeavy:
				return "MechWeightClass_Ultraheavy".Translate();
			default:
				throw new Exception("Unknown mech weight class " + wc);
			}
		}
	}
}
