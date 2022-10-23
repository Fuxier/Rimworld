using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003D4 RID: 980
	public class Graphic_Cluster : Graphic_Collection
	{
		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06001C10 RID: 7184 RVA: 0x000AB851 File Offset: 0x000A9A51
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[Rand.Range(0, this.subGraphics.Length)].MatSingle;
			}
		}

		// Token: 0x06001C11 RID: 7185 RVA: 0x000AB86D File Offset: 0x000A9A6D
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Log.ErrorOnce("Graphic_Scatter cannot draw realtime.", 9432243);
		}

		// Token: 0x06001C12 RID: 7186 RVA: 0x000AB880 File Offset: 0x000A9A80
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			Vector3 a = thing.TrueCenter();
			Rand.PushState();
			Rand.Seed = thing.Position.GetHashCode();
			Filth filth = thing as Filth;
			int num;
			if (filth == null)
			{
				num = 3;
			}
			else
			{
				num = filth.thickness;
			}
			for (int i = 0; i < num; i++)
			{
				Material matSingle = this.MatSingle;
				Vector3 center = a + new Vector3(Rand.Range(-0.45f, 0.45f), 0f, Rand.Range(-0.45f, 0.45f));
				Vector2 size = new Vector2(Rand.Range(this.data.drawSize.x * 0.8f, this.data.drawSize.x * 1.2f), Rand.Range(this.data.drawSize.y * 0.8f, this.data.drawSize.y * 1.2f));
				float rot = (float)Rand.RangeInclusive(0, 360) + extraRotation;
				bool flipUv = Rand.Value < 0.5f;
				Vector2[] uvs;
				Color32 color;
				Graphic.TryGetTextureAtlasReplacementInfo(matSingle, thing.def.category.ToAtlasGroup(), flipUv, true, out matSingle, out uvs, out color);
				Printer_Plane.PrintPlane(layer, center, size, matSingle, rot, flipUv, uvs, new Color32[]
				{
					color,
					color,
					color,
					color
				}, 0.01f, 0f);
			}
			Rand.PopState();
		}

		// Token: 0x06001C13 RID: 7187 RVA: 0x000ABA08 File Offset: 0x000A9C08
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Scatter(subGraphic[0]=",
				this.subGraphics[0].ToString(),
				", count=",
				this.subGraphics.Length,
				")"
			});
		}

		// Token: 0x04001422 RID: 5154
		private const float PositionVariance = 0.45f;

		// Token: 0x04001423 RID: 5155
		private const float SizeVariance = 0.2f;

		// Token: 0x04001424 RID: 5156
		private const float SizeFactorMin = 0.8f;

		// Token: 0x04001425 RID: 5157
		private const float SizeFactorMax = 1.2f;
	}
}
