using System;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000342 RID: 834
	public class Hediff_Deathrest : HediffWithComps
	{
		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06001642 RID: 5698 RVA: 0x00083508 File Offset: 0x00081708
		private Gene_Deathrest DeathrestGene
		{
			get
			{
				if (this.cachedGene == null)
				{
					Pawn_GeneTracker genes = this.pawn.genes;
					this.cachedGene = ((genes != null) ? genes.GetFirstGeneOfType<Gene_Deathrest>() : null);
				}
				return this.cachedGene;
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06001643 RID: 5699 RVA: 0x00083535 File Offset: 0x00081735
		public bool Paused
		{
			get
			{
				if (this.lastPauseCheckTick < Find.TickManager.TicksGame + 120)
				{
					this.lastPauseCheckTick = Find.TickManager.TicksGame;
					this.cachedPaused = SanguophageUtility.ShouldBeDeathrestingOrInComaInsteadOfDead(this.pawn);
				}
				return this.cachedPaused;
			}
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06001644 RID: 5700 RVA: 0x00083573 File Offset: 0x00081773
		public override string LabelInBrackets
		{
			get
			{
				if (this.Paused)
				{
					return base.LabelInBrackets;
				}
				return this.DeathrestGene.DeathrestPercent.ToStringPercent("F0");
			}
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06001645 RID: 5701 RVA: 0x0008359C File Offset: 0x0008179C
		public override string TipStringExtra
		{
			get
			{
				string text = base.TipStringExtra;
				if (this.Paused)
				{
					if (!text.NullOrEmpty())
					{
						text += "\n";
					}
					text += "PawnWillKeepDeathrestingLethalInjuries".Translate(this.pawn.Named("PAWN")).Colorize(ColorLibrary.RedReadable);
				}
				return text;
			}
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06001646 RID: 5702 RVA: 0x000835F8 File Offset: 0x000817F8
		public override bool ShouldRemove
		{
			get
			{
				return this.DeathrestGene == null || base.ShouldRemove;
			}
		}

		// Token: 0x06001647 RID: 5703 RVA: 0x0008360A File Offset: 0x0008180A
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			Gene_Deathrest deathrestGene = this.DeathrestGene;
			if (deathrestGene == null)
			{
				return;
			}
			deathrestGene.Notify_DeathrestStarted();
		}

		// Token: 0x06001648 RID: 5704 RVA: 0x00083624 File Offset: 0x00081824
		public override void PostRemoved()
		{
			base.PostRemoved();
			Gene_Deathrest deathrestGene = this.DeathrestGene;
			if (deathrestGene != null)
			{
				deathrestGene.Notify_DeathrestEnded();
			}
			if (this.pawn.Spawned && this.pawn.CurJobDef == JobDefOf.Deathrest)
			{
				this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x0008367C File Offset: 0x0008187C
		public override void PostTick()
		{
			base.PostTick();
			bool paused = this.Paused;
			Gene_Deathrest deathrestGene = this.DeathrestGene;
			if (deathrestGene == null)
			{
				return;
			}
			deathrestGene.TickDeathresting(paused);
		}

		// Token: 0x04001189 RID: 4489
		private Need_Deathrest cachedNeed;

		// Token: 0x0400118A RID: 4490
		private Gene_Deathrest cachedGene;

		// Token: 0x0400118B RID: 4491
		private int lastPauseCheckTick = -1;

		// Token: 0x0400118C RID: 4492
		private bool cachedPaused;

		// Token: 0x0400118D RID: 4493
		private const int PauseCheckInterval = 120;
	}
}
