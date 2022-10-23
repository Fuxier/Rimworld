using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003DD RID: 989
	public class Graphic_Genepack : Graphic_Collection
	{
		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06001C2C RID: 7212 RVA: 0x000AC500 File Offset: 0x000AA700
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[0].MatSingle;
			}
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x000AC50F File Offset: 0x000AA70F
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_Genepack>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data, null);
		}

		// Token: 0x06001C2E RID: 7214 RVA: 0x000AC52C File Offset: 0x000AA72C
		public Graphic SubGraphicFor(Thing thing)
		{
			Genepack genepack;
			if (thing == null || (genepack = (thing as Genepack)) == null || genepack.GeneSet == null)
			{
				return this.subGraphics[0];
			}
			using (List<GeneDef>.Enumerator enumerator = genepack.GeneSet.GenesListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.biostatArc > 0)
					{
						return this.subGraphics[this.subGraphics.Length - 1];
					}
				}
			}
			return this.SubGraphicForGeneCount(genepack.GeneSet.GenesListForReading.Count);
		}

		// Token: 0x06001C2F RID: 7215 RVA: 0x000AC5D0 File Offset: 0x000AA7D0
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.MatSingleFor(thing);
		}

		// Token: 0x06001C30 RID: 7216 RVA: 0x000AC5E3 File Offset: 0x000AA7E3
		public override Material MatSingleFor(Thing thing)
		{
			return this.SubGraphicFor(thing).MatSingle;
		}

		// Token: 0x06001C31 RID: 7217 RVA: 0x000AC5F1 File Offset: 0x000AA7F1
		public Graphic SubGraphicForGeneCount(int geneCount)
		{
			geneCount = Mathf.Min(geneCount, 4);
			return this.subGraphics[Mathf.Min(geneCount, this.subGraphics.Length - 1)];
		}

		// Token: 0x04001437 RID: 5175
		private const int MaxDisplayedGenes = 4;
	}
}
