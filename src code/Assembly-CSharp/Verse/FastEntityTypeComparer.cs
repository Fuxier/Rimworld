using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001EF RID: 495
	public class FastEntityTypeComparer : IEqualityComparer<ThingCategory>
	{
		// Token: 0x06000E4B RID: 3659 RVA: 0x00025E0A File Offset: 0x0002400A
		public bool Equals(ThingCategory x, ThingCategory y)
		{
			return x == y;
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x000282AB File Offset: 0x000264AB
		public int GetHashCode(ThingCategory obj)
		{
			return (int)obj;
		}

		// Token: 0x04000C77 RID: 3191
		public static readonly FastEntityTypeComparer Instance = new FastEntityTypeComparer();
	}
}
