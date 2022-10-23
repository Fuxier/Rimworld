using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000BF RID: 191
	public class CompProperties
	{
		// Token: 0x06000601 RID: 1537 RVA: 0x000207EF File Offset: 0x0001E9EF
		public CompProperties()
		{
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00020807 File Offset: 0x0001EA07
		public CompProperties(Type compClass)
		{
			this.compClass = compClass;
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude, Thing thing = null)
		{
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x00020826 File Offset: 0x0001EA26
		public virtual IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return parentDef.defName + " has CompProperties with null compClass.";
			}
			yield break;
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void ResolveReferences(ThingDef parentDef)
		{
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x0002083D File Offset: 0x0001EA3D
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			return Enumerable.Empty<StatDrawEntry>();
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void PostLoadSpecial(ThingDef parent)
		{
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Notify_PostUnlockedByResearch(ThingDef parent)
		{
		}

		// Token: 0x04000376 RID: 886
		[TranslationHandle]
		public Type compClass = typeof(ThingComp);
	}
}
