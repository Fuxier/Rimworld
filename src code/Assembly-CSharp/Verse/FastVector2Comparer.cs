using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000395 RID: 917
	public class FastVector2Comparer : IEqualityComparer<Vector2>
	{
		// Token: 0x06001A4B RID: 6731 RVA: 0x0009EB58 File Offset: 0x0009CD58
		public bool Equals(Vector2 x, Vector2 y)
		{
			return x == y;
		}

		// Token: 0x06001A4C RID: 6732 RVA: 0x0009EB61 File Offset: 0x0009CD61
		public int GetHashCode(Vector2 obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x04001323 RID: 4899
		public static readonly FastVector2Comparer Instance = new FastVector2Comparer();
	}
}
