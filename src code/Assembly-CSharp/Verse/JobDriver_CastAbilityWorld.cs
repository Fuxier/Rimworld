using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000086 RID: 134
	public class JobDriver_CastAbilityWorld : JobDriver
	{
		// Token: 0x060004D2 RID: 1234 RVA: 0x00002662 File Offset: 0x00000862
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0001AD0A File Offset: 0x00018F0A
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_General.Do(delegate
			{
				this.pawn.stances.SetStance(new Stance_WarmupAbilityWorld(this.job.ability.def.verbProperties.warmupTime.SecondsToTicks(), null, this.job.ability.verb));
			});
			yield return Toils_General.Do(delegate
			{
				this.job.ability.Activate(this.job.globalTarget);
			});
			yield break;
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0001AD1A File Offset: 0x00018F1A
		public override string GetReport()
		{
			return "UsingVerb".Translate(this.job.ability.def.label, this.job.globalTarget.Label);
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x0001AD5A File Offset: 0x00018F5A
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			Ability ability = this.job.ability;
			if (ability == null)
			{
				return;
			}
			ability.Notify_StartedCasting();
		}
	}
}
