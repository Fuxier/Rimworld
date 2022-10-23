using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020004FE RID: 1278
	public class DiaOption
	{
		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x060026F0 RID: 9968 RVA: 0x000FA221 File Offset: 0x000F8421
		public static DiaOption DefaultOK
		{
			get
			{
				return new DiaOption("OK".Translate())
				{
					resolveTree = true
				};
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x060026F1 RID: 9969 RVA: 0x000FA23E File Offset: 0x000F843E
		protected Dialog_NodeTree OwningDialog
		{
			get
			{
				return (Dialog_NodeTree)this.dialog;
			}
		}

		// Token: 0x060026F2 RID: 9970 RVA: 0x000FA24C File Offset: 0x000F844C
		public DiaOption()
		{
			this.text = "OK".Translate();
		}

		// Token: 0x060026F3 RID: 9971 RVA: 0x000FA299 File Offset: 0x000F8499
		public DiaOption(string text)
		{
			this.text = text;
		}

		// Token: 0x060026F4 RID: 9972 RVA: 0x000FA2D0 File Offset: 0x000F84D0
		public DiaOption(Dialog_InfoCard.Hyperlink hyperlink)
		{
			this.hyperlink = hyperlink;
			this.text = "ViewHyperlink".Translate(hyperlink.Label);
		}

		// Token: 0x060026F5 RID: 9973 RVA: 0x000FA330 File Offset: 0x000F8530
		public DiaOption(DiaOptionMold def)
		{
			this.text = def.Text;
			DiaNodeMold diaNodeMold = def.RandomLinkNode();
			if (diaNodeMold != null)
			{
				this.link = new DiaNode(diaNodeMold);
			}
		}

		// Token: 0x060026F6 RID: 9974 RVA: 0x000FA38A File Offset: 0x000F858A
		public void Disable(string newDisabledReason)
		{
			this.disabled = true;
			this.disabledReason = newDisabledReason;
		}

		// Token: 0x060026F7 RID: 9975 RVA: 0x000FA39A File Offset: 0x000F859A
		public void SetText(string newText)
		{
			this.text = newText;
		}

		// Token: 0x060026F8 RID: 9976 RVA: 0x000FA3A4 File Offset: 0x000F85A4
		public float OptOnGUI(Rect rect, bool active = true)
		{
			Color textColor = Widgets.NormalOptionColor;
			string text = this.text;
			if (this.disabled)
			{
				textColor = this.DisabledOptionColor;
				if (this.disabledReason != null)
				{
					text = text + " (" + this.disabledReason + ")";
				}
			}
			rect.height = Text.CalcHeight(text, rect.width);
			if (this.hyperlink.def != null)
			{
				Widgets.HyperlinkWithIcon(rect, this.hyperlink, text, 2f, 6f, null, false, null);
			}
			else if (Widgets.ButtonText(rect, text, false, !this.disabled, textColor, active && !this.disabled, null))
			{
				this.Activate();
			}
			return rect.height;
		}

		// Token: 0x060026F9 RID: 9977 RVA: 0x000FA46C File Offset: 0x000F866C
		protected void Activate()
		{
			if (this.clickSound != null && !this.resolveTree)
			{
				this.clickSound.PlayOneShotOnCamera(null);
			}
			if (this.resolveTree)
			{
				this.OwningDialog.Close(true);
			}
			if (this.action != null)
			{
				this.action();
			}
			if (this.linkLateBind != null)
			{
				this.OwningDialog.GotoNode(this.linkLateBind());
				return;
			}
			if (this.link != null)
			{
				this.OwningDialog.GotoNode(this.link);
			}
		}

		// Token: 0x04001990 RID: 6544
		public Window dialog;

		// Token: 0x04001991 RID: 6545
		protected string text;

		// Token: 0x04001992 RID: 6546
		public DiaNode link;

		// Token: 0x04001993 RID: 6547
		public Func<DiaNode> linkLateBind;

		// Token: 0x04001994 RID: 6548
		public bool resolveTree;

		// Token: 0x04001995 RID: 6549
		public Action action;

		// Token: 0x04001996 RID: 6550
		public bool disabled;

		// Token: 0x04001997 RID: 6551
		public string disabledReason;

		// Token: 0x04001998 RID: 6552
		public SoundDef clickSound = SoundDefOf.PageChange;

		// Token: 0x04001999 RID: 6553
		public Dialog_InfoCard.Hyperlink hyperlink;

		// Token: 0x0400199A RID: 6554
		protected readonly Color DisabledOptionColor = new Color(0.5f, 0.5f, 0.5f);
	}
}
