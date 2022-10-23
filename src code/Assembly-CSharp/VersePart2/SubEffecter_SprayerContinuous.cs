using System;

namespace Verse
{
	// Token: 0x02000550 RID: 1360
	public class SubEffecter_SprayerContinuous : SubEffecter_Sprayer
	{
		// Token: 0x0600298F RID: 10639 RVA: 0x00109F7C File Offset: 0x0010817C
		public SubEffecter_SprayerContinuous(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
			this.ticksUntilMote = def.initialDelayTicks;
		}

		// Token: 0x06002990 RID: 10640 RVA: 0x00109F94 File Offset: 0x00108194
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (this.moteCount >= this.def.maxMoteCount)
			{
				return;
			}
			this.ticksUntilMote--;
			if (this.ticksUntilMote <= 0)
			{
				base.MakeMote(A, B, -1);
				this.ticksUntilMote = this.def.ticksBetweenMotes;
				this.moteCount++;
			}
		}

		// Token: 0x04001B75 RID: 7029
		private int ticksUntilMote;

		// Token: 0x04001B76 RID: 7030
		private int moteCount;
	}
}
