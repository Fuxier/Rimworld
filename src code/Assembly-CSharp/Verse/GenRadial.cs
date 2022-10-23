using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000039 RID: 57
	public static class GenRadial
	{
		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060002EA RID: 746 RVA: 0x0000F594 File Offset: 0x0000D794
		public static float MaxRadialPatternRadius
		{
			get
			{
				return GenRadial.RadialPatternRadii[GenRadial.RadialPatternRadii.Length - 1];
			}
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000F5A8 File Offset: 0x0000D7A8
		static GenRadial()
		{
			GenRadial.SetupManualRadialPattern();
			GenRadial.SetupRadialPattern();
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000F5FC File Offset: 0x0000D7FC
		private static void SetupManualRadialPattern()
		{
			GenRadial.ManualRadialPattern[0] = new IntVec3(0, 0, 0);
			GenRadial.ManualRadialPattern[1] = new IntVec3(0, 0, -1);
			GenRadial.ManualRadialPattern[2] = new IntVec3(1, 0, 0);
			GenRadial.ManualRadialPattern[3] = new IntVec3(0, 0, 1);
			GenRadial.ManualRadialPattern[4] = new IntVec3(-1, 0, 0);
			GenRadial.ManualRadialPattern[5] = new IntVec3(1, 0, -1);
			GenRadial.ManualRadialPattern[6] = new IntVec3(1, 0, 1);
			GenRadial.ManualRadialPattern[7] = new IntVec3(-1, 0, 1);
			GenRadial.ManualRadialPattern[8] = new IntVec3(-1, 0, -1);
			GenRadial.ManualRadialPattern[9] = new IntVec3(2, 0, 0);
			GenRadial.ManualRadialPattern[10] = new IntVec3(-2, 0, 0);
			GenRadial.ManualRadialPattern[11] = new IntVec3(0, 0, 2);
			GenRadial.ManualRadialPattern[12] = new IntVec3(0, 0, -2);
			GenRadial.ManualRadialPattern[13] = new IntVec3(2, 0, 1);
			GenRadial.ManualRadialPattern[14] = new IntVec3(2, 0, -1);
			GenRadial.ManualRadialPattern[15] = new IntVec3(-2, 0, 1);
			GenRadial.ManualRadialPattern[16] = new IntVec3(-2, 0, -1);
			GenRadial.ManualRadialPattern[17] = new IntVec3(-1, 0, 2);
			GenRadial.ManualRadialPattern[18] = new IntVec3(1, 0, 2);
			GenRadial.ManualRadialPattern[19] = new IntVec3(-1, 0, -2);
			GenRadial.ManualRadialPattern[20] = new IntVec3(1, 0, -2);
			GenRadial.ManualRadialPattern[21] = new IntVec3(2, 0, 2);
			GenRadial.ManualRadialPattern[22] = new IntVec3(-2, 0, -2);
			GenRadial.ManualRadialPattern[23] = new IntVec3(2, 0, -2);
			GenRadial.ManualRadialPattern[24] = new IntVec3(-2, 0, 2);
			GenRadial.ManualRadialPattern[25] = new IntVec3(3, 0, 0);
			GenRadial.ManualRadialPattern[26] = new IntVec3(0, 0, 3);
			GenRadial.ManualRadialPattern[27] = new IntVec3(-3, 0, 0);
			GenRadial.ManualRadialPattern[28] = new IntVec3(0, 0, -3);
			GenRadial.ManualRadialPattern[29] = new IntVec3(3, 0, 1);
			GenRadial.ManualRadialPattern[30] = new IntVec3(-3, 0, -1);
			GenRadial.ManualRadialPattern[31] = new IntVec3(1, 0, 3);
			GenRadial.ManualRadialPattern[32] = new IntVec3(-1, 0, -3);
			GenRadial.ManualRadialPattern[33] = new IntVec3(-3, 0, 1);
			GenRadial.ManualRadialPattern[34] = new IntVec3(3, 0, -1);
			GenRadial.ManualRadialPattern[35] = new IntVec3(-1, 0, 3);
			GenRadial.ManualRadialPattern[36] = new IntVec3(1, 0, -3);
			GenRadial.ManualRadialPattern[37] = new IntVec3(3, 0, 2);
			GenRadial.ManualRadialPattern[38] = new IntVec3(-3, 0, -2);
			GenRadial.ManualRadialPattern[39] = new IntVec3(2, 0, 3);
			GenRadial.ManualRadialPattern[40] = new IntVec3(-2, 0, -3);
			GenRadial.ManualRadialPattern[41] = new IntVec3(-3, 0, 2);
			GenRadial.ManualRadialPattern[42] = new IntVec3(3, 0, -2);
			GenRadial.ManualRadialPattern[43] = new IntVec3(-2, 0, 3);
			GenRadial.ManualRadialPattern[44] = new IntVec3(2, 0, -3);
			GenRadial.ManualRadialPattern[45] = new IntVec3(3, 0, 3);
			GenRadial.ManualRadialPattern[46] = new IntVec3(3, 0, -3);
			GenRadial.ManualRadialPattern[47] = new IntVec3(-3, 0, 3);
			GenRadial.ManualRadialPattern[48] = new IntVec3(-3, 0, -3);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000F9F0 File Offset: 0x0000DBF0
		private static void SetupRadialPattern()
		{
			List<IntVec3> list = new List<IntVec3>();
			for (int i = -60; i < 60; i++)
			{
				for (int j = -60; j < 60; j++)
				{
					list.Add(new IntVec3(i, 0, j));
				}
			}
			list.Sort(delegate(IntVec3 A, IntVec3 B)
			{
				float num = (float)A.LengthHorizontalSquared;
				float num2 = (float)B.LengthHorizontalSquared;
				if (num < num2)
				{
					return -1;
				}
				if (num == num2)
				{
					return 0;
				}
				return 1;
			});
			for (int k = 0; k < 10000; k++)
			{
				GenRadial.RadialPattern[k] = list[k];
				GenRadial.RadialPatternRadii[k] = list[k].LengthHorizontal;
			}
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000FA8B File Offset: 0x0000DC8B
		public static int NumCellsToFillForRadius_ManualRadialPattern(int radius)
		{
			if (radius == 0)
			{
				return 1;
			}
			if (radius == 1)
			{
				return 9;
			}
			if (radius == 2)
			{
				return 21;
			}
			if (radius == 3)
			{
				return 37;
			}
			Log.Error("NumSquares radius error");
			return 0;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000FAB4 File Offset: 0x0000DCB4
		public static int NumCellsInRadius(float radius)
		{
			if (radius >= GenRadial.MaxRadialPatternRadius)
			{
				Log.Error(string.Concat(new object[]
				{
					"Not enough squares to get to radius ",
					radius,
					". Max is ",
					GenRadial.MaxRadialPatternRadius
				}));
				return 10000;
			}
			float num = radius + float.Epsilon;
			int num2 = Array.BinarySearch<float>(GenRadial.RadialPatternRadii, num);
			if (num2 < 0)
			{
				return ~num2;
			}
			for (int i = num2; i < 10000; i++)
			{
				if (GenRadial.RadialPatternRadii[i] > num)
				{
					return i;
				}
			}
			return 10000;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000FB41 File Offset: 0x0000DD41
		public static float RadiusOfNumCells(int numCells)
		{
			return GenRadial.RadialPatternRadii[numCells];
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000FB4A File Offset: 0x0000DD4A
		public static IEnumerable<IntVec3> RadialPatternInRadius(float radius)
		{
			int numSquares = GenRadial.NumCellsInRadius(radius);
			int num;
			for (int i = 0; i < numSquares; i = num + 1)
			{
				yield return GenRadial.RadialPattern[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000FB5A File Offset: 0x0000DD5A
		public static IEnumerable<IntVec3> RadialCellsAround(IntVec3 center, float radius, bool useCenter)
		{
			int numSquares = GenRadial.NumCellsInRadius(radius);
			int num;
			for (int i = useCenter ? 0 : 1; i < numSquares; i = num + 1)
			{
				yield return GenRadial.RadialPattern[i] + center;
				num = i;
			}
			yield break;
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000FB78 File Offset: 0x0000DD78
		public static IEnumerable<IntVec3> RadialCellsAround(IntVec3 center, float minRadius, float maxRadius)
		{
			int numSquares = GenRadial.NumCellsInRadius(maxRadius);
			int num;
			for (int i = 0; i < numSquares; i = num + 1)
			{
				if (GenRadial.RadialPattern[i].LengthHorizontal >= minRadius)
				{
					yield return GenRadial.RadialPattern[i] + center;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000FB96 File Offset: 0x0000DD96
		public static IEnumerable<Thing> RadialDistinctThingsAround(IntVec3 center, Map map, float radius, bool useCenter)
		{
			int numCells = GenRadial.NumCellsInRadius(radius);
			HashSet<Thing> returnedThings = null;
			int num;
			for (int i = useCenter ? 0 : 1; i < numCells; i = num + 1)
			{
				IntVec3 c = GenRadial.RadialPattern[i] + center;
				if (c.InBounds(map))
				{
					List<Thing> thingList = c.GetThingList(map);
					int j = 0;
					while (j < thingList.Count)
					{
						Thing thing = thingList[j];
						if (thing.def.size.x <= 1 && thing.def.size.z <= 1)
						{
							goto IL_FA;
						}
						if (returnedThings == null)
						{
							returnedThings = new HashSet<Thing>();
						}
						if (!returnedThings.Contains(thing))
						{
							returnedThings.Add(thing);
							goto IL_FA;
						}
						IL_111:
						num = j;
						j = num + 1;
						continue;
						IL_FA:
						yield return thing;
						goto IL_111;
					}
					thingList = null;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000FBBC File Offset: 0x0000DDBC
		public static void ProcessEquidistantCells(IntVec3 center, float radius, Func<List<IntVec3>, bool> processor, Map map = null)
		{
			if (GenRadial.working)
			{
				Log.Error("Nested calls to ProcessEquidistantCells() are not allowed.");
				return;
			}
			GenRadial.tmpCells.Clear();
			GenRadial.working = true;
			try
			{
				float num = -1f;
				int num2 = GenRadial.NumCellsInRadius(radius);
				for (int i = 0; i < num2; i++)
				{
					IntVec3 intVec = center + GenRadial.RadialPattern[i];
					if (map == null || intVec.InBounds(map))
					{
						float num3 = (float)intVec.DistanceToSquared(center);
						if (Mathf.Abs(num3 - num) > 0.0001f)
						{
							if (GenRadial.tmpCells.Any<IntVec3>() && processor(GenRadial.tmpCells))
							{
								return;
							}
							num = num3;
							GenRadial.tmpCells.Clear();
						}
						GenRadial.tmpCells.Add(intVec);
					}
				}
				if (GenRadial.tmpCells.Any<IntVec3>())
				{
					processor(GenRadial.tmpCells);
				}
			}
			finally
			{
				GenRadial.tmpCells.Clear();
				GenRadial.working = false;
			}
		}

		// Token: 0x040000A7 RID: 167
		public static IntVec3[] ManualRadialPattern = new IntVec3[49];

		// Token: 0x040000A8 RID: 168
		public static IntVec3[] RadialPattern = new IntVec3[10000];

		// Token: 0x040000A9 RID: 169
		private static float[] RadialPatternRadii = new float[10000];

		// Token: 0x040000AA RID: 170
		private const int RadialPatternCount = 10000;

		// Token: 0x040000AB RID: 171
		private static List<IntVec3> tmpCells = new List<IntVec3>();

		// Token: 0x040000AC RID: 172
		private static bool working = false;
	}
}
