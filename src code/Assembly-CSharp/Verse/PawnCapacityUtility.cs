using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000362 RID: 866
	public static class PawnCapacityUtility
	{
		// Token: 0x0600173C RID: 5948 RVA: 0x00088654 File Offset: 0x00086854
		public static bool BodyCanEverDoCapacity(BodyDef bodyDef, PawnCapacityDef capacity)
		{
			return capacity.Worker.CanHaveCapacity(bodyDef);
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x00088664 File Offset: 0x00086864
		public static float CalculateCapacityLevel(HediffSet diffSet, PawnCapacityDef capacity, List<PawnCapacityUtility.CapacityImpactor> impactors = null, bool forTradePrice = false)
		{
			if (capacity.zeroIfCannotBeAwake && !diffSet.pawn.health.capacities.CanBeAwake)
			{
				if (impactors != null)
				{
					impactors.Add(new PawnCapacityUtility.CapacityImpactorCapacity
					{
						capacity = PawnCapacityDefOf.Consciousness
					});
				}
				return 0f;
			}
			float num = capacity.Worker.CalculateCapacityLevel(diffSet, impactors);
			if (num > 0f)
			{
				float num2 = 99999f;
				float num3 = 1f;
				for (int i = 0; i < diffSet.hediffs.Count; i++)
				{
					Hediff hediff = diffSet.hediffs[i];
					if (!forTradePrice || hediff.def.priceImpact)
					{
						List<PawnCapacityModifier> capMods = hediff.CapMods;
						if (capMods != null)
						{
							for (int j = 0; j < capMods.Count; j++)
							{
								PawnCapacityModifier pawnCapacityModifier = capMods[j];
								if (pawnCapacityModifier.capacity == capacity)
								{
									num += pawnCapacityModifier.offset;
									float num4 = pawnCapacityModifier.postFactor;
									if (hediff.CurStage != null && hediff.CurStage.capacityFactorEffectMultiplier != null)
									{
										num4 = StatWorker.ScaleFactor(num4, hediff.pawn.GetStatValue(hediff.CurStage.capacityFactorEffectMultiplier, true, -1));
									}
									num3 *= num4;
									float num5 = pawnCapacityModifier.EvaluateSetMax(diffSet.pawn);
									if (num5 < num2)
									{
										num2 = num5;
									}
									if (impactors != null)
									{
										impactors.Add(new PawnCapacityUtility.CapacityImpactorHediff
										{
											hediff = hediff
										});
									}
								}
							}
						}
					}
				}
				if (ModsConfig.BiotechActive && diffSet.pawn.genes != null)
				{
					for (int k = 0; k < diffSet.pawn.genes.GenesListForReading.Count; k++)
					{
						Gene gene = diffSet.pawn.genes.GenesListForReading[k];
						List<PawnCapacityModifier> capMods2 = gene.def.capMods;
						if (!capMods2.NullOrEmpty<PawnCapacityModifier>())
						{
							for (int l = 0; l < capMods2.Count; l++)
							{
								PawnCapacityModifier pawnCapacityModifier2 = capMods2[l];
								if (pawnCapacityModifier2.capacity == capacity)
								{
									num += pawnCapacityModifier2.offset;
									num3 *= pawnCapacityModifier2.postFactor;
									float num6 = pawnCapacityModifier2.EvaluateSetMax(diffSet.pawn);
									if (num6 < num2)
									{
										num2 = num6;
									}
									if (impactors != null)
									{
										impactors.Add(new PawnCapacityUtility.CapacityImpactorGene
										{
											gene = gene
										});
									}
								}
							}
						}
					}
				}
				num *= num3;
				num = Mathf.Min(num, num2);
			}
			num = Mathf.Max(num, capacity.minValue);
			return GenMath.RoundedHundredth(num);
		}

		// Token: 0x0600173E RID: 5950 RVA: 0x000888D4 File Offset: 0x00086AD4
		public static float CalculatePartEfficiency(HediffSet diffSet, BodyPartRecord part, bool ignoreAddedParts = false, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			for (BodyPartRecord parent = part.parent; parent != null; parent = parent.parent)
			{
				if (diffSet.HasDirectlyAddedPartFor(parent))
				{
					Hediff_AddedPart firstHediffMatchingPart = diffSet.GetFirstHediffMatchingPart<Hediff_AddedPart>(parent);
					if (impactors != null)
					{
						impactors.Add(new PawnCapacityUtility.CapacityImpactorHediff
						{
							hediff = firstHediffMatchingPart
						});
					}
					return firstHediffMatchingPart.def.addedPartProps.partEfficiency;
				}
			}
			if (part.parent != null && diffSet.PartIsMissing(part.parent))
			{
				return 0f;
			}
			float num = 1f;
			if (!ignoreAddedParts)
			{
				for (int i = 0; i < diffSet.hediffs.Count; i++)
				{
					Hediff_AddedPart hediff_AddedPart = diffSet.hediffs[i] as Hediff_AddedPart;
					if (hediff_AddedPart != null && hediff_AddedPart.Part == part)
					{
						num *= hediff_AddedPart.def.addedPartProps.partEfficiency;
						if (hediff_AddedPart.def.addedPartProps.partEfficiency != 1f && impactors != null)
						{
							impactors.Add(new PawnCapacityUtility.CapacityImpactorHediff
							{
								hediff = hediff_AddedPart
							});
						}
					}
				}
			}
			float b = -1f;
			float num2 = 0f;
			bool flag = false;
			for (int j = 0; j < diffSet.hediffs.Count; j++)
			{
				if (diffSet.hediffs[j].Part == part && diffSet.hediffs[j].CurStage != null)
				{
					HediffStage curStage = diffSet.hediffs[j].CurStage;
					num2 += curStage.partEfficiencyOffset;
					flag |= curStage.partIgnoreMissingHP;
					if (curStage.partEfficiencyOffset != 0f && curStage.becomeVisible && impactors != null)
					{
						impactors.Add(new PawnCapacityUtility.CapacityImpactorHediff
						{
							hediff = diffSet.hediffs[j]
						});
					}
				}
			}
			if (!flag)
			{
				float num3 = diffSet.GetPartHealth(part) / part.def.GetMaxHealth(diffSet.pawn);
				if (num3 != 1f)
				{
					if (DamageWorker_AddInjury.ShouldReduceDamageToPreservePart(part))
					{
						num3 = Mathf.InverseLerp(0.1f, 1f, num3);
					}
					if (impactors != null)
					{
						impactors.Add(new PawnCapacityUtility.CapacityImpactorBodyPartHealth
						{
							bodyPart = part
						});
					}
					num *= num3;
				}
			}
			num += num2;
			if (num > 0.0001f)
			{
				num = Mathf.Max(num, b);
			}
			return Mathf.Max(num, 0f);
		}

		// Token: 0x0600173F RID: 5951 RVA: 0x00088B08 File Offset: 0x00086D08
		public static float CalculateImmediatePartEfficiencyAndRecord(HediffSet diffSet, BodyPartRecord part, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			if (diffSet.AncestorHasDirectlyAddedParts(part))
			{
				return 1f;
			}
			return PawnCapacityUtility.CalculatePartEfficiency(diffSet, part, false, impactors);
		}

		// Token: 0x06001740 RID: 5952 RVA: 0x00088B24 File Offset: 0x00086D24
		public static float CalculateNaturalPartsAverageEfficiency(HediffSet diffSet, BodyPartGroupDef bodyPartGroup)
		{
			float num = 0f;
			int num2 = 0;
			foreach (BodyPartRecord part in from x in diffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where x.groups.Contains(bodyPartGroup)
			select x)
			{
				if (!diffSet.PartOrAnyAncestorHasDirectlyAddedParts(part))
				{
					num += PawnCapacityUtility.CalculatePartEfficiency(diffSet, part, false, null);
				}
				num2++;
			}
			if (num2 == 0 || num < 0f)
			{
				return 0f;
			}
			return num / (float)num2;
		}

		// Token: 0x06001741 RID: 5953 RVA: 0x00088BC8 File Offset: 0x00086DC8
		public static float CalculateTagEfficiency(HediffSet diffSet, BodyPartTagDef tag, float maximum = 3.4028235E+38f, FloatRange lerp = default(FloatRange), List<PawnCapacityUtility.CapacityImpactor> impactors = null, float bestPartEfficiencySpecialWeight = -1f)
		{
			BodyDef body = diffSet.pawn.RaceProps.body;
			float num = 0f;
			int num2 = 0;
			float num3 = 0f;
			List<PawnCapacityUtility.CapacityImpactor> list = null;
			foreach (BodyPartRecord part in body.GetPartsWithTag(tag))
			{
				float num4 = PawnCapacityUtility.CalculatePartEfficiency(diffSet, part, false, list);
				if (impactors != null && num4 != 1f && list == null)
				{
					list = new List<PawnCapacityUtility.CapacityImpactor>();
					PawnCapacityUtility.CalculatePartEfficiency(diffSet, part, false, list);
				}
				num += num4;
				num3 = Mathf.Max(num3, num4);
				num2++;
			}
			if (num2 == 0)
			{
				return 1f;
			}
			float num5;
			if (bestPartEfficiencySpecialWeight >= 0f && num2 >= 2)
			{
				num5 = num3 * bestPartEfficiencySpecialWeight + (num - num3) / (float)(num2 - 1) * (1f - bestPartEfficiencySpecialWeight);
			}
			else
			{
				num5 = num / (float)num2;
			}
			float num6 = num5;
			if (lerp != default(FloatRange))
			{
				num6 = lerp.LerpThroughRange(num6);
			}
			num6 = Mathf.Min(num6, maximum);
			if (impactors != null && list != null && (maximum != 1f || num5 <= 1f || num6 == 1f))
			{
				impactors.AddRange(list);
			}
			return num6;
		}

		// Token: 0x06001742 RID: 5954 RVA: 0x00088D04 File Offset: 0x00086F04
		public static float CalculateLimbEfficiency(HediffSet diffSet, BodyPartTagDef limbCoreTag, BodyPartTagDef limbSegmentTag, BodyPartTagDef limbDigitTag, float appendageWeight, out float functionalPercentage, List<PawnCapacityUtility.CapacityImpactor> impactors)
		{
			BodyDef body = diffSet.pawn.RaceProps.body;
			float num = 0f;
			int num2 = 0;
			int num3 = 0;
			Func<BodyPartRecord, float> <>9__0;
			foreach (BodyPartRecord bodyPartRecord in body.GetPartsWithTag(limbCoreTag))
			{
				float num4 = PawnCapacityUtility.CalculateImmediatePartEfficiencyAndRecord(diffSet, bodyPartRecord, impactors);
				foreach (BodyPartRecord part in bodyPartRecord.GetConnectedParts(limbSegmentTag))
				{
					num4 *= PawnCapacityUtility.CalculateImmediatePartEfficiencyAndRecord(diffSet, part, impactors);
				}
				if (bodyPartRecord.HasChildParts(limbDigitTag))
				{
					float a = num4;
					float num5 = num4;
					IEnumerable<BodyPartRecord> childParts = bodyPartRecord.GetChildParts(limbDigitTag);
					Func<BodyPartRecord, float> selector;
					if ((selector = <>9__0) == null)
					{
						selector = (<>9__0 = ((BodyPartRecord digitPart) => PawnCapacityUtility.CalculateImmediatePartEfficiencyAndRecord(diffSet, digitPart, impactors)));
					}
					num4 = Mathf.Lerp(a, num5 * childParts.Average(selector), appendageWeight);
				}
				num += num4;
				num2++;
				if (num4 > 0f)
				{
					num3++;
				}
			}
			if (num2 == 0)
			{
				functionalPercentage = 0f;
				return 0f;
			}
			functionalPercentage = (float)num3 / (float)num2;
			return num / (float)num2;
		}

		// Token: 0x02001E26 RID: 7718
		public abstract class CapacityImpactor
		{
			// Token: 0x17001EAE RID: 7854
			// (get) Token: 0x0600B7C4 RID: 47044 RVA: 0x00002662 File Offset: 0x00000862
			public virtual bool IsDirect
			{
				get
				{
					return true;
				}
			}

			// Token: 0x0600B7C5 RID: 47045
			public abstract string Readable(Pawn pawn);
		}

		// Token: 0x02001E27 RID: 7719
		public class CapacityImpactorBodyPartHealth : PawnCapacityUtility.CapacityImpactor
		{
			// Token: 0x0600B7C7 RID: 47047 RVA: 0x00419F68 File Offset: 0x00418168
			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}: {1} / {2}", this.bodyPart.LabelCap, pawn.health.hediffSet.GetPartHealth(this.bodyPart), this.bodyPart.def.GetMaxHealth(pawn));
			}

			// Token: 0x040076F7 RID: 30455
			public BodyPartRecord bodyPart;
		}

		// Token: 0x02001E28 RID: 7720
		public class CapacityImpactorCapacity : PawnCapacityUtility.CapacityImpactor
		{
			// Token: 0x17001EAF RID: 7855
			// (get) Token: 0x0600B7C9 RID: 47049 RVA: 0x0000249D File Offset: 0x0000069D
			public override bool IsDirect
			{
				get
				{
					return false;
				}
			}

			// Token: 0x0600B7CA RID: 47050 RVA: 0x00419FC4 File Offset: 0x004181C4
			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}: {1}%", this.capacity.GetLabelFor(pawn).CapitalizeFirst(), (pawn.health.capacities.GetLevel(this.capacity) * 100f).ToString("F0"));
			}

			// Token: 0x040076F8 RID: 30456
			public PawnCapacityDef capacity;
		}

		// Token: 0x02001E29 RID: 7721
		public class CapacityImpactorHediff : PawnCapacityUtility.CapacityImpactor
		{
			// Token: 0x0600B7CC RID: 47052 RVA: 0x0041A015 File Offset: 0x00418215
			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}", this.hediff.LabelCap);
			}

			// Token: 0x040076F9 RID: 30457
			public Hediff hediff;
		}

		// Token: 0x02001E2A RID: 7722
		public class CapacityImpactorGene : PawnCapacityUtility.CapacityImpactor
		{
			// Token: 0x0600B7CE RID: 47054 RVA: 0x0041A02C File Offset: 0x0041822C
			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}", this.gene.LabelCap);
			}

			// Token: 0x040076FA RID: 30458
			public Gene gene;
		}

		// Token: 0x02001E2B RID: 7723
		public class CapacityImpactorPain : PawnCapacityUtility.CapacityImpactor
		{
			// Token: 0x17001EB0 RID: 7856
			// (get) Token: 0x0600B7D0 RID: 47056 RVA: 0x0000249D File Offset: 0x0000069D
			public override bool IsDirect
			{
				get
				{
					return false;
				}
			}

			// Token: 0x0600B7D1 RID: 47057 RVA: 0x0041A044 File Offset: 0x00418244
			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}: {1}%", "Pain".Translate(), (pawn.health.hediffSet.PainTotal * 100f).ToString("F0"));
			}
		}
	}
}
