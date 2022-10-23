using System;

namespace Verse
{
	// Token: 0x02000477 RID: 1143
	[AttributeUsage(AttributeTargets.Field)]
	public class DescriptionAttribute : Attribute
	{
		// Token: 0x060022B6 RID: 8886 RVA: 0x000DDD29 File Offset: 0x000DBF29
		public DescriptionAttribute(string description)
		{
			this.description = description;
		}

		// Token: 0x0400160A RID: 5642
		public string description;
	}
}
