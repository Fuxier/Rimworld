using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200056B RID: 1387
	public static class GenSight
	{
		// Token: 0x06002ABB RID: 10939 RVA: 0x00110C9C File Offset: 0x0010EE9C
		public static bool LineOfSight(IntVec3 start, IntVec3 end, Map map, bool skipFirstCell = false, Func<IntVec3, bool> validator = null, int halfXOffset = 0, int halfZOffset = 0)
		{
			if (!start.InBounds(map) || !end.InBounds(map))
			{
				return false;
			}
			bool flag;
			if (start.x == end.x)
			{
				flag = (start.z < end.z);
			}
			else
			{
				flag = (start.x < end.x);
			}
			int num = Mathf.Abs(end.x - start.x);
			int num2 = Mathf.Abs(end.z - start.z);
			int num3 = start.x;
			int num4 = start.z;
			int i = 1 + num + num2;
			int num5 = (end.x > start.x) ? 1 : -1;
			int num6 = (end.z > start.z) ? 1 : -1;
			num *= 4;
			num2 *= 4;
			num += halfXOffset * 2;
			num2 += halfZOffset * 2;
			int num7 = num / 2 - num2 / 2;
			IntVec3 intVec = default(IntVec3);
			while (i > 1)
			{
				intVec.x = num3;
				intVec.z = num4;
				if (!skipFirstCell || !(intVec == start))
				{
					if (!intVec.CanBeSeenOverFast(map))
					{
						return false;
					}
					if (validator != null && !validator(intVec))
					{
						return false;
					}
				}
				if (num7 > 0 || (num7 == 0 && flag))
				{
					num3 += num5;
					num7 -= num2;
				}
				else
				{
					num4 += num6;
					num7 += num;
				}
				i--;
			}
			return true;
		}

		// Token: 0x06002ABC RID: 10940 RVA: 0x00110DE8 File Offset: 0x0010EFE8
		public static bool LineOfSight(IntVec3 start, IntVec3 end, Map map, CellRect startRect, CellRect endRect, Func<IntVec3, bool> validator = null)
		{
			if (!start.InBounds(map) || !end.InBounds(map))
			{
				return false;
			}
			bool flag;
			if (start.x == end.x)
			{
				flag = (start.z < end.z);
			}
			else
			{
				flag = (start.x < end.x);
			}
			int num = Mathf.Abs(end.x - start.x);
			int num2 = Mathf.Abs(end.z - start.z);
			int num3 = start.x;
			int num4 = start.z;
			int i = 1 + num + num2;
			int num5 = (end.x > start.x) ? 1 : -1;
			int num6 = (end.z > start.z) ? 1 : -1;
			int num7 = num - num2;
			num *= 2;
			num2 *= 2;
			IntVec3 intVec = default(IntVec3);
			while (i > 1)
			{
				intVec.x = num3;
				intVec.z = num4;
				if (endRect.Contains(intVec))
				{
					return true;
				}
				if (!startRect.Contains(intVec))
				{
					if (!intVec.CanBeSeenOverFast(map))
					{
						return false;
					}
					if (validator != null && !validator(intVec))
					{
						return false;
					}
				}
				if (num7 > 0 || (num7 == 0 && flag))
				{
					num3 += num5;
					num7 -= num2;
				}
				else
				{
					num4 += num6;
					num7 += num;
				}
				i--;
			}
			return true;
		}

		// Token: 0x06002ABD RID: 10941 RVA: 0x00110F2C File Offset: 0x0010F12C
		public static IEnumerable<IntVec3> PointsOnLineOfSight(IntVec3 start, IntVec3 end)
		{
			bool sideOnEqual;
			if (start.x == end.x)
			{
				sideOnEqual = (start.z < end.z);
			}
			else
			{
				sideOnEqual = (start.x < end.x);
			}
			int dx = Mathf.Abs(end.x - start.x);
			int dz = Mathf.Abs(end.z - start.z);
			int x = start.x;
			int z = start.z;
			int i = 1 + dx + dz;
			int x_inc = (end.x > start.x) ? 1 : -1;
			int z_inc = (end.z > start.z) ? 1 : -1;
			int error = dx - dz;
			dx *= 2;
			dz *= 2;
			IntVec3 c = default(IntVec3);
			while (i > 1)
			{
				c.x = x;
				c.z = z;
				yield return c;
				if (error > 0 || (error == 0 && sideOnEqual))
				{
					x += x_inc;
					error -= dz;
				}
				else
				{
					z += z_inc;
					error += dx;
				}
				int num = i - 1;
				i = num;
			}
			yield break;
		}

		// Token: 0x06002ABE RID: 10942 RVA: 0x00110F44 File Offset: 0x0010F144
		public static void PointsOnLineOfSight(IntVec3 start, IntVec3 end, Action<IntVec3> visitor)
		{
			bool flag;
			if (start.x == end.x)
			{
				flag = (start.z < end.z);
			}
			else
			{
				flag = (start.x < end.x);
			}
			int num = Mathf.Abs(end.x - start.x);
			int num2 = Mathf.Abs(end.z - start.z);
			int num3 = start.x;
			int num4 = start.z;
			int i = 1 + num + num2;
			int num5 = (end.x > start.x) ? 1 : -1;
			int num6 = (end.z > start.z) ? 1 : -1;
			int num7 = num - num2;
			num *= 2;
			num2 *= 2;
			IntVec3 obj = default(IntVec3);
			while (i > 1)
			{
				obj.x = num3;
				obj.z = num4;
				visitor(obj);
				if (num7 > 0 || (num7 == 0 && flag))
				{
					num3 += num5;
					num7 -= num2;
				}
				else
				{
					num4 += num6;
					num7 += num;
				}
				i--;
			}
		}

		// Token: 0x06002ABF RID: 10943 RVA: 0x00111048 File Offset: 0x0010F248
		public static IntVec3 LastPointOnLineOfSight(IntVec3 start, IntVec3 end, Func<IntVec3, bool> validator, bool skipFirstCell = false)
		{
			foreach (IntVec3 intVec in GenSight.PointsOnLineOfSight(start, end))
			{
				if ((!skipFirstCell || !(intVec == start)) && !validator(intVec))
				{
					return intVec;
				}
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06002AC0 RID: 10944 RVA: 0x001110B0 File Offset: 0x0010F2B0
		public static bool LineOfSightToEdges(IntVec3 start, IntVec3 end, Map map, bool skipFirstCell = false, Func<IntVec3, bool> validator = null)
		{
			if (GenSight.LineOfSight(start, end, map, skipFirstCell, validator, 0, 0))
			{
				return true;
			}
			int num = (start * 2).DistanceToSquared(end * 2);
			for (int i = 0; i < 4; i++)
			{
				if ((start * 2).DistanceToSquared(end * 2 + GenAdj.CardinalDirections[i]) <= num && GenSight.LineOfSight(start, end, map, skipFirstCell, validator, GenAdj.CardinalDirections[i].x, GenAdj.CardinalDirections[i].z))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002AC1 RID: 10945 RVA: 0x00111144 File Offset: 0x0010F344
		public static bool LineOfSightToThing(IntVec3 start, Thing t, Map map, bool skipFirstCell = false, Func<IntVec3, bool> validator = null)
		{
			if (t.def.size == IntVec2.One)
			{
				return GenSight.LineOfSight(start, t.Position, map, false, null, 0, 0);
			}
			foreach (IntVec3 end in t.OccupiedRect())
			{
				if (GenSight.LineOfSight(start, end, map, skipFirstCell, validator, 0, 0))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002AC2 RID: 10946 RVA: 0x001111D4 File Offset: 0x0010F3D4
		public static List<IntVec3> BresenhamCellsBetween(IntVec3 a, IntVec3 b)
		{
			return GenSight.BresenhamCellsBetween(a.x, a.z, b.x, b.z);
		}

		// Token: 0x06002AC3 RID: 10947 RVA: 0x001111F4 File Offset: 0x0010F3F4
		public static List<IntVec3> BresenhamCellsBetween(int x0, int y0, int x1, int y1)
		{
			GenSight.tmpCells.Clear();
			int num = Mathf.Abs(x1 - x0);
			int num2 = (x0 < x1) ? 1 : -1;
			int num3 = -Mathf.Abs(y1 - y0);
			int num4 = (y0 < y1) ? 1 : -1;
			int num5 = num + num3;
			int num6 = 1000;
			do
			{
				GenSight.tmpCells.Add(new IntVec3(x0, 0, y0));
				if (x0 == x1 && y0 == y1)
				{
					goto IL_8B;
				}
				int num7 = 2 * num5;
				if (num7 >= num3)
				{
					num5 += num3;
					x0 += num2;
				}
				if (num7 <= num)
				{
					num5 += num;
					y0 += num4;
				}
				num6--;
			}
			while (num6 > 0);
			Log.Error("BresenhamCellsBetween exceeded iterations limit of 1000.");
			IL_8B:
			return GenSight.tmpCells;
		}

		// Token: 0x04001BF1 RID: 7153
		private static List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
