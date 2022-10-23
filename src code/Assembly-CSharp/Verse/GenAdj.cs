using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200002E RID: 46
	public static class GenAdj
	{
		// Token: 0x0600020C RID: 524 RVA: 0x0000ABB8 File Offset: 0x00008DB8
		static GenAdj()
		{
			GenAdj.SetupAdjacencyTables();
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000AC48 File Offset: 0x00008E48
		private static void SetupAdjacencyTables()
		{
			GenAdj.CardinalDirections[0] = new IntVec3(0, 0, 1);
			GenAdj.CardinalDirections[1] = new IntVec3(1, 0, 0);
			GenAdj.CardinalDirections[2] = new IntVec3(0, 0, -1);
			GenAdj.CardinalDirections[3] = new IntVec3(-1, 0, 0);
			GenAdj.CardinalDirectionsAndInside[0] = new IntVec3(0, 0, 1);
			GenAdj.CardinalDirectionsAndInside[1] = new IntVec3(1, 0, 0);
			GenAdj.CardinalDirectionsAndInside[2] = new IntVec3(0, 0, -1);
			GenAdj.CardinalDirectionsAndInside[3] = new IntVec3(-1, 0, 0);
			GenAdj.CardinalDirectionsAndInside[4] = new IntVec3(0, 0, 0);
			GenAdj.CardinalDirectionsAround[0] = new IntVec3(0, 0, -1);
			GenAdj.CardinalDirectionsAround[1] = new IntVec3(-1, 0, 0);
			GenAdj.CardinalDirectionsAround[2] = new IntVec3(0, 0, 1);
			GenAdj.CardinalDirectionsAround[3] = new IntVec3(1, 0, 0);
			GenAdj.DiagonalDirections[0] = new IntVec3(-1, 0, -1);
			GenAdj.DiagonalDirections[1] = new IntVec3(-1, 0, 1);
			GenAdj.DiagonalDirections[2] = new IntVec3(1, 0, 1);
			GenAdj.DiagonalDirections[3] = new IntVec3(1, 0, -1);
			GenAdj.DiagonalDirectionsAround[0] = new IntVec3(-1, 0, -1);
			GenAdj.DiagonalDirectionsAround[1] = new IntVec3(-1, 0, 1);
			GenAdj.DiagonalDirectionsAround[2] = new IntVec3(1, 0, 1);
			GenAdj.DiagonalDirectionsAround[3] = new IntVec3(1, 0, -1);
			GenAdj.AdjacentCells[0] = new IntVec3(0, 0, 1);
			GenAdj.AdjacentCells[1] = new IntVec3(1, 0, 0);
			GenAdj.AdjacentCells[2] = new IntVec3(0, 0, -1);
			GenAdj.AdjacentCells[3] = new IntVec3(-1, 0, 0);
			GenAdj.AdjacentCells[4] = new IntVec3(1, 0, -1);
			GenAdj.AdjacentCells[5] = new IntVec3(1, 0, 1);
			GenAdj.AdjacentCells[6] = new IntVec3(-1, 0, 1);
			GenAdj.AdjacentCells[7] = new IntVec3(-1, 0, -1);
			GenAdj.AdjacentCellsAndInside[0] = new IntVec3(0, 0, 1);
			GenAdj.AdjacentCellsAndInside[1] = new IntVec3(1, 0, 0);
			GenAdj.AdjacentCellsAndInside[2] = new IntVec3(0, 0, -1);
			GenAdj.AdjacentCellsAndInside[3] = new IntVec3(-1, 0, 0);
			GenAdj.AdjacentCellsAndInside[4] = new IntVec3(1, 0, -1);
			GenAdj.AdjacentCellsAndInside[5] = new IntVec3(1, 0, 1);
			GenAdj.AdjacentCellsAndInside[6] = new IntVec3(-1, 0, 1);
			GenAdj.AdjacentCellsAndInside[7] = new IntVec3(-1, 0, -1);
			GenAdj.AdjacentCellsAndInside[8] = new IntVec3(0, 0, 0);
			GenAdj.AdjacentCellsAround[0] = new IntVec3(0, 0, 1);
			GenAdj.AdjacentCellsAround[1] = new IntVec3(1, 0, 1);
			GenAdj.AdjacentCellsAround[2] = new IntVec3(1, 0, 0);
			GenAdj.AdjacentCellsAround[3] = new IntVec3(1, 0, -1);
			GenAdj.AdjacentCellsAround[4] = new IntVec3(0, 0, -1);
			GenAdj.AdjacentCellsAround[5] = new IntVec3(-1, 0, -1);
			GenAdj.AdjacentCellsAround[6] = new IntVec3(-1, 0, 0);
			GenAdj.AdjacentCellsAround[7] = new IntVec3(-1, 0, 1);
			GenAdj.AdjacentCellsAroundBottom[0] = new IntVec3(0, 0, -1);
			GenAdj.AdjacentCellsAroundBottom[1] = new IntVec3(-1, 0, -1);
			GenAdj.AdjacentCellsAroundBottom[2] = new IntVec3(-1, 0, 0);
			GenAdj.AdjacentCellsAroundBottom[3] = new IntVec3(-1, 0, 1);
			GenAdj.AdjacentCellsAroundBottom[4] = new IntVec3(0, 0, 1);
			GenAdj.AdjacentCellsAroundBottom[5] = new IntVec3(1, 0, 1);
			GenAdj.AdjacentCellsAroundBottom[6] = new IntVec3(1, 0, 0);
			GenAdj.AdjacentCellsAroundBottom[7] = new IntVec3(1, 0, -1);
			GenAdj.AdjacentCellsAroundBottom[8] = new IntVec3(0, 0, 0);
			GenAdj.AdjacentCellsAndInsideForUV[0] = new IntVec3(0, 0, -1);
			GenAdj.AdjacentCellsAndInsideForUV[1] = new IntVec3(-1, 0, -1);
			GenAdj.AdjacentCellsAndInsideForUV[2] = new IntVec3(-1, 0, 0);
			GenAdj.AdjacentCellsAndInsideForUV[3] = new IntVec3(-1, 0, 1);
			GenAdj.AdjacentCellsAndInsideForUV[4] = new IntVec3(0, 0, 1);
			GenAdj.AdjacentCellsAndInsideForUV[5] = new IntVec3(1, 0, 1);
			GenAdj.AdjacentCellsAndInsideForUV[6] = new IntVec3(1, 0, 0);
			GenAdj.AdjacentCellsAndInsideForUV[7] = new IntVec3(1, 0, -1);
			GenAdj.AdjacentCellsAndInsideForUV[8] = new IntVec3(0, 0, 0);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000B118 File Offset: 0x00009318
		public static List<IntVec3> AdjacentCells8WayRandomized()
		{
			if (GenAdj.adjRandomOrderList == null)
			{
				GenAdj.adjRandomOrderList = new List<IntVec3>();
				for (int i = 0; i < 8; i++)
				{
					GenAdj.adjRandomOrderList.Add(GenAdj.AdjacentCells[i]);
				}
			}
			GenAdj.adjRandomOrderList.Shuffle<IntVec3>();
			return GenAdj.adjRandomOrderList;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000B166 File Offset: 0x00009366
		public static IEnumerable<IntVec3> CellsOccupiedBy(Thing t)
		{
			if (t.def.size.x == 1 && t.def.size.z == 1)
			{
				yield return t.Position;
			}
			else
			{
				foreach (IntVec3 intVec in GenAdj.CellsOccupiedBy(t.Position, t.Rotation, t.def.size))
				{
					yield return intVec;
				}
				IEnumerator<IntVec3> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000B176 File Offset: 0x00009376
		public static IEnumerable<IntVec3> CellsOccupiedBy(IntVec3 center, Rot4 rotation, IntVec2 size)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rotation);
			int num = center.x - (size.x - 1) / 2;
			int minZ = center.z - (size.z - 1) / 2;
			int maxX = num + size.x - 1;
			int maxZ = minZ + size.z - 1;
			int num2;
			for (int i = num; i <= maxX; i = num2 + 1)
			{
				for (int j = minZ; j <= maxZ; j = num2 + 1)
				{
					yield return new IntVec3(i, 0, j);
					num2 = j;
				}
				num2 = i;
			}
			yield break;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000B194 File Offset: 0x00009394
		public static IEnumerable<IntVec3> CellsAdjacent8Way(TargetInfo pack)
		{
			if (pack.HasThing)
			{
				foreach (IntVec3 intVec in GenAdj.CellsAdjacent8Way(pack.Thing))
				{
					yield return intVec;
				}
				IEnumerator<IntVec3> enumerator = null;
			}
			else
			{
				int num;
				for (int i = 0; i < 8; i = num + 1)
				{
					yield return pack.Cell + GenAdj.AdjacentCells[i];
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000B1A4 File Offset: 0x000093A4
		public static IEnumerable<IntVec3> CellsAdjacent8Way(Thing t)
		{
			return GenAdj.CellsAdjacent8Way(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000B1C2 File Offset: 0x000093C2
		public static IEnumerable<IntVec3> CellsAdjacent8Way(IntVec3 thingCenter, Rot4 thingRot, IntVec2 thingSize)
		{
			GenAdj.AdjustForRotation(ref thingCenter, ref thingSize, thingRot);
			int minX = thingCenter.x - (thingSize.x - 1) / 2 - 1;
			int maxX = minX + thingSize.x + 1;
			int minZ = thingCenter.z - (thingSize.z - 1) / 2 - 1;
			int maxZ = minZ + thingSize.z + 1;
			IntVec3 cur = new IntVec3(minX - 1, 0, minZ);
			do
			{
				cur.x++;
				yield return cur;
			}
			while (cur.x < maxX);
			do
			{
				cur.z++;
				yield return cur;
			}
			while (cur.z < maxZ);
			do
			{
				cur.x--;
				yield return cur;
			}
			while (cur.x > minX);
			do
			{
				cur.z--;
				yield return cur;
			}
			while (cur.z > minZ + 1);
			yield break;
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000B1E0 File Offset: 0x000093E0
		public static IEnumerable<IntVec3> CellsAdjacentCardinal(Thing t)
		{
			return GenAdj.CellsAdjacentCardinal(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000B1FE File Offset: 0x000093FE
		public static IEnumerable<IntVec3> CellsAdjacentCardinal(IntVec3 center, Rot4 rot, IntVec2 size)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			int minX = center.x - (size.x - 1) / 2 - 1;
			int maxX = minX + size.x + 1;
			int minZ = center.z - (size.z - 1) / 2 - 1;
			int maxZ = minZ + size.z + 1;
			IntVec3 cur = new IntVec3(minX, 0, minZ);
			do
			{
				cur.x++;
				yield return cur;
			}
			while (cur.x < maxX - 1);
			cur.x++;
			do
			{
				cur.z++;
				yield return cur;
			}
			while (cur.z < maxZ - 1);
			cur.z++;
			do
			{
				cur.x--;
				yield return cur;
			}
			while (cur.x > minX + 1);
			cur.x--;
			do
			{
				cur.z--;
				yield return cur;
			}
			while (cur.z > minZ + 1);
			yield break;
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000B21C File Offset: 0x0000941C
		public static IEnumerable<IntVec3> CellsAdjacentAlongEdge(IntVec3 thingCent, Rot4 thingRot, IntVec2 thingSize, LinkDirections dir)
		{
			GenAdj.AdjustForRotation(ref thingCent, ref thingSize, thingRot);
			int minX = thingCent.x - (thingSize.x - 1) / 2 - 1;
			int minZ = thingCent.z - (thingSize.z - 1) / 2 - 1;
			int maxX = minX + thingSize.x + 1;
			int maxZ = minZ + thingSize.z + 1;
			if (dir == LinkDirections.Down)
			{
				int num;
				for (int x = minX; x <= maxX; x = num + 1)
				{
					yield return new IntVec3(x, thingCent.y, minZ - 1);
					num = x;
				}
			}
			if (dir == LinkDirections.Up)
			{
				int num;
				for (int x = minX; x <= maxX; x = num + 1)
				{
					yield return new IntVec3(x, thingCent.y, maxZ + 1);
					num = x;
				}
			}
			if (dir == LinkDirections.Left)
			{
				int num;
				for (int x = minZ; x <= maxZ; x = num + 1)
				{
					yield return new IntVec3(minX - 1, thingCent.y, x);
					num = x;
				}
			}
			if (dir == LinkDirections.Right)
			{
				int num;
				for (int x = minZ; x <= maxZ; x = num + 1)
				{
					yield return new IntVec3(maxX + 1, thingCent.y, x);
					num = x;
				}
			}
			yield break;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000B241 File Offset: 0x00009441
		public static IEnumerable<IntVec3> CellsAdjacent8WayAndInside(this Thing thing)
		{
			IntVec3 position = thing.Position;
			IntVec2 size = thing.def.size;
			Rot4 rotation = thing.Rotation;
			GenAdj.AdjustForRotation(ref position, ref size, rotation);
			int num = position.x - (size.x - 1) / 2 - 1;
			int minZ = position.z - (size.z - 1) / 2 - 1;
			int maxX = num + size.x + 1;
			int maxZ = minZ + size.z + 1;
			int num2;
			for (int i = num; i <= maxX; i = num2 + 1)
			{
				for (int j = minZ; j <= maxZ; j = num2 + 1)
				{
					yield return new IntVec3(i, 0, j);
					num2 = j;
				}
				num2 = i;
			}
			yield break;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000B251 File Offset: 0x00009451
		public static void GetAdjacentCorners(LocalTargetInfo target, out IntVec3 BL, out IntVec3 TL, out IntVec3 TR, out IntVec3 BR)
		{
			if (target.HasThing)
			{
				GenAdj.GetAdjacentCorners(target.Thing.OccupiedRect(), out BL, out TL, out TR, out BR);
				return;
			}
			GenAdj.GetAdjacentCorners(CellRect.SingleCell(target.Cell), out BL, out TL, out TR, out BR);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000B28C File Offset: 0x0000948C
		private static void GetAdjacentCorners(CellRect rect, out IntVec3 BL, out IntVec3 TL, out IntVec3 TR, out IntVec3 BR)
		{
			BL = new IntVec3(rect.minX - 1, 0, rect.minZ - 1);
			TL = new IntVec3(rect.minX - 1, 0, rect.maxZ + 1);
			TR = new IntVec3(rect.maxX + 1, 0, rect.maxZ + 1);
			BR = new IntVec3(rect.maxX + 1, 0, rect.minZ - 1);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000B30A File Offset: 0x0000950A
		public static IntVec3 RandomAdjacentCell8Way(this IntVec3 root)
		{
			return root + GenAdj.AdjacentCells[Rand.RangeInclusive(0, 7)];
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000B323 File Offset: 0x00009523
		public static IntVec3 RandomAdjacentCellCardinal(this IntVec3 root)
		{
			return root + GenAdj.CardinalDirections[Rand.RangeInclusive(0, 3)];
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000B33C File Offset: 0x0000953C
		public static IntVec3 RandomAdjacentCell8Way(this Thing t)
		{
			CellRect cellRect = t.OccupiedRect();
			CellRect cellRect2 = cellRect.ExpandedBy(1);
			IntVec3 randomCell;
			do
			{
				randomCell = cellRect2.RandomCell;
			}
			while (cellRect.Contains(randomCell));
			return randomCell;
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000B36C File Offset: 0x0000956C
		public static IntVec3 RandomAdjacentCellCardinal(this Thing t)
		{
			CellRect cellRect = t.OccupiedRect();
			IntVec3 randomCell = cellRect.RandomCell;
			if (Rand.Value < 0.5f)
			{
				if (Rand.Value < 0.5f)
				{
					randomCell.x = cellRect.minX - 1;
				}
				else
				{
					randomCell.x = cellRect.maxX + 1;
				}
			}
			else if (Rand.Value < 0.5f)
			{
				randomCell.z = cellRect.minZ - 1;
			}
			else
			{
				randomCell.z = cellRect.maxZ + 1;
			}
			return randomCell;
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000B3EF File Offset: 0x000095EF
		public static bool TryFindRandomAdjacentCell8WayWithRoom(Thing t, out IntVec3 result)
		{
			return GenAdj.TryFindRandomAdjacentCell8WayWithRoom(t.Position, t.Rotation, t.def.size, t.Map, out result);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000B414 File Offset: 0x00009614
		public static bool TryFindRandomAdjacentCell8WayWithRoom(IntVec3 center, Rot4 rot, IntVec2 size, Map map, out IntVec3 result)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			GenAdj.validCells.Clear();
			foreach (IntVec3 intVec in GenAdj.CellsAdjacent8Way(center, rot, size))
			{
				if (intVec.InBounds(map) && intVec.GetRoom(map) != null)
				{
					GenAdj.validCells.Add(intVec);
				}
			}
			return GenAdj.validCells.TryRandomElement(out result);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000B49C File Offset: 0x0000969C
		public static bool AdjacentTo8WayOrInside(this IntVec3 me, LocalTargetInfo other)
		{
			if (other.HasThing)
			{
				return me.AdjacentTo8WayOrInside(other.Thing);
			}
			return me.AdjacentTo8WayOrInside(other.Cell);
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000B4C4 File Offset: 0x000096C4
		public static bool AdjacentTo8Way(this IntVec3 me, IntVec3 other)
		{
			int num = me.x - other.x;
			int num2 = me.z - other.z;
			if (num == 0 && num2 == 0)
			{
				return false;
			}
			if (num < 0)
			{
				num *= -1;
			}
			if (num2 < 0)
			{
				num2 *= -1;
			}
			return num <= 1 && num2 <= 1;
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000B514 File Offset: 0x00009714
		public static bool AdjacentTo8WayOrInside(this IntVec3 me, IntVec3 other)
		{
			int num = me.x - other.x;
			int num2 = me.z - other.z;
			if (num < 0)
			{
				num *= -1;
			}
			if (num2 < 0)
			{
				num2 *= -1;
			}
			return num <= 1 && num2 <= 1;
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000B55C File Offset: 0x0000975C
		public static bool IsAdjacentToCardinalOrInside(this IntVec3 me, CellRect other)
		{
			if (other.IsEmpty)
			{
				return false;
			}
			CellRect cellRect = other.ExpandedBy(1);
			return cellRect.Contains(me) && !cellRect.IsCorner(me);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000B594 File Offset: 0x00009794
		public static bool IsAdjacentToCardinalOrInside(this Thing t1, Thing t2)
		{
			return GenAdj.IsAdjacentToCardinalOrInside(t1.OccupiedRect(), t2.OccupiedRect());
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000B5A8 File Offset: 0x000097A8
		public static bool IsAdjacentToCardinalOrInside(CellRect rect1, CellRect rect2)
		{
			if (rect1.IsEmpty || rect2.IsEmpty)
			{
				return false;
			}
			CellRect cellRect = rect1.ExpandedBy(1);
			int minX = cellRect.minX;
			int maxX = cellRect.maxX;
			int minZ = cellRect.minZ;
			int maxZ = cellRect.maxZ;
			int i = minX;
			int j = minZ;
			while (i <= maxX)
			{
				if (rect2.Contains(new IntVec3(i, 0, j)) && (i != minX || j != minZ) && (i != minX || j != maxZ) && (i != maxX || j != minZ) && (i != maxX || j != maxZ))
				{
					return true;
				}
				i++;
			}
			i--;
			for (j++; j <= maxZ; j++)
			{
				if (rect2.Contains(new IntVec3(i, 0, j)) && (i != minX || j != minZ) && (i != minX || j != maxZ) && (i != maxX || j != minZ) && (i != maxX || j != maxZ))
				{
					return true;
				}
			}
			j--;
			for (i--; i >= minX; i--)
			{
				if (rect2.Contains(new IntVec3(i, 0, j)) && (i != minX || j != minZ) && (i != minX || j != maxZ) && (i != maxX || j != minZ) && (i != maxX || j != maxZ))
				{
					return true;
				}
			}
			i++;
			for (j--; j > minZ; j--)
			{
				if (rect2.Contains(new IntVec3(i, 0, j)) && (i != minX || j != minZ) && (i != minX || j != maxZ) && (i != maxX || j != minZ) && (i != maxX || j != maxZ))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000B73F File Offset: 0x0000993F
		public static bool AdjacentTo8WayOrInside(this IntVec3 root, Thing t)
		{
			return root.AdjacentTo8WayOrInside(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000B760 File Offset: 0x00009960
		public static bool AdjacentTo8WayOrInside(this IntVec3 root, IntVec3 center, Rot4 rot, IntVec2 size)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			int num = center.x - (size.x - 1) / 2 - 1;
			int num2 = center.z - (size.z - 1) / 2 - 1;
			int num3 = num + size.x + 1;
			int num4 = num2 + size.z + 1;
			return root.x >= num && root.x <= num3 && root.z >= num2 && root.z <= num4;
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000B7DC File Offset: 0x000099DC
		public static bool AdjacentTo8WayOrInside(this Thing a, Thing b)
		{
			return GenAdj.AdjacentTo8WayOrInside(a.OccupiedRect(), b.OccupiedRect());
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000B7F0 File Offset: 0x000099F0
		public static bool AdjacentTo8WayOrInside(CellRect rect1, CellRect rect2)
		{
			return !rect1.IsEmpty && !rect2.IsEmpty && rect1.ExpandedBy(1).Overlaps(rect2);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000B822 File Offset: 0x00009A22
		public static bool IsInside(this IntVec3 root, Thing t)
		{
			return GenAdj.IsInside(root, t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000B844 File Offset: 0x00009A44
		public static bool IsInside(IntVec3 root, IntVec3 center, Rot4 rot, IntVec2 size)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			int num = center.x - (size.x - 1) / 2;
			int num2 = center.z - (size.z - 1) / 2;
			int num3 = num + size.x - 1;
			int num4 = num2 + size.z - 1;
			return root.x >= num && root.x <= num3 && root.z >= num2 && root.z <= num4;
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000B8BC File Offset: 0x00009ABC
		public static CellRect OccupiedRect(this Thing t)
		{
			return GenAdj.OccupiedRect(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000B8DA File Offset: 0x00009ADA
		public static CellRect OccupiedRect(IntVec3 center, Rot4 rot, IntVec2 size)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			return new CellRect(center.x - (size.x - 1) / 2, center.z - (size.z - 1) / 2, size.x, size.z);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000B91C File Offset: 0x00009B1C
		public static void AdjustForRotation(ref IntVec3 center, ref IntVec2 size, Rot4 rot)
		{
			if (size.x == 1 && size.z == 1)
			{
				return;
			}
			if (rot.IsHorizontal)
			{
				int x = size.x;
				size.x = size.z;
				size.z = x;
			}
			switch (rot.AsInt)
			{
			case 0:
				break;
			case 1:
				if (size.z % 2 == 0)
				{
					center.z--;
					return;
				}
				break;
			case 2:
				if (size.x % 2 == 0)
				{
					center.x--;
				}
				if (size.z % 2 == 0)
				{
					center.z--;
					return;
				}
				break;
			case 3:
				if (size.x % 2 == 0)
				{
					center.x--;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x04000086 RID: 134
		public static IntVec3[] CardinalDirections = new IntVec3[4];

		// Token: 0x04000087 RID: 135
		public static IntVec3[] CardinalDirectionsAndInside = new IntVec3[5];

		// Token: 0x04000088 RID: 136
		public static IntVec3[] CardinalDirectionsAround = new IntVec3[4];

		// Token: 0x04000089 RID: 137
		public static IntVec3[] DiagonalDirections = new IntVec3[4];

		// Token: 0x0400008A RID: 138
		public static IntVec3[] DiagonalDirectionsAround = new IntVec3[4];

		// Token: 0x0400008B RID: 139
		public static IntVec3[] AdjacentCells = new IntVec3[8];

		// Token: 0x0400008C RID: 140
		public static IntVec3[] AdjacentCellsAndInside = new IntVec3[9];

		// Token: 0x0400008D RID: 141
		public static IntVec3[] AdjacentCellsAround = new IntVec3[8];

		// Token: 0x0400008E RID: 142
		public static IntVec3[] AdjacentCellsAroundBottom = new IntVec3[9];

		// Token: 0x0400008F RID: 143
		public static IntVec3[] AdjacentCellsAndInsideForUV = new IntVec3[9];

		// Token: 0x04000090 RID: 144
		private static List<IntVec3> adjRandomOrderList;

		// Token: 0x04000091 RID: 145
		private static List<IntVec3> validCells = new List<IntVec3>();
	}
}
