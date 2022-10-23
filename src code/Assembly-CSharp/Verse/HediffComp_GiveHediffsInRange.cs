using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002EE RID: 750
	public class HediffComp_GiveHediffsInRange : HediffComp
	{
		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x060014F3 RID: 5363 RVA: 0x0007EAAE File Offset: 0x0007CCAE
		public HediffCompProperties_GiveHediffsInRange Props
		{
			get
			{
				return (HediffCompProperties_GiveHediffsInRange)this.props;
			}
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x0007EABC File Offset: 0x0007CCBC
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (!this.parent.pawn.Awake() || this.parent.pawn.health == null || this.parent.pawn.health.InPainShock || !this.parent.pawn.Spawned)
			{
				return;
			}
			if (!this.Props.hideMoteWhenNotDrafted || this.parent.pawn.Drafted)
			{
				if (this.Props.mote != null && (this.mote == null || this.mote.Destroyed))
				{
					this.mote = MoteMaker.MakeAttachedOverlay(this.parent.pawn, this.Props.mote, Vector3.zero, 1f, -1f);
				}
				if (this.mote != null)
				{
					this.mote.Maintain();
				}
			}
			List<Pawn> list;
			if (this.Props.onlyPawnsInSameFaction && this.parent.pawn.Faction != null)
			{
				list = this.parent.pawn.Map.mapPawns.SpawnedPawnsInFaction(this.parent.pawn.Faction);
			}
			else
			{
				list = this.parent.pawn.Map.mapPawns.AllPawnsSpawned;
			}
			foreach (Pawn pawn in list)
			{
				if (pawn.RaceProps.Humanlike && !pawn.Dead && pawn.health != null && pawn != this.parent.pawn && pawn.Position.DistanceTo(this.parent.pawn.Position) <= this.Props.range && this.Props.targetingParameters.CanTarget(pawn, null))
				{
					Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(this.Props.hediff, false);
					if (hediff == null)
					{
						hediff = pawn.health.AddHediff(this.Props.hediff, pawn.health.hediffSet.GetBrain(), null, null);
						hediff.Severity = this.Props.initialSeverity;
						HediffComp_Link hediffComp_Link = hediff.TryGetComp<HediffComp_Link>();
						if (hediffComp_Link != null)
						{
							hediffComp_Link.drawConnection = true;
							hediffComp_Link.other = this.parent.pawn;
						}
					}
					HediffComp_Disappears hediffComp_Disappears = hediff.TryGetComp<HediffComp_Disappears>();
					if (hediffComp_Disappears == null)
					{
						Log.Error("HediffComp_GiveHediffsInRange has a hediff in props which does not have a HediffComp_Disappears");
					}
					else
					{
						hediffComp_Disappears.ticksToDisappear = 5;
					}
				}
			}
		}

		// Token: 0x040010F5 RID: 4341
		private Mote mote;
	}
}
