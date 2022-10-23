using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003DA RID: 986
	public class Graphic_FleckSplash : Graphic_Fleck
	{
		// Token: 0x06001C23 RID: 7203 RVA: 0x000AC114 File Offset: 0x000AA314
		public override void DrawFleck(FleckDrawData drawData, DrawBatch batch)
		{
			drawData.propertyBlock = (drawData.propertyBlock ?? batch.GetPropertyBlock());
			drawData.propertyBlock.SetColor(ShaderPropertyIDs.ShockwaveColor, new Color(1f, 1f, 1f, drawData.alpha));
			drawData.propertyBlock.SetFloat(ShaderPropertyIDs.ShockwaveSpan, drawData.calculatedShockwaveSpan);
			drawData.drawLayer = SubcameraDefOf.WaterDepth.LayerId;
			base.DrawFleck(drawData, batch);
		}

		// Token: 0x06001C24 RID: 7204 RVA: 0x000AC194 File Offset: 0x000AA394
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"FleckSplash(path=",
				this.path,
				", shader=",
				base.Shader,
				", color=",
				this.color,
				", colorTwo=unsupported)"
			});
		}
	}
}
