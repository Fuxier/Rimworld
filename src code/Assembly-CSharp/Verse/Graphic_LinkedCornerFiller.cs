using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003E0 RID: 992
	public class Graphic_LinkedCornerFiller : Graphic_Linked
	{
		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06001C42 RID: 7234 RVA: 0x000AC9DA File Offset: 0x000AABDA
		public override LinkDrawerType LinkerType
		{
			get
			{
				return LinkDrawerType.CornerFiller;
			}
		}

		// Token: 0x06001C43 RID: 7235 RVA: 0x000AC790 File Offset: 0x000AA990
		public Graphic_LinkedCornerFiller(Graphic subGraphic) : base(subGraphic)
		{
		}

		// Token: 0x06001C44 RID: 7236 RVA: 0x000AC9DD File Offset: 0x000AABDD
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return new Graphic_LinkedCornerFiller(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo))
			{
				data = this.data
			};
		}

		// Token: 0x06001C45 RID: 7237 RVA: 0x000ACA00 File Offset: 0x000AAC00
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			base.Print(layer, thing, extraRotation);
			IntVec3 position = thing.Position;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec = thing.Position + GenAdj.DiagonalDirectionsAround[i];
				if (this.ShouldLinkWith(intVec, thing) && (i != 0 || (this.ShouldLinkWith(position + IntVec3.West, thing) && this.ShouldLinkWith(position + IntVec3.South, thing))) && (i != 1 || (this.ShouldLinkWith(position + IntVec3.West, thing) && this.ShouldLinkWith(position + IntVec3.North, thing))) && (i != 2 || (this.ShouldLinkWith(position + IntVec3.East, thing) && this.ShouldLinkWith(position + IntVec3.North, thing))) && (i != 3 || (this.ShouldLinkWith(position + IntVec3.East, thing) && this.ShouldLinkWith(position + IntVec3.South, thing))))
				{
					Vector3 center = thing.DrawPos + GenAdj.DiagonalDirectionsAround[i].ToVector3().normalized * Graphic_LinkedCornerFiller.CoverOffsetDist + Altitudes.AltIncVect + new Vector3(0f, 0f, 0.09f);
					Vector2 size = new Vector2(0.5f, 0.5f);
					if (!intVec.InBounds(thing.Map))
					{
						if (intVec.x == -1)
						{
							center.x -= 1f;
							size.x *= 5f;
						}
						if (intVec.z == -1)
						{
							center.z -= 1f;
							size.y *= 5f;
						}
						if (intVec.x == thing.Map.Size.x)
						{
							center.x += 1f;
							size.x *= 5f;
						}
						if (intVec.z == thing.Map.Size.z)
						{
							center.z += 1f;
							size.y *= 5f;
						}
					}
					Printer_Plane.PrintPlane(layer, center, size, this.LinkedDrawMatFrom(thing, thing.Position), extraRotation, false, Graphic_LinkedCornerFiller.CornerFillUVs, null, 0.01f, 0f);
				}
			}
		}

		// Token: 0x04001439 RID: 5177
		private const float ShiftUp = 0.09f;

		// Token: 0x0400143A RID: 5178
		private const float CoverSize = 0.5f;

		// Token: 0x0400143B RID: 5179
		private static readonly float CoverSizeCornerCorner = new Vector2(0.5f, 0.5f).magnitude;

		// Token: 0x0400143C RID: 5180
		private static readonly float DistCenterCorner = new Vector2(0.5f, 0.5f).magnitude;

		// Token: 0x0400143D RID: 5181
		private static readonly float CoverOffsetDist = Graphic_LinkedCornerFiller.DistCenterCorner - Graphic_LinkedCornerFiller.CoverSizeCornerCorner * 0.5f;

		// Token: 0x0400143E RID: 5182
		private static readonly Vector2[] CornerFillUVs = new Vector2[]
		{
			new Vector2(0.5f, 0.6f),
			new Vector2(0.5f, 0.6f),
			new Vector2(0.5f, 0.6f),
			new Vector2(0.5f, 0.6f)
		};
	}
}
