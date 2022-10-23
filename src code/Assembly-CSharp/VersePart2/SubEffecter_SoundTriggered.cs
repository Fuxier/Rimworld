using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200054E RID: 1358
	public class SubEffecter_SoundTriggered : SubEffecter
	{
		// Token: 0x0600298A RID: 10634 RVA: 0x0001A6A9 File Offset: 0x000188A9
		public SubEffecter_SoundTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x0600298B RID: 10635 RVA: 0x00109694 File Offset: 0x00107894
		public override void SubTrigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1)
		{
			this.def.soundDef.PlayOneShot(new TargetInfo(A.Cell, A.Map, false));
		}
	}
}
