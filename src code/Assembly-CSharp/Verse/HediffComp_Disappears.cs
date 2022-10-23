using System;
using RimWorld;
using UnityEngine;
using Verse.Noise;

namespace Verse
{
	// Token: 0x020002D5 RID: 725
	public class HediffComp_Disappears : HediffComp
	{
		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x060014A0 RID: 5280 RVA: 0x0007D78E File Offset: 0x0007B98E
		public HediffCompProperties_Disappears Props
		{
			get
			{
				return (HediffCompProperties_Disappears)this.props;
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x060014A1 RID: 5281 RVA: 0x0007D79B File Offset: 0x0007B99B
		public override bool CompShouldRemove
		{
			get
			{
				return base.CompShouldRemove || this.ticksToDisappear <= 0 || (this.Props.requiredMentalState != null && base.Pawn.MentalStateDef != this.Props.requiredMentalState);
			}
		}

		// Token: 0x060014A2 RID: 5282 RVA: 0x0007D7DC File Offset: 0x0007B9DC
		private static float AddNoiseToProgress(float progress, int seed)
		{
			float num = (float)Perlin.GetValue((double)progress, 0.0, 0.0, 9.0, seed, 2.0, 0.5, 6, QualityMode.Medium);
			float num2 = 0.25f * (1f - progress);
			return Mathf.Clamp01(progress + num2 * num);
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x060014A3 RID: 5283 RVA: 0x0007D83A File Offset: 0x0007BA3A
		public float Progress
		{
			get
			{
				return 1f - (float)this.ticksToDisappear / (float)Math.Max(1, this.disappearsAfterTicks);
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x060014A4 RID: 5284 RVA: 0x0007D857 File Offset: 0x0007BA57
		public float NoisyProgress
		{
			get
			{
				return HediffComp_Disappears.AddNoiseToProgress(this.Progress, this.seed);
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x060014A5 RID: 5285 RVA: 0x0007D86A File Offset: 0x0007BA6A
		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (!this.Props.showRemainingTime)
				{
					return base.CompLabelInBracketsExtra;
				}
				return this.ticksToDisappear.ToStringTicksToPeriod(true, true, true, true, this.Props.canUseDecimalsShortForm);
			}
		}

		// Token: 0x060014A6 RID: 5286 RVA: 0x0007D89A File Offset: 0x0007BA9A
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.disappearsAfterTicks = this.Props.disappearsAfterTicks.RandomInRange;
			this.seed = Rand.Int;
			this.ticksToDisappear = this.disappearsAfterTicks;
		}

		// Token: 0x060014A7 RID: 5287 RVA: 0x0007D8D0 File Offset: 0x0007BAD0
		public override void CompPostPostRemoved()
		{
			if (!this.Props.messageOnDisappear.NullOrEmpty() && PawnUtility.ShouldSendNotificationAbout(base.Pawn))
			{
				Messages.Message(this.Props.messageOnDisappear.Formatted(base.Pawn.Named("PAWN")), base.Pawn, MessageTypeDefOf.PositiveEvent, true);
			}
		}

		// Token: 0x060014A8 RID: 5288 RVA: 0x0007D937 File Offset: 0x0007BB37
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksToDisappear--;
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x0007D948 File Offset: 0x0007BB48
		public override void CompPostMerged(Hediff other)
		{
			base.CompPostMerged(other);
			HediffComp_Disappears hediffComp_Disappears = other.TryGetComp<HediffComp_Disappears>();
			if (hediffComp_Disappears != null && hediffComp_Disappears.ticksToDisappear > this.ticksToDisappear)
			{
				this.ticksToDisappear = hediffComp_Disappears.ticksToDisappear;
			}
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x0007D980 File Offset: 0x0007BB80
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksToDisappear, "ticksToDisappear", 0, false);
			Scribe_Values.Look<int>(ref this.disappearsAfterTicks, "disappearsAfterTicks", 0, false);
			Scribe_Values.Look<int>(ref this.seed, "seed", 0, false);
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x0007D9B8 File Offset: 0x0007BBB8
		public override string CompDebugString()
		{
			return "ticksToDisappear: " + this.ticksToDisappear;
		}

		// Token: 0x040010C6 RID: 4294
		public int ticksToDisappear;

		// Token: 0x040010C7 RID: 4295
		public int disappearsAfterTicks;

		// Token: 0x040010C8 RID: 4296
		public int seed;

		// Token: 0x040010C9 RID: 4297
		private const float NoiseScale = 0.25f;

		// Token: 0x040010CA RID: 4298
		private const float NoiseWiggliness = 9f;
	}
}
