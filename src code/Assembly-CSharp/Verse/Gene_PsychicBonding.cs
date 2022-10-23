using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020002B1 RID: 689
	public class Gene_PsychicBonding : Gene
	{
		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x060013B2 RID: 5042 RVA: 0x00077C88 File Offset: 0x00075E88
		public bool CanBondToNewPawn
		{
			get
			{
				return this.bondedPawn == null && !this.pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicBondTorn, false) && this.pawn.needs.mood.thoughts.memories.GetFirstMemoryOfDef(ThoughtDefOf.PsychicBondTorn) == null;
			}
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x00077CE8 File Offset: 0x00075EE8
		public override void PostAdd()
		{
			base.PostAdd();
			Hediff_PsychicBond hediff_PsychicBond = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond, false) as Hediff_PsychicBond;
			if (hediff_PsychicBond != null)
			{
				this.bondedPawn = (Pawn)hediff_PsychicBond.target;
			}
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x00077D30 File Offset: 0x00075F30
		public override void PostRemove()
		{
			base.PostRemove();
			this.Notify_MyOrPartnersGeneRemoved();
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x00077D40 File Offset: 0x00075F40
		public void BondTo(Pawn newBond)
		{
			if (!ModLister.CheckBiotech("Psychic bonding"))
			{
				return;
			}
			if (newBond == null)
			{
				return;
			}
			if (this.bondedPawn == newBond)
			{
				return;
			}
			if (this.bondedPawn != null)
			{
				Log.Error("Tried to bond to more than one pawn.");
				return;
			}
			this.bondedPawn = newBond;
			this.pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefIf(ThoughtDefOf.PsychicBondTorn, (Thought_Memory m) => m.otherPawn == this.bondedPawn);
			this.bondedPawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefIf(ThoughtDefOf.PsychicBondTorn, (Thought_Memory m) => m.otherPawn == this.pawn);
			Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBondTorn, false);
			if (firstHediffOfDef != null)
			{
				this.pawn.health.RemoveHediff(firstHediffOfDef);
			}
			Hediff firstHediffOfDef2 = this.bondedPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBondTorn, false);
			if (firstHediffOfDef2 != null)
			{
				this.bondedPawn.health.RemoveHediff(firstHediffOfDef2);
			}
			Hediff_PsychicBond hediff_PsychicBond = (Hediff_PsychicBond)HediffMaker.MakeHediff(HediffDefOf.PsychicBond, this.pawn, null);
			hediff_PsychicBond.target = this.bondedPawn;
			this.pawn.health.AddHediff(hediff_PsychicBond, null, null, null);
			Pawn_GeneTracker genes = this.bondedPawn.genes;
			Gene_PsychicBonding gene_PsychicBonding = (genes != null) ? genes.GetFirstGeneOfType<Gene_PsychicBonding>() : null;
			if (gene_PsychicBonding != null)
			{
				gene_PsychicBonding.BondTo(this.pawn);
				return;
			}
			Hediff_PsychicBond hediff_PsychicBond2 = (Hediff_PsychicBond)HediffMaker.MakeHediff(HediffDefOf.PsychicBond, this.bondedPawn, null);
			hediff_PsychicBond2.target = this.pawn;
			this.bondedPawn.health.AddHediff(hediff_PsychicBond2, null, null, null);
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x00077EF0 File Offset: 0x000760F0
		public void RemoveBond()
		{
			if (this.bondedPawn == null)
			{
				return;
			}
			Pawn_NeedsTracker needs = this.pawn.needs;
			if (needs != null)
			{
				Need_Mood mood = needs.mood;
				if (mood != null)
				{
					ThoughtHandler thoughts = mood.thoughts;
					if (thoughts != null)
					{
						MemoryThoughtHandler memories = thoughts.memories;
						if (memories != null)
						{
							memories.TryGainMemory(ThoughtDefOf.PsychicBondTorn, this.bondedPawn, null);
						}
					}
				}
			}
			Pawn_NeedsTracker needs2 = this.bondedPawn.needs;
			if (needs2 != null)
			{
				Need_Mood mood2 = needs2.mood;
				if (mood2 != null)
				{
					ThoughtHandler thoughts2 = mood2.thoughts;
					if (thoughts2 != null)
					{
						MemoryThoughtHandler memories2 = thoughts2.memories;
						if (memories2 != null)
						{
							memories2.TryGainMemory(ThoughtDefOf.PsychicBondTorn, this.pawn, null);
						}
					}
				}
			}
			Pawn pawn = this.bondedPawn;
			this.bondedPawn = null;
			Hediff_PsychicBond hediff_PsychicBond = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond, false) as Hediff_PsychicBond;
			if (hediff_PsychicBond != null && hediff_PsychicBond.target == pawn)
			{
				this.pawn.health.RemoveHediff(hediff_PsychicBond);
			}
			Hediff_PsychicBond hediff_PsychicBond2 = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond, false) as Hediff_PsychicBond;
			if (hediff_PsychicBond2 != null)
			{
				pawn.health.RemoveHediff(hediff_PsychicBond2);
			}
			Pawn_GeneTracker genes = pawn.genes;
			if (genes != null)
			{
				Gene_PsychicBonding firstGeneOfType = genes.GetFirstGeneOfType<Gene_PsychicBonding>();
				if (firstGeneOfType != null)
				{
					firstGeneOfType.RemoveBond();
				}
			}
			if (!this.pawn.Dead)
			{
				if (pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBondTorn, false) == null)
				{
					Hediff_PsychicBondTorn hediff_PsychicBondTorn = (Hediff_PsychicBondTorn)HediffMaker.MakeHediff(HediffDefOf.PsychicBondTorn, pawn, null);
					hediff_PsychicBondTorn.target = this.pawn;
					pawn.health.AddHediff(hediff_PsychicBondTorn, null, null, null);
				}
				MentalBreakDef mentalBreakDef;
				if ((from d in DefDatabase<MentalBreakDef>.AllDefsListForReading
				where d.intensity == MentalBreakIntensity.Extreme && d.Worker.BreakCanOccur(this.pawn)
				select d).TryRandomElementByWeight((MentalBreakDef d) => d.Worker.CommonalityFor(this.pawn, true), out mentalBreakDef))
				{
					mentalBreakDef.Worker.TryStart(this.pawn, "MentalStateReason_BondedHumanDeath".Translate(pawn), false);
				}
			}
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x000780D4 File Offset: 0x000762D4
		public void Notify_MyOrPartnersGeneRemoved()
		{
			if (this.bondedPawn != null)
			{
				Pawn_GeneTracker genes = this.bondedPawn.genes;
				if (((genes != null) ? genes.GetFirstGeneOfType<Gene_PsychicBonding>() : null) == null)
				{
					Pawn pawn = this.bondedPawn;
					this.bondedPawn = null;
					Hediff_PsychicBond hediff_PsychicBond = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond, false) as Hediff_PsychicBond;
					if (hediff_PsychicBond != null && hediff_PsychicBond.target == pawn)
					{
						this.pawn.health.RemoveHediff(hediff_PsychicBond);
					}
					Hediff_PsychicBond hediff_PsychicBond2 = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond, false) as Hediff_PsychicBond;
					if (hediff_PsychicBond2 != null)
					{
						pawn.health.RemoveHediff(hediff_PsychicBond2);
					}
					Pawn_GeneTracker genes2 = pawn.genes;
					if (genes2 == null)
					{
						return;
					}
					Gene_PsychicBonding firstGeneOfType = genes2.GetFirstGeneOfType<Gene_PsychicBonding>();
					if (firstGeneOfType == null)
					{
						return;
					}
					firstGeneOfType.Notify_MyOrPartnersGeneRemoved();
					return;
				}
			}
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x00078194 File Offset: 0x00076394
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (DebugSettings.ShowDevGizmos && this.CanBondToNewPawn && this.pawn.Spawned)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Bond to random pawn",
					action = delegate()
					{
						Pawn newBond;
						if ((from x in this.pawn.Map.mapPawns.SpawnedPawnsInFaction(this.pawn.Faction)
						where x.RaceProps.Humanlike && x != this.pawn
						select x).TryRandomElement(out newBond))
						{
							this.BondTo(newBond);
						}
					}
				};
			}
			yield break;
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x000781A4 File Offset: 0x000763A4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.bondedPawn, "bondedPawn", false);
		}

		// Token: 0x0400104D RID: 4173
		private Pawn bondedPawn;
	}
}
