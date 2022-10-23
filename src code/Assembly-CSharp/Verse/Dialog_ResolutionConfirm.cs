using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004F4 RID: 1268
	public class Dialog_ResolutionConfirm : Window
	{
		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x060026A8 RID: 9896 RVA: 0x000F88AE File Offset: 0x000F6AAE
		private float TimeUntilRevert
		{
			get
			{
				return this.startTime + 10f - Time.realtimeSinceStartup;
			}
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x060026A9 RID: 9897 RVA: 0x000F88C2 File Offset: 0x000F6AC2
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 300f);
			}
		}

		// Token: 0x060026AA RID: 9898 RVA: 0x000F88D3 File Offset: 0x000F6AD3
		private Dialog_ResolutionConfirm()
		{
			this.startTime = Time.realtimeSinceStartup;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x060026AB RID: 9899 RVA: 0x000F88FB File Offset: 0x000F6AFB
		public Dialog_ResolutionConfirm(bool oldFullscreen) : this()
		{
			this.oldFullscreen = oldFullscreen;
			this.oldRes = new IntVec2(Screen.width, Screen.height);
			this.oldUIScale = Prefs.UIScale;
		}

		// Token: 0x060026AC RID: 9900 RVA: 0x000F892A File Offset: 0x000F6B2A
		public Dialog_ResolutionConfirm(IntVec2 oldRes) : this()
		{
			this.oldFullscreen = Screen.fullScreen;
			this.oldRes = oldRes;
			this.oldUIScale = Prefs.UIScale;
		}

		// Token: 0x060026AD RID: 9901 RVA: 0x000F894F File Offset: 0x000F6B4F
		public Dialog_ResolutionConfirm(float oldUIScale) : this()
		{
			this.oldFullscreen = Screen.fullScreen;
			this.oldRes = new IntVec2(Screen.width, Screen.height);
			this.oldUIScale = oldUIScale;
		}

		// Token: 0x060026AE RID: 9902 RVA: 0x000F8980 File Offset: 0x000F6B80
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			string label = "ConfirmResolutionChange".Translate(Mathf.CeilToInt(this.TimeUntilRevert));
			Widgets.Label(new Rect(0f, 0f, inRect.width, inRect.height), label);
			if (Widgets.ButtonText(new Rect(0f, inRect.height - 35f, inRect.width / 2f - 20f, 35f), "ResolutionKeep".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(inRect.width / 2f + 20f, inRect.height - 35f, inRect.width / 2f - 20f, 35f), "ResolutionRevert".Translate(), true, true, true, null))
			{
				this.Revert();
				this.Close(true);
			}
		}

		// Token: 0x060026AF RID: 9903 RVA: 0x000F8A9C File Offset: 0x000F6C9C
		private void Revert()
		{
			if (Prefs.LogVerbose)
			{
				Log.Message(string.Concat(new object[]
				{
					"Reverting screen settings to ",
					this.oldRes.x,
					"x",
					this.oldRes.z,
					", fs=",
					this.oldFullscreen.ToString()
				}));
			}
			ResolutionUtility.SetResolutionRaw(this.oldRes.x, this.oldRes.z, this.oldFullscreen);
			Prefs.FullScreen = this.oldFullscreen;
			Prefs.ScreenWidth = this.oldRes.x;
			Prefs.ScreenHeight = this.oldRes.z;
			Prefs.UIScale = this.oldUIScale;
			GenUI.ClearLabelWidthCache();
		}

		// Token: 0x060026B0 RID: 9904 RVA: 0x000F8B68 File Offset: 0x000F6D68
		public override void WindowUpdate()
		{
			if (this.TimeUntilRevert <= 0f)
			{
				this.Revert();
				this.Close(true);
			}
		}

		// Token: 0x04001946 RID: 6470
		private float startTime;

		// Token: 0x04001947 RID: 6471
		private IntVec2 oldRes;

		// Token: 0x04001948 RID: 6472
		private bool oldFullscreen;

		// Token: 0x04001949 RID: 6473
		private float oldUIScale;

		// Token: 0x0400194A RID: 6474
		private const float RevertTime = 10f;
	}
}
