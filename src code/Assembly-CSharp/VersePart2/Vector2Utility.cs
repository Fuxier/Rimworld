using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000027 RID: 39
	public static class Vector2Utility
	{
		// Token: 0x0600019E RID: 414 RVA: 0x00009176 File Offset: 0x00007376
		public static Vector2 Rotated(this Vector2 v)
		{
			return new Vector2(v.y, v.x);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00009189 File Offset: 0x00007389
		public static Vector2 RotatedBy(this Vector2 v, Rot4 rot)
		{
			return v.RotatedBy(rot.AsAngle);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00009198 File Offset: 0x00007398
		public static Vector2 RotatedBy(this Vector2 v, float degrees)
		{
			float num = Mathf.Sin(degrees * 0.017453292f);
			float num2 = Mathf.Cos(degrees * 0.017453292f);
			return new Vector2(num2 * v.x - num * v.y, num * v.x + num2 * v.y);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x000091E6 File Offset: 0x000073E6
		public static float AngleTo(this Vector2 a, Vector2 b)
		{
			return Mathf.Atan2(-(b.y - a.y), b.x - a.x) * 57.29578f;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000920E File Offset: 0x0000740E
		public static Vector3 ScaledBy(this Vector3 a, Vector3 b)
		{
			return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000923C File Offset: 0x0000743C
		public static Vector2 Moved(this Vector2 v, float angle, float distance)
		{
			return new Vector2(v.x + Mathf.Cos(angle * 0.017453292f) * distance, v.y - Mathf.Sin(angle * 0.017453292f) * distance);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000926D File Offset: 0x0000746D
		public static Vector2 FromAngle(float angle)
		{
			return new Vector2(Mathf.Cos(angle * 0.017453292f), -Mathf.Sin(angle * 0.017453292f));
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000928D File Offset: 0x0000748D
		public static float ToAngle(this Vector2 v)
		{
			return Mathf.Atan2(-v.y, v.x) * 57.29578f;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x000092A7 File Offset: 0x000074A7
		public static float Cross(this Vector2 u, Vector2 v)
		{
			return u.x * v.y - u.y * v.x;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x000092C4 File Offset: 0x000074C4
		public static float DistanceToRect(this Vector2 u, Rect rect)
		{
			if (rect.Contains(u))
			{
				return 0f;
			}
			if (u.x < rect.xMin && u.y < rect.yMin)
			{
				return Vector2.Distance(u, new Vector2(rect.xMin, rect.yMin));
			}
			if (u.x > rect.xMax && u.y < rect.yMin)
			{
				return Vector2.Distance(u, new Vector2(rect.xMax, rect.yMin));
			}
			if (u.x < rect.xMin && u.y > rect.yMax)
			{
				return Vector2.Distance(u, new Vector2(rect.xMin, rect.yMax));
			}
			if (u.x > rect.xMax && u.y > rect.yMax)
			{
				return Vector2.Distance(u, new Vector2(rect.xMax, rect.yMax));
			}
			if (u.x < rect.xMin)
			{
				return rect.xMin - u.x;
			}
			if (u.x > rect.xMax)
			{
				return u.x - rect.xMax;
			}
			if (u.y < rect.yMin)
			{
				return rect.yMin - u.y;
			}
			return u.y - rect.yMax;
		}
	}
}
