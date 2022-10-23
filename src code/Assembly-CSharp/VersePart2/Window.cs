using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000506 RID: 1286
	public abstract class Window
	{
		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06002715 RID: 10005 RVA: 0x000FB038 File Offset: 0x000F9238
		public virtual Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 500f);
			}
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06002716 RID: 10006 RVA: 0x000FB25B File Offset: 0x000F945B
		protected virtual float Margin
		{
			get
			{
				return 18f;
			}
		}

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06002717 RID: 10007 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual bool IsDebug
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x06002718 RID: 10008 RVA: 0x000FB262 File Offset: 0x000F9462
		public bool IsOpen
		{
			get
			{
				return Find.WindowStack.IsOpen(this);
			}
		}

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x06002719 RID: 10009 RVA: 0x000029B0 File Offset: 0x00000BB0
		public virtual QuickSearchWidget CommonSearchWidget
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x0600271A RID: 10010 RVA: 0x000FB26F File Offset: 0x000F946F
		public virtual string CloseButtonText
		{
			get
			{
				return "CloseButton".Translate();
			}
		}

		// Token: 0x0600271B RID: 10011 RVA: 0x000FB280 File Offset: 0x000F9480
		public Window()
		{
			this.soundAppear = SoundDefOf.DialogBoxAppear;
			this.soundClose = SoundDefOf.Click;
			this.onGUIProfilerLabelCached = "WindowOnGUI: " + base.GetType().Name;
			this.extraOnGUIProfilerLabelCached = "ExtraOnGUI: " + base.GetType().Name;
			this.innerWindowOnGUICached = new GUI.WindowFunction(this.InnerWindowOnGUI);
			this.notify_CommonSearchChangedCached = new Action(this.Notify_CommonSearchChanged);
		}

		// Token: 0x0600271C RID: 10012 RVA: 0x000FB37D File Offset: 0x000F957D
		public virtual void WindowUpdate()
		{
			if (this.sustainerAmbient != null)
			{
				this.sustainerAmbient.Maintain();
			}
		}

		// Token: 0x0600271D RID: 10013
		public abstract void DoWindowContents(Rect inRect);

		// Token: 0x0600271E RID: 10014 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void ExtraOnGUI()
		{
		}

		// Token: 0x0600271F RID: 10015 RVA: 0x000FB394 File Offset: 0x000F9594
		public virtual void PreOpen()
		{
			this.SetInitialSizeAndPosition();
			QuickSearchWidget commonSearchWidget = this.CommonSearchWidget;
			if (commonSearchWidget != null)
			{
				commonSearchWidget.Reset();
			}
			if (this.layer == WindowLayer.Dialog)
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					Find.DesignatorManager.Dragger.EndDrag();
					Find.DesignatorManager.Deselect();
					Find.Selector.Notify_DialogOpened();
				}
				if (Find.World != null)
				{
					Find.WorldSelector.Notify_DialogOpened();
				}
			}
		}

		// Token: 0x06002720 RID: 10016 RVA: 0x000FB3FD File Offset: 0x000F95FD
		public virtual void PostOpen()
		{
			if (this.soundAppear != null)
			{
				this.soundAppear.PlayOneShotOnCamera(null);
			}
			if (this.soundAmbient != null)
			{
				this.sustainerAmbient = this.soundAmbient.TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.PerFrame));
			}
		}

		// Token: 0x06002721 RID: 10017 RVA: 0x00002662 File Offset: 0x00000862
		public virtual bool OnCloseRequest()
		{
			return true;
		}

		// Token: 0x06002722 RID: 10018 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void PreClose()
		{
		}

		// Token: 0x06002723 RID: 10019 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void PostClose()
		{
		}

		// Token: 0x06002724 RID: 10020 RVA: 0x000FB434 File Offset: 0x000F9634
		public virtual void WindowOnGUI()
		{
			if (!this.drawInScreenshotMode && Find.UIRoot.screenshotMode.Active)
			{
				return;
			}
			if (this.onlyDrawInDevMode && !Prefs.DevMode)
			{
				return;
			}
			if (this.resizeable)
			{
				if (this.resizer == null)
				{
					this.resizer = new WindowResizer();
				}
				if (this.resizeLater)
				{
					this.resizeLater = false;
					this.windowRect = this.resizeLaterRect;
				}
			}
			this.windowRect = this.windowRect.Rounded();
			this.windowRect = GUI.Window(this.ID, this.windowRect, this.innerWindowOnGUICached, "", Widgets.EmptyStyle);
		}

		// Token: 0x06002725 RID: 10021 RVA: 0x000FB4DC File Offset: 0x000F96DC
		private void InnerWindowOnGUI(int x)
		{
			UnityGUIBugsFixer.OnGUI();
			SteamDeck.WindowOnGUI();
			OriginalEventUtility.RecordOriginalEvent(Event.current);
			Rect rect = this.windowRect.AtZero();
			Find.WindowStack.currentlyDrawnWindow = this;
			if (this.doWindowBackground)
			{
				Widgets.DrawWindowBackground(rect);
			}
			if (KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				Find.WindowStack.Notify_PressedCancel();
			}
			if (KeyBindingDefOf.Accept.KeyDownEvent)
			{
				Find.WindowStack.Notify_PressedAccept();
			}
			if (Event.current.type == EventType.MouseDown)
			{
				Find.WindowStack.Notify_ClickedInsideWindow(this);
			}
			if (Event.current.type == EventType.KeyDown && !Find.WindowStack.GetsInput(this))
			{
				Event.current.Use();
			}
			if (!this.optionalTitle.NullOrEmpty())
			{
				GUI.Label(new Rect(this.Margin, this.Margin, this.windowRect.width, 25f), this.optionalTitle);
			}
			if (this.doCloseX && Widgets.CloseButtonFor(rect))
			{
				this.Close(true);
			}
			if (this.resizeable && Event.current.type != EventType.Repaint)
			{
				Rect lhs = this.resizer.DoResizeControl(this.windowRect);
				if (lhs != this.windowRect)
				{
					this.resizeLater = true;
					this.resizeLaterRect = lhs;
				}
			}
			Rect rect2 = rect.ContractedBy(this.Margin);
			if (!this.optionalTitle.NullOrEmpty())
			{
				rect2.yMin += this.Margin + 25f;
			}
			QuickSearchWidget commonSearchWidget = this.CommonSearchWidget;
			if (commonSearchWidget != null)
			{
				Rect rect3 = new Rect(rect.x + this.commonSearchWidgetOffset.x, rect.height - 55f + this.commonSearchWidgetOffset.y, Window.QuickSearchSize.x, Window.QuickSearchSize.y);
				commonSearchWidget.OnGUI(rect3, this.notify_CommonSearchChangedCached);
			}
			Widgets.BeginGroup(rect2);
			try
			{
				this.DoWindowContents(rect2.AtZero());
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception filling window for ",
					base.GetType(),
					": ",
					ex
				}));
			}
			Widgets.EndGroup();
			this.LateWindowOnGUI(rect2);
			if (this.grayOutIfOtherDialogOpen)
			{
				IList<Window> windows = Find.WindowStack.Windows;
				for (int i = 0; i < windows.Count; i++)
				{
					if (windows[i].layer == WindowLayer.Dialog && !(windows[i] is Page) && windows[i] != this)
					{
						Widgets.DrawRectFast(rect, new Color(0f, 0f, 0f, 0.5f), null);
						break;
					}
				}
			}
			if (this.resizeable && Event.current.type == EventType.Repaint)
			{
				this.resizer.DoResizeControl(this.windowRect);
			}
			if (this.doCloseButton)
			{
				Text.Font = GameFont.Small;
				if (Widgets.ButtonText(new Rect(rect.width / 2f - Window.CloseButSize.x / 2f, rect.height - 55f, Window.CloseButSize.x, Window.CloseButSize.y), this.CloseButtonText, true, true, true, null))
				{
					this.Close(true);
				}
			}
			if (KeyBindingDefOf.Cancel.KeyDownEvent && this.IsOpen)
			{
				this.OnCancelKeyPressed();
			}
			if (this.draggable)
			{
				GUI.DragWindow();
			}
			else if (Event.current.type == EventType.MouseDown)
			{
				Event.current.Use();
			}
			ScreenFader.OverlayOnGUI(rect.size);
			Find.WindowStack.currentlyDrawnWindow = null;
			OriginalEventUtility.Reset();
		}

		// Token: 0x06002726 RID: 10022 RVA: 0x000034B7 File Offset: 0x000016B7
		protected virtual void LateWindowOnGUI(Rect inRect)
		{
		}

		// Token: 0x06002727 RID: 10023 RVA: 0x000FB880 File Offset: 0x000F9A80
		public virtual void Close(bool doCloseSound = true)
		{
			Find.WindowStack.TryRemove(this, doCloseSound);
		}

		// Token: 0x06002728 RID: 10024 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual bool CausesMessageBackground()
		{
			return false;
		}

		// Token: 0x06002729 RID: 10025 RVA: 0x000FB890 File Offset: 0x000F9A90
		protected virtual void SetInitialSizeAndPosition()
		{
			Vector2 initialSize = this.InitialSize;
			this.windowRect = new Rect(((float)UI.screenWidth - initialSize.x) / 2f, ((float)UI.screenHeight - initialSize.y) / 2f, initialSize.x, initialSize.y);
			this.windowRect = this.windowRect.Rounded();
		}

		// Token: 0x0600272A RID: 10026 RVA: 0x000FB8F2 File Offset: 0x000F9AF2
		public virtual void OnCancelKeyPressed()
		{
			if (this.closeOnCancel)
			{
				this.Close(true);
				Event.current.Use();
			}
			if (this.openMenuOnCancel)
			{
				Find.MainTabsRoot.ToggleTab(MainButtonDefOf.Menu, true);
			}
		}

		// Token: 0x0600272B RID: 10027 RVA: 0x000FB925 File Offset: 0x000F9B25
		public virtual void OnAcceptKeyPressed()
		{
			if (this.closeOnAccept)
			{
				this.Close(true);
				Event.current.Use();
			}
		}

		// Token: 0x0600272C RID: 10028 RVA: 0x000FB940 File Offset: 0x000F9B40
		public virtual void Notify_ResolutionChanged()
		{
			this.SetInitialSizeAndPosition();
		}

		// Token: 0x0600272D RID: 10029 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Notify_CommonSearchChanged()
		{
		}

		// Token: 0x0600272E RID: 10030 RVA: 0x000FB948 File Offset: 0x000F9B48
		public virtual void Notify_ClickOutsideWindow()
		{
			QuickSearchWidget commonSearchWidget = this.CommonSearchWidget;
			if (commonSearchWidget == null)
			{
				return;
			}
			commonSearchWidget.Unfocus();
		}

		// Token: 0x040019B9 RID: 6585
		public WindowLayer layer = WindowLayer.Dialog;

		// Token: 0x040019BA RID: 6586
		public string optionalTitle;

		// Token: 0x040019BB RID: 6587
		public bool doCloseX;

		// Token: 0x040019BC RID: 6588
		public bool doCloseButton;

		// Token: 0x040019BD RID: 6589
		public bool closeOnAccept = true;

		// Token: 0x040019BE RID: 6590
		public bool closeOnCancel = true;

		// Token: 0x040019BF RID: 6591
		public bool forceCatchAcceptAndCancelEventEvenIfUnfocused;

		// Token: 0x040019C0 RID: 6592
		public bool closeOnClickedOutside;

		// Token: 0x040019C1 RID: 6593
		public bool forcePause;

		// Token: 0x040019C2 RID: 6594
		public bool preventCameraMotion = true;

		// Token: 0x040019C3 RID: 6595
		public bool preventDrawTutor;

		// Token: 0x040019C4 RID: 6596
		public bool doWindowBackground = true;

		// Token: 0x040019C5 RID: 6597
		public bool onlyOneOfTypeAllowed = true;

		// Token: 0x040019C6 RID: 6598
		public bool absorbInputAroundWindow;

		// Token: 0x040019C7 RID: 6599
		public bool resizeable;

		// Token: 0x040019C8 RID: 6600
		public bool draggable;

		// Token: 0x040019C9 RID: 6601
		public bool drawShadow = true;

		// Token: 0x040019CA RID: 6602
		public bool focusWhenOpened = true;

		// Token: 0x040019CB RID: 6603
		public float shadowAlpha = 1f;

		// Token: 0x040019CC RID: 6604
		public SoundDef soundAppear;

		// Token: 0x040019CD RID: 6605
		public SoundDef soundClose;

		// Token: 0x040019CE RID: 6606
		public SoundDef soundAmbient;

		// Token: 0x040019CF RID: 6607
		public bool silenceAmbientSound;

		// Token: 0x040019D0 RID: 6608
		public bool grayOutIfOtherDialogOpen;

		// Token: 0x040019D1 RID: 6609
		public Vector2 commonSearchWidgetOffset = new Vector2(0f, Window.CloseButSize.y - Window.QuickSearchSize.y) / 2f;

		// Token: 0x040019D2 RID: 6610
		public bool openMenuOnCancel;

		// Token: 0x040019D3 RID: 6611
		public bool preventSave;

		// Token: 0x040019D4 RID: 6612
		public bool drawInScreenshotMode = true;

		// Token: 0x040019D5 RID: 6613
		public bool onlyDrawInDevMode;

		// Token: 0x040019D6 RID: 6614
		public const float StandardMargin = 18f;

		// Token: 0x040019D7 RID: 6615
		public const float FooterRowHeight = 55f;

		// Token: 0x040019D8 RID: 6616
		public static readonly Vector2 CloseButSize = new Vector2(120f, 40f);

		// Token: 0x040019D9 RID: 6617
		public static readonly Vector2 QuickSearchSize = new Vector2(240f, 24f);

		// Token: 0x040019DA RID: 6618
		public int ID;

		// Token: 0x040019DB RID: 6619
		public Rect windowRect;

		// Token: 0x040019DC RID: 6620
		private Sustainer sustainerAmbient;

		// Token: 0x040019DD RID: 6621
		private WindowResizer resizer;

		// Token: 0x040019DE RID: 6622
		private bool resizeLater;

		// Token: 0x040019DF RID: 6623
		private Rect resizeLaterRect;

		// Token: 0x040019E0 RID: 6624
		private string onGUIProfilerLabelCached;

		// Token: 0x040019E1 RID: 6625
		public string extraOnGUIProfilerLabelCached;

		// Token: 0x040019E2 RID: 6626
		private GUI.WindowFunction innerWindowOnGUICached;

		// Token: 0x040019E3 RID: 6627
		private Action notify_CommonSearchChangedCached;
	}
}
