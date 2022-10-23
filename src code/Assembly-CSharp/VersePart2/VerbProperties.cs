using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000EB RID: 235
	public class VerbProperties
	{
		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000696 RID: 1686 RVA: 0x0002398B File Offset: 0x00021B8B
		public bool CausesTimeSlowdown
		{
			get
			{
				return this.ai_IsWeapon && this.forceNormalTimeSpeed;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000697 RID: 1687 RVA: 0x0002399D File Offset: 0x00021B9D
		public bool LaunchesProjectile
		{
			get
			{
				return typeof(Verb_LaunchProjectile).IsAssignableFrom(this.verbClass);
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000698 RID: 1688 RVA: 0x000239B4 File Offset: 0x00021BB4
		public bool Ranged
		{
			get
			{
				return this.LaunchesProjectile || typeof(Verb_ShootBeam).IsAssignableFrom(this.verbClass) || typeof(Verb_SpewFire).IsAssignableFrom(this.verbClass) || typeof(Verb_Spray).IsAssignableFrom(this.verbClass);
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x00023A10 File Offset: 0x00021C10
		public string AccuracySummaryString
		{
			get
			{
				return string.Concat(new string[]
				{
					this.accuracyTouch.ToStringPercent(),
					" - ",
					this.accuracyShort.ToStringPercent(),
					" - ",
					this.accuracyMedium.ToStringPercent(),
					" - ",
					this.accuracyLong.ToStringPercent()
				});
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x0600069A RID: 1690 RVA: 0x00023A78 File Offset: 0x00021C78
		public bool IsMeleeAttack
		{
			get
			{
				return typeof(Verb_MeleeAttack).IsAssignableFrom(this.verbClass);
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x0600069B RID: 1691 RVA: 0x00023A90 File Offset: 0x00021C90
		public bool CausesExplosion
		{
			get
			{
				return this.defaultProjectile != null && (typeof(Projectile_Explosive).IsAssignableFrom(this.defaultProjectile.thingClass) || typeof(Projectile_DoomsdayRocket).IsAssignableFrom(this.defaultProjectile.thingClass) || this.defaultProjectile.GetCompProperties<CompProperties_Explosive>() != null);
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600069C RID: 1692 RVA: 0x00023AF0 File Offset: 0x00021CF0
		public float ForcedMissRadius
		{
			get
			{
				if (this.isMortar && this.forcedMissRadiusClassicMortars >= 0f)
				{
					Storyteller storyteller = Find.Storyteller;
					if (((storyteller != null) ? storyteller.difficulty : null) != null && Find.Storyteller.difficulty.classicMortars)
					{
						return this.forcedMissRadiusClassicMortars;
					}
				}
				return this.forcedMissRadius;
			}
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00023B43 File Offset: 0x00021D43
		public float AdjustedRange(Verb ownerVerb, Pawn attacker)
		{
			if (this.rangeStat == null)
			{
				return this.range;
			}
			return attacker.GetStatValue(this.rangeStat, true, -1);
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x00023B62 File Offset: 0x00021D62
		public float AdjustedMeleeDamageAmount(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate melee damage amount for a verb with different verb props. verb=" + ownerVerb, 5469809);
				return 0f;
			}
			return this.AdjustedMeleeDamageAmount(ownerVerb.tool, attacker, ownerVerb.EquipmentSource, ownerVerb.HediffCompSource);
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00023BA4 File Offset: 0x00021DA4
		public float AdjustedMeleeDamageAmount(Tool tool, Pawn attacker, Thing equipment, HediffComp_VerbGiver hediffCompSource)
		{
			if (!this.IsMeleeAttack)
			{
				Log.ErrorOnce(string.Format("Attempting to get melee damage for a non-melee verb {0}", this), 26181238);
			}
			float num;
			if (tool != null)
			{
				num = tool.AdjustedBaseMeleeDamageAmount(equipment, this.meleeDamageDef);
			}
			else
			{
				num = (float)this.meleeDamageBaseAmount;
			}
			if (attacker != null)
			{
				num *= this.GetDamageFactorFor(tool, attacker, hediffCompSource);
			}
			return num;
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x00023BFC File Offset: 0x00021DFC
		public float AdjustedMeleeDamageAmount(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff, HediffComp_VerbGiver hediffCompSource)
		{
			if (!this.IsMeleeAttack)
			{
				Log.ErrorOnce(string.Format("Attempting to get melee damage for a non-melee verb {0}", this), 26181238);
			}
			float num;
			if (tool != null)
			{
				num = tool.AdjustedBaseMeleeDamageAmount(equipment, equipmentStuff, this.meleeDamageDef);
			}
			else
			{
				num = (float)this.meleeDamageBaseAmount;
			}
			if (attacker != null)
			{
				num *= this.GetDamageFactorFor(tool, attacker, hediffCompSource);
			}
			return num;
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x00023C54 File Offset: 0x00021E54
		public float AdjustedArmorPenetration(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate armor penetration for a verb with different verb props. verb=" + ownerVerb, 9865767);
				return 0f;
			}
			return this.AdjustedArmorPenetration(ownerVerb.tool, attacker, ownerVerb.EquipmentSource, ownerVerb.HediffCompSource);
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00023C94 File Offset: 0x00021E94
		public float AdjustedArmorPenetration(Tool tool, Pawn attacker, Thing equipment, HediffComp_VerbGiver hediffCompSource)
		{
			float num;
			if (tool != null)
			{
				num = tool.armorPenetration;
			}
			else
			{
				num = this.meleeArmorPenetrationBase;
			}
			if (num < 0f)
			{
				num = this.AdjustedMeleeDamageAmount(tool, attacker, equipment, hediffCompSource) * 0.015f;
			}
			else if (equipment != null)
			{
				float statValue = equipment.GetStatValue(StatDefOf.MeleeWeapon_DamageMultiplier, true, -1);
				num *= statValue;
			}
			return num;
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x00023CE8 File Offset: 0x00021EE8
		public float AdjustedArmorPenetration(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff, HediffComp_VerbGiver hediffCompSource)
		{
			float num;
			if (tool != null)
			{
				num = tool.armorPenetration;
			}
			else
			{
				num = this.meleeArmorPenetrationBase;
			}
			if (num < 0f)
			{
				num = this.AdjustedMeleeDamageAmount(tool, attacker, equipment, equipmentStuff, hediffCompSource) * 0.015f;
			}
			else if (equipment != null)
			{
				float statValueAbstract = equipment.GetStatValueAbstract(StatDefOf.MeleeWeapon_DamageMultiplier, equipmentStuff);
				num *= statValueAbstract;
			}
			return num;
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x00023D3C File Offset: 0x00021F3C
		private float AdjustedExpectedDamageForVerbUsableInMelee(Tool tool, Pawn attacker, Thing equipment, HediffComp_VerbGiver hediffCompSource)
		{
			if (this.IsMeleeAttack)
			{
				return this.AdjustedMeleeDamageAmount(tool, attacker, equipment, hediffCompSource);
			}
			if (this.LaunchesProjectile && this.defaultProjectile != null)
			{
				return (float)this.defaultProjectile.projectile.GetDamageAmount(equipment, null);
			}
			return 0f;
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00023D7C File Offset: 0x00021F7C
		private float AdjustedExpectedDamageForVerbUsableInMelee(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff, HediffComp_VerbGiver hediffCompSource)
		{
			if (this.IsMeleeAttack)
			{
				return this.AdjustedMeleeDamageAmount(tool, attacker, equipment, equipmentStuff, hediffCompSource);
			}
			if (this.LaunchesProjectile && this.defaultProjectile != null)
			{
				return (float)this.defaultProjectile.projectile.GetDamageAmount(equipment, equipmentStuff, null);
			}
			return 0f;
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00023DCC File Offset: 0x00021FCC
		public float AdjustedMeleeSelectionWeight(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate melee selection weight for a verb with different verb props. verb=" + ownerVerb, 385716351);
				return 0f;
			}
			return this.AdjustedMeleeSelectionWeight(ownerVerb.tool, attacker, ownerVerb.EquipmentSource, ownerVerb.HediffCompSource, ownerVerb.DirectOwner is Pawn);
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x00023E24 File Offset: 0x00022024
		public float AdjustedMeleeSelectionWeight(Tool tool, Pawn attacker, Thing equipment, HediffComp_VerbGiver hediffCompSource, bool comesFromPawnNativeVerbs)
		{
			if (!this.IsMeleeAttack)
			{
				return 0f;
			}
			if (attacker != null && attacker.RaceProps.intelligence < this.minIntelligence)
			{
				return 0f;
			}
			float num = 1f;
			float num2 = this.AdjustedExpectedDamageForVerbUsableInMelee(tool, attacker, equipment, hediffCompSource);
			if (num2 >= 0.001f || !typeof(Verb_MeleeApplyHediff).IsAssignableFrom(this.verbClass))
			{
				num *= num2 * num2;
			}
			num *= this.commonality;
			if (tool != null)
			{
				num *= tool.chanceFactor;
			}
			if (comesFromPawnNativeVerbs && (tool == null || !tool.alwaysTreatAsWeapon))
			{
				num *= 0.3f;
			}
			return num;
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x00023EC0 File Offset: 0x000220C0
		public float AdjustedMeleeSelectionWeight(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff, HediffComp_VerbGiver hediffCompSource, bool comesFromPawnNativeVerbs)
		{
			if (!this.IsMeleeAttack)
			{
				return 0f;
			}
			if (attacker != null && attacker.RaceProps.intelligence < this.minIntelligence)
			{
				return 0f;
			}
			float num = 1f;
			float num2 = this.AdjustedExpectedDamageForVerbUsableInMelee(tool, attacker, equipment, equipmentStuff, hediffCompSource);
			if (num2 >= 0.001f || !typeof(Verb_MeleeApplyHediff).IsAssignableFrom(this.verbClass))
			{
				num *= num2 * num2;
			}
			num *= this.commonality;
			if (tool != null)
			{
				num *= tool.chanceFactor;
			}
			if (comesFromPawnNativeVerbs && (tool == null || !tool.alwaysTreatAsWeapon))
			{
				num *= 0.3f;
			}
			return num;
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x00023F5D File Offset: 0x0002215D
		public float AdjustedCooldown(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate cooldown for a verb with different verb props. verb=" + ownerVerb, 19485711);
				return 0f;
			}
			return this.AdjustedCooldown(ownerVerb.tool, attacker, ownerVerb.EquipmentSource);
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x00023F98 File Offset: 0x00022198
		public float AdjustedCooldown(Tool tool, Pawn attacker, Thing equipment)
		{
			if (tool != null)
			{
				return tool.AdjustedCooldown(equipment);
			}
			if (equipment != null && !this.IsMeleeAttack)
			{
				float num = equipment.GetStatValue(StatDefOf.RangedWeapon_Cooldown, true, -1);
				if (ModsConfig.BiotechActive && attacker != null)
				{
					num *= attacker.GetStatValue(StatDefOf.RangedCooldownFactor, true, -1);
				}
				return num;
			}
			return this.defaultCooldownTime;
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x00023FEC File Offset: 0x000221EC
		public float AdjustedCooldown(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff)
		{
			if (tool != null)
			{
				return tool.AdjustedCooldown(equipment, equipmentStuff);
			}
			if (equipment != null && !this.IsMeleeAttack)
			{
				float num = equipment.GetStatValueAbstract(StatDefOf.RangedWeapon_Cooldown, equipmentStuff);
				if (ModsConfig.BiotechActive && attacker != null)
				{
					num *= attacker.GetStatValue(StatDefOf.RangedCooldownFactor, true, -1);
				}
				return num;
			}
			return this.defaultCooldownTime;
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x00024041 File Offset: 0x00022241
		public int AdjustedCooldownTicks(Verb ownerVerb, Pawn attacker)
		{
			return this.AdjustedCooldown(ownerVerb, attacker).SecondsToTicks();
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x00024050 File Offset: 0x00022250
		private float AdjustedAccuracy(RangeCategory cat, Thing equipment)
		{
			if (equipment != null)
			{
				StatDef stat = null;
				switch (cat)
				{
				case RangeCategory.Touch:
					stat = StatDefOf.AccuracyTouch;
					break;
				case RangeCategory.Short:
					stat = StatDefOf.AccuracyShort;
					break;
				case RangeCategory.Medium:
					stat = StatDefOf.AccuracyMedium;
					break;
				case RangeCategory.Long:
					stat = StatDefOf.AccuracyLong;
					break;
				}
				return equipment.GetStatValue(stat, true, -1);
			}
			switch (cat)
			{
			case RangeCategory.Touch:
				return this.accuracyTouch;
			case RangeCategory.Short:
				return this.accuracyShort;
			case RangeCategory.Medium:
				return this.accuracyMedium;
			case RangeCategory.Long:
				return this.accuracyLong;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x000240DB File Offset: 0x000222DB
		public float AdjustedFullCycleTime(Verb ownerVerb, Pawn attacker)
		{
			return this.warmupTime + this.AdjustedCooldown(ownerVerb, attacker) + ((this.burstShotCount - 1) * this.ticksBetweenBurstShots).TicksToSeconds();
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00024101 File Offset: 0x00022301
		public float GetDamageFactorFor(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate damage factor for a verb with different verb props. verb=" + ownerVerb, 94324562);
				return 1f;
			}
			return this.GetDamageFactorFor(ownerVerb.tool, attacker, ownerVerb.HediffCompSource);
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x0002413C File Offset: 0x0002233C
		public float GetDamageFactorFor(Tool tool, Pawn attacker, HediffComp_VerbGiver hediffCompSource)
		{
			float num = 1f;
			if (attacker != null)
			{
				if (hediffCompSource != null)
				{
					num *= PawnCapacityUtility.CalculatePartEfficiency(hediffCompSource.Pawn.health.hediffSet, hediffCompSource.parent.Part, true, null);
				}
				else if (attacker != null && this.AdjustedLinkedBodyPartsGroup(tool) != null)
				{
					float num2 = PawnCapacityUtility.CalculateNaturalPartsAverageEfficiency(attacker.health.hediffSet, this.AdjustedLinkedBodyPartsGroup(tool));
					if (this.AdjustedEnsureLinkedBodyPartsGroupAlwaysUsable(tool))
					{
						num2 = Mathf.Max(num2, 0.4f);
					}
					num *= num2;
				}
				if (attacker != null && this.IsMeleeAttack)
				{
					num *= attacker.ageTracker.CurLifeStage.meleeDamageFactor;
					if (ModsConfig.BiotechActive)
					{
						num *= attacker.GetStatValue(StatDefOf.MeleeDamageFactor, true, -1);
					}
				}
			}
			return num;
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x000241F2 File Offset: 0x000223F2
		public BodyPartGroupDef AdjustedLinkedBodyPartsGroup(Tool tool)
		{
			if (tool != null)
			{
				return tool.linkedBodyPartsGroup;
			}
			return this.linkedBodyPartsGroup;
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x00024204 File Offset: 0x00022404
		public bool AdjustedEnsureLinkedBodyPartsGroupAlwaysUsable(Tool tool)
		{
			if (tool != null)
			{
				return tool.ensureLinkedBodyPartsGroupAlwaysUsable;
			}
			return this.ensureLinkedBodyPartsGroupAlwaysUsable;
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00024216 File Offset: 0x00022416
		public float EffectiveMinRange(LocalTargetInfo target, Thing caster)
		{
			return this.EffectiveMinRange(VerbUtility.AllowAdjacentShot(target, caster));
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x00024228 File Offset: 0x00022428
		public float EffectiveMinRange(bool allowAdjacentShot)
		{
			float num = this.minRange;
			if (!allowAdjacentShot && !this.IsMeleeAttack && this.LaunchesProjectile)
			{
				num = Mathf.Max(num, 1.421f);
			}
			return num;
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0002425C File Offset: 0x0002245C
		public float GetHitChanceFactor(Thing equipment, float dist)
		{
			float value;
			if (dist <= 3f)
			{
				value = this.AdjustedAccuracy(RangeCategory.Touch, equipment);
			}
			else if (dist <= 12f)
			{
				value = Mathf.Lerp(this.AdjustedAccuracy(RangeCategory.Touch, equipment), this.AdjustedAccuracy(RangeCategory.Short, equipment), (dist - 3f) / 9f);
			}
			else if (dist <= 25f)
			{
				value = Mathf.Lerp(this.AdjustedAccuracy(RangeCategory.Short, equipment), this.AdjustedAccuracy(RangeCategory.Medium, equipment), (dist - 12f) / 13f);
			}
			else if (dist <= 40f)
			{
				value = Mathf.Lerp(this.AdjustedAccuracy(RangeCategory.Medium, equipment), this.AdjustedAccuracy(RangeCategory.Long, equipment), (dist - 25f) / 15f);
			}
			else
			{
				value = this.AdjustedAccuracy(RangeCategory.Long, equipment);
			}
			return Mathf.Clamp(value, 0.01f, 1f);
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x00024321 File Offset: 0x00022521
		public float GetForceMissFactorFor(Thing equipment, Pawn caster)
		{
			if (equipment.def.building != null && equipment.def.building.IsMortar)
			{
				return caster.GetStatValueForPawn(StatDefOf.MortarMissRadiusFactor, caster, true);
			}
			return 1f;
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x00024358 File Offset: 0x00022558
		public void DrawRadiusRing(IntVec3 center)
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (!this.IsMeleeAttack && this.targetable)
			{
				float num = this.EffectiveMinRange(true);
				if (num > 0f && num < GenRadial.MaxRadialPatternRadius)
				{
					GenDraw.DrawRadiusRing(center, num);
				}
				if (this.range < (float)(Find.CurrentMap.Size.x + Find.CurrentMap.Size.z) && this.range < GenRadial.MaxRadialPatternRadius)
				{
					Func<IntVec3, bool> predicate = null;
					if (this.drawHighlightWithLineOfSight)
					{
						predicate = ((IntVec3 c) => GenSight.LineOfSight(center, c, Find.CurrentMap, false, null, 0, 0));
					}
					GenDraw.DrawRadiusRing(center, this.range, Color.white, predicate);
				}
			}
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x00024418 File Offset: 0x00022618
		public override string ToString()
		{
			string str;
			if (!this.label.NullOrEmpty())
			{
				str = this.label;
			}
			else
			{
				str = string.Concat(new object[]
				{
					"range=",
					this.range,
					", defaultProjectile=",
					this.defaultProjectile.ToStringSafe<ThingDef>()
				});
			}
			return "VerbProperties(" + str + ")";
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x00024483 File Offset: 0x00022683
		public new VerbProperties MemberwiseClone()
		{
			return (VerbProperties)base.MemberwiseClone();
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x00024490 File Offset: 0x00022690
		public IEnumerable<string> ConfigErrors(ThingDef parent)
		{
			if (parent.race != null && this.linkedBodyPartsGroup != null && !parent.race.body.AllParts.Any((BodyPartRecord part) => part.groups.Contains(this.linkedBodyPartsGroup)))
			{
				yield return string.Concat(new object[]
				{
					"has verb with linkedBodyPartsGroup ",
					this.linkedBodyPartsGroup,
					" but body ",
					parent.race.body,
					" has no parts with that group."
				});
			}
			if (this.LaunchesProjectile && this.defaultProjectile != null && this.forcedMissRadius > 0f != this.CausesExplosion)
			{
				yield return "has incorrect forcedMiss settings; explosive projectiles and only explosive projectiles should have forced miss enabled";
			}
			yield break;
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x000244A7 File Offset: 0x000226A7
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x040004F5 RID: 1269
		public VerbCategory category = VerbCategory.Misc;

		// Token: 0x040004F6 RID: 1270
		[TranslationHandle]
		public Type verbClass = typeof(Verb);

		// Token: 0x040004F7 RID: 1271
		[MustTranslate]
		public string label;

		// Token: 0x040004F8 RID: 1272
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedLabel;

		// Token: 0x040004F9 RID: 1273
		public bool isPrimary = true;

		// Token: 0x040004FA RID: 1274
		public bool violent = true;

		// Token: 0x040004FB RID: 1275
		public float minRange;

		// Token: 0x040004FC RID: 1276
		public float range = 1.42f;

		// Token: 0x040004FD RID: 1277
		public StatDef rangeStat;

		// Token: 0x040004FE RID: 1278
		public int burstShotCount = 1;

		// Token: 0x040004FF RID: 1279
		public int ticksBetweenBurstShots = 15;

		// Token: 0x04000500 RID: 1280
		public float noiseRadius = 3f;

		// Token: 0x04000501 RID: 1281
		public bool hasStandardCommand;

		// Token: 0x04000502 RID: 1282
		public bool targetable = true;

		// Token: 0x04000503 RID: 1283
		public bool nonInterruptingSelfCast;

		// Token: 0x04000504 RID: 1284
		public TargetingParameters targetParams = new TargetingParameters();

		// Token: 0x04000505 RID: 1285
		public bool requireLineOfSight = true;

		// Token: 0x04000506 RID: 1286
		public bool mustCastOnOpenGround;

		// Token: 0x04000507 RID: 1287
		public bool forceNormalTimeSpeed = true;

		// Token: 0x04000508 RID: 1288
		public bool onlyManualCast;

		// Token: 0x04000509 RID: 1289
		public bool stopBurstWithoutLos = true;

		// Token: 0x0400050A RID: 1290
		public SurpriseAttackProps surpriseAttack;

		// Token: 0x0400050B RID: 1291
		public float commonality = 1f;

		// Token: 0x0400050C RID: 1292
		public Intelligence minIntelligence;

		// Token: 0x0400050D RID: 1293
		public float consumeFuelPerShot;

		// Token: 0x0400050E RID: 1294
		public float consumeFuelPerBurst;

		// Token: 0x0400050F RID: 1295
		public bool stunTargetOnCastStart;

		// Token: 0x04000510 RID: 1296
		public float warmupTime;

		// Token: 0x04000511 RID: 1297
		public float defaultCooldownTime;

		// Token: 0x04000512 RID: 1298
		public string commandIcon;

		// Token: 0x04000513 RID: 1299
		public SoundDef soundCast;

		// Token: 0x04000514 RID: 1300
		public SoundDef soundCastTail;

		// Token: 0x04000515 RID: 1301
		public SoundDef soundAiming;

		// Token: 0x04000516 RID: 1302
		public float muzzleFlashScale;

		// Token: 0x04000517 RID: 1303
		public ThingDef impactMote;

		// Token: 0x04000518 RID: 1304
		public FleckDef impactFleck;

		// Token: 0x04000519 RID: 1305
		public bool drawAimPie = true;

		// Token: 0x0400051A RID: 1306
		public EffecterDef warmupEffecter;

		// Token: 0x0400051B RID: 1307
		public bool drawHighlightWithLineOfSight;

		// Token: 0x0400051C RID: 1308
		public ThingDef aimingLineMote;

		// Token: 0x0400051D RID: 1309
		public float? aimingLineMoteFixedLength;

		// Token: 0x0400051E RID: 1310
		public ThingDef aimingChargeMote;

		// Token: 0x0400051F RID: 1311
		public float aimingChargeMoteOffset;

		// Token: 0x04000520 RID: 1312
		public ThingDef aimingTargetMote;

		// Token: 0x04000521 RID: 1313
		public BodyPartGroupDef linkedBodyPartsGroup;

		// Token: 0x04000522 RID: 1314
		public bool ensureLinkedBodyPartsGroupAlwaysUsable;

		// Token: 0x04000523 RID: 1315
		public DamageDef meleeDamageDef;

		// Token: 0x04000524 RID: 1316
		public int meleeDamageBaseAmount = 1;

		// Token: 0x04000525 RID: 1317
		public float meleeArmorPenetrationBase = -1f;

		// Token: 0x04000526 RID: 1318
		public bool ai_IsWeapon = true;

		// Token: 0x04000527 RID: 1319
		public bool ai_IsBuildingDestroyer;

		// Token: 0x04000528 RID: 1320
		public float ai_AvoidFriendlyFireRadius;

		// Token: 0x04000529 RID: 1321
		public bool ai_RangedAlawaysShootGroundBelowTarget;

		// Token: 0x0400052A RID: 1322
		public bool ai_IsDoorDestroyer;

		// Token: 0x0400052B RID: 1323
		public bool ai_ProjectileLaunchingIgnoresMeleeThreats;

		// Token: 0x0400052C RID: 1324
		public float ai_TargetHasRangedAttackScoreOffset;

		// Token: 0x0400052D RID: 1325
		public ThingDef defaultProjectile;

		// Token: 0x0400052E RID: 1326
		private float forcedMissRadius;

		// Token: 0x0400052F RID: 1327
		private float forcedMissRadiusClassicMortars = -1f;

		// Token: 0x04000530 RID: 1328
		public bool forcedMissEvenDispersal;

		// Token: 0x04000531 RID: 1329
		private bool isMortar;

		// Token: 0x04000532 RID: 1330
		public float accuracyTouch = 1f;

		// Token: 0x04000533 RID: 1331
		public float accuracyShort = 1f;

		// Token: 0x04000534 RID: 1332
		public float accuracyMedium = 1f;

		// Token: 0x04000535 RID: 1333
		public float accuracyLong = 1f;

		// Token: 0x04000536 RID: 1334
		public bool canGoWild = true;

		// Token: 0x04000537 RID: 1335
		public DamageDef beamDamageDef;

		// Token: 0x04000538 RID: 1336
		public float beamWidth = 1f;

		// Token: 0x04000539 RID: 1337
		public float beamMaxDeviation;

		// Token: 0x0400053A RID: 1338
		public FleckDef beamGroundFleckDef;

		// Token: 0x0400053B RID: 1339
		public EffecterDef beamEndEffecterDef;

		// Token: 0x0400053C RID: 1340
		public ThingDef beamMoteDef;

		// Token: 0x0400053D RID: 1341
		public float beamFleckChancePerTick;

		// Token: 0x0400053E RID: 1342
		public float beamCurvature;

		// Token: 0x0400053F RID: 1343
		public float beamChanceToStartFire;

		// Token: 0x04000540 RID: 1344
		public float beamChanceToAttachFire;

		// Token: 0x04000541 RID: 1345
		public float beamStartOffset;

		// Token: 0x04000542 RID: 1346
		public float beamFullWidthRange;

		// Token: 0x04000543 RID: 1347
		public FleckDef beamLineFleckDef;

		// Token: 0x04000544 RID: 1348
		public SimpleCurve beamLineFleckChanceCurve;

		// Token: 0x04000545 RID: 1349
		public FloatRange beamFireSizeRange = FloatRange.ZeroToOne;

		// Token: 0x04000546 RID: 1350
		public SoundDef soundCastBeam;

		// Token: 0x04000547 RID: 1351
		public bool beamTargetsGround;

		// Token: 0x04000548 RID: 1352
		public float sprayWidth;

		// Token: 0x04000549 RID: 1353
		public float sprayArching;

		// Token: 0x0400054A RID: 1354
		public int sprayNumExtraCells;

		// Token: 0x0400054B RID: 1355
		public int sprayThicknessCells = 1;

		// Token: 0x0400054C RID: 1356
		public EffecterDef sprayEffecterDef;

		// Token: 0x0400054D RID: 1357
		public ThingDef spawnDef;

		// Token: 0x0400054E RID: 1358
		public TaleDef colonyWideTaleDef;

		// Token: 0x0400054F RID: 1359
		public int affectedCellCount;

		// Token: 0x04000550 RID: 1360
		public BodyPartTagDef bodypartTagTarget;

		// Token: 0x04000551 RID: 1361
		public RulePackDef rangedFireRulepack;

		// Token: 0x04000552 RID: 1362
		public SoundDef soundLanding;

		// Token: 0x04000553 RID: 1363
		public EffecterDef flightEffecterDef;

		// Token: 0x04000554 RID: 1364
		public bool flyWithCarriedThing = true;

		// Token: 0x04000555 RID: 1365
		public MechWorkModeDef workModeDef;

		// Token: 0x04000556 RID: 1366
		public const float DefaultArmorPenetrationPerDamage = 0.015f;

		// Token: 0x04000557 RID: 1367
		private const float VerbSelectionWeightFactor_BodyPart = 0.3f;

		// Token: 0x04000558 RID: 1368
		private const float MinLinkedBodyPartGroupEfficiencyIfMustBeAlwaysUsable = 0.4f;
	}
}
