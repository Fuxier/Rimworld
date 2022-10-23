using System;

namespace Verse
{
	// Token: 0x02000131 RID: 305
	public class RoomStatScoreStage
	{
		// Token: 0x060007E8 RID: 2024 RVA: 0x00028388 File Offset: 0x00026588
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x040007F5 RID: 2037
		public float minScore = float.MinValue;

		// Token: 0x040007F6 RID: 2038
		public string label;

		// Token: 0x040007F7 RID: 2039
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabel;
	}
}
