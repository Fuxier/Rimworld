using System;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000463 RID: 1123
	public abstract class Dialog_DebugOptionLister : Dialog_OptionLister
	{
		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x0600227B RID: 8827 RVA: 0x000DC2DE File Offset: 0x000DA4DE
		protected virtual int HighlightedIndex
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x0600227C RID: 8828 RVA: 0x000DC2E1 File Offset: 0x000DA4E1
		public Dialog_DebugOptionLister()
		{
			this.forcePause = true;
		}

		// Token: 0x0600227D RID: 8829 RVA: 0x000DC2F0 File Offset: 0x000DA4F0
		protected bool DebugAction(string label, Action action, bool highlight)
		{
			bool result = false;
			if (!base.FilterAllows(label))
			{
				return false;
			}
			if (this.listing.ButtonDebug(label, highlight))
			{
				this.Close(true);
				action();
				result = true;
			}
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight += 24f;
			}
			return result;
		}

		// Token: 0x0600227E RID: 8830 RVA: 0x000DC348 File Offset: 0x000DA548
		protected void DebugToolMap(string label, Action toolAction, bool highlight)
		{
			if (WorldRendererUtility.WorldRenderedNow)
			{
				return;
			}
			if (!base.FilterAllows(label))
			{
				GUI.color = new Color(1f, 1f, 1f, 0.3f);
			}
			if (this.listing.ButtonDebug(label, highlight))
			{
				this.Close(true);
				DebugTools.curTool = new DebugTool(label, toolAction, null);
			}
			GUI.color = Color.white;
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight += 24f;
			}
		}

		// Token: 0x0600227F RID: 8831 RVA: 0x000DC3D0 File Offset: 0x000DA5D0
		protected void DebugToolMapForPawns(string label, Action<Pawn> pawnAction, bool highlight)
		{
			this.DebugToolMap(label, delegate
			{
				if (UI.MouseCell().InBounds(Find.CurrentMap))
				{
					foreach (Pawn obj in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
					where t is Pawn
					select t).Cast<Pawn>().ToList<Pawn>())
					{
						pawnAction(obj);
					}
				}
			}, highlight);
		}

		// Token: 0x06002280 RID: 8832 RVA: 0x000DC400 File Offset: 0x000DA600
		protected void DebugToolWorld(string label, Action toolAction, bool highlight)
		{
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				return;
			}
			if (!base.FilterAllows(label))
			{
				GUI.color = new Color(1f, 1f, 1f, 0.3f);
			}
			if (this.listing.ButtonDebug(label, highlight))
			{
				this.Close(true);
				DebugTools.curTool = new DebugTool(label, toolAction, null);
			}
			GUI.color = Color.white;
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight += 24f;
			}
		}

		// Token: 0x06002281 RID: 8833 RVA: 0x000DC488 File Offset: 0x000DA688
		protected override void DoListingItems()
		{
			if (KeyBindingDefOf.Dev_ChangeSelectedDebugAction.IsDownEvent)
			{
				this.ChangeHighlightedOption();
			}
		}

		// Token: 0x06002282 RID: 8834 RVA: 0x000034B7 File Offset: 0x000016B7
		protected virtual void ChangeHighlightedOption()
		{
		}

		// Token: 0x06002283 RID: 8835 RVA: 0x000DC49C File Offset: 0x000DA69C
		protected void CheckboxLabeledDebug(string label, ref bool checkOn, bool highlighted)
		{
			if (!base.FilterAllows(label))
			{
				GUI.color = new Color(1f, 1f, 1f, 0.3f);
			}
			this.listing.LabelCheckboxDebug(label, ref checkOn, highlighted);
			GUI.color = Color.white;
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight += 24f;
			}
		}

		// Token: 0x06002284 RID: 8836 RVA: 0x000DC507 File Offset: 0x000DA707
		protected void DoLabel(string label)
		{
			Text.Font = GameFont.Small;
			this.listing.Label(label, -1f, null);
			this.totalOptionsHeight += Text.CalcHeight(label, 300f) + 2f;
		}

		// Token: 0x06002285 RID: 8837 RVA: 0x000DC540 File Offset: 0x000DA740
		protected void DoGap()
		{
			this.listing.Gap(7f);
			this.totalOptionsHeight += 7f;
		}

		// Token: 0x040015E9 RID: 5609
		protected int currentHighlightIndex;

		// Token: 0x040015EA RID: 5610
		protected int prioritizedHighlightedIndex;

		// Token: 0x040015EB RID: 5611
		private const float DebugOptionsGap = 7f;
	}
}
