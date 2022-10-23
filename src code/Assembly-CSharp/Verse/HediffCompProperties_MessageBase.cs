using System;

namespace Verse
{
	// Token: 0x02000308 RID: 776
	public class HediffCompProperties_MessageBase : HediffCompProperties
	{
		// Token: 0x0600154A RID: 5450 RVA: 0x00080045 File Offset: 0x0007E245
		public HediffCompProperties_MessageBase()
		{
			this.compClass = typeof(HediffComp_MessageBase);
		}

		// Token: 0x04001124 RID: 4388
		public MessageTypeDef messageType;

		// Token: 0x04001125 RID: 4389
		[MustTranslate]
		public string message;

		// Token: 0x04001126 RID: 4390
		public bool onlyMessageForColonistsOrPrisoners = true;
	}
}
