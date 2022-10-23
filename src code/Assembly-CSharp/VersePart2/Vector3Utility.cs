using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000026 RID: 38
	public static class Vector3Utility
	{
		// Token: 0x06000194 RID: 404 RVA: 0x00008F92 File Offset: 0x00007192
		public static Vector3 HorizontalVectorFromAngle(float angle)
		{
			return Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00008FAC File Offset: 0x000071AC
		public static float AngleFlat(this Vector3 v)
		{
			if (v.x == 0f && v.z == 0f)
			{
				return 0f;
			}
			return Quaternion.LookRotation(v).eulerAngles.y;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00008FEC File Offset: 0x000071EC
		public static Vector3 RandomHorizontalOffset(float maxDist)
		{
			float d = Rand.Range(0f, maxDist);
			float y = (float)Rand.Range(0, 360);
			return Quaternion.Euler(new Vector3(0f, y, 0f)) * Vector3.forward * d;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00009037 File Offset: 0x00007237
		public static Vector3 Yto0(this Vector3 v3)
		{
			return new Vector3(v3.x, 0f, v3.z);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000904F File Offset: 0x0000724F
		public static Vector3 RotatedBy(this Vector3 v3, float angle)
		{
			return Quaternion.AngleAxis(angle, Vector3.up) * v3;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00009064 File Offset: 0x00007264
		public static Vector3 RotatedBy(this Vector3 orig, Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
				return orig;
			case 1:
				return new Vector3(orig.z, orig.y, -orig.x);
			case 2:
				return new Vector3(-orig.x, orig.y, -orig.z);
			case 3:
				return new Vector3(-orig.z, orig.y, orig.x);
			default:
				return orig;
			}
		}

		// Token: 0x0600019A RID: 410 RVA: 0x000090E0 File Offset: 0x000072E0
		public static float AngleToFlat(this Vector3 a, Vector3 b)
		{
			return new Vector2(a.x, a.z).AngleTo(new Vector2(b.x, b.z));
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000910C File Offset: 0x0000730C
		public static Vector3 FromAngleFlat(float angle)
		{
			Vector2 vector = Vector2Utility.FromAngle(angle);
			return new Vector3(vector.x, 0f, vector.y);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00009136 File Offset: 0x00007336
		public static float ToAngleFlat(this Vector3 v)
		{
			return new Vector2(v.x, v.z).ToAngle();
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000914E File Offset: 0x0000734E
		public static Vector3 Abs(this Vector3 v)
		{
			return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
		}
	}
}
