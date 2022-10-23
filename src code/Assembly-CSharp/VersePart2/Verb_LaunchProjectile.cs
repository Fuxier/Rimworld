using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005AD RID: 1453
	public class Verb_LaunchProjectile : Verb
	{
		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x06002C81 RID: 11393 RVA: 0x0011A854 File Offset: 0x00118A54
		public virtual ThingDef Projectile
		{
			get
			{
				if (base.EquipmentSource != null)
				{
					CompChangeableProjectile comp = base.EquipmentSource.GetComp<CompChangeableProjectile>();
					if (comp != null && comp.Loaded)
					{
						return comp.Projectile;
					}
				}
				return this.verbProps.defaultProjectile;
			}
		}

		// Token: 0x06002C82 RID: 11394 RVA: 0x0011A894 File Offset: 0x00118A94
		public override void WarmupComplete()
		{
			base.WarmupComplete();
			Find.BattleLog.Add(new BattleLogEntry_RangedFire(this.caster, this.currentTarget.HasThing ? this.currentTarget.Thing : null, (base.EquipmentSource != null) ? base.EquipmentSource.def : null, this.Projectile, this.ShotsPerBurst > 1));
		}

		// Token: 0x06002C83 RID: 11395 RVA: 0x0011A8FC File Offset: 0x00118AFC
		protected IntVec3 GetForcedMissTarget(float forcedMissRadius)
		{
			if (this.verbProps.forcedMissEvenDispersal)
			{
				if (this.forcedMissTargetEvenDispersalCache.Count <= 0)
				{
					this.forcedMissTargetEvenDispersalCache.AddRange(Verb_LaunchProjectile.GenerateEvenDispersalForcedMissTargets(this.currentTarget.Cell, forcedMissRadius, this.burstShotsLeft));
					this.forcedMissTargetEvenDispersalCache.SortByDescending((IntVec3 p) => p.DistanceToSquared(this.Caster.Position));
				}
				if (this.forcedMissTargetEvenDispersalCache.Count > 0)
				{
					return this.forcedMissTargetEvenDispersalCache.Pop<IntVec3>();
				}
			}
			int max = GenRadial.NumCellsInRadius(forcedMissRadius);
			int num = Rand.Range(0, max);
			return this.currentTarget.Cell + GenRadial.RadialPattern[num];
		}

		// Token: 0x06002C84 RID: 11396 RVA: 0x0011A9A1 File Offset: 0x00118BA1
		private static IEnumerable<IntVec3> GenerateEvenDispersalForcedMissTargets(IntVec3 root, float radius, int count)
		{
			float randomRotationOffset = Rand.Range(0f, 360f);
			float goldenRatio = (1f + Mathf.Pow(5f, 0.5f)) / 2f;
			int num3;
			for (int i = 0; i < count; i = num3 + 1)
			{
				float f = 6.2831855f * (float)i / goldenRatio;
				float f2 = Mathf.Acos(1f - 2f * ((float)i + 0.5f) / (float)count);
				float num = (float)((int)(Mathf.Cos(f) * Mathf.Sin(f2) * radius));
				int num2 = (int)(Mathf.Cos(f2) * radius);
				Vector3 vect = new Vector3(num, 0f, (float)num2).RotatedBy(randomRotationOffset);
				yield return root + vect.ToIntVec3();
				num3 = i;
			}
			yield break;
		}

		// Token: 0x06002C85 RID: 11397 RVA: 0x0011A9C0 File Offset: 0x00118BC0
		protected override bool TryCastShot()
		{
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				return false;
			}
			ThingDef projectile = this.Projectile;
			if (projectile == null)
			{
				return false;
			}
			ShootLine shootLine;
			bool flag = base.TryFindShootLineFromTo(this.caster.Position, this.currentTarget, out shootLine);
			if (this.verbProps.stopBurstWithoutLos && !flag)
			{
				return false;
			}
			if (base.EquipmentSource != null)
			{
				CompChangeableProjectile comp = base.EquipmentSource.GetComp<CompChangeableProjectile>();
				if (comp != null)
				{
					comp.Notify_ProjectileLaunched();
				}
				CompReloadable comp2 = base.EquipmentSource.GetComp<CompReloadable>();
				if (comp2 != null)
				{
					comp2.UsedOnce();
				}
			}
			this.lastShotTick = Find.TickManager.TicksGame;
			Thing thing = this.caster;
			Thing equipment = base.EquipmentSource;
			CompMannable compMannable = this.caster.TryGetComp<CompMannable>();
			if (compMannable != null && compMannable.ManningPawn != null)
			{
				thing = compMannable.ManningPawn;
				equipment = this.caster;
			}
			Vector3 drawPos = this.caster.DrawPos;
			Projectile projectile2 = (Projectile)GenSpawn.Spawn(projectile, shootLine.Source, this.caster.Map, WipeMode.Vanish);
			if (this.verbProps.ForcedMissRadius > 0.5f)
			{
				float num = this.verbProps.ForcedMissRadius;
				Pawn caster;
				if (thing != null && (caster = (thing as Pawn)) != null)
				{
					num *= this.verbProps.GetForceMissFactorFor(equipment, caster);
				}
				float num2 = VerbUtility.CalculateAdjustedForcedMiss(num, this.currentTarget.Cell - this.caster.Position);
				if (num2 > 0.5f)
				{
					IntVec3 forcedMissTarget = this.GetForcedMissTarget(num2);
					if (forcedMissTarget != this.currentTarget.Cell)
					{
						this.ThrowDebugText("ToRadius");
						this.ThrowDebugText("Rad\nDest", forcedMissTarget);
						ProjectileHitFlags projectileHitFlags = ProjectileHitFlags.NonTargetWorld;
						if (Rand.Chance(0.5f))
						{
							projectileHitFlags = ProjectileHitFlags.All;
						}
						if (!this.canHitNonTargetPawnsNow)
						{
							projectileHitFlags &= ~ProjectileHitFlags.NonTargetPawns;
						}
						projectile2.Launch(thing, drawPos, forcedMissTarget, this.currentTarget, projectileHitFlags, this.preventFriendlyFire, equipment, null);
						return true;
					}
				}
			}
			ShotReport shotReport = ShotReport.HitReportFor(this.caster, this, this.currentTarget);
			Thing randomCoverToMissInto = shotReport.GetRandomCoverToMissInto();
			ThingDef targetCoverDef = (randomCoverToMissInto != null) ? randomCoverToMissInto.def : null;
			if (this.verbProps.canGoWild && !Rand.Chance(shotReport.AimOnTargetChance_IgnoringPosture))
			{
				shootLine.ChangeDestToMissWild(shotReport.AimOnTargetChance_StandardTarget);
				this.ThrowDebugText("ToWild" + (this.canHitNonTargetPawnsNow ? "\nchntp" : ""));
				this.ThrowDebugText("Wild\nDest", shootLine.Dest);
				ProjectileHitFlags projectileHitFlags2 = ProjectileHitFlags.NonTargetWorld;
				if (Rand.Chance(0.5f) && this.canHitNonTargetPawnsNow)
				{
					projectileHitFlags2 |= ProjectileHitFlags.NonTargetPawns;
				}
				projectile2.Launch(thing, drawPos, shootLine.Dest, this.currentTarget, projectileHitFlags2, this.preventFriendlyFire, equipment, targetCoverDef);
				return true;
			}
			if (this.currentTarget.Thing != null && this.currentTarget.Thing.def.CanBenefitFromCover && !Rand.Chance(shotReport.PassCoverChance))
			{
				this.ThrowDebugText("ToCover" + (this.canHitNonTargetPawnsNow ? "\nchntp" : ""));
				this.ThrowDebugText("Cover\nDest", randomCoverToMissInto.Position);
				ProjectileHitFlags projectileHitFlags3 = ProjectileHitFlags.NonTargetWorld;
				if (this.canHitNonTargetPawnsNow)
				{
					projectileHitFlags3 |= ProjectileHitFlags.NonTargetPawns;
				}
				projectile2.Launch(thing, drawPos, randomCoverToMissInto, this.currentTarget, projectileHitFlags3, this.preventFriendlyFire, equipment, targetCoverDef);
				return true;
			}
			ProjectileHitFlags projectileHitFlags4 = ProjectileHitFlags.IntendedTarget;
			if (this.canHitNonTargetPawnsNow)
			{
				projectileHitFlags4 |= ProjectileHitFlags.NonTargetPawns;
			}
			if (!this.currentTarget.HasThing || this.currentTarget.Thing.def.Fillage == FillCategory.Full)
			{
				projectileHitFlags4 |= ProjectileHitFlags.NonTargetWorld;
			}
			this.ThrowDebugText("ToHit" + (this.canHitNonTargetPawnsNow ? "\nchntp" : ""));
			if (this.currentTarget.Thing != null)
			{
				projectile2.Launch(thing, drawPos, this.currentTarget, this.currentTarget, projectileHitFlags4, this.preventFriendlyFire, equipment, targetCoverDef);
				this.ThrowDebugText("Hit\nDest", this.currentTarget.Cell);
			}
			else
			{
				projectile2.Launch(thing, drawPos, shootLine.Dest, this.currentTarget, projectileHitFlags4, this.preventFriendlyFire, equipment, targetCoverDef);
				this.ThrowDebugText("Hit\nDest", shootLine.Dest);
			}
			return true;
		}

		// Token: 0x06002C86 RID: 11398 RVA: 0x0011AE23 File Offset: 0x00119023
		private void ThrowDebugText(string text)
		{
			if (DebugViewSettings.drawShooting)
			{
				MoteMaker.ThrowText(this.caster.DrawPos, this.caster.Map, text, -1f);
			}
		}

		// Token: 0x06002C87 RID: 11399 RVA: 0x0011AE4D File Offset: 0x0011904D
		private void ThrowDebugText(string text, IntVec3 c)
		{
			if (DebugViewSettings.drawShooting)
			{
				MoteMaker.ThrowText(c.ToVector3Shifted(), this.caster.Map, text, -1f);
			}
		}

		// Token: 0x06002C88 RID: 11400 RVA: 0x0011AE74 File Offset: 0x00119074
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = true;
			ThingDef projectile = this.Projectile;
			if (projectile == null)
			{
				return 0f;
			}
			float num = projectile.projectile.explosionRadius + projectile.projectile.explosionRadiusDisplayPadding;
			float forcedMissRadius = this.verbProps.ForcedMissRadius;
			if (forcedMissRadius > 0f && this.verbProps.burstShotCount > 1)
			{
				num += forcedMissRadius;
			}
			return num;
		}

		// Token: 0x06002C89 RID: 11401 RVA: 0x0011AED4 File Offset: 0x001190D4
		public override bool Available()
		{
			if (!base.Available())
			{
				return false;
			}
			if (this.CasterIsPawn)
			{
				Pawn casterPawn = this.CasterPawn;
				if (casterPawn.Faction != Faction.OfPlayer && !this.verbProps.ai_ProjectileLaunchingIgnoresMeleeThreats && casterPawn.mindState.MeleeThreatStillThreat && casterPawn.mindState.meleeThreat.Position.AdjacentTo8WayOrInside(casterPawn.Position))
				{
					return false;
				}
			}
			return this.Projectile != null;
		}

		// Token: 0x06002C8A RID: 11402 RVA: 0x0011AF49 File Offset: 0x00119149
		public override void Reset()
		{
			base.Reset();
			this.forcedMissTargetEvenDispersalCache.Clear();
		}

		// Token: 0x04001D2D RID: 7469
		private List<IntVec3> forcedMissTargetEvenDispersalCache = new List<IntVec3>();
	}
}
