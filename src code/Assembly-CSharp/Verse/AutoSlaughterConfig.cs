using System;

namespace Verse
{
	// Token: 0x020001B4 RID: 436
	public class AutoSlaughterConfig : IExposable
	{
		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000C4C RID: 3148 RVA: 0x000448D8 File Offset: 0x00042AD8
		public bool AnyLimit
		{
			get
			{
				return this.maxTotal != -1 || this.maxMales != -1 || this.maxFemales != -1 || this.maxMalesYoung != -1 || this.maxFemalesYoung != -1 || !this.allowSlaughterPregnant || !this.allowSlaughterBonded;
			}
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x00044928 File Offset: 0x00042B28
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.animal, "animal");
			Scribe_Values.Look<int>(ref this.maxTotal, "maxTotal", -1, false);
			Scribe_Values.Look<int>(ref this.maxMales, "maxMales", -1, false);
			Scribe_Values.Look<int>(ref this.maxMalesYoung, "maxMalesYoung", -1, false);
			Scribe_Values.Look<int>(ref this.maxFemales, "maxFemales", -1, false);
			Scribe_Values.Look<int>(ref this.maxFemalesYoung, "maxFemalesYoung", -1, false);
			Scribe_Values.Look<bool>(ref this.allowSlaughterPregnant, "allowSlaughterPregnant", false, false);
			Scribe_Values.Look<bool>(ref this.allowSlaughterBonded, "allowSlaughterBonded", false, false);
		}

		// Token: 0x04000B41 RID: 2881
		public ThingDef animal;

		// Token: 0x04000B42 RID: 2882
		public int maxTotal = -1;

		// Token: 0x04000B43 RID: 2883
		public int maxMales = -1;

		// Token: 0x04000B44 RID: 2884
		public int maxMalesYoung = -1;

		// Token: 0x04000B45 RID: 2885
		public int maxFemales = -1;

		// Token: 0x04000B46 RID: 2886
		public int maxFemalesYoung = -1;

		// Token: 0x04000B47 RID: 2887
		public bool allowSlaughterPregnant;

		// Token: 0x04000B48 RID: 2888
		public bool allowSlaughterBonded;

		// Token: 0x04000B49 RID: 2889
		public string uiMaxTotalBuffer;

		// Token: 0x04000B4A RID: 2890
		public string uiMaxMalesBuffer;

		// Token: 0x04000B4B RID: 2891
		public string uiMaxMalesYoungBuffer;

		// Token: 0x04000B4C RID: 2892
		public string uiMaxFemalesBuffer;

		// Token: 0x04000B4D RID: 2893
		public string uiMaxFemalesYoungBuffer;

		// Token: 0x04000B4E RID: 2894
		public const int NoLimit = -1;
	}
}
