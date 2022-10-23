using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004F0 RID: 1264
	public abstract class Dialog_Rename : Window
	{
		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06002697 RID: 9879 RVA: 0x000F84B0 File Offset: 0x000F66B0
		private bool AcceptsInput
		{
			get
			{
				return this.startAcceptingInputAtFrame <= Time.frameCount;
			}
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06002698 RID: 9880 RVA: 0x000F84C2 File Offset: 0x000F66C2
		protected virtual int MaxNameLength
		{
			get
			{
				return 28;
			}
		}

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06002699 RID: 9881 RVA: 0x000F84C6 File Offset: 0x000F66C6
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(280f, 175f);
			}
		}

		// Token: 0x0600269A RID: 9882 RVA: 0x000F84D7 File Offset: 0x000F66D7
		public Dialog_Rename()
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.closeOnClickedOutside = true;
		}

		// Token: 0x0600269B RID: 9883 RVA: 0x000F8502 File Offset: 0x000F6702
		public void WasOpenedByHotkey()
		{
			this.startAcceptingInputAtFrame = Time.frameCount + 1;
		}

		// Token: 0x0600269C RID: 9884 RVA: 0x000F8511 File Offset: 0x000F6711
		protected virtual AcceptanceReport NameIsValid(string name)
		{
			if (name.Length == 0)
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600269D RID: 9885 RVA: 0x000F8528 File Offset: 0x000F6728
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			bool flag = false;
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
			{
				flag = true;
				Event.current.Use();
			}
			GUI.SetNextControlName("RenameField");
			string text = Widgets.TextField(new Rect(0f, 15f, inRect.width, 35f), this.curName);
			if (this.AcceptsInput && text.Length < this.MaxNameLength)
			{
				this.curName = text;
			}
			else if (!this.AcceptsInput)
			{
				((TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl)).SelectAll();
			}
			if (!this.focusedRenameField)
			{
				UI.FocusControl("RenameField", this);
				this.focusedRenameField = true;
			}
			if (Widgets.ButtonText(new Rect(15f, inRect.height - 35f - 15f, inRect.width - 15f - 15f, 35f), "OK", true, true, true, null) || flag)
			{
				AcceptanceReport acceptanceReport = this.NameIsValid(this.curName);
				if (!acceptanceReport.Accepted)
				{
					if (acceptanceReport.Reason.NullOrEmpty())
					{
						Messages.Message("NameIsInvalid".Translate(), MessageTypeDefOf.RejectInput, false);
						return;
					}
					Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
					return;
				}
				else
				{
					this.SetName(this.curName);
					Find.WindowStack.TryRemove(this, true);
				}
			}
		}

		// Token: 0x0600269E RID: 9886
		protected abstract void SetName(string name);

		// Token: 0x0400193F RID: 6463
		protected string curName;

		// Token: 0x04001940 RID: 6464
		private bool focusedRenameField;

		// Token: 0x04001941 RID: 6465
		private int startAcceptingInputAtFrame;
	}
}
