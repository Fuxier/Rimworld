using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020004AB RID: 1195
	public class NewQuestLetter : ChoiceLetter
	{
		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06002406 RID: 9222 RVA: 0x000E5F3F File Offset: 0x000E413F
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				if (this.quest != null && !this.quest.hidden)
				{
					yield return base.Option_ViewInQuestsTab("ViewQuest", false);
				}
				if (this.lookTargets.IsValid())
				{
					yield return base.Option_JumpToLocation;
				}
				yield return base.Option_Close;
				yield break;
			}
		}

		// Token: 0x06002407 RID: 9223 RVA: 0x000E5F50 File Offset: 0x000E4150
		public override void OpenLetter()
		{
			if (this.quest != null && !base.ArchivedOnly)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
				((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.quest);
				Find.LetterStack.RemoveLetter(this);
				return;
			}
			base.OpenLetter();
		}
	}
}
