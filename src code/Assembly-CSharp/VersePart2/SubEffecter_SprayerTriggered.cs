using System;

namespace Verse
{
	// Token: 0x02000552 RID: 1362
	public class SubEffecter_SprayerTriggered : SubEffecter_Sprayer
	{
		// Token: 0x06002993 RID: 10643 RVA: 0x00109FF4 File Offset: 0x001081F4
		public SubEffecter_SprayerTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06002994 RID: 10644 RVA: 0x0010A072 File Offset: 0x00108272
		public override void SubTrigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1)
		{
			base.MakeMote(A, B, overrideSpawnTick);
		}
	}
}
