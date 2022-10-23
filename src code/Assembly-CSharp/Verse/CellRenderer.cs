using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001C0 RID: 448
	public static class CellRenderer
	{
		// Token: 0x06000C7A RID: 3194 RVA: 0x00045DC8 File Offset: 0x00043FC8
		private static void InitFrame()
		{
			if (Time.frameCount != CellRenderer.lastCameraUpdateFrame)
			{
				CellRenderer.viewRect = Find.CameraDriver.CurrentViewRect;
				CellRenderer.lastCameraUpdateFrame = Time.frameCount;
			}
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x00045DEF File Offset: 0x00043FEF
		private static Material MatFromColorPct(float colorPct, bool transparent)
		{
			return DebugMatsSpectrum.Mat(GenMath.PositiveMod(Mathf.RoundToInt(colorPct * 100f), 100), transparent);
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x00045E0A File Offset: 0x0004400A
		public static void RenderCell(IntVec3 c, float colorPct = 0.5f)
		{
			CellRenderer.RenderCell(c, CellRenderer.MatFromColorPct(colorPct, true));
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x00045E1C File Offset: 0x0004401C
		public static void RenderCell(IntVec3 c, Material mat)
		{
			CellRenderer.InitFrame();
			if (!CellRenderer.viewRect.Contains(c))
			{
				return;
			}
			Vector3 position = c.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, mat, 0);
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x00045E58 File Offset: 0x00044058
		public static void RenderSpot(IntVec3 c, float colorPct = 0.5f, float scale = 0.15f)
		{
			CellRenderer.RenderSpot(c.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays), colorPct, scale);
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x00045E6A File Offset: 0x0004406A
		public static void RenderSpot(Vector3 loc, float colorPct = 0.5f, float scale = 0.15f)
		{
			CellRenderer.RenderSpot(loc, CellRenderer.MatFromColorPct(colorPct, false), scale);
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x00045E7C File Offset: 0x0004407C
		public static void RenderSpot(Vector3 loc, Material mat, float scale = 0.15f)
		{
			CellRenderer.InitFrame();
			if (!CellRenderer.viewRect.Contains(loc.ToIntVec3()))
			{
				return;
			}
			loc.y = AltitudeLayer.MetaOverlays.AltitudeFor();
			Vector3 s = new Vector3(scale, 1f, scale);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(loc, Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.circle, matrix, mat, 0);
		}

		// Token: 0x04000B74 RID: 2932
		private static int lastCameraUpdateFrame = -1;

		// Token: 0x04000B75 RID: 2933
		private static CellRect viewRect;
	}
}
