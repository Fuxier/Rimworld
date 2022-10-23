using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200007F RID: 127
	public class ConditionalStatAffecter_Clothed : ConditionalStatAffecter
	{
		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x0001A97C File Offset: 0x00018B7C
		public override string Label
		{
			get
			{
				return "StatsReport_Clothed".Translate();
			}
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0001A990 File Offset: 0x00018B90
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
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}
	}
}
