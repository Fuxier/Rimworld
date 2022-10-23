using System;

namespace Verse
{
	// Token: 0x02000553 RID: 1363
	public class SubEffecter_SprayerTriggeredChance : SubEffecter_Sprayer
	{
		// Token: 0x06002995 RID: 10645 RVA: 0x00109FF4 File Offset: 0x001081F4
		public SubEffecter_SprayerTriggeredChance(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06002996 RID: 10646 RVA: 0x0010A080 File Offset: 0x00108280
		public override void SubTrigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1)
		{
			float chancePerTick = this.def.chancePerTick;
			if (Rand.Value < chancePerTick)
			{
				base.MakeMote(A, B, overrideSpawnTick);
			}
		}
	}
}
