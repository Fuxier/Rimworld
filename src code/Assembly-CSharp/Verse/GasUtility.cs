using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001E7 RID: 487
	public static class GasUtility
	{
		// Token: 0x06000DAE RID: 3502 RVA: 0x0004BFD0 File Offset: 0x0004A1D0
		public static string GetLabel(this GasType gasType)
		{
			if (gasType == GasType.BlindSmoke)
			{
				return "BlindSmoke".Translate();
			}
			if (gasType == GasType.ToxGas)
			{
				return "ToxGas".Translate();
			}
			if (gasType != GasType.RotStink)
			{
				Log.ErrorOnce("Trying to get unknown gas type label.", 172091);
				return string.Empty;
			}
			return "RotStink".Translate();
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x0004C030 File Offset: 0x0004A230
		public static void AddGas(IntVec3 cell, Map map, GasType gasType, float radius)
		{
			int num = GenRadial.NumCellsInRadius(radius);
			map.gasGrid.AddGas(cell, gasType, 255 * num, true);
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x0004C059 File Offset: 0x0004A259
		public static void AddGas(IntVec3 cell, Map map, GasType gasType, int amount)
		{
			map.gasGrid.AddGas(cell, gasType, amount, true);
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x0004C06A File Offset: 0x0004A26A
		public static byte GasDentity(this IntVec3 cell, Map map, GasType gasType)
		{
			return map.gasGrid.DensityAt(cell, gasType);
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x0004C079 File Offset: 0x0004A279
		public static bool AnyGas(this IntVec3 cell, Map map, GasType gasType)
		{
			return map.gasGrid.DensityAt(cell, gasType) > 0;
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x0004C08C File Offset: 0x0004A28C
		public static int RotStinkToSpawnForCorpse(Corpse corpse)
		{
			if (GenTemperature.RotRateAtTemperature((float)Mathf.RoundToInt(corpse.AmbientTemperature)) <= 0f)
			{
				return 0;
			}
			if (corpse.GetRotStage() == RotStage.Rotting)
			{
				float num = corpse.InnerPawn.BodySize;
				if (corpse.InnerPawn.RaceProps.Humanlike)
				{
					num *= 1.15f;
				}
				return Mathf.CeilToInt(num * 52f);
			}
			return 0;
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x0004C0F0 File Offset: 0x0004A2F0
		public static void PawnGasEffectsTick(Pawn pawn)
		{
			if (!ModsConfig.BiotechActive || !pawn.Spawned || !pawn.IsHashIntervalTick(50))
			{
				return;
			}
			byte b = pawn.Position.GasDentity(pawn.Map, GasType.ToxGas);
			if (b > 0)
			{
				float num = (float)b / 255f;
				Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ToxicBuildup, false);
				if (firstHediffOfDef != null && firstHediffOfDef.CurStageIndex == firstHediffOfDef.def.stages.Count - 1)
				{
					num *= 0.25f;
				}
				if (GasUtility.ShouldGetGasExposureHediff(pawn))
				{
					pawn.health.AddHediff(HediffDefOf.ToxGasExposure, null, null, null);
				}
				GameCondition_ToxicFallout.DoPawnToxicDamage(pawn, false, num);
			}
			if (pawn.Spawned && pawn.Position.GasDentity(pawn.Map, GasType.RotStink) > 0 && (pawn.RaceProps.Animal || pawn.RaceProps.Humanlike) && !pawn.health.hediffSet.HasHediff(HediffDefOf.LungRotExposure, false) && GasUtility.GetLungRotAffectedBodyParts(pawn).Any<BodyPartRecord>())
			{
				pawn.health.AddHediff(HediffDefOf.LungRotExposure, null, null, null);
			}
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x0004C218 File Offset: 0x0004A418
		public static IEnumerable<BodyPartRecord> GetLungRotAffectedBodyParts(Pawn pawn)
		{
			return from p in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where p.def == BodyPartDefOf.Lung && !pawn.health.hediffSet.hediffs.Any((Hediff x) => x.Part == p && x.def.preventsLungRot)
			select p;
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x0004C25C File Offset: 0x0004A45C
		private static bool ShouldGetGasExposureHediff(Pawn pawn)
		{
			if (!pawn.RaceProps.Humanlike && !pawn.RaceProps.Humanlike)
			{
				return false;
			}
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.ToxGasExposure, false))
			{
				return false;
			}
			if (pawn.apparel != null)
			{
				using (List<Apparel>.Enumerator enumerator = pawn.apparel.WornApparel.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def.apparel.immuneToToxGasExposure)
						{
							return false;
						}
					}
				}
			}
			if (pawn.genes != null)
			{
				using (List<Gene>.Enumerator enumerator2 = pawn.genes.GenesListForReading.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.def.immuneToToxGasExposure)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x04000C50 RID: 3152
		public const float BlindingGasAccuracyPenalty = 0.7f;

		// Token: 0x04000C51 RID: 3153
		public const int RotStinkPerRawMeatRotting = 2;

		// Token: 0x04000C52 RID: 3154
		private const float RotStinkPerBodySize = 52f;

		// Token: 0x04000C53 RID: 3155
		private const float RotStinkHumanlikeFactor = 1.15f;

		// Token: 0x04000C54 RID: 3156
		private const int GasCheckInterval = 50;

		// Token: 0x04000C55 RID: 3157
		private const float ToxGasEffectOnExtremeBuildupFactor = 0.25f;
	}
}
