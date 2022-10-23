using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003D1 RID: 977
	[StaticConstructorOnStartup]
	public class GraphicMote_RandomWithAgeSecs : Graphic_Random
	{
		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06001BFB RID: 7163 RVA: 0x00002662 File Offset: 0x00000862
		protected virtual bool ForcePropertyBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001BFC RID: 7164 RVA: 0x000AB35C File Offset: 0x000A955C
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Mote mote = (Mote)thing;
			GraphicMote_RandomWithAgeSecs.propertyBlock.SetColor(ShaderPropertyIDs.Color, this.color);
			GraphicMote_RandomWithAgeSecs.propertyBlock.SetFloat(ShaderPropertyIDs.AgeSecs, mote.AgeSecs);
			GraphicMote_RandomWithAgeSecs.propertyBlock.SetFloat(ShaderPropertyIDs.AgeSecsPausable, mote.AgeSecsPausable);
			Graphic_Mote.DrawMote(this.data, this.SubGraphicFor((Mote)thing).MatSingle, base.Color, loc, rot, thingDef, thing, 0, this.ForcePropertyBlock, GraphicMote_RandomWithAgeSecs.propertyBlock);
		}

		// Token: 0x06001BFD RID: 7165 RVA: 0x000AB3E4 File Offset: 0x000A95E4
		public Graphic SubGraphicFor(Mote mote)
		{
			return this.subGraphics[mote.offsetRandom % this.subGraphics.Length];
		}

		// Token: 0x06001BFE RID: 7166 RVA: 0x000AB3FC File Offset: 0x000A95FC
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Mote(path=",
				this.path,
				", shader=",
				base.Shader,
				", color=",
				this.color,
				", colorTwo=unsupported)"
			});
		}

		// Token: 0x04001420 RID: 5152
		protected static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
	}
}
