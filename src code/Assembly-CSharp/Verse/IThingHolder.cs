using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000405 RID: 1029
	public interface IThingHolder
	{
		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06001DCB RID: 7627
		IThingHolder ParentHolder { get; }

		// Token: 0x06001DCC RID: 7628
		void GetChildHolders(List<IThingHolder> outChildren);

		// Token: 0x06001DCD RID: 7629
		ThingOwner GetDirectlyHeldThings();
	}
}
