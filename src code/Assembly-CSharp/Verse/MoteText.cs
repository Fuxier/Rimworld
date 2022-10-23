using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003F8 RID: 1016
	public class MoteText : MoteThrown
	{
		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06001CE9 RID: 7401 RVA: 0x000AF73D File Offset: 0x000AD93D
		protected float TimeBeforeStartFadeout
		{
			get
			{
				if (this.overrideTimeBeforeStartFadeout < 0f)
				{
					return base.SolidTime;
				}
				return this.overrideTimeBeforeStartFadeout;
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06001CEA RID: 7402 RVA: 0x000AF759 File Offset: 0x000AD959
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.TimeBeforeStartFadeout + this.def.mote.fadeOutTime;
			}
		}

		// Token: 0x06001CEB RID: 7403 RVA: 0x000034B7 File Offset: 0x000016B7
		public override void Draw()
		{
		}

		// Token: 0x06001CEC RID: 7404 RVA: 0x000AF780 File Offset: 0x000AD980
		public override void DrawGUIOverlay()
		{
			float a = 1f - (base.AgeSecs - this.TimeBeforeStartFadeout) / this.def.mote.fadeOutTime;
			Color color = new Color(this.textColor.r, this.textColor.g, this.textColor.b, a);
			GenMapUI.DrawText(new Vector2(this.exactPosition.x, this.exactPosition.z), this.text, color);
		}

		// Token: 0x04001480 RID: 5248
		public string text;

		// Token: 0x04001481 RID: 5249
		public Color textColor = Color.white;

		// Token: 0x04001482 RID: 5250
		public float overrideTimeBeforeStartFadeout = -1f;
	}
}
