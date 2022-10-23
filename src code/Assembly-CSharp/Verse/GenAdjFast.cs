using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200002F RID: 47
	public static class GenAdjFast
	{
		// Token: 0x0600022F RID: 559 RVA: 0x0000B9D4 File Offset: 0x00009BD4
		public static List<IntVec3> AdjacentCells8Way(LocalTargetInfo pack)
		{
			if (pack.HasThing)
			{
				return GenAdjFast.AdjacentCells8Way((Thing)pack);
			}
			return GenAdjFast.AdjacentCells8Way((IntVec3)pack);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000B9F8 File Offset: 0x00009BF8
		public static List<IntVec3> AdjacentCells8Way(IntVec3 root)
		{
			if (GenAdjFast.working)
			{
				throw new InvalidOperationException("GenAdjFast is already working.");
			}
			GenAdjFast.resultList.Clear();
			GenAdjFast.working = true;
			for (int i = 0; i < 8; i++)
			{
				GenAdjFast.resultList.Add(root + GenAdj.AdjacentCells[i]);
			}
			GenAdjFast.working = false;
			return GenAdjFast.resultList;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000BA59 File Offset: 0x00009C59
		private static List<IntVec3> AdjacentCells8Way(Thing t)
		{
			return GenAdjFast.AdjacentCells8Way(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000BA78 File Offset: 0x00009C78
		public static List<IntVec3> AdjacentCells8Way(IntVec3 thingCenter, Rot4 thingRot, IntVec2 thingSize)
		{
			if (thingSize.x == 1 && thingSize.z == 1)
			{
				return GenAdjFast.AdjacentCells8Way(thingCenter);
			}
			if (GenAdjFast.working)
			{
				throw new InvalidOperationException("GenAdjFast is already working.");
			}
			GenAdjFast.resultList.Clear();
			GenAdjFast.working = true;
			GenAdj.AdjustForRotation(ref thingCenter, ref thingSize, thingRot);
			int num = thingCenter.x - (thingSize.x - 1) / 2 - 1;
			int num2 = num + thingSize.x + 1;
			int num3 = thingCenter.z - (thingSize.z - 1) / 2 - 1;
			int num4 = num3 + thingSize.z + 1;
			IntVec3 intVec = new IntVec3(num - 1, 0, num3);
			do
			{
				intVec.x++;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.x < num2);
			do
			{
				intVec.z++;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.z < num4);
			do
			{
				intVec.x--;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.x > num);
			do
			{
				intVec.z--;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.z > num3 + 1);
			GenAdjFast.working = false;
			return GenAdjFast.resultList;
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000BBA9 File Offset: 0x00009DA9
		public static List<IntVec3> AdjacentCellsCardinal(LocalTargetInfo pack)
		{
			if (pack.HasThing)
			{
				return GenAdjFast.AdjacentCellsCardinal((Thing)pack);
			}
			return GenAdjFast.AdjacentCellsCardinal((IntVec3)pack);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000BBCC File Offset: 0x00009DCC
		public static List<IntVec3> AdjacentCellsCardinal(IntVec3 root)
		{
			if (GenAdjFast.working)
			{
				throw new InvalidOperationException("GenAdjFast is already working.");
			}
			GenAdjFast.resultList.Clear();
			GenAdjFast.working = true;
			for (int i = 0; i < 4; i++)
			{
				GenAdjFast.resultList.Add(root + GenAdj.CardinalDirections[i]);
			}
			GenAdjFast.working = false;
			return GenAdjFast.resultList;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000BC2D File Offset: 0x00009E2D
		private static List<IntVec3> AdjacentCellsCardinal(Thing t)
		{
			return GenAdjFast.AdjacentCellsCardinal(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000BC4C File Offset: 0x00009E4C
		public static List<IntVec3> AdjacentCellsCardinal(IntVec3 thingCenter, Rot4 thingRot, IntVec2 thingSize)
		{
			if (thingSize.x == 1 && thingSize.z == 1)
			{
				return GenAdjFast.AdjacentCellsCardinal(thingCenter);
			}
			if (GenAdjFast.working)
			{
				throw new InvalidOperationException("GenAdjFast is already working.");
			}
			GenAdjFast.resultList.Clear();
			GenAdjFast.working = true;
			GenAdj.AdjustForRotation(ref thingCenter, ref thingSize, thingRot);
			int num = thingCenter.x - (thingSize.x - 1) / 2 - 1;
			int num2 = num + thingSize.x + 1;
			int num3 = thingCenter.z - (thingSize.z - 1) / 2 - 1;
			int num4 = num3 + thingSize.z + 1;
			IntVec3 intVec = new IntVec3(num, 0, num3);
			do
			{
				intVec.x++;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.x < num2 - 1);
			intVec.x++;
			do
			{
				intVec.z++;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.z < num4 - 1);
			intVec.z++;
			do
			{
				intVec.x--;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.x > num + 1);
			intVec.x--;
			do
			{
				intVec.z--;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.z > num3 + 1);
			GenAdjFast.working = false;
			return GenAdjFast.resultList;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000BDA8 File Offset: 0x00009FA8
		public static void AdjacentThings8Way(Thing thing, List<Thing> outThings)
		{
			outThings.Clear();
			if (!thing.Spawned)
			{
				return;
			}
			Map map = thing.Map;
			List<IntVec3> list = GenAdjFast.AdjacentCells8Way(thing);
			for (int i = 0; i < list.Count; i++)
			{
				List<Thing> thingList = list[i].GetThingList(map);
				for (int j = 0; j < thingList.Count; j++)
				{
					if (!outThings.Contains(thingList[j]))
					{
						outThings.Add(thingList[j]);
					}
				}
			}
		}

		// Token: 0x04000092 RID: 146
		private static List<IntVec3> resultList = new List<IntVec3>();

		// Token: 0x04000093 RID: 147
		private static bool working = false;
	}
}
