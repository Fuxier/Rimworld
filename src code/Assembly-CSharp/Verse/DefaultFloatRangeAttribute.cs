using System;

namespace Verse
{
	// Token: 0x02000471 RID: 1137
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultFloatRangeAttribute : DefaultValueAttribute
	{
		// Token: 0x060022AF RID: 8879 RVA: 0x000DDC80 File Offset: 0x000DBE80
		public DefaultFloatRangeAttribute(float min, float max) : base(new FloatRange(min, max))
		{
		}
	}
}
