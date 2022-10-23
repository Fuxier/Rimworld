using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200034D RID: 845
	public class Hediff_PsychicBondTorn : HediffWithTarget
	{
		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x060016B5 RID: 5813 RVA: 0x0008555F File Offset: 0x0008375F
		public override string LabelBase
		{
			get
			{
				string labelBase = base.LabelBase;
				string str = " (";
				Thing target = this.target;
				return labelBase + str + ((target != null) ? target.LabelShortCap : null) + ")";
			}
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x060016B6 RID: 5814 RVA: 0x0000249D File Offset: 0x0000069D
		public override bool Visible
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060016B7 RID: 5815 RVA: 0x0008563F File Offset: 0x0008383F
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.creationTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x060016B8 RID: 5816 RVA: 0x00085660 File Offset: 0x00083860
		public override void Notify_Resurrected()
		{
			bool flag = false;
			Pawn pawn;
			if ((pawn = (this.target as Pawn)) != null && !pawn.Dead && !pawn.Destroyed && this.creationTick >= 0 && Find.TickManager.TicksGame - this.creationTick <= ThoughtDefOf.PsychicBondTorn.DurationTicks)
			{
				Pawn_GeneTracker genes = this.pawn.genes;
				Gene_PsychicBonding gene_PsychicBonding = (genes != null) ? genes.GetFirstGeneOfType<Gene_PsychicBonding>() : null;
				if (gene_PsychicBonding != null)
				{
					gene_PsychicBonding.BondTo(pawn);
				}
				else
				{
					Pawn_GeneTracker genes2 = pawn.genes;
					if (genes2 != null)
					{
						Gene_PsychicBonding firstGeneOfType = genes2.GetFirstGeneOfType<Gene_PsychicBonding>();
						if (firstGeneOfType != null)
						{
							firstGeneOfType.BondTo(this.pawn);
						}
					}
				}
				flag = true;
			}
			if (!flag)
			{
				this.pawn.health.RemoveHediff(this);
			}
		}

		// Token: 0x060016B9 RID: 5817 RVA: 0x00085710 File Offset: 0x00083910
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.creationTick, "creationTick", -1, false);
		}

		// Token: 0x0400119F RID: 4511
		private int creationTick = -1;
	}
}
