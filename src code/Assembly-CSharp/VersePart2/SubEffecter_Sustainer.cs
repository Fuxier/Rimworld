using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000555 RID: 1365
	public class SubEffecter_Sustainer : SubEffecter
	{
		// Token: 0x0600299A RID: 10650 RVA: 0x0001A6A9 File Offset: 0x000188A9
		public SubEffecter_Sustainer(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x0600299B RID: 10651 RVA: 0x0010A0F8 File Offset: 0x001082F8
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.age++;
			if (this.age > this.def.ticksBeforeSustainerStart)
			{
				if (this.sustainer == null || this.sustainer.Ended)
				{
					SoundInfo info = SoundInfo.InMap(A, MaintenanceType.PerTick);
					this.sustainer = this.def.soundDef.TrySpawnSustainer(info);
					return;
				}
				this.sustainer.Maintain();
			}
		}

		// Token: 0x04001B78 RID: 7032
		private int age;

		// Token: 0x04001B79 RID: 7033
		private Sustainer sustainer;
	}
}
