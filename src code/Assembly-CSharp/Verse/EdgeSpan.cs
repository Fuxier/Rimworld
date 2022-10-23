using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000242 RID: 578
	public struct EdgeSpan
	{
		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06001068 RID: 4200 RVA: 0x00060230 File Offset: 0x0005E430
		public bool IsValid
		{
			get
			{
				return this.length > 0;
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06001069 RID: 4201 RVA: 0x0006023B File Offset: 0x0005E43B
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				int num;
				for (int i = 0; i < this.length; i = num + 1)
				{
					if (this.dir == SpanDirection.North)
					{
						yield return new IntVec3(this.root.x, 0, this.root.z + i);
					}
					else if (this.dir == SpanDirection.East)
					{
						yield return new IntVec3(this.root.x + i, 0, this.root.z);
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x00060250 File Offset: 0x0005E450
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(root=",
				this.root,
				", dir=",
				this.dir.ToString(),
				" + length=",
				this.length,
				")"
			});
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x000602B8 File Offset: 0x0005E4B8
		public EdgeSpan(IntVec3 root, SpanDirection dir, int length)
		{
			this.root = root;
			this.dir = dir;
			this.length = length;
		}

		// Token: 0x0600106C RID: 4204 RVA: 0x000602D0 File Offset: 0x0005E4D0
		public ulong UniqueHashCode()
		{
			ulong num = this.root.UniqueHashCode();
			if (this.dir == SpanDirection.East)
			{
				num += 17592186044416UL;
			}
			return num + (ulong)(281474976710656L * (long)this.length);
		}

		// Token: 0x04000E71 RID: 3697
		public IntVec3 root;

		// Token: 0x04000E72 RID: 3698
		public SpanDirection dir;

		// Token: 0x04000E73 RID: 3699
		public int length;
	}
}
