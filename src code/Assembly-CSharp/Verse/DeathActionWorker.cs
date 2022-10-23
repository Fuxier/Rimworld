using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000D6 RID: 214
	public abstract class DeathActionWorker
	{
		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x000220DE File Offset: 0x000202DE
		public virtual RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_Died;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x0600063A RID: 1594 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual bool DangerousInMelee
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600063B RID: 1595
		public abstract void PawnDied(Corpse corpse);
	}
}
