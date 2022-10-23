using System;

namespace Verse
{
	// Token: 0x0200032F RID: 815
	public class HediffDefFactor
	{
		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x060015CC RID: 5580 RVA: 0x0008193A File Offset: 0x0007FB3A
		public HediffDef HediffDef
		{
			get
			{
				return this.hediffDef;
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x060015CD RID: 5581 RVA: 0x00081942 File Offset: 0x0007FB42
		public float Factor
		{
			get
			{
				return this.factor;
			}
		}

		// Token: 0x04001167 RID: 4455
		private HediffDef hediffDef;

		// Token: 0x04001168 RID: 4456
		private float factor = 1f;
	}
}
