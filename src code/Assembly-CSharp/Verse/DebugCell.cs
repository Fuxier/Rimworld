using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001BB RID: 443
	internal sealed class DebugCell
	{
		// Token: 0x06000C6D RID: 3181 RVA: 0x00045A84 File Offset: 0x00043C84
		public void Draw()
		{
			if (this.customMat != null)
			{
				CellRenderer.RenderCell(this.c, this.customMat);
				return;
			}
			CellRenderer.RenderCell(this.c, this.colorPct);
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x00045AB8 File Offset: 0x00043CB8
		public void OnGUI()
		{
			if (this.displayString != null)
			{
				Vector2 vector = this.c.ToUIPosition();
				Rect rect = new Rect(vector.x - 20f, vector.y - 20f, 40f, 40f);
				if (new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight).Overlaps(rect))
				{
					Widgets.Label(rect, this.displayString);
				}
			}
		}

		// Token: 0x04000B63 RID: 2915
		public IntVec3 c;

		// Token: 0x04000B64 RID: 2916
		public string displayString;

		// Token: 0x04000B65 RID: 2917
		public float colorPct;

		// Token: 0x04000B66 RID: 2918
		public int ticksLeft;

		// Token: 0x04000B67 RID: 2919
		public Material customMat;
	}
}
