using System;
using RimWorld;
using UnityEngine;
using Verse.Noise;
using Verse.Sound;
using Verse.Steam;

namespace Verse
{
	// Token: 0x020004D1 RID: 1233
	public abstract class UIRoot
	{
		// Token: 0x0600252F RID: 9519 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Init()
		{
		}

		// Token: 0x06002530 RID: 9520 RVA: 0x000EC1B8 File Offset: 0x000EA3B8
		public virtual void UIRootOnGUI()
		{
			DebugInputLogger.InputLogOnGUI();
			UnityGUIBugsFixer.OnGUI();
			SteamDeck.OnGUI();
			SteamDeck.RootOnGUI();
			OriginalEventUtility.RecordOriginalEvent(Event.current);
			Text.StartOfOnGUI();
			this.CheckOpenLogWindow();
			DelayedErrorWindowRequest.DelayedErrorWindowRequestOnGUI();
			if (!this.screenshotMode.FiltersCurrentEvent)
			{
				this.debugWindowOpener.DevToolStarterOnGUI();
			}
			this.windows.HandleEventsHighPriority();
			this.screenshotMode.ScreenshotModesOnGUI();
			if (!this.screenshotMode.FiltersCurrentEvent)
			{
				TooltipHandler.DoTooltipGUI();
				this.feedbackFloaters.FeedbackOnGUI();
				DragSliderManager.DragSlidersOnGUI();
				Messages.MessagesDoGUI();
			}
			this.shortcutKeys.ShortcutKeysOnGUI();
			NoiseDebugUI.NoiseDebugOnGUI();
			Debug.developerConsoleVisible = false;
			if (Current.Game != null)
			{
				GameComponentUtility.GameComponentOnGUI();
				CellInspectorDrawer.OnGUI();
			}
			OriginalEventUtility.Reset();
		}

		// Token: 0x06002531 RID: 9521 RVA: 0x000EC274 File Offset: 0x000EA474
		public virtual void UIRootUpdate()
		{
			ScreenshotTaker.Update();
			DragSliderManager.DragSlidersUpdate();
			this.windows.WindowsUpdate();
			MouseoverSounds.ResolveFrame();
			UIHighlighter.UIHighlighterUpdate();
			Messages.Update();
			CellInspectorDrawer.Update();
		}

		// Token: 0x06002532 RID: 9522 RVA: 0x000EC29F File Offset: 0x000EA49F
		private void CheckOpenLogWindow()
		{
			if (EditWindow_Log.wantsToOpen && !Find.WindowStack.IsOpen(typeof(EditWindow_Log)))
			{
				Find.WindowStack.Add(new EditWindow_Log());
				EditWindow_Log.wantsToOpen = false;
			}
		}

		// Token: 0x040017D1 RID: 6097
		public WindowStack windows = new WindowStack();

		// Token: 0x040017D2 RID: 6098
		protected DebugWindowsOpener debugWindowOpener = new DebugWindowsOpener();

		// Token: 0x040017D3 RID: 6099
		public ScreenshotModeHandler screenshotMode = new ScreenshotModeHandler();

		// Token: 0x040017D4 RID: 6100
		private ShortcutKeys shortcutKeys = new ShortcutKeys();

		// Token: 0x040017D5 RID: 6101
		public FeedbackFloaters feedbackFloaters = new FeedbackFloaters();
	}
}
