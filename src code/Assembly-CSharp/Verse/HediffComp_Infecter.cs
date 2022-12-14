using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002FC RID: 764
	public class HediffComp_Infecter : HediffComp
	{
		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x0600151B RID: 5403 RVA: 0x0007F477 File Offset: 0x0007D677
		public HediffCompProperties_Infecter Props
		{
			get
			{
				return (HediffCompProperties_Infecter)this.props;
			}
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x0007F484 File Offset: 0x0007D684
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			if (this.parent.IsPermanent())
			{
				this.ticksUntilInfect = -2;
				return;
			}
			if (this.parent.Part.def.IsSolid(this.parent.Part, base.Pawn.health.hediffSet.hediffs))
			{
				this.ticksUntilInfect = -2;
				return;
			}
			if (base.Pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(this.parent.Part))
			{
				this.ticksUntilInfect = -2;
				return;
			}
			float num = this.Props.infectionChance;
			if (base.Pawn.RaceProps.Animal)
			{
				num *= 0.1f;
			}
			if (Rand.Value <= num)
			{
				this.ticksUntilInfect = HealthTuning.InfectionDelayRange.RandomInRange;
				return;
			}
			this.ticksUntilInfect = -2;
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x0007F55C File Offset: 0x0007D75C
		public override void CompExposeData()
		{
			Scribe_Values.Look<float>(ref this.infectionChanceFactorFromTendRoom, "infectionChanceFactor", 0f, false);
			Scribe_Values.Look<int>(ref this.ticksUntilInfect, "ticksUntilInfect", -2, false);
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x0007F587 File Offset: 0x0007D787
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (this.ticksUntilInfect > 0)
			{
				this.ticksUntilInfect--;
				if (this.ticksUntilInfect == 0)
				{
					this.CheckMakeInfection();
				}
			}
		}

		// Token: 0x0600151F RID: 5407 RVA: 0x0007F5B0 File Offset: 0x0007D7B0
		public override void CompTended(float quality, float maxQuality, int batchPosition = 0)
		{
			base.CompTended(quality, maxQuality, batchPosition);
			if (base.Pawn.Spawned)
			{
				Room room = base.Pawn.GetRoom(RegionType.Set_All);
				if (room != null)
				{
					this.infectionChanceFactorFromTendRoom = room.GetStat(RoomStatDefOf.InfectionChanceFactor);
				}
			}
		}

		// Token: 0x06001520 RID: 5408 RVA: 0x0007F5F8 File Offset: 0x0007D7F8
		private void CheckMakeInfection()
		{
			if (base.Pawn.health.immunity.DiseaseContractChanceFactor(HediffDefOf.WoundInfection, this.parent.Part) <= 0.001f)
			{
				this.ticksUntilInfect = -3;
				return;
			}
			float num = 1f;
			HediffComp_TendDuration hediffComp_TendDuration = this.parent.TryGetComp<HediffComp_TendDuration>();
			if (hediffComp_TendDuration != null && hediffComp_TendDuration.IsTended)
			{
				num *= this.infectionChanceFactorFromTendRoom;
				num *= HediffComp_Infecter.InfectionChanceFactorFromTendQualityCurve.Evaluate(hediffComp_TendDuration.tendQuality);
			}
			num *= HediffComp_Infecter.InfectionChanceFactorFromSeverityCurve.Evaluate(this.parent.Severity);
			if (base.Pawn.Faction == Faction.OfPlayer)
			{
				num *= Find.Storyteller.difficulty.playerPawnInfectionChanceFactor;
			}
			if (Rand.Value < num)
			{
				this.ticksUntilInfect = -4;
				base.Pawn.health.AddHediff(HediffDefOf.WoundInfection, this.parent.Part, null, null);
				return;
			}
			this.ticksUntilInfect = -3;
		}

		// Token: 0x06001521 RID: 5409 RVA: 0x0007F6F4 File Offset: 0x0007D8F4
		public override string CompDebugString()
		{
			if (this.ticksUntilInfect > 0)
			{
				return string.Concat(new object[]
				{
					"infection may appear in: ",
					this.ticksUntilInfect,
					" ticks\ninfectChnceFactorFromTendRoom: ",
					this.infectionChanceFactorFromTendRoom.ToStringPercent()
				});
			}
			if (this.ticksUntilInfect == -4)
			{
				return "already created infection";
			}
			if (this.ticksUntilInfect == -3)
			{
				return "failed to make infection";
			}
			if (this.ticksUntilInfect == -2)
			{
				return "will not make infection";
			}
			if (this.ticksUntilInfect == -1)
			{
				return "uninitialized data!";
			}
			return "unexpected ticksUntilInfect = " + this.ticksUntilInfect;
		}

		// Token: 0x04001108 RID: 4360
		private int ticksUntilInfect = -1;

		// Token: 0x04001109 RID: 4361
		private float infectionChanceFactorFromTendRoom = 1f;

		// Token: 0x0400110A RID: 4362
		private const int UninitializedValue = -1;

		// Token: 0x0400110B RID: 4363
		private const int WillNotInfectValue = -2;

		// Token: 0x0400110C RID: 4364
		private const int FailedToMakeInfectionValue = -3;

		// Token: 0x0400110D RID: 4365
		private const int AlreadyMadeInfectionValue = -4;

		// Token: 0x0400110E RID: 4366
		private static readonly SimpleCurve InfectionChanceFactorFromTendQualityCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.7f),
				true
			},
			{
				new CurvePoint(1f, 0.4f),
				true
			}
		};

		// Token: 0x0400110F RID: 4367
		private static readonly SimpleCurve InfectionChanceFactorFromSeverityCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 0.1f),
				true
			},
			{
				new CurvePoint(12f, 1f),
				true
			}
		};
	}
}
