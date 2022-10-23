using System;

namespace Verse
{
	// Token: 0x020002DA RID: 730
	public class HediffCompProperties_Discoverable : HediffCompProperties
	{
		// Token: 0x060014B7 RID: 5303 RVA: 0x0007DAA8 File Offset: 0x0007BCA8
		public HediffCompProperties_Discoverable()
		{
			this.compClass = typeof(HediffComp_Discoverable);
		}

		// Token: 0x040010CC RID: 4300
		public bool sendLetterWhenDiscovered;

		// Token: 0x040010CD RID: 4301
		public string discoverLetterLabel;

		// Token: 0x040010CE RID: 4302
		public string discoverLetterText;

		// Token: 0x040010CF RID: 4303
		public MessageTypeDef messageType;

		// Token: 0x040010D0 RID: 4304
		public LetterDef letterType;
	}
}
