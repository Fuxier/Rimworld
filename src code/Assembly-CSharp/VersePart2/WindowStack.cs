using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000508 RID: 1288
	public class WindowStack
	{
		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x06002732 RID: 10034 RVA: 0x000FBB40 File Offset: 0x000F9D40
		public int Count
		{
			get
			{
				return this.windows.Count;
			}
		}

		// Token: 0x17000765 RID: 1893
		public Window this[int index]
		{
			get
			{
				return this.windows[index];
			}
		}

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06002734 RID: 10036 RVA: 0x000FBB5B File Offset: 0x000F9D5B
		public IList<Window> Windows
		{
			get
			{
				return this.windows.AsReadOnly();
			}
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06002735 RID: 10037 RVA: 0x000FBB68 File Offset: 0x000F9D68
		public FloatMenu FloatMenu
		{
			get
			{
				return this.WindowOfType<FloatMenu>();
			}
		}

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06002736 RID: 10038 RVA: 0x000FBB70 File Offset: 0x000F9D70
		public bool WindowsForcePause
		{
			get
			{
				for (int i = 0; i < this.windows.Count; i++)
				{
					if (this.windows[i].forcePause)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06002737 RID: 10039 RVA: 0x000FBBAC File Offset: 0x000F9DAC
		public bool WindowsPreventCameraMotion
		{
			get
			{
				for (int i = 0; i < this.windows.Count; i++)
				{
					if (this.windows[i].preventCameraMotion)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06002738 RID: 10040 RVA: 0x000FBBE8 File Offset: 0x000F9DE8
		public bool WindowsPreventDrawTutor
		{
			get
			{
				for (int i = 0; i < this.windows.Count; i++)
				{
					if (this.windows[i].preventDrawTutor)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06002739 RID: 10041 RVA: 0x000FBC21 File Offset: 0x000F9E21
		public float SecondsSinceClosedGameStartDialog
		{
			get
			{
				if (this.gameStartDialogOpen)
				{
					return 0f;
				}
				if (this.timeGameStartDialogClosed < 0f)
				{
					return 9999999f;
				}
				return Time.time - this.timeGameStartDialogClosed;
			}
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x0600273A RID: 10042 RVA: 0x000FBC50 File Offset: 0x000F9E50
		public bool MouseObscuredNow
		{
			get
			{
				return this.GetWindowAt(UI.MousePosUIInvertedUseEventIfCan) != this.currentlyDrawnWindow;
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x0600273B RID: 10043 RVA: 0x000FBC68 File Offset: 0x000F9E68
		public bool CurrentWindowGetsInput
		{
			get
			{
				return this.GetsInput(this.currentlyDrawnWindow);
			}
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x0600273C RID: 10044 RVA: 0x000FBC78 File Offset: 0x000F9E78
		public bool NonImmediateDialogWindowOpen
		{
			get
			{
				for (int i = 0; i < this.windows.Count; i++)
				{
					if (!(this.windows[i] is ImmediateWindow) && this.windows[i].layer == WindowLayer.Dialog)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x0600273D RID: 10045 RVA: 0x000FBCC8 File Offset: 0x000F9EC8
		public bool WindowsPreventSave
		{
			get
			{
				for (int i = 0; i < this.windows.Count; i++)
				{
					if (this.windows[i].preventSave)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x0600273E RID: 10046 RVA: 0x000FBD04 File Offset: 0x000F9F04
		public bool AnyWindowAbsorbingAllInput
		{
			get
			{
				using (List<Window>.Enumerator enumerator = this.windows.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.absorbInputAroundWindow)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x0600273F RID: 10047 RVA: 0x000FBD60 File Offset: 0x000F9F60
		public void WindowsUpdate()
		{
			this.AdjustWindowsIfResolutionChanged();
			for (int i = 0; i < this.windows.Count; i++)
			{
				this.windows[i].WindowUpdate();
			}
		}

		// Token: 0x06002740 RID: 10048 RVA: 0x000FBD9C File Offset: 0x000F9F9C
		public void HandleEventsHighPriority()
		{
			if (Event.current.type == EventType.MouseDown && this.GetWindowAt(UI.GUIToScreenPoint(Event.current.mousePosition)) == null)
			{
				bool flag = this.CloseWindowsBecauseClicked(null);
				this.NotifyOutsideClicks(null);
				if (flag)
				{
					Event.current.Use();
				}
			}
			if (KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				this.Notify_PressedCancel();
			}
			if (KeyBindingDefOf.Accept.KeyDownEvent)
			{
				this.Notify_PressedAccept();
			}
			if ((Event.current.type == EventType.MouseDown || Event.current.type == EventType.KeyDown) && !this.GetsInput(null))
			{
				Event.current.Use();
			}
		}

		// Token: 0x06002741 RID: 10049 RVA: 0x000FBE38 File Offset: 0x000FA038
		public void WindowStackOnGUI()
		{
			this.windowStackOnGUITmpList.Clear();
			this.windowStackOnGUITmpList.AddRange(this.windows);
			for (int i = this.windowStackOnGUITmpList.Count - 1; i >= 0; i--)
			{
				this.windowStackOnGUITmpList[i].ExtraOnGUI();
			}
			this.UpdateImmediateWindowsList();
			this.windowStackOnGUITmpList.Clear();
			this.windowStackOnGUITmpList.AddRange(this.windows);
			int j = 0;
			while (j < this.windowStackOnGUITmpList.Count)
			{
				if (!this.windowStackOnGUITmpList[j].drawShadow)
				{
					goto IL_EF;
				}
				if (this.windowStackOnGUITmpList[j].drawInScreenshotMode || !Find.UIRoot.screenshotMode.Active)
				{
					GUI.color = new Color(1f, 1f, 1f, this.windowStackOnGUITmpList[j].shadowAlpha);
					Widgets.DrawShadowAround(this.windowStackOnGUITmpList[j].windowRect);
					GUI.color = Color.white;
					goto IL_EF;
				}
				IL_100:
				j++;
				continue;
				IL_EF:
				this.windowStackOnGUITmpList[j].WindowOnGUI();
				goto IL_100;
			}
			if (this.updateInternalWindowsOrderLater)
			{
				this.updateInternalWindowsOrderLater = false;
				this.UpdateInternalWindowsOrder();
			}
		}

		// Token: 0x06002742 RID: 10050 RVA: 0x000FBF70 File Offset: 0x000FA170
		public void Notify_ClickedInsideWindow(Window window)
		{
			if (this.GetsInput(window))
			{
				this.windows.Remove(window);
				this.InsertAtCorrectPositionInList(window);
				this.focusedWindow = window;
			}
			else
			{
				Event.current.Use();
			}
			this.CloseWindowsBecauseClicked(window);
			this.NotifyOutsideClicks(window);
			this.updateInternalWindowsOrderLater = true;
		}

		// Token: 0x06002743 RID: 10051 RVA: 0x000FBFC3 File Offset: 0x000FA1C3
		public void Notify_ManuallySetFocus(Window window)
		{
			this.focusedWindow = window;
			this.updateInternalWindowsOrderLater = true;
		}

		// Token: 0x06002744 RID: 10052 RVA: 0x000FBFD4 File Offset: 0x000FA1D4
		public void Notify_PressedCancel()
		{
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				if ((this.windows[i].closeOnCancel || this.windows[i].forceCatchAcceptAndCancelEventEvenIfUnfocused) && this.GetsInput(this.windows[i]))
				{
					this.windows[i].OnCancelKeyPressed();
					return;
				}
			}
		}

		// Token: 0x06002745 RID: 10053 RVA: 0x000FC048 File Offset: 0x000FA248
		public void Notify_PressedAccept()
		{
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				if ((this.windows[i].closeOnAccept || this.windows[i].forceCatchAcceptAndCancelEventEvenIfUnfocused) && this.GetsInput(this.windows[i]))
				{
					this.windows[i].OnAcceptKeyPressed();
					return;
				}
			}
		}

		// Token: 0x06002746 RID: 10054 RVA: 0x000FC0B9 File Offset: 0x000FA2B9
		public void Notify_GameStartDialogOpened()
		{
			this.gameStartDialogOpen = true;
		}

		// Token: 0x06002747 RID: 10055 RVA: 0x000FC0C2 File Offset: 0x000FA2C2
		public void Notify_GameStartDialogClosed()
		{
			this.timeGameStartDialogClosed = Time.time;
			this.gameStartDialogOpen = false;
		}

		// Token: 0x06002748 RID: 10056 RVA: 0x000FC0D8 File Offset: 0x000FA2D8
		public bool IsOpen<WindowType>()
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i] is WindowType)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002749 RID: 10057 RVA: 0x000FC114 File Offset: 0x000FA314
		public bool IsOpen(Type type)
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i].GetType() == type)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600274A RID: 10058 RVA: 0x000FC153 File Offset: 0x000FA353
		public bool IsOpen(Window window)
		{
			return this.windows.Contains(window);
		}

		// Token: 0x0600274B RID: 10059 RVA: 0x000FC164 File Offset: 0x000FA364
		public WindowType WindowOfType<WindowType>() where WindowType : class
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i] is WindowType)
				{
					return this.windows[i] as WindowType;
				}
			}
			return default(WindowType);
		}

		// Token: 0x0600274C RID: 10060 RVA: 0x000FC1BC File Offset: 0x000FA3BC
		public bool GetsInput(Window window)
		{
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				if (this.windows[i] == window)
				{
					return true;
				}
				if (this.windows[i].absorbInputAroundWindow)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600274D RID: 10061 RVA: 0x000FC208 File Offset: 0x000FA408
		public void Add(Window window)
		{
			this.RemoveWindowsOfType(window.GetType());
			window.ID = WindowStack.uniqueWindowID++;
			window.PreOpen();
			this.InsertAtCorrectPositionInList(window);
			this.FocusAfterInsertIfShould(window);
			this.updateInternalWindowsOrderLater = true;
			window.PostOpen();
		}

		// Token: 0x0600274E RID: 10062 RVA: 0x000FC258 File Offset: 0x000FA458
		public void ImmediateWindow(int ID, Rect rect, WindowLayer layer, Action doWindowFunc, bool doBackground = true, bool absorbInputAroundWindow = false, float shadowAlpha = 1f, Action doClickOutsideFunc = null)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			if (ID == 0)
			{
				Log.Warning("Used 0 as immediate window ID.");
				return;
			}
			ID = -Math.Abs(ID);
			bool flag = false;
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i].ID == ID)
				{
					ImmediateWindow immediateWindow = (ImmediateWindow)this.windows[i];
					immediateWindow.windowRect = rect;
					immediateWindow.doWindowFunc = doWindowFunc;
					immediateWindow.doClickOutsideFunc = doClickOutsideFunc;
					immediateWindow.layer = layer;
					immediateWindow.doWindowBackground = doBackground;
					immediateWindow.absorbInputAroundWindow = absorbInputAroundWindow;
					immediateWindow.shadowAlpha = shadowAlpha;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.AddNewImmediateWindow(ID, rect, layer, doWindowFunc, doBackground, absorbInputAroundWindow, shadowAlpha, doClickOutsideFunc);
			}
			this.immediateWindowsRequests.Add(ID);
		}

		// Token: 0x0600274F RID: 10063 RVA: 0x000FC324 File Offset: 0x000FA524
		public bool TryRemove(Type windowType, bool doCloseSound = true)
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i].GetType() == windowType)
				{
					return this.TryRemove(this.windows[i], doCloseSound);
				}
			}
			return false;
		}

		// Token: 0x06002750 RID: 10064 RVA: 0x000FC378 File Offset: 0x000FA578
		public bool TryRemoveAssignableFromType(Type windowType, bool doCloseSound = true)
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (windowType.IsAssignableFrom(this.windows[i].GetType()))
				{
					return this.TryRemove(this.windows[i], doCloseSound);
				}
			}
			return false;
		}

		// Token: 0x06002751 RID: 10065 RVA: 0x000FC3CC File Offset: 0x000FA5CC
		public bool TryRemove(Window window, bool doCloseSound = true)
		{
			bool flag = false;
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i] == window)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
			if (!window.OnCloseRequest())
			{
				return false;
			}
			if (doCloseSound && window.soundClose != null)
			{
				window.soundClose.PlayOneShotOnCamera(null);
			}
			window.PreClose();
			this.windows.Remove(window);
			window.PostClose();
			if (this.focusedWindow == window)
			{
				if (this.windows.Count > 0)
				{
					this.focusedWindow = this.windows[this.windows.Count - 1];
				}
				else
				{
					this.focusedWindow = null;
				}
				this.updateInternalWindowsOrderLater = true;
			}
			return true;
		}

		// Token: 0x06002752 RID: 10066 RVA: 0x000FC48C File Offset: 0x000FA68C
		public Window GetWindowAt(Vector2 pos)
		{
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				if (this.windows[i].windowRect.Contains(pos))
				{
					return this.windows[i];
				}
			}
			return null;
		}

		// Token: 0x06002753 RID: 10067 RVA: 0x000FC4D8 File Offset: 0x000FA6D8
		private void AddNewImmediateWindow(int ID, Rect rect, WindowLayer layer, Action doWindowFunc, bool doBackground, bool absorbInputAroundWindow, float shadowAlpha, Action doClickOutsideFunc)
		{
			if (ID >= 0)
			{
				Log.Error("Invalid immediate window ID.");
				return;
			}
			ImmediateWindow immediateWindow = new ImmediateWindow();
			immediateWindow.ID = ID;
			immediateWindow.layer = layer;
			immediateWindow.doWindowFunc = doWindowFunc;
			immediateWindow.doClickOutsideFunc = doClickOutsideFunc;
			immediateWindow.doWindowBackground = doBackground;
			immediateWindow.absorbInputAroundWindow = absorbInputAroundWindow;
			immediateWindow.shadowAlpha = shadowAlpha;
			immediateWindow.PreOpen();
			immediateWindow.windowRect = rect;
			this.InsertAtCorrectPositionInList(immediateWindow);
			this.FocusAfterInsertIfShould(immediateWindow);
			this.updateInternalWindowsOrderLater = true;
			immediateWindow.PostOpen();
		}

		// Token: 0x06002754 RID: 10068 RVA: 0x000FC558 File Offset: 0x000FA758
		private void UpdateImmediateWindowsList()
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			this.updateImmediateWindowsListTmpList.Clear();
			this.updateImmediateWindowsListTmpList.AddRange(this.windows);
			for (int i = 0; i < this.updateImmediateWindowsListTmpList.Count; i++)
			{
				if (this.IsImmediateWindow(this.updateImmediateWindowsListTmpList[i]))
				{
					bool flag = false;
					for (int j = 0; j < this.immediateWindowsRequests.Count; j++)
					{
						if (this.immediateWindowsRequests[j] == this.updateImmediateWindowsListTmpList[i].ID)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.TryRemove(this.updateImmediateWindowsListTmpList[i], true);
					}
				}
			}
			this.immediateWindowsRequests.Clear();
		}

		// Token: 0x06002755 RID: 10069 RVA: 0x000FC618 File Offset: 0x000FA818
		private void InsertAtCorrectPositionInList(Window window)
		{
			int index = 0;
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (window.layer >= this.windows[i].layer)
				{
					index = i + 1;
				}
			}
			this.windows.Insert(index, window);
			this.updateInternalWindowsOrderLater = true;
		}

		// Token: 0x06002756 RID: 10070 RVA: 0x000FC670 File Offset: 0x000FA870
		private void FocusAfterInsertIfShould(Window window)
		{
			if (!window.focusWhenOpened)
			{
				return;
			}
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				if (this.windows[i] == window)
				{
					this.focusedWindow = this.windows[i];
					this.updateInternalWindowsOrderLater = true;
					return;
				}
				if (this.windows[i] == this.focusedWindow)
				{
					break;
				}
			}
		}

		// Token: 0x06002757 RID: 10071 RVA: 0x000FC6DC File Offset: 0x000FA8DC
		private void AdjustWindowsIfResolutionChanged()
		{
			IntVec2 a = new IntVec2(UI.screenWidth, UI.screenHeight);
			if (!UnityGUIBugsFixer.ResolutionsEqual(a, this.prevResolution))
			{
				this.prevResolution = a;
				for (int i = 0; i < this.windows.Count; i++)
				{
					this.windows[i].Notify_ResolutionChanged();
				}
				if (Current.ProgramState == ProgramState.Playing)
				{
					Find.ColonistBar.MarkColonistsDirty();
				}
			}
		}

		// Token: 0x06002758 RID: 10072 RVA: 0x000FC748 File Offset: 0x000FA948
		private void RemoveWindowsOfType(Type type)
		{
			this.removeWindowsOfTypeTmpList.Clear();
			this.removeWindowsOfTypeTmpList.AddRange(this.windows);
			for (int i = 0; i < this.removeWindowsOfTypeTmpList.Count; i++)
			{
				if (this.removeWindowsOfTypeTmpList[i].onlyOneOfTypeAllowed && this.removeWindowsOfTypeTmpList[i].GetType() == type)
				{
					this.TryRemove(this.removeWindowsOfTypeTmpList[i], true);
				}
			}
		}

		// Token: 0x06002759 RID: 10073 RVA: 0x000FC7C8 File Offset: 0x000FA9C8
		private void NotifyOutsideClicks(Window clickedWindow)
		{
			foreach (Window window in this.windows)
			{
				if (window != clickedWindow)
				{
					window.Notify_ClickOutsideWindow();
				}
			}
		}

		// Token: 0x0600275A RID: 10074 RVA: 0x000FC820 File Offset: 0x000FAA20
		private bool CloseWindowsBecauseClicked(Window clickedWindow)
		{
			this.closeWindowsTmpList.Clear();
			this.closeWindowsTmpList.AddRange(this.windows);
			bool result = false;
			int num = this.closeWindowsTmpList.Count - 1;
			while (num >= 0 && this.closeWindowsTmpList[num] != clickedWindow)
			{
				if (this.closeWindowsTmpList[num].closeOnClickedOutside)
				{
					result = true;
					this.TryRemove(this.closeWindowsTmpList[num], true);
				}
				num--;
			}
			return result;
		}

		// Token: 0x0600275B RID: 10075 RVA: 0x000FC89C File Offset: 0x000FAA9C
		private bool IsImmediateWindow(Window window)
		{
			return window.ID < 0;
		}

		// Token: 0x0600275C RID: 10076 RVA: 0x000FC8A8 File Offset: 0x000FAAA8
		private void UpdateInternalWindowsOrder()
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				GUI.BringWindowToFront(this.windows[i].ID);
			}
			if (this.focusedWindow != null)
			{
				GUI.FocusWindow(this.focusedWindow.ID);
			}
		}

		// Token: 0x040019E8 RID: 6632
		public Window currentlyDrawnWindow;

		// Token: 0x040019E9 RID: 6633
		private List<Window> windows = new List<Window>();

		// Token: 0x040019EA RID: 6634
		private List<int> immediateWindowsRequests = new List<int>();

		// Token: 0x040019EB RID: 6635
		private bool updateInternalWindowsOrderLater;

		// Token: 0x040019EC RID: 6636
		private Window focusedWindow;

		// Token: 0x040019ED RID: 6637
		private static int uniqueWindowID;

		// Token: 0x040019EE RID: 6638
		private bool gameStartDialogOpen;

		// Token: 0x040019EF RID: 6639
		private float timeGameStartDialogClosed = -1f;

		// Token: 0x040019F0 RID: 6640
		private IntVec2 prevResolution = new IntVec2(UI.screenWidth, UI.screenHeight);

		// Token: 0x040019F1 RID: 6641
		private List<Window> windowStackOnGUITmpList = new List<Window>();

		// Token: 0x040019F2 RID: 6642
		private List<Window> updateImmediateWindowsListTmpList = new List<Window>();

		// Token: 0x040019F3 RID: 6643
		private List<Window> removeWindowsOfTypeTmpList = new List<Window>();

		// Token: 0x040019F4 RID: 6644
		private List<Window> closeWindowsTmpList = new List<Window>();
	}
}
