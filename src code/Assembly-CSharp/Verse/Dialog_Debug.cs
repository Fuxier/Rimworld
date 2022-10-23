using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200045E RID: 1118
	public class Dialog_Debug : Window
	{
		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x0600225A RID: 8794 RVA: 0x00002662 File Offset: 0x00000862
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x0600225B RID: 8795 RVA: 0x00004E17 File Offset: 0x00003017
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x0600225C RID: 8796 RVA: 0x000DB5A0 File Offset: 0x000D97A0
		public string Filter
		{
			get
			{
				return this.filter;
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x0600225D RID: 8797 RVA: 0x000DB5A8 File Offset: 0x000D97A8
		private float FilterX
		{
			get
			{
				DebugActionNode debugActionNode = this.currentNode;
				if (((debugActionNode != null) ? debugActionNode.parent : null) == null || !this.currentNode.parent.IsRoot)
				{
					return 130f;
				}
				return 0f;
			}
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x0600225E RID: 8798 RVA: 0x000DB5DB File Offset: 0x000D97DB
		private int HighlightedIndex
		{
			get
			{
				return this.currentTabMenu.HighlightedIndex(this.currentHighlightIndex, this.prioritizedHighlightedIndex);
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x0600225F RID: 8799 RVA: 0x000DB5F4 File Offset: 0x000D97F4
		public DebugActionNode CurrentNode
		{
			get
			{
				return this.currentNode;
			}
		}

		// Token: 0x06002260 RID: 8800 RVA: 0x000DB5FC File Offset: 0x000D97FC
		public Dialog_Debug()
		{
			this.Setup();
			this.SwitchTab(DebugTabMenuDefOf.Actions);
		}

		// Token: 0x06002261 RID: 8801 RVA: 0x000DB636 File Offset: 0x000D9836
		public Dialog_Debug(DebugTabMenuDef def)
		{
			this.Setup();
			this.SwitchTab(def);
		}

		// Token: 0x06002262 RID: 8802 RVA: 0x000DB66C File Offset: 0x000D986C
		private void Setup()
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.onlyOneOfTypeAllowed = true;
			this.absorbInputAroundWindow = true;
			this.focusFilter = true;
			this.menuDefsSorted.AddRange(DefDatabase<DebugTabMenuDef>.AllDefs.ToList<DebugTabMenuDef>());
			this.menuDefsSorted.SortBy((DebugTabMenuDef x) => x.displayOrder, (DebugTabMenuDef y) => y.label);
		}

		// Token: 0x06002263 RID: 8803 RVA: 0x000DB6FC File Offset: 0x000D98FC
		public void SwitchTab(DebugTabMenuDef def)
		{
			Dialog_Debug.TrySetupNodeGraph();
			this.scrollPosition = Vector2.zero;
			this.currentHighlightIndex = 0;
			this.prioritizedHighlightedIndex = 0;
			this.currentTabMenu = (this.menus.ContainsKey(def) ? this.menus[def] : DebugTabMenu.CreateMenu(def, this, Dialog_Debug.rootNode));
			this.currentTabMenu.Enter(Dialog_Debug.roots[def]);
		}

		// Token: 0x06002264 RID: 8804 RVA: 0x000DB76C File Offset: 0x000D996C
		public static void TrySetupNodeGraph()
		{
			if (Dialog_Debug.rootNode != null)
			{
				return;
			}
			Dialog_Debug.rootNode = new DebugActionNode("Root", DebugActionType.Action, null, null);
			foreach (DebugTabMenuDef debugTabMenuDef in DefDatabase<DebugTabMenuDef>.AllDefs)
			{
				Dialog_Debug.roots.Add(debugTabMenuDef, DebugTabMenu.CreateMenu(debugTabMenuDef, null, Dialog_Debug.rootNode).InitActions(Dialog_Debug.rootNode));
			}
		}

		// Token: 0x06002265 RID: 8805 RVA: 0x000DB7EC File Offset: 0x000D99EC
		private void DrawTabs(Rect rect)
		{
			this.tabs.Clear();
			using (List<DebugTabMenuDef>.Enumerator enumerator = this.menuDefsSorted.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DebugTabMenuDef d = enumerator.Current;
					this.tabs.Add(new TabRecord(d.LabelCap, delegate()
					{
						this.SwitchTab(d);
					}, this.currentTabMenu.def == d));
				}
			}
			TabDrawer.DrawTabs<TabRecord>(rect, this.tabs, 200f);
		}

		// Token: 0x06002266 RID: 8806 RVA: 0x000DB8A8 File Offset: 0x000D9AA8
		public override void DoWindowContents(Rect inRect)
		{
			GUI.SetNextControlName("DebugFilter");
			Text.Font = GameFont.Small;
			Rect rect = new Rect(this.FilterX, 0f, Dialog_Debug.FilterInputSize.x, Dialog_Debug.FilterInputSize.y);
			this.filter = Widgets.TextField(rect, this.filter);
			Rect rect2 = new Rect(rect.xMax + 10f, 32f, inRect.width - rect.width - 10f, 32f);
			this.DrawTabs(rect2);
			if ((Event.current.type == EventType.KeyDown || Event.current.type == EventType.Repaint) && this.focusFilter)
			{
				GUI.FocusControl("DebugFilter");
				this.filter = string.Empty;
				this.focusFilter = false;
			}
			if (KeyBindingDefOf.Dev_ChangeSelectedDebugAction.IsDownEvent)
			{
				int highlightedIndex = this.HighlightedIndex;
				if (highlightedIndex >= 0)
				{
					for (int i = 0; i < this.currentTabMenu.Count; i++)
					{
						int index = (highlightedIndex + i + 1) % this.currentTabMenu.Count;
						if (this.FilterAllows(this.currentTabMenu.LabelAtIndex(index)))
						{
							this.prioritizedHighlightedIndex = index;
							break;
						}
					}
				}
			}
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight = 0f;
			}
			Rect outRect = new Rect(inRect);
			outRect.yMin += 42f;
			int num = (int)(this.InitialSize.x / 200f);
			float height = Mathf.Max(outRect.height, (this.totalOptionsHeight + 50f * (float)(num - 1)) / (float)num);
			Rect rect3 = new Rect(0f, 0f, outRect.width - 16f, height);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect3, true);
			this.listing = new Listing_Standard(inRect, () => this.scrollPosition);
			this.listing.ColumnWidth = (rect3.width - 17f * (float)(num - 1)) / (float)num;
			this.listing.Begin(rect3);
			this.currentTabMenu.ListOptions(this.HighlightedIndex);
			this.listing.End();
			Widgets.EndScrollView();
			if (this.currentNode.parent != null && !this.currentNode.parent.IsRoot)
			{
				GameFont font = Text.Font;
				Text.Font = GameFont.Small;
				if (Widgets.ButtonText(new Rect(0f, 0f, 120f, 32f), "Back", true, true, true, null))
				{
					this.currentNode.parent.Enter(this);
				}
				if (!this.currentNode.IsRoot)
				{
					Text.Anchor = TextAnchor.UpperRight;
					Text.Font = GameFont.Tiny;
					Widgets.Label(new Rect(0f, 0f, outRect.width - 24f - 10f, 32f), this.currentNode.Path.Colorize(ColoredText.SubtleGrayColor));
					Text.Anchor = TextAnchor.UpperLeft;
				}
				Text.Font = font;
			}
		}

		// Token: 0x06002267 RID: 8807 RVA: 0x000DBBB4 File Offset: 0x000D9DB4
		public override void OnAcceptKeyPressed()
		{
			if (GUI.GetNameOfFocusedControl() == "DebugFilter")
			{
				int highlightedIndex = this.HighlightedIndex;
				this.currentTabMenu.OnAcceptKeyPressed(highlightedIndex);
				Event.current.Use();
			}
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x000DBBF0 File Offset: 0x000D9DF0
		public override void OnCancelKeyPressed()
		{
			if (this.currentNode.parent != null && !this.currentNode.parent.IsRoot)
			{
				this.currentNode.parent.Enter(this);
				Event.current.Use();
				return;
			}
			base.OnCancelKeyPressed();
		}

		// Token: 0x06002269 RID: 8809 RVA: 0x000DBC40 File Offset: 0x000D9E40
		public static DebugActionNode GetNode(string path)
		{
			Dialog_Debug.<>c__DisplayClass41_0 CS$<>8__locals1 = new Dialog_Debug.<>c__DisplayClass41_0();
			Dialog_Debug.TrySetupNodeGraph();
			DebugActionNode debugActionNode = Dialog_Debug.rootNode;
			CS$<>8__locals1.s = path.Split(new char[]
			{
				'\\'
			});
			int i;
			int j;
			for (i = 0; i < CS$<>8__locals1.s.Length; i = j + 1)
			{
				DebugActionNode debugActionNode2 = debugActionNode.children.FirstOrDefault((DebugActionNode x) => x.label == CS$<>8__locals1.s[i]);
				if (debugActionNode2 == null)
				{
					return null;
				}
				debugActionNode = debugActionNode2;
				debugActionNode.TrySetupChildren();
				j = i;
			}
			return debugActionNode;
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x000DBCD8 File Offset: 0x000D9ED8
		public void SetCurrentNode(DebugActionNode node)
		{
			this.currentNode = node;
			foreach (DebugActionNode debugActionNode in this.currentNode.children)
			{
				debugActionNode.DirtyLabelCache();
			}
			this.scrollPosition = Vector2.zero;
			this.filter = string.Empty;
			this.currentHighlightIndex = 0;
			this.prioritizedHighlightedIndex = 0;
			DebugTabMenu debugTabMenu = this.currentTabMenu;
			if (debugTabMenu == null)
			{
				return;
			}
			debugTabMenu.Recache();
		}

		// Token: 0x0600226B RID: 8811 RVA: 0x000DBD68 File Offset: 0x000D9F68
		public void DrawNode(DebugActionNode node, bool highlight)
		{
			if (node.settingsField != null)
			{
				this.DoCheckbox(node, highlight);
				return;
			}
			this.DoButton(node, highlight);
		}

		// Token: 0x0600226C RID: 8812 RVA: 0x000DBD8C File Offset: 0x000D9F8C
		private void DoButton(DebugActionNode node, bool highlight)
		{
			string labelNow = node.LabelNow;
			if (!this.FilterAllows(labelNow))
			{
				GUI.color = Dialog_Debug.DisallowedColor;
			}
			DebugActionButtonResult debugActionButtonResult = this.listing.ButtonDebugPinnable(labelNow, highlight, Prefs.DebugActionsPalette.Contains(node.Path));
			if (debugActionButtonResult != DebugActionButtonResult.ButtonPressed)
			{
				if (debugActionButtonResult == DebugActionButtonResult.PinPressed)
				{
					Dialog_DevPalette.ToggleAction(node.Path);
				}
			}
			else
			{
				node.Enter(this);
			}
			GUI.color = Color.white;
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight += 22f + this.listing.verticalSpacing;
			}
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x000DBE24 File Offset: 0x000DA024
		private void DoCheckbox(DebugActionNode node, bool highlight)
		{
			string labelNow = node.LabelNow;
			FieldInfo settingsField = node.settingsField;
			bool flag = (bool)settingsField.GetValue(null);
			bool flag2 = flag;
			if (!this.FilterAllows(labelNow))
			{
				GUI.color = Dialog_Debug.DisallowedColor;
			}
			DebugActionButtonResult debugActionButtonResult = this.listing.CheckboxPinnable(labelNow, ref flag, highlight, Prefs.DebugActionsPalette.Contains(node.Path));
			if (debugActionButtonResult != DebugActionButtonResult.ButtonPressed)
			{
				if (debugActionButtonResult == DebugActionButtonResult.PinPressed)
				{
					Dialog_DevPalette.ToggleAction(node.Path);
				}
			}
			else
			{
				node.Enter(this);
			}
			GUI.color = Color.white;
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight += Text.LineHeight;
			}
			if (flag != flag2)
			{
				settingsField.SetValue(null, flag);
				MethodInfo method = settingsField.DeclaringType.GetMethod(settingsField.Name + "Toggled", BindingFlags.Static | BindingFlags.Public);
				if (method != null)
				{
					method.Invoke(null, null);
				}
			}
		}

		// Token: 0x0600226E RID: 8814 RVA: 0x000DBF10 File Offset: 0x000DA110
		public void DoLabel(string label)
		{
			Text.Font = GameFont.Small;
			this.listing.Label(label, -1f, null);
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight += Text.CalcHeight(label, 300f) + 2f;
			}
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x000DBF61 File Offset: 0x000DA161
		public void DoGap(float gapSize = 7f)
		{
			this.listing.Gap(gapSize);
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight += gapSize;
			}
		}

		// Token: 0x06002270 RID: 8816 RVA: 0x000DBF8A File Offset: 0x000DA18A
		public bool FilterAllows(string label)
		{
			return this.filter.NullOrEmpty() || label.NullOrEmpty() || label.IndexOf(this.filter, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x000DBFB8 File Offset: 0x000DA1B8
		public static void ResetStaticData()
		{
			Dialog_Debug.rootNode = null;
			Dialog_Debug.roots.Clear();
		}

		// Token: 0x040015CA RID: 5578
		public static DebugActionNode rootNode;

		// Token: 0x040015CB RID: 5579
		private DebugActionNode currentNode;

		// Token: 0x040015CC RID: 5580
		private List<TabRecord> tabs = new List<TabRecord>();

		// Token: 0x040015CD RID: 5581
		private Dictionary<DebugTabMenuDef, DebugTabMenu> menus = new Dictionary<DebugTabMenuDef, DebugTabMenu>();

		// Token: 0x040015CE RID: 5582
		private static Dictionary<DebugTabMenuDef, DebugActionNode> roots = new Dictionary<DebugTabMenuDef, DebugActionNode>();

		// Token: 0x040015CF RID: 5583
		private List<DebugTabMenuDef> menuDefsSorted = new List<DebugTabMenuDef>();

		// Token: 0x040015D0 RID: 5584
		private DebugTabMenu currentTabMenu;

		// Token: 0x040015D1 RID: 5585
		private float totalOptionsHeight;

		// Token: 0x040015D2 RID: 5586
		private Listing_Standard listing;

		// Token: 0x040015D3 RID: 5587
		private string filter;

		// Token: 0x040015D4 RID: 5588
		private bool focusFilter;

		// Token: 0x040015D5 RID: 5589
		private int currentHighlightIndex;

		// Token: 0x040015D6 RID: 5590
		private int prioritizedHighlightedIndex;

		// Token: 0x040015D7 RID: 5591
		private Vector2 scrollPosition;

		// Token: 0x040015D8 RID: 5592
		private const string FilterControlName = "DebugFilter";

		// Token: 0x040015D9 RID: 5593
		private const float DebugOptionsGap = 7f;

		// Token: 0x040015DA RID: 5594
		private static readonly Color DisallowedColor = new Color(1f, 1f, 1f, 0.3f);

		// Token: 0x040015DB RID: 5595
		private static readonly Vector2 FilterInputSize = new Vector2(200f, 30f);

		// Token: 0x040015DC RID: 5596
		private const float AssumedBiggestElementHeight = 50f;

		// Token: 0x040015DD RID: 5597
		private const float BackButtonWidth = 120f;
	}
}
