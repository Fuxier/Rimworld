using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003E4 RID: 996
	public class Graphic_MoteWithAgeSecs : Graphic_Mote
	{
		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06001C58 RID: 7256 RVA: 0x00002662 File Offset: 0x00000862
		protected override bool ForcePropertyBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001C59 RID: 7257 RVA: 0x000AD040 File Offset: 0x000AB240
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Mote mote = (Mote)thing;
			Graphic_Mote.propertyBlock.SetColor(ShaderPropertyIDs.Color, this.color);
			Graphic_Mote.propertyBlock.SetFloat(ShaderPropertyIDs.AgeSecs, mote.AgeSecs);
			Graphic_Mote.propertyBlock.SetFloat(ShaderPropertyIDs.AgeSecsPausable, mote.AgeSecsPausable);
			Graphic_Mote.propertyBlock.SetFloat(ShaderPropertyIDs.RandomPerObject, (float)Gen.HashCombineInt(mote.spawnTick, mote.DrawPos.GetHashCode()));
			base.DrawMoteInternal(loc, rot, thingDef, thing, 0);
		}

		// Token: 0x06001C5A RID: 7258 RVA: 0x000AD0D0 File Offset: 0x000AB2D0
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Graphic_MoteWithAgeSecs(path=",
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
