using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020004AC RID: 1196
	public class StandardLetter : ChoiceLetter
	{
		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06002409 RID: 9225 RVA: 0x000E5FA9 File Offset: 0x000E41A9
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				yield return base.Option_Close;
				if (this.lookTargets.IsValid())
				{
					yield return base.Option_JumpToLocation;
				}
				if (this.quest != null && !this.quest.hidden)
				{
					yield return base.Option_ViewInQuestsTab("ViewRelatedQuest", false);
				}
				if (!this.hyperlinkThingDefs.NullOrEmpty<ThingDef>())
				{
					int num;
					for (int i = 0; i < this.hyperlinkThingDefs.Count; i = num + 1)
					{
						yield return base.Option_ViewInfoCard(i);
						num = i;
					}
				}
				if (!this.hyperlinkHediffDefs.NullOrEmpty<HediffDef>())
				{
					int i = (this.hyperlinkThingDefs == null) ? 0 : this.hyperlinkThingDefs.Count;
					int num;
					for (int j = 0; j < this.hyperlinkHediffDefs.Count; j = num + 1)
					{
						yield return base.Option_ViewInfoCard(i + j);
						num = j;
					}
				}
				yield break;
			}
		}
	}
}
