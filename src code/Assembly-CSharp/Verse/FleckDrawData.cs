using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003D7 RID: 983
	public struct FleckDrawData
	{
		// Token: 0x04001427 RID: 5159
		public Vector3 pos;

		// Token: 0x04001428 RID: 5160
		public float rotation;

		// Token: 0x04001429 RID: 5161
		public Vector3 scale;

		// Token: 0x0400142A RID: 5162
		public float alpha;

		// Token: 0x0400142B RID: 5163
		public Color color;

		// Token: 0x0400142C RID: 5164
		public int drawLayer;

		// Token: 0x0400142D RID: 5165
		public Color? overrideColor;

		// Token: 0x0400142E RID: 5166
		public DrawBatchPropertyBlock propertyBlock;

		// Token: 0x0400142F RID: 5167
		public float ageSecs;

		// Token: 0x04001430 RID: 5168
		public float id;

		// Token: 0x04001431 RID: 5169
		public float calculatedShockwaveSpan;
	}
}
