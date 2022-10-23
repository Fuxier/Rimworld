using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000567 RID: 1383
	public static class GenGeo
	{
		// Token: 0x06002A93 RID: 10899 RVA: 0x0010FE34 File Offset: 0x0010E034
		public static float AngleDifferenceBetween(float A, float B)
		{
			float num = A + 360f;
			float num2 = B + 360f;
			float num3 = 9999f;
			float num4 = A - B;
			if (num4 < 0f)
			{
				num4 *= -1f;
			}
			if (num4 < num3)
			{
				num3 = num4;
			}
			num4 = num - B;
			if (num4 < 0f)
			{
				num4 *= -1f;
			}
			if (num4 < num3)
			{
				num3 = num4;
			}
			num4 = A - num2;
			if (num4 < 0f)
			{
				num4 *= -1f;
			}
			if (num4 < num3)
			{
				num3 = num4;
			}
			return num3;
		}

		// Token: 0x06002A94 RID: 10900 RVA: 0x0010FEAA File Offset: 0x0010E0AA
		public static float MagnitudeHorizontal(this Vector3 v)
		{
			return (float)Math.Sqrt((double)(v.x * v.x + v.z * v.z));
		}

		// Token: 0x06002A95 RID: 10901 RVA: 0x0010FECE File Offset: 0x0010E0CE
		public static float MagnitudeHorizontalSquared(this Vector3 v)
		{
			return v.x * v.x + v.z * v.z;
		}

		// Token: 0x06002A96 RID: 10902 RVA: 0x0010FEEC File Offset: 0x0010E0EC
		public static bool LinesIntersect(Vector3 line1V1, Vector3 line1V2, Vector3 line2V1, Vector3 line2V2)
		{
			float num = line1V2.z - line1V1.z;
			float num2 = line1V1.x - line1V2.x;
			float num3 = num * line1V1.x + num2 * line1V1.z;
			float num4 = line2V2.z - line2V1.z;
			float num5 = line2V1.x - line2V2.x;
			float num6 = num4 * line2V1.x + num5 * line2V1.z;
			float num7 = num * num5 - num4 * num2;
			if (num7 == 0f)
			{
				return false;
			}
			float num8 = (num5 * num3 - num2 * num6) / num7;
			float num9 = (num * num6 - num4 * num3) / num7;
			return (num8 <= line1V1.x || num8 <= line1V2.x) && (num8 <= line2V1.x || num8 <= line2V2.x) && (num8 >= line1V1.x || num8 >= line1V2.x) && (num8 >= line2V1.x || num8 >= line2V2.x) && (num9 <= line1V1.z || num9 <= line1V2.z) && (num9 <= line2V1.z || num9 <= line2V2.z) && (num9 >= line1V1.z || num9 >= line1V2.z) && (num9 >= line2V1.z || num9 >= line2V2.z);
		}

		// Token: 0x06002A97 RID: 10903 RVA: 0x00110030 File Offset: 0x0010E230
		public static bool IntersectLineCircle(Vector2 center, float radius, Vector2 lineA, Vector2 lineB)
		{
			Vector2 lhs = center - lineA;
			Vector2 vector = lineB - lineA;
			float num = Vector2.Dot(vector, vector);
			float num2 = Vector2.Dot(lhs, vector) / num;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			else if (num2 > 1f)
			{
				num2 = 1f;
			}
			Vector2 vector2 = vector * num2 + lineA - center;
			return Vector2.Dot(vector2, vector2) <= radius * radius;
		}

		// Token: 0x06002A98 RID: 10904 RVA: 0x0011009C File Offset: 0x0010E29C
		public static bool IntersectLineCircleOutline(Vector2 center, float radius, Vector2 lineA, Vector2 lineB)
		{
			bool flag = (lineA - center).sqrMagnitude <= radius * radius;
			bool flag2 = (lineB - center).sqrMagnitude <= radius * radius;
			return (!flag || !flag2) && GenGeo.IntersectLineCircle(center, radius, lineA, lineB);
		}

		// Token: 0x06002A99 RID: 10905 RVA: 0x001100E8 File Offset: 0x0010E2E8
		public static Vector3 RegularPolygonVertexPositionVec3(int polygonVertices, int vertexIndex)
		{
			Vector2 vector = GenGeo.RegularPolygonVertexPosition(polygonVertices, vertexIndex, 0f);
			return new Vector3(vector.x, 0f, vector.y);
		}

		// Token: 0x06002A9A RID: 10906 RVA: 0x00110118 File Offset: 0x0010E318
		public static Vector2 RegularPolygonVertexPosition(int polygonVertices, int vertexIndex, float angleOffset = 0f)
		{
			if (vertexIndex < 0 || vertexIndex >= polygonVertices)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Vertex index out of bounds. polygonVertices=",
					polygonVertices,
					" vertexIndex=",
					vertexIndex
				}));
				return Vector2.zero;
			}
			if (polygonVertices == 1)
			{
				return Vector2.zero;
			}
			return GenGeo.CalculatePolygonVertexPosition(polygonVertices, vertexIndex, angleOffset);
		}

		// Token: 0x06002A9B RID: 10907 RVA: 0x00110178 File Offset: 0x0010E378
		private static Vector2 CalculatePolygonVertexPosition(int polygonVertices, int vertexIndex, float angleOffset = 0f)
		{
			float num = 6.2831855f / (float)polygonVertices * (float)vertexIndex;
			num += 3.1415927f;
			num += 0.017453292f * angleOffset;
			return new Vector3(Mathf.Cos(num), Mathf.Sin(num));
		}

		// Token: 0x06002A9C RID: 10908 RVA: 0x001101BC File Offset: 0x0010E3BC
		public static Vector2 InverseQuadBilinear(Vector2 p, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
		{
			float num = (p0 - p).Cross(p0 - p2);
			float num2 = ((p0 - p).Cross(p1 - p3) + (p1 - p).Cross(p0 - p2)) / 2f;
			float num3 = (p1 - p).Cross(p1 - p3);
			float num4 = num2 * num2 - num * num3;
			if (num4 < 0f)
			{
				return new Vector2(-1f, -1f);
			}
			num4 = Mathf.Sqrt(num4);
			float num5;
			if (Mathf.Abs(num - 2f * num2 + num3) < 0.0001f)
			{
				num5 = num / (num - num3);
			}
			else
			{
				float num6 = (num - num2 + num4) / (num - 2f * num2 + num3);
				float num7 = (num - num2 - num4) / (num - 2f * num2 + num3);
				if (Mathf.Abs(num6 - 0.5f) < Mathf.Abs(num7 - 0.5f))
				{
					num5 = num6;
				}
				else
				{
					num5 = num7;
				}
			}
			float num8 = (1f - num5) * (p0.x - p2.x) + num5 * (p1.x - p3.x);
			float num9 = (1f - num5) * (p0.y - p2.y) + num5 * (p1.y - p3.y);
			if (Mathf.Abs(num8) < Mathf.Abs(num9))
			{
				return new Vector2(num5, ((1f - num5) * (p0.y - p.y) + num5 * (p1.y - p.y)) / num9);
			}
			return new Vector2(num5, ((1f - num5) * (p0.x - p.x) + num5 * (p1.x - p.x)) / num8);
		}

		// Token: 0x06002A9D RID: 10909 RVA: 0x0011037C File Offset: 0x0010E57C
		public static int GetAdjacencyScore(this CellRect rect, CellRect other)
		{
			if (rect.Overlaps(other))
			{
				return 0;
			}
			if (rect.maxZ == other.minZ - 1 && rect.minX < other.maxX && rect.maxX > other.minX)
			{
				int num = Mathf.Max(rect.minX, other.minX);
				return Mathf.Min(rect.maxX, other.maxX) - num;
			}
			if (rect.minZ == other.maxZ + 1 && rect.minX < other.maxX && rect.maxX > other.minX)
			{
				int num2 = Mathf.Max(rect.minX, other.minX);
				return Mathf.Min(rect.maxX, other.maxX) - num2;
			}
			if (rect.minX == other.maxX + 1 && rect.minZ < other.maxZ && rect.maxZ > other.minZ)
			{
				int num3 = Mathf.Max(rect.minZ, other.minZ);
				return Mathf.Min(rect.maxZ, other.maxZ) - num3;
			}
			if (rect.maxX == other.minX - 1 && rect.minZ < other.maxZ && rect.maxZ > other.minZ)
			{
				int num4 = Mathf.Max(rect.minZ, other.minZ);
				return Mathf.Min(rect.maxZ, other.maxZ) - num4;
			}
			return 0;
		}

		// Token: 0x06002A9E RID: 10910 RVA: 0x001104E0 File Offset: 0x0010E6E0
		public static CellRect ExpandToFit(this CellRect rect, IntVec3 position)
		{
			if (rect.Contains(position))
			{
				return rect;
			}
			rect.minX = Mathf.Min(rect.minX, position.x);
			rect.minZ = Mathf.Min(rect.minZ, position.z);
			rect.maxX = Mathf.Max(rect.maxX, position.x);
			rect.maxZ = Mathf.Max(rect.maxZ, position.z);
			return rect;
		}
	}
}
