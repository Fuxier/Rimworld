using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200032E RID: 814
	public class HediffComp_VatGrowing : HediffComp
	{
		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x060015C9 RID: 5577 RVA: 0x000818EC File Offset: 0x0007FAEC
		public override bool CompShouldRemove
		{
			get
			{
				return base.Pawn.Spawned || !(base.Pawn.ParentHolder is Building_GrowthVat);
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x060015CA RID: 5578 RVA: 0x00081913 File Offset: 0x0007FB13
		public override string CompTipStringExtra
		{
			get
			{
				return "AgingSpeed".Translate() + ": x" + 20;
			}
		}
	}
}
