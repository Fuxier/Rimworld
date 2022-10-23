using System;

namespace Verse
{
	// Token: 0x02000554 RID: 1364
	public class SubEffecter_SprayerTriggeredDelayed : SubEffecter_SprayerTriggered
	{
		// Token: 0x06002997 RID: 10647 RVA: 0x0010A0AA File Offset: 0x001082AA
		public SubEffecter_SprayerTriggeredDelayed(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06002998 RID: 10648 RVA: 0x0010A0BB File Offset: 0x001082BB
		public override void SubTrigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1)
		{
			this.ticksLeft = this.def.initialDelayTicks;
		}

		// Token: 0x06002999 RID: 10649 RVA: 0x0010A0CE File Offset: 0x001082CE
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (this.ticksLeft == 0)
			{
				base.MakeMote(A, B, -1);
			}
			if (this.ticksLeft >= 0)
			{
				this.ticksLeft--;
			}
		}

		// Token: 0x04001B77 RID: 7031
		private int ticksLeft = -1;
	}
}
