using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000CB RID: 203
	public class GraphicData
	{
		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x000217B3 File Offset: 0x0001F9B3
		public bool Linked
		{
			get
			{
				return this.linkType > LinkDrawerType.None;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600061D RID: 1565 RVA: 0x000217BE File Offset: 0x0001F9BE
		public Graphic Graphic
		{
			get
			{
				if (this.cachedGraphic == null)
				{
					this.Init();
				}
				return this.cachedGraphic;
			}
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x000217D4 File Offset: 0x0001F9D4
		public void ExplicitlyInitCachedGraphic()
		{
			this.cachedGraphic = this.Graphic;
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x000217E4 File Offset: 0x0001F9E4
		public void CopyFrom(GraphicData other)
		{
			this.texPath = other.texPath;
			this.maskPath = other.maskPath;
			this.graphicClass = other.graphicClass;
			this.shaderType = other.shaderType;
			this.color = other.color;
			this.colorTwo = other.colorTwo;
			this.drawSize = other.drawSize;
			this.drawOffset = other.drawOffset;
			this.drawOffsetNorth = other.drawOffsetNorth;
			this.drawOffsetEast = other.drawOffsetEast;
			this.drawOffsetSouth = other.drawOffsetSouth;
			this.drawOffsetWest = other.drawOffsetSouth;
			this.onGroundRandomRotateAngle = other.onGroundRandomRotateAngle;
			this.drawRotated = other.drawRotated;
			this.allowFlip = other.allowFlip;
			this.flipExtraRotation = other.flipExtraRotation;
			this.shadowData = other.shadowData;
			this.damageData = other.damageData;
			this.linkType = other.linkType;
			this.linkFlags = other.linkFlags;
			this.asymmetricLink = other.asymmetricLink;
			this.allowAtlasing = other.allowAtlasing;
			this.renderInstanced = other.renderInstanced;
			this.renderQueue = other.renderQueue;
			this.cachedGraphic = null;
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x00021918 File Offset: 0x0001FB18
		private void Init()
		{
			if (this.graphicClass == null)
			{
				this.cachedGraphic = null;
				return;
			}
			ShaderTypeDef cutout = this.shaderType;
			if (cutout == null)
			{
				cutout = ShaderTypeDefOf.Cutout;
			}
			Shader shader = cutout.Shader;
			this.cachedGraphic = GraphicDatabase.Get(this.graphicClass, this.texPath, shader, this.drawSize, this.color, this.colorTwo, this, this.shaderParameters, this.maskPath);
			if (this.onGroundRandomRotateAngle > 0.01f)
			{
				this.cachedGraphic = new Graphic_RandomRotated(this.cachedGraphic, this.onGroundRandomRotateAngle);
			}
			if (this.Linked)
			{
				this.cachedGraphic = GraphicUtility.WrapLinked(this.cachedGraphic, this.linkType);
			}
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x000219CC File Offset: 0x0001FBCC
		public void ResolveReferencesSpecial()
		{
			if (this.damageData != null)
			{
				this.damageData.ResolveReferencesSpecial();
			}
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x000219E4 File Offset: 0x0001FBE4
		public Vector3 DrawOffsetForRot(Rot4 rot)
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
				Vector3? vector = this.drawOffsetWest;
				if (vector == null)
				{
					return this.drawOffset;
				}
				return vector.GetValueOrDefault();
			}
			default:
				return this.drawOffset;
			}
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00021A94 File Offset: 0x0001FC94
		public Graphic GraphicColoredFor(Thing t)
		{
			if (t.DrawColor.IndistinguishableFrom(this.Graphic.Color) && t.DrawColorTwo.IndistinguishableFrom(this.Graphic.ColorTwo))
			{
				return this.Graphic;
			}
			return this.Graphic.GetColoredVersion(this.Graphic.Shader, t.DrawColor, t.DrawColorTwo);
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00021AFA File Offset: 0x0001FCFA
		internal IEnumerable<string> ConfigErrors(ThingDef thingDef)
		{
			if (this.graphicClass == null)
			{
				yield return "graphicClass is null";
			}
			if (this.texPath.NullOrEmpty())
			{
				yield return "texPath is null or empty";
			}
			if (thingDef != null)
			{
				if (thingDef.drawerType == DrawerType.RealtimeOnly && this.Linked)
				{
					yield return "does not add to map mesh but has a link drawer. Link drawers can only work on the map mesh.";
				}
				if (!thingDef.rotatable && (this.drawOffsetNorth != null || this.drawOffsetEast != null || this.drawOffsetSouth != null || this.drawOffsetWest != null))
				{
					yield return "not rotatable but has rotational draw offset(s).";
				}
			}
			if ((this.shaderType == ShaderTypeDefOf.Cutout || this.shaderType == ShaderTypeDefOf.CutoutComplex) && thingDef.mote != null && (thingDef.mote.fadeInTime > 0f || thingDef.mote.fadeOutTime > 0f))
			{
				yield return "mote fades but uses cutout shader type. It will abruptly disappear when opacity falls under the cutout threshold.";
			}
			if (this.linkType == LinkDrawerType.Asymmetric != (this.asymmetricLink != null))
			{
				yield return "linkType=Asymmetric requires <asymmetricLink> and vice versa";
			}
			yield break;
		}

		// Token: 0x040003C5 RID: 965
		[NoTranslate]
		public string texPath;

		// Token: 0x040003C6 RID: 966
		[NoTranslate]
		public string maskPath;

		// Token: 0x040003C7 RID: 967
		public Type graphicClass;

		// Token: 0x040003C8 RID: 968
		public ShaderTypeDef shaderType;

		// Token: 0x040003C9 RID: 969
		public List<ShaderParameter> shaderParameters;

		// Token: 0x040003CA RID: 970
		public Color color = Color.white;

		// Token: 0x040003CB RID: 971
		public Color colorTwo = Color.white;

		// Token: 0x040003CC RID: 972
		public Vector2 drawSize = Vector2.one;

		// Token: 0x040003CD RID: 973
		public Vector3 drawOffset = Vector3.zero;

		// Token: 0x040003CE RID: 974
		public Vector3? drawOffsetNorth;

		// Token: 0x040003CF RID: 975
		public Vector3? drawOffsetEast;

		// Token: 0x040003D0 RID: 976
		public Vector3? drawOffsetSouth;

		// Token: 0x040003D1 RID: 977
		public Vector3? drawOffsetWest;

		// Token: 0x040003D2 RID: 978
		public float onGroundRandomRotateAngle;

		// Token: 0x040003D3 RID: 979
		public bool drawRotated = true;

		// Token: 0x040003D4 RID: 980
		public bool allowFlip = true;

		// Token: 0x040003D5 RID: 981
		public float flipExtraRotation;

		// Token: 0x040003D6 RID: 982
		public bool renderInstanced;

		// Token: 0x040003D7 RID: 983
		public bool allowAtlasing = true;

		// Token: 0x040003D8 RID: 984
		public int renderQueue;

		// Token: 0x040003D9 RID: 985
		public float overlayOpacity;

		// Token: 0x040003DA RID: 986
		public ShadowData shadowData;

		// Token: 0x040003DB RID: 987
		public DamageGraphicData damageData;

		// Token: 0x040003DC RID: 988
		public LinkDrawerType linkType;

		// Token: 0x040003DD RID: 989
		public LinkFlags linkFlags;

		// Token: 0x040003DE RID: 990
		public AsymmetricLinkData asymmetricLink;

		// Token: 0x040003DF RID: 991
		[Unsaved(false)]
		private Graphic cachedGraphic;
	}
}
