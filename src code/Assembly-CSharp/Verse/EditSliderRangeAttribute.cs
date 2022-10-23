using System;

namespace Verse
{
	// Token: 0x0200046E RID: 1134
	[AttributeUsage(AttributeTargets.Field)]
	public class EditSliderRangeAttribute : Attribute
	{
		// Token: 0x060022AB RID: 8875 RVA: 0x000DDC1C File Offset: 0x000DBE1C
		public EditSliderRangeAttribute(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x04001605 RID: 5637
		public float min;

		// Token: 0x04001606 RID: 5638
		public float max = 1f;
	}
}
