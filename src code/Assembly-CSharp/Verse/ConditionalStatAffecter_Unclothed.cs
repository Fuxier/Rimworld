using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000080 RID: 128
	public class ConditionalStatAffecter_Unclothed : ConditionalStatAffecter
	{
		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060004C2 RID: 1218 RVA: 0x0001AA24 File Offset: 0x00018C24
		public override string Label
		{
			get
			{
				return "StatsReport_Unclothed".Translate();
			}
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0001AA38 File Offset: 0x00018C38
		public override bool Applies(StatRequest req)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			Pawn pawn;
			if (req.HasThing && (pawn = (req.Thing as Pawn)) != null && pawn.apparel != null)
			{
				using (List<Apparel>.Enumerator enumerator = pawn.apparel.WornApparel.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def.apparel.countsAsClothingForNudity)
						{
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}
	}
}
