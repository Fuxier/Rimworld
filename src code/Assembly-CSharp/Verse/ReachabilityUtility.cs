using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000238 RID: 568
	public static class ReachabilityUtility
	{
		// Token: 0x06001010 RID: 4112 RVA: 0x0005DA78 File Offset: 0x0005BC78
		public static bool CanReach(this Pawn pawn, LocalTargetInfo dest, PathEndMode peMode, Danger maxDanger, bool canBashDoors = false, bool canBashFences = false, TraverseMode mode = TraverseMode.ByPawn)
		{
			return pawn.Spawned && pawn.Map.reachability.CanReach(pawn.Position, dest, peMode, TraverseParms.For(pawn, maxDanger, mode, canBashDoors, false, canBashFences));
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0005DAB8 File Offset: 0x0005BCB8
		public static bool CanReachNonLocal(this Pawn pawn, TargetInfo dest, PathEndMode peMode, Danger maxDanger, bool canBashDoors = false, TraverseMode mode = TraverseMode.ByPawn)
		{
			return pawn.Spawned && pawn.Map.reachability.CanReachNonLocal(pawn.Position, dest, peMode, TraverseParms.For(pawn, maxDanger, mode, canBashDoors, false, false));
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x0005DAF4 File Offset: 0x0005BCF4
		public static bool CanReachMapEdge(this Pawn p)
		{
			return p.Spawned && p.Map.reachability.CanReachMapEdge(p.Position, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false, false, false));
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x0005DB24 File Offset: 0x0005BD24
		public static void ClearCache()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].reachability.ClearCache();
			}
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x0005DB5C File Offset: 0x0005BD5C
		public static void ClearCacheFor(Pawn p)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].reachability.ClearCacheFor(p);
			}
		}
	}
}
