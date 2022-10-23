using System;

namespace Verse
{
	// Token: 0x02000317 RID: 791
	public class HediffComp_RecoveryThought : HediffComp
	{
		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06001572 RID: 5490 RVA: 0x00080800 File Offset: 0x0007EA00
		public HediffCompProperties_RecoveryThought Props
		{
			get
			{
				return (HediffCompProperties_RecoveryThought)this.props;
			}
		}

		// Token: 0x06001573 RID: 5491 RVA: 0x00080810 File Offset: 0x0007EA10
		public override void CompPostPostRemoved()
		{
			base.CompPostPostRemoved();
			if (!base.Pawn.Dead && base.Pawn.needs.mood != null)
			{
				base.Pawn.needs.mood.thoughts.memories.TryGainMemory(this.Props.thought, null, null);
			}
		}
	}
}
