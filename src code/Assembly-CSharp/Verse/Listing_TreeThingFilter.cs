using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004B7 RID: 1207
	public class Listing_TreeThingFilter : Listing_Tree
	{
		// Token: 0x06002472 RID: 9330 RVA: 0x000E8A64 File Offset: 0x000E6C64
		public Listing_TreeThingFilter(ThingFilter filter, ThingFilter parentFilter, IEnumerable<ThingDef> forceHiddenDefs, IEnumerable<SpecialThingFilterDef> forceHiddenFilters, List<ThingDef> suppressSmallVolumeTags, QuickSearchFilter searchFilter)
		{
			this.filter = filter;
			this.parentFilter = parentFilter;
			if (forceHiddenDefs != null)
			{
				this.forceHiddenDefs = forceHiddenDefs.ToList<ThingDef>();
			}
			if (forceHiddenFilters != null)
			{
				this.tempForceHiddenSpecialFilters = forceHiddenFilters.ToList<SpecialThingFilterDef>();
			}
			this.suppressSmallVolumeTags = suppressSmallVolumeTags;
			this.searchFilter = searchFilter;
		}

		// Token: 0x06002473 RID: 9331 RVA: 0x000E8AB8 File Offset: 0x000E6CB8
		public void ListCategoryChildren(TreeNode_ThingCategory node, int openMask, Map map, Rect visibleRect)
		{
			this.visibleRect = visibleRect;
			int num = 0;
			foreach (SpecialThingFilterDef sfDef in node.catDef.ParentsSpecialThingFilterDefs)
			{
				if (this.Visible(sfDef, node))
				{
					this.DoSpecialFilter(sfDef, num);
				}
			}
			this.DoCategoryChildren(node, num, openMask, map, false);
		}

		// Token: 0x06002474 RID: 9332 RVA: 0x000E8B2C File Offset: 0x000E6D2C
		private void DoCategoryChildren(TreeNode_ThingCategory node, int indentLevel, int openMask, Map map, bool subtreeMatchedSearch)
		{
			Listing_TreeThingFilter.<>c__DisplayClass13_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.subtreeMatchedSearch = subtreeMatchedSearch;
			List<SpecialThingFilterDef> childSpecialFilters = node.catDef.childSpecialFilters;
			for (int i = 0; i < childSpecialFilters.Count; i++)
			{
				if (this.Visible(childSpecialFilters[i], node))
				{
					this.DoSpecialFilter(childSpecialFilters[i], indentLevel);
				}
			}
			foreach (TreeNode_ThingCategory treeNode_ThingCategory in node.ChildCategoryNodes)
			{
				if (this.Visible(treeNode_ThingCategory) && !this.<DoCategoryChildren>g__HideCategoryDueToSearch|13_0(treeNode_ThingCategory, ref CS$<>8__locals1))
				{
					this.DoCategory(treeNode_ThingCategory, indentLevel, openMask, map, CS$<>8__locals1.subtreeMatchedSearch);
				}
			}
			foreach (ThingDef thingDef in node.catDef.SortedChildThingDefs)
			{
				if (this.Visible(thingDef) && !this.<DoCategoryChildren>g__HideThingDueToSearch|13_1(thingDef, ref CS$<>8__locals1))
				{
					this.DoThingDef(thingDef, indentLevel, map);
				}
			}
		}

		// Token: 0x06002475 RID: 9333 RVA: 0x000E8C4C File Offset: 0x000E6E4C
		private void DoSpecialFilter(SpecialThingFilterDef sfDef, int nestLevel)
		{
			if (!sfDef.configurable)
			{
				return;
			}
			Color? textColor = null;
			if (this.searchFilter.Matches(sfDef))
			{
				this.matchCount++;
			}
			else
			{
				textColor = new Color?(Listing_TreeThingFilter.NoMatchColor);
			}
			if (this.CurrentRowVisibleOnScreen())
			{
				base.LabelLeft("*" + sfDef.LabelCap, sfDef.description, nestLevel, 0f, textColor);
				bool flag = this.filter.Allows(sfDef);
				bool flag2 = flag;
				Widgets.Checkbox(new Vector2(this.LabelWidth, this.curY), ref flag, this.lineHeight, false, true, null, null);
				if (flag != flag2)
				{
					this.filter.SetAllow(sfDef, flag);
				}
			}
			base.EndLine();
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x000E8D0C File Offset: 0x000E6F0C
		private void DoCategory(TreeNode_ThingCategory node, int indentLevel, int openMask, Map map, bool subtreeMatchedSearch)
		{
			Color? textColor = null;
			if (this.searchFilter.Active)
			{
				if (this.CategoryMatches(node))
				{
					subtreeMatchedSearch = true;
					this.matchCount++;
				}
				else
				{
					textColor = new Color?(Listing_TreeThingFilter.NoMatchColor);
				}
			}
			if (this.CurrentRowVisibleOnScreen())
			{
				base.OpenCloseWidget(node, indentLevel, openMask);
				base.LabelLeft(node.LabelCap, node.catDef.description, indentLevel, 0f, textColor);
				MultiCheckboxState multiCheckboxState = this.AllowanceStateOf(node);
				MultiCheckboxState multiCheckboxState2 = Widgets.CheckboxMulti(new Rect(this.LabelWidth, this.curY, this.lineHeight, this.lineHeight), multiCheckboxState, true);
				if (multiCheckboxState != multiCheckboxState2)
				{
					this.filter.SetAllow(node.catDef, multiCheckboxState2 == MultiCheckboxState.On, this.forceHiddenDefs, this.hiddenSpecialFilters);
				}
			}
			base.EndLine();
			if (this.IsOpen(node, openMask))
			{
				this.DoCategoryChildren(node, indentLevel + 1, openMask, map, subtreeMatchedSearch);
			}
		}

		// Token: 0x06002477 RID: 9335 RVA: 0x000E8DF8 File Offset: 0x000E6FF8
		private void DoThingDef(ThingDef tDef, int nestLevel, Map map)
		{
			Color? color = null;
			if (this.searchFilter.Matches(tDef))
			{
				this.matchCount++;
			}
			else
			{
				color = new Color?(Listing_TreeThingFilter.NoMatchColor);
			}
			if (tDef.uiIcon != null && tDef.uiIcon != BaseContent.BadTex)
			{
				nestLevel++;
				Widgets.DefIcon(new Rect(base.XAtIndentLevel(nestLevel) - 6f, this.curY, 20f, 20f), tDef, null, 1f, null, true, color, null, null);
			}
			if (this.CurrentRowVisibleOnScreen())
			{
				object obj = (this.suppressSmallVolumeTags == null || !this.suppressSmallVolumeTags.Contains(tDef)) && tDef.IsStuff && tDef.smallVolume;
				string text = tDef.DescriptionDetailed;
				object obj2 = obj;
				if (obj2 != null)
				{
					text += "\n\n" + "ThisIsSmallVolume".Translate(10.ToStringCached());
				}
				float num = -4f;
				if (obj2 != null)
				{
					Rect rect = new Rect(this.LabelWidth - 19f, this.curY, 19f, 20f);
					Text.Font = GameFont.Tiny;
					Text.Anchor = TextAnchor.UpperRight;
					GUI.color = Color.gray;
					Widgets.Label(rect, "/" + 10.ToStringCached());
					Text.Font = GameFont.Small;
					GenUI.ResetLabelAlign();
					GUI.color = Color.white;
				}
				num -= 19f;
				if (map != null)
				{
					int count = map.resourceCounter.GetCount(tDef);
					if (count > 0)
					{
						string text2 = count.ToStringCached();
						Rect rect2 = new Rect(0f, this.curY, this.LabelWidth + num, 40f);
						Text.Font = GameFont.Tiny;
						Text.Anchor = TextAnchor.UpperRight;
						GUI.color = new Color(0.5f, 0.5f, 0.1f);
						Widgets.Label(rect2, text2);
						num -= Text.CalcSize(text2).x;
						GenUI.ResetLabelAlign();
						Text.Font = GameFont.Small;
						GUI.color = Color.white;
					}
				}
				base.LabelLeft(tDef.LabelCap, text, nestLevel, num, color);
				bool flag = this.filter.Allows(tDef);
				bool flag2 = flag;
				Widgets.Checkbox(new Vector2(this.LabelWidth, this.curY), ref flag, this.lineHeight, false, true, null, null);
				if (flag != flag2)
				{
					this.filter.SetAllow(tDef, flag);
				}
			}
			base.EndLine();
		}

		// Token: 0x06002478 RID: 9336 RVA: 0x000E9064 File Offset: 0x000E7264
		public MultiCheckboxState AllowanceStateOf(TreeNode_ThingCategory cat)
		{
			int num = 0;
			int num2 = 0;
			foreach (ThingDef thingDef in cat.catDef.DescendantThingDefs)
			{
				if (this.Visible(thingDef))
				{
					num++;
					if (this.filter.Allows(thingDef))
					{
						num2++;
					}
				}
			}
			bool flag = false;
			foreach (SpecialThingFilterDef sf in cat.catDef.DescendantSpecialThingFilterDefs)
			{
				if (this.Visible(sf, cat) && !this.filter.Allows(sf))
				{
					flag = true;
					break;
				}
			}
			if (num2 == 0)
			{
				return MultiCheckboxState.Off;
			}
			if (num == num2 && !flag)
			{
				return MultiCheckboxState.On;
			}
			return MultiCheckboxState.Partial;
		}

		// Token: 0x06002479 RID: 9337 RVA: 0x000E9144 File Offset: 0x000E7344
		private bool Visible(ThingDef td)
		{
			if (!td.PlayerAcquirable)
			{
				return false;
			}
			if (this.forceHiddenDefs != null && this.forceHiddenDefs.Contains(td))
			{
				return false;
			}
			if (this.parentFilter != null)
			{
				if (!this.parentFilter.Allows(td))
				{
					return false;
				}
				if (this.parentFilter.IsAlwaysDisallowedDueToSpecialFilters(td))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x000E919C File Offset: 0x000E739C
		public override bool IsOpen(TreeNode node, int openMask)
		{
			TreeNode_ThingCategory node2;
			return base.IsOpen(node, openMask) || ((node2 = (node as TreeNode_ThingCategory)) != null && this.searchFilter.Active && this.ThisOrDescendantsVisibleAndMatchesSearch(node2));
		}

		// Token: 0x0600247B RID: 9339 RVA: 0x000E91D8 File Offset: 0x000E73D8
		private bool ThisOrDescendantsVisibleAndMatchesSearch(TreeNode_ThingCategory node)
		{
			if (this.Visible(node) && this.CategoryMatches(node))
			{
				return true;
			}
			foreach (ThingDef td in node.catDef.childThingDefs)
			{
				if (this.<ThisOrDescendantsVisibleAndMatchesSearch>g__ThingDefVisibleAndMatches|20_0(td))
				{
					return true;
				}
			}
			foreach (SpecialThingFilterDef sf in node.catDef.childSpecialFilters)
			{
				if (this.<ThisOrDescendantsVisibleAndMatchesSearch>g__SpecialFilterVisibleAndMatches|20_1(sf, node))
				{
					return true;
				}
			}
			foreach (ThingCategoryDef thingCategoryDef in node.catDef.childCategories)
			{
				if (this.ThisOrDescendantsVisibleAndMatchesSearch(thingCategoryDef.treeNode))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600247C RID: 9340 RVA: 0x000E92F8 File Offset: 0x000E74F8
		private bool CategoryMatches(TreeNode_ThingCategory node)
		{
			return this.searchFilter.Matches(node.catDef.label);
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x000E9310 File Offset: 0x000E7510
		private bool Visible(TreeNode_ThingCategory node)
		{
			return node.catDef.DescendantThingDefs.Any(new Func<ThingDef, bool>(this.Visible));
		}

		// Token: 0x0600247E RID: 9342 RVA: 0x000E9330 File Offset: 0x000E7530
		private bool Visible(SpecialThingFilterDef filter, TreeNode_ThingCategory node)
		{
			if (this.parentFilter != null && !this.parentFilter.Allows(filter))
			{
				return false;
			}
			if (this.hiddenSpecialFilters == null)
			{
				this.CalculateHiddenSpecialFilters(node);
			}
			for (int i = 0; i < this.hiddenSpecialFilters.Count; i++)
			{
				if (this.hiddenSpecialFilters[i] == filter)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600247F RID: 9343 RVA: 0x000E938C File Offset: 0x000E758C
		private bool CurrentRowVisibleOnScreen()
		{
			Rect other = new Rect(0f, this.curY, base.ColumnWidth, this.lineHeight);
			return this.visibleRect.Overlaps(other);
		}

		// Token: 0x06002480 RID: 9344 RVA: 0x000E93C3 File Offset: 0x000E75C3
		private void CalculateHiddenSpecialFilters(TreeNode_ThingCategory node)
		{
			this.hiddenSpecialFilters = Listing_TreeThingFilter.GetCachedHiddenSpecialFilters(node, this.parentFilter);
			if (this.tempForceHiddenSpecialFilters != null)
			{
				this.hiddenSpecialFilters = new List<SpecialThingFilterDef>(this.hiddenSpecialFilters);
				this.hiddenSpecialFilters.AddRange(this.tempForceHiddenSpecialFilters);
			}
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x000E9404 File Offset: 0x000E7604
		private static List<SpecialThingFilterDef> GetCachedHiddenSpecialFilters(TreeNode_ThingCategory node, ThingFilter parentFilter)
		{
			ValueTuple<TreeNode_ThingCategory, ThingFilter> key = new ValueTuple<TreeNode_ThingCategory, ThingFilter>(node, parentFilter);
			List<SpecialThingFilterDef> list;
			if (!Listing_TreeThingFilter.cachedHiddenSpecialFilters.TryGetValue(key, out list))
			{
				list = Listing_TreeThingFilter.CalculateHiddenSpecialFilters(node, parentFilter);
				Listing_TreeThingFilter.cachedHiddenSpecialFilters.Add(key, list);
			}
			return list;
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x000E9440 File Offset: 0x000E7640
		private static List<SpecialThingFilterDef> CalculateHiddenSpecialFilters(TreeNode_ThingCategory node, ThingFilter parentFilter)
		{
			List<SpecialThingFilterDef> list = new List<SpecialThingFilterDef>();
			IEnumerable<SpecialThingFilterDef> enumerable = node.catDef.ParentsSpecialThingFilterDefs.Concat(node.catDef.DescendantSpecialThingFilterDefs);
			IEnumerable<ThingDef> enumerable2 = node.catDef.DescendantThingDefs;
			if (parentFilter != null)
			{
				enumerable2 = from x in enumerable2
				where parentFilter.Allows(x)
				select x;
			}
			foreach (SpecialThingFilterDef specialThingFilterDef in enumerable)
			{
				bool flag = false;
				foreach (ThingDef def in enumerable2)
				{
					if (specialThingFilterDef.Worker.CanEverMatch(def))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(specialThingFilterDef);
				}
			}
			return list;
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x000E9534 File Offset: 0x000E7734
		public static void ResetStaticData()
		{
			Listing_TreeThingFilter.cachedHiddenSpecialFilters.Clear();
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x000E955B File Offset: 0x000E775B
		[CompilerGenerated]
		private bool <DoCategoryChildren>g__HideCategoryDueToSearch|13_0(TreeNode_ThingCategory subCat, ref Listing_TreeThingFilter.<>c__DisplayClass13_0 A_2)
		{
			return !(!this.searchFilter.Active | A_2.subtreeMatchedSearch) && !this.CategoryMatches(subCat) && !this.ThisOrDescendantsVisibleAndMatchesSearch(subCat);
		}

		// Token: 0x06002486 RID: 9350 RVA: 0x000E958D File Offset: 0x000E778D
		[CompilerGenerated]
		private bool <DoCategoryChildren>g__HideThingDueToSearch|13_1(ThingDef tDef, ref Listing_TreeThingFilter.<>c__DisplayClass13_0 A_2)
		{
			return !(!this.searchFilter.Active | A_2.subtreeMatchedSearch) && !this.searchFilter.Matches(tDef);
		}

		// Token: 0x06002487 RID: 9351 RVA: 0x000E95B7 File Offset: 0x000E77B7
		[CompilerGenerated]
		private bool <ThisOrDescendantsVisibleAndMatchesSearch>g__ThingDefVisibleAndMatches|20_0(ThingDef td)
		{
			return this.Visible(td) && this.searchFilter.Matches(td);
		}

		// Token: 0x06002488 RID: 9352 RVA: 0x000E95D0 File Offset: 0x000E77D0
		[CompilerGenerated]
		private bool <ThisOrDescendantsVisibleAndMatchesSearch>g__SpecialFilterVisibleAndMatches|20_1(SpecialThingFilterDef sf, TreeNode_ThingCategory subCat)
		{
			return this.Visible(sf, subCat) && this.searchFilter.Matches(sf);
		}

		// Token: 0x0400175E RID: 5982
		private static readonly Color NoMatchColor = Color.grey;

		// Token: 0x0400175F RID: 5983
		private static readonly LRUCache<ValueTuple<TreeNode_ThingCategory, ThingFilter>, List<SpecialThingFilterDef>> cachedHiddenSpecialFilters = new LRUCache<ValueTuple<TreeNode_ThingCategory, ThingFilter>, List<SpecialThingFilterDef>>(500);

		// Token: 0x04001760 RID: 5984
		private ThingFilter filter;

		// Token: 0x04001761 RID: 5985
		private ThingFilter parentFilter;

		// Token: 0x04001762 RID: 5986
		private List<SpecialThingFilterDef> hiddenSpecialFilters;

		// Token: 0x04001763 RID: 5987
		private List<ThingDef> forceHiddenDefs;

		// Token: 0x04001764 RID: 5988
		private List<SpecialThingFilterDef> tempForceHiddenSpecialFilters;

		// Token: 0x04001765 RID: 5989
		private List<ThingDef> suppressSmallVolumeTags;

		// Token: 0x04001766 RID: 5990
		protected QuickSearchFilter searchFilter;

		// Token: 0x04001767 RID: 5991
		public int matchCount;

		// Token: 0x04001768 RID: 5992
		private Rect visibleRect;
	}
}
