using System;
using System.Reflection;

namespace Verse
{
	// Token: 0x02000030 RID: 48
	public static class GenAttribute
	{
		// Token: 0x06000239 RID: 569 RVA: 0x0000BE38 File Offset: 0x0000A038
		public static bool HasAttribute<T>(this MemberInfo memberInfo) where T : Attribute
		{
			T t;
			return memberInfo.TryGetAttribute(out t);
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000BE50 File Offset: 0x0000A050
		public static bool TryGetAttribute<T>(this MemberInfo memberInfo, out T customAttribute) where T : Attribute
		{
			object[] customAttributes = memberInfo.GetCustomAttributes(typeof(T), true);
			if (customAttributes.Length == 0)
			{
				customAttribute = default(T);
				return false;
			}
			for (int i = 0; i < customAttributes.Length; i++)
			{
				if (customAttributes[i] is T)
				{
					customAttribute = (T)((object)customAttributes[i]);
					return true;
				}
			}
			customAttribute = default(T);
			return false;
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000BEAC File Offset: 0x0000A0AC
		public static T TryGetAttribute<T>(this MemberInfo memberInfo) where T : Attribute
		{
			T result = default(T);
			memberInfo.TryGetAttribute(out result);
			return result;
		}
	}
}
