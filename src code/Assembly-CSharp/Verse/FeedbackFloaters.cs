using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000491 RID: 1169
	public class FeedbackFloaters
	{
		// Token: 0x06002357 RID: 9047 RVA: 0x000E2810 File Offset: 0x000E0A10
		public void AddFeedback(FeedbackItem newFeedback)
		{
			this.feeders.Add(newFeedback);
		}

		// Token: 0x06002358 RID: 9048 RVA: 0x000E2820 File Offset: 0x000E0A20
		public void FeedbackUpdate()
		{
			for (int i = this.feeders.Count - 1; i >= 0; i--)
			{
				this.feeders[i].Update();
				if (this.feeders[i].TimeLeft <= 0f)
				{
					this.feeders.Remove(this.feeders[i]);
				}
			}
		}

		// Token: 0x06002359 RID: 9049 RVA: 0x000E2888 File Offset: 0x000E0A88
		public void FeedbackOnGUI()
		{
			foreach (FeedbackItem feedbackItem in this.feeders)
			{
				feedbackItem.FeedbackOnGUI();
			}
		}

		// Token: 0x040016B2 RID: 5810
		protected List<FeedbackItem> feeders = new List<FeedbackItem>();
	}
}
