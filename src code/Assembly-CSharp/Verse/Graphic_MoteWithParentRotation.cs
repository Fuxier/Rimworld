using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003E5 RID: 997
	public class Graphic_MoteWithParentRotation : Graphic_Mote
	{
		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06001C5C RID: 7260 RVA: 0x00002662 File Offset: 0x00000862
		protected override bool ForcePropertyBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001C5D RID: 7261 RVA: 0x000AD130 File Offset: 0x000AB330
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			MoteAttached moteAttached = (MoteAttached)thing;
			Graphic_Mote.propertyBlock.SetColor(ShaderPropertyIDs.Color, this.color);
			if (moteAttached != null && moteAttached.link1.Linked)
			{
				Graphic_Mote.propertyBlock.SetInt(ShaderPropertyIDs.Rotation, moteAttached.link1.Target.Thing.Rotation.AsInt);
			}
			base.DrawMoteInternal(loc, rot, thingDef, thing, 0);
		}

		// Token: 0x06001C5E RID: 7262 RVA: 0x000AD1A8 File Offset: 0x000AB3A8
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Graphic_MoteWithParentRotation(path=",
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
