using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000023 RID: 35
	public struct IntVec2 : IEquatable<IntVec2>
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000146 RID: 326 RVA: 0x000082F2 File Offset: 0x000064F2
		public bool IsInvalid
		{
			get
			{
				return this.x < -500;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00008301 File Offset: 0x00006501
		public bool IsValid
		{
			get
			{
				return this.x >= -500;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000148 RID: 328 RVA: 0x00008313 File Offset: 0x00006513
		public static IntVec2 Zero
		{
			get
			{
				return new IntVec2(0, 0);
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000149 RID: 329 RVA: 0x0000831C File Offset: 0x0000651C
		public static IntVec2 One
		{
			get
			{
				return new IntVec2(1, 1);
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600014A RID: 330 RVA: 0x00008325 File Offset: 0x00006525
		public static IntVec2 Two
		{
			get
			{
				return new IntVec2(2, 2);
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600014B RID: 331 RVA: 0x0000832E File Offset: 0x0000652E
		public static IntVec2 North
		{
			get
			{
				return new IntVec2(0, 1);
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00008337 File Offset: 0x00006537
		public static IntVec2 East
		{
			get
			{
				return new IntVec2(1, 0);
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00008340 File Offset: 0x00006540
		public static IntVec2 South
		{
			get
			{
				return new IntVec2(0, -1);
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00008349 File Offset: 0x00006549
		public static IntVec2 West
		{
			get
			{
				return new IntVec2(-1, 0);
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00008352 File Offset: 0x00006552
		public float Magnitude
		{
			get
			{
				return Mathf.Sqrt((float)(this.x * this.x + this.z * this.z));
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00008375 File Offset: 0x00006575
		public int MagnitudeManhattan
		{
			get
			{
				return Mathf.Abs(this.x) + Mathf.Abs(this.z);
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000151 RID: 337 RVA: 0x0000838E File Offset: 0x0000658E
		public int Area
		{
			get
			{
				return Mathf.Abs(this.x) * Mathf.Abs(this.z);
			}
		}

		// Token: 0x06000152 RID: 338 RVA: 0x000083A7 File Offset: 0x000065A7
		public IntVec2(int newX, int newZ)
		{
			this.x = newX;
			this.z = newZ;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x000083B7 File Offset: 0x000065B7
		public IntVec2(Vector2 v2)
		{
			this.x = (int)v2.x;
			this.z = (int)v2.y;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x000083D3 File Offset: 0x000065D3
		public Vector2 ToVector2()
		{
			return new Vector2((float)this.x, (float)this.z);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x000083E8 File Offset: 0x000065E8
		public Vector3 ToVector3()
		{
			return new Vector3((float)this.x, 0f, (float)this.z);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00008402 File Offset: 0x00006602
		public IntVec2 Rotated()
		{
			return new IntVec2(this.z, this.x);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00008418 File Offset: 0x00006618
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.x.ToString(),
				", ",
				this.z.ToString(),
				")"
			});
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00008464 File Offset: 0x00006664
		public string ToStringCross()
		{
			return this.x.ToString() + " x " + this.z.ToString();
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00008488 File Offset: 0x00006688
		public static IntVec2 FromString(string str)
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
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			int newX = Convert.ToInt32(array[0], invariantCulture);
			int newZ = Convert.ToInt32(array[1], invariantCulture);
			return new IntVec2(newX, newZ);
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600015A RID: 346 RVA: 0x000084EC File Offset: 0x000066EC
		public static IntVec2 Invalid
		{
			get
			{
				return new IntVec2(-1000, -1000);
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x000084FD File Offset: 0x000066FD
		public Vector2 ToVector2Shifted()
		{
			return new Vector2((float)this.x + 0.5f, (float)this.z + 0.5f);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000851E File Offset: 0x0000671E
		public static IntVec2 operator +(IntVec2 a, IntVec2 b)
		{
			return new IntVec2(a.x + b.x, a.z + b.z);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000853F File Offset: 0x0000673F
		public static IntVec2 operator -(IntVec2 a, IntVec2 b)
		{
			return new IntVec2(a.x - b.x, a.z - b.z);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00008560 File Offset: 0x00006760
		public static IntVec2 operator *(IntVec2 a, int b)
		{
			return new IntVec2(a.x * b, a.z * b);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00008577 File Offset: 0x00006777
		public static IntVec2 operator /(IntVec2 a, int b)
		{
			return new IntVec2(a.x / b, a.z / b);
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000160 RID: 352 RVA: 0x0000858E File Offset: 0x0000678E
		public IntVec3 ToIntVec3
		{
			get
			{
				return new IntVec3(this.x, 0, this.z);
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x000085A2 File Offset: 0x000067A2
		public static bool operator ==(IntVec2 a, IntVec2 b)
		{
			return a.x == b.x && a.z == b.z;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x000085C3 File Offset: 0x000067C3
		public static bool operator !=(IntVec2 a, IntVec2 b)
		{
			return a.x != b.x || a.z != b.z;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x000085E4 File Offset: 0x000067E4
		public override bool Equals(object obj)
		{
			return obj is IntVec2 && this.Equals((IntVec2)obj);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x000085FC File Offset: 0x000067FC
		public bool Equals(IntVec2 other)
		{
			return this.x == other.x && this.z == other.z;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000861C File Offset: 0x0000681C
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.x, this.z);
		}

		// Token: 0x0400005C RID: 92
		public int x;

		// Token: 0x0400005D RID: 93
		public int z;
	}
}
