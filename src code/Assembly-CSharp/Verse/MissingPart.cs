using System;

namespace Verse
{
	// Token: 0x02000120 RID: 288
	public class MissingPart
	{
		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000776 RID: 1910 RVA: 0x000266CD File Offset: 0x000248CD
		public BodyPartDef BodyPart
		{
			get
			{
				return this.bodyPart;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000777 RID: 1911 RVA: 0x000266D5 File Offset: 0x000248D5
		public HediffDef Injury
		{
			get
			{
				return this.injury;
			}
		}

		// Token: 0x0400076E RID: 1902
		private BodyPartDef bodyPart;

		// Token: 0x0400076F RID: 1903
		private HediffDef injury;
	}
}
