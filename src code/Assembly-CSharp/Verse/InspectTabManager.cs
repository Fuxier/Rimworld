using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000076 RID: 118
	public static class InspectTabManager
	{
		// Token: 0x060004A5 RID: 1189 RVA: 0x0001A258 File Offset: 0x00018458
		public static InspectTabBase GetSharedInstance(Type tabType)
		{
			InspectTabBase inspectTabBase;
			if (InspectTabManager.sharedInstances.TryGetValue(tabType, out inspectTabBase))
			{
				return inspectTabBase;
			}
			inspectTabBase = (InspectTabBase)Activator.CreateInstance(tabType);
			InspectTabManager.sharedInstances.Add(tabType, inspectTabBase);
			return inspectTabBase;
		}

		// Token: 0x04000217 RID: 535
		private static Dictionary<Type, InspectTabBase> sharedInstances = new Dictionary<Type, InspectTabBase>();
	}
}
