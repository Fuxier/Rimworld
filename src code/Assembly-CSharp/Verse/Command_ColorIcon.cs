using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000496 RID: 1174
	[StaticConstructorOnStartup]
	public class Command_ColorIcon : Command_Action
	{
		// Token: 0x0600237E RID: 9086 RVA: 0x000E334C File Offset: 0x000E154C
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth, parms);
			if (this.color != null)
			{
				Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
				RectDivider rectDivider = new RectDivider(rect.ContractedBy(4f), 1552930585, null);
				GUI.DrawTexture(rectDivider.NewCol(16f, HorizontalJustification.Right).NewRow(16f, VerticalJustification.Top), Command_ColorIcon.ColorCircleTex, ScaleMode.ScaleToFit, true, 1f, this.color.Value, 0f, 0f);
			}
			return result;
		}

		// Token: 0x040016D2 RID: 5842
		private static readonly Texture2D ColorCircleTex = ContentFinder<Texture2D>.Get("UI/Overlays/TargetHighlight_Square", true);

		// Token: 0x040016D3 RID: 5843
		public Color32? color;

		// Token: 0x040016D4 RID: 5844
		private const int colorCircleDiameter = 16;

		// Token: 0x040016D5 RID: 5845
		private const float colorCircleGap = 4f;
	}
}
