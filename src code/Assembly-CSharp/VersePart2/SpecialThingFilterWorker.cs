using System;

namespace Verse
{
	// Token: 0x020005A2 RID: 1442
	public abstract class SpecialThingFilterWorker
	{
		// Token: 0x06002BE4 RID: 11236
		public abstract bool Matches(Thing t);

		// Token: 0x06002BE5 RID: 11237 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual bool AlwaysMatches(ThingDef def)
		{
			return false;
		}

		// Token: 0x06002BE6 RID: 11238 RVA: 0x00002662 File Offset: 0x00000862
		public virtual bool CanEverMatch(ThingDef def)
		{
			return true;
		}
	}
}
