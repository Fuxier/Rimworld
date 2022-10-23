using System;

namespace Verse
{
	// Token: 0x02000458 RID: 1112
	[AttributeUsage(AttributeTargets.Method)]
	public class DebugOutputAttribute : Attribute
	{
		// Token: 0x06002241 RID: 8769 RVA: 0x000DA637 File Offset: 0x000D8837
		public DebugOutputAttribute()
		{
		}

		// Token: 0x06002242 RID: 8770 RVA: 0x000DA64A File Offset: 0x000D884A
		public DebugOutputAttribute(bool onlyWhenPlaying)
		{
			this.onlyWhenPlaying = onlyWhenPlaying;
		}

		// Token: 0x06002243 RID: 8771 RVA: 0x000DA664 File Offset: 0x000D8864
		public DebugOutputAttribute(string category, bool onlyWhenPlaying = false) : this(onlyWhenPlaying)
		{
			this.category = category;
		}

		// Token: 0x040015C6 RID: 5574
		public string name;

		// Token: 0x040015C7 RID: 5575
		public string category = "General";

		// Token: 0x040015C8 RID: 5576
		public bool onlyWhenPlaying;
	}
}
