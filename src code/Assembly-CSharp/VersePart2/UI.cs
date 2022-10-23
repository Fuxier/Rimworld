using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000042 RID: 66
	public static class UI
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000391 RID: 913 RVA: 0x00014098 File Offset: 0x00012298
		public static Vector2 MousePositionOnUI
		{
			get
			{
				return Input.mousePosition / Prefs.UIScale;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000392 RID: 914 RVA: 0x000140B0 File Offset: 0x000122B0
		public static Vector2 MousePositionOnUIInverted
		{
			get
			{
				Vector2 mousePositionOnUI = UI.MousePositionOnUI;
				mousePositionOnUI.y = (float)UI.screenHeight - mousePositionOnUI.y;
				return mousePositionOnUI;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000393 RID: 915 RVA: 0x000140D8 File Offset: 0x000122D8
		public static Vector2 MousePosUIInvertedUseEventIfCan
		{
			get
			{
				if (Event.current != null)
				{
					return UI.GUIToScreenPoint(Event.current.mousePosition);
				}
				return UI.MousePositionOnUIInverted;
			}
		}

		// Token: 0x06000394 RID: 916 RVA: 0x000140F8 File Offset: 0x000122F8
		public static void ApplyUIScale()
		{
			if (Prefs.UIScale == 1f)
			{
				UI.screenWidth = Screen.width;
				UI.screenHeight = Screen.height;
				return;
			}
			UI.screenWidth = Mathf.RoundToInt((float)Screen.width / Prefs.UIScale);
			UI.screenHeight = Mathf.RoundToInt((float)Screen.height / Prefs.UIScale);
			float uiscale = Prefs.UIScale;
			float uiscale2 = Prefs.UIScale;
			GUI.matrix = Matrix4x4.TRS(new Vector3(0f, 0f, 0f), Quaternion.identity, new Vector3(uiscale, uiscale2, 1f));
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0001418D File Offset: 0x0001238D
		public static void FocusControl(string controlName, Window window)
		{
			GUI.FocusControl(controlName);
			Find.WindowStack.Notify_ManuallySetFocus(window);
		}

		// Token: 0x06000396 RID: 918 RVA: 0x000141A0 File Offset: 0x000123A0
		public static void UnfocusCurrentControl()
		{
			GUI.FocusControl(null);
		}

		// Token: 0x06000397 RID: 919 RVA: 0x000141A8 File Offset: 0x000123A8
		public static void UnfocusCurrentTextField()
		{
			GUI.SetNextControlName("FOR_UNFOCUS");
			GUI.TextField(default(Rect), "");
			GUI.FocusControl("FOR_UNFOCUS");
		}

		// Token: 0x06000398 RID: 920 RVA: 0x000141DD File Offset: 0x000123DD
		public static Vector2 GUIToScreenPoint(Vector2 guiPoint)
		{
			return GUIUtility.GUIToScreenPoint(guiPoint / Prefs.UIScale);
		}

		// Token: 0x06000399 RID: 921 RVA: 0x000141F0 File Offset: 0x000123F0
		public static Rect GUIToScreenRect(Rect guiRect)
		{
			return new Rect
			{
				min = UI.GUIToScreenPoint(guiRect.min),
				max = UI.GUIToScreenPoint(guiRect.max)
			};
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0001422C File Offset: 0x0001242C
		public static void RotateAroundPivot(float angle, Vector2 center)
		{
			GUIUtility.RotateAroundPivot(angle, center * Prefs.UIScale);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00014240 File Offset: 0x00012440
		public static Vector2 MapToUIPosition(this Vector3 v)
		{
			Vector3 vector = Find.Camera.WorldToScreenPoint(v) / Prefs.UIScale;
			return new Vector2(vector.x, (float)UI.screenHeight - vector.y);
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0001427B File Offset: 0x0001247B
		public static Vector3 UIToMapPosition(float x, float y)
		{
			return UI.UIToMapPosition(new Vector2(x, y));
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0001428C File Offset: 0x0001248C
		public static Vector3 UIToMapPosition(Vector2 screenLoc)
		{
			Ray ray = Find.Camera.ScreenPointToRay(screenLoc * Prefs.UIScale);
			return new Vector3(ray.origin.x, 0f, ray.origin.z);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x000142D6 File Offset: 0x000124D6
		public static float CurUICellSize()
		{
			return (new Vector3(1f, 0f, 0f).MapToUIPosition() - new Vector3(0f, 0f, 0f).MapToUIPosition()).x;
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00014314 File Offset: 0x00012514
		public static Vector3 MouseMapPosition()
		{
			return UI.UIToMapPosition(UI.MousePositionOnUI);
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00014320 File Offset: 0x00012520
		public static IntVec3 MouseCell()
		{
			return UI.UIToMapPosition(UI.MousePositionOnUI).ToIntVec3();
		}

		// Token: 0x040000DC RID: 220
		public static int screenWidth;

		// Token: 0x040000DD RID: 221
		public static int screenHeight;
	}
}
