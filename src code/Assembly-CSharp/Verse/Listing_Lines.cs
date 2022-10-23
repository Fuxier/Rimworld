using System;

namespace Verse
{
	// Token: 0x020004B2 RID: 1202
	public abstract class Listing_Lines : Listing
	{
		// Token: 0x0600243B RID: 9275 RVA: 0x000E7305 File Offset: 0x000E5505
		protected void EndLine()
		{
			this.curY += this.lineHeight + this.verticalSpacing;
		}

		// Token: 0x0400174C RID: 5964
		public float lineHeight = 20f;
	}
}
