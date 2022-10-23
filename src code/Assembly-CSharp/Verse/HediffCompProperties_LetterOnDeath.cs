using System;

namespace Verse
{
	// Token: 0x02000302 RID: 770
	public class HediffCompProperties_LetterOnDeath : HediffCompProperties
	{
		// Token: 0x06001534 RID: 5428 RVA: 0x0007FABC File Offset: 0x0007DCBC
		public HediffCompProperties_LetterOnDeath()
		{
			this.compClass = typeof(HediffComp_LetterOnDeath);
		}

		// Token: 0x04001112 RID: 4370
		public LetterDef letterDef;

		// Token: 0x04001113 RID: 4371
		[MustTranslate]
		public string letterText;

		// Token: 0x04001114 RID: 4372
		[MustTranslate]
		public string letterLabel;

		// Token: 0x04001115 RID: 4373
		public bool onlyIfNoMechanitorDied;
	}
}
