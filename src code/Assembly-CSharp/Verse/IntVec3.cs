using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000024 RID: 36
	public struct IntVec3 : IEquatable<IntVec3>
	{
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000166 RID: 358 RVA: 0x0000862F File Offset: 0x0000682F
		public IntVec2 ToIntVec2
		{
			get
			{
				return new IntVec2(this.x, this.z);
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00008642 File Offset: 0x00006842
		public bool IsValid
		{
			get
			{
				return this.y >= 0;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00008650 File Offset: 0x00006850
		public int LengthHorizontalSquared
		{
			get
			{
				return this.x * this.x + this.z * this.z;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000169 RID: 361 RVA: 0x0000866D File Offset: 0x0000686D
		public float LengthHorizontal
		{
			get
			{
				return GenMath.Sqrt((float)(this.x * this.x + this.z * this.z));
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00008690 File Offset: 0x00006890
		public int LengthManhattan
		{
			get
			{
				return ((this.x >= 0) ? this.x : (-this.x)) + ((this.z >= 0) ? this.z : (-this.z));
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600016B RID: 363 RVA: 0x000086C4 File Offset: 0x000068C4
		public float AngleFlat
		{
			get
			{
				if (this.x == 0 && this.z == 0)
				{
					return 0f;
				}
				return Quaternion.LookRotation(this.ToVector3()).eulerAngles.y;
			}
		}

		// Token: 0x0600016C RID: 364 RVA: 0x000086FF File Offset: 0x000068FF
		public IntVec3(int newX, int newY, int newZ)
		{
			this.x = newX;
			this.y = newY;
			this.z = newZ;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00008716 File Offset: 0x00006916
		public IntVec3(Vector3 v)
		{
			this.x = (int)v.x;
			this.y = 0;
			this.z = (int)v.z;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00008739 File Offset: 0x00006939
		public IntVec3(Vector2 v)
		{
			this.x = (int)v.x;
			this.y = 0;
			this.z = (int)v.y;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000875C File Offset: 0x0000695C
		public static IntVec3 FromString(string str)
		{
			str = str.TrimStart(new char[]
			{
				'('
			});
			str = str.TrimEnd(new char[]
			{
				')'
			});
			string[] array = str.Split(new char[]
			{
				','
			});
			IntVec3 result;
			try
			{
				CultureInfo invariantCulture = CultureInfo.InvariantCulture;
				int newX = Convert.ToInt32(array[0], invariantCulture);
				int newY = Convert.ToInt32(array[1], invariantCulture);
				int newZ = Convert.ToInt32(array[2], invariantCulture);
				result = new IntVec3(newX, newY, newZ);
			}
			catch (Exception arg)
			{
				Log.Warning(str + " is not a valid IntVec3 format. Exception: " + arg);
				result = IntVec3.Invalid;
			}
			return result;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00008804 File Offset: 0x00006A04
		public Vector3 ToVector3()
		{
			return new Vector3((float)this.x, (float)this.y, (float)this.z);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00008820 File Offset: 0x00006A20
		public Vector3 ToVector3Shifted()
		{
			return new Vector3((float)this.x + 0.5f, (float)this.y, (float)this.z + 0.5f);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00008848 File Offset: 0x00006A48
		public Vector3 ToVector3ShiftedWithAltitude(AltitudeLayer AltLayer)
		{
			return this.ToVector3ShiftedWithAltitude(AltLayer.AltitudeFor());
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00008856 File Offset: 0x00006A56
		public Vector3 ToVector3ShiftedWithAltitude(float AddedAltitude)
		{
			return new Vector3((float)this.x + 0.5f, (float)this.y + AddedAltitude, (float)this.z + 0.5f);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00008880 File Offset: 0x00006A80
		public bool InHorDistOf(IntVec3 otherLoc, float maxDist)
		{
			float num = (float)(this.x - otherLoc.x);
			float num2 = (float)(this.z - otherLoc.z);
			return num * num + num2 * num2 <= maxDist * maxDist;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x000088B8 File Offset: 0x00006AB8
		public static IntVec3 FromVector3(Vector3 v)
		{
			return IntVec3.FromVector3(v, 0);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000088C1 File Offset: 0x00006AC1
		public static IntVec3 FromVector3(Vector3 v, int newY)
		{
			return new IntVec3((int)v.x, newY, (int)v.z);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x000088D7 File Offset: 0x00006AD7
		public Vector2 ToUIPosition()
		{
			return this.ToVector3Shifted().MapToUIPosition();
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000088E4 File Offset: 0x00006AE4
		public Rect ToUIRect()
		{
			Vector2 vector = this.ToVector3().MapToUIPosition();
			Vector2 vector2 = (this + IntVec3.NorthEast).ToVector3().MapToUIPosition();
			return new Rect(vector.x, vector2.y, vector2.x - vector.x, vector.y - vector2.y);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00008948 File Offset: 0x00006B48
		public bool AdjacentToCardinal(IntVec3 other)
		{
			return this.IsValid && ((other.z == this.z && (other.x == this.x + 1 || other.x == this.x - 1)) || (other.x == this.x && (other.z == this.z + 1 || other.z == this.z - 1)));
		}

		// Token: 0x0600017A RID: 378 RVA: 0x000089C0 File Offset: 0x00006BC0
		public bool AdjacentToDiagonal(IntVec3 other)
		{
			return this.IsValid && Mathf.Abs(this.x - other.x) == 1 && Mathf.Abs(this.z - other.z) == 1;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x000089F8 File Offset: 0x00006BF8
		public bool AdjacentToCardinal(District district)
		{
			if (!this.IsValid)
			{
				return false;
			}
			Map map = district.Map;
			if (this.InBounds(map) && this.GetDistrict(map, RegionType.Set_All) == district)
			{
				return true;
			}
			IntVec3[] cardinalDirections = GenAdj.CardinalDirections;
			for (int i = 0; i < cardinalDirections.Length; i++)
			{
				IntVec3 intVec = this + cardinalDirections[i];
				if (intVec.InBounds(map) && intVec.GetDistrict(map, RegionType.Set_All) == district)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00008A76 File Offset: 0x00006C76
		public IntVec3 ClampInsideMap(Map map)
		{
			return this.ClampInsideRect(CellRect.WholeMap(map));
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00008A84 File Offset: 0x00006C84
		public IntVec3 ClampInsideRect(CellRect rect)
		{
			this.x = Mathf.Clamp(this.x, rect.minX, rect.maxX);
			this.y = 0;
			this.z = Mathf.Clamp(this.z, rect.minZ, rect.maxZ);
			return this;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00008AD8 File Offset: 0x00006CD8
		public static IntVec3 operator +(IntVec3 a, IntVec3 b)
		{
			return new IntVec3(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00008B06 File Offset: 0x00006D06
		public static IntVec3 operator -(IntVec3 a, IntVec3 b)
		{
			return new IntVec3(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00008B34 File Offset: 0x00006D34
		public static IntVec3 operator *(IntVec3 a, int i)
		{
			return new IntVec3(a.x * i, a.y * i, a.z * i);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00008B53 File Offset: 0x00006D53
		public static IntVec3 operator *(int i, IntVec3 a)
		{
			return new IntVec3(a.x * i, a.y * i, a.z * i);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00008B72 File Offset: 0x00006D72
		public static bool operator ==(IntVec3 a, IntVec3 b)
		{
			return a.x == b.x && a.z == b.z && a.y == b.y;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00008BA1 File Offset: 0x00006DA1
		public static bool operator !=(IntVec3 a, IntVec3 b)
		{
			return a.x != b.x || a.z != b.z || a.y != b.y;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00008BD0 File Offset: 0x00006DD0
		public override bool Equals(object obj)
		{
			return obj is IntVec3 && this.Equals((IntVec3)obj);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00008BE8 File Offset: 0x00006DE8
		public bool Equals(IntVec3 other)
		{
			return this.x == other.x && this.z == other.z && this.y == other.y;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00008C16 File Offset: 0x00006E16
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(Gen.HashCombineInt(Gen.HashCombineInt(0, this.x), this.y), this.z);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00008C3A File Offset: 0x00006E3A
		public ulong UniqueHashCode()
		{
			return (ulong)(0L + (long)this.x + 4096L * (long)this.z + 16777216L * (long)this.y);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00008C64 File Offset: 0x00006E64
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.x.ToString(),
				", ",
				this.y.ToString(),
				", ",
				this.z.ToString(),
				")"
			});
		}

		// Token: 0x0400005E RID: 94
		public int x;

		// Token: 0x0400005F RID: 95
		public int y;

		// Token: 0x04000060 RID: 96
		public int z;

		// Token: 0x04000061 RID: 97
		public static readonly IntVec3 Zero = new IntVec3(0, 0, 0);

		// Token: 0x04000062 RID: 98
		public static readonly IntVec3 North = new IntVec3(0, 0, 1);

		// Token: 0x04000063 RID: 99
		public static readonly IntVec3 East = new IntVec3(1, 0, 0);

		// Token: 0x04000064 RID: 100
		public static readonly IntVec3 South = new IntVec3(0, 0, -1);

		// Token: 0x04000065 RID: 101
		public static readonly IntVec3 West = new IntVec3(-1, 0, 0);

		// Token: 0x04000066 RID: 102
		public static readonly IntVec3 NorthWest = new IntVec3(-1, 0, 1);

		// Token: 0x04000067 RID: 103
		public static readonly IntVec3 NorthEast = new IntVec3(1, 0, 1);

		// Token: 0x04000068 RID: 104
		public static readonly IntVec3 SouthWest = new IntVec3(-1, 0, -1);

		// Token: 0x04000069 RID: 105
		public static readonly IntVec3 SouthEast = new IntVec3(1, 0, -1);

		// Token: 0x0400006A RID: 106
		public static readonly IntVec3 Invalid = new IntVec3(-1000, -1000, -1000);
	}
}
