using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200007E RID: 126
	public class ConditionalStatAffecter_InSunlight : ConditionalStatAffecter
	{
		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x0001A917 File Offset: 0x00018B17
		public override string Label
		{
			get
			{
				return "StatsReport_InSunlight".Translate();
			}
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0001A928 File Offset: 0x00018B28
		public override bool Applies(StatRequest req)
		{
			return ModsConfig.BiotechActive && (req.HasThing && req.Thing.Spawned) && req.Thing.Position.InSunlight(req.Thing.Map);
		}
	}
}
