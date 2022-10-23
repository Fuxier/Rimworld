using System;

namespace Verse
{
	// Token: 0x020002B9 RID: 697
	public class DamageWorker_Frostbite : DamageWorker_AddInjury
	{
		// Token: 0x060013E0 RID: 5088 RVA: 0x00079732 File Offset: 0x00077932
		protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			base.FinalizeAndAddInjury(pawn, totalDamage, dinfo, result);
		}
	}
}
