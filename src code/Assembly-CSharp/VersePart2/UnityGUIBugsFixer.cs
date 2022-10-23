using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x020004E7 RID: 1255
	public static class UnityGUIBugsFixer
	{
		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x060025C1 RID: 9665 RVA: 0x000F0A22 File Offset: 0x000EEC22
		public static bool IsSteamDeckOrLinuxBuild
		{
			get
			{
				return SteamDeck.IsSteamDeck || Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.LinuxPlayer;
			}
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x060025C2 RID: 9666 RVA: 0x000F0A40 File Offset: 0x000EEC40
		public static List<Resolution> ScreenResolutionsWithoutDuplicates
		{
			get
			{
				UnityGUIBugsFixer.resolutions.Clear();
				Resolution[] array = Screen.resolutions;
				for (int i = 0; i < array.Length; i++)
				{
					bool flag = false;
					for (int j = 0; j < UnityGUIBugsFixer.resolutions.Count; j++)
					{
						if (UnityGUIBugsFixer.resolutions[j].width == array[i].width && UnityGUIBugsFixer.resolutions[j].height == array[i].height)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						UnityGUIBugsFixer.resolutions.Add(array[i]);
					}
				}
				return UnityGUIBugsFixer.resolutions;
			}
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x060025C3 RID: 9667 RVA: 0x000F0AE3 File Offset: 0x000EECE3
		public static Vector2 CurrentEventDelta
		{
			get
			{
				return UnityGUIBugsFixer.currentEventDelta;
			}
		}

		// Token: 0x060025C4 RID: 9668 RVA: 0x000F0AEA File Offset: 0x000EECEA
		public static void OnGUI()
		{
			UnityGUIBugsFixer.RememberMouseStateForIsLeftMouseButtonPressed();
			UnityGUIBugsFixer.FixSteamDeckMousePositionNeverUpdating();
			UnityGUIBugsFixer.FixScrolling();
			UnityGUIBugsFixer.FixShift();
			UnityGUIBugsFixer.FixDelta();
		}

		// Token: 0x060025C5 RID: 9669 RVA: 0x000F0B08 File Offset: 0x000EED08
		private static void FixScrolling()
		{
			if (Event.current.type == EventType.ScrollWheel && (Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.LinuxPlayer))
			{
				Vector2 delta = Event.current.delta;
				Event.current.delta = new Vector2(delta.x, delta.y * -6f);
			}
		}

		// Token: 0x060025C6 RID: 9670 RVA: 0x000F0B60 File Offset: 0x000EED60
		private static void FixShift()
		{
			if ((Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.LinuxPlayer) && !Event.current.shift)
			{
				Event.current.shift = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
			}
		}

		// Token: 0x060025C7 RID: 9671 RVA: 0x000F0BAE File Offset: 0x000EEDAE
		public static bool ResolutionsEqual(IntVec2 a, IntVec2 b)
		{
			return a == b;
		}

		// Token: 0x060025C8 RID: 9672 RVA: 0x000F0BB8 File Offset: 0x000EEDB8
		private static void FixDelta()
		{
			Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
			if (UnityGUIBugsFixer.IsSteamDeckOrLinuxBuild)
			{
				if (Event.current.rawType == EventType.MouseDown)
				{
					UnityGUIBugsFixer.lastMousePosition = new Vector2?(mousePositionOnUIInverted);
					UnityGUIBugsFixer.lastMousePositionFrame = Time.frameCount;
					return;
				}
				if (Event.current.type != EventType.Repaint)
				{
					UnityGUIBugsFixer.currentEventDelta = default(Vector2);
					return;
				}
				if (Time.frameCount != UnityGUIBugsFixer.lastMousePositionFrame)
				{
					if (UnityGUIBugsFixer.lastMousePosition != null)
					{
						UnityGUIBugsFixer.currentEventDelta = mousePositionOnUIInverted - UnityGUIBugsFixer.lastMousePosition.Value;
					}
					else
					{
						UnityGUIBugsFixer.currentEventDelta = default(Vector2);
					}
					UnityGUIBugsFixer.lastMousePosition = new Vector2?(mousePositionOnUIInverted);
					UnityGUIBugsFixer.lastMousePositionFrame = Time.frameCount;
					return;
				}
			}
			else if (Event.current.rawType == EventType.MouseDrag)
			{
				if (mousePositionOnUIInverted != UnityGUIBugsFixer.lastMousePosition || Time.frameCount != UnityGUIBugsFixer.lastMousePositionFrame)
				{
					if (UnityGUIBugsFixer.lastMousePosition != null)
					{
						UnityGUIBugsFixer.currentEventDelta = mousePositionOnUIInverted - UnityGUIBugsFixer.lastMousePosition.Value;
					}
					else
					{
						UnityGUIBugsFixer.currentEventDelta = default(Vector2);
					}
					UnityGUIBugsFixer.lastMousePosition = new Vector2?(mousePositionOnUIInverted);
					UnityGUIBugsFixer.lastMousePositionFrame = Time.frameCount;
					return;
				}
			}
			else
			{
				UnityGUIBugsFixer.currentEventDelta = Event.current.delta;
				if (Event.current.rawType == EventType.MouseDown)
				{
					UnityGUIBugsFixer.lastMousePosition = new Vector2?(mousePositionOnUIInverted);
					UnityGUIBugsFixer.lastMousePositionFrame = Time.frameCount;
					return;
				}
				if (Event.current.rawType == EventType.MouseUp)
				{
					UnityGUIBugsFixer.lastMousePosition = null;
				}
			}
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x000F0D2F File Offset: 0x000EEF2F
		public static void Notify_BeginGroup()
		{
			UnityGUIBugsFixer.FixSteamDeckMousePositionNeverUpdating();
		}

		// Token: 0x060025CA RID: 9674 RVA: 0x000F0D2F File Offset: 0x000EEF2F
		public static void Notify_EndGroup()
		{
			UnityGUIBugsFixer.FixSteamDeckMousePositionNeverUpdating();
		}

		// Token: 0x060025CB RID: 9675 RVA: 0x000F0D2F File Offset: 0x000EEF2F
		public static void Notify_BeginScrollView()
		{
			UnityGUIBugsFixer.FixSteamDeckMousePositionNeverUpdating();
		}

		// Token: 0x060025CC RID: 9676 RVA: 0x000F0D2F File Offset: 0x000EEF2F
		public static void Notify_EndScrollView()
		{
			UnityGUIBugsFixer.FixSteamDeckMousePositionNeverUpdating();
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x000F0D2F File Offset: 0x000EEF2F
		public static void Notify_GUIMatrixChanged()
		{
			UnityGUIBugsFixer.FixSteamDeckMousePositionNeverUpdating();
		}

		// Token: 0x060025CE RID: 9678 RVA: 0x000F0D38 File Offset: 0x000EEF38
		private static void FixSteamDeckMousePositionNeverUpdating()
		{
			if (UnityGUIBugsFixer.IsSteamDeckOrLinuxBuild)
			{
				Vector2 vector = UI.MousePositionOnUIInverted;
				vector = GUIUtility.ScreenToGUIPoint(vector * Prefs.UIScale);
				Event.current.mousePosition = vector;
			}
		}

		// Token: 0x060025CF RID: 9679 RVA: 0x000F0D6E File Offset: 0x000EEF6E
		private static void RememberMouseStateForIsLeftMouseButtonPressed()
		{
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
			{
				UnityGUIBugsFixer.leftMouseButtonPressed = true;
				return;
			}
			if (Event.current.rawType == EventType.MouseUp && Event.current.button == 0)
			{
				UnityGUIBugsFixer.leftMouseButtonPressed = false;
			}
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x000F0DAE File Offset: 0x000EEFAE
		public static bool IsLeftMouseButtonPressed()
		{
			return Input.GetMouseButton(0) || UnityGUIBugsFixer.leftMouseButtonPressed;
		}

		// Token: 0x040018A4 RID: 6308
		private static List<Resolution> resolutions = new List<Resolution>();

		// Token: 0x040018A5 RID: 6309
		private static Vector2 currentEventDelta;

		// Token: 0x040018A6 RID: 6310
		private static int lastMousePositionFrame;

		// Token: 0x040018A7 RID: 6311
		private static bool leftMouseButtonPressed;

		// Token: 0x040018A8 RID: 6312
		private const float ScrollFactor = -6f;

		// Token: 0x040018A9 RID: 6313
		private static Vector2? lastMousePosition;
	}
}
