using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002C4 RID: 708
	public static class HealthUtility
	{
		// Token: 0x060013F7 RID: 5111 RVA: 0x0007A02C File Offset: 0x0007822C
		public static string GetGeneralConditionLabel(Pawn pawn, bool shortVersion = false)
		{
			if (pawn.health.Dead)
			{
				return "Dead".Translate();
			}
			if (pawn.Deathresting)
			{
				return "Deathresting".Translate().CapitalizeFirst();
			}
			if (!pawn.health.capacities.CanBeAwake)
			{
				return "Unconscious".Translate();
			}
			if (pawn.health.InPainShock)
			{
				return (shortVersion && "PainShockShort".CanTranslate()) ? "PainShockShort".Translate() : "PainShock".Translate();
			}
			if (pawn.Downed && !LifeStageUtility.AlwaysDowned(pawn))
			{
				return "Incapacitated".Translate();
			}
			bool flag = false;
			for (int i = 0; i < pawn.health.hediffSet.hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = pawn.health.hediffSet.hediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && !hediff_Injury.IsPermanent())
				{
					flag = true;
				}
			}
			if (flag)
			{
				return "Injured".Translate();
			}
			if (pawn.health.hediffSet.PainTotal > 0.3f)
			{
				return "InPain".Translate();
			}
			return "Healthy".Translate();
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x0007A184 File Offset: 0x00078384
		public static Pair<string, Color> GetPartConditionLabel(Pawn pawn, BodyPartRecord part)
		{
			float partHealth = pawn.health.hediffSet.GetPartHealth(part);
			float maxHealth = part.def.GetMaxHealth(pawn);
			float num = partHealth / maxHealth;
			Color second = Color.white;
			string first;
			if (partHealth <= 0f)
			{
				Hediff_MissingPart hediff_MissingPart = null;
				List<Hediff_MissingPart> missingPartsCommonAncestors = pawn.health.hediffSet.GetMissingPartsCommonAncestors();
				for (int i = 0; i < missingPartsCommonAncestors.Count; i++)
				{
					if (missingPartsCommonAncestors[i].Part == part)
					{
						hediff_MissingPart = missingPartsCommonAncestors[i];
						break;
					}
				}
				if (hediff_MissingPart == null)
				{
					bool fresh = false;
					if (hediff_MissingPart != null && hediff_MissingPart.IsFreshNonSolidExtremity)
					{
						fresh = true;
					}
					bool solid = part.def.IsSolid(part, pawn.health.hediffSet.hediffs);
					first = HealthUtility.GetGeneralDestroyedPartLabel(part, fresh, solid);
					second = Color.gray;
				}
				else
				{
					first = hediff_MissingPart.LabelCap;
					second = hediff_MissingPart.LabelColor;
				}
			}
			else if (num < 0.4f)
			{
				first = "SeriouslyImpaired".Translate();
				second = HealthUtility.RedColor;
			}
			else if (num < 0.7f)
			{
				first = "Impaired".Translate();
				second = HealthUtility.ImpairedColor;
			}
			else if (num < 0.999f)
			{
				first = "SlightlyImpaired".Translate();
				second = HealthUtility.SlightlyImpairedColor;
			}
			else
			{
				first = "GoodCondition".Translate();
				second = HealthUtility.GoodConditionColor;
			}
			return new Pair<string, Color>(first, second);
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x0007A2EC File Offset: 0x000784EC
		public static string GetGeneralDestroyedPartLabel(BodyPartRecord part, bool fresh, bool solid)
		{
			if (part.parent == null)
			{
				return "SeriouslyImpaired".Translate();
			}
			if (part.depth != BodyPartDepth.Inside && !fresh)
			{
				return "MissingBodyPart".Translate();
			}
			if (solid)
			{
				return "ShatteredBodyPart".Translate();
			}
			return "DestroyedBodyPart".Translate();
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x0007A350 File Offset: 0x00078550
		private static IEnumerable<BodyPartRecord> HittablePartsViolence(HediffSet bodyModel)
		{
			return from x in bodyModel.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where x.depth == BodyPartDepth.Outside || (x.depth == BodyPartDepth.Inside && x.def.IsSolid(x, bodyModel.hediffs))
			select x;
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x0007A38C File Offset: 0x0007858C
		public static void HealNonPermanentInjuriesAndRestoreLegs(Pawn p)
		{
			if (p.Dead)
			{
				return;
			}
			HealthUtility.tmpHediffs.Clear();
			HealthUtility.tmpHediffs.AddRange(p.health.hediffSet.hediffs);
			for (int i = 0; i < HealthUtility.tmpHediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = HealthUtility.tmpHediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && !hediff_Injury.IsPermanent())
				{
					p.health.RemoveHediff(hediff_Injury);
				}
				else
				{
					Hediff_MissingPart hediff_MissingPart = HealthUtility.tmpHediffs[i] as Hediff_MissingPart;
					if (hediff_MissingPart != null && hediff_MissingPart.Part.def.tags.Contains(BodyPartTagDefOf.MovingLimbCore) && (hediff_MissingPart.Part.parent == null || p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Contains(hediff_MissingPart.Part.parent)))
					{
						p.health.RestorePart(hediff_MissingPart.Part, null, true);
					}
				}
			}
			HealthUtility.tmpHediffs.Clear();
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x0007A48C File Offset: 0x0007868C
		[Obsolete]
		public static void HealNonPermanentInjuriesAndFreshWounds(Pawn p)
		{
			if (p.Dead)
			{
				return;
			}
			HealthUtility.tmpHediffs.Clear();
			HealthUtility.tmpHediffs.AddRange(p.health.hediffSet.hediffs);
			foreach (Hediff hediff in HealthUtility.tmpHediffs)
			{
				if (hediff.def.everCurableByItem)
				{
					Hediff_Injury hediff_Injury;
					Hediff_MissingPart hediff_MissingPart;
					if ((hediff_Injury = (hediff as Hediff_Injury)) != null && !hediff_Injury.IsPermanent())
					{
						p.health.RemoveHediff(hediff_Injury);
					}
					else if ((hediff_MissingPart = (hediff as Hediff_MissingPart)) != null && hediff_MissingPart.IsFresh)
					{
						hediff_MissingPart.IsFresh = false;
						p.health.Notify_HediffChanged(hediff_MissingPart);
					}
				}
			}
			HealthUtility.tmpHediffs.Clear();
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x0007A560 File Offset: 0x00078760
		[Obsolete]
		public static Hediff_Injury HealRandomPermanentInjury(Pawn p)
		{
			if (p.Dead)
			{
				return null;
			}
			HealthUtility.tmpHediffs.Clear();
			foreach (Hediff hediff in p.health.hediffSet.hediffs)
			{
				Hediff_Injury hd;
				if (hediff.def.everCurableByItem && (hd = (hediff as Hediff_Injury)) != null && hd.IsPermanent())
				{
					HealthUtility.tmpHediffs.Add(hediff);
				}
			}
			Hediff_Injury hediff_Injury = HealthUtility.tmpHediffs.RandomElementWithFallback(null) as Hediff_Injury;
			if (hediff_Injury != null)
			{
				p.health.RemoveHediff(hediff_Injury);
			}
			HealthUtility.tmpHediffs.Clear();
			return hediff_Injury;
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x0007A620 File Offset: 0x00078820
		public static void GiveRandomSurgeryInjuries(Pawn p, int totalDamage, BodyPartRecord operatedPart)
		{
			IEnumerable<BodyPartRecord> source;
			if (operatedPart == null)
			{
				source = from x in p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
				where !x.def.conceptual
				select x;
			}
			else
			{
				source = from x in p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
				where !x.def.conceptual
				select x into pa
				where pa == operatedPart || pa.parent == operatedPart || (operatedPart != null && operatedPart.parent == pa)
				select pa;
			}
			source = from x in source
			where HealthUtility.GetMinHealthOfPartsWeWantToAvoidDestroying(x, p) >= 2f
			select x;
			BodyPartRecord brain = p.health.hediffSet.GetBrain();
			if (brain != null)
			{
				float maxBrainHealth = brain.def.GetMaxHealth(p);
				source = from x in source
				where x != brain || p.health.hediffSet.GetPartHealth(x) >= maxBrainHealth * 0.5f + 1f
				select x;
			}
			while (totalDamage > 0 && source.Any<BodyPartRecord>())
			{
				BodyPartRecord bodyPartRecord = source.RandomElementByWeight((BodyPartRecord x) => x.coverageAbs);
				float partHealth = p.health.hediffSet.GetPartHealth(bodyPartRecord);
				int num = Mathf.Max(3, GenMath.RoundRandom(partHealth * Rand.Range(0.5f, 1f)));
				float minHealthOfPartsWeWantToAvoidDestroying = HealthUtility.GetMinHealthOfPartsWeWantToAvoidDestroying(bodyPartRecord, p);
				if (minHealthOfPartsWeWantToAvoidDestroying - (float)num < 1f)
				{
					num = Mathf.RoundToInt(minHealthOfPartsWeWantToAvoidDestroying - 1f);
				}
				if (bodyPartRecord == brain && partHealth - (float)num < brain.def.GetMaxHealth(p) * 0.5f)
				{
					num = Mathf.Max(Mathf.RoundToInt(partHealth - brain.def.GetMaxHealth(p) * 0.5f), 1);
				}
				if (num <= 0)
				{
					break;
				}
				DamageDef def = Rand.Element<DamageDef>(DamageDefOf.Cut, DamageDefOf.Scratch, DamageDefOf.Stab, DamageDefOf.Crush);
				DamageInfo dinfo = new DamageInfo(def, (float)num, 0f, -1f, null, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true);
				dinfo.SetIgnoreArmor(true);
				dinfo.SetIgnoreInstantKillProtection(true);
				p.TakeDamage(dinfo);
				totalDamage -= num;
			}
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x0007A8AC File Offset: 0x00078AAC
		private static float GetMinHealthOfPartsWeWantToAvoidDestroying(BodyPartRecord part, Pawn pawn)
		{
			float num = 999999f;
			while (part != null)
			{
				if (HealthUtility.ShouldRandomSurgeryInjuriesAvoidDestroying(part, pawn))
				{
					num = Mathf.Min(num, pawn.health.hediffSet.GetPartHealth(part));
				}
				part = part.parent;
			}
			return num;
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x0007A8F0 File Offset: 0x00078AF0
		private static bool ShouldRandomSurgeryInjuriesAvoidDestroying(BodyPartRecord part, Pawn pawn)
		{
			if (part == pawn.RaceProps.body.corePart)
			{
				return true;
			}
			if (part.def.tags.Any((BodyPartTagDef x) => x.vital))
			{
				return true;
			}
			for (int i = 0; i < part.parts.Count; i++)
			{
				if (HealthUtility.ShouldRandomSurgeryInjuriesAvoidDestroying(part.parts[i], pawn))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x0007A974 File Offset: 0x00078B74
		public static void DamageUntilDowned(Pawn p, bool allowBleedingWounds = true)
		{
			if (p.Downed)
			{
				return;
			}
			HediffSet hediffSet = p.health.hediffSet;
			p.health.forceDowned = true;
			IEnumerable<BodyPartRecord> source = from x in HealthUtility.HittablePartsViolence(hediffSet)
			where !p.health.hediffSet.hediffs.Any((Hediff y) => y.Part == x && y.CurStage != null && y.CurStage.partEfficiencyOffset < 0f)
			select x;
			int num = 0;
			while (num < 300 && !p.Downed && source.Any<BodyPartRecord>())
			{
				num++;
				BodyPartRecord bodyPartRecord = source.RandomElementByWeight((BodyPartRecord x) => x.coverageAbs);
				int num2 = Mathf.RoundToInt(hediffSet.GetPartHealth(bodyPartRecord));
				float statValue = p.GetStatValue(StatDefOf.IncomingDamageFactor, true, -1);
				if (statValue > 0f)
				{
					num2 = (int)((float)num2 / statValue);
				}
				num2 -= 3;
				if (num2 >= 8)
				{
					DamageDef damageDef;
					if (bodyPartRecord.depth == BodyPartDepth.Outside)
					{
						if (!allowBleedingWounds && bodyPartRecord.def.bleedRate > 0f)
						{
							damageDef = DamageDefOf.Blunt;
						}
						else
						{
							damageDef = HealthUtility.RandomViolenceDamageType();
						}
					}
					else
					{
						damageDef = DamageDefOf.Blunt;
					}
					int num3 = Rand.RangeInclusive(Mathf.RoundToInt((float)num2 * 0.65f), num2);
					HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(damageDef, p, bodyPartRecord);
					if (!p.health.WouldDieAfterAddingHediff(hediffDefFromDamage, bodyPartRecord, (float)num3 * p.GetStatValue(StatDefOf.IncomingDamageFactor, true, -1)))
					{
						DamageInfo dinfo = new DamageInfo(damageDef, (float)num3, 999f, -1f, null, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true);
						dinfo.SetAllowDamagePropagation(false);
						p.TakeDamage(dinfo);
					}
				}
			}
			if (p.Dead)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(p + " died during GiveInjuriesToForceDowned");
				for (int i = 0; i < p.health.hediffSet.hediffs.Count; i++)
				{
					stringBuilder.AppendLine("   -" + p.health.hediffSet.hediffs[i].ToString());
				}
				Log.Error(stringBuilder.ToString());
			}
			p.health.forceDowned = false;
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x0007ABD4 File Offset: 0x00078DD4
		public static void DamageUntilDead(Pawn p)
		{
			HediffSet hediffSet = p.health.hediffSet;
			int num = 0;
			while (!p.Dead && num < 200 && HealthUtility.HittablePartsViolence(hediffSet).Any<BodyPartRecord>())
			{
				num++;
				BodyPartRecord bodyPartRecord = HealthUtility.HittablePartsViolence(hediffSet).RandomElementByWeight((BodyPartRecord x) => x.coverageAbs);
				int num2 = Rand.RangeInclusive(8, 25);
				DamageDef def;
				if (bodyPartRecord.depth == BodyPartDepth.Outside)
				{
					def = HealthUtility.RandomViolenceDamageType();
				}
				else
				{
					def = DamageDefOf.Blunt;
				}
				DamageInfo dinfo = new DamageInfo(def, (float)num2, 999f, -1f, null, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true);
				dinfo.SetIgnoreInstantKillProtection(true);
				p.TakeDamage(dinfo);
			}
			if (!p.Dead)
			{
				Log.Error(p + " not killed during GiveInjuriesToKill");
			}
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x0007ACA8 File Offset: 0x00078EA8
		public static void DamageLegsUntilIncapableOfMoving(Pawn p, bool allowBleedingWounds = true)
		{
			int num = 0;
			p.health.forceDowned = true;
			Func<BodyPartRecord, bool> <>9__0;
			while (p.health.capacities.CapableOf(PawnCapacityDefOf.Moving) && num < 300)
			{
				num++;
				IEnumerable<BodyPartRecord> notMissingParts = p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null);
				Func<BodyPartRecord, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((BodyPartRecord x) => x.def.tags.Contains(BodyPartTagDefOf.MovingLimbCore) && p.health.hediffSet.GetPartHealth(x) >= 2f));
				}
				IEnumerable<BodyPartRecord> source = notMissingParts.Where(predicate);
				if (!source.Any<BodyPartRecord>())
				{
					break;
				}
				BodyPartRecord bodyPartRecord = source.RandomElement<BodyPartRecord>();
				float maxHealth = bodyPartRecord.def.GetMaxHealth(p);
				float partHealth = p.health.hediffSet.GetPartHealth(bodyPartRecord);
				int min = Mathf.Clamp(Mathf.RoundToInt(maxHealth * 0.12f), 1, (int)partHealth - 1);
				int max = Mathf.Clamp(Mathf.RoundToInt(maxHealth * 0.27f), 1, (int)partHealth - 1);
				int num2 = Rand.RangeInclusive(min, max);
				DamageDef damageDef;
				if (!allowBleedingWounds && bodyPartRecord.def.bleedRate > 0f)
				{
					damageDef = DamageDefOf.Blunt;
				}
				else
				{
					damageDef = HealthUtility.RandomViolenceDamageType();
				}
				HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(damageDef, p, bodyPartRecord);
				if (p.health.WouldDieAfterAddingHediff(hediffDefFromDamage, bodyPartRecord, (float)num2))
				{
					break;
				}
				DamageInfo dinfo = new DamageInfo(damageDef, (float)num2, 999f, -1f, null, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true);
				dinfo.SetAllowDamagePropagation(false);
				p.TakeDamage(dinfo);
			}
			p.health.forceDowned = false;
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x0007AE50 File Offset: 0x00079050
		public static void DamageLimbsUntilIncapableOfManipulation(Pawn p, bool allowBleedingWounds = true)
		{
			int num = 0;
			p.health.forceDowned = true;
			Func<BodyPartRecord, bool> <>9__0;
			while (p.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) && num < 300)
			{
				num++;
				IEnumerable<BodyPartRecord> notMissingParts = p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null);
				Func<BodyPartRecord, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((BodyPartRecord x) => x.def.tags.Contains(BodyPartTagDefOf.ManipulationLimbCore) && p.health.hediffSet.GetPartHealth(x) >= 2f));
				}
				IEnumerable<BodyPartRecord> source = notMissingParts.Where(predicate);
				if (!source.Any<BodyPartRecord>())
				{
					break;
				}
				BodyPartRecord bodyPartRecord = source.RandomElement<BodyPartRecord>();
				float maxHealth = bodyPartRecord.def.GetMaxHealth(p);
				float partHealth = p.health.hediffSet.GetPartHealth(bodyPartRecord);
				int min = Mathf.Clamp(Mathf.RoundToInt(maxHealth * 0.12f), 1, (int)partHealth - 1);
				int max = Mathf.Clamp(Mathf.RoundToInt(maxHealth * 0.27f), 1, (int)partHealth - 1);
				int num2 = Rand.RangeInclusive(min, max);
				DamageDef damageDef = HealthUtility.RandomViolenceDamageType();
				HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(damageDef, p, bodyPartRecord);
				if (p.health.WouldDieAfterAddingHediff(hediffDefFromDamage, bodyPartRecord, (float)num2))
				{
					break;
				}
				DamageInfo dinfo = new DamageInfo(damageDef, (float)num2, 999f, -1f, null, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true);
				dinfo.SetAllowDamagePropagation(false);
				p.TakeDamage(dinfo);
			}
			p.health.forceDowned = false;
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x0007AFD8 File Offset: 0x000791D8
		public static DamageDef RandomViolenceDamageType()
		{
			switch (Rand.RangeInclusive(0, 4))
			{
			case 0:
				return DamageDefOf.Bullet;
			case 1:
				return DamageDefOf.Blunt;
			case 2:
				return DamageDefOf.Stab;
			case 3:
				return DamageDefOf.Scratch;
			case 4:
				return DamageDefOf.Cut;
			default:
				return null;
			}
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x0007B028 File Offset: 0x00079228
		public static DamageDef RandomPermanentInjuryDamageType(bool allowFrostbite)
		{
			switch (Rand.RangeInclusive(0, 3 + (allowFrostbite ? 1 : 0)))
			{
			case 0:
				return DamageDefOf.Bullet;
			case 1:
				return DamageDefOf.Scratch;
			case 2:
				return DamageDefOf.Bite;
			case 3:
				return DamageDefOf.Stab;
			case 4:
				return DamageDefOf.Frostbite;
			default:
				throw new Exception();
			}
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x0007B084 File Offset: 0x00079284
		public static HediffDef GetHediffDefFromDamage(DamageDef dam, Pawn pawn, BodyPartRecord part)
		{
			HediffDef result = dam.hediff;
			if (part.def.IsSkinCovered(part, pawn.health.hediffSet) && dam.hediffSkin != null)
			{
				result = dam.hediffSkin;
			}
			if (part.def.IsSolid(part, pawn.health.hediffSet.hediffs) && dam.hediffSolid != null)
			{
				result = dam.hediffSolid;
			}
			return result;
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x0007B0F0 File Offset: 0x000792F0
		public static bool TryAnesthetize(Pawn pawn)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return false;
			}
			pawn.health.forceDowned = true;
			pawn.health.AddHediff(HediffDefOf.Anesthetic, null, null, null);
			pawn.health.forceDowned = false;
			return true;
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x0007B144 File Offset: 0x00079344
		public static void AdjustSeverity(Pawn pawn, HediffDef hdDef, float sevOffset)
		{
			if (sevOffset == 0f)
			{
				return;
			}
			Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(hdDef, false);
			if (hediff != null)
			{
				hediff.Severity += sevOffset;
				return;
			}
			if (sevOffset > 0f)
			{
				hediff = HediffMaker.MakeHediff(hdDef, pawn, null);
				hediff.Severity = sevOffset;
				pawn.health.AddHediff(hediff, null, null, null);
			}
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x0007B1B0 File Offset: 0x000793B0
		public static BodyPartRemovalIntent PartRemovalIntent(Pawn pawn, BodyPartRecord part)
		{
			if (pawn.health.hediffSet.hediffs.Any((Hediff d) => d.Visible && d.Part == part && d.def.isBad))
			{
				return BodyPartRemovalIntent.Amputate;
			}
			return BodyPartRemovalIntent.Harvest;
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x0007B1F0 File Offset: 0x000793F0
		public static int TicksUntilDeathDueToBloodLoss(Pawn pawn)
		{
			float bleedRateTotal = pawn.health.hediffSet.BleedRateTotal;
			if (bleedRateTotal < 0.0001f)
			{
				return int.MaxValue;
			}
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss, false);
			float num = (firstHediffOfDef != null) ? firstHediffOfDef.Severity : 0f;
			return (int)((1f - num) / bleedRateTotal * 60000f);
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x0007B254 File Offset: 0x00079454
		public static TaggedString FixWorstHealthCondition(Pawn pawn)
		{
			Hediff hediff = HealthUtility.FindLifeThreateningHediff(pawn);
			if (hediff != null)
			{
				return HealthUtility.Cure(hediff);
			}
			if (HealthUtility.TicksUntilDeathDueToBloodLoss(pawn) < 2500)
			{
				Hediff hediff2 = HealthUtility.FindMostBleedingHediff(pawn);
				if (hediff2 != null)
				{
					return HealthUtility.Cure(hediff2);
				}
			}
			if (pawn.health.hediffSet.GetBrain() != null)
			{
				Hediff_Injury hediff_Injury = HealthUtility.FindPermanentInjury(pawn, Gen.YieldSingle<BodyPartRecord>(pawn.health.hediffSet.GetBrain()));
				if (hediff_Injury != null)
				{
					return HealthUtility.Cure(hediff_Injury);
				}
			}
			float coverageAbsWithChildren = ThingDefOf.Human.race.body.GetPartsWithDef(BodyPartDefOf.Hand).First<BodyPartRecord>().coverageAbsWithChildren;
			BodyPartRecord bodyPartRecord = HealthUtility.FindBiggestMissingBodyPart(pawn, coverageAbsWithChildren);
			if (bodyPartRecord != null)
			{
				return HealthUtility.Cure(bodyPartRecord, pawn);
			}
			Hediff_Injury hediff_Injury2 = HealthUtility.FindPermanentInjury(pawn, from x in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where x.def == BodyPartDefOf.Eye
			select x);
			if (hediff_Injury2 != null)
			{
				return HealthUtility.Cure(hediff_Injury2);
			}
			Hediff hediff3 = HealthUtility.FindImmunizableHediffWhichCanKill(pawn);
			if (hediff3 != null)
			{
				return HealthUtility.Cure(hediff3);
			}
			Hediff hediff4 = HealthUtility.FindNonInjuryMiscBadHediff(pawn, true);
			if (hediff4 != null)
			{
				return HealthUtility.Cure(hediff4);
			}
			Hediff hediff5 = HealthUtility.FindNonInjuryMiscBadHediff(pawn, false);
			if (hediff5 != null)
			{
				return HealthUtility.Cure(hediff5);
			}
			if (pawn.health.hediffSet.GetBrain() != null)
			{
				Hediff_Injury hediff_Injury3 = HealthUtility.FindInjury(pawn, Gen.YieldSingle<BodyPartRecord>(pawn.health.hediffSet.GetBrain()));
				if (hediff_Injury3 != null)
				{
					return HealthUtility.Cure(hediff_Injury3);
				}
			}
			BodyPartRecord bodyPartRecord2 = HealthUtility.FindBiggestMissingBodyPart(pawn, 0f);
			if (bodyPartRecord2 != null)
			{
				return HealthUtility.Cure(bodyPartRecord2, pawn);
			}
			Hediff_Addiction hediff_Addiction = HealthUtility.FindAddiction(pawn);
			if (hediff_Addiction != null)
			{
				return HealthUtility.Cure(hediff_Addiction);
			}
			Hediff_Injury hediff_Injury4 = HealthUtility.FindPermanentInjury(pawn, null);
			if (hediff_Injury4 != null)
			{
				return HealthUtility.Cure(hediff_Injury4);
			}
			Hediff_Injury hediff_Injury5 = HealthUtility.FindInjury(pawn, null);
			if (hediff_Injury5 != null)
			{
				return HealthUtility.Cure(hediff_Injury5);
			}
			return null;
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x0007B428 File Offset: 0x00079628
		private static Hediff FindLifeThreateningHediff(Pawn pawn)
		{
			Hediff hediff = null;
			float num = -1f;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].Visible && hediffs[i].def.everCurableByItem && !hediffs[i].FullyImmune())
				{
					HediffStage curStage = hediffs[i].CurStage;
					bool flag = curStage != null && curStage.lifeThreatening;
					bool flag2 = hediffs[i].def.lethalSeverity >= 0f && hediffs[i].Severity / hediffs[i].def.lethalSeverity >= 0.8f;
					if (flag || flag2)
					{
						float num2 = (hediffs[i].Part != null) ? hediffs[i].Part.coverageAbsWithChildren : 999f;
						if (hediff == null || num2 > num)
						{
							hediff = hediffs[i];
							num = num2;
						}
					}
				}
			}
			return hediff;
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x0007B540 File Offset: 0x00079740
		private static Hediff FindMostBleedingHediff(Pawn pawn)
		{
			float num = 0f;
			Hediff hediff = null;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].Visible && hediffs[i].def.everCurableByItem)
				{
					float bleedRate = hediffs[i].BleedRate;
					if (bleedRate > 0f && (bleedRate > num || hediff == null))
					{
						num = bleedRate;
						hediff = hediffs[i];
					}
				}
			}
			return hediff;
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x0007B5C4 File Offset: 0x000797C4
		private static Hediff FindImmunizableHediffWhichCanKill(Pawn pawn)
		{
			Hediff hediff = null;
			float num = -1f;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].Visible && hediffs[i].def.everCurableByItem && hediffs[i].TryGetComp<HediffComp_Immunizable>() != null && !hediffs[i].FullyImmune() && HealthUtility.CanEverKill(hediffs[i]))
				{
					float severity = hediffs[i].Severity;
					if (hediff == null || severity > num)
					{
						hediff = hediffs[i];
						num = severity;
					}
				}
			}
			return hediff;
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x0007B668 File Offset: 0x00079868
		private static Hediff FindNonInjuryMiscBadHediff(Pawn pawn, bool onlyIfCanKill)
		{
			Hediff hediff = null;
			float num = -1f;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].Visible && hediffs[i].def.isBad && hediffs[i].def.everCurableByItem && !(hediffs[i] is Hediff_Injury) && !(hediffs[i] is Hediff_MissingPart) && !(hediffs[i] is Hediff_Addiction) && !(hediffs[i] is Hediff_AddedPart) && (!onlyIfCanKill || HealthUtility.CanEverKill(hediffs[i])))
				{
					float num2 = (hediffs[i].Part != null) ? hediffs[i].Part.coverageAbsWithChildren : 999f;
					if (hediff == null || num2 > num)
					{
						hediff = hediffs[i];
						num = num2;
					}
				}
			}
			return hediff;
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x0007B768 File Offset: 0x00079968
		private static BodyPartRecord FindBiggestMissingBodyPart(Pawn pawn, float minCoverage = 0f)
		{
			BodyPartRecord bodyPartRecord = null;
			foreach (Hediff_MissingPart hediff_MissingPart in pawn.health.hediffSet.GetMissingPartsCommonAncestors())
			{
				if (hediff_MissingPart.Part.coverageAbsWithChildren >= minCoverage && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(hediff_MissingPart.Part) && (bodyPartRecord == null || hediff_MissingPart.Part.coverageAbsWithChildren > bodyPartRecord.coverageAbsWithChildren))
				{
					bodyPartRecord = hediff_MissingPart.Part;
				}
			}
			return bodyPartRecord;
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x0007B804 File Offset: 0x00079A04
		private static Hediff_Addiction FindAddiction(Pawn pawn)
		{
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_Addiction hediff_Addiction = hediffs[i] as Hediff_Addiction;
				if (hediff_Addiction != null && hediff_Addiction.Visible && hediff_Addiction.def.everCurableByItem)
				{
					return hediff_Addiction;
				}
			}
			return null;
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x0007B85C File Offset: 0x00079A5C
		private static Hediff_Injury FindPermanentInjury(Pawn pawn, IEnumerable<BodyPartRecord> allowedBodyParts = null)
		{
			Hediff_Injury hediff_Injury = null;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury2 = hediffs[i] as Hediff_Injury;
				if (hediff_Injury2 != null && hediff_Injury2.Visible && hediff_Injury2.IsPermanent() && hediff_Injury2.def.everCurableByItem && (allowedBodyParts == null || allowedBodyParts.Contains(hediff_Injury2.Part)) && (hediff_Injury == null || hediff_Injury2.Severity > hediff_Injury.Severity))
				{
					hediff_Injury = hediff_Injury2;
				}
			}
			return hediff_Injury;
		}

		// Token: 0x06001414 RID: 5140 RVA: 0x0007B8E0 File Offset: 0x00079AE0
		private static Hediff_Injury FindInjury(Pawn pawn, IEnumerable<BodyPartRecord> allowedBodyParts = null)
		{
			Hediff_Injury hediff_Injury = null;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury2 = hediffs[i] as Hediff_Injury;
				if (hediff_Injury2 != null && hediff_Injury2.Visible && hediff_Injury2.def.everCurableByItem && (allowedBodyParts == null || allowedBodyParts.Contains(hediff_Injury2.Part)) && (hediff_Injury == null || hediff_Injury2.Severity > hediff_Injury.Severity))
				{
					hediff_Injury = hediff_Injury2;
				}
			}
			return hediff_Injury;
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x0007B95C File Offset: 0x00079B5C
		public static TaggedString Cure(Hediff hediff)
		{
			Pawn pawn = hediff.pawn;
			pawn.health.RemoveHediff(hediff);
			if (hediff.def.cureAllAtOnceIfCuredByItem)
			{
				int num = 0;
				for (;;)
				{
					num++;
					if (num > 10000)
					{
						break;
					}
					Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(hediff.def, false);
					if (firstHediffOfDef == null)
					{
						goto IL_63;
					}
					pawn.health.RemoveHediff(firstHediffOfDef);
				}
				Log.Error("Too many iterations.");
			}
			IL_63:
			return "HealingCureHediff".Translate(pawn, hediff.def.label);
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x0007B9EC File Offset: 0x00079BEC
		private static TaggedString Cure(BodyPartRecord part, Pawn pawn)
		{
			pawn.health.RestorePart(part, null, true);
			return "HealingRestoreBodyPart".Translate(pawn, part.Label);
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x0007BA18 File Offset: 0x00079C18
		private static bool CanEverKill(Hediff hediff)
		{
			if (hediff.def.stages != null)
			{
				for (int i = 0; i < hediff.def.stages.Count; i++)
				{
					if (hediff.def.stages[i].lifeThreatening)
					{
						return true;
					}
				}
			}
			return hediff.def.lethalSeverity >= 0f;
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x0007BA7C File Offset: 0x00079C7C
		public static bool IsMissingSightBodyPart(Pawn p)
		{
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_MissingPart hediff_MissingPart;
				if ((hediff_MissingPart = (hediffs[i] as Hediff_MissingPart)) != null && hediff_MissingPart.Part.def.tags.Contains(BodyPartTagDefOf.SightSource))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04001090 RID: 4240
		public static readonly Color GoodConditionColor = new Color(0.6f, 0.8f, 0.65f);

		// Token: 0x04001091 RID: 4241
		public static readonly Color RedColor = ColorLibrary.RedReadable;

		// Token: 0x04001092 RID: 4242
		public static readonly Color ImpairedColor = new Color(0.9f, 0.35f, 0f);

		// Token: 0x04001093 RID: 4243
		public static readonly Color SlightlyImpairedColor = new Color(0.9f, 0.7f, 0f);

		// Token: 0x04001094 RID: 4244
		private static List<Hediff> tmpHediffs = new List<Hediff>();
	}
}
