using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020005B0 RID: 1456
	public class Verb_ShootBeam : Verb
	{
		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06002C96 RID: 11414 RVA: 0x0011AFF1 File Offset: 0x001191F1
		protected override int ShotsPerBurst
		{
			get
			{
				return this.verbProps.burstShotCount;
			}
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x06002C97 RID: 11415 RVA: 0x0011B0B3 File Offset: 0x001192B3
		public float ShotProgress
		{
			get
			{
				return (float)this.ticksToNextPathStep / (float)this.verbProps.ticksBetweenBurstShots;
			}
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06002C98 RID: 11416 RVA: 0x0011B0CC File Offset: 0x001192CC
		public Vector3 InterpolatedPosition
		{
			get
			{
				Vector3 b = base.CurrentTarget.CenterVector3 - this.initialTargetPosition;
				return Vector3.Lerp(this.path[this.burstShotsLeft], this.path[Mathf.Min(this.burstShotsLeft + 1, this.path.Count - 1)], this.ShotProgress) + b;
			}
		}

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x06002C99 RID: 11417 RVA: 0x0011B13C File Offset: 0x0011933C
		public override float? AimAngleOverride
		{
			get
			{
				if (this.state != VerbState.Bursting)
				{
					return null;
				}
				return new float?((this.InterpolatedPosition - this.caster.DrawPos).AngleFlat());
			}
		}

		// Token: 0x06002C9A RID: 11418 RVA: 0x0011B17C File Offset: 0x0011937C
		protected override bool TryCastShot()
		{
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
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
			this.ticksToNextPathStep = this.verbProps.ticksBetweenBurstShots;
			IntVec3 intVec = this.InterpolatedPosition.Yto0().ToIntVec3();
			IntVec3 intVec2 = GenSight.LastPointOnLineOfSight(this.caster.Position, intVec, (IntVec3 c) => c.CanBeSeenOverFast(this.caster.Map), true);
			this.HitCell(intVec2.IsValid ? intVec2 : intVec);
			return true;
		}

		// Token: 0x06002C9B RID: 11419 RVA: 0x0011B27A File Offset: 0x0011947A
		public override bool TryStartCastOn(LocalTargetInfo castTarg, LocalTargetInfo destTarg, bool surpriseAttack = false, bool canHitNonTargetPawns = true, bool preventFriendlyFire = false, bool nonInterruptingSelfCast = false)
		{
			return base.TryStartCastOn(this.verbProps.beamTargetsGround ? castTarg.Cell : castTarg, destTarg, surpriseAttack, canHitNonTargetPawns, preventFriendlyFire, nonInterruptingSelfCast);
		}

		// Token: 0x06002C9C RID: 11420 RVA: 0x0011B2A8 File Offset: 0x001194A8
		public override void BurstingTick()
		{
			this.ticksToNextPathStep--;
			Vector3 vector = this.InterpolatedPosition;
			IntVec3 intVec = vector.ToIntVec3();
			Vector3 vector2 = this.InterpolatedPosition - this.caster.Position.ToVector3Shifted();
			float num = vector2.MagnitudeHorizontal();
			Vector3 normalized = vector2.Yto0().normalized;
			IntVec3 b = GenSight.LastPointOnLineOfSight(this.caster.Position, intVec, (IntVec3 c) => c.CanBeSeenOverFast(this.caster.Map), true);
			if (b.IsValid)
			{
				num -= (intVec - b).LengthHorizontal;
				vector = this.caster.Position.ToVector3Shifted() + normalized * num;
				intVec = vector.ToIntVec3();
			}
			Vector3 offsetA = normalized * this.verbProps.beamStartOffset;
			Vector3 vector3 = vector - intVec.ToVector3Shifted();
			if (this.mote != null)
			{
				this.mote.UpdateTargets(new TargetInfo(this.caster.Position, this.caster.Map, false), new TargetInfo(intVec, this.caster.Map, false), offsetA, vector3);
				this.mote.Maintain();
			}
			if (this.verbProps.beamGroundFleckDef != null && Rand.Chance(this.verbProps.beamFleckChancePerTick))
			{
				FleckMaker.Static(vector, this.caster.Map, this.verbProps.beamGroundFleckDef, 1f);
			}
			if (this.endEffecter == null && this.verbProps.beamEndEffecterDef != null)
			{
				this.endEffecter = this.verbProps.beamEndEffecterDef.Spawn(intVec, this.caster.Map, vector3, 1f);
			}
			if (this.endEffecter != null)
			{
				this.endEffecter.offset = vector3;
				this.endEffecter.EffectTick(new TargetInfo(intVec, this.caster.Map, false), TargetInfo.Invalid);
				this.endEffecter.ticksLeft--;
			}
			if (this.verbProps.beamLineFleckDef != null)
			{
				float num2 = 1f * num;
				int num3 = 0;
				while ((float)num3 < num2)
				{
					if (Rand.Chance(this.verbProps.beamLineFleckChanceCurve.Evaluate((float)num3 / num2)))
					{
						Vector3 b2 = (float)num3 * normalized - normalized * Rand.Value + normalized / 2f;
						FleckMaker.Static(this.caster.Position.ToVector3Shifted() + b2, this.caster.Map, this.verbProps.beamLineFleckDef, 1f);
					}
					num3++;
				}
			}
			Sustainer sustainer = this.sustainer;
			if (sustainer == null)
			{
				return;
			}
			sustainer.Maintain();
		}

		// Token: 0x06002C9D RID: 11421 RVA: 0x0011B56C File Offset: 0x0011976C
		public override void WarmupComplete()
		{
			this.burstShotsLeft = this.ShotsPerBurst;
			this.state = VerbState.Bursting;
			this.initialTargetPosition = this.currentTarget.CenterVector3;
			this.path.Clear();
			Vector3 vector = (this.currentTarget.CenterVector3 - this.caster.Position.ToVector3Shifted()).Yto0();
			float magnitude = vector.magnitude;
			Vector3 normalized = vector.normalized;
			Vector3 a = normalized.RotatedBy(-90f);
			float num = (this.verbProps.beamFullWidthRange > 0f) ? Mathf.Min(magnitude / this.verbProps.beamFullWidthRange, 1f) : 1f;
			float d = (this.verbProps.beamWidth + 1f) * num / (float)this.ShotsPerBurst;
			Vector3 vector2 = this.currentTarget.CenterVector3.Yto0() - a * this.verbProps.beamWidth / 2f * num;
			this.path.Add(vector2);
			for (int i = 0; i < this.ShotsPerBurst; i++)
			{
				Vector3 a2 = normalized * (Rand.Value * this.verbProps.beamMaxDeviation) - normalized / 2f;
				Vector3 b = Mathf.Sin(((float)i / (float)this.ShotsPerBurst + 0.5f) * 3.1415927f * 57.29578f) * this.verbProps.beamCurvature * -normalized - normalized * this.verbProps.beamMaxDeviation / 2f;
				this.path.Add(vector2 + (a2 + b) * num);
				vector2 += a * d;
			}
			if (this.verbProps.beamMoteDef != null)
			{
				this.mote = MoteMaker.MakeInteractionOverlay(this.verbProps.beamMoteDef, this.caster, new TargetInfo(this.path[0].ToIntVec3(), this.caster.Map, false));
			}
			base.TryCastNextBurstShot();
			this.ticksToNextPathStep = this.verbProps.ticksBetweenBurstShots;
			Effecter effecter = this.endEffecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			if (this.verbProps.soundCastBeam != null)
			{
				this.sustainer = this.verbProps.soundCastBeam.TrySpawnSustainer(SoundInfo.InMap(this.caster, MaintenanceType.PerTick));
			}
		}

		// Token: 0x06002C9E RID: 11422 RVA: 0x0011B802 File Offset: 0x00119A02
		private bool CanHit(Thing thing)
		{
			return thing.Spawned && !CoverUtility.ThingCovered(thing, this.caster.Map);
		}

		// Token: 0x06002C9F RID: 11423 RVA: 0x0011B822 File Offset: 0x00119A22
		private void HitCell(IntVec3 cell)
		{
			this.ApplyDamage(VerbUtility.ThingsToHit(cell, this.caster.Map, new Func<Thing, bool>(this.CanHit)).RandomElementWithFallback(null));
		}

		// Token: 0x06002CA0 RID: 11424 RVA: 0x0011B850 File Offset: 0x00119A50
		private void ApplyDamage(Thing thing)
		{
			IntVec3 intVec = this.InterpolatedPosition.Yto0().ToIntVec3();
			IntVec3 intVec2 = GenSight.LastPointOnLineOfSight(this.caster.Position, intVec, (IntVec3 c) => c.CanBeSeenOverFast(this.caster.Map), true);
			if (intVec2.IsValid)
			{
				intVec = intVec2;
			}
			Map map = this.caster.Map;
			if (thing != null && this.verbProps.beamDamageDef != null)
			{
				float angleFlat = (this.currentTarget.Cell - this.caster.Position).AngleFlat;
				BattleLogEntry_RangedImpact log = new BattleLogEntry_RangedImpact(base.EquipmentSource, thing, this.currentTarget.Thing, base.EquipmentSource.def, null, null);
				DamageInfo dinfo = new DamageInfo(this.verbProps.beamDamageDef, (float)this.verbProps.beamDamageDef.defaultDamage, this.verbProps.beamDamageDef.defaultArmorPenetration, angleFlat, base.EquipmentSource, null, base.EquipmentSource.def, DamageInfo.SourceCategory.ThingOrUnknown, this.currentTarget.Thing, true, true);
				thing.TakeDamage(dinfo).AssociateWithLog(log);
				if (thing.CanEverAttachFire())
				{
					if (Rand.Chance(this.verbProps.beamChanceToAttachFire))
					{
						thing.TryAttachFire(this.verbProps.beamFireSizeRange.RandomInRange);
						return;
					}
				}
				else if (Rand.Chance(this.verbProps.beamChanceToStartFire))
				{
					FireUtility.TryStartFireIn(intVec, map, this.verbProps.beamFireSizeRange.RandomInRange);
				}
			}
		}

		// Token: 0x06002CA1 RID: 11425 RVA: 0x0011B9C4 File Offset: 0x00119BC4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Vector3>(ref this.path, "path", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.ticksToNextPathStep, "ticksToNextPathStep", 0, false);
			Scribe_Values.Look<Vector3>(ref this.initialTargetPosition, "initialTargetPosition", default(Vector3), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.path == null)
			{
				this.path = new List<Vector3>();
			}
		}

		// Token: 0x04001D2E RID: 7470
		private List<Vector3> path = new List<Vector3>();

		// Token: 0x04001D2F RID: 7471
		private int ticksToNextPathStep;

		// Token: 0x04001D30 RID: 7472
		private Vector3 initialTargetPosition;

		// Token: 0x04001D31 RID: 7473
		private MoteDualAttached mote;

		// Token: 0x04001D32 RID: 7474
		private Effecter endEffecter;

		// Token: 0x04001D33 RID: 7475
		private Sustainer sustainer;

		// Token: 0x04001D34 RID: 7476
		private const int NumSubdivisionsPerUnitLength = 1;
	}
}
