using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x0200010C RID: 268
	public class WorkModeDrawer_Escort : WorkModeDrawer
	{
		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000728 RID: 1832 RVA: 0x0000249D File Offset: 0x0000069D
		protected override bool DrawIconAtTarget
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x00025B22 File Offset: 0x00023D22
		public override GlobalTargetInfo GetTargetForLine(MechanitorControlGroup group)
		{
			return group.Tracker.Pawn;
		}
	}
}
