using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000306 RID: 774
	public class HediffCompProperties_MessageAfterTicks : HediffCompProperties
	{
		// Token: 0x06001542 RID: 5442 RVA: 0x0007FE34 File Offset: 0x0007E034
		public HediffCompProperties_MessageAfterTicks()
		{
			this.compClass = typeof(HediffComp_MessageAfterTicks);
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x0007FE4C File Offset: 0x0007E04C
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			if (this.ticks <= 0)
			{
				yield return "ticks must be a positive value";
			}
			yield break;
		}

		// Token: 0x0400111D RID: 4381
		public int ticks;

		// Token: 0x0400111E RID: 4382
		public MessageTypeDef messageType;

		// Token: 0x0400111F RID: 4383
		public LetterDef letterType;

		// Token: 0x04001120 RID: 4384
		[MustTranslate]
		public string message;

		// Token: 0x04001121 RID: 4385
		[MustTranslate]
		public string letterLabel;

		// Token: 0x04001122 RID: 4386
		[MustTranslate]
		public string letterText;
	}
}
