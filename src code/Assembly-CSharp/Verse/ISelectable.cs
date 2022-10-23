using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020004E2 RID: 1250
	public interface ISelectable
	{
		// Token: 0x060025B3 RID: 9651
		IEnumerable<Gizmo> GetGizmos();

		// Token: 0x060025B4 RID: 9652
		string GetInspectString();

		// Token: 0x060025B5 RID: 9653
		IEnumerable<InspectTabBase> GetInspectTabs();
	}
}
