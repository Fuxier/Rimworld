using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000062 RID: 98
	public class RitualStageOnTickActions : IExposable
	{
		// Token: 0x0600045A RID: 1114 RVA: 0x000182EC File Offset: 0x000164EC
		public void ExposeData()
		{
			Scribe_Collections.Look<ActionOnTick>(ref this.actions, "actions", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x04000175 RID: 373
		public List<ActionOnTick> actions = new List<ActionOnTick>();
	}
}
