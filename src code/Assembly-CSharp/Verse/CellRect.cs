using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200001D RID: 29
	public struct CellRect : IEquatable<CellRect>, IEnumerable<IntVec3>, IEnumerable
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00006544 File Offset: 0x00004744
		public static CellRect Empty
		{
			get
			{
				return new CellRect(0, 0, 0, 0);
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000BC RID: 188 RVA: 0x0000654F File Offset: 0x0000474F
		public bool IsEmpty
		{
			get
			{
				return this.Width <= 0 || this.Height <= 0;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00006568 File Offset: 0x00004768
		public int Area
		{
			get
			{
				return this.Width * this.Height;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00006577 File Offset: 0x00004777
		// (set) Token: 0x060000BF RID: 191 RVA: 0x00006598 File Offset: 0x00004798
		public int Width
		{
			get
			{
				if (this.minX > this.maxX)
				{
					return 0;
				}
				return this.maxX - this.minX + 1;
			}
			set
			{
				this.maxX = this.minX + Mathf.Max(value, 0) - 1;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x000065B0 File Offset: 0x000047B0
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x000065D1 File Offset: 0x000047D1
		public int Height
		{
			get
			{
				if (this.minZ > this.maxZ)
				{
					return 0;
				}
				return this.maxZ - this.minZ + 1;
			}
			set
			{
				this.maxZ = this.minZ + Mathf.Max(value, 0) - 1;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x000065E9 File Offset: 0x000047E9
		public IEnumerable<IntVec3> Corners
		{
			get
			{
				if (this.IsEmpty)
				{
					yield break;
				}
				yield return new IntVec3(this.minX, 0, this.minZ);
				if (this.Width > 1)
				{
					yield return new IntVec3(this.maxX, 0, this.minZ);
				}
				if (this.Height > 1)
				{
					yield return new IntVec3(this.minX, 0, this.maxZ);
					if (this.Width > 1)
					{
						yield return new IntVec3(this.maxX, 0, this.maxZ);
					}
				}
				yield break;
			}
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000065FE File Offset: 0x000047FE
		[Obsolete("Use foreach on the cellrect instead")]
		public CellRect.CellRectIterator GetIterator()
		{
			return new CellRect.CellRectIterator(this);
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x0000660B File Offset: 0x0000480B
		public IntVec3 BottomLeft
		{
			get
			{
				return new IntVec3(this.minX, 0, this.minZ);
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x0000661F File Offset: 0x0000481F
		public IntVec3 BottomRight
		{
			get
			{
				return new IntVec3(this.maxX, 0, this.minZ);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00006633 File Offset: 0x00004833
		public IntVec3 TopRight
		{
			get
			{
				return new IntVec3(this.maxX, 0, this.maxZ);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00006647 File Offset: 0x00004847
		public IntVec3 TopLeft
		{
			get
			{
				return new IntVec3(this.minX, 0, this.maxZ);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x0000665B File Offset: 0x0000485B
		public IntVec3 RandomCell
		{
			get
			{
				return new IntVec3(Rand.RangeInclusive(this.minX, this.maxX), 0, Rand.RangeInclusive(this.minZ, this.maxZ));
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00006685 File Offset: 0x00004885
		public IntVec3 CenterCell
		{
			get
			{
				return new IntVec3(this.minX + this.Width / 2, 0, this.minZ + this.Height / 2);
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000CA RID: 202 RVA: 0x000066AB File Offset: 0x000048AB
		public Vector3 CenterVector3
		{
			get
			{
				return new Vector3((float)this.minX + (float)this.Width / 2f, 0f, (float)this.minZ + (float)this.Height / 2f);
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000CB RID: 203 RVA: 0x000066E1 File Offset: 0x000048E1
		public Vector3 RandomVector3
		{
			get
			{
				return new Vector3(Rand.Range((float)this.minX, (float)this.maxX + 1f), 0f, Rand.Range((float)this.minZ, (float)this.maxZ + 1f));
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000CC RID: 204 RVA: 0x0000671F File Offset: 0x0000491F
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				int num;
				for (int z = this.minZ; z <= this.maxZ; z = num + 1)
				{
					for (int x = this.minX; x <= this.maxX; x = num + 1)
					{
						yield return new IntVec3(x, 0, z);
						num = x;
					}
					num = z;
				}
				yield break;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000CD RID: 205 RVA: 0x00006734 File Offset: 0x00004934
		public IEnumerable<IntVec2> Cells2D
		{
			get
			{
				int num;
				for (int z = this.minZ; z <= this.maxZ; z = num + 1)
				{
					for (int x = this.minX; x <= this.maxX; x = num + 1)
					{
						yield return new IntVec2(x, z);
						num = x;
					}
					num = z;
				}
				yield break;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00006749 File Offset: 0x00004949
		public IEnumerable<IntVec3> EdgeCells
		{
			get
			{
				if (this.IsEmpty)
				{
					yield break;
				}
				int x = this.minX;
				int z = this.minZ;
				int num;
				while (x <= this.maxX)
				{
					yield return new IntVec3(x, 0, z);
					num = x;
					x = num + 1;
				}
				num = x;
				x = num - 1;
				num = z;
				for (z = num + 1; z <= this.maxZ; z = num + 1)
				{
					yield return new IntVec3(x, 0, z);
					num = z;
				}
				num = z;
				z = num - 1;
				num = x;
				for (x = num - 1; x >= this.minX; x = num - 1)
				{
					yield return new IntVec3(x, 0, z);
					num = x;
				}
				num = x;
				x = num + 1;
				num = z;
				for (z = num - 1; z > this.minZ; z = num - 1)
				{
					yield return new IntVec3(x, 0, z);
					num = z;
				}
				yield break;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000CF RID: 207 RVA: 0x0000675E File Offset: 0x0000495E
		public int EdgeCellsCount
		{
			get
			{
				if (this.Area == 0)
				{
					return 0;
				}
				if (this.Area == 1)
				{
					return 1;
				}
				return this.Width * 2 + (this.Height - 2) * 2;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00006788 File Offset: 0x00004988
		public IEnumerable<IntVec3> AdjacentCellsCardinal
		{
			get
			{
				if (this.IsEmpty)
				{
					yield break;
				}
				int num;
				for (int x = this.minX; x <= this.maxX; x = num + 1)
				{
					yield return new IntVec3(x, 0, this.minZ - 1);
					yield return new IntVec3(x, 0, this.maxZ + 1);
					num = x;
				}
				for (int x = this.minZ; x <= this.maxZ; x = num + 1)
				{
					yield return new IntVec3(this.minX - 1, 0, x);
					yield return new IntVec3(this.maxX + 1, 0, x);
					num = x;
				}
				yield break;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x0000679D File Offset: 0x0000499D
		public IEnumerable<IntVec3> AdjacentCells
		{
			get
			{
				if (this.IsEmpty)
				{
					yield break;
				}
				foreach (IntVec3 intVec in this.AdjacentCellsCardinal)
				{
					yield return intVec;
				}
				IEnumerator<IntVec3> enumerator = null;
				yield return new IntVec3(this.minX - 1, 0, this.minZ - 1);
				yield return new IntVec3(this.maxX + 1, 0, this.minZ - 1);
				yield return new IntVec3(this.minX - 1, 0, this.maxZ + 1);
				yield return new IntVec3(this.maxX + 1, 0, this.maxZ + 1);
				yield break;
				yield break;
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000067B4 File Offset: 0x000049B4
		public IntVec3 FarthestPoint(IntVec3 startingPoint, Rot4 direction)
		{
			if (!this.Contains(startingPoint))
			{
				throw new ArgumentException(startingPoint.ToString());
			}
			switch (direction.AsInt)
			{
			case 0:
				return new IntVec3(startingPoint.x, 0, this.maxZ);
			case 1:
				return new IntVec3(this.maxX, 0, startingPoint.z);
			case 2:
				return new IntVec3(startingPoint.x, 0, this.minZ);
			case 3:
				return new IntVec3(this.minX, 0, startingPoint.z);
			default:
				throw new ArgumentException(direction.ToString());
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000685B File Offset: 0x00004A5B
		public static bool operator ==(CellRect lhs, CellRect rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00006865 File Offset: 0x00004A65
		public static bool operator !=(CellRect lhs, CellRect rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00006871 File Offset: 0x00004A71
		public CellRect(int minX, int minZ, int width, int height)
		{
			this.minX = minX;
			this.minZ = minZ;
			this.maxX = minX + width - 1;
			this.maxZ = minZ + height - 1;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00006898 File Offset: 0x00004A98
		public static CellRect WholeMap(Map map)
		{
			return new CellRect(0, 0, map.Size.x, map.Size.z);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x000068B8 File Offset: 0x00004AB8
		public static CellRect FromLimits(int minX, int minZ, int maxX, int maxZ)
		{
			return new CellRect
			{
				minX = Mathf.Min(minX, maxX),
				minZ = Mathf.Min(minZ, maxZ),
				maxX = Mathf.Max(maxX, minX),
				maxZ = Mathf.Max(maxZ, minZ)
			};
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00006908 File Offset: 0x00004B08
		public static CellRect FromLimits(IntVec3 first, IntVec3 second)
		{
			return new CellRect
			{
				minX = Mathf.Min(first.x, second.x),
				minZ = Mathf.Min(first.z, second.z),
				maxX = Mathf.Max(first.x, second.x),
				maxZ = Mathf.Max(first.z, second.z)
			};
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00006980 File Offset: 0x00004B80
		public static CellRect CenteredOn(IntVec3 center, int radius)
		{
			return new CellRect
			{
				minX = center.x - radius,
				maxX = center.x + radius,
				minZ = center.z - radius,
				maxZ = center.z + radius
			};
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000069D4 File Offset: 0x00004BD4
		public static CellRect CenteredOn(IntVec3 center, int width, int height)
		{
			CellRect cellRect = default(CellRect);
			cellRect.minX = center.x - width / 2;
			cellRect.minZ = center.z - height / 2;
			cellRect.maxX = cellRect.minX + width - 1;
			cellRect.maxZ = cellRect.minZ + height - 1;
			return cellRect;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00006A2E File Offset: 0x00004C2E
		public static CellRect ViewRect(Map map)
		{
			if (Current.ProgramState != ProgramState.Playing || Find.CurrentMap != map || WorldRendererUtility.WorldRenderedNow)
			{
				return CellRect.Empty;
			}
			return Find.CameraDriver.CurrentViewRect;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00006A57 File Offset: 0x00004C57
		public static CellRect SingleCell(IntVec3 c)
		{
			return new CellRect(c.x, c.z, 1, 1);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00006A6C File Offset: 0x00004C6C
		public bool InBounds(Map map)
		{
			return this.minX >= 0 && this.minZ >= 0 && this.maxX < map.Size.x && this.maxZ < map.Size.z;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00006AA8 File Offset: 0x00004CA8
		public bool FullyContainedWithin(CellRect within)
		{
			CellRect rhs = this;
			rhs.ClipInsideRect(within);
			return this == rhs;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00006AD4 File Offset: 0x00004CD4
		public bool Overlaps(CellRect other)
		{
			return !this.IsEmpty && !other.IsEmpty && (this.minX <= other.maxX && this.maxX >= other.minX && this.maxZ >= other.minZ) && this.minZ <= other.maxZ;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00006B34 File Offset: 0x00004D34
		public bool IsOnEdge(IntVec3 c)
		{
			return (c.x == this.minX && c.z >= this.minZ && c.z <= this.maxZ) || (c.x == this.maxX && c.z >= this.minZ && c.z <= this.maxZ) || (c.z == this.minZ && c.x >= this.minX && c.x <= this.maxX) || (c.z == this.maxZ && c.x >= this.minX && c.x <= this.maxX);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00006BF4 File Offset: 0x00004DF4
		public bool IsOnEdge(IntVec3 c, Rot4 rot)
		{
			if (rot == Rot4.West)
			{
				return c.x == this.minX && c.z >= this.minZ && c.z <= this.maxZ;
			}
			if (rot == Rot4.East)
			{
				return c.x == this.maxX && c.z >= this.minZ && c.z <= this.maxZ;
			}
			if (rot == Rot4.South)
			{
				return c.z == this.minZ && c.x >= this.minX && c.x <= this.maxX;
			}
			return c.z == this.maxZ && c.x >= this.minX && c.x <= this.maxX;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00006CE8 File Offset: 0x00004EE8
		public bool IsOnEdge(IntVec3 c, int edgeWidth)
		{
			return this.Contains(c) && (c.x < this.minX + edgeWidth || c.z < this.minZ + edgeWidth || c.x >= this.maxX + 1 - edgeWidth || c.z >= this.maxZ + 1 - edgeWidth);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00006D4C File Offset: 0x00004F4C
		public bool IsCorner(IntVec3 c)
		{
			return (c.x == this.minX && c.z == this.minZ) || (c.x == this.maxX && c.z == this.minZ) || (c.x == this.minX && c.z == this.maxZ) || (c.x == this.maxX && c.z == this.maxZ);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00006DD0 File Offset: 0x00004FD0
		public Rot4 GetClosestEdge(IntVec3 c)
		{
			int num = Mathf.Abs(c.x - this.minX);
			int num2 = Mathf.Abs(c.x - this.maxX);
			int num3 = Mathf.Abs(c.z - this.maxZ);
			int num4 = Mathf.Abs(c.z - this.minZ);
			return GenMath.MinBy<Rot4>(Rot4.West, (float)num, Rot4.East, (float)num2, Rot4.North, (float)num3, Rot4.South, (float)num4);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00006E4C File Offset: 0x0000504C
		public CellRect ClipInsideMap(Map map)
		{
			if (this.minX < 0)
			{
				this.minX = 0;
			}
			if (this.minZ < 0)
			{
				this.minZ = 0;
			}
			if (this.maxX > map.Size.x - 1)
			{
				this.maxX = map.Size.x - 1;
			}
			if (this.maxZ > map.Size.z - 1)
			{
				this.maxZ = map.Size.z - 1;
			}
			return this;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00006ED0 File Offset: 0x000050D0
		public CellRect ClipInsideRect(CellRect otherRect)
		{
			if (this.minX < otherRect.minX)
			{
				this.minX = otherRect.minX;
			}
			if (this.maxX > otherRect.maxX)
			{
				this.maxX = otherRect.maxX;
			}
			if (this.minZ < otherRect.minZ)
			{
				this.minZ = otherRect.minZ;
			}
			if (this.maxZ > otherRect.maxZ)
			{
				this.maxZ = otherRect.maxZ;
			}
			return this;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00006F4B File Offset: 0x0000514B
		public bool Contains(IntVec3 c)
		{
			return c.x >= this.minX && c.x <= this.maxX && c.z >= this.minZ && c.z <= this.maxZ;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00006F8C File Offset: 0x0000518C
		public float ClosestDistSquaredTo(IntVec3 c)
		{
			if (this.Contains(c))
			{
				return 0f;
			}
			if (c.x < this.minX)
			{
				if (c.z < this.minZ)
				{
					return (float)(c - new IntVec3(this.minX, 0, this.minZ)).LengthHorizontalSquared;
				}
				if (c.z > this.maxZ)
				{
					return (float)(c - new IntVec3(this.minX, 0, this.maxZ)).LengthHorizontalSquared;
				}
				return (float)((this.minX - c.x) * (this.minX - c.x));
			}
			else if (c.x > this.maxX)
			{
				if (c.z < this.minZ)
				{
					return (float)(c - new IntVec3(this.maxX, 0, this.minZ)).LengthHorizontalSquared;
				}
				if (c.z > this.maxZ)
				{
					return (float)(c - new IntVec3(this.maxX, 0, this.maxZ)).LengthHorizontalSquared;
				}
				return (float)((c.x - this.maxX) * (c.x - this.maxX));
			}
			else
			{
				if (c.z < this.minZ)
				{
					return (float)((this.minZ - c.z) * (this.minZ - c.z));
				}
				return (float)((c.z - this.maxZ) * (c.z - this.maxZ));
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00007108 File Offset: 0x00005308
		public IntVec3 ClosestCellTo(IntVec3 c)
		{
			if (this.Contains(c))
			{
				return c;
			}
			if (c.x < this.minX)
			{
				if (c.z < this.minZ)
				{
					return new IntVec3(this.minX, 0, this.minZ);
				}
				if (c.z > this.maxZ)
				{
					return new IntVec3(this.minX, 0, this.maxZ);
				}
				return new IntVec3(this.minX, 0, c.z);
			}
			else if (c.x > this.maxX)
			{
				if (c.z < this.minZ)
				{
					return new IntVec3(this.maxX, 0, this.minZ);
				}
				if (c.z > this.maxZ)
				{
					return new IntVec3(this.maxX, 0, this.maxZ);
				}
				return new IntVec3(this.maxX, 0, c.z);
			}
			else
			{
				if (c.z < this.minZ)
				{
					return new IntVec3(c.x, 0, this.minZ);
				}
				return new IntVec3(c.x, 0, this.maxZ);
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000721C File Offset: 0x0000541C
		public bool InNoBuildEdgeArea(Map map)
		{
			return !this.IsEmpty && (this.minX < 10 || this.minZ < 10 || this.maxX >= map.Size.x - 10 || this.maxZ >= map.Size.z - 10);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00007278 File Offset: 0x00005478
		public IEnumerable<IntVec3> GetEdgeCells(Rot4 dir)
		{
			if (dir == Rot4.North)
			{
				int num;
				for (int x = this.minX; x <= this.maxX; x = num + 1)
				{
					yield return new IntVec3(x, 0, this.maxZ);
					num = x;
				}
			}
			else if (dir == Rot4.South)
			{
				int num;
				for (int x = this.minX; x <= this.maxX; x = num + 1)
				{
					yield return new IntVec3(x, 0, this.minZ);
					num = x;
				}
			}
			else if (dir == Rot4.West)
			{
				int num;
				for (int x = this.minZ; x <= this.maxZ; x = num + 1)
				{
					yield return new IntVec3(this.minX, 0, x);
					num = x;
				}
			}
			else if (dir == Rot4.East)
			{
				int num;
				for (int x = this.minZ; x <= this.maxZ; x = num + 1)
				{
					yield return new IntVec3(this.maxX, 0, x);
					num = x;
				}
			}
			yield break;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00007294 File Offset: 0x00005494
		public bool TryFindRandomInnerRectTouchingEdge(IntVec2 size, out CellRect rect, Predicate<CellRect> predicate = null)
		{
			if (this.Width < size.x || this.Height < size.z)
			{
				rect = CellRect.Empty;
				return false;
			}
			if (size.x <= 0 || size.z <= 0 || this.IsEmpty)
			{
				rect = CellRect.Empty;
				return false;
			}
			CellRect cellRect = this;
			cellRect.maxX -= size.x - 1;
			cellRect.maxZ -= size.z - 1;
			IntVec3 intVec;
			if (cellRect.EdgeCells.Where(delegate(IntVec3 x)
			{
				if (predicate == null)
				{
					return true;
				}
				CellRect obj = new CellRect(x.x, x.z, size.x, size.z);
				return predicate(obj);
			}).TryRandomElement(out intVec))
			{
				rect = new CellRect(intVec.x, intVec.z, size.x, size.z);
				return true;
			}
			rect = CellRect.Empty;
			return false;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000073AC File Offset: 0x000055AC
		public bool TryFindRandomInnerRect(IntVec2 size, out CellRect rect, Predicate<CellRect> predicate = null)
		{
			if (this.Width < size.x || this.Height < size.z)
			{
				rect = CellRect.Empty;
				return false;
			}
			if (size.x <= 0 || size.z <= 0 || this.IsEmpty)
			{
				rect = CellRect.Empty;
				return false;
			}
			CellRect cellRect = this;
			cellRect.maxX -= size.x - 1;
			cellRect.maxZ -= size.z - 1;
			IntVec3 intVec;
			if (cellRect.Cells.Where(delegate(IntVec3 x)
			{
				if (predicate == null)
				{
					return true;
				}
				CellRect obj = new CellRect(x.x, x.z, size.x, size.z);
				return predicate(obj);
			}).TryRandomElement(out intVec))
			{
				rect = new CellRect(intVec.x, intVec.z, size.x, size.z);
				return true;
			}
			rect = CellRect.Empty;
			return false;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000074C4 File Offset: 0x000056C4
		public CellRect ExpandedBy(int dist)
		{
			CellRect result = this;
			result.minX -= dist;
			result.minZ -= dist;
			result.maxX += dist;
			result.maxZ += dist;
			return result;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00007509 File Offset: 0x00005709
		public CellRect ContractedBy(int dist)
		{
			return this.ExpandedBy(-dist);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00007513 File Offset: 0x00005713
		public CellRect MovedBy(IntVec2 offset)
		{
			return this.MovedBy(offset.ToIntVec3);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00007524 File Offset: 0x00005724
		public CellRect MovedBy(IntVec3 offset)
		{
			CellRect result = this;
			result.minX += offset.x;
			result.minZ += offset.z;
			result.maxX += offset.x;
			result.maxZ += offset.z;
			return result;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00007580 File Offset: 0x00005780
		public CellRect Encapsulate(CellRect otherRect)
		{
			if (this.minX > otherRect.minX)
			{
				this.minX = otherRect.minX;
			}
			if (this.maxX < otherRect.maxX)
			{
				this.maxX = otherRect.maxX;
			}
			if (this.minZ > otherRect.minZ)
			{
				this.minZ = otherRect.minZ;
			}
			if (this.maxZ < otherRect.maxZ)
			{
				this.maxZ = otherRect.maxZ;
			}
			return this;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x000075FB File Offset: 0x000057FB
		public int IndexOf(IntVec3 location)
		{
			return location.x - this.minX + (location.z - this.minZ) * this.Width;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00007620 File Offset: 0x00005820
		public void DebugDraw()
		{
			float y = AltitudeLayer.MetaOverlays.AltitudeFor();
			Vector3 vector = new Vector3((float)this.minX, y, (float)this.minZ);
			Vector3 vector2 = new Vector3((float)this.minX, y, (float)(this.maxZ + 1));
			Vector3 vector3 = new Vector3((float)(this.maxX + 1), y, (float)(this.maxZ + 1));
			Vector3 vector4 = new Vector3((float)(this.maxX + 1), y, (float)this.minZ);
			GenDraw.DrawLineBetween(vector, vector2);
			GenDraw.DrawLineBetween(vector2, vector3);
			GenDraw.DrawLineBetween(vector3, vector4);
			GenDraw.DrawLineBetween(vector4, vector);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000076B3 File Offset: 0x000058B3
		public CellRect.Enumerator GetEnumerator()
		{
			return new CellRect.Enumerator(this);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x000076C0 File Offset: 0x000058C0
		IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000076C0 File Offset: 0x000058C0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x000076D0 File Offset: 0x000058D0
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.minX,
				",",
				this.minZ,
				",",
				this.maxX,
				",",
				this.maxZ,
				")"
			});
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000774C File Offset: 0x0000594C
		public static CellRect FromString(string str)
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
			int num = Convert.ToInt32(array[0], invariantCulture);
			int num2 = Convert.ToInt32(array[1], invariantCulture);
			int num3 = Convert.ToInt32(array[2], invariantCulture);
			int num4 = Convert.ToInt32(array[3], invariantCulture);
			return new CellRect(num, num2, num3 - num + 1, num4 - num2 + 1);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x000077D0 File Offset: 0x000059D0
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(Gen.HashCombineInt(Gen.HashCombineInt(Gen.HashCombineInt(0, this.minX), this.maxX), this.minZ), this.maxZ);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000077FF File Offset: 0x000059FF
		public override bool Equals(object obj)
		{
			return obj is CellRect && this.Equals((CellRect)obj);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00007817 File Offset: 0x00005A17
		public bool Equals(CellRect other)
		{
			return this.minX == other.minX && this.maxX == other.maxX && this.minZ == other.minZ && this.maxZ == other.maxZ;
		}

		// Token: 0x0400004F RID: 79
		public int minX;

		// Token: 0x04000050 RID: 80
		public int maxX;

		// Token: 0x04000051 RID: 81
		public int minZ;

		// Token: 0x04000052 RID: 82
		public int maxZ;

		// Token: 0x02001C3E RID: 7230
		public struct Enumerator : IEnumerator<IntVec3>, IEnumerator, IDisposable
		{
			// Token: 0x17001D42 RID: 7490
			// (get) Token: 0x0600AE2C RID: 44588 RVA: 0x003F9A2F File Offset: 0x003F7C2F
			public IntVec3 Current
			{
				get
				{
					return new IntVec3(this.x, 0, this.z);
				}
			}

			// Token: 0x17001D43 RID: 7491
			// (get) Token: 0x0600AE2D RID: 44589 RVA: 0x003F9A43 File Offset: 0x003F7C43
			object IEnumerator.Current
			{
				get
				{
					return new IntVec3(this.x, 0, this.z);
				}
			}

			// Token: 0x0600AE2E RID: 44590 RVA: 0x003F9A5C File Offset: 0x003F7C5C
			public Enumerator(CellRect ir)
			{
				this.ir = ir;
				this.x = ir.minX - 1;
				this.z = ir.minZ;
			}

			// Token: 0x0600AE2F RID: 44591 RVA: 0x003F9A80 File Offset: 0x003F7C80
			public bool MoveNext()
			{
				this.x++;
				if (this.x > this.ir.maxX)
				{
					this.x = this.ir.minX;
					this.z++;
				}
				return this.z <= this.ir.maxZ;
			}

			// Token: 0x0600AE30 RID: 44592 RVA: 0x003F9AE3 File Offset: 0x003F7CE3
			public void Reset()
			{
				this.x = this.ir.minX - 1;
				this.z = this.ir.minZ;
			}

			// Token: 0x0600AE31 RID: 44593 RVA: 0x000034B7 File Offset: 0x000016B7
			void IDisposable.Dispose()
			{
			}

			// Token: 0x04006EFE RID: 28414
			private CellRect ir;

			// Token: 0x04006EFF RID: 28415
			private int x;

			// Token: 0x04006F00 RID: 28416
			private int z;
		}

		// Token: 0x02001C3F RID: 7231
		[Obsolete("Do not use this anymore, CellRect has a struct-enumerator as substitute")]
		public struct CellRectIterator
		{
			// Token: 0x17001D44 RID: 7492
			// (get) Token: 0x0600AE32 RID: 44594 RVA: 0x003F9B09 File Offset: 0x003F7D09
			public IntVec3 Current
			{
				get
				{
					return new IntVec3(this.x, 0, this.z);
				}
			}

			// Token: 0x0600AE33 RID: 44595 RVA: 0x003F9B1D File Offset: 0x003F7D1D
			public CellRectIterator(CellRect cr)
			{
				this.minX = cr.minX;
				this.maxX = cr.maxX;
				this.maxZ = cr.maxZ;
				this.x = cr.minX;
				this.z = cr.minZ;
			}

			// Token: 0x0600AE34 RID: 44596 RVA: 0x003F9B5B File Offset: 0x003F7D5B
			public void MoveNext()
			{
				this.x++;
				if (this.x > this.maxX)
				{
					this.x = this.minX;
					this.z++;
				}
			}

			// Token: 0x0600AE35 RID: 44597 RVA: 0x003F9B93 File Offset: 0x003F7D93
			public bool Done()
			{
				return this.z > this.maxZ;
			}

			// Token: 0x04006F01 RID: 28417
			private int maxX;

			// Token: 0x04006F02 RID: 28418
			private int minX;

			// Token: 0x04006F03 RID: 28419
			private int maxZ;

			// Token: 0x04006F04 RID: 28420
			private int x;

			// Token: 0x04006F05 RID: 28421
			private int z;
		}
	}
}
