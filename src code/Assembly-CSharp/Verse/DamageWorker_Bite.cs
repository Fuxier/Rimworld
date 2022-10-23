using System;

namespace Verse
{
	// Token: 0x020002B5 RID: 693
	public class DamageWorker_Bite : DamageWorker_AddInjury
	{
		// Token: 0x060013D3 RID: 5075 RVA: 0x00078E66 File Offset: 0x00077066
		protected override BodyPartRecord ChooseHitPart(DamageInfo dinfo, Pawn pawn)
		{
			return pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, BodyPartDepth.Outside, null);
		}
	}
}
