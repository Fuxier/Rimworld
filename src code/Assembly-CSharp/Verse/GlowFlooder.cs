using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001D8 RID: 472
	public class GlowFlooder
	{
		// Token: 0x06000D2B RID: 3371 RVA: 0x00049C1C File Offset: 0x00047E1C
		public GlowFlooder(Map map)
		{
			this.map = map;
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
			this.calcGrid = new GlowFlooder.GlowFloodCell[this.mapSizeX * this.mapSizeZ];
			this.openSet = new FastPriorityQueue<int>(new GlowFlooder.CompareGlowFlooderLightSquares(this.calcGrid));
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x00049CA0 File Offset: 0x00047EA0
		public void AddFloodGlowFor(CompGlower theGlower, ColorInt[] glowGrid)
		{
			this.cellIndices = this.map.cellIndices;
			this.glowGrid = glowGrid;
			this.glower = theGlower;
			this.attenLinearSlope = -1f / theGlower.Props.glowRadius;
			Building[] innerArray = this.map.edificeGrid.InnerArray;
			IntVec3 position = theGlower.parent.Position;
			int num = Mathf.RoundToInt(this.glower.Props.glowRadius * 100f);
			int num2 = this.cellIndices.CellToIndex(position);
			this.InitStatusesAndPushStartNode(ref num2, position);
			while (this.openSet.Count != 0)
			{
				num2 = this.openSet.Pop();
				IntVec3 intVec = this.cellIndices.IndexToCell(num2);
				this.calcGrid[num2].status = this.statusFinalizedValue;
				this.SetGlowGridFromDist(num2);
				for (int i = 0; i < 8; i++)
				{
					uint num3 = (uint)(intVec.x + (int)GlowFlooder.Directions[i, 0]);
					uint num4 = (uint)(intVec.z + (int)GlowFlooder.Directions[i, 1]);
					if ((ulong)num3 < (ulong)((long)this.mapSizeX) && (ulong)num4 < (ulong)((long)this.mapSizeZ))
					{
						int x = (int)num3;
						int z = (int)num4;
						int num5 = this.cellIndices.CellToIndex(x, z);
						if (this.calcGrid[num5].status != this.statusFinalizedValue)
						{
							this.blockers[i] = innerArray[num5];
							if (this.blockers[i] != null)
							{
								if (this.blockers[i].def.blockLight)
								{
									goto IL_2DB;
								}
								this.blockers[i] = null;
							}
							int num6;
							if (i < 4)
							{
								num6 = 100;
							}
							else
							{
								num6 = 141;
							}
							int num7 = this.calcGrid[num2].intDist + num6;
							if (num7 <= num)
							{
								if (i >= 4)
								{
									switch (i)
									{
									case 4:
										if (this.blockers[0] != null && this.blockers[1] != null)
										{
											goto IL_2DB;
										}
										break;
									case 5:
										if (this.blockers[1] != null && this.blockers[2] != null)
										{
											goto IL_2DB;
										}
										break;
									case 6:
										if (this.blockers[2] != null && this.blockers[3] != null)
										{
											goto IL_2DB;
										}
										break;
									case 7:
										if (this.blockers[0] != null && this.blockers[3] != null)
										{
											goto IL_2DB;
										}
										break;
									}
								}
								if (this.calcGrid[num5].status <= this.statusUnseenValue)
								{
									this.calcGrid[num5].intDist = 999999;
									this.calcGrid[num5].status = this.statusOpenValue;
								}
								if (num7 < this.calcGrid[num5].intDist)
								{
									this.calcGrid[num5].intDist = num7;
									this.calcGrid[num5].status = this.statusOpenValue;
									this.openSet.Push(num5);
								}
							}
						}
					}
					IL_2DB:;
				}
			}
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x00049F9C File Offset: 0x0004819C
		private void InitStatusesAndPushStartNode(ref int curIndex, IntVec3 start)
		{
			this.statusUnseenValue += 3U;
			this.statusOpenValue += 3U;
			this.statusFinalizedValue += 3U;
			curIndex = this.cellIndices.CellToIndex(start);
			this.openSet.Clear();
			this.calcGrid[curIndex].intDist = 100;
			this.openSet.Clear();
			this.openSet.Push(curIndex);
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x0004A018 File Offset: 0x00048218
		private void SetGlowGridFromDist(int index)
		{
			float num = (float)this.calcGrid[index].intDist / 100f;
			ColorInt colorInt = default(ColorInt);
			if (num <= this.glower.Props.glowRadius)
			{
				float b = 1f / (num * num);
				float b2 = Mathf.Lerp(1f + this.attenLinearSlope * num, b, 0.4f);
				colorInt = this.glower.GlowColor * b2;
				colorInt.a = 0;
			}
			if (colorInt.r > 0 || colorInt.g > 0 || colorInt.b > 0)
			{
				colorInt.ClampToNonNegative();
				this.glowGrid[index] += colorInt;
				if (num < this.glower.Props.overlightRadius)
				{
					this.glowGrid[index].a = 1;
				}
			}
		}

		// Token: 0x04000BFB RID: 3067
		private Map map;

		// Token: 0x04000BFC RID: 3068
		private GlowFlooder.GlowFloodCell[] calcGrid;

		// Token: 0x04000BFD RID: 3069
		private FastPriorityQueue<int> openSet;

		// Token: 0x04000BFE RID: 3070
		private uint statusUnseenValue;

		// Token: 0x04000BFF RID: 3071
		private uint statusOpenValue = 1U;

		// Token: 0x04000C00 RID: 3072
		private uint statusFinalizedValue = 2U;

		// Token: 0x04000C01 RID: 3073
		private int mapSizeX;

		// Token: 0x04000C02 RID: 3074
		private int mapSizeZ;

		// Token: 0x04000C03 RID: 3075
		private CompGlower glower;

		// Token: 0x04000C04 RID: 3076
		private CellIndices cellIndices;

		// Token: 0x04000C05 RID: 3077
		private ColorInt[] glowGrid;

		// Token: 0x04000C06 RID: 3078
		private float attenLinearSlope;

		// Token: 0x04000C07 RID: 3079
		private Thing[] blockers = new Thing[8];

		// Token: 0x04000C08 RID: 3080
		private static readonly sbyte[,] Directions = new sbyte[,]
		{
			{
				0,
				-1
			},
			{
				1,
				0
			},
			{
				0,
				1
			},
			{
				-1,
				0
			},
			{
				1,
				-1
			},
			{
				1,
				1
			},
			{
				-1,
				1
			},
			{
				-1,
				-1
			}
		};

		// Token: 0x02001D61 RID: 7521
		private struct GlowFloodCell
		{
			// Token: 0x04007401 RID: 29697
			public int intDist;

			// Token: 0x04007402 RID: 29698
			public uint status;
		}

		// Token: 0x02001D62 RID: 7522
		private class CompareGlowFlooderLightSquares : IComparer<int>
		{
			// Token: 0x0600B42D RID: 46125 RVA: 0x0041038B File Offset: 0x0040E58B
			public CompareGlowFlooderLightSquares(GlowFlooder.GlowFloodCell[] grid)
			{
				this.grid = grid;
			}

			// Token: 0x0600B42E RID: 46126 RVA: 0x0041039A File Offset: 0x0040E59A
			public int Compare(int a, int b)
			{
				return this.grid[a].intDist.CompareTo(this.grid[b].intDist);
			}

			// Token: 0x04007403 RID: 29699
			private GlowFlooder.GlowFloodCell[] grid;
		}
	}
}
