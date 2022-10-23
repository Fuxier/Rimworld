using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003E8 RID: 1000
	public class Graphic_Random : Graphic_Collection
	{
		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06001C76 RID: 7286 RVA: 0x000AB851 File Offset: 0x000A9A51
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[Rand.Range(0, this.subGraphics.Length)].MatSingle;
			}
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06001C77 RID: 7287 RVA: 0x000ADC08 File Offset: 0x000ABE08
		public int SubGraphicsCount
		{
			get
			{
				return this.subGraphics.Length;
			}
		}

		// Token: 0x06001C78 RID: 7288 RVA: 0x000ADC12 File Offset: 0x000ABE12
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			if (newColorTwo != Color.white)
			{
				Log.ErrorOnce("Cannot use Graphic_Random.GetColoredVersion with a non-white colorTwo.", 9910251);
			}
			return GraphicDatabase.Get<Graphic_Random>(this.path, newShader, this.drawSize, newColor, Color.white, this.data, null);
		}

		// Token: 0x06001C79 RID: 7289 RVA: 0x000AC5D0 File Offset: 0x000AA7D0
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.MatSingleFor(thing);
		}

		// Token: 0x06001C7A RID: 7290 RVA: 0x000ADC4F File Offset: 0x000ABE4F
		public override Material MatSingleFor(Thing thing)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.SubGraphicFor(thing).MatSingle;
		}

		// Token: 0x06001C7B RID: 7291 RVA: 0x000ADC68 File Offset: 0x000ABE68
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Graphic graphic;
			if (thing != null)
			{
				graphic = this.SubGraphicFor(thing);
			}
			else
			{
				graphic = this.subGraphics[0];
			}
			graphic.DrawWorker(loc, rot, thingDef, thing, extraRotation);
			if (base.ShadowGraphic != null)
			{
				base.ShadowGraphic.DrawWorker(loc, rot, thingDef, thing, extraRotation);
			}
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x000ADCB4 File Offset: 0x000ABEB4
		public Graphic SubGraphicFor(Thing thing)
		{
			if (thing == null)
			{
				return this.subGraphics[0];
			}
			int num = thing.overrideGraphicIndex ?? thing.thingIDNumber;
			return this.subGraphics[num % this.subGraphics.Length];
		}

		// Token: 0x06001C7D RID: 7293 RVA: 0x000ADCFE File Offset: 0x000ABEFE
		public Graphic SubGraphicAtIndex(int index)
		{
			return this.subGraphics[index % this.subGraphics.Length];
		}

		// Token: 0x06001C7E RID: 7294 RVA: 0x000ADD11 File Offset: 0x000ABF11
		public Graphic FirstSubgraphic()
		{
			return this.subGraphics[0];
		}

		// Token: 0x06001C7F RID: 7295 RVA: 0x000ADD1C File Offset: 0x000ABF1C
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			Graphic graphic;
			if (thing != null)
			{
				graphic = this.SubGraphicFor(thing);
			}
			else
			{
				graphic = this.subGraphics[0];
			}
			graphic.Print(layer, thing, extraRotation);
			if (base.ShadowGraphic != null && thing != null)
			{
				base.ShadowGraphic.Print(layer, thing, extraRotation);
			}
		}

		// Token: 0x06001C80 RID: 7296 RVA: 0x000ADD61 File Offset: 0x000ABF61
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Random(path=",
				this.path,
				", count=",
				this.subGraphics.Length,
				")"
			});
		}
	}
}
