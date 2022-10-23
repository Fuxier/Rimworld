using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000115 RID: 277
	public class FastPawnCapacityDefComparer : IEqualityComparer<PawnCapacityDef>
	{
		// Token: 0x06000740 RID: 1856 RVA: 0x00025E0A File Offset: 0x0002400A
		public bool Equals(PawnCapacityDef x, PawnCapacityDef y)
		{
			return x == y;
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x00025E10 File Offset: 0x00024010
		public int GetHashCode(PawnCapacityDef obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x040006C7 RID: 1735
		public static readonly FastPawnCapacityDefComparer Instance = new FastPawnCapacityDefComparer();
	}
}
