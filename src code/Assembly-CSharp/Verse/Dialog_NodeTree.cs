using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000502 RID: 1282
	public class Dialog_NodeTree : Window
	{
		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06002703 RID: 9987 RVA: 0x000FAC20 File Offset: 0x000F8E20
		public override Vector2 InitialSize
		{
			get
			{
				int num = 480;
				if (this.curNode.options.Count > 5)
				{
					Text.Font = GameFont.Small;
					num += (this.curNode.options.Count - 5) * (int)(Text.LineHeight + 7f);
				}
				return new Vector2(620f, (float)Mathf.Min(num, UI.screenHeight));
			}
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06002704 RID: 9988 RVA: 0x000FAC84 File Offset: 0x000F8E84
		private bool InteractiveNow
		{
			get
			{
				return Time.realtimeSinceStartup >= this.makeInteractiveAtTime;
			}
		}

		// Token: 0x06002705 RID: 9989 RVA: 0x000FAC98 File Offset: 0x000F8E98
		public Dialog_NodeTree(DiaNode nodeRoot, bool delayInteractivity = false, bool radioMode = false, string title = null)
		{
			this.title = title;
			this.GotoNode(nodeRoot);
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			if (delayInteractivity)
			{
				this.makeInteractiveAtTime = RealTime.LastRealTime + 1f;
			}
			this.soundAppear = SoundDefOf.CommsWindow_Open;
			this.soundClose = SoundDefOf.CommsWindow_Close;
			if (radioMode)
			{
				this.soundAmbient = SoundDefOf.RadioComms_Ambience;
			}
		}

		// Token: 0x06002706 RID: 9990 RVA: 0x000FAD19 File Offset: 0x000F8F19
		public override void PreClose()
		{
			base.PreClose();
			this.curNode.PreClose();
		}

		// Token: 0x06002707 RID: 9991 RVA: 0x000FAD2C File Offset: 0x000F8F2C
		public override void PostClose()
		{
			base.PostClose();
			if (this.closeAction != null)
			{
				this.closeAction();
			}
		}

		// Token: 0x06002708 RID: 9992 RVA: 0x000FAD48 File Offset: 0x000F8F48
		public override void WindowOnGUI()
		{
			if (this.screenFillColor != Color.clear)
			{
				GUI.color = this.screenFillColor;
				GUI.DrawTexture(new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight), BaseContent.WhiteTex);
				GUI.color = Color.white;
			}
			base.WindowOnGUI();
		}

		// Token: 0x06002709 RID: 9993 RVA: 0x000FADA8 File Offset: 0x000F8FA8
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = inRect.AtZero();
			if (this.title != null)
			{
				Text.Font = GameFont.Small;
				Rect rect2 = rect;
				rect2.height = 36f;
				rect.yMin += 53f;
				Widgets.DrawTitleBG(rect2);
				rect2.xMin += 9f;
				rect2.yMin += 5f;
				Widgets.Label(rect2, this.title);
			}
			this.DrawNode(rect);
		}

		// Token: 0x0600270A RID: 9994 RVA: 0x000FAE2C File Offset: 0x000F902C
		protected void DrawNode(Rect rect)
		{
			Widgets.BeginGroup(rect);
			Text.Font = GameFont.Small;
			float num = Mathf.Min(this.optTotalHeight, rect.height - 100f - this.Margin * 2f);
			Rect outRect = new Rect(0f, 0f, rect.width, rect.height - Mathf.Max(num, this.minOptionsAreaHeight));
			float width = rect.width - 16f;
			Rect rect2 = new Rect(0f, 0f, width, Text.CalcHeight(this.curNode.text, width));
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect2, true);
			Widgets.Label(rect2, this.curNode.text.Resolve());
			Widgets.EndScrollView();
			Widgets.BeginScrollView(new Rect(0f, rect.height - num, rect.width, num), ref this.optsScrollPosition, new Rect(0f, 0f, rect.width - 16f, this.optTotalHeight), true);
			float num2 = 0f;
			float num3 = 0f;
			for (int i = 0; i < this.curNode.options.Count; i++)
			{
				Rect rect3 = new Rect(15f, num2, rect.width - 30f, 999f);
				float num4 = this.curNode.options[i].OptOnGUI(rect3, this.InteractiveNow);
				num2 += num4 + 7f;
				num3 += num4 + 7f;
			}
			if (Event.current.type == EventType.Layout)
			{
				this.optTotalHeight = num3;
			}
			Widgets.EndScrollView();
			Widgets.EndGroup();
		}

		// Token: 0x0600270B RID: 9995 RVA: 0x000FAFE0 File Offset: 0x000F91E0
		public void GotoNode(DiaNode node)
		{
			foreach (DiaOption diaOption in node.options)
			{
				diaOption.dialog = this;
			}
			this.curNode = node;
		}

		// Token: 0x040019A0 RID: 6560
		private Vector2 scrollPosition;

		// Token: 0x040019A1 RID: 6561
		private Vector2 optsScrollPosition;

		// Token: 0x040019A2 RID: 6562
		protected string title;

		// Token: 0x040019A3 RID: 6563
		protected DiaNode curNode;

		// Token: 0x040019A4 RID: 6564
		public Action closeAction;

		// Token: 0x040019A5 RID: 6565
		private float makeInteractiveAtTime;

		// Token: 0x040019A6 RID: 6566
		public Color screenFillColor = Color.clear;

		// Token: 0x040019A7 RID: 6567
		protected float minOptionsAreaHeight;

		// Token: 0x040019A8 RID: 6568
		private const float InteractivityDelay = 1f;

		// Token: 0x040019A9 RID: 6569
		private const float TitleHeight = 36f;

		// Token: 0x040019AA RID: 6570
		protected const float OptHorMargin = 15f;

		// Token: 0x040019AB RID: 6571
		protected const float OptVerticalSpace = 7f;

		// Token: 0x040019AC RID: 6572
		private const int ResizeIfMoreOptionsThan = 5;

		// Token: 0x040019AD RID: 6573
		private const float MinSpaceLeftForTextAfterOptionsResizing = 100f;

		// Token: 0x040019AE RID: 6574
		private float optTotalHeight;
	}
}
