using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200054D RID: 1357
	public class SubEffecter_SoundIntermittent : SubEffecter
	{
		// Token: 0x06002988 RID: 10632 RVA: 0x00109627 File Offset: 0x00107827
		public SubEffecter_SoundIntermittent(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
			this.ticksUntilSound = def.intermittentSoundInterval.RandomInRange;
		}

		// Token: 0x06002989 RID: 10633 RVA: 0x00109644 File Offset: 0x00107844
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilSound--;
			if (this.ticksUntilSound <= 0)
			{
				this.def.soundDef.PlayOneShot(A);
				this.ticksUntilSound = this.def.intermittentSoundInterval.RandomInRange;
			}
		}

		// Token: 0x04001B72 RID: 7026
		protected int ticksUntilSound;
	}
}
