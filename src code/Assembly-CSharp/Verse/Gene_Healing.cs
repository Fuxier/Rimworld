using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002AD RID: 685
	public class Gene_Healing : Gene
	{
		// Token: 0x0600139E RID: 5022 RVA: 0x00077905 File Offset: 0x00075B05
		public override void PostAdd()
		{
			base.PostAdd();
			this.ResetInterval();
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x00077913 File Offset: 0x00075B13
		public override void Tick()
		{
			base.Tick();
			this.ticksToHeal--;
			if (this.ticksToHeal <= 0)
			{
				HediffComp_HealPermanentWounds.TryHealRandomPermanentWound(this.pawn, this.LabelCap);
				this.ResetInterval();
			}
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x0007794C File Offset: 0x00075B4C
		private void ResetInterval()
		{
			this.ticksToHeal = Gene_Healing.HealingIntervalTicksRange.RandomInRange;
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x0007796C File Offset: 0x00075B6C
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Heal permanent wound",
					action = delegate()
					{
						HediffComp_HealPermanentWounds.TryHealRandomPermanentWound(this.pawn, this.LabelCap);
						this.ResetInterval();
					}
				};
			}
			yield break;
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x0007797C File Offset: 0x00075B7C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToHeal, "ticksToHeal", 0, false);
		}

		// Token: 0x04001048 RID: 4168
		private int ticksToHeal;

		// Token: 0x04001049 RID: 4169
		private static readonly IntRange HealingIntervalTicksRange = new IntRange(900000, 1800000);
	}
}
