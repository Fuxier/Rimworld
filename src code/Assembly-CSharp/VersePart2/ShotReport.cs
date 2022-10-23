using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200059F RID: 1439
	public struct ShotReport
	{
		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x06002BCC RID: 11212 RVA: 0x001160F8 File Offset: 0x001142F8
		private float FactorFromPosture
		{
			get
			{
				if (this.target.HasThing)
				{
					Pawn pawn = this.target.Thing as Pawn;
					if (pawn != null && this.distance >= 4.5f && pawn.GetPosture() != PawnPosture.Standing)
					{
						return 0.2f;
					}
				}
				return 1f;
			}
		}

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x06002BCD RID: 11213 RVA: 0x00116148 File Offset: 0x00114348
		private float FactorFromExecution
		{
			get
			{
				if (this.target.HasThing)
				{
					Pawn pawn = this.target.Thing as Pawn;
					if (pawn != null && this.distance <= 3.9f && pawn.GetPosture() != PawnPosture.Standing)
					{
						return 7.5f;
					}
				}
				return 1f;
			}
		}

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x06002BCE RID: 11214 RVA: 0x00116198 File Offset: 0x00114398
		public float AimOnTargetChance_StandardTarget
		{
			get
			{
				float num = this.factorFromShooterAndDist * this.factorFromEquipment * this.factorFromWeather * this.factorFromCoveringGas * this.FactorFromExecution;
				num += this.offsetFromDarkness;
				if (num < 0.0201f)
				{
					num = 0.0201f;
				}
				return num;
			}
		}

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x06002BCF RID: 11215 RVA: 0x001161E0 File Offset: 0x001143E0
		public float AimOnTargetChance_IgnoringPosture
		{
			get
			{
				return this.AimOnTargetChance_StandardTarget * this.factorFromTargetSize;
			}
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x06002BD0 RID: 11216 RVA: 0x001161EF File Offset: 0x001143EF
		public float AimOnTargetChance
		{
			get
			{
				return this.AimOnTargetChance_IgnoringPosture * this.FactorFromPosture;
			}
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x06002BD1 RID: 11217 RVA: 0x001161FE File Offset: 0x001143FE
		public float PassCoverChance
		{
			get
			{
				return 1f - this.coversOverallBlockChance;
			}
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x06002BD2 RID: 11218 RVA: 0x0011620C File Offset: 0x0011440C
		public float TotalEstimatedHitChance
		{
			get
			{
				return Mathf.Clamp01(this.AimOnTargetChance * this.PassCoverChance);
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x06002BD3 RID: 11219 RVA: 0x00116220 File Offset: 0x00114420
		public ShootLine ShootLine
		{
			get
			{
				return this.shootLine;
			}
		}

		// Token: 0x06002BD4 RID: 11220 RVA: 0x00116228 File Offset: 0x00114428
		public static ShotReport HitReportFor(Thing caster, Verb verb, LocalTargetInfo target)
		{
			IntVec3 cell = target.Cell;
			ShotReport shotReport;
			shotReport.distance = (cell - caster.Position).LengthHorizontal;
			shotReport.target = target.ToTargetInfo(caster.Map);
			shotReport.factorFromShooterAndDist = ShotReport.HitFactorFromShooter(caster, shotReport.distance);
			shotReport.factorFromEquipment = verb.verbProps.GetHitChanceFactor(verb.EquipmentSource, shotReport.distance);
			shotReport.covers = CoverUtility.CalculateCoverGiverSet(target, caster.Position, caster.Map);
			shotReport.coversOverallBlockChance = CoverUtility.CalculateOverallBlockChance(target, caster.Position, caster.Map);
			shotReport.factorFromCoveringGas = 1f;
			if (verb.TryFindShootLineFromTo(verb.caster.Position, target, out shotReport.shootLine))
			{
				using (IEnumerator<IntVec3> enumerator = shotReport.shootLine.Points().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.AnyGas(caster.Map, GasType.BlindSmoke))
						{
							shotReport.factorFromCoveringGas = 0.7f;
						}
					}
					goto IL_11F;
				}
			}
			shotReport.shootLine = new ShootLine(IntVec3.Invalid, IntVec3.Invalid);
			IL_11F:
			if (!caster.Position.Roofed(caster.Map) || !target.Cell.Roofed(caster.Map))
			{
				shotReport.factorFromWeather = caster.Map.weatherManager.CurWeatherAccuracyMultiplier;
			}
			else
			{
				shotReport.factorFromWeather = 1f;
			}
			if (target.HasThing)
			{
				Pawn pawn = target.Thing as Pawn;
				if (pawn != null)
				{
					shotReport.factorFromTargetSize = pawn.BodySize;
				}
				else
				{
					shotReport.factorFromTargetSize = target.Thing.def.fillPercent * (float)target.Thing.def.size.x * (float)target.Thing.def.size.z * 2.5f;
				}
				shotReport.factorFromTargetSize = Mathf.Clamp(shotReport.factorFromTargetSize, 0.5f, 2f);
			}
			else
			{
				shotReport.factorFromTargetSize = 1f;
			}
			shotReport.forcedMissRadius = verb.verbProps.ForcedMissRadius;
			shotReport.offsetFromDarkness = 0f;
			if (ModsConfig.IdeologyActive && target.HasThing)
			{
				if (DarknessCombatUtility.IsOutdoorsAndLit(target.Thing))
				{
					shotReport.offsetFromDarkness = caster.GetStatValue(StatDefOf.ShootingAccuracyOutdoorsLitOffset, true, -1);
				}
				else if (DarknessCombatUtility.IsOutdoorsAndDark(target.Thing))
				{
					shotReport.offsetFromDarkness = caster.GetStatValue(StatDefOf.ShootingAccuracyOutdoorsDarkOffset, true, -1);
				}
				else if (DarknessCombatUtility.IsIndoorsAndDark(target.Thing))
				{
					shotReport.offsetFromDarkness = caster.GetStatValue(StatDefOf.ShootingAccuracyIndoorsDarkOffset, true, -1);
				}
				else if (DarknessCombatUtility.IsIndoorsAndLit(target.Thing))
				{
					shotReport.offsetFromDarkness = caster.GetStatValue(StatDefOf.ShootingAccuracyIndoorsLitOffset, true, -1);
				}
			}
			return shotReport;
		}

		// Token: 0x06002BD5 RID: 11221 RVA: 0x0011651C File Offset: 0x0011471C
		public static float HitFactorFromShooter(Thing caster, float distance)
		{
			float num;
			if (caster is Pawn)
			{
				num = caster.GetStatValue(StatDefOf.ShootingAccuracyPawn, true, -1);
				if (distance <= 3f)
				{
					num *= caster.GetStatValue(StatDefOf.ShootingAccuracyFactor_Touch, true, -1);
				}
				else if (distance <= 12f)
				{
					num *= Mathf.Lerp(caster.GetStatValue(StatDefOf.ShootingAccuracyFactor_Touch, true, -1), caster.GetStatValue(StatDefOf.ShootingAccuracyFactor_Short, true, -1), (distance - 3f) / 12f - 3f);
				}
				else if (distance <= 25f)
				{
					num *= Mathf.Lerp(caster.GetStatValue(StatDefOf.ShootingAccuracyFactor_Short, true, -1), caster.GetStatValue(StatDefOf.ShootingAccuracyFactor_Medium, true, -1), (distance - 12f) / 25f - 12f);
				}
				else if (distance <= 40f)
				{
					num *= Mathf.Lerp(caster.GetStatValue(StatDefOf.ShootingAccuracyFactor_Medium, true, -1), caster.GetStatValue(StatDefOf.ShootingAccuracyFactor_Long, true, -1), (distance - 25f) / 40f - 25f);
				}
				else
				{
					num *= caster.GetStatValue(StatDefOf.ShootingAccuracyFactor_Long, true, -1);
				}
			}
			else
			{
				num = caster.GetStatValue(StatDefOf.ShootingAccuracyTurret, true, -1);
			}
			return ShotReport.HitFactorFromShooter(num, distance);
		}

		// Token: 0x06002BD6 RID: 11222 RVA: 0x00116646 File Offset: 0x00114846
		public static float HitFactorFromShooter(float accRating, float distance)
		{
			return Mathf.Max(Mathf.Pow(accRating, distance), 0.0201f);
		}

		// Token: 0x06002BD7 RID: 11223 RVA: 0x0011665C File Offset: 0x0011485C
		public string GetTextReadout()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.forcedMissRadius > 0.5f)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("WeaponMissRadius".Translate() + ": " + this.forcedMissRadius.ToString("F1"));
				stringBuilder.AppendLine("DirectHitChance".Translate() + ": " + (1f / (float)GenRadial.NumCellsInRadius(this.forcedMissRadius)).ToStringPercent());
			}
			else
			{
				stringBuilder.AppendLine(this.TotalEstimatedHitChance.ToStringPercent());
				stringBuilder.AppendLine("   " + "ShootReportShooterAbility".Translate() + ": " + this.factorFromShooterAndDist.ToStringPercent());
				stringBuilder.AppendLine("   " + "ShootReportWeapon".Translate() + ": " + this.factorFromEquipment.ToStringPercent());
				if (this.target.HasThing && this.factorFromTargetSize != 1f)
				{
					stringBuilder.AppendLine("   " + "TargetSize".Translate() + ": " + this.factorFromTargetSize.ToStringPercent());
				}
				if (this.factorFromWeather < 0.99f)
				{
					stringBuilder.AppendLine("   " + "Weather".Translate() + ": " + this.factorFromWeather.ToStringPercent());
				}
				if (this.factorFromCoveringGas < 0.99f)
				{
					stringBuilder.AppendLine("   " + "BlindSmoke".Translate().CapitalizeFirst() + ": " + this.factorFromCoveringGas.ToStringPercent());
				}
				if (this.FactorFromPosture < 0.9999f)
				{
					stringBuilder.AppendLine("   " + "TargetProne".Translate() + ": " + this.FactorFromPosture.ToStringPercent());
				}
				if (this.FactorFromExecution != 1f)
				{
					stringBuilder.AppendLine("   " + "Execution".Translate() + ": " + this.FactorFromExecution.ToStringPercent());
				}
				if (ModsConfig.IdeologyActive && this.target.HasThing && this.offsetFromDarkness != 0f)
				{
					if (DarknessCombatUtility.IsOutdoorsAndLit(this.target.Thing))
					{
						stringBuilder.AppendLine("   " + StatDefOf.ShootingAccuracyOutdoorsLitOffset.LabelCap + ": " + this.offsetFromDarkness.ToStringPercent());
					}
					else if (DarknessCombatUtility.IsOutdoorsAndDark(this.target.Thing))
					{
						stringBuilder.AppendLine("   " + StatDefOf.ShootingAccuracyOutdoorsDarkOffset.LabelCap + ": " + this.offsetFromDarkness.ToStringPercent());
					}
					else if (DarknessCombatUtility.IsIndoorsAndDark(this.target.Thing))
					{
						stringBuilder.AppendLine("   " + StatDefOf.ShootingAccuracyIndoorsDarkOffset.LabelCap + ": " + this.offsetFromDarkness.ToStringPercent());
					}
					else if (DarknessCombatUtility.IsIndoorsAndLit(this.target.Thing))
					{
						stringBuilder.AppendLine("   " + StatDefOf.ShootingAccuracyIndoorsLitOffset.LabelCap + "   " + this.offsetFromDarkness.ToStringPercent());
					}
				}
				if (this.PassCoverChance < 1f)
				{
					stringBuilder.AppendLine("   " + "ShootingCover".Translate() + ": " + this.PassCoverChance.ToStringPercent());
					for (int i = 0; i < this.covers.Count; i++)
					{
						CoverInfo coverInfo = this.covers[i];
						if (coverInfo.BlockChance > 0f)
						{
							stringBuilder.AppendLine("     " + "CoverThingBlocksPercentOfShots".Translate(coverInfo.Thing.LabelCap, coverInfo.BlockChance.ToStringPercent(), new NamedArgument(coverInfo.Thing.def, "COVER")).CapitalizeFirst());
						}
					}
				}
				else
				{
					stringBuilder.AppendLine("   (" + "NoCoverLower".Translate() + ")");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002BD8 RID: 11224 RVA: 0x00116B70 File Offset: 0x00114D70
		public Thing GetRandomCoverToMissInto()
		{
			CoverInfo coverInfo;
			if (this.covers.TryRandomElementByWeight((CoverInfo c) => c.BlockChance, out coverInfo))
			{
				return coverInfo.Thing;
			}
			return null;
		}

		// Token: 0x04001CD5 RID: 7381
		private TargetInfo target;

		// Token: 0x04001CD6 RID: 7382
		private float distance;

		// Token: 0x04001CD7 RID: 7383
		private List<CoverInfo> covers;

		// Token: 0x04001CD8 RID: 7384
		private float coversOverallBlockChance;

		// Token: 0x04001CD9 RID: 7385
		private float factorFromShooterAndDist;

		// Token: 0x04001CDA RID: 7386
		private float factorFromEquipment;

		// Token: 0x04001CDB RID: 7387
		private float factorFromTargetSize;

		// Token: 0x04001CDC RID: 7388
		private float factorFromWeather;

		// Token: 0x04001CDD RID: 7389
		private float forcedMissRadius;

		// Token: 0x04001CDE RID: 7390
		private float offsetFromDarkness;

		// Token: 0x04001CDF RID: 7391
		private float factorFromCoveringGas;

		// Token: 0x04001CE0 RID: 7392
		private ShootLine shootLine;
	}
}
