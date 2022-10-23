using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000453 RID: 1107
	public class DebugWindowsOpener
	{
		// Token: 0x0600221D RID: 8733 RVA: 0x000D97B0 File Offset: 0x000D79B0
		public DebugWindowsOpener()
		{
			this.drawButtonsCached = new Action(this.DrawButtons);
		}

		// Token: 0x0600221E RID: 8734 RVA: 0x000D9800 File Offset: 0x000D7A00
		public void DevToolStarterOnGUI()
		{
			if (!Prefs.DevMode)
			{
				return;
			}
			Vector2 vector = new Vector2((float)UI.screenWidth * 0.5f, 3f);
			int num = 6;
			if (Current.ProgramState == ProgramState.Playing)
			{
				num += 2;
			}
			float num2 = 25f;
			if (Current.ProgramState == ProgramState.Playing && DebugSettings.godMode)
			{
				num2 += 15f;
			}
			Find.WindowStack.ImmediateWindow(1593759361, new Rect(vector.x, vector.y, (float)num * 28f + 240f, num2).Rounded(), WindowLayer.GameUI, this.drawButtonsCached, false, false, 0f, delegate
			{
				this.quickSearchWidget.Unfocus();
			});
			if (KeyBindingDefOf.Dev_ToggleDebugLog.KeyDownEvent)
			{
				this.ToggleLogWindow();
				Event.current.Use();
			}
			if (KeyBindingDefOf.Dev_ToggleDebugActionsMenu.KeyDownEvent)
			{
				this.ToggleDebugActionsMenu();
				Event.current.Use();
			}
			if (KeyBindingDefOf.Dev_ToggleDebugLogMenu.KeyDownEvent)
			{
				this.ToggleDebugLogMenu();
				Event.current.Use();
			}
			if (KeyBindingDefOf.Dev_ToggleDebugSettingsMenu.KeyDownEvent)
			{
				this.ToggleDebugSettingsMenu();
				Event.current.Use();
			}
			if (KeyBindingDefOf.Dev_ToggleDevPalette.KeyDownEvent && Current.ProgramState == ProgramState.Playing)
			{
				DebugSettings.devPalette = !DebugSettings.devPalette;
				this.TryOpenOrClosePalette();
				Event.current.Use();
			}
			if (KeyBindingDefOf.Dev_ToggleDebugInspector.KeyDownEvent)
			{
				this.ToggleDebugInspector();
				Event.current.Use();
			}
			if (Current.ProgramState == ProgramState.Playing && KeyBindingDefOf.Dev_ToggleGodMode.KeyDownEvent)
			{
				this.ToggleGodMode();
				Event.current.Use();
			}
		}

		// Token: 0x0600221F RID: 8735 RVA: 0x000D9988 File Offset: 0x000D7B88
		private void DrawButtons()
		{
			this.widgetRow.Init(0f, 0f, UIDirection.RightThenUp, 99999f, 4f);
			if (this.widgetRow.ButtonIcon(TexButton.ToggleLog, "Open the debug log.", null, null, null, true, -1f))
			{
				this.ToggleLogWindow();
			}
			if (this.widgetRow.ButtonIcon(TexButton.ToggleTweak, "Open tweakvalues menu.\n\nThis lets you change internal values.", null, null, null, true, -1f))
			{
				this.ToggleTweakValuesMenu();
			}
			if (this.widgetRow.ButtonIcon(TexButton.OpenInspectSettings, "Open the view settings.\n\nThis lets you see special debug visuals.", null, null, null, true, -1f))
			{
				this.ToggleDebugSettingsMenu();
			}
			if (this.widgetRow.ButtonIcon(TexButton.OpenDebugActionsMenu, "Open debug actions menu.\n\nThis lets you spawn items and force various events.", null, null, null, true, -1f))
			{
				this.ToggleDebugActionsMenu();
			}
			if (this.widgetRow.ButtonIcon(TexButton.OpenDebugActionsMenu, "Open debug logging menu.", null, null, null, true, -1f))
			{
				this.ToggleDebugLogMenu();
			}
			if (this.widgetRow.ButtonIcon(TexButton.OpenInspector, "Open the inspector.\n\nThis lets you inspect what's happening in the game, down to individual variables.", null, null, null, true, -1f))
			{
				this.ToggleDebugInspector();
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (this.widgetRow.ButtonIcon(DebugSettings.godMode ? TexButton.GodModeEnabled : TexButton.GodModeDisabled, "Toggle god mode.\n\nWhen god mode is on, you can build stuff instantly, for free, and sell things that aren't yours.", null, null, null, true, -1f))
				{
					this.ToggleGodMode();
				}
				bool devPalette = DebugSettings.devPalette;
				this.widgetRow.ToggleableIcon(ref devPalette, TexButton.ToggleDevPalette, "Toggle the dev palette.\n\nAllows you to setup a palette of debug actions for ease of use.", null, null);
				if (devPalette != DebugSettings.devPalette)
				{
					DebugSettings.devPalette = devPalette;
					this.TryOpenOrClosePalette();
				}
				bool pauseOnError = Prefs.PauseOnError;
				this.widgetRow.ToggleableIcon(ref pauseOnError, TexButton.TogglePauseOnError, "Pause the game when an error is logged.", null, null);
				Prefs.PauseOnError = pauseOnError;
				if (Current.Game.CurrentMap != null && Time.frameCount - this.searchJumpLastFrame > 10 && this.quickSearchWidget.CurrentlyFocused() && (Event.current.type == EventType.KeyDown || Event.current.type == EventType.Layout) && Event.current.keyCode == KeyCode.Return && this.quickSearchWidget.filter.Active)
				{
					this.foundThingsCached = (from t in Find.CurrentMap.listerThings.AllThings
					where t.def.selectable && t.Label.ToLower().Contains(this.quickSearchWidget.filter.Text.ToLower())
					select t).ToList<Thing>();
					if (!this.foundThingsCached.NullOrEmpty<Thing>())
					{
						Find.Selector.ClearSelection();
						foreach (Thing obj in this.foundThingsCached)
						{
							Find.Selector.Select(obj, true, true);
						}
						Thing t2 = (from t in this.foundThingsCached
						where t != this.lastJumpedObject
						select t).RandomElementWithFallback(this.foundThingsCached.First<Thing>());
						CameraJumper.TryJump(t2, CameraJumper.MovementMode.Pan);
						this.lastJumpedObject = t2;
						this.searchJumpLastFrame = Time.frameCount;
					}
				}
				Rect rect = new Rect(this.widgetRow.FinalX, 0f, 240f, 24f);
				this.quickSearchWidget.OnGUI(rect, new Action(this.OnSearchChanged));
				if (Event.current.type == EventType.Layout && Event.current.keyCode == KeyCode.Escape)
				{
					this.quickSearchWidget.Unfocus();
				}
			}
		}

		// Token: 0x06002220 RID: 8736 RVA: 0x000D9D8C File Offset: 0x000D7F8C
		private void ToggleLogWindow()
		{
			if (!Find.WindowStack.TryRemove(typeof(EditWindow_Log), true))
			{
				Find.WindowStack.Add(new EditWindow_Log());
			}
		}

		// Token: 0x06002221 RID: 8737 RVA: 0x000D9DB4 File Offset: 0x000D7FB4
		private void ToggleDebugSettingsMenu()
		{
			Dialog_Debug dialog_Debug = Find.WindowStack.WindowOfType<Dialog_Debug>();
			if (dialog_Debug == null)
			{
				Find.WindowStack.Add(new Dialog_Debug(DebugTabMenuDefOf.Settings));
				return;
			}
			dialog_Debug.SwitchTab(DebugTabMenuDefOf.Settings);
		}

		// Token: 0x06002222 RID: 8738 RVA: 0x000D9DF0 File Offset: 0x000D7FF0
		private void ToggleDebugActionsMenu()
		{
			Dialog_Debug dialog_Debug = Find.WindowStack.WindowOfType<Dialog_Debug>();
			if (dialog_Debug == null)
			{
				Find.WindowStack.Add(new Dialog_Debug(DebugTabMenuDefOf.Actions));
				return;
			}
			dialog_Debug.SwitchTab(DebugTabMenuDefOf.Actions);
		}

		// Token: 0x06002223 RID: 8739 RVA: 0x000D9E2B File Offset: 0x000D802B
		private void ToggleTweakValuesMenu()
		{
			if (!Find.WindowStack.TryRemove(typeof(EditWindow_TweakValues), true))
			{
				Find.WindowStack.Add(new EditWindow_TweakValues());
			}
		}

		// Token: 0x06002224 RID: 8740 RVA: 0x000D9E54 File Offset: 0x000D8054
		private void ToggleDebugLogMenu()
		{
			Dialog_Debug dialog_Debug = Find.WindowStack.WindowOfType<Dialog_Debug>();
			if (dialog_Debug == null)
			{
				Find.WindowStack.Add(new Dialog_Debug(DebugTabMenuDefOf.Output));
				return;
			}
			dialog_Debug.SwitchTab(DebugTabMenuDefOf.Output);
		}

		// Token: 0x06002225 RID: 8741 RVA: 0x000D9E8F File Offset: 0x000D808F
		private void ToggleDebugInspector()
		{
			if (!Find.WindowStack.TryRemove(typeof(EditWindow_DebugInspector), true))
			{
				Find.WindowStack.Add(new EditWindow_DebugInspector());
			}
		}

		// Token: 0x06002226 RID: 8742 RVA: 0x000D9EB7 File Offset: 0x000D80B7
		private void ToggleGodMode()
		{
			DebugSettings.godMode = !DebugSettings.godMode;
			if (DebugSettings.godMode)
			{
				SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				return;
			}
			SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
		}

		// Token: 0x06002227 RID: 8743 RVA: 0x000D9EE4 File Offset: 0x000D80E4
		public void TryOpenOrClosePalette()
		{
			if (DebugSettings.devPalette)
			{
				Find.WindowStack.Add(new Dialog_DevPalette());
				return;
			}
			Find.WindowStack.TryRemove(typeof(Dialog_DevPalette), true);
		}

		// Token: 0x06002228 RID: 8744 RVA: 0x000D9F13 File Offset: 0x000D8113
		private void OnSearchChanged()
		{
			this.lastJumpedObject = null;
		}

		// Token: 0x040015B6 RID: 5558
		private QuickSearchWidget quickSearchWidget = new QuickSearchWidget();

		// Token: 0x040015B7 RID: 5559
		protected List<Thing> foundThingsCached = new List<Thing>();

		// Token: 0x040015B8 RID: 5560
		protected Thing lastJumpedObject;

		// Token: 0x040015B9 RID: 5561
		private int searchJumpLastFrame = -1;

		// Token: 0x040015BA RID: 5562
		private Action drawButtonsCached;

		// Token: 0x040015BB RID: 5563
		private WidgetRow widgetRow = new WidgetRow();

		// Token: 0x040015BC RID: 5564
		private const float SearchBarWidth = 240f;
	}
}
