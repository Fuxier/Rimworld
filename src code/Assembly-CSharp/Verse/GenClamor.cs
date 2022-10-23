using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200055D RID: 1373
	public static class GenClamor
	{
		// Token: 0x06002A21 RID: 10785 RVA: 0x0010C9D7 File Offset: 0x0010ABD7
		public static void DoClamor(Thing source, float radius, ClamorDef type)
		{
			GenClamor.DoClamor(source, source.Position, radius, type);
		}

		// Token: 0x06002A22 RID: 10786 RVA: 0x0010C9E8 File Offset: 0x0010ABE8
		public static void DoClamor(Thing source, IntVec3 position, float radius, ClamorDef type)
		{
			GenClamor.DoClamor(source, position, radius, delegate(Thing _, Pawn hearer)
			{
				hearer.HearClamor(source, type);
			});
		}

		// Token: 0x06002A23 RID: 10787 RVA: 0x0010CA22 File Offset: 0x0010AC22
		public static void DoClamor(Thing source, float radius, GenClamor.ClamorEffect clamorEffect)
		{
			GenClamor.DoClamor(source, source.Position, radius, clamorEffect);
		}

		// Token: 0x06002A24 RID: 10788 RVA: 0x0010CA34 File Offset: 0x0010AC34
		public static void DoClamor(Thing source, IntVec3 position, float radius, GenClamor.ClamorEffect clamorEffect)
		{
			if (source.MapHeld == null)
			{
				return;
			}
			Region region = position.GetRegion(source.MapHeld, RegionType.Set_Passable);
			if (region == null)
			{
				return;
			}
			RegionTraverser.BreadthFirstTraverse(region, (Region from, Region r) => r.door == null || r.door.Open, delegate(Region r)
			{
				List<Thing> list = r.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn);
				for (int i = 0; i < list.Count; i++)
				{
					Pawn pawn = list[i] as Pawn;
					float num = Mathf.Clamp01(pawn.health.capacities.GetLevel(PawnCapacityDefOf.Hearing));
					if (num > 0f && pawn.Position.InHorDistOf(position, radius * num))
					{
						clamorEffect(source, pawn);
					}
				}
				return false;
			}, 15, RegionType.Set_Passable);
		}

		// Token: 0x0200211F RID: 8479
		// (Invoke) Token: 0x0600C607 RID: 50695
		public delegate void ClamorEffect(Thing source, Pawn hearer);
	}
}
