using System;

namespace Verse
{
	// Token: 0x02000314 RID: 788
	public class HediffCompProperties_ReactOnDamage : HediffCompProperties
	{
		// Token: 0x0600156B RID: 5483 RVA: 0x000806BC File Offset: 0x0007E8BC
		public HediffCompProperties_ReactOnDamage()
		{
			this.compClass = typeof(HediffComp_ReactOnDamage);
		}

		// Token: 0x04001139 RID: 4409
		public DamageDef damageDefIncoming;

		// Token: 0x0400113A RID: 4410
		public BodyPartDef createHediffOn;

		// Token: 0x0400113B RID: 4411
		public HediffDef createHediff;

		// Token: 0x0400113C RID: 4412
		public bool vomit;
	}
}
