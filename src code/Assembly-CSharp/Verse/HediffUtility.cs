using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000339 RID: 825
	public static class HediffUtility
	{
		// Token: 0x060015FB RID: 5627 RVA: 0x00082394 File Offset: 0x00080594
		public static T TryGetComp<T>(this Hediff hd) where T : HediffComp
		{
			HediffWithComps hediffWithComps = hd as HediffWithComps;
			if (hediffWithComps == null)
			{
				return default(T);
			}
			if (hediffWithComps.comps != null)
			{
				for (int i = 0; i < hediffWithComps.comps.Count; i++)
				{
					T t = hediffWithComps.comps[i] as T;
					if (t != null)
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x060015FC RID: 5628 RVA: 0x00082400 File Offset: 0x00080600
		public static bool IsTended(this Hediff hd)
		{
			HediffWithComps hediffWithComps = hd as HediffWithComps;
			if (hediffWithComps == null)
			{
				return false;
			}
			HediffComp_TendDuration hediffComp_TendDuration = hediffWithComps.TryGetComp<HediffComp_TendDuration>();
			return hediffComp_TendDuration != null && hediffComp_TendDuration.IsTended;
		}

		// Token: 0x060015FD RID: 5629 RVA: 0x0008242C File Offset: 0x0008062C
		public static bool IsPermanent(this Hediff hd)
		{
			HediffWithComps hediffWithComps = hd as HediffWithComps;
			if (hediffWithComps == null)
			{
				return false;
			}
			HediffComp_GetsPermanent hediffComp_GetsPermanent = hediffWithComps.TryGetComp<HediffComp_GetsPermanent>();
			return hediffComp_GetsPermanent != null && hediffComp_GetsPermanent.IsPermanent;
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x00082458 File Offset: 0x00080658
		public static bool FullyImmune(this Hediff hd)
		{
			HediffWithComps hediffWithComps = hd as HediffWithComps;
			if (hediffWithComps == null)
			{
				return false;
			}
			HediffComp_Immunizable hediffComp_Immunizable = hediffWithComps.TryGetComp<HediffComp_Immunizable>();
			return hediffComp_Immunizable != null && hediffComp_Immunizable.FullyImmune;
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x00082483 File Offset: 0x00080683
		public static bool CanHealFromTending(this Hediff_Injury hd)
		{
			return hd.IsTended() && !hd.IsPermanent();
		}

		// Token: 0x06001600 RID: 5632 RVA: 0x00082498 File Offset: 0x00080698
		public static bool CanHealNaturally(this Hediff_Injury hd)
		{
			return !hd.IsPermanent();
		}

		// Token: 0x06001601 RID: 5633 RVA: 0x000824A4 File Offset: 0x000806A4
		public static int CountAddedAndImplantedParts(this HediffSet hs)
		{
			int num = 0;
			List<Hediff> hediffs = hs.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def.countsAsAddedPartOrImplant)
				{
					num++;
				}
			}
			return num;
		}
	}
}
