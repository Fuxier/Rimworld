using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003DB RID: 987
	public class Graphic_Flicker : Graphic_Collection
	{
		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06001C26 RID: 7206 RVA: 0x000AB851 File Offset: 0x000A9A51
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[Rand.Range(0, this.subGraphics.Length)].MatSingle;
			}
		}

		// Token: 0x06001C27 RID: 7207 RVA: 0x000AC1EC File Offset: 0x000AA3EC
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			if (thingDef == null)
			{
				Log.ErrorOnce("Fire DrawWorker with null thingDef: " + loc, 3427324);
				return;
			}
			if (this.subGraphics == null)
			{
				Log.ErrorOnce("Graphic_Flicker has no subgraphics " + thingDef, 358773632);
				return;
			}
			int num = Find.TickManager.TicksGame;
			if (thing != null)
			{
				num += Mathf.Abs(thing.thingIDNumber ^ 8453458);
			}
			int num2 = num / 15;
			int num3 = Mathf.Abs(num2 ^ ((thing != null) ? thing.thingIDNumber : 0) * 391) % this.subGraphics.Length;
			float num4 = 1f;
			CompFireOverlayBase compFireOverlayBase = null;
			Fire fire = thing as Fire;
			if (fire != null)
			{
				num4 = fire.fireSize;
			}
			else if (thing != null)
			{
				compFireOverlayBase = thing.TryGetComp<CompFireOverlayBase>();
				if (compFireOverlayBase != null)
				{
					num4 = compFireOverlayBase.FireSize;
				}
				else
				{
					compFireOverlayBase = thing.TryGetComp<CompDarklightOverlay>();
					if (compFireOverlayBase != null)
					{
						num4 = compFireOverlayBase.FireSize;
					}
				}
			}
			if (num3 < 0 || num3 >= this.subGraphics.Length)
			{
				Log.ErrorOnce("Fire drawing out of range: " + num3, 7453435);
				num3 = 0;
			}
			Graphic graphic = this.subGraphics[num3];
			float num5 = (compFireOverlayBase == null) ? Mathf.Min(num4 / 1.2f, 1.2f) : num4;
			Vector3 a = GenRadial.RadialPattern[num2 % GenRadial.RadialPattern.Length].ToVector3() / GenRadial.MaxRadialPatternRadius;
			a *= 0.05f;
			Vector3 vector = loc + a * num4;
			if (compFireOverlayBase != null)
			{
				vector += compFireOverlayBase.Props.offset;
			}
			Vector3 s = new Vector3(num5, 1f, num5);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(vector, Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, graphic.MatSingle, 0);
		}

		// Token: 0x06001C28 RID: 7208 RVA: 0x000AC3BC File Offset: 0x000AA5BC
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Flicker(subGraphic[0]=",
				this.subGraphics[0].ToString(),
				", count=",
				this.subGraphics.Length,
				")"
			});
		}

		// Token: 0x04001432 RID: 5170
		private const int BaseTicksPerFrameChange = 15;

		// Token: 0x04001433 RID: 5171
		private const int ExtraTicksPerFrameChange = 10;

		// Token: 0x04001434 RID: 5172
		private const float MaxOffset = 0.05f;
	}
}
