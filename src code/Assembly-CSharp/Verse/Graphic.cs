using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003CD RID: 973
	public class Graphic
	{
		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06001BC1 RID: 7105 RVA: 0x000AA5E8 File Offset: 0x000A87E8
		public Shader Shader
		{
			get
			{
				Material matSingle = this.MatSingle;
				if (matSingle != null)
				{
					return matSingle.shader;
				}
				return ShaderDatabase.Cutout;
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06001BC2 RID: 7106 RVA: 0x000AA611 File Offset: 0x000A8811
		public Graphic_Shadow ShadowGraphic
		{
			get
			{
				if (this.cachedShadowGraphicInt == null && this.data != null && this.data.shadowData != null)
				{
					this.cachedShadowGraphicInt = new Graphic_Shadow(this.data.shadowData);
				}
				return this.cachedShadowGraphicInt;
			}
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06001BC3 RID: 7107 RVA: 0x000AA64C File Offset: 0x000A884C
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06001BC4 RID: 7108 RVA: 0x000AA654 File Offset: 0x000A8854
		public Color ColorTwo
		{
			get
			{
				return this.colorTwo;
			}
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06001BC5 RID: 7109 RVA: 0x000AA65C File Offset: 0x000A885C
		public virtual Material MatSingle
		{
			get
			{
				return BaseContent.BadMat;
			}
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06001BC6 RID: 7110 RVA: 0x000AA663 File Offset: 0x000A8863
		public virtual Material MatWest
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06001BC7 RID: 7111 RVA: 0x000AA663 File Offset: 0x000A8863
		public virtual Material MatSouth
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06001BC8 RID: 7112 RVA: 0x000AA663 File Offset: 0x000A8863
		public virtual Material MatEast
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06001BC9 RID: 7113 RVA: 0x000AA663 File Offset: 0x000A8863
		public virtual Material MatNorth
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06001BCA RID: 7114 RVA: 0x000AA66B File Offset: 0x000A886B
		public virtual bool WestFlipped
		{
			get
			{
				return this.DataAllowsFlip && !this.ShouldDrawRotated;
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06001BCB RID: 7115 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual bool EastFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06001BCC RID: 7116 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual bool ShouldDrawRotated
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06001BCD RID: 7117 RVA: 0x00004E2A File Offset: 0x0000302A
		public virtual float DrawRotatedExtraAngleOffset
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06001BCE RID: 7118 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual bool UseSameGraphicForGhost
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06001BCF RID: 7119 RVA: 0x000AA680 File Offset: 0x000A8880
		protected bool DataAllowsFlip
		{
			get
			{
				return this.data == null || this.data.allowFlip;
			}
		}

		// Token: 0x06001BD0 RID: 7120 RVA: 0x000AA698 File Offset: 0x000A8898
		public static bool TryGetTextureAtlasReplacementInfo(Material mat, TextureAtlasGroup group, bool flipUv, bool vertexColors, out Material material, out Vector2[] uvs, out Color32 vertexColor)
		{
			material = mat;
			uvs = null;
			vertexColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			Graphic.AtlasReplacementInfoCacheKey key = new Graphic.AtlasReplacementInfoCacheKey(mat, group, flipUv, vertexColors);
			Graphic.CachedAtlasReplacementInfo cachedAtlasReplacementInfo;
			if (Graphic.replacementInfoCache.TryGetValue(key, out cachedAtlasReplacementInfo))
			{
				material = cachedAtlasReplacementInfo.material;
				uvs = cachedAtlasReplacementInfo.uvs;
				if (vertexColors)
				{
					vertexColor = cachedAtlasReplacementInfo.vertexColor;
				}
				return true;
			}
			StaticTextureAtlasTile staticTextureAtlasTile;
			if (!GlobalTextureAtlasManager.TryGetStaticTile(group, (Texture2D)mat.mainTexture, out staticTextureAtlasTile, false))
			{
				return false;
			}
			MaterialRequest materialRequest;
			if (!MaterialPool.TryGetRequestForMat(mat, out materialRequest))
			{
				Log.Error("Tried getting texture atlas replacement info for a material that was not created by MaterialPool!");
				return false;
			}
			uvs = new Vector2[4];
			Printer_Plane.GetUVs(staticTextureAtlasTile.uvRect, out uvs[0], out uvs[1], out uvs[2], out uvs[3], flipUv);
			materialRequest.mainTex = staticTextureAtlasTile.atlas.ColorTexture;
			if (vertexColors)
			{
				vertexColor = materialRequest.color;
				materialRequest.color = Color.white;
			}
			if (materialRequest.maskTex != null)
			{
				materialRequest.maskTex = staticTextureAtlasTile.atlas.MaskTexture;
			}
			material = MaterialPool.MatFrom(materialRequest);
			Graphic.replacementInfoCache.Add(key, new Graphic.CachedAtlasReplacementInfo
			{
				material = material,
				uvs = uvs,
				vertexColor = vertexColor
			});
			return true;
		}

		// Token: 0x06001BD1 RID: 7121 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void TryInsertIntoAtlas(TextureAtlasGroup groupKey)
		{
		}

		// Token: 0x06001BD2 RID: 7122 RVA: 0x000AA809 File Offset: 0x000A8A09
		public virtual void Init(GraphicRequest req)
		{
			Log.ErrorOnce("Cannot init Graphic of class " + base.GetType().ToString(), 658928);
		}

		// Token: 0x06001BD3 RID: 7123 RVA: 0x000AA82C File Offset: 0x000A8A2C
		public virtual Material MatAt(Rot4 rot, Thing thing = null)
		{
			switch (rot.AsInt)
			{
			case 0:
				return this.MatNorth;
			case 1:
				return this.MatEast;
			case 2:
				return this.MatSouth;
			case 3:
				return this.MatWest;
			default:
				return BaseContent.BadMat;
			}
		}

		// Token: 0x06001BD4 RID: 7124 RVA: 0x000AA87C File Offset: 0x000A8A7C
		public virtual Mesh MeshAt(Rot4 rot)
		{
			Vector2 vector = this.drawSize;
			if (rot.IsHorizontal && !this.ShouldDrawRotated)
			{
				vector = vector.Rotated();
			}
			if ((rot == Rot4.West && this.WestFlipped) || (rot == Rot4.East && this.EastFlipped))
			{
				return MeshPool.GridPlaneFlip(vector);
			}
			return MeshPool.GridPlane(vector);
		}

		// Token: 0x06001BD5 RID: 7125 RVA: 0x000AA663 File Offset: 0x000A8863
		public virtual Material MatSingleFor(Thing thing)
		{
			return this.MatSingle;
		}

		// Token: 0x06001BD6 RID: 7126 RVA: 0x000AA8DF File Offset: 0x000A8ADF
		public Vector3 DrawOffset(Rot4 rot)
		{
			if (this.data == null)
			{
				return Vector3.zero;
			}
			return this.data.DrawOffsetForRot(rot);
		}

		// Token: 0x06001BD7 RID: 7127 RVA: 0x000AA8FB File Offset: 0x000A8AFB
		public void Draw(Vector3 loc, Rot4 rot, Thing thing, float extraRotation = 0f)
		{
			this.DrawWorker(loc, rot, thing.def, thing, extraRotation);
		}

		// Token: 0x06001BD8 RID: 7128 RVA: 0x000AA90E File Offset: 0x000A8B0E
		public void DrawFromDef(Vector3 loc, Rot4 rot, ThingDef thingDef, float extraRotation = 0f)
		{
			this.DrawWorker(loc, rot, thingDef, null, extraRotation);
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x000AA91C File Offset: 0x000A8B1C
		public virtual void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Mesh mesh = this.MeshAt(rot);
			Quaternion quaternion = this.QuatFromRot(rot);
			if (extraRotation != 0f)
			{
				quaternion *= Quaternion.Euler(Vector3.up * extraRotation);
			}
			loc += this.DrawOffset(rot);
			Material mat = this.MatAt(rot, thing);
			this.DrawMeshInt(mesh, loc, quaternion, mat);
			if (this.ShadowGraphic != null)
			{
				this.ShadowGraphic.DrawWorker(loc, rot, thingDef, thing, extraRotation);
			}
		}

		// Token: 0x06001BDA RID: 7130 RVA: 0x000AA997 File Offset: 0x000A8B97
		protected virtual void DrawMeshInt(Mesh mesh, Vector3 loc, Quaternion quat, Material mat)
		{
			Graphics.DrawMesh(mesh, loc, quat, mat, 0);
		}

		// Token: 0x06001BDB RID: 7131 RVA: 0x000AA9A4 File Offset: 0x000A8BA4
		public virtual void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			Vector2 vector;
			bool flag;
			if (this.ShouldDrawRotated)
			{
				vector = this.drawSize;
				flag = false;
			}
			else
			{
				if (!thing.Rotation.IsHorizontal)
				{
					vector = this.drawSize;
				}
				else
				{
					vector = this.drawSize.Rotated();
				}
				flag = ((thing.Rotation == Rot4.West && this.WestFlipped) || (thing.Rotation == Rot4.East && this.EastFlipped));
			}
			if (thing.MultipleItemsPerCellDrawn())
			{
				vector *= 0.8f;
			}
			float num = this.AngleFromRot(thing.Rotation) + extraRotation;
			if (flag && this.data != null)
			{
				num += this.data.flipExtraRotation;
			}
			Vector3 center = thing.TrueCenter() + this.DrawOffset(thing.Rotation);
			Material mat = this.MatAt(thing.Rotation, thing);
			Vector2[] uvs;
			Color32 color;
			Graphic.TryGetTextureAtlasReplacementInfo(mat, thing.def.category.ToAtlasGroup(), flag, true, out mat, out uvs, out color);
			Printer_Plane.PrintPlane(layer, center, vector, mat, num, flag, uvs, new Color32[]
			{
				color,
				color,
				color,
				color
			}, 0.01f, 0f);
			if (this.ShadowGraphic != null && thing != null)
			{
				this.ShadowGraphic.Print(layer, thing, 0f);
			}
		}

		// Token: 0x06001BDC RID: 7132 RVA: 0x000AAB05 File Offset: 0x000A8D05
		public virtual Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			Log.ErrorOnce("CloneColored not implemented on this subclass of Graphic: " + base.GetType().ToString(), 66300);
			return BaseContent.BadGraphic;
		}

		// Token: 0x06001BDD RID: 7133 RVA: 0x000AAB2B File Offset: 0x000A8D2B
		[Obsolete("Will be removed in a future release")]
		public virtual Graphic GetCopy(Vector2 newDrawSize)
		{
			return this.GetCopy(newDrawSize, null);
		}

		// Token: 0x06001BDE RID: 7134 RVA: 0x000AAB35 File Offset: 0x000A8D35
		public virtual Graphic GetCopy(Vector2 newDrawSize, Shader overrideShader)
		{
			return GraphicDatabase.Get(base.GetType(), this.path, overrideShader ?? this.Shader, newDrawSize, this.color, this.colorTwo, null);
		}

		// Token: 0x06001BDF RID: 7135 RVA: 0x000AAB64 File Offset: 0x000A8D64
		public virtual Graphic GetShadowlessGraphic()
		{
			if (this.data == null || this.data.shadowData == null)
			{
				return this;
			}
			if (this.cachedShadowlessGraphicInt == null)
			{
				GraphicData graphicData = new GraphicData();
				graphicData.CopyFrom(this.data);
				graphicData.shadowData = null;
				this.cachedShadowlessGraphicInt = graphicData.Graphic;
			}
			return this.cachedShadowlessGraphicInt;
		}

		// Token: 0x06001BE0 RID: 7136 RVA: 0x000AABBC File Offset: 0x000A8DBC
		protected float AngleFromRot(Rot4 rot)
		{
			if (this.ShouldDrawRotated)
			{
				float num = rot.AsAngle;
				num += this.DrawRotatedExtraAngleOffset;
				if ((rot == Rot4.West && this.WestFlipped) || (rot == Rot4.East && this.EastFlipped))
				{
					num += 180f;
				}
				return num;
			}
			return 0f;
		}

		// Token: 0x06001BE1 RID: 7137 RVA: 0x000AAC1C File Offset: 0x000A8E1C
		protected Quaternion QuatFromRot(Rot4 rot)
		{
			float num = this.AngleFromRot(rot);
			if (num == 0f)
			{
				return Quaternion.identity;
			}
			return Quaternion.AngleAxis(num, Vector3.up);
		}

		// Token: 0x0400140A RID: 5130
		public GraphicData data;

		// Token: 0x0400140B RID: 5131
		public string path;

		// Token: 0x0400140C RID: 5132
		public string maskPath;

		// Token: 0x0400140D RID: 5133
		public Color color = Color.white;

		// Token: 0x0400140E RID: 5134
		public Color colorTwo = Color.white;

		// Token: 0x0400140F RID: 5135
		public Vector2 drawSize = Vector2.one;

		// Token: 0x04001410 RID: 5136
		private Graphic_Shadow cachedShadowGraphicInt;

		// Token: 0x04001411 RID: 5137
		private Graphic cachedShadowlessGraphicInt;

		// Token: 0x04001412 RID: 5138
		private static Dictionary<Graphic.AtlasReplacementInfoCacheKey, Graphic.CachedAtlasReplacementInfo> replacementInfoCache = new Dictionary<Graphic.AtlasReplacementInfoCacheKey, Graphic.CachedAtlasReplacementInfo>();

		// Token: 0x02001E95 RID: 7829
		private struct AtlasReplacementInfoCacheKey : IEquatable<Graphic.AtlasReplacementInfoCacheKey>
		{
			// Token: 0x0600B9A0 RID: 47520 RVA: 0x00420138 File Offset: 0x0041E338
			public AtlasReplacementInfoCacheKey(Material mat, TextureAtlasGroup group, bool flipUv, bool vertexColors)
			{
				this.mat = mat;
				this.group = group;
				this.flipUv = flipUv;
				this.vertexColors = vertexColors;
				this.hash = Gen.HashCombine<int>(mat.GetHashCode(), group.GetHashCode());
				if (flipUv)
				{
					this.hash = ~this.hash;
				}
				if (vertexColors)
				{
					this.hash ^= 123893723;
				}
			}

			// Token: 0x0600B9A1 RID: 47521 RVA: 0x004201A6 File Offset: 0x0041E3A6
			public bool Equals(Graphic.AtlasReplacementInfoCacheKey other)
			{
				return this.mat == other.mat && this.group == other.group && this.flipUv == other.flipUv && this.vertexColors == other.vertexColors;
			}

			// Token: 0x0600B9A2 RID: 47522 RVA: 0x004201E2 File Offset: 0x0041E3E2
			public override int GetHashCode()
			{
				return this.hash;
			}

			// Token: 0x04007869 RID: 30825
			public readonly Material mat;

			// Token: 0x0400786A RID: 30826
			public readonly TextureAtlasGroup group;

			// Token: 0x0400786B RID: 30827
			public readonly bool flipUv;

			// Token: 0x0400786C RID: 30828
			public readonly bool vertexColors;

			// Token: 0x0400786D RID: 30829
			private readonly int hash;
		}

		// Token: 0x02001E96 RID: 7830
		private struct CachedAtlasReplacementInfo
		{
			// Token: 0x0400786E RID: 30830
			public Material material;

			// Token: 0x0400786F RID: 30831
			public Vector2[] uvs;

			// Token: 0x04007870 RID: 30832
			public Color32 vertexColor;
		}
	}
}
