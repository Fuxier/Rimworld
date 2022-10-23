using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200059D RID: 1437
	public struct ShootLine
	{
		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x06002BC4 RID: 11204 RVA: 0x00115CD0 File Offset: 0x00113ED0
		public IntVec3 Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x06002BC5 RID: 11205 RVA: 0x00115CD8 File Offset: 0x00113ED8
		public IntVec3 Dest
		{
			get
			{
				return this.dest;
			}
		}

		// Token: 0x06002BC6 RID: 11206 RVA: 0x00115CE0 File Offset: 0x00113EE0
		public ShootLine(IntVec3 source, IntVec3 dest)
		{
			this.source = source;
			this.dest = dest;
		}

		// Token: 0x06002BC7 RID: 11207 RVA: 0x00115CF0 File Offset: 0x00113EF0
		public void ChangeDestToMissWild(float aimOnChance)
		{
			float num = ShootTuning.MissDistanceFromAimOnChanceCurves.Evaluate(aimOnChance, Rand.Value);
			if (num < 0f)
			{
				Log.ErrorOnce("Attempted to wild-miss less than zero tiles away", 94302089);
			}
			IntVec3 a;
			do
			{
				Vector2 unitVector = Rand.UnitVector2;
				Vector3 b = new Vector3(unitVector.x * num, 0f, unitVector.y * num);
				a = (this.dest.ToVector3Shifted() + b).ToIntVec3();
			}
			while (Vector3.Dot((this.dest - this.source).ToVector3(), (a - this.source).ToVector3()) < 0f);
			this.dest = a;
		}

		// Token: 0x06002BC8 RID: 11208 RVA: 0x00115DA0 File Offset: 0x00113FA0
		public IEnumerable<IntVec3> Points()
		{
			return GenSight.PointsOnLineOfSight(this.source, this.dest);
		}

		// Token: 0x06002BC9 RID: 11209 RVA: 0x00115DB4 File Offset: 0x00113FB4
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.source,
				"->",
				this.dest,
				")"
			});
		}

		// Token: 0x06002BCA RID: 11210 RVA: 0x00115E00 File Offset: 0x00114000
		[DebugOutput]
		public static void WildMissResults()
		{
			IntVec3 intVec = new IntVec3(100, 0, 0);
			ShootLine shootLine = new ShootLine(IntVec3.Zero, intVec);
			IEnumerable<int> enumerable = Enumerable.Range(0, 101);
			IEnumerable<int> colValues = Enumerable.Range(0, 12);
			int[,] results = new int[enumerable.Count<int>(), colValues.Count<int>()];
			foreach (int num in enumerable)
			{
				for (int i = 0; i < 10000; i++)
				{
					ShootLine shootLine2 = shootLine;
					shootLine2.ChangeDestToMissWild((float)num / 100f);
					if (shootLine2.dest.z == 0 && shootLine2.dest.x > intVec.x)
					{
						results[num, shootLine2.dest.x - intVec.x]++;
					}
				}
			}
			DebugTables.MakeTablesDialog<int, int>(colValues, (int cells) => cells.ToString() + "-away\ncell\nhit%", enumerable, (int hitchance) => ((float)hitchance / 100f).ToStringPercent() + " aimon chance", delegate(int cells, int hitchance)
			{
				float num2 = (float)hitchance / 100f;
				if (cells == 0)
				{
					return num2.ToStringPercent();
				}
				return ((float)results[hitchance, cells] / 10000f * (1f - num2)).ToStringPercent();
			}, "");
		}

		// Token: 0x04001CB6 RID: 7350
		private IntVec3 source;

		// Token: 0x04001CB7 RID: 7351
		private IntVec3 dest;
	}
}
