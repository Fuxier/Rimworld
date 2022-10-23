using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200059C RID: 1436
	public static class ShootLeanUtility
	{
		// Token: 0x06002BBF RID: 11199 RVA: 0x00115978 File Offset: 0x00113B78
		private static bool[] GetWorkingBlockedArray()
		{
			if (ShootLeanUtility.blockedArrays.Count > 0)
			{
				return ShootLeanUtility.blockedArrays.Dequeue();
			}
			return new bool[8];
		}

		// Token: 0x06002BC0 RID: 11200 RVA: 0x00115998 File Offset: 0x00113B98
		private static void ReturnWorkingBlockedArray(bool[] ar)
		{
			ShootLeanUtility.blockedArrays.Enqueue(ar);
			if (ShootLeanUtility.blockedArrays.Count > 128)
			{
				Log.ErrorOnce("Too many blocked arrays to be feasible. >128", 388121);
			}
		}

		// Token: 0x06002BC1 RID: 11201 RVA: 0x001159C8 File Offset: 0x00113BC8
		public static void LeanShootingSourcesFromTo(IntVec3 shooterLoc, IntVec3 targetPos, Map map, List<IntVec3> listToFill)
		{
			listToFill.Clear();
			float angleFlat = (targetPos - shooterLoc).AngleFlat;
			bool flag = angleFlat > 270f || angleFlat < 90f;
			bool flag2 = angleFlat > 90f && angleFlat < 270f;
			bool flag3 = angleFlat > 180f;
			bool flag4 = angleFlat < 180f;
			bool[] workingBlockedArray = ShootLeanUtility.GetWorkingBlockedArray();
			for (int i = 0; i < 8; i++)
			{
				workingBlockedArray[i] = !(shooterLoc + GenAdj.AdjacentCells[i]).CanBeSeenOver(map);
			}
			if (!workingBlockedArray[1] && ((workingBlockedArray[0] && !workingBlockedArray[5] && flag) || (workingBlockedArray[2] && !workingBlockedArray[4] && flag2)))
			{
				listToFill.Add(shooterLoc + new IntVec3(1, 0, 0));
			}
			if (!workingBlockedArray[3] && ((workingBlockedArray[0] && !workingBlockedArray[6] && flag) || (workingBlockedArray[2] && !workingBlockedArray[7] && flag2)))
			{
				listToFill.Add(shooterLoc + new IntVec3(-1, 0, 0));
			}
			if (!workingBlockedArray[2] && ((workingBlockedArray[3] && !workingBlockedArray[7] && flag3) || (workingBlockedArray[1] && !workingBlockedArray[4] && flag4)))
			{
				listToFill.Add(shooterLoc + new IntVec3(0, 0, -1));
			}
			if (!workingBlockedArray[0] && ((workingBlockedArray[3] && !workingBlockedArray[6] && flag3) || (workingBlockedArray[1] && !workingBlockedArray[5] && flag4)))
			{
				listToFill.Add(shooterLoc + new IntVec3(0, 0, 1));
			}
			for (int j = 0; j < 4; j++)
			{
				if (!workingBlockedArray[j] && (j != 0 || flag) && (j != 1 || flag4) && (j != 2 || flag2) && (j != 3 || flag3) && (shooterLoc + GenAdj.AdjacentCells[j]).GetCover(map) != null)
				{
					listToFill.Add(shooterLoc + GenAdj.AdjacentCells[j]);
				}
			}
			ShootLeanUtility.ReturnWorkingBlockedArray(workingBlockedArray);
		}

		// Token: 0x06002BC2 RID: 11202 RVA: 0x00115BD8 File Offset: 0x00113DD8
		public static void CalcShootableCellsOf(List<IntVec3> outCells, Thing t)
		{
			outCells.Clear();
			if (t is Pawn)
			{
				outCells.Add(t.Position);
				for (int i = 0; i < 4; i++)
				{
					IntVec3 intVec = t.Position + GenAdj.CardinalDirections[i];
					if (intVec.CanBeSeenOver(t.Map))
					{
						outCells.Add(intVec);
					}
				}
				return;
			}
			outCells.Add(t.Position);
			if (t.def.size.x != 1 || t.def.size.z != 1)
			{
				foreach (IntVec3 intVec2 in t.OccupiedRect())
				{
					if (intVec2 != t.Position)
					{
						outCells.Add(intVec2);
					}
				}
			}
		}

		// Token: 0x04001CB5 RID: 7349
		private static Queue<bool[]> blockedArrays = new Queue<bool[]>();
	}
}
