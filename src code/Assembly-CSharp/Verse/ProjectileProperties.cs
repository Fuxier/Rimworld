using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000CF RID: 207
	public class ProjectileProperties
	{
		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600062D RID: 1581 RVA: 0x00021BE7 File Offset: 0x0001FDE7
		public float StoppingPower
		{
			get
			{
				if (this.stoppingPower != 0f)
				{
					return this.stoppingPower;
				}
				if (this.damageDef != null)
				{
					return this.damageDef.defaultStoppingPower;
				}
				return 0f;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600062E RID: 1582 RVA: 0x00021C16 File Offset: 0x0001FE16
		public float SpeedTilesPerTick
		{
			get
			{
				return this.speed / 100f;
			}
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00021C24 File Offset: 0x0001FE24
		public int GetDamageAmount(Thing weapon, StringBuilder explanation = null)
		{
			float weaponDamageMultiplier = (weapon != null) ? weapon.GetStatValue(StatDefOf.RangedWeapon_DamageMultiplier, true, -1) : 1f;
			return this.GetDamageAmount(weaponDamageMultiplier, explanation);
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00021C54 File Offset: 0x0001FE54
		public int GetDamageAmount(ThingDef weapon, ThingDef weaponStuff, StringBuilder explanation = null)
		{
			float weaponDamageMultiplier = (weapon != null) ? weapon.GetStatValueAbstract(StatDefOf.RangedWeapon_DamageMultiplier, weaponStuff) : 1f;
			return this.GetDamageAmount(weaponDamageMultiplier, explanation);
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x00021C80 File Offset: 0x0001FE80
		public int GetDamageAmount(float weaponDamageMultiplier, StringBuilder explanation = null)
		{
			int num;
			if (this.damageAmountBase != -1)
			{
				num = this.damageAmountBase;
			}
			else
			{
				if (this.damageDef == null)
				{
					Log.ErrorOnce("Failed to find sane damage amount", 91094882);
					return 1;
				}
				num = this.damageDef.defaultDamage;
			}
			if (explanation != null)
			{
				explanation.AppendLine("StatsReport_BaseValue".Translate() + ": " + num);
				explanation.Append("StatsReport_QualityMultiplier".Translate() + ": " + weaponDamageMultiplier.ToStringPercent());
			}
			num = Mathf.RoundToInt((float)num * weaponDamageMultiplier);
			if (explanation != null)
			{
				explanation.AppendLine();
				explanation.AppendLine();
				explanation.Append("StatsReport_FinalValue".Translate() + ": " + num);
			}
			return num;
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x00021D68 File Offset: 0x0001FF68
		public float GetArmorPenetration(Thing weapon, StringBuilder explanation = null)
		{
			float weaponDamageMultiplier = (weapon != null) ? weapon.GetStatValue(StatDefOf.RangedWeapon_DamageMultiplier, true, -1) : 1f;
			return this.GetArmorPenetration(weaponDamageMultiplier, explanation);
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x00021D98 File Offset: 0x0001FF98
		public float GetArmorPenetration(float weaponDamageMultiplier, StringBuilder explanation = null)
		{
			if (this.damageDef.armorCategory == null)
			{
				return 0f;
			}
			float num;
			if (this.damageAmountBase != -1 || this.armorPenetrationBase >= 0f)
			{
				num = this.armorPenetrationBase;
			}
			else
			{
				if (this.damageDef == null)
				{
					return 0f;
				}
				num = this.damageDef.defaultArmorPenetration;
			}
			if (num < 0f)
			{
				num = (float)this.GetDamageAmount(null, null) * 0.015f;
			}
			if (explanation != null)
			{
				explanation.AppendLine("StatsReport_BaseValue".Translate() + ": " + num.ToStringPercent());
				explanation.AppendLine();
				explanation.Append("StatsReport_QualityMultiplier".Translate() + ": " + weaponDamageMultiplier.ToStringPercent());
			}
			num *= weaponDamageMultiplier;
			if (explanation != null)
			{
				explanation.AppendLine();
				explanation.AppendLine();
				explanation.Append("StatsReport_FinalValue".Translate() + ": " + num.ToStringPercent());
			}
			return num;
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x00021EAC File Offset: 0x000200AC
		public IEnumerable<string> ConfigErrors(ThingDef parent)
		{
			if (this.alwaysFreeIntercept && this.flyOverhead)
			{
				yield return "alwaysFreeIntercept and flyOverhead are both true";
			}
			if (this.damageAmountBase == -1 && this.damageDef != null && this.damageDef.defaultDamage == -1)
			{
				yield return "no damage amount specified for projectile";
			}
			if (this.filth != null && this.filthCount.TrueMax <= 0)
			{
				yield return "has filth but filthCount <= 0.";
			}
			yield break;
		}

		// Token: 0x040003F8 RID: 1016
		public float speed = 5f;

		// Token: 0x040003F9 RID: 1017
		public bool flyOverhead;

		// Token: 0x040003FA RID: 1018
		public bool alwaysFreeIntercept;

		// Token: 0x040003FB RID: 1019
		public DamageDef damageDef;

		// Token: 0x040003FC RID: 1020
		private int damageAmountBase = -1;

		// Token: 0x040003FD RID: 1021
		private float armorPenetrationBase = -1f;

		// Token: 0x040003FE RID: 1022
		public float stoppingPower = 0.5f;

		// Token: 0x040003FF RID: 1023
		public List<ExtraDamage> extraDamages;

		// Token: 0x04000400 RID: 1024
		public float arcHeightFactor;

		// Token: 0x04000401 RID: 1025
		public float shadowSize;

		// Token: 0x04000402 RID: 1026
		public EffecterDef landedEffecter;

		// Token: 0x04000403 RID: 1027
		public SoundDef soundHitThickRoof;

		// Token: 0x04000404 RID: 1028
		public SoundDef soundExplode;

		// Token: 0x04000405 RID: 1029
		public SoundDef soundImpactAnticipate;

		// Token: 0x04000406 RID: 1030
		public SoundDef soundAmbient;

		// Token: 0x04000407 RID: 1031
		public SoundDef soundImpact;

		// Token: 0x04000408 RID: 1032
		public float explosionRadius;

		// Token: 0x04000409 RID: 1033
		public float explosionRadiusDisplayPadding;

		// Token: 0x0400040A RID: 1034
		public int explosionDelay;

		// Token: 0x0400040B RID: 1035
		public ThingDef preExplosionSpawnThingDef;

		// Token: 0x0400040C RID: 1036
		public float preExplosionSpawnChance = 1f;

		// Token: 0x0400040D RID: 1037
		public int preExplosionSpawnThingCount = 1;

		// Token: 0x0400040E RID: 1038
		public ThingDef postExplosionSpawnThingDef;

		// Token: 0x0400040F RID: 1039
		public ThingDef postExplosionSpawnThingDefWater;

		// Token: 0x04000410 RID: 1040
		public float postExplosionSpawnChance = 1f;

		// Token: 0x04000411 RID: 1041
		public int postExplosionSpawnThingCount = 1;

		// Token: 0x04000412 RID: 1042
		public GasType? postExplosionGasType;

		// Token: 0x04000413 RID: 1043
		public bool applyDamageToExplosionCellsNeighbors;

		// Token: 0x04000414 RID: 1044
		public float explosionChanceToStartFire;

		// Token: 0x04000415 RID: 1045
		public bool explosionDamageFalloff;

		// Token: 0x04000416 RID: 1046
		public float screenShakeFactor = 1f;

		// Token: 0x04000417 RID: 1047
		public ThingDef filth;

		// Token: 0x04000418 RID: 1048
		public IntRange filthCount;

		// Token: 0x04000419 RID: 1049
		public int numExtraHitCells;

		// Token: 0x0400041A RID: 1050
		public float bulletChanceToStartFire;

		// Token: 0x0400041B RID: 1051
		public FloatRange bulletFireSizeRange = new FloatRange(0.55f, 0.85f);

		// Token: 0x0400041C RID: 1052
		public EffecterDef explosionEffect;

		// Token: 0x0400041D RID: 1053
		public bool ai_IsIncendiary;
	}
}
