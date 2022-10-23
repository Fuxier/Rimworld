using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020004A7 RID: 1191
	public abstract class ChoiceLetter : LetterWithTimeout
	{
		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x060023D0 RID: 9168 RVA: 0x000E534E File Offset: 0x000E354E
		// (set) Token: 0x060023D1 RID: 9169 RVA: 0x000E5356 File Offset: 0x000E3556
		public TaggedString Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value.CapitalizeFirst();
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x060023D2 RID: 9170
		public abstract IEnumerable<DiaOption> Choices { get; }

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x060023D3 RID: 9171 RVA: 0x000E5365 File Offset: 0x000E3565
		protected DiaOption Option_Close
		{
			get
			{
				return new DiaOption("Close".Translate())
				{
					action = delegate()
					{
						Find.LetterStack.RemoveLetter(this);
					},
					resolveTree = true
				};
			}
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x060023D4 RID: 9172 RVA: 0x000E5394 File Offset: 0x000E3594
		protected DiaOption Option_JumpToLocation
		{
			get
			{
				GlobalTargetInfo target = this.lookTargets.TryGetPrimaryTarget();
				DiaOption diaOption = new DiaOption("JumpToLocation".Translate());
				diaOption.action = delegate()
				{
					CameraJumper.TryJumpAndSelect(target, CameraJumper.MovementMode.Pan);
					Find.LetterStack.RemoveLetter(this);
				};
				diaOption.resolveTree = true;
				if (!CameraJumper.CanJump(target))
				{
					diaOption.Disable(null);
				}
				return diaOption;
			}
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x060023D5 RID: 9173 RVA: 0x000E5404 File Offset: 0x000E3604
		protected DiaOption Option_JumpToLocationAndPostpone
		{
			get
			{
				GlobalTargetInfo target = this.lookTargets.TryGetPrimaryTarget();
				DiaOption diaOption = new DiaOption("JumpToLocation".Translate());
				diaOption.action = delegate()
				{
					CameraJumper.TryJumpAndSelect(target, CameraJumper.MovementMode.Pan);
				};
				diaOption.resolveTree = true;
				if (!CameraJumper.CanJump(target))
				{
					diaOption.Disable(null);
				}
				return diaOption;
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x060023D6 RID: 9174 RVA: 0x000E546B File Offset: 0x000E366B
		protected DiaOption Option_Reject
		{
			get
			{
				return new DiaOption("RejectLetter".Translate())
				{
					action = delegate()
					{
						Find.LetterStack.RemoveLetter(this);
					},
					resolveTree = true
				};
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x060023D7 RID: 9175 RVA: 0x000E549C File Offset: 0x000E369C
		protected DiaOption Option_Postpone
		{
			get
			{
				DiaOption diaOption = new DiaOption("PostponeLetter".Translate());
				diaOption.resolveTree = true;
				if (this.LastTickBeforeTimeout)
				{
					diaOption.Disable(null);
				}
				return diaOption;
			}
		}

		// Token: 0x060023D8 RID: 9176 RVA: 0x000E54D8 File Offset: 0x000E36D8
		protected DiaOption Option_ViewInQuestsTab(string labelKey = "ViewRelatedQuest", bool postpone = false)
		{
			TaggedString taggedString = labelKey.Translate();
			if (this.title != this.quest.name && !this.quest.hidden)
			{
				taggedString += ": " + this.quest.name;
			}
			DiaOption diaOption = new DiaOption(taggedString);
			diaOption.action = delegate()
			{
				if (this.quest != null)
				{
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
					((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.quest);
					if (!postpone)
					{
						Find.LetterStack.RemoveLetter(this);
					}
				}
			};
			diaOption.resolveTree = true;
			if (this.quest == null || this.quest.hidden)
			{
				diaOption.Disable(null);
			}
			return diaOption;
		}

		// Token: 0x060023D9 RID: 9177 RVA: 0x000E5584 File Offset: 0x000E3784
		protected DiaOption Option_ViewInfoCard(int index)
		{
			int num = (this.hyperlinkThingDefs == null) ? 0 : this.hyperlinkThingDefs.Count;
			if (index >= num)
			{
				return new DiaOption(new Dialog_InfoCard.Hyperlink(this.hyperlinkHediffDefs[index - num], -1));
			}
			return new DiaOption(new Dialog_InfoCard.Hyperlink(this.hyperlinkThingDefs[index], -1));
		}

		// Token: 0x060023DA RID: 9178 RVA: 0x000E55E0 File Offset: 0x000E37E0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Values.Look<TaggedString>(ref this.text, "text", default(TaggedString), false);
			Scribe_Values.Look<bool>(ref this.radioMode, "radioMode", false, false);
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
			Scribe_Collections.Look<ThingDef>(ref this.hyperlinkThingDefs, "hyperlinkThingDefs", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<HediffDef>(ref this.hyperlinkHediffDefs, "hyperlinkHediffDefs", LookMode.Def, Array.Empty<object>());
		}

		// Token: 0x060023DB RID: 9179 RVA: 0x000E566E File Offset: 0x000E386E
		protected override string GetMouseoverText()
		{
			return this.text.Resolve();
		}

		// Token: 0x060023DC RID: 9180 RVA: 0x000E567C File Offset: 0x000E387C
		public override void OpenLetter()
		{
			DiaNode diaNode = new DiaNode(this.text);
			diaNode.options.AddRange(this.Choices);
			Dialog_NodeTreeWithFactionInfo window = new Dialog_NodeTreeWithFactionInfo(diaNode, this.relatedFaction, false, this.radioMode, this.title);
			Find.WindowStack.Add(window);
		}

		// Token: 0x04001722 RID: 5922
		public string title;

		// Token: 0x04001723 RID: 5923
		private TaggedString text;

		// Token: 0x04001724 RID: 5924
		public bool radioMode;

		// Token: 0x04001725 RID: 5925
		public Quest quest;

		// Token: 0x04001726 RID: 5926
		public List<ThingDef> hyperlinkThingDefs;

		// Token: 0x04001727 RID: 5927
		public List<HediffDef> hyperlinkHediffDefs;
	}
}
