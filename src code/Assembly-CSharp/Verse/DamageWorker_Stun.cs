using System;

namespace Verse
{
	// Token: 0x020002BC RID: 700
	public class DamageWorker_Stun : DamageWorker
	{
		// Token: 0x060013E8 RID: 5096 RVA: 0x00079A29 File Offset: 0x00077C29
		public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing victim)
		{
			DamageWorker.DamageResult damageResult = base.Apply(dinfo, victim);
			damageResult.stunned = true;
			return damageResult;
		}
	}
}
