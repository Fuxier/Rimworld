using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020003C6 RID: 966
	public class Explosion : Thing
	{
		// Token: 0x06001B95 RID: 7061 RVA: 0x000A969C File Offset: 0x000A789C
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.cellsToAffect = SimplePool<List<IntVec3>>.Get();
				this.cellsToAffect.Clear();
				this.damagedThings = SimplePool<List<Thing>>.Get();
				this.damagedThings.Clear();
				this.addedCellsAffectedOnlyByDamage = SimplePool<HashSet<IntVec3>>.Get();
				this.addedCellsAffectedOnlyByDamage.Clear();
			}
		}

		// Token: 0x06001B96 RID: 7062 RVA: 0x000A96F8 File Offset: 0x000A78F8
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			this.cellsToAffect.Clear();
			SimplePool<List<IntVec3>>.Return(this.cellsToAffect);
			this.cellsToAffect = null;
			this.damagedThings.Clear();
			SimplePool<List<Thing>>.Return(this.damagedThings);
			this.damagedThings = null;
			this.addedCellsAffectedOnlyByDamage.Clear();
			SimplePool<HashSet<IntVec3>>.Return(this.addedCellsAffectedOnlyByDamage);
			this.addedCellsAffectedOnlyByDamage = null;
		}

		// Token: 0x06001B97 RID: 7063 RVA: 0x000A9764 File Offset: 0x000A7964
		public virtual void StartExplosion(SoundDef explosionSound, List<Thing> ignoredThings)
		{
			if (!base.Spawned)
			{
				Log.Error("Called StartExplosion() on unspawned thing.");
				return;
			}
			this.startTick = Find.TickManager.TicksGame;
			this.ignoredThings = ignoredThings;
			this.cellsToAffect.Clear();
			this.damagedThings.Clear();
			this.addedCellsAffectedOnlyByDamage.Clear();
			this.cellsToAffect.AddRange(this.damType.Worker.ExplosionCellsToHit(this));
			if (this.applyDamageToExplosionCellsNeighbors)
			{
				this.AddCellsNeighbors(this.cellsToAffect);
			}
			this.damType.Worker.ExplosionStart(this, this.cellsToAffect);
			this.PlayExplosionSound(explosionSound);
			FleckMaker.WaterSplash(base.Position.ToVector3Shifted(), base.Map, this.radius * 6f, 20f);
			this.cellsToAffect.Sort((IntVec3 a, IntVec3 b) => this.GetCellAffectTick(b).CompareTo(this.GetCellAffectTick(a)));
			RegionTraverser.BreadthFirstTraverse(base.Position, base.Map, (Region from, Region to) => true, delegate(Region x)
			{
				List<Thing> allThings = x.ListerThings.AllThings;
				for (int i = allThings.Count - 1; i >= 0; i--)
				{
					if (allThings[i].Spawned)
					{
						allThings[i].Notify_Explosion(this);
					}
				}
				return false;
			}, 25, RegionType.Set_Passable);
		}

		// Token: 0x06001B98 RID: 7064 RVA: 0x000A988C File Offset: 0x000A7A8C
		public override void Tick()
		{
			int ticksGame = Find.TickManager.TicksGame;
			int num = this.cellsToAffect.Count - 1;
			while (num >= 0 && ticksGame >= this.GetCellAffectTick(this.cellsToAffect[num]))
			{
				try
				{
					this.AffectCell(this.cellsToAffect[num]);
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Explosion could not affect cell ",
						this.cellsToAffect[num],
						": ",
						ex
					}));
				}
				this.cellsToAffect.RemoveAt(num);
				num--;
			}
			if (!this.cellsToAffect.Any<IntVec3>())
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06001B99 RID: 7065 RVA: 0x000A9950 File Offset: 0x000A7B50
		public int GetDamageAmountAt(IntVec3 c)
		{
			if (!this.damageFalloff)
			{
				return this.damAmount;
			}
			float t = c.DistanceTo(base.Position) / this.radius;
			return Mathf.Max(GenMath.RoundRandom(Mathf.Lerp((float)this.damAmount, (float)this.damAmount * 0.2f, t)), 1);
		}

		// Token: 0x06001B9A RID: 7066 RVA: 0x000A99A8 File Offset: 0x000A7BA8
		public float GetArmorPenetrationAt(IntVec3 c)
		{
			if (!this.damageFalloff)
			{
				return this.armorPenetration;
			}
			float t = c.DistanceTo(base.Position) / this.radius;
			return Mathf.Lerp(this.armorPenetration, this.armorPenetration * 0.2f, t);
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x000A99F0 File Offset: 0x000A7BF0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
			Scribe_Defs.Look<DamageDef>(ref this.damType, "damType");
			Scribe_Values.Look<int>(ref this.damAmount, "damAmount", 0, false);
			Scribe_Values.Look<float>(ref this.armorPenetration, "armorPenetration", 0f, false);
			Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
			Scribe_Defs.Look<ThingDef>(ref this.weapon, "weapon");
			Scribe_Defs.Look<ThingDef>(ref this.projectile, "projectile");
			Scribe_References.Look<Thing>(ref this.intendedTarget, "intendedTarget", false);
			Scribe_Values.Look<bool>(ref this.applyDamageToExplosionCellsNeighbors, "applyDamageToExplosionCellsNeighbors", false, false);
			Scribe_Defs.Look<ThingDef>(ref this.preExplosionSpawnThingDef, "preExplosionSpawnThingDef");
			Scribe_Values.Look<float>(ref this.preExplosionSpawnChance, "preExplosionSpawnChance", 0f, false);
			Scribe_Values.Look<int>(ref this.preExplosionSpawnThingCount, "preExplosionSpawnThingCount", 1, false);
			Scribe_Defs.Look<ThingDef>(ref this.postExplosionSpawnThingDef, "postExplosionSpawnThingDef");
			Scribe_Defs.Look<ThingDef>(ref this.postExplosionSpawnThingDefWater, "postExplosionSpawnThingDefWater");
			Scribe_Values.Look<float>(ref this.postExplosionSpawnChance, "postExplosionSpawnChance", 0f, false);
			Scribe_Values.Look<int>(ref this.postExplosionSpawnThingCount, "postExplosionSpawnThingCount", 1, false);
			Scribe_Values.Look<GasType?>(ref this.postExplosionGasType, "postExplosionGasType", null, false);
			Scribe_Values.Look<float>(ref this.chanceToStartFire, "chanceToStartFire", 0f, false);
			Scribe_Values.Look<bool>(ref this.damageFalloff, "dealMoreDamageAtCenter", false, false);
			Scribe_Values.Look<IntVec3?>(ref this.needLOSToCell1, "needLOSToCell1", null, false);
			Scribe_Values.Look<IntVec3?>(ref this.needLOSToCell2, "needLOSToCell2", null, false);
			Scribe_Values.Look<FloatRange?>(ref this.affectedAngle, "affectedAngle", null, false);
			Scribe_Values.Look<float>(ref this.propagationSpeed, "propagationSpeed", 0f, false);
			Scribe_Values.Look<float>(ref this.excludeRadius, "canTargetLocations", 0f, false);
			Scribe_Values.Look<bool>(ref this.doSoundEffects, "doSoundEffects", true, false);
			Scribe_Values.Look<bool>(ref this.doVisualEffects, "doVisualEffects", true, false);
			Scribe_Values.Look<float>(ref this.screenShakeFactor, "screenShakeFactor", 1f, false);
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Collections.Look<IntVec3>(ref this.cellsToAffect, "cellsToAffect", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.damagedThings, "damagedThings", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.ignoredThings, "ignoredThings", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<IntVec3>(ref this.addedCellsAffectedOnlyByDamage, "addedCellsAffectedOnlyByDamage", LookMode.Value);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.damagedThings != null)
				{
					this.damagedThings.RemoveAll((Thing x) => x == null);
				}
				if (this.ignoredThings != null)
				{
					this.ignoredThings.RemoveAll((Thing x) => x == null);
				}
			}
		}

		// Token: 0x06001B9C RID: 7068 RVA: 0x000A9CF0 File Offset: 0x000A7EF0
		private int GetCellAffectTick(IntVec3 cell)
		{
			return this.startTick + (int)((cell - base.Position).LengthHorizontal * 1.5f / this.propagationSpeed);
		}

		// Token: 0x06001B9D RID: 7069 RVA: 0x000A9D28 File Offset: 0x000A7F28
		private void AffectCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return;
			}
			if (this.excludeRadius > 0f && (float)c.DistanceToSquared(base.Position) < this.excludeRadius * this.excludeRadius)
			{
				return;
			}
			bool flag = this.ShouldCellBeAffectedOnlyByDamage(c);
			if (!flag && Rand.Chance(this.preExplosionSpawnChance) && c.Walkable(base.Map))
			{
				this.TrySpawnExplosionThing(this.preExplosionSpawnThingDef, c, this.preExplosionSpawnThingCount);
			}
			this.damType.Worker.ExplosionAffectCell(this, c, this.damagedThings, this.ignoredThings, !flag);
			if (!flag)
			{
				if (Rand.Chance(this.postExplosionSpawnChance) && c.Walkable(base.Map))
				{
					ThingDef thingDef = c.GetTerrain(base.Map).IsWater ? (this.postExplosionSpawnThingDefWater ?? this.postExplosionSpawnThingDef) : this.postExplosionSpawnThingDef;
					this.TrySpawnExplosionThing(thingDef, c, this.postExplosionSpawnThingCount);
				}
				if (this.postExplosionGasType != null)
				{
					GasUtility.AddGas(c, base.Map, this.postExplosionGasType.Value, 255);
				}
			}
			float num = this.chanceToStartFire;
			if (this.damageFalloff)
			{
				num *= Mathf.Lerp(1f, 0.2f, c.DistanceTo(base.Position) / this.radius);
			}
			if (Rand.Chance(num))
			{
				FireUtility.TryStartFireIn(c, base.Map, Rand.Range(0.1f, 0.925f));
			}
		}

		// Token: 0x06001B9E RID: 7070 RVA: 0x000A9EA4 File Offset: 0x000A80A4
		private void TrySpawnExplosionThing(ThingDef thingDef, IntVec3 c, int count)
		{
			if (thingDef == null)
			{
				return;
			}
			if (thingDef.IsFilth)
			{
				FilthMaker.TryMakeFilth(c, base.Map, thingDef, count, FilthSourceFlags.None, true);
				return;
			}
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			thing.stackCount = count;
			GenSpawn.Spawn(thing, c, base.Map, WipeMode.Vanish);
			CompReleaseGas compReleaseGas = thing.TryGetComp<CompReleaseGas>();
			if (compReleaseGas != null)
			{
				compReleaseGas.StartRelease();
			}
		}

		// Token: 0x06001B9F RID: 7071 RVA: 0x000A9EFC File Offset: 0x000A80FC
		private void PlayExplosionSound(SoundDef explosionSound)
		{
			if (!this.doSoundEffects)
			{
				return;
			}
			bool flag;
			if (Prefs.DevMode)
			{
				flag = (explosionSound != null);
			}
			else
			{
				flag = !explosionSound.NullOrUndefined();
			}
			if (flag)
			{
				explosionSound.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				return;
			}
			this.damType.soundExplosion.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
		}

		// Token: 0x06001BA0 RID: 7072 RVA: 0x000A9F74 File Offset: 0x000A8174
		private void AddCellsNeighbors(List<IntVec3> cells)
		{
			Explosion.tmpCells.Clear();
			this.addedCellsAffectedOnlyByDamage.Clear();
			for (int i = 0; i < cells.Count; i++)
			{
				Explosion.tmpCells.Add(cells[i]);
			}
			for (int j = 0; j < cells.Count; j++)
			{
				if (cells[j].Walkable(base.Map))
				{
					for (int k = 0; k < GenAdj.AdjacentCells.Length; k++)
					{
						IntVec3 intVec = cells[j] + GenAdj.AdjacentCells[k];
						if (intVec.InBounds(base.Map) && Explosion.tmpCells.Add(intVec))
						{
							this.addedCellsAffectedOnlyByDamage.Add(intVec);
						}
					}
				}
			}
			cells.Clear();
			foreach (IntVec3 item in Explosion.tmpCells)
			{
				cells.Add(item);
			}
			Explosion.tmpCells.Clear();
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x000AA088 File Offset: 0x000A8288
		private bool ShouldCellBeAffectedOnlyByDamage(IntVec3 c)
		{
			return this.applyDamageToExplosionCellsNeighbors && this.addedCellsAffectedOnlyByDamage.Contains(c);
		}

		// Token: 0x040013DD RID: 5085
		public float radius;

		// Token: 0x040013DE RID: 5086
		public DamageDef damType;

		// Token: 0x040013DF RID: 5087
		public int damAmount;

		// Token: 0x040013E0 RID: 5088
		public float armorPenetration;

		// Token: 0x040013E1 RID: 5089
		public Thing instigator;

		// Token: 0x040013E2 RID: 5090
		public ThingDef weapon;

		// Token: 0x040013E3 RID: 5091
		public ThingDef projectile;

		// Token: 0x040013E4 RID: 5092
		public Thing intendedTarget;

		// Token: 0x040013E5 RID: 5093
		public bool applyDamageToExplosionCellsNeighbors;

		// Token: 0x040013E6 RID: 5094
		public FloatRange? affectedAngle;

		// Token: 0x040013E7 RID: 5095
		public ThingDef preExplosionSpawnThingDef;

		// Token: 0x040013E8 RID: 5096
		public float preExplosionSpawnChance;

		// Token: 0x040013E9 RID: 5097
		public int preExplosionSpawnThingCount = 1;

		// Token: 0x040013EA RID: 5098
		public ThingDef postExplosionSpawnThingDef;

		// Token: 0x040013EB RID: 5099
		public ThingDef postExplosionSpawnThingDefWater;

		// Token: 0x040013EC RID: 5100
		public float postExplosionSpawnChance;

		// Token: 0x040013ED RID: 5101
		public int postExplosionSpawnThingCount = 1;

		// Token: 0x040013EE RID: 5102
		public GasType? postExplosionGasType;

		// Token: 0x040013EF RID: 5103
		public float chanceToStartFire;

		// Token: 0x040013F0 RID: 5104
		public bool damageFalloff;

		// Token: 0x040013F1 RID: 5105
		public IntVec3? needLOSToCell1;

		// Token: 0x040013F2 RID: 5106
		public IntVec3? needLOSToCell2;

		// Token: 0x040013F3 RID: 5107
		public bool doVisualEffects = true;

		// Token: 0x040013F4 RID: 5108
		public float propagationSpeed = 1f;

		// Token: 0x040013F5 RID: 5109
		public float excludeRadius;

		// Token: 0x040013F6 RID: 5110
		public bool doSoundEffects = true;

		// Token: 0x040013F7 RID: 5111
		public float screenShakeFactor = 1f;

		// Token: 0x040013F8 RID: 5112
		private int startTick;

		// Token: 0x040013F9 RID: 5113
		private List<IntVec3> cellsToAffect;

		// Token: 0x040013FA RID: 5114
		private List<Thing> damagedThings;

		// Token: 0x040013FB RID: 5115
		private List<Thing> ignoredThings;

		// Token: 0x040013FC RID: 5116
		private HashSet<IntVec3> addedCellsAffectedOnlyByDamage;

		// Token: 0x040013FD RID: 5117
		private const float DamageFactorAtEdge = 0.2f;

		// Token: 0x040013FE RID: 5118
		private static HashSet<IntVec3> tmpCells = new HashSet<IntVec3>();
	}
}
