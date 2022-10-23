using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000472 RID: 1138
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultEmptyListAttribute : DefaultValueAttribute
	{
		// Token: 0x060022B0 RID: 8880 RVA: 0x000DDC94 File Offset: 0x000DBE94
		public DefaultEmptyListAttribute(Type type) : base(type)
		{
		}

		// Token: 0x060022B1 RID: 8881 RVA: 0x000DDCA0 File Offset: 0x000DBEA0
		public override bool ObjIsDefault(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType().GetGenericTypeDefinition() != typeof(List<>))
			{
				return false;
			}
			Type[] genericArguments = obj.GetType().GetGenericArguments();
			return genericArguments.Length == 1 && !(genericArguments[0] != (Type)this.value) && (int)obj.GetType().GetProperty("Count").GetValue(obj, null) == 0;
		}
	}
}
