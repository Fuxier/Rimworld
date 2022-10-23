using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004F8 RID: 1272
	public class Dialog_MessageBox : Window
	{
		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x060026D8 RID: 9944 RVA: 0x000F983D File Offset: 0x000F7A3D
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(640f, 460f);
			}
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x060026D9 RID: 9945 RVA: 0x000F984E File Offset: 0x000F7A4E
		private float TimeUntilInteractive
		{
			get
			{
				return this.interactionDelay - (Time.realtimeSinceStartup - this.creationRealTime);
			}
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x060026DA RID: 9946 RVA: 0x000F9863 File Offset: 0x000F7A63
		private bool InteractionDelayExpired
		{
			get
			{
				return this.TimeUntilInteractive <= 0f;
			}
		}

		// Token: 0x060026DB RID: 9947 RVA: 0x000F9878 File Offset: 0x000F7A78
		public static Dialog_MessageBox CreateConfirmation(TaggedString text, Action confirmedAct, bool destructive = false, string title = null, WindowLayer layer = WindowLayer.Dialog)
		{
			return new Dialog_MessageBox(text, "Confirm".Translate(), confirmedAct, "GoBack".Translate(), null, title, destructive, confirmedAct, delegate()
			{
			}, layer);
		}

		// Token: 0x060026DC RID: 9948 RVA: 0x000F98D0 File Offset: 0x000F7AD0
		public Dialog_MessageBox(TaggedString text, string buttonAText = null, Action buttonAAction = null, string buttonBText = null, Action buttonBAction = null, string title = null, bool buttonADestructive = false, Action acceptAction = null, Action cancelAction = null, WindowLayer layer = WindowLayer.Dialog)
		{
			this.text = text;
			this.buttonAText = buttonAText;
			this.buttonAAction = buttonAAction;
			this.buttonADestructive = buttonADestructive;
			this.buttonBText = buttonBText;
			this.buttonBAction = buttonBAction;
			this.title = title;
			this.acceptAction = acceptAction;
			this.cancelAction = cancelAction;
			this.layer = layer;
			if (buttonAText.NullOrEmpty())
			{
				this.buttonAText = "OK".Translate();
			}
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.creationRealTime = RealTime.LastRealTime;
			this.onlyOneOfTypeAllowed = false;
			bool flag = buttonAAction == null && buttonBAction == null && this.buttonCAction == null;
			this.forceCatchAcceptAndCancelEventEvenIfUnfocused = (acceptAction != null || cancelAction != null || flag);
			this.closeOnAccept = flag;
			this.closeOnCancel = flag;
		}

		// Token: 0x060026DD RID: 9949 RVA: 0x000F99C0 File Offset: 0x000F7BC0
		public override void DoWindowContents(Rect inRect)
		{
			float num = inRect.y;
			if (!this.title.NullOrEmpty())
			{
				Text.Font = GameFont.Medium;
				Widgets.Label(new Rect(0f, num, inRect.width, 42f), this.title);
				num += 42f;
			}
			if (this.image != null)
			{
				float num2 = (float)this.image.width / (float)this.image.height;
				float num3 = 270f * num2;
				GUI.DrawTexture(new Rect(inRect.x + (inRect.width - num3) / 2f, num, num3, 270f), this.image);
				num += 280f;
			}
			Text.Font = GameFont.Small;
			Rect outRect = new Rect(inRect.x, num, inRect.width, inRect.height - 35f - 5f - num);
			float width = outRect.width - 16f;
			Rect viewRect = new Rect(0f, 0f, width, Text.CalcHeight(this.text, width));
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			Widgets.Label(new Rect(0f, 0f, viewRect.width, viewRect.height), this.text);
			Widgets.EndScrollView();
			int num4 = this.buttonCText.NullOrEmpty() ? 2 : 3;
			float num5 = inRect.width / (float)num4;
			float width2 = num5 - 10f;
			if (this.buttonADestructive)
			{
				GUI.color = new Color(1f, 0.3f, 0.35f);
			}
			string label = this.InteractionDelayExpired ? this.buttonAText : (this.buttonAText + "(" + Mathf.Ceil(this.TimeUntilInteractive).ToString("F0") + ")");
			if (Widgets.ButtonText(new Rect(num5 * (float)(num4 - 1) + 10f, inRect.height - 35f, width2, 35f), label, true, true, true, null) && this.InteractionDelayExpired)
			{
				if (this.buttonAAction != null)
				{
					this.buttonAAction();
				}
				this.Close(true);
			}
			GUI.color = Color.white;
			if (this.buttonBText != null && Widgets.ButtonText(new Rect(0f, inRect.height - 35f, width2, 35f), this.buttonBText, true, true, true, null))
			{
				if (this.buttonBAction != null)
				{
					this.buttonBAction();
				}
				this.Close(true);
			}
			if (this.buttonCText != null && Widgets.ButtonText(new Rect(num5, inRect.height - 35f, width2, 35f), this.buttonCText, true, true, true, null))
			{
				if (this.buttonCAction != null)
				{
					this.buttonCAction();
				}
				if (this.buttonCClose)
				{
					this.Close(true);
				}
			}
		}

		// Token: 0x060026DE RID: 9950 RVA: 0x000F9CC7 File Offset: 0x000F7EC7
		public override void OnCancelKeyPressed()
		{
			if (this.cancelAction != null)
			{
				this.cancelAction();
				this.Close(true);
				return;
			}
			base.OnCancelKeyPressed();
		}

		// Token: 0x060026DF RID: 9951 RVA: 0x000F9CEA File Offset: 0x000F7EEA
		public override void OnAcceptKeyPressed()
		{
			if (this.acceptAction != null)
			{
				this.acceptAction();
				this.Close(true);
				return;
			}
			base.OnAcceptKeyPressed();
		}

		// Token: 0x04001965 RID: 6501
		public TaggedString text;

		// Token: 0x04001966 RID: 6502
		public string title;

		// Token: 0x04001967 RID: 6503
		public string buttonAText;

		// Token: 0x04001968 RID: 6504
		public Action buttonAAction;

		// Token: 0x04001969 RID: 6505
		public bool buttonADestructive;

		// Token: 0x0400196A RID: 6506
		public string buttonBText;

		// Token: 0x0400196B RID: 6507
		public Action buttonBAction;

		// Token: 0x0400196C RID: 6508
		public string buttonCText;

		// Token: 0x0400196D RID: 6509
		public Action buttonCAction;

		// Token: 0x0400196E RID: 6510
		public bool buttonCClose = true;

		// Token: 0x0400196F RID: 6511
		public float interactionDelay;

		// Token: 0x04001970 RID: 6512
		public Action acceptAction;

		// Token: 0x04001971 RID: 6513
		public Action cancelAction;

		// Token: 0x04001972 RID: 6514
		public Texture2D image;

		// Token: 0x04001973 RID: 6515
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04001974 RID: 6516
		private float creationRealTime = -1f;

		// Token: 0x04001975 RID: 6517
		private const float TitleHeight = 42f;

		// Token: 0x04001976 RID: 6518
		protected const float ButtonHeight = 35f;
	}
}
