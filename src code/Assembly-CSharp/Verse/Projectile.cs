using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020003FC RID: 1020
	[StaticConstructorOnStartup]
	public abstract class Projectile : ThingWithComps
	{
		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06001CFE RID: 7422 RVA: 0x000AFCA1 File Offset: 0x000ADEA1
		// (set) Token: 0x06001CFF RID: 7423 RVA: 0x000AFCD1 File Offset: 0x000ADED1
		public ProjectileHitFlags HitFlags
		{
			get
			{
				if (this.def.projectile.alwaysFreeIntercept)
				{
					return ProjectileHitFlags.All;
				}
				if (this.def.projectile.flyOverhead)
				{
					return ProjectileHitFlags.None;
				}
				return this.desiredHitFlags;
			}
			set
			{
				this.desiredHitFlags = value;
			}
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06001D00 RID: 7424 RVA: 0x000AFCDC File Offset: 0x000ADEDC
		protected float StartingTicksToImpact
		{
			get
			{
				float num = (this.origin - this.destination).magnitude / this.def.projectile.SpeedTilesPerTick;
				if (num <= 0f)
				{
					num = 0.001f;
				}
				return num;
			}
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06001D01 RID: 7425 RVA: 0x000AFD23 File Offset: 0x000ADF23
		protected IntVec3 DestinationCell
		{
			get
			{
				return new IntVec3(this.destination);
			}
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06001D02 RID: 7426 RVA: 0x000AFD30 File Offset: 0x000ADF30
		public virtual Vector3 ExactPosition
		{
			get
			{
				Vector3 b = (this.destination - this.origin).Yto0() * this.DistanceCoveredFraction;
				return this.origin.Yto0() + b + Vector3.up * this.def.Altitude;
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06001D03 RID: 7427 RVA: 0x000AFD8A File Offset: 0x000ADF8A
		protected float DistanceCoveredFraction
		{
			get
			{
				return Mathf.Clamp01(1f - (float)this.ticksToImpact / this.StartingTicksToImpact);
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06001D04 RID: 7428 RVA: 0x000AFDA5 File Offset: 0x000ADFA5
		public virtual Quaternion ExactRotation
		{
			get
			{
				return Quaternion.LookRotation((this.destination - this.origin).Yto0());
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06001D05 RID: 7429 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual bool AnimalsFleeImpact
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06001D06 RID: 7430 RVA: 0x000AFDC2 File Offset: 0x000ADFC2
		public override Vector3 DrawPos
		{
			get
			{
				return this.ExactPosition;
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06001D07 RID: 7431 RVA: 0x000AFDCA File Offset: 0x000ADFCA
		public virtual Material DrawMat
		{
			get
			{
				return this.def.DrawMatSingle;
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06001D08 RID: 7432 RVA: 0x000AFDD7 File Offset: 0x000ADFD7
		public virtual int DamageAmount
		{
			get
			{
				return this.def.projectile.GetDamageAmount(this.weaponDamageMultiplier, null);
			}
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06001D09 RID: 7433 RVA: 0x000AFDF0 File Offset: 0x000ADFF0
		public virtual float ArmorPenetration
		{
			get
			{
				return this.def.projectile.GetArmorPenetration(this.weaponDamageMultiplier, null);
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06001D0A RID: 7434 RVA: 0x000AFE09 File Offset: 0x000AE009
		public ThingDef EquipmentDef
		{
			get
			{
				return this.equipmentDef;
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06001D0B RID: 7435 RVA: 0x000AFE11 File Offset: 0x000AE011
		public Thing Launcher
		{
			get
			{
				return this.launcher;
			}
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x000AFE1C File Offset: 0x000AE01C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Vector3>(ref this.origin, "origin", default(Vector3), false);
			Scribe_Values.Look<Vector3>(ref this.destination, "destination", default(Vector3), false);
			Scribe_Values.Look<int>(ref this.ticksToImpact, "ticksToImpact", 0, false);
			Scribe_TargetInfo.Look(ref this.usedTarget, "usedTarget");
			Scribe_TargetInfo.Look(ref this.intendedTarget, "intendedTarget");
			Scribe_References.Look<Thing>(ref this.launcher, "launcher", false);
			Scribe_Defs.Look<ThingDef>(ref this.equipmentDef, "equipmentDef");
			Scribe_Defs.Look<ThingDef>(ref this.targetCoverDef, "targetCoverDef");
			Scribe_Values.Look<ProjectileHitFlags>(ref this.desiredHitFlags, "desiredHitFlags", ProjectileHitFlags.All, false);
			Scribe_Values.Look<float>(ref this.weaponDamageMultiplier, "weaponDamageMultiplier", 1f, false);
			Scribe_Values.Look<bool>(ref this.preventFriendlyFire, "preventFriendlyFire", false, false);
			Scribe_Values.Look<bool>(ref this.landed, "landed", false, false);
		}

		// Token: 0x06001D0D RID: 7437 RVA: 0x000AFF14 File Offset: 0x000AE114
		public void Launch(Thing launcher, LocalTargetInfo usedTarget, LocalTargetInfo intendedTarget, ProjectileHitFlags hitFlags, bool preventFriendlyFire = false, Thing equipment = null)
		{
			this.Launch(launcher, base.Position.ToVector3Shifted(), usedTarget, intendedTarget, hitFlags, preventFriendlyFire, equipment, null);
		}

		// Token: 0x06001D0E RID: 7438 RVA: 0x000AFF40 File Offset: 0x000AE140
		public virtual void Launch(Thing launcher, Vector3 origin, LocalTargetInfo usedTarget, LocalTargetInfo intendedTarget, ProjectileHitFlags hitFlags, bool preventFriendlyFire = false, Thing equipment = null, ThingDef targetCoverDef = null)
		{
			this.launcher = launcher;
			this.origin = origin;
			this.usedTarget = usedTarget;
			this.intendedTarget = intendedTarget;
			this.targetCoverDef = targetCoverDef;
			this.preventFriendlyFire = preventFriendlyFire;
			this.HitFlags = hitFlags;
			if (equipment != null)
			{
				this.equipmentDef = equipment.def;
				this.weaponDamageMultiplier = equipment.GetStatValue(StatDefOf.RangedWeapon_DamageMultiplier, true, -1);
			}
			else
			{
				this.equipmentDef = null;
				this.weaponDamageMultiplier = 1f;
			}
			this.destination = usedTarget.Cell.ToVector3Shifted() + Gen.RandomHorizontalVector(0.3f);
			this.ticksToImpact = Mathf.CeilToInt(this.StartingTicksToImpact);
			if (this.ticksToImpact < 1)
			{
				this.ticksToImpact = 1;
			}
			if (!this.def.projectile.soundAmbient.NullOrUndefined())
			{
				SoundInfo info = SoundInfo.InMap(this, MaintenanceType.PerTick);
				this.ambientSustainer = this.def.projectile.soundAmbient.TrySpawnSustainer(info);
			}
		}

		// Token: 0x06001D0F RID: 7439 RVA: 0x000B0040 File Offset: 0x000AE240
		public override void Tick()
		{
			base.Tick();
			if (this.landed)
			{
				return;
			}
			Vector3 exactPosition = this.ExactPosition;
			this.ticksToImpact--;
			if (!this.ExactPosition.InBounds(base.Map))
			{
				this.ticksToImpact++;
				base.Position = this.ExactPosition.ToIntVec3();
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			Vector3 exactPosition2 = this.ExactPosition;
			if (this.CheckForFreeInterceptBetween(exactPosition, exactPosition2))
			{
				return;
			}
			base.Position = this.ExactPosition.ToIntVec3();
			if (this.ticksToImpact == 60 && Find.TickManager.CurTimeSpeed == TimeSpeed.Normal && this.def.projectile.soundImpactAnticipate != null)
			{
				this.def.projectile.soundImpactAnticipate.PlayOneShot(this);
			}
			if (this.ticksToImpact <= 0)
			{
				if (this.DestinationCell.InBounds(base.Map))
				{
					base.Position = this.DestinationCell;
				}
				this.ImpactSomething();
				return;
			}
			if (this.ambientSustainer != null)
			{
				this.ambientSustainer.Maintain();
			}
		}

		// Token: 0x06001D10 RID: 7440 RVA: 0x000B0154 File Offset: 0x000AE354
		private bool CheckForFreeInterceptBetween(Vector3 lastExactPos, Vector3 newExactPos)
		{
			if (lastExactPos == newExactPos)
			{
				return false;
			}
			List<Thing> list = base.Map.listerThings.ThingsInGroup(ThingRequestGroup.ProjectileInterceptor);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].TryGetComp<CompProjectileInterceptor>().CheckIntercept(this, lastExactPos, newExactPos))
				{
					this.Impact(null, true);
					return true;
				}
			}
			IntVec3 intVec = lastExactPos.ToIntVec3();
			IntVec3 intVec2 = newExactPos.ToIntVec3();
			if (intVec2 == intVec)
			{
				return false;
			}
			if (!intVec.InBounds(base.Map) || !intVec2.InBounds(base.Map))
			{
				return false;
			}
			if (intVec2.AdjacentToCardinal(intVec))
			{
				return this.CheckForFreeIntercept(intVec2);
			}
			if (VerbUtility.InterceptChanceFactorFromDistance(this.origin, intVec2) <= 0f)
			{
				return false;
			}
			Vector3 vector = lastExactPos;
			Vector3 v = newExactPos - lastExactPos;
			Vector3 b = v.normalized * 0.2f;
			int num = (int)(v.MagnitudeHorizontal() / 0.2f);
			Projectile.checkedCells.Clear();
			int num2 = 0;
			for (;;)
			{
				vector += b;
				IntVec3 intVec3 = vector.ToIntVec3();
				if (!Projectile.checkedCells.Contains(intVec3))
				{
					if (this.CheckForFreeIntercept(intVec3))
					{
						break;
					}
					Projectile.checkedCells.Add(intVec3);
				}
				num2++;
				if (num2 > num)
				{
					return false;
				}
				if (intVec3 == intVec2)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001D11 RID: 7441 RVA: 0x000B02A0 File Offset: 0x000AE4A0
		private bool CheckForFreeIntercept(IntVec3 c)
		{
			if (this.destination.ToIntVec3() == c)
			{
				return false;
			}
			float num = VerbUtility.InterceptChanceFactorFromDistance(this.origin, c);
			if (num <= 0f)
			{
				return false;
			}
			bool flag = false;
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (this.CanHit(thing))
				{
					bool flag2 = false;
					if (thing.def.Fillage == FillCategory.Full)
					{
						Building_Door building_Door = thing as Building_Door;
						if (building_Door == null || !building_Door.Open)
						{
							this.ThrowDebugText("int-wall", c);
							this.Impact(thing, false);
							return true;
						}
						flag2 = true;
					}
					float num2 = 0f;
					Pawn pawn = thing as Pawn;
					if (pawn != null)
					{
						num2 = 0.4f * Mathf.Clamp(pawn.BodySize, 0.1f, 2f);
						if (pawn.GetPosture() != PawnPosture.Standing)
						{
							num2 *= 0.1f;
						}
						if (this.launcher != null && pawn.Faction != null && this.launcher.Faction != null && !pawn.Faction.HostileTo(this.launcher.Faction))
						{
							if (this.preventFriendlyFire)
							{
								num2 = 0f;
								this.ThrowDebugText("ff-miss", c);
							}
							else
							{
								num2 *= Find.Storyteller.difficulty.friendlyFireChanceFactor;
							}
						}
					}
					else if (thing.def.fillPercent > 0.2f)
					{
						if (flag2)
						{
							num2 = 0.05f;
						}
						else if (this.DestinationCell.AdjacentTo8Way(c))
						{
							num2 = thing.def.fillPercent * 1f;
						}
						else
						{
							num2 = thing.def.fillPercent * 0.15f;
						}
					}
					num2 *= num;
					if (num2 > 1E-05f)
					{
						if (Rand.Chance(num2))
						{
							this.ThrowDebugText("int-" + num2.ToStringPercent(), c);
							this.Impact(thing, false);
							return true;
						}
						flag = true;
						this.ThrowDebugText(num2.ToStringPercent(), c);
					}
				}
			}
			if (!flag)
			{
				this.ThrowDebugText("o", c);
			}
			return false;
		}

		// Token: 0x06001D12 RID: 7442 RVA: 0x000B04CA File Offset: 0x000AE6CA
		private void ThrowDebugText(string text, IntVec3 c)
		{
			if (DebugViewSettings.drawShooting)
			{
				MoteMaker.ThrowText(c.ToVector3Shifted(), base.Map, text, -1f);
			}
		}

		// Token: 0x06001D13 RID: 7443 RVA: 0x000B04EC File Offset: 0x000AE6EC
		public override void Draw()
		{
			float num = this.ArcHeightFactor * GenMath.InverseParabola(this.DistanceCoveredFraction);
			Vector3 drawPos = this.DrawPos;
			Vector3 position = drawPos + new Vector3(0f, 0f, 1f) * num;
			if (this.def.projectile.shadowSize > 0f)
			{
				this.DrawShadow(drawPos, num);
			}
			Graphics.DrawMesh(MeshPool.GridPlane(this.def.graphicData.drawSize), position, this.ExactRotation, this.DrawMat, 0);
			base.Comps_PostDraw();
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06001D14 RID: 7444 RVA: 0x000B0584 File Offset: 0x000AE784
		private float ArcHeightFactor
		{
			get
			{
				float num = this.def.projectile.arcHeightFactor;
				float num2 = (this.destination - this.origin).MagnitudeHorizontalSquared();
				if (num * num > num2 * 0.2f * 0.2f)
				{
					num = Mathf.Sqrt(num2) * 0.2f;
				}
				return num;
			}
		}

		// Token: 0x06001D15 RID: 7445 RVA: 0x000B05DC File Offset: 0x000AE7DC
		protected bool CanHit(Thing thing)
		{
			if (!thing.Spawned)
			{
				return false;
			}
			if (thing == this.launcher)
			{
				return false;
			}
			ProjectileHitFlags hitFlags = this.HitFlags;
			if (hitFlags == ProjectileHitFlags.None)
			{
				return false;
			}
			if (CoverUtility.ThingCovered(thing, base.Map))
			{
				return false;
			}
			if (thing == this.intendedTarget && (hitFlags & ProjectileHitFlags.IntendedTarget) != ProjectileHitFlags.None)
			{
				return true;
			}
			if (thing != this.intendedTarget)
			{
				if (thing is Pawn)
				{
					if ((hitFlags & ProjectileHitFlags.NonTargetPawns) != ProjectileHitFlags.None)
					{
						return true;
					}
				}
				else if ((hitFlags & ProjectileHitFlags.NonTargetWorld) != ProjectileHitFlags.None)
				{
					return true;
				}
			}
			return thing == this.intendedTarget && thing.def.Fillage == FillCategory.Full;
		}

		// Token: 0x06001D16 RID: 7446 RVA: 0x000B0684 File Offset: 0x000AE884
		protected virtual void ImpactSomething()
		{
			if (this.def.projectile.flyOverhead)
			{
				RoofDef roofDef = base.Map.roofGrid.RoofAt(base.Position);
				if (roofDef != null)
				{
					if (roofDef.isThickRoof)
					{
						this.ThrowDebugText("hit-thick-roof", base.Position);
						this.def.projectile.soundHitThickRoof.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
						this.Destroy(DestroyMode.Vanish);
						return;
					}
					if (base.Position.GetEdifice(base.Map) == null || base.Position.GetEdifice(base.Map).def.Fillage != FillCategory.Full)
					{
						RoofCollapserImmediate.DropRoofInCells(base.Position, base.Map, null);
					}
				}
			}
			if (!this.usedTarget.HasThing || !this.CanHit(this.usedTarget.Thing))
			{
				List<Thing> list = VerbUtility.ThingsToHit(base.Position, base.Map, new Func<Thing, bool>(this.CanHit));
				list.Shuffle<Thing>();
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing = list[i];
					Pawn pawn = thing as Pawn;
					float num;
					if (pawn != null)
					{
						num = 0.5f * Mathf.Clamp(pawn.BodySize, 0.1f, 2f);
						if (pawn.GetPosture() != PawnPosture.Standing && (this.origin - this.destination).MagnitudeHorizontalSquared() >= 20.25f)
						{
							num *= 0.2f;
						}
						if (this.launcher != null && pawn.Faction != null && this.launcher.Faction != null && !pawn.Faction.HostileTo(this.launcher.Faction))
						{
							num *= VerbUtility.InterceptChanceFactorFromDistance(this.origin, base.Position);
						}
					}
					else
					{
						num = 1.5f * thing.def.fillPercent;
					}
					if (Rand.Chance(num))
					{
						this.ThrowDebugText("hit-" + num.ToStringPercent(), base.Position);
						this.Impact(list.RandomElement<Thing>(), false);
						return;
					}
					this.ThrowDebugText("miss-" + num.ToStringPercent(), base.Position);
				}
				this.Impact(null, false);
				return;
			}
			Pawn pawn2 = this.usedTarget.Thing as Pawn;
			if (pawn2 != null && pawn2.GetPosture() != PawnPosture.Standing && (this.origin - this.destination).MagnitudeHorizontalSquared() >= 20.25f && !Rand.Chance(0.2f))
			{
				this.ThrowDebugText("miss-laying", base.Position);
				this.Impact(null, false);
				return;
			}
			this.Impact(this.usedTarget.Thing, false);
		}

		// Token: 0x06001D17 RID: 7447 RVA: 0x000B0948 File Offset: 0x000AEB48
		protected virtual void Impact(Thing hitThing, bool blockedByShield = false)
		{
			GenClamor.DoClamor(this, 12f, ClamorDefOf.Impact);
			if (!blockedByShield && this.def.projectile.landedEffecter != null)
			{
				this.def.projectile.landedEffecter.Spawn(base.Position, base.Map, 1f).Cleanup();
			}
			this.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x06001D18 RID: 7448 RVA: 0x000B09AC File Offset: 0x000AEBAC
		private void DrawShadow(Vector3 drawLoc, float height)
		{
			if (Projectile.shadowMaterial == null)
			{
				return;
			}
			float num = this.def.projectile.shadowSize * Mathf.Lerp(1f, 0.6f, height);
			Vector3 s = new Vector3(num, 1f, num);
			Vector3 b = new Vector3(0f, -0.01f, 0f);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(drawLoc + b, Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, Projectile.shadowMaterial, 0);
		}

		// Token: 0x0400148C RID: 5260
		private static readonly Material shadowMaterial = MaterialPool.MatFrom("Things/Skyfaller/SkyfallerShadowCircle", ShaderDatabase.Transparent);

		// Token: 0x0400148D RID: 5261
		protected Vector3 origin;

		// Token: 0x0400148E RID: 5262
		protected Vector3 destination;

		// Token: 0x0400148F RID: 5263
		public LocalTargetInfo usedTarget;

		// Token: 0x04001490 RID: 5264
		public LocalTargetInfo intendedTarget;

		// Token: 0x04001491 RID: 5265
		protected ThingDef equipmentDef;

		// Token: 0x04001492 RID: 5266
		protected Thing launcher;

		// Token: 0x04001493 RID: 5267
		protected ThingDef targetCoverDef;

		// Token: 0x04001494 RID: 5268
		private ProjectileHitFlags desiredHitFlags = ProjectileHitFlags.All;

		// Token: 0x04001495 RID: 5269
		protected float weaponDamageMultiplier = 1f;

		// Token: 0x04001496 RID: 5270
		protected bool preventFriendlyFire;

		// Token: 0x04001497 RID: 5271
		protected bool landed;

		// Token: 0x04001498 RID: 5272
		protected int ticksToImpact;

		// Token: 0x04001499 RID: 5273
		private Sustainer ambientSustainer;

		// Token: 0x0400149A RID: 5274
		private static List<IntVec3> checkedCells = new List<IntVec3>();
	}
}
