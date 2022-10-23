using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x0200052A RID: 1322
	public static class LookTargetsUtility
	{
		// Token: 0x06002877 RID: 10359 RVA: 0x0010532A File Offset: 0x0010352A
		public static bool IsValid(this LookTargets lookTargets)
		{
			return lookTargets != null && lookTargets.IsValid;
		}

		// Token: 0x06002878 RID: 10360 RVA: 0x00105337 File Offset: 0x00103537
		public static GlobalTargetInfo TryGetPrimaryTarget(this LookTargets lookTargets)
		{
			if (lookTargets == null)
			{
				return GlobalTargetInfo.Invalid;
			}
			return lookTargets.PrimaryTarget;
		}

		// Token: 0x06002879 RID: 10361 RVA: 0x00105348 File Offset: 0x00103548
		public static void TryHighlight(this LookTargets lookTargets, bool arrow = true, bool colonistBar = true, bool circleOverlay = false)
		{
			if (lookTargets == null)
			{
				return;
			}
			lookTargets.Highlight(arrow, colonistBar, circleOverlay);
		}
	}
}
