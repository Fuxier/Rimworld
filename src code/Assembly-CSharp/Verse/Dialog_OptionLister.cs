using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004F9 RID: 1273
	public abstract class Dialog_OptionLister : Window
	{
		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x060026E0 RID: 9952 RVA: 0x00004E17 File Offset: 0x00003017
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
			}
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x060026E1 RID: 9953 RVA: 0x00002662 File Offset: 0x00000862
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060026E2 RID: 9954 RVA: 0x000F9D0D File Offset: 0x000F7F0D
		public Dialog_OptionLister()
		{
			this.doCloseX = true;
			this.onlyOneOfTypeAllowed = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x060026E3 RID: 9955 RVA: 0x000F9D3C File Offset: 0x000F7F3C
		public override void PostOpen()
		{
			base.PostOpen();
			if (this.focusOnFilterOnOpen)
			{
				this.focusFilter = true;
			}
		}

		// Token: 0x060026E4 RID: 9956 RVA: 0x000F9D54 File Offset: 0x000F7F54
		public override void DoWindowContents(Rect inRect)
		{
			GUI.SetNextControlName("DebugFilter");
			if (Event.current.type == EventType.KeyDown && (KeyBindingDefOf.Dev_ToggleDebugSettingsMenu.KeyDownEvent || KeyBindingDefOf.Dev_ToggleDebugActionsMenu.KeyDownEvent))
			{
				return;
			}
			this.filter = Widgets.TextField(new Rect(0f, 0f, 200f, 30f), this.filter);
			if ((Event.current.type == EventType.KeyDown || Event.current.type == EventType.Repaint) && this.focusFilter)
			{
				GUI.FocusControl("DebugFilter");
				this.filter = "";
				this.focusFilter = false;
			}
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight = 0f;
			}
			Rect outRect = new Rect(inRect);
			outRect.yMin += 35f;
			int num = (int)(this.InitialSize.x / 200f);
			float num2 = (this.totalOptionsHeight + 24f * (float)(num - 1)) / (float)num;
			if (num2 < outRect.height)
			{
				num2 = outRect.height;
			}
			Rect rect = new Rect(0f, 0f, outRect.width - 16f, num2);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect, true);
			this.listing = new Listing_Standard(inRect, () => this.scrollPosition);
			this.listing.ColumnWidth = (rect.width - 17f * (float)(num - 1)) / (float)num;
			this.listing.Begin(rect);
			this.DoListingItems();
			this.listing.End();
			Widgets.EndScrollView();
		}

		// Token: 0x060026E5 RID: 9957 RVA: 0x000F9EED File Offset: 0x000F80ED
		public override void PostClose()
		{
			base.PostClose();
			UI.UnfocusCurrentControl();
		}

		// Token: 0x060026E6 RID: 9958
		protected abstract void DoListingItems();

		// Token: 0x060026E7 RID: 9959 RVA: 0x000F9EFA File Offset: 0x000F80FA
		protected bool FilterAllows(string label)
		{
			return this.filter.NullOrEmpty() || label.NullOrEmpty() || label.IndexOf(this.filter, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x04001977 RID: 6519
		protected Vector2 scrollPosition;

		// Token: 0x04001978 RID: 6520
		protected string filter = "";

		// Token: 0x04001979 RID: 6521
		protected float totalOptionsHeight;

		// Token: 0x0400197A RID: 6522
		protected Listing_Standard listing;

		// Token: 0x0400197B RID: 6523
		protected bool focusOnFilterOnOpen = true;

		// Token: 0x0400197C RID: 6524
		private bool focusFilter;

		// Token: 0x0400197D RID: 6525
		protected const string FilterControlName = "DebugFilter";
	}
}
