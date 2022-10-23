using System;

namespace Verse
{
	// Token: 0x02000079 RID: 121
	public class SubEffecter_DrifterEmoteContinuous : SubEffecter_DrifterEmote
	{
		// Token: 0x060004B0 RID: 1200 RVA: 0x0001A826 File Offset: 0x00018A26
		public SubEffecter_DrifterEmoteContinuous(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0001A830 File Offset: 0x00018A30
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilMote--;
			if (this.ticksUntilMote <= 0)
			{
				base.MakeMote(A, -1);
				this.ticksUntilMote = this.def.ticksBetweenMotes;
			}
		}

		// Token: 0x0400021A RID: 538
		private int ticksUntilMote;
	}
}
