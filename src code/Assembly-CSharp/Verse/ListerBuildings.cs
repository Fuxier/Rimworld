using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x020001EE RID: 494
	public sealed class ListerBuildings
	{
		// Token: 0x06000E3D RID: 3645 RVA: 0x0004E294 File Offset: 0x0004C494
		public void Add(Building b)
		{
			if (b.def.building != null && b.def.building.isNaturalRock)
			{
				return;
			}
			if (b.Faction == Faction.OfPlayer)
			{
				this.allBuildingsColonist.Add(b);
				if (b is IAttackTarget)
				{
					this.allBuildingsColonistCombatTargets.Add(b);
				}
			}
			else
			{
				this.allBuildingsNonColonist.Add(b);
			}
			CompProperties_Power compProperties = b.def.GetCompProperties<CompProperties_Power>();
			if (compProperties != null && compProperties.shortCircuitInRain)
			{
				this.allBuildingsColonistElecFire.Add(b);
			}
			if (b.TryGetComp<CompAnimalPenMarker>() != null)
			{
				this.allBuildingsAnimalPenMarkers.Add(b);
			}
			if (b.def == ThingDefOf.CaravanPackingSpot)
			{
				this.allBuildingsHitchingPosts.Add(b);
			}
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x0004E350 File Offset: 0x0004C550
		public void Remove(Building b)
		{
			this.allBuildingsColonist.Remove(b);
			this.allBuildingsNonColonist.Remove(b);
			if (b is IAttackTarget)
			{
				this.allBuildingsColonistCombatTargets.Remove(b);
			}
			CompProperties_Power compProperties = b.def.GetCompProperties<CompProperties_Power>();
			if (compProperties != null && compProperties.shortCircuitInRain)
			{
				this.allBuildingsColonistElecFire.Remove(b);
			}
			this.allBuildingsAnimalPenMarkers.Remove(b);
			this.allBuildingsHitchingPosts.Remove(b);
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x0004E3CA File Offset: 0x0004C5CA
		public void RegisterInstallBlueprint(Blueprint_Install blueprint)
		{
			this.reinstallationMap.Add(blueprint.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified(), blueprint);
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x0004E3E4 File Offset: 0x0004C5E4
		public void DeregisterInstallBlueprint(Blueprint_Install blueprint)
		{
			Thing miniToInstallOrBuildingToReinstall = blueprint.MiniToInstallOrBuildingToReinstall;
			Thing thing = (miniToInstallOrBuildingToReinstall != null) ? miniToInstallOrBuildingToReinstall.GetInnerIfMinified() : null;
			if (thing != null)
			{
				this.reinstallationMap.Remove(thing);
				return;
			}
			Thing thing2 = null;
			foreach (KeyValuePair<Thing, Blueprint_Install> keyValuePair in this.reinstallationMap)
			{
				if (keyValuePair.Value == blueprint)
				{
					thing2 = keyValuePair.Key;
					break;
				}
			}
			if (thing2 != null)
			{
				this.reinstallationMap.Remove(thing2);
			}
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x0004E47C File Offset: 0x0004C67C
		public bool ColonistsHaveBuilding(ThingDef def)
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (this.allBuildingsColonist[i].def == def)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x0004E4B8 File Offset: 0x0004C6B8
		public bool ColonistsHaveBuilding(Func<Thing, bool> predicate)
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (predicate(this.allBuildingsColonist[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x0004E4F4 File Offset: 0x0004C6F4
		public bool ColonistsHaveResearchBench()
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (this.allBuildingsColonist[i] is Building_ResearchBench)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x0004E530 File Offset: 0x0004C730
		public bool ColonistsHaveBuildingWithPowerOn(ThingDef def)
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (this.allBuildingsColonist[i].def == def)
				{
					CompPowerTrader compPowerTrader = this.allBuildingsColonist[i].TryGetComp<CompPowerTrader>();
					if (compPowerTrader == null || compPowerTrader.PowerOn)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x0004E587 File Offset: 0x0004C787
		public IEnumerable<Building> AllBuildingsColonistOfDef(ThingDef def)
		{
			int num;
			for (int i = 0; i < this.allBuildingsColonist.Count; i = num + 1)
			{
				if (this.allBuildingsColonist[i].def == def)
				{
					yield return this.allBuildingsColonist[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x0004E59E File Offset: 0x0004C79E
		public IEnumerable<Building> AllBuildingsNonColonistOfDef(ThingDef def)
		{
			int num;
			for (int i = 0; i < this.allBuildingsNonColonist.Count; i = num + 1)
			{
				if (this.allBuildingsNonColonist[i].def == def)
				{
					yield return this.allBuildingsNonColonist[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x0004E5B5 File Offset: 0x0004C7B5
		public bool TryGetReinstallBlueprint(Thing building, out Blueprint_Install bp)
		{
			return this.reinstallationMap.TryGetValue(building, out bp);
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x0004E5C4 File Offset: 0x0004C7C4
		public IEnumerable<T> AllBuildingsColonistOfClass<T>() where T : Building
		{
			int num;
			for (int i = 0; i < this.allBuildingsColonist.Count; i = num + 1)
			{
				T t = this.allBuildingsColonist[i] as T;
				if (t != null)
				{
					yield return t;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x0004E5D4 File Offset: 0x0004C7D4
		public void Notify_FactionRemoved(Faction faction)
		{
			for (int i = 0; i < this.allBuildingsNonColonist.Count; i++)
			{
				if (this.allBuildingsNonColonist[i].Faction == faction)
				{
					this.allBuildingsNonColonist[i].SetFaction(null, null);
				}
			}
		}

		// Token: 0x04000C70 RID: 3184
		public List<Building> allBuildingsColonist = new List<Building>();

		// Token: 0x04000C71 RID: 3185
		public List<Building> allBuildingsNonColonist = new List<Building>();

		// Token: 0x04000C72 RID: 3186
		public HashSet<Building> allBuildingsColonistCombatTargets = new HashSet<Building>();

		// Token: 0x04000C73 RID: 3187
		public HashSet<Building> allBuildingsColonistElecFire = new HashSet<Building>();

		// Token: 0x04000C74 RID: 3188
		public HashSet<Building> allBuildingsAnimalPenMarkers = new HashSet<Building>();

		// Token: 0x04000C75 RID: 3189
		public HashSet<Building> allBuildingsHitchingPosts = new HashSet<Building>();

		// Token: 0x04000C76 RID: 3190
		private Dictionary<Thing, Blueprint_Install> reinstallationMap = new Dictionary<Thing, Blueprint_Install>();
	}
}
