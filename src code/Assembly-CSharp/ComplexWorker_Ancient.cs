using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse;

// Token: 0x0200000C RID: 12
public class ComplexWorker_Ancient : ComplexWorker
{
	// Token: 0x06000030 RID: 48 RVA: 0x00003828 File Offset: 0x00001A28
	public override Faction GetFixedHostileFactionForThreats()
	{
		if (!Rand.Chance(this.def.fixedHostileFactionChance))
		{
			return null;
		}
		if (Faction.OfInsects != null && Faction.OfMechanoids != null)
		{
			if (!Rand.Bool)
			{
				return Faction.OfMechanoids;
			}
			return Faction.OfInsects;
		}
		else
		{
			if (Faction.OfInsects != null)
			{
				return Faction.OfInsects;
			}
			return Faction.OfMechanoids;
		}
	}

	// Token: 0x06000031 RID: 49 RVA: 0x0000387C File Offset: 0x00001A7C
	protected override void PostSpawnStructure(List<List<CellRect>> rooms, Map map, List<Thing> allSpawnedThings)
	{
		ComplexWorker_Ancient.<>c__DisplayClass1_0 CS$<>8__locals1;
		CS$<>8__locals1.map = map;
		if (ModsConfig.IdeologyActive)
		{
			if (this.def.roomRewardCrateFactor > 0f)
			{
				int num = 0;
				for (int i = 0; i < allSpawnedThings.Count; i++)
				{
					if (allSpawnedThings[i] is Building_Crate)
					{
						num++;
					}
				}
				int num2 = Mathf.RoundToInt((float)rooms.Count * this.def.roomRewardCrateFactor) - num;
				if (num2 <= 0)
				{
					return;
				}
				ThingSetMakerDef thingSetMakerDef = this.def.rewardThingSetMakerDef ?? ThingSetMakerDefOf.Reward_ItemsStandard;
				foreach (IEnumerable<CellRect> source in rooms.InRandomOrder(null))
				{
					bool flag = true;
					IEnumerable<IntVec3> enumerable = source.SelectMany((CellRect r) => r.Cells);
					foreach (IntVec3 c in enumerable)
					{
						Building edifice = c.GetEdifice(CS$<>8__locals1.map);
						if (edifice != null && edifice is Building_Crate)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						IntVec3 loc;
						if (ComplexUtility.TryFindRandomSpawnCell(ThingDefOf.AncientHermeticCrate, enumerable, CS$<>8__locals1.map, out loc, 1, new Rot4?(Rot4.South)))
						{
							Building_Crate building_Crate = (Building_Crate)GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.AncientHermeticCrate, null), loc, CS$<>8__locals1.map, Rot4.South, WipeMode.Vanish, false);
							List<Thing> list = thingSetMakerDef.root.Generate(default(ThingSetMakerParams));
							for (int j = list.Count - 1; j >= 0; j--)
							{
								Thing thing = list[j];
								if (!building_Crate.TryAcceptThing(thing, false))
								{
									thing.Destroy(DestroyMode.Vanish);
								}
							}
							num2--;
						}
						if (num2 <= 0)
						{
							break;
						}
					}
				}
			}
			using (IEnumerator<List<CellRect>> enumerator = rooms.InRandomOrder(null).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					foreach (IntVec3 intVec in enumerator.Current.SelectMany((CellRect r) => r.Cells).InRandomOrder(null))
					{
						if (ComplexWorker_Ancient.<PostSpawnStructure>g__CanPlaceCommsConsoleAt|1_0(intVec, ref CS$<>8__locals1))
						{
							GenSpawn.Spawn(ThingDefOf.AncientCommsConsole, intVec, CS$<>8__locals1.map, WipeMode.Vanish);
							return;
						}
					}
				}
			}
		}
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00003B6C File Offset: 0x00001D6C
	[CompilerGenerated]
	internal static bool <PostSpawnStructure>g__CanPlaceCommsConsoleAt|1_0(IntVec3 cell, ref ComplexWorker_Ancient.<>c__DisplayClass1_0 A_1)
	{
		using (CellRect.Enumerator enumerator = GenAdj.OccupiedRect(cell, Rot4.North, ThingDefOf.AncientCommsConsole.Size).ExpandedBy(1).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetEdifice(A_1.map) != null)
				{
					return false;
				}
			}
		}
		return true;
	}
}
