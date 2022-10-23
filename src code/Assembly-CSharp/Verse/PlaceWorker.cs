using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200022C RID: 556
	public abstract class PlaceWorker
	{
		// Token: 0x06000FB9 RID: 4025 RVA: 0x00002662 File Offset: 0x00000862
		public virtual bool IsBuildDesignatorVisible(BuildableDef def)
		{
			return true;
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x0005B7B8 File Offset: 0x000599B8
		public virtual AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void PostPlace(Map map, BuildableDef def, IntVec3 loc, Rot4 rot)
		{
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual bool ForceAllowPlaceOver(BuildableDef other)
		{
			return false;
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x0005B7BF File Offset: 0x000599BF
		public virtual IEnumerable<TerrainAffordanceDef> DisplayAffordances()
		{
			return Enumerable.Empty<TerrainAffordanceDef>();
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void DrawMouseAttachments(BuildableDef def)
		{
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void DrawPlaceMouseAttachments(float curX, ref float curY, BuildableDef def, IntVec3 center, Rot4 rot)
		{
		}
	}
}
