using System;

namespace Verse
{
	// Token: 0x020003D9 RID: 985
	public class Graphic_FleckPulse : Graphic_Fleck
	{
		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06001C1F RID: 7199 RVA: 0x0000249D File Offset: 0x0000069D
		protected override bool AllowInstancing
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001C20 RID: 7200 RVA: 0x000AC05C File Offset: 0x000AA25C
		public override void DrawFleck(FleckDrawData drawData, DrawBatch batch)
		{
			drawData.propertyBlock = (drawData.propertyBlock ?? batch.GetPropertyBlock());
			drawData.propertyBlock.SetFloat(ShaderPropertyIDs.AgeSecs, drawData.ageSecs);
			drawData.propertyBlock.SetFloat(ShaderPropertyIDs.RandomPerObject, drawData.id);
			base.DrawFleck(drawData, batch);
		}

		// Token: 0x06001C21 RID: 7201 RVA: 0x000AC0B4 File Offset: 0x000AA2B4
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Graphic_FleckPulse(path=",
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
