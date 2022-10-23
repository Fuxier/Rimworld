using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000C6 RID: 198
	public class DamageWorker
	{
		// Token: 0x06000611 RID: 1553 RVA: 0x00020B90 File Offset: 0x0001ED90
		public virtual DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing victim)
		{
			DamageWorker.DamageResult damageResult = new DamageWorker.DamageResult();
			if (victim.SpawnedOrAnyParentSpawned)
			{
				ImpactSoundUtility.PlayImpactSound(victim, dinfo.Def.impactSoundType, victim.MapHeld);
			}
			if (victim.def.useHitPoints && dinfo.Def.harmsHealth)
			{
				float num = dinfo.Amount;
				if (victim.def.category == ThingCategory.Building)
				{
					num *= dinfo.Def.buildingDamageFactor;
					if (victim.def.passability == Traversability.Impassable)
					{
						num *= dinfo.Def.buildingDamageFactorImpassable;
					}
					else
					{
						num *= dinfo.Def.buildingDamageFactorPassable;
					}
					if (ModsConfig.BiotechActive && dinfo.Instigator != null && (dinfo.WeaponBodyPartGroup != null || (dinfo.Weapon != null && dinfo.Weapon.IsMeleeWeapon)) && victim.def.IsDoor)
					{
						num *= dinfo.Instigator.GetStatValue(StatDefOf.MeleeDoorDamageFactor, true, -1);
					}
				}
				if (victim.def.category == ThingCategory.Plant)
				{
					num *= dinfo.Def.plantDamageFactor;
				}
				else if (victim.def.IsCorpse)
				{
					num *= dinfo.Def.corpseDamageFactor;
				}
				damageResult.totalDamageDealt = (float)Mathf.Min(victim.HitPoints, GenMath.RoundRandom(num));
				victim.HitPoints -= Mathf.RoundToInt(damageResult.totalDamageDealt);
				if (victim.HitPoints <= 0)
				{
					victim.HitPoints = 0;
					victim.Kill(new DamageInfo?(dinfo), null);
				}
			}
			return damageResult;
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00020D18 File Offset: 0x0001EF18
		public virtual void ExplosionStart(Explosion explosion, List<IntVec3> cellsToAffect)
		{
			if (this.def.explosionHeatEnergyPerCell > 1E-45f)
			{
				GenTemperature.PushHeat(explosion.Position, explosion.Map, this.def.explosionHeatEnergyPerCell * (float)cellsToAffect.Count);
			}
			if (explosion.doVisualEffects)
			{
				FleckMaker.Static(explosion.Position, explosion.Map, FleckDefOf.ExplosionFlash, explosion.radius * 6f);
				if (explosion.Map == Find.CurrentMap)
				{
					float magnitude = (explosion.Position.ToVector3Shifted() - Find.Camera.transform.position).magnitude;
					Find.CameraDriver.shaker.DoShake(4f * explosion.radius * explosion.screenShakeFactor / magnitude);
				}
				this.ExplosionVisualEffectCenter(explosion);
			}
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00020DEC File Offset: 0x0001EFEC
		protected virtual void ExplosionVisualEffectCenter(Explosion explosion)
		{
			for (int i = 0; i < 4; i++)
			{
				FleckMaker.ThrowSmoke(explosion.Position.ToVector3Shifted() + Gen.RandomHorizontalVector(explosion.radius * 0.7f), explosion.Map, explosion.radius * 0.6f);
			}
			if (this.def.explosionCenterFleck != null)
			{
				FleckMaker.Static(explosion.Position.ToVector3Shifted(), explosion.Map, this.def.explosionCenterFleck, 1f);
			}
			else if (this.def.explosionCenterMote != null)
			{
				MoteMaker.MakeStaticMote(explosion.Position.ToVector3Shifted(), explosion.Map, this.def.explosionCenterMote, 1f, false);
			}
			if (this.def.explosionCenterEffecter != null)
			{
				this.def.explosionCenterEffecter.Spawn(explosion.Position, explosion.Map, Vector3.zero, 1f);
			}
			if (this.def.explosionInteriorMote != null || this.def.explosionInteriorFleck != null || this.def.explosionInteriorEffecter != null)
			{
				int num = Mathf.RoundToInt(3.1415927f * explosion.radius * explosion.radius / 6f * this.def.explosionInteriorCellCountMultiplier);
				for (int j = 0; j < num; j++)
				{
					Vector3 b = Gen.RandomHorizontalVector(explosion.radius * this.def.explosionInteriorCellDistanceMultiplier);
					if (this.def.explosionInteriorEffecter != null)
					{
						Vector3 vect = explosion.Position.ToVector3Shifted() + b;
						this.def.explosionInteriorEffecter.Spawn(explosion.Position, vect.ToIntVec3(), explosion.Map, 1f);
					}
					else if (this.def.explosionInteriorFleck != null)
					{
						FleckMaker.ThrowExplosionInterior(explosion.Position.ToVector3Shifted() + b, explosion.Map, this.def.explosionInteriorFleck);
					}
					else
					{
						MoteMaker.ThrowExplosionInteriorMote(explosion.Position.ToVector3Shifted() + b, explosion.Map, this.def.explosionInteriorMote);
					}
				}
			}
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00021018 File Offset: 0x0001F218
		public virtual void ExplosionAffectCell(Explosion explosion, IntVec3 c, List<Thing> damagedThings, List<Thing> ignoredThings, bool canThrowMotes)
		{
			if ((this.def.explosionCellMote != null || this.def.explosionCellFleck != null) && canThrowMotes)
			{
				float t = Mathf.Clamp01((explosion.Position - c).LengthHorizontal / explosion.radius);
				Color color = Color.Lerp(this.def.explosionColorCenter, this.def.explosionColorEdge, t);
				if (this.def.explosionCellMote != null)
				{
					Mote mote = c.GetFirstThing(explosion.Map, this.def.explosionCellMote) as Mote;
					if (mote != null)
					{
						mote.spawnTick = Find.TickManager.TicksGame;
					}
					else
					{
						MoteMaker.ThrowExplosionCell(c, explosion.Map, this.def.explosionCellMote, color);
					}
				}
				else
				{
					FleckMaker.ThrowExplosionCell(c, explosion.Map, this.def.explosionCellFleck, color);
				}
			}
			if (this.def.explosionCellEffecter != null && (this.def.explosionCellEffecterMaxRadius < 1E-45f || c.InHorDistOf(explosion.Position, this.def.explosionCellEffecterMaxRadius)) && Rand.Chance(this.def.explosionCellEffecterChance))
			{
				this.def.explosionCellEffecter.Spawn(explosion.Position, c, explosion.Map, 1f);
			}
			DamageWorker.thingsToAffect.Clear();
			float num = float.MinValue;
			bool flag = false;
			List<Thing> list = explosion.Map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (thing.def.category != ThingCategory.Mote && thing.def.category != ThingCategory.Ethereal)
				{
					DamageWorker.thingsToAffect.Add(thing);
					if (thing.def.Fillage == FillCategory.Full && thing.def.Altitude > num)
					{
						flag = true;
						num = thing.def.Altitude;
					}
				}
			}
			for (int j = 0; j < DamageWorker.thingsToAffect.Count; j++)
			{
				if (DamageWorker.thingsToAffect[j].def.Altitude >= num)
				{
					this.ExplosionDamageThing(explosion, DamageWorker.thingsToAffect[j], damagedThings, ignoredThings, c);
				}
			}
			if (!flag)
			{
				this.ExplosionDamageTerrain(explosion, c);
			}
			if (this.def.explosionSnowMeltAmount > 0.0001f)
			{
				float lengthHorizontal = (c - explosion.Position).LengthHorizontal;
				float num2 = 1f - lengthHorizontal / explosion.radius;
				if (num2 > 0f)
				{
					explosion.Map.snowGrid.AddDepth(c, -num2 * this.def.explosionSnowMeltAmount);
				}
			}
			if (this.def == DamageDefOf.Bomb || this.def == DamageDefOf.Flame)
			{
				List<Thing> list2 = explosion.Map.listerThings.ThingsOfDef(ThingDefOf.RectTrigger);
				for (int k = 0; k < list2.Count; k++)
				{
					RectTrigger rectTrigger = (RectTrigger)list2[k];
					if (rectTrigger.activateOnExplosion && rectTrigger.Rect.Contains(c))
					{
						rectTrigger.ActivatedBy(null);
					}
				}
			}
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00021340 File Offset: 0x0001F540
		protected virtual void ExplosionDamageThing(Explosion explosion, Thing t, List<Thing> damagedThings, List<Thing> ignoredThings, IntVec3 cell)
		{
			if (t.def.category == ThingCategory.Mote || t.def.category == ThingCategory.Ethereal)
			{
				return;
			}
			if (damagedThings.Contains(t))
			{
				return;
			}
			damagedThings.Add(t);
			if (ignoredThings != null && ignoredThings.Contains(t))
			{
				return;
			}
			if (this.def == DamageDefOf.Bomb && t.def == ThingDefOf.Fire && !t.Destroyed)
			{
				t.Destroy(DestroyMode.Vanish);
				return;
			}
			float angle;
			if (t.Position == explosion.Position)
			{
				angle = (float)Rand.RangeInclusive(0, 359);
			}
			else
			{
				angle = (t.Position - explosion.Position).AngleFlat;
			}
			DamageInfo dinfo = new DamageInfo(this.def, (float)explosion.GetDamageAmountAt(cell), explosion.GetArmorPenetrationAt(cell), angle, explosion.instigator, null, explosion.weapon, DamageInfo.SourceCategory.ThingOrUnknown, explosion.intendedTarget, true, true);
			if (this.def.explosionAffectOutsidePartsOnly)
			{
				dinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
			}
			BattleLogEntry_ExplosionImpact battleLogEntry_ExplosionImpact = null;
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				battleLogEntry_ExplosionImpact = new BattleLogEntry_ExplosionImpact(explosion.instigator, t, explosion.weapon, explosion.projectile, this.def);
				Find.BattleLog.Add(battleLogEntry_ExplosionImpact);
			}
			DamageWorker.DamageResult damageResult = t.TakeDamage(dinfo);
			damageResult.AssociateWithLog(battleLogEntry_ExplosionImpact);
			if (pawn != null && damageResult.wounded && pawn.stances != null)
			{
				pawn.stances.stagger.StaggerFor(95, 0.17f);
			}
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x000214B4 File Offset: 0x0001F6B4
		protected virtual void ExplosionDamageTerrain(Explosion explosion, IntVec3 c)
		{
			if (this.def != DamageDefOf.Bomb)
			{
				return;
			}
			if (!explosion.Map.terrainGrid.CanRemoveTopLayerAt(c))
			{
				return;
			}
			TerrainDef terrain = c.GetTerrain(explosion.Map);
			if (terrain.destroyOnBombDamageThreshold < 0f)
			{
				return;
			}
			if ((float)explosion.GetDamageAmountAt(c) >= terrain.destroyOnBombDamageThreshold)
			{
				explosion.Map.terrainGrid.Notify_TerrainDestroyed(c);
			}
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x0002151F File Offset: 0x0001F71F
		public IEnumerable<IntVec3> ExplosionCellsToHit(Explosion explosion)
		{
			return this.ExplosionCellsToHit(explosion.Position, explosion.Map, explosion.radius, explosion.needLOSToCell1, explosion.needLOSToCell2, explosion.affectedAngle);
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x0002154C File Offset: 0x0001F74C
		public virtual IEnumerable<IntVec3> ExplosionCellsToHit(IntVec3 center, Map map, float radius, IntVec3? needLOSToCell1 = null, IntVec3? needLOSToCell2 = null, FloatRange? affectedAngle = null)
		{
			DamageWorker.openCells.Clear();
			DamageWorker.adjWallCells.Clear();
			float num = (affectedAngle != null) ? affectedAngle.GetValueOrDefault().min : 0f;
			float num2 = (affectedAngle != null) ? affectedAngle.GetValueOrDefault().max : 0f;
			int num3 = GenRadial.NumCellsInRadius(radius);
			for (int i = 0; i < num3; i++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map) && GenSight.LineOfSight(center, intVec, map, true, null, 0, 0))
				{
					if (affectedAngle != null)
					{
						float lengthHorizontal = (intVec - center).LengthHorizontal;
						float num4 = lengthHorizontal / radius;
						if (lengthHorizontal <= 0.5f)
						{
							goto IL_173;
						}
						float num5 = Mathf.Atan2((float)(-(float)(intVec.z - center.z)), (float)(intVec.x - center.x)) * 57.29578f;
						float num6 = num;
						float num7 = num2;
						if (num5 - num6 < -0.5f * num4 || num5 - num7 > 0.5f * num4)
						{
							goto IL_173;
						}
					}
					if (needLOSToCell1 != null || needLOSToCell2 != null)
					{
						bool flag = needLOSToCell1 != null && GenSight.LineOfSight(needLOSToCell1.Value, intVec, map, false, null, 0, 0);
						bool flag2 = needLOSToCell2 != null && GenSight.LineOfSight(needLOSToCell2.Value, intVec, map, false, null, 0, 0);
						if (!flag && !flag2)
						{
							goto IL_173;
						}
					}
					DamageWorker.openCells.Add(intVec);
				}
				IL_173:;
			}
			for (int j = 0; j < DamageWorker.openCells.Count; j++)
			{
				IntVec3 intVec2 = DamageWorker.openCells[j];
				if (intVec2.Walkable(map))
				{
					for (int k = 0; k < 4; k++)
					{
						IntVec3 intVec3 = intVec2 + GenAdj.CardinalDirections[k];
						if (intVec3.InHorDistOf(center, radius) && intVec3.InBounds(map) && !intVec3.Standable(map) && intVec3.GetEdifice(map) != null && !DamageWorker.openCells.Contains(intVec3) && !DamageWorker.adjWallCells.Contains(intVec3))
						{
							DamageWorker.adjWallCells.Add(intVec3);
						}
					}
				}
			}
			return DamageWorker.openCells.Concat(DamageWorker.adjWallCells);
		}

		// Token: 0x0400039C RID: 924
		public DamageDef def;

		// Token: 0x0400039D RID: 925
		private const float ExplosionCamShakeMultiplier = 4f;

		// Token: 0x0400039E RID: 926
		private static List<Thing> thingsToAffect = new List<Thing>();

		// Token: 0x0400039F RID: 927
		private static List<IntVec3> openCells = new List<IntVec3>();

		// Token: 0x040003A0 RID: 928
		private static List<IntVec3> adjWallCells = new List<IntVec3>();

		// Token: 0x02001CBE RID: 7358
		public class DamageResult
		{
			// Token: 0x17001DB6 RID: 7606
			// (get) Token: 0x0600B0BF RID: 45247 RVA: 0x0040160F File Offset: 0x003FF80F
			public BodyPartRecord LastHitPart
			{
				get
				{
					if (this.parts == null)
					{
						return null;
					}
					if (this.parts.Count <= 0)
					{
						return null;
					}
					return this.parts[this.parts.Count - 1];
				}
			}

			// Token: 0x0600B0C0 RID: 45248 RVA: 0x00401644 File Offset: 0x003FF844
			public void AddPart(Thing hitThing, BodyPartRecord part)
			{
				if (this.hitThing != null && this.hitThing != hitThing)
				{
					Log.ErrorOnce("Single damage worker referring to multiple things; will cause issues with combat log", 30667935);
				}
				this.hitThing = hitThing;
				if (this.parts == null)
				{
					this.parts = new List<BodyPartRecord>();
				}
				this.parts.Add(part);
			}

			// Token: 0x0600B0C1 RID: 45249 RVA: 0x00401697 File Offset: 0x003FF897
			public void AddHediff(Hediff hediff)
			{
				if (this.hediffs == null)
				{
					this.hediffs = new List<Hediff>();
				}
				this.hediffs.Add(hediff);
			}

			// Token: 0x0600B0C2 RID: 45250 RVA: 0x004016B8 File Offset: 0x003FF8B8
			public void AssociateWithLog(LogEntry_DamageResult log)
			{
				if (log == null)
				{
					return;
				}
				Pawn hitPawn = this.hitThing as Pawn;
				if (hitPawn != null)
				{
					List<BodyPartRecord> list = null;
					List<bool> recipientPartsDestroyed = null;
					if (!this.parts.NullOrEmpty<BodyPartRecord>() && hitPawn != null)
					{
						list = this.parts.Distinct<BodyPartRecord>().ToList<BodyPartRecord>();
						recipientPartsDestroyed = (from part in list
						select hitPawn.health.hediffSet.GetPartHealth(part) <= 0f).ToList<bool>();
					}
					log.FillTargets(list, recipientPartsDestroyed, this.deflected);
				}
				if (this.hediffs != null)
				{
					for (int i = 0; i < this.hediffs.Count; i++)
					{
						this.hediffs[i].combatLogEntry = new WeakReference<LogEntry>(log);
						this.hediffs[i].combatLogText = log.ToGameStringFromPOV(null, false);
					}
				}
			}

			// Token: 0x04007142 RID: 28994
			public bool wounded;

			// Token: 0x04007143 RID: 28995
			public bool headshot;

			// Token: 0x04007144 RID: 28996
			public bool deflected;

			// Token: 0x04007145 RID: 28997
			public bool stunned;

			// Token: 0x04007146 RID: 28998
			public bool deflectedByMetalArmor;

			// Token: 0x04007147 RID: 28999
			public bool diminished;

			// Token: 0x04007148 RID: 29000
			public bool diminishedByMetalArmor;

			// Token: 0x04007149 RID: 29001
			public Thing hitThing;

			// Token: 0x0400714A RID: 29002
			public List<BodyPartRecord> parts;

			// Token: 0x0400714B RID: 29003
			public List<Hediff> hediffs;

			// Token: 0x0400714C RID: 29004
			public float totalDamageDealt;
		}
	}
}
