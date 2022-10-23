using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005AC RID: 1452
	public static class VerbUtility
	{
		// Token: 0x06002C6D RID: 11373 RVA: 0x0011A354 File Offset: 0x00118554
		public static ThingDef GetProjectile(this Verb verb)
		{
			Verb_LaunchProjectile verb_LaunchProjectile = verb as Verb_LaunchProjectile;
			if (verb_LaunchProjectile == null)
			{
				return null;
			}
			return verb_LaunchProjectile.Projectile;
		}

		// Token: 0x06002C6E RID: 11374 RVA: 0x0011A374 File Offset: 0x00118574
		public static DamageDef GetDamageDef(this Verb verb)
		{
			if (!verb.verbProps.LaunchesProjectile)
			{
				return verb.verbProps.meleeDamageDef;
			}
			ThingDef projectile = verb.GetProjectile();
			if (projectile != null)
			{
				return projectile.projectile.damageDef;
			}
			return null;
		}

		// Token: 0x06002C6F RID: 11375 RVA: 0x0011A3B4 File Offset: 0x001185B4
		public static bool IsIncendiary_Ranged(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.ai_IsIncendiary;
		}

		// Token: 0x06002C70 RID: 11376 RVA: 0x0011A3D8 File Offset: 0x001185D8
		public static bool IsIncendiary_Melee(this Verb verb)
		{
			if (verb.tool != null && !verb.tool.extraMeleeDamages.NullOrEmpty<ExtraDamage>())
			{
				for (int i = 0; i < verb.tool.extraMeleeDamages.Count; i++)
				{
					if (verb.tool.extraMeleeDamages[i].def == DamageDefOf.Flame)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002C71 RID: 11377 RVA: 0x0011A43C File Offset: 0x0011863C
		public static bool ProjectileFliesOverhead(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.flyOverhead;
		}

		// Token: 0x06002C72 RID: 11378 RVA: 0x0011A460 File Offset: 0x00118660
		public static bool HarmsHealth(this Verb verb)
		{
			DamageDef damageDef = verb.GetDamageDef();
			return damageDef != null && damageDef.harmsHealth;
		}

		// Token: 0x06002C73 RID: 11379 RVA: 0x0011A47F File Offset: 0x0011867F
		public static bool IsEMP(this Verb verb)
		{
			return verb.GetDamageDef() == DamageDefOf.EMP;
		}

		// Token: 0x06002C74 RID: 11380 RVA: 0x0011A490 File Offset: 0x00118690
		public static bool UsesExplosiveProjectiles(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.explosionRadius > 0f;
		}

		// Token: 0x06002C75 RID: 11381 RVA: 0x0011A4BC File Offset: 0x001186BC
		public static List<Verb> GetConcreteExampleVerbs(Def def, ThingDef stuff = null)
		{
			List<Verb> result = null;
			ThingDef thingDef = def as ThingDef;
			if (thingDef != null)
			{
				Thing concreteExample = thingDef.GetConcreteExample(stuff);
				if (concreteExample is Pawn)
				{
					result = ((Pawn)concreteExample).VerbTracker.AllVerbs;
				}
				else if (concreteExample is ThingWithComps)
				{
					result = ((ThingWithComps)concreteExample).GetComp<CompEquippable>().AllVerbs;
				}
				else
				{
					result = null;
				}
			}
			HediffDef hediffDef = def as HediffDef;
			if (hediffDef != null)
			{
				result = hediffDef.ConcreteExample.TryGetComp<HediffComp_VerbGiver>().VerbTracker.AllVerbs;
			}
			return result;
		}

		// Token: 0x06002C76 RID: 11382 RVA: 0x0011A538 File Offset: 0x00118738
		public static float CalculateAdjustedForcedMiss(float forcedMiss, IntVec3 vector)
		{
			float num = (float)vector.LengthHorizontalSquared;
			if (num < 9f)
			{
				return 0f;
			}
			if (num < 25f)
			{
				return forcedMiss * 0.5f;
			}
			if (num < 49f)
			{
				return forcedMiss * 0.8f;
			}
			return forcedMiss;
		}

		// Token: 0x06002C77 RID: 11383 RVA: 0x0011A580 File Offset: 0x00118780
		public static float InterceptChanceFactorFromDistance(Vector3 origin, IntVec3 c)
		{
			float num = (c.ToVector3Shifted() - origin).MagnitudeHorizontalSquared();
			if (num <= 25f)
			{
				return 0f;
			}
			if (num >= 144f)
			{
				return 1f;
			}
			return Mathf.InverseLerp(25f, 144f, num);
		}

		// Token: 0x06002C78 RID: 11384 RVA: 0x0011A5CC File Offset: 0x001187CC
		public static IEnumerable<VerbUtility.VerbPropertiesWithSource> GetAllVerbProperties(List<VerbProperties> verbProps, List<Tool> tools)
		{
			if (verbProps != null)
			{
				int num;
				for (int i = 0; i < verbProps.Count; i = num + 1)
				{
					yield return new VerbUtility.VerbPropertiesWithSource(verbProps[i]);
					num = i;
				}
			}
			if (tools != null)
			{
				int num;
				for (int i = 0; i < tools.Count; i = num + 1)
				{
					foreach (ManeuverDef maneuverDef in tools[i].Maneuvers)
					{
						yield return new VerbUtility.VerbPropertiesWithSource(maneuverDef.verb, tools[i], maneuverDef);
					}
					IEnumerator<ManeuverDef> enumerator = null;
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06002C79 RID: 11385 RVA: 0x0011A5E4 File Offset: 0x001187E4
		public static bool AllowAdjacentShot(LocalTargetInfo target, Thing caster)
		{
			if (!(caster is Pawn))
			{
				return true;
			}
			Pawn pawn = target.Thing as Pawn;
			return pawn == null || !pawn.HostileTo(caster) || pawn.Downed;
		}

		// Token: 0x06002C7A RID: 11386 RVA: 0x0011A61C File Offset: 0x0011881C
		public static VerbSelectionCategory GetSelectionCategory(this Verb v, Pawn p, float highestWeight)
		{
			float num = VerbUtility.InitialVerbWeight(v, p);
			if (num >= highestWeight * 0.95f)
			{
				return VerbSelectionCategory.Best;
			}
			if (num < highestWeight * 0.25f)
			{
				return VerbSelectionCategory.Worst;
			}
			return VerbSelectionCategory.Mid;
		}

		// Token: 0x06002C7B RID: 11387 RVA: 0x0011A64A File Offset: 0x0011884A
		public static float InitialVerbWeight(Verb v, Pawn p)
		{
			return VerbUtility.DPS(v, p) * VerbUtility.AdditionalSelectionFactor(v);
		}

		// Token: 0x06002C7C RID: 11388 RVA: 0x0011A65A File Offset: 0x0011885A
		public static float DPS(Verb v, Pawn p)
		{
			return v.verbProps.AdjustedMeleeDamageAmount(v, p) * (1f + v.verbProps.AdjustedArmorPenetration(v, p)) * v.verbProps.accuracyTouch / v.verbProps.AdjustedFullCycleTime(v, p);
		}

		// Token: 0x06002C7D RID: 11389 RVA: 0x0011A698 File Offset: 0x00118898
		private static float AdditionalSelectionFactor(Verb v)
		{
			float num = (v.tool != null) ? v.tool.chanceFactor : 1f;
			if (v.verbProps.meleeDamageDef != null && !v.verbProps.meleeDamageDef.additionalHediffs.NullOrEmpty<DamageDefAdditionalHediff>())
			{
				foreach (DamageDefAdditionalHediff damageDefAdditionalHediff in v.verbProps.meleeDamageDef.additionalHediffs)
				{
					num += 0.1f;
				}
			}
			return num;
		}

		// Token: 0x06002C7E RID: 11390 RVA: 0x0011A738 File Offset: 0x00118938
		public static float FinalSelectionWeight(Verb verb, Pawn p, List<Verb> allMeleeVerbs, float highestWeight)
		{
			VerbSelectionCategory selectionCategory = verb.GetSelectionCategory(p, highestWeight);
			if (selectionCategory == VerbSelectionCategory.Worst)
			{
				return 0f;
			}
			int num = 0;
			using (List<Verb>.Enumerator enumerator = allMeleeVerbs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetSelectionCategory(p, highestWeight) == selectionCategory)
					{
						num++;
					}
				}
			}
			return 1f / (float)num * ((selectionCategory == VerbSelectionCategory.Mid) ? 0.25f : 0.75f);
		}

		// Token: 0x06002C7F RID: 11391 RVA: 0x0011A7BC File Offset: 0x001189BC
		public static List<Thing> ThingsToHit(IntVec3 cell, Map map, Func<Thing, bool> filter)
		{
			VerbUtility.cellThingsFiltered.Clear();
			List<Thing> thingList = cell.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if ((thing.def.category == ThingCategory.Building || thing.def.category == ThingCategory.Pawn || thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Plant) && filter(thing))
				{
					VerbUtility.cellThingsFiltered.Add(thing);
				}
			}
			return VerbUtility.cellThingsFiltered;
		}

		// Token: 0x04001D2C RID: 7468
		private static readonly List<Thing> cellThingsFiltered = new List<Thing>();

		// Token: 0x02002155 RID: 8533
		public struct VerbPropertiesWithSource
		{
			// Token: 0x17001F4B RID: 8011
			// (get) Token: 0x0600C6C0 RID: 50880 RVA: 0x0043F3FA File Offset: 0x0043D5FA
			public ToolCapacityDef ToolCapacity
			{
				get
				{
					if (this.maneuver == null)
					{
						return null;
					}
					return this.maneuver.requiredCapacity;
				}
			}

			// Token: 0x0600C6C1 RID: 50881 RVA: 0x0043F411 File Offset: 0x0043D611
			public VerbPropertiesWithSource(VerbProperties verbProps)
			{
				this.verbProps = verbProps;
				this.tool = null;
				this.maneuver = null;
			}

			// Token: 0x0600C6C2 RID: 50882 RVA: 0x0043F428 File Offset: 0x0043D628
			public VerbPropertiesWithSource(VerbProperties verbProps, Tool tool, ManeuverDef maneuver)
			{
				this.verbProps = verbProps;
				this.tool = tool;
				this.maneuver = maneuver;
			}

			// Token: 0x04008425 RID: 33829
			public VerbProperties verbProps;

			// Token: 0x04008426 RID: 33830
			public Tool tool;

			// Token: 0x04008427 RID: 33831
			public ManeuverDef maneuver;
		}
	}
}
