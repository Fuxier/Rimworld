using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002DE RID: 734
	public class HediffCompProperties_DrugEffectFactor : HediffCompProperties
	{
		// Token: 0x060014C8 RID: 5320 RVA: 0x0007E151 File Offset: 0x0007C351
		public HediffCompProperties_DrugEffectFactor()
		{
			this.compClass = typeof(HediffComp_DrugEffectFactor);
		}

		// Token: 0x040010D6 RID: 4310
		public ChemicalDef chemical;
	}
}
