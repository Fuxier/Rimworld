using System;

namespace Verse
{
	// Token: 0x0200017E RID: 382
	public static class NamedArgumentUtility
	{
		// Token: 0x06000A88 RID: 2696 RVA: 0x00036D0D File Offset: 0x00034F0D
		public static NamedArgument Named(this object arg, string label)
		{
			return new NamedArgument(arg, label);
		}
	}
}
