using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200037C RID: 892
	public class Stance_Cooldown : Stance_Busy
	{
		// Token: 0x060019D4 RID: 6612 RVA: 0x0009BDBD File Offset: 0x00099FBD
		public Stance_Cooldown()
		{
		}

		// Token: 0x060019D5 RID: 6613 RVA: 0x0009BDC5 File Offset: 0x00099FC5
		public Stance_Cooldown(int ticks, LocalTargetInfo focusTarg, Verb verb) : base(ticks, focusTarg, verb)
		{
		}

		// Token: 0x060019D6 RID: 6614 RVA: 0x0009BDD0 File Offset: 0x00099FD0
		public override void StanceDraw()
		{
			if (Find.Selector.IsSelected(this.stanceTracker.pawn))
			{
				float radius = Mathf.Min(0.5f, (float)this.ticksLeft * 0.002f);
				GenDraw.DrawCooldownCircle(this.stanceTracker.pawn.Drawer.DrawPos + new Vector3(0f, 0.2f, 0f), radius);
			}
		}

		// Token: 0x040012D6 RID: 4822
		private const float RadiusPerTick = 0.002f;

		// Token: 0x040012D7 RID: 4823
		private const float MaxRadius = 0.5f;
	}
}
