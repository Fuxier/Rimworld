using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000025 RID: 37
	public static class IntVec3Utility
	{
		// Token: 0x0600018A RID: 394 RVA: 0x00008D63 File Offset: 0x00006F63
		public static IntVec3 ToIntVec3(this Vector3 vect)
		{
			return new IntVec3(vect);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00008D6C File Offset: 0x00006F6C
		public static float DistanceTo(this IntVec3 a, IntVec3 b)
		{
			return (a - b).LengthHorizontal;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00008D88 File Offset: 0x00006F88
		public static int DistanceToSquared(this IntVec3 a, IntVec3 b)
		{
			return (a - b).LengthHorizontalSquared;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00008DA4 File Offset: 0x00006FA4
		public static IntVec3 RotatedBy(this IntVec3 orig, RotationDirection rot)
		{
			switch (rot)
			{
			case RotationDirection.None:
				return orig;
			case RotationDirection.Clockwise:
				return new IntVec3(orig.z, orig.y, -orig.x);
			case RotationDirection.Opposite:
				return new IntVec3(-orig.x, orig.y, -orig.z);
			case RotationDirection.Counterclockwise:
				return new IntVec3(-orig.z, orig.y, orig.x);
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00008E1C File Offset: 0x0000701C
		public static IntVec3 RotatedBy(this IntVec3 orig, Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
				return orig;
			case 1:
				return new IntVec3(orig.z, orig.y, -orig.x);
			case 2:
				return new IntVec3(-orig.x, orig.y, -orig.z);
			case 3:
				return new IntVec3(-orig.z, orig.y, orig.x);
			default:
				return orig;
			}
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00008E98 File Offset: 0x00007098
		public static IntVec3 Inverse(this IntVec3 orig)
		{
			return new IntVec3(-orig.x, -orig.y, -orig.z);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00008EB4 File Offset: 0x000070B4
		public static int ManhattanDistanceFlat(IntVec3 a, IntVec3 b)
		{
			return Math.Abs(a.x - b.x) + Math.Abs(a.z - b.z);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00008EDB File Offset: 0x000070DB
		public static IntVec3 RandomHorizontalOffset(float maxDist)
		{
			return Vector3Utility.RandomHorizontalOffset(maxDist).ToIntVec3();
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00008EE8 File Offset: 0x000070E8
		public static int DistanceToEdge(this IntVec3 v, Map map)
		{
			return Mathf.Max(Mathf.Min(Mathf.Min(Mathf.Min(v.x, v.z), map.Size.x - v.x - 1), map.Size.z - v.z - 1), 0);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00008F40 File Offset: 0x00007140
		public static int Determinant(this IntVec3 p, IntVec3 p1, IntVec3 p2)
		{
			int num = (p2.x - p1.x) * (p.z - p1.z) - (p.x - p1.x) * (p2.z - p1.z);
			if (num > 0)
			{
				return -1;
			}
			if (num < 0)
			{
				return 1;
			}
			return 0;
		}
	}
}
