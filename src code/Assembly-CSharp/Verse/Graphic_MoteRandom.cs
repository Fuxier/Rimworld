using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003E3 RID: 995
	[StaticConstructorOnStartup]
	public class Graphic_MoteRandom : Graphic_Random
	{
		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06001C52 RID: 7250 RVA: 0x0000249D File Offset: 0x0000069D
		protected virtual bool ForcePropertyBlock
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001C53 RID: 7251 RVA: 0x000ACF9C File Offset: 0x000AB19C
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Graphic_Mote.DrawMote(this.data, this.SubGraphicFor((Mote)thing).MatSingle, base.Color, loc, rot, thingDef, thing, 0, this.ForcePropertyBlock, null);
		}

		// Token: 0x06001C54 RID: 7252 RVA: 0x000AB3E4 File Offset: 0x000A95E4
		public Graphic SubGraphicFor(Mote mote)
		{
			return this.subGraphics[mote.offsetRandom % this.subGraphics.Length];
		}

		// Token: 0x06001C55 RID: 7253 RVA: 0x000ACFDC File Offset: 0x000AB1DC
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

		// Token: 0x04001440 RID: 5184
		protected static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
	}
}
