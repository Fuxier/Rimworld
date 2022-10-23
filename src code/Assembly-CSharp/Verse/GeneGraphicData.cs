using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200006F RID: 111
	public class GeneGraphicData
	{
		// Token: 0x06000478 RID: 1144 RVA: 0x00019A78 File Offset: 0x00017C78
		private string GraphicPathFor(Pawn pawn)
		{
			if (!this.graphicPaths.NullOrEmpty<string>())
			{
				return this.graphicPaths[pawn.thingIDNumber % this.graphicPaths.Count];
			}
			if (pawn.gender == Gender.Female && !this.graphicPathFemale.NullOrEmpty())
			{
				return this.graphicPathFemale;
			}
			return this.graphicPath;
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00019AD4 File Offset: 0x00017CD4
		private Color GetColorFor(Pawn pawn)
		{
			Color a;
			switch (this.colorType)
			{
			case GeneColorType.Hair:
				a = pawn.story.HairColor;
				goto IL_56;
			case GeneColorType.Skin:
				a = pawn.story.SkinColor;
				goto IL_56;
			}
			a = (this.color ?? Color.white);
			IL_56:
			return a * this.colorRGBPostFactor;
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00019B48 File Offset: 0x00017D48
		public ValueTuple<Graphic, Graphic> GetGraphics(Pawn pawn, Shader skinShader, Color rottingColor)
		{
			Shader shader = this.useSkinShader ? skinShader : ShaderDatabase.Transparent;
			string path = this.GraphicPathFor(pawn);
			Color colorFor = this.GetColorFor(pawn);
			Graphic item = GraphicDatabase.Get<Graphic_Multi>(path, shader, Vector2.one, colorFor, Color.white);
			Graphic item2 = GraphicDatabase.Get<Graphic_Multi>(path, shader, Vector2.one, this.color ?? rottingColor, Color.white);
			return new ValueTuple<Graphic, Graphic>(item, item2);
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00019BBC File Offset: 0x00017DBC
		public Vector3 DrawOffsetAt(Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
			{
				Vector3? vector = this.drawOffsetNorth;
				if (vector == null)
				{
					return this.drawOffset;
				}
				return vector.GetValueOrDefault();
			}
			case 1:
			{
				Vector3? vector = this.drawOffsetEast;
				if (vector == null)
				{
					return this.drawOffset;
				}
				return vector.GetValueOrDefault();
			}
			case 2:
			{
				Vector3? vector = this.drawOffsetSouth;
				if (vector == null)
				{
					return this.drawOffset;
				}
				return vector.GetValueOrDefault();
			}
			case 3:
			{
				Vector3 result = this.drawOffsetEast ?? this.drawOffset;
				result.x *= -1f;
				return result;
			}
			default:
				return Vector3.zero;
			}
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00019C80 File Offset: 0x00017E80
		public IEnumerable<string> ConfigErrors()
		{
			if (!this.graphicPaths.NullOrEmpty<string>())
			{
				if (!this.graphicPath.NullOrEmpty())
				{
					yield return "defines both graphicPaths and graphicPath.";
				}
				if (!this.graphicPathFemale.NullOrEmpty())
				{
					yield return "defines both graphicPaths and graphicPathFemale.";
				}
			}
			yield break;
		}

		// Token: 0x040001F2 RID: 498
		[NoTranslate]
		public string graphicPath;

		// Token: 0x040001F3 RID: 499
		[NoTranslate]
		public string graphicPathFemale;

		// Token: 0x040001F4 RID: 500
		[NoTranslate]
		public List<string> graphicPaths;

		// Token: 0x040001F5 RID: 501
		public GeneColorType colorType;

		// Token: 0x040001F6 RID: 502
		public Color? color;

		// Token: 0x040001F7 RID: 503
		public float colorRGBPostFactor = 1f;

		// Token: 0x040001F8 RID: 504
		public float drawScale = 1f;

		// Token: 0x040001F9 RID: 505
		public bool drawWhileDessicated;

		// Token: 0x040001FA RID: 506
		public bool visibleNorth = true;

		// Token: 0x040001FB RID: 507
		public bool useSkinShader;

		// Token: 0x040001FC RID: 508
		public bool drawIfFaceCovered;

		// Token: 0x040001FD RID: 509
		public bool skinIsHairColor;

		// Token: 0x040001FE RID: 510
		public bool tattoosVisible = true;

		// Token: 0x040001FF RID: 511
		public FurDef fur;

		// Token: 0x04000200 RID: 512
		public GeneDrawLoc drawLoc = GeneDrawLoc.HeadMiddle;

		// Token: 0x04000201 RID: 513
		public GeneDrawLayer layer;

		// Token: 0x04000202 RID: 514
		public bool drawNorthAfterHair;

		// Token: 0x04000203 RID: 515
		public bool drawOnEyes;

		// Token: 0x04000204 RID: 516
		private Vector3 drawOffset = Vector3.zero;

		// Token: 0x04000205 RID: 517
		private Vector3? drawOffsetNorth;

		// Token: 0x04000206 RID: 518
		private Vector3? drawOffsetSouth;

		// Token: 0x04000207 RID: 519
		private Vector3? drawOffsetEast;

		// Token: 0x04000208 RID: 520
		public float narrowCrownHorizontalOffset;
	}
}
