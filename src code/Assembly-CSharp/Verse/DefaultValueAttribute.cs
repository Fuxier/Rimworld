using System;

namespace Verse
{
	// Token: 0x02000470 RID: 1136
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultValueAttribute : Attribute
	{
		// Token: 0x060022AD RID: 8877 RVA: 0x000DDC4C File Offset: 0x000DBE4C
		public DefaultValueAttribute(object value)
		{
			this.value = value;
		}

		// Token: 0x060022AE RID: 8878 RVA: 0x000DDC5B File Offset: 0x000DBE5B
		public virtual bool ObjIsDefault(object obj)
		{
			if (obj == null)
			{
				return this.value == null;
			}
			return this.value != null && this.value.Equals(obj);
		}

		// Token: 0x04001608 RID: 5640
		public object value;
	}
}
