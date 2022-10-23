using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004F7 RID: 1271
	public class Dialog_InfoCard : Window
	{
		// Token: 0x060026BA RID: 9914 RVA: 0x000F8E9C File Offset: 0x000F709C
		public static IEnumerable<Dialog_InfoCard.Hyperlink> DefsToHyperlinks(IEnumerable<ThingDef> defs)
		{
			if (defs == null)
			{
				return null;
			}
			return from def in defs
			select new Dialog_InfoCard.Hyperlink(def, -1);
		}

		// Token: 0x060026BB RID: 9915 RVA: 0x000F8EC8 File Offset: 0x000F70C8
		public static IEnumerable<Dialog_InfoCard.Hyperlink> DefsToHyperlinks(IEnumerable<DefHyperlink> links)
		{
			if (links == null)
			{
				return null;
			}
			return from link in links
			select new Dialog_InfoCard.Hyperlink(link.def, -1);
		}

		// Token: 0x060026BC RID: 9916 RVA: 0x000F8EF4 File Offset: 0x000F70F4
		public static IEnumerable<Dialog_InfoCard.Hyperlink> TitleDefsToHyperlinks(IEnumerable<DefHyperlink> links)
		{
			if (links == null)
			{
				return null;
			}
			return from link in links
			select new Dialog_InfoCard.Hyperlink((RoyalTitleDef)link.def, link.faction, -1);
		}

		// Token: 0x060026BD RID: 9917 RVA: 0x000F8F20 File Offset: 0x000F7120
		public static void PushCurrentToHistoryAndClose()
		{
			Dialog_InfoCard dialog_InfoCard = Find.WindowStack.WindowOfType<Dialog_InfoCard>();
			if (dialog_InfoCard == null)
			{
				return;
			}
			Dialog_InfoCard.history.Add(new Dialog_InfoCard.Hyperlink(dialog_InfoCard, StatsReportUtility.SelectedStatIndex));
			Find.WindowStack.TryRemove(dialog_InfoCard, false);
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x060026BE RID: 9918 RVA: 0x000F8F5E File Offset: 0x000F715E
		private Def Def
		{
			get
			{
				if (this.thing != null)
				{
					return this.thing.def;
				}
				if (this.worldObject != null)
				{
					return this.worldObject.def;
				}
				return this.def;
			}
		}

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x060026BF RID: 9919 RVA: 0x000F8F8E File Offset: 0x000F718E
		private Pawn ThingPawn
		{
			get
			{
				return this.thing as Pawn;
			}
		}

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x060026C0 RID: 9920 RVA: 0x000F8F9B File Offset: 0x000F719B
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(950f, 760f);
			}
		}

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x060026C1 RID: 9921 RVA: 0x00004E2A File Offset: 0x0000302A
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x060026C2 RID: 9922 RVA: 0x000F8FAC File Offset: 0x000F71AC
		public override QuickSearchWidget CommonSearchWidget
		{
			get
			{
				if (this.tab != Dialog_InfoCard.InfoCardTab.Stats)
				{
					return null;
				}
				return StatsReportUtility.QuickSearchWidget;
			}
		}

		// Token: 0x060026C3 RID: 9923 RVA: 0x000F8FBD File Offset: 0x000F71BD
		public Dialog_InfoCard(Thing thing, Precept_ThingStyle precept = null)
		{
			this.thing = thing;
			this.precept = precept;
			this.tab = Dialog_InfoCard.InfoCardTab.Stats;
			this.Setup();
		}

		// Token: 0x060026C4 RID: 9924 RVA: 0x000F8FE0 File Offset: 0x000F71E0
		public Dialog_InfoCard(Def onlyDef, Precept_ThingStyle precept = null)
		{
			this.def = onlyDef;
			this.precept = precept;
			this.Setup();
		}

		// Token: 0x060026C5 RID: 9925 RVA: 0x000F8FFC File Offset: 0x000F71FC
		public Dialog_InfoCard(ThingDef thingDef, ThingDef stuff, Precept_ThingStyle precept = null)
		{
			this.def = thingDef;
			this.stuff = stuff;
			this.precept = precept;
			this.Setup();
		}

		// Token: 0x060026C6 RID: 9926 RVA: 0x000F901F File Offset: 0x000F721F
		public Dialog_InfoCard(RoyalTitleDef titleDef, Faction faction, Pawn pawn = null)
		{
			this.titleDef = titleDef;
			this.faction = faction;
			this.pawn = pawn;
			this.Setup();
		}

		// Token: 0x060026C7 RID: 9927 RVA: 0x000F9042 File Offset: 0x000F7242
		public Dialog_InfoCard(Hediff hediff)
		{
			this.hediff = hediff;
			this.Setup();
		}

		// Token: 0x060026C8 RID: 9928 RVA: 0x000F9057 File Offset: 0x000F7257
		public Dialog_InfoCard(Faction faction)
		{
			this.faction = faction;
			this.Setup();
		}

		// Token: 0x060026C9 RID: 9929 RVA: 0x000F906C File Offset: 0x000F726C
		public Dialog_InfoCard(WorldObject worldObject)
		{
			this.worldObject = worldObject;
			this.Setup();
		}

		// Token: 0x060026CA RID: 9930 RVA: 0x000F9081 File Offset: 0x000F7281
		public override void Notify_CommonSearchChanged()
		{
			StatsReportUtility.Notify_QuickSearchChanged();
		}

		// Token: 0x060026CB RID: 9931 RVA: 0x000F9088 File Offset: 0x000F7288
		public override void Close(bool doCloseSound = true)
		{
			base.Close(doCloseSound);
			Dialog_InfoCard.history.Clear();
		}

		// Token: 0x060026CC RID: 9932 RVA: 0x000F909C File Offset: 0x000F729C
		private void Setup()
		{
			this.forcePause = true;
			this.doCloseButton = true;
			this.doCloseX = true;
			this.absorbInputAroundWindow = true;
			this.closeOnClickedOutside = true;
			this.soundAppear = SoundDefOf.InfoCard_Open;
			this.soundClose = SoundDefOf.InfoCard_Close;
			StatsReportUtility.Reset();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.InfoCard, KnowledgeAmount.Total);
		}

		// Token: 0x060026CD RID: 9933 RVA: 0x000F90F2 File Offset: 0x000F72F2
		public void SetTab(Dialog_InfoCard.InfoCardTab infoCardTab)
		{
			this.tab = infoCardTab;
		}

		// Token: 0x060026CE RID: 9934 RVA: 0x000F90FC File Offset: 0x000F72FC
		private static bool ShowMaterialsButton(Rect containerRect, bool withBackButtonOffset = false)
		{
			float num = containerRect.x + containerRect.width - 14f - 200f - 16f;
			if (withBackButtonOffset)
			{
				num -= 136f;
			}
			return Widgets.ButtonText(new Rect(num, containerRect.y + 18f, 200f, 40f), "ShowMaterials".Translate(), true, true, true, null);
		}

		// Token: 0x060026CF RID: 9935 RVA: 0x000F9174 File Offset: 0x000F7374
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(inRect);
			rect = rect.ContractedBy(18f);
			rect.height = 34f;
			rect.x += 34f;
			Text.Font = GameFont.Medium;
			Widgets.Label(rect, this.GetTitle());
			Rect rect2 = new Rect(inRect.x + 9f, rect.y, 34f, 34f);
			if (this.thing != null)
			{
				Widgets.ThingIcon(rect2, this.thing, 1f, null, false);
			}
			else
			{
				Widgets.DefIcon(rect2, this.def, this.stuff, 1f, null, true, null, null, null);
			}
			Rect rect3 = new Rect(inRect);
			rect3.yMin = rect.yMax;
			rect3.yMax -= 38f;
			Rect rect4 = rect3;
			List<TabRecord> list = new List<TabRecord>();
			TabRecord item = new TabRecord("TabStats".Translate(), delegate()
			{
				this.tab = Dialog_InfoCard.InfoCardTab.Stats;
			}, this.tab == Dialog_InfoCard.InfoCardTab.Stats);
			list.Add(item);
			if (this.ThingPawn != null)
			{
				if (this.ThingPawn.RaceProps.Humanlike)
				{
					TabRecord item2 = new TabRecord("TabCharacter".Translate(), delegate()
					{
						this.tab = Dialog_InfoCard.InfoCardTab.Character;
					}, this.tab == Dialog_InfoCard.InfoCardTab.Character);
					list.Add(item2);
				}
				TabRecord item3 = new TabRecord("TabHealth".Translate(), delegate()
				{
					this.tab = Dialog_InfoCard.InfoCardTab.Health;
				}, this.tab == Dialog_InfoCard.InfoCardTab.Health);
				list.Add(item3);
				if (ModsConfig.RoyaltyActive && this.ThingPawn.RaceProps.Humanlike && this.ThingPawn.Faction == Faction.OfPlayer && !this.ThingPawn.IsQuestLodger() && this.ThingPawn.royalty != null && PermitsCardUtility.selectedFaction != null)
				{
					TabRecord item4 = new TabRecord("TabPermits".Translate(), delegate()
					{
						this.tab = Dialog_InfoCard.InfoCardTab.Permits;
					}, this.tab == Dialog_InfoCard.InfoCardTab.Permits);
					list.Add(item4);
				}
				TabRecord item5 = new TabRecord("TabRecords".Translate(), delegate()
				{
					this.tab = Dialog_InfoCard.InfoCardTab.Records;
				}, this.tab == Dialog_InfoCard.InfoCardTab.Records);
				list.Add(item5);
			}
			if (list.Count > 1)
			{
				rect4.yMin += 45f;
				TabDrawer.DrawTabs<TabRecord>(rect4, list, 200f);
			}
			this.FillCard(rect4.ContractedBy(18f));
			if (this.def != null && this.def is BuildableDef)
			{
				IEnumerable<ThingDef> enumerable = GenStuff.AllowedStuffsFor((BuildableDef)this.def, TechLevel.Undefined);
				if (enumerable.Count<ThingDef>() > 0 && Dialog_InfoCard.ShowMaterialsButton(inRect, Dialog_InfoCard.history.Count > 0))
				{
					List<FloatMenuOption> list2 = new List<FloatMenuOption>();
					foreach (ThingDef thingDef in enumerable)
					{
						ThingDef localStuff = thingDef;
						list2.Add(new FloatMenuOption(thingDef.LabelCap, delegate()
						{
							this.stuff = localStuff;
							this.Setup();
						}, thingDef, null, false, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, null));
					}
					Find.WindowStack.Add(new FloatMenu(list2));
				}
			}
			if (Dialog_InfoCard.history.Count > 0 && Widgets.BackButtonFor(inRect))
			{
				Dialog_InfoCard.Hyperlink hyperlink = Dialog_InfoCard.history[Dialog_InfoCard.history.Count - 1];
				Dialog_InfoCard.history.RemoveAt(Dialog_InfoCard.history.Count - 1);
				Find.WindowStack.TryRemove(typeof(Dialog_InfoCard), false);
				hyperlink.ActivateHyperlink();
			}
		}

		// Token: 0x060026D0 RID: 9936 RVA: 0x000F9578 File Offset: 0x000F7778
		protected void FillCard(Rect cardRect)
		{
			if (this.tab == Dialog_InfoCard.InfoCardTab.Stats)
			{
				if (this.thing != null)
				{
					Thing innerThing = this.thing;
					MinifiedThing minifiedThing = this.thing as MinifiedThing;
					if (minifiedThing != null)
					{
						innerThing = minifiedThing.InnerThing;
					}
					StatsReportUtility.DrawStatsReport(cardRect, innerThing);
				}
				else if (this.titleDef != null)
				{
					StatsReportUtility.DrawStatsReport(cardRect, this.titleDef, this.faction, this.pawn);
				}
				else if (this.hediff != null)
				{
					StatsReportUtility.DrawStatsReport(cardRect, this.hediff);
				}
				else if (this.faction != null)
				{
					StatsReportUtility.DrawStatsReport(cardRect, this.faction);
				}
				else if (this.worldObject != null)
				{
					StatsReportUtility.DrawStatsReport(cardRect, this.worldObject);
				}
				else if (this.def is AbilityDef)
				{
					StatsReportUtility.DrawStatsReport(cardRect, (AbilityDef)this.def);
				}
				else
				{
					StatsReportUtility.DrawStatsReport(cardRect, this.def, this.stuff);
				}
			}
			else if (this.tab == Dialog_InfoCard.InfoCardTab.Character)
			{
				CharacterCardUtility.DrawCharacterCard(cardRect, (Pawn)this.thing, null, default(Rect), true);
			}
			else if (this.tab == Dialog_InfoCard.InfoCardTab.Health)
			{
				HealthCardUtility.DrawPawnHealthCard(cardRect, (Pawn)this.thing, false, false, null);
			}
			else if (this.tab == Dialog_InfoCard.InfoCardTab.Records)
			{
				RecordsCardUtility.DrawRecordsCard(cardRect, (Pawn)this.thing);
			}
			else if (this.tab == Dialog_InfoCard.InfoCardTab.Permits)
			{
				PermitsCardUtility.DrawRecordsCard(cardRect, (Pawn)this.thing);
			}
			if (this.executeAfterFillCardOnce != null)
			{
				this.executeAfterFillCardOnce();
				this.executeAfterFillCardOnce = null;
			}
		}

		// Token: 0x060026D1 RID: 9937 RVA: 0x000F9700 File Offset: 0x000F7900
		private string GetTitle()
		{
			if (this.thing != null)
			{
				if (this.precept == null)
				{
					return this.thing.LabelCapNoCount;
				}
				return this.precept.LabelCap;
			}
			else
			{
				if (this.worldObject != null)
				{
					return this.worldObject.LabelCap;
				}
				if (this.hediff != null)
				{
					return this.hediff.def.LabelCap;
				}
				ThingDef thingDef = this.Def as ThingDef;
				if (thingDef != null)
				{
					if (this.precept == null)
					{
						return GenLabel.ThingLabel(thingDef, this.stuff, 1).CapitalizeFirst();
					}
					return this.precept.LabelCap;
				}
				else
				{
					AbilityDef abilityDef = this.Def as AbilityDef;
					if (abilityDef != null)
					{
						return abilityDef.LabelCap;
					}
					if (this.titleDef != null)
					{
						return this.titleDef.GetLabelCapForBothGenders();
					}
					if (this.faction != null)
					{
						return this.faction.Name;
					}
					if (this.precept == null)
					{
						return this.Def.LabelCap;
					}
					return this.precept.LabelCap;
				}
			}
		}

		// Token: 0x04001956 RID: 6486
		private const float ShowMaterialsButtonWidth = 200f;

		// Token: 0x04001957 RID: 6487
		private const float ShowMaterialsButtonHeight = 40f;

		// Token: 0x04001958 RID: 6488
		private const float ShowMaterialsMargin = 16f;

		// Token: 0x04001959 RID: 6489
		private Action executeAfterFillCardOnce;

		// Token: 0x0400195A RID: 6490
		private static List<Dialog_InfoCard.Hyperlink> history = new List<Dialog_InfoCard.Hyperlink>();

		// Token: 0x0400195B RID: 6491
		private Thing thing;

		// Token: 0x0400195C RID: 6492
		private ThingDef stuff;

		// Token: 0x0400195D RID: 6493
		private Precept_ThingStyle precept;

		// Token: 0x0400195E RID: 6494
		private Def def;

		// Token: 0x0400195F RID: 6495
		private WorldObject worldObject;

		// Token: 0x04001960 RID: 6496
		private RoyalTitleDef titleDef;

		// Token: 0x04001961 RID: 6497
		private Faction faction;

		// Token: 0x04001962 RID: 6498
		private Pawn pawn;

		// Token: 0x04001963 RID: 6499
		private Hediff hediff;

		// Token: 0x04001964 RID: 6500
		private Dialog_InfoCard.InfoCardTab tab;

		// Token: 0x020020DF RID: 8415
		public enum InfoCardTab : byte
		{
			// Token: 0x0400827A RID: 33402
			Stats,
			// Token: 0x0400827B RID: 33403
			Character,
			// Token: 0x0400827C RID: 33404
			Health,
			// Token: 0x0400827D RID: 33405
			Records,
			// Token: 0x0400827E RID: 33406
			Permits
		}

		// Token: 0x020020E0 RID: 8416
		public struct Hyperlink
		{
			// Token: 0x17001F2E RID: 7982
			// (get) Token: 0x0600C54D RID: 50509 RVA: 0x0043BAE3 File Offset: 0x00439CE3
			public bool HasGeneOwnerThing
			{
				get
				{
					return this.thingIsGeneOwner && this.thing != null;
				}
			}

			// Token: 0x17001F2F RID: 7983
			// (get) Token: 0x0600C54E RID: 50510 RVA: 0x0043BAF8 File Offset: 0x00439CF8
			public string Label
			{
				get
				{
					string result = null;
					if (this.worldObject != null)
					{
						result = this.worldObject.Label;
					}
					else if (this.def != null && this.def is ThingDef && this.stuff != null)
					{
						result = (this.def as ThingDef).label;
					}
					else if (this.def != null)
					{
						result = this.def.label;
					}
					else if (this.thing != null && !this.thingIsGeneOwner)
					{
						result = this.thing.Label;
					}
					else if (this.titleDef != null)
					{
						result = this.titleDef.GetLabelCapForBothGenders();
					}
					else if (this.quest != null)
					{
						result = this.quest.name;
					}
					else if (this.ideo != null)
					{
						result = this.ideo.name;
					}
					else if (this.HasGeneOwnerThing)
					{
						result = "InspectGenes".Translate();
					}
					return result;
				}
			}

			// Token: 0x0600C54F RID: 50511 RVA: 0x0043BBE4 File Offset: 0x00439DE4
			public Hyperlink(Dialog_InfoCard infoCard, int statIndex = -1)
			{
				this.def = infoCard.def;
				this.thing = infoCard.thing;
				this.stuff = infoCard.stuff;
				this.worldObject = infoCard.worldObject;
				this.titleDef = infoCard.titleDef;
				this.faction = infoCard.faction;
				this.selectedStatIndex = statIndex;
				this.quest = null;
				this.ideo = null;
				this.thingIsGeneOwner = false;
			}

			// Token: 0x0600C550 RID: 50512 RVA: 0x0043BC58 File Offset: 0x00439E58
			public Hyperlink(Def def, int statIndex = -1)
			{
				this.def = def;
				this.thing = null;
				this.stuff = null;
				this.worldObject = null;
				this.titleDef = null;
				this.faction = null;
				this.selectedStatIndex = statIndex;
				this.quest = null;
				this.ideo = null;
				this.thingIsGeneOwner = false;
			}

			// Token: 0x0600C551 RID: 50513 RVA: 0x0043BCAC File Offset: 0x00439EAC
			public Hyperlink(RoyalTitleDef titleDef, Faction faction, int statIndex = -1)
			{
				this.def = null;
				this.thing = null;
				this.stuff = null;
				this.worldObject = null;
				this.titleDef = titleDef;
				this.faction = faction;
				this.selectedStatIndex = statIndex;
				this.quest = null;
				this.ideo = null;
				this.thingIsGeneOwner = false;
			}

			// Token: 0x0600C552 RID: 50514 RVA: 0x0043BD00 File Offset: 0x00439F00
			public Hyperlink(Thing thing, int statIndex = -1, bool thingIsGeneOwner = false)
			{
				this.thing = thing;
				this.stuff = null;
				this.def = null;
				this.worldObject = null;
				this.titleDef = null;
				this.faction = null;
				this.selectedStatIndex = statIndex;
				this.quest = null;
				this.ideo = null;
				this.thingIsGeneOwner = thingIsGeneOwner;
			}

			// Token: 0x0600C553 RID: 50515 RVA: 0x0043BD54 File Offset: 0x00439F54
			public Hyperlink(Quest quest, int statIndex = -1)
			{
				this.def = null;
				this.thing = null;
				this.stuff = null;
				this.worldObject = null;
				this.titleDef = null;
				this.faction = null;
				this.selectedStatIndex = statIndex;
				this.quest = quest;
				this.ideo = null;
				this.thingIsGeneOwner = false;
			}

			// Token: 0x0600C554 RID: 50516 RVA: 0x0043BDA8 File Offset: 0x00439FA8
			public Hyperlink(Ideo ideo)
			{
				this.def = null;
				this.thing = null;
				this.stuff = null;
				this.worldObject = null;
				this.titleDef = null;
				this.faction = null;
				this.selectedStatIndex = 0;
				this.quest = null;
				this.ideo = ideo;
				this.thingIsGeneOwner = false;
			}

			// Token: 0x0600C555 RID: 50517 RVA: 0x0043BDFC File Offset: 0x00439FFC
			public void ActivateHyperlink()
			{
				if (this.ideo != null)
				{
					Dialog_InfoCard dialog_InfoCard = Find.WindowStack.WindowOfType<Dialog_InfoCard>();
					if (dialog_InfoCard != null)
					{
						dialog_InfoCard.Close(true);
					}
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Ideos, true);
					IdeoUIUtility.SetSelected(this.ideo);
					return;
				}
				if (this.quest != null)
				{
					Dialog_InfoCard dialog_InfoCard2 = Find.WindowStack.WindowOfType<Dialog_InfoCard>();
					if (dialog_InfoCard2 != null)
					{
						dialog_InfoCard2.Close(true);
					}
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
					((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.quest);
					return;
				}
				if (this.HasGeneOwnerThing)
				{
					if (ThingSelectionUtility.SelectableByMapClick(this.thing))
					{
						Find.Selector.Select(this.thing, true, true);
						InspectPaneUtility.OpenTab(typeof(ITab_Genes));
					}
					return;
				}
				Dialog_InfoCard dialog_InfoCard3 = null;
				if (this.def == null && this.thing == null && this.worldObject == null && this.titleDef == null)
				{
					dialog_InfoCard3 = Find.WindowStack.WindowOfType<Dialog_InfoCard>();
				}
				else
				{
					Dialog_InfoCard.PushCurrentToHistoryAndClose();
					if (this.worldObject != null)
					{
						dialog_InfoCard3 = new Dialog_InfoCard(this.worldObject);
					}
					else if (this.def != null && this.def is ThingDef && (this.stuff != null || GenStuff.DefaultStuffFor((ThingDef)this.def) != null))
					{
						dialog_InfoCard3 = new Dialog_InfoCard(this.def as ThingDef, this.stuff ?? GenStuff.DefaultStuffFor((ThingDef)this.def), null);
					}
					else if (this.def != null)
					{
						dialog_InfoCard3 = new Dialog_InfoCard(this.def, null);
					}
					else if (this.thing != null)
					{
						dialog_InfoCard3 = new Dialog_InfoCard(this.thing, null);
					}
					else if (this.titleDef != null)
					{
						dialog_InfoCard3 = new Dialog_InfoCard(this.titleDef, this.faction, null);
					}
				}
				if (dialog_InfoCard3 == null)
				{
					return;
				}
				int localSelectedStatIndex = this.selectedStatIndex;
				if (this.selectedStatIndex >= 0)
				{
					dialog_InfoCard3.executeAfterFillCardOnce = delegate()
					{
						StatsReportUtility.SelectEntry(localSelectedStatIndex);
					};
				}
				Find.WindowStack.Add(dialog_InfoCard3);
			}

			// Token: 0x0400827F RID: 33407
			public Thing thing;

			// Token: 0x04008280 RID: 33408
			public ThingDef stuff;

			// Token: 0x04008281 RID: 33409
			public Def def;

			// Token: 0x04008282 RID: 33410
			public WorldObject worldObject;

			// Token: 0x04008283 RID: 33411
			public RoyalTitleDef titleDef;

			// Token: 0x04008284 RID: 33412
			public Faction faction;

			// Token: 0x04008285 RID: 33413
			public Quest quest;

			// Token: 0x04008286 RID: 33414
			public Ideo ideo;

			// Token: 0x04008287 RID: 33415
			public int selectedStatIndex;

			// Token: 0x04008288 RID: 33416
			public bool thingIsGeneOwner;
		}
	}
}
