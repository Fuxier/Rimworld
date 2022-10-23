using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000462 RID: 1122
	public class Dialog_DebugOptionListLister : Dialog_DebugOptionLister
	{
		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06002275 RID: 8821 RVA: 0x000DC028 File Offset: 0x000DA228
		protected override int HighlightedIndex
		{
			get
			{
				if (this.options.NullOrEmpty<DebugMenuOption>())
				{
					return base.HighlightedIndex;
				}
				if (base.FilterAllows(this.options[this.prioritizedHighlightedIndex].label))
				{
					return this.prioritizedHighlightedIndex;
				}
				if (this.filter.NullOrEmpty())
				{
					return 0;
				}
				for (int i = 0; i < this.options.Count; i++)
				{
					if (base.FilterAllows(this.options[i].label))
					{
						this.currentHighlightIndex = i;
						break;
					}
				}
				return this.currentHighlightIndex;
			}
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x000DC0BB File Offset: 0x000DA2BB
		public Dialog_DebugOptionListLister(IEnumerable<DebugMenuOption> options)
		{
			this.options = options.ToList<DebugMenuOption>();
		}

		// Token: 0x06002277 RID: 8823 RVA: 0x000DC0D0 File Offset: 0x000DA2D0
		protected override void DoListingItems()
		{
			base.DoListingItems();
			int highlightedIndex = this.HighlightedIndex;
			for (int i = 0; i < this.options.Count; i++)
			{
				DebugMenuOption debugMenuOption = this.options[i];
				bool highlight = highlightedIndex == i;
				DebugMenuOptionMode mode = debugMenuOption.mode;
				if (mode != DebugMenuOptionMode.Action)
				{
					if (mode == DebugMenuOptionMode.Tool)
					{
						base.DebugToolMap(debugMenuOption.label, debugMenuOption.method, highlight);
					}
				}
				else
				{
					base.DebugAction(debugMenuOption.label, debugMenuOption.method, highlight);
				}
			}
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x000DC150 File Offset: 0x000DA350
		protected override void ChangeHighlightedOption()
		{
			int highlightedIndex = this.HighlightedIndex;
			for (int i = 0; i < this.options.Count; i++)
			{
				int num = (highlightedIndex + i + 1) % this.options.Count;
				if (base.FilterAllows(this.options[num].label))
				{
					this.prioritizedHighlightedIndex = num;
					return;
				}
			}
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x000DC1B0 File Offset: 0x000DA3B0
		public static void ShowSimpleDebugMenu<T>(IEnumerable<T> elements, Func<T, string> label, Action<T> chosen)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<T> enumerator = elements.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					T t = enumerator.Current;
					list.Add(new DebugMenuOption(label(t), DebugMenuOptionMode.Action, delegate()
					{
						chosen(t);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x0600227A RID: 8826 RVA: 0x000DC24C File Offset: 0x000DA44C
		public override void OnAcceptKeyPressed()
		{
			if (GUI.GetNameOfFocusedControl() == "DebugFilter")
			{
				int highlightedIndex = this.HighlightedIndex;
				if (highlightedIndex >= 0)
				{
					this.Close(true);
					if (this.options[highlightedIndex].mode == DebugMenuOptionMode.Action)
					{
						this.options[highlightedIndex].method();
					}
					else
					{
						DebugTools.curTool = new DebugTool(this.options[highlightedIndex].label, this.options[highlightedIndex].method, null);
					}
				}
				Event.current.Use();
			}
		}

		// Token: 0x040015E8 RID: 5608
		protected List<DebugMenuOption> options;
	}
}
