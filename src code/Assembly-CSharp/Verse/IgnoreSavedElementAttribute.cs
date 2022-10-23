using System;

namespace Verse
{
	// Token: 0x02000479 RID: 1145
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class IgnoreSavedElementAttribute : Attribute
	{
		// Token: 0x060022B8 RID: 8888 RVA: 0x000DDD38 File Offset: 0x000DBF38
		public IgnoreSavedElementAttribute(string elementToIgnore)
		{
			this.elementToIgnore = elementToIgnore;
		}

		// Token: 0x0400160B RID: 5643
		public string elementToIgnore;
	}
}
