using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Verse
{
	// Token: 0x0200057B RID: 1403
	public static class LargestAreaFinder
	{
		// Token: 0x06002B0B RID: 11019 RVA: 0x00113410 File Offset: 0x00111610
		public static CellRect FindLargestRect(Map map, Predicate<IntVec3> predicate, int breakEarlyOn = -1)
		{
			LargestAreaFinder.<>c__DisplayClass3_0 CS$<>8__locals1;
			CS$<>8__locals1.breakEarlyOn = breakEarlyOn;
			if (LargestAreaFinder.visited == null)
			{
				LargestAreaFinder.visited = new BoolGrid(map);
			}
			LargestAreaFinder.visited.ClearAndResizeTo(map);
			Rand.PushState(map.uniqueID ^ 484111219);
			CS$<>8__locals1.largestRect = CellRect.Empty;
			for (int i = 0; i < 3; i++)
			{
				LargestAreaFinder.tmpProcessed.Clear();
				foreach (IntVec3 c in map.cellsInRandomOrder.GetAll().InRandomOrder(LargestAreaFinder.randomOrderWorkingList))
				{
					CellRect largestRect = LargestAreaFinder.FindLargestRectAt(c, map, LargestAreaFinder.tmpProcessed, predicate);
					if (largestRect.Area > CS$<>8__locals1.largestRect.Area)
					{
						CS$<>8__locals1.largestRect = largestRect;
						if (LargestAreaFinder.<FindLargestRect>g__ShouldBreakEarly|3_0(ref CS$<>8__locals1))
						{
							break;
						}
					}
				}
				if (LargestAreaFinder.<FindLargestRect>g__ShouldBreakEarly|3_0(ref CS$<>8__locals1))
				{
					break;
				}
			}
			Rand.PopState();
			return CS$<>8__locals1.largestRect;
		}

		// Token: 0x06002B0C RID: 11020 RVA: 0x0011350C File Offset: 0x0011170C
		private static CellRect FindLargestRectAt(IntVec3 c, Map map, HashSet<IntVec3> processed, Predicate<IntVec3> predicate)
		{
			if (processed.Contains(c) || !predicate(c))
			{
				return CellRect.Empty;
			}
			return LargestAreaFinder.ExpandRect(CellRect.SingleCell(c), map, processed, predicate, true);
		}

		// Token: 0x06002B0D RID: 11021 RVA: 0x00113538 File Offset: 0x00111738
		public static CellRect ExpandRect(CellRect rect, Map map, HashSet<IntVec3> processed, Predicate<IntVec3> predicate, bool keepSquare = false)
		{
			LargestAreaFinder.<>c__DisplayClass5_0 CS$<>8__locals1;
			CS$<>8__locals1.rect = rect;
			CS$<>8__locals1.processed = processed;
			CS$<>8__locals1.predicate = predicate;
			bool flag;
			do
			{
				flag = false;
				if (!keepSquare || CS$<>8__locals1.rect.Width <= CS$<>8__locals1.rect.Height)
				{
					if (CS$<>8__locals1.rect.maxX + 1 < map.Size.x && LargestAreaFinder.<ExpandRect>g__CanExpand|5_0(Rot4.East, ref CS$<>8__locals1))
					{
						CS$<>8__locals1.rect.maxX = CS$<>8__locals1.rect.maxX + 1;
						flag = true;
					}
					if (CS$<>8__locals1.rect.minX > 0 && LargestAreaFinder.<ExpandRect>g__CanExpand|5_0(Rot4.West, ref CS$<>8__locals1))
					{
						CS$<>8__locals1.rect.minX = CS$<>8__locals1.rect.minX - 1;
						flag = true;
					}
				}
				if (!keepSquare || CS$<>8__locals1.rect.Height <= CS$<>8__locals1.rect.Width)
				{
					if (CS$<>8__locals1.rect.maxZ + 1 < map.Size.z && LargestAreaFinder.<ExpandRect>g__CanExpand|5_0(Rot4.North, ref CS$<>8__locals1))
					{
						CS$<>8__locals1.rect.maxZ = CS$<>8__locals1.rect.maxZ + 1;
						flag = true;
					}
					if (CS$<>8__locals1.rect.minZ > 0 && LargestAreaFinder.<ExpandRect>g__CanExpand|5_0(Rot4.South, ref CS$<>8__locals1))
					{
						CS$<>8__locals1.rect.minZ = CS$<>8__locals1.rect.minZ - 1;
						flag = true;
					}
				}
			}
			while (flag);
			foreach (IntVec3 item in CS$<>8__locals1.rect)
			{
				CS$<>8__locals1.processed.Add(item);
			}
			return CS$<>8__locals1.rect;
		}

		// Token: 0x06002B0F RID: 11023 RVA: 0x001136DE File Offset: 0x001118DE
		[CompilerGenerated]
		internal static bool <FindLargestRect>g__ShouldBreakEarly|3_0(ref LargestAreaFinder.<>c__DisplayClass3_0 A_0)
		{
			return A_0.breakEarlyOn >= 0 && A_0.largestRect.Width >= A_0.breakEarlyOn && A_0.largestRect.Height >= A_0.breakEarlyOn;
		}

		// Token: 0x06002B10 RID: 11024 RVA: 0x00113714 File Offset: 0x00111914
		[CompilerGenerated]
		internal static bool <ExpandRect>g__CanExpand|5_0(Rot4 dir, ref LargestAreaFinder.<>c__DisplayClass5_0 A_1)
		{
			foreach (IntVec3 a in A_1.rect.GetEdgeCells(dir))
			{
				IntVec3 intVec = a + dir.FacingCell;
				if (A_1.processed.Contains(intVec) || !A_1.predicate(intVec))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04001C16 RID: 7190
		private static BoolGrid visited;

		// Token: 0x04001C17 RID: 7191
		private static List<IntVec3> randomOrderWorkingList = new List<IntVec3>();

		// Token: 0x04001C18 RID: 7192
		private static HashSet<IntVec3> tmpProcessed = new HashSet<IntVec3>();
	}
}
