using System;

namespace Verse
{
	// Token: 0x0200007B RID: 123
	public class SubEffecter_DrifterEmoteTriggered : SubEffecter_DrifterEmote
	{
		// Token: 0x060004B4 RID: 1204 RVA: 0x0001A826 File Offset: 0x00018A26
		public SubEffecter_DrifterEmoteTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0001A88D File Offset: 0x00018A8D
		public override void SubTrigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1)
		{
			base.MakeMote(A, overrideSpawnTick);
		}
	}
}
