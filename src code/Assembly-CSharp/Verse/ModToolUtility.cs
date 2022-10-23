using System;

namespace Verse
{
	// Token: 0x0200047C RID: 1148
	public static class ModToolUtility
	{
		// Token: 0x060022CD RID: 8909 RVA: 0x000DE5DB File Offset: 0x000DC7DB
		public static bool IsValueEditable(this Type type)
		{
			return type.IsValueType || type == typeof(string);
		}
	}
}
