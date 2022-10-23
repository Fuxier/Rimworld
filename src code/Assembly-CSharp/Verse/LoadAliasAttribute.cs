using System;

namespace Verse
{
	// Token: 0x02000476 RID: 1142
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class LoadAliasAttribute : Attribute
	{
		// Token: 0x060022B5 RID: 8885 RVA: 0x000DDD1A File Offset: 0x000DBF1A
		public LoadAliasAttribute(string alias)
		{
			this.alias = alias;
		}

		// Token: 0x04001609 RID: 5641
		public string alias;
	}
}
