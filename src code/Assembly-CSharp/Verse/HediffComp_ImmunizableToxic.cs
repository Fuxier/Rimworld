using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000332 RID: 818
	public class HediffComp_ImmunizableToxic : HediffComp_Immunizable
	{
		// Token: 0x060015DF RID: 5599 RVA: 0x00081CB0 File Offset: 0x0007FEB0
		public override float SeverityChangePerDay()
		{
			float num = base.SeverityChangePerDay();
			if (num < 0f && base.Pawn.Spawned && ModsConfig.BiotechActive)
			{
				if (base.Pawn.Position.IsPolluted(base.Pawn.Map) && base.Pawn.GetStatValue(StatDefOf.ToxicEnvironmentResistance, true, -1) < 1f)
				{
					num = 0f;
				}
				else if (!base.Pawn.Position.Roofed(base.Pawn.Map))
				{
					if (base.Pawn.Map.GameConditionManager.ActiveConditions.Any((GameCondition x) => x.def.conditionClass == typeof(GameCondition_ToxicFallout)) && base.Pawn.GetStatValue(StatDefOf.ToxicResistance, true, -1) < 1f)
					{
						num = 0f;
					}
				}
			}
			return num;
		}
	}
}
