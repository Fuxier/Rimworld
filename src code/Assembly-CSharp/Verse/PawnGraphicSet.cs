using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002A7 RID: 679
	public class PawnGraphicSet
	{
		// Token: 0x170003BC RID: 956
		// (get) Token: 0x0600134E RID: 4942 RVA: 0x00073C31 File Offset: 0x00071E31
		public bool AllResolved
		{
			get
			{
				return this.nakedGraphic != null;
			}
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x00073C3C File Offset: 0x00071E3C
		public List<Material> MatsBodyBaseAt(Rot4 facing, bool dead, RotDrawMode bodyCondition = RotDrawMode.Fresh, bool drawClothes = true)
		{
			int num = facing.AsInt + 1000 * (int)bodyCondition;
			if (drawClothes)
			{
				num += 10000;
			}
			if (dead)
			{
				num += 100000;
			}
			if (num != this.cachedMatsBodyBaseHash)
			{
				this.cachedMatsBodyBase.Clear();
				this.cachedMatsBodyBaseHash = num;
				if (bodyCondition == RotDrawMode.Fresh)
				{
					if (dead && this.corpseGraphic != null)
					{
						this.cachedMatsBodyBase.Add(this.corpseGraphic.MatAt(facing, null));
					}
					else
					{
						this.cachedMatsBodyBase.Add(this.nakedGraphic.MatAt(facing, null));
					}
				}
				else if (bodyCondition == RotDrawMode.Rotting || this.dessicatedGraphic == null)
				{
					this.cachedMatsBodyBase.Add(this.rottingGraphic.MatAt(facing, null));
				}
				else if (bodyCondition == RotDrawMode.Dessicated)
				{
					this.cachedMatsBodyBase.Add(this.dessicatedGraphic.MatAt(facing, null));
				}
				if (drawClothes)
				{
					for (int i = 0; i < this.apparelGraphics.Count; i++)
					{
						if ((this.apparelGraphics[i].sourceApparel.def.apparel.shellRenderedBehindHead || this.apparelGraphics[i].sourceApparel.def.apparel.LastLayer != ApparelLayerDefOf.Shell) && !PawnRenderer.RenderAsPack(this.apparelGraphics[i].sourceApparel) && this.apparelGraphics[i].sourceApparel.def.apparel.LastLayer != ApparelLayerDefOf.Overhead && this.apparelGraphics[i].sourceApparel.def.apparel.LastLayer != ApparelLayerDefOf.EyeCover)
						{
							this.cachedMatsBodyBase.Add(this.apparelGraphics[i].graphic.MatAt(facing, null));
						}
					}
				}
			}
			return this.cachedMatsBodyBase;
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06001350 RID: 4944 RVA: 0x00073E10 File Offset: 0x00072010
		public GraphicMeshSet HairMeshSet
		{
			get
			{
				return HumanlikeMeshPoolUtility.GetHumanlikeHairSetForPawn(this.pawn);
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06001351 RID: 4945 RVA: 0x00073E1D File Offset: 0x0007201D
		public GraphicMeshSet BeardMeshSet
		{
			get
			{
				return HumanlikeMeshPoolUtility.GetHumanlikeBeardSetForPawn(this.pawn);
			}
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x00073E2C File Offset: 0x0007202C
		public Material HeadMatAt(Rot4 facing, RotDrawMode bodyCondition = RotDrawMode.Fresh, bool stump = false, bool portrait = false, bool allowOverride = true)
		{
			Material material = null;
			if (bodyCondition == RotDrawMode.Fresh)
			{
				if (stump)
				{
					material = this.headStumpGraphic.MatAt(facing, null);
				}
				else
				{
					material = this.headGraphic.MatAt(facing, null);
				}
			}
			else if (bodyCondition == RotDrawMode.Rotting)
			{
				if (stump)
				{
					material = this.desiccatedHeadStumpGraphic.MatAt(facing, null);
				}
				else
				{
					material = this.desiccatedHeadGraphic.MatAt(facing, null);
				}
			}
			else if (bodyCondition == RotDrawMode.Dessicated && !stump)
			{
				material = this.skullGraphic.MatAt(facing, null);
			}
			if (material != null && allowOverride)
			{
				if (!portrait && this.pawn.IsInvisible())
				{
					material = InvisibilityMatPool.GetInvisibleMat(material);
				}
				material = this.flasher.GetDamagedMat(material);
			}
			return material;
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x00073ED0 File Offset: 0x000720D0
		public Material HairMatAt(Rot4 facing, bool portrait = false, bool cached = false)
		{
			if (this.hairGraphic == null)
			{
				return null;
			}
			Material material = this.hairGraphic.MatAt(facing, null);
			if (!portrait && this.pawn.IsInvisible())
			{
				material = InvisibilityMatPool.GetInvisibleMat(material);
			}
			if (!cached)
			{
				return this.flasher.GetDamagedMat(material);
			}
			return material;
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x00073F20 File Offset: 0x00072120
		public Material FurMatAt(Rot4 facing, bool portrait = false, bool cached = false)
		{
			if (this.furCoveredGraphic == null)
			{
				return null;
			}
			Material material = this.furCoveredGraphic.MatAt(facing, null);
			if (!portrait && this.pawn.IsInvisible())
			{
				material = InvisibilityMatPool.GetInvisibleMat(material);
			}
			if (!cached)
			{
				return this.flasher.GetDamagedMat(material);
			}
			return material;
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x00073F70 File Offset: 0x00072170
		public Material BeardMatAt(Rot4 facing, bool portrait = false, bool cached = false)
		{
			if (this.beardGraphic == null)
			{
				return null;
			}
			Material material = this.beardGraphic.MatAt(facing, null);
			if (!portrait && this.pawn.IsInvisible())
			{
				material = InvisibilityMatPool.GetInvisibleMat(material);
			}
			if (!cached)
			{
				return this.flasher.GetDamagedMat(material);
			}
			return material;
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x00073FC0 File Offset: 0x000721C0
		public Material SwaddledBabyMatAt(Rot4 facing, bool portrait = false, bool cached = false)
		{
			if (this.swaddledBabyGraphic == null)
			{
				return null;
			}
			Material material = this.swaddledBabyGraphic.MatAt(facing, null);
			if (!portrait && this.pawn.IsInvisible())
			{
				material = InvisibilityMatPool.GetInvisibleMat(material);
			}
			if (!cached)
			{
				return this.flasher.GetDamagedMat(material);
			}
			return material;
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x00074010 File Offset: 0x00072210
		public PawnGraphicSet(Pawn pawn)
		{
			this.pawn = pawn;
			this.flasher = new DamageFlasher(pawn);
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x0007405E File Offset: 0x0007225E
		public void ClearCache()
		{
			this.cachedMatsBodyBaseHash = -1;
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x00074068 File Offset: 0x00072268
		public void ResolveAllGraphics()
		{
			this.ClearCache();
			if (this.pawn.RaceProps.Humanlike)
			{
				Color color = this.pawn.story.SkinColorOverriden ? (PawnGraphicSet.RottingColorDefault * this.pawn.story.SkinColor) : PawnGraphicSet.RottingColorDefault;
				this.nakedGraphic = GraphicDatabase.Get<Graphic_Multi>(this.pawn.story.bodyType.bodyNakedGraphicPath, ShaderUtility.GetSkinShader(this.pawn.story.SkinColorOverriden), Vector2.one, this.pawn.story.SkinColor);
				this.rottingGraphic = GraphicDatabase.Get<Graphic_Multi>(this.pawn.story.bodyType.bodyNakedGraphicPath, ShaderUtility.GetSkinShader(this.pawn.story.SkinColorOverriden), Vector2.one, color);
				this.dessicatedGraphic = GraphicDatabase.Get<Graphic_Multi>(this.pawn.story.bodyType.bodyDessicatedGraphicPath, ShaderDatabase.Cutout);
				if (ModLister.BiotechInstalled)
				{
					if (this.pawn.story.furDef != null)
					{
						this.furCoveredGraphic = GraphicDatabase.Get<Graphic_Multi>(this.pawn.story.furDef.GetFurBodyGraphicPath(this.pawn), ShaderDatabase.CutoutSkinOverlay, Vector2.one, this.pawn.story.HairColor);
					}
					else
					{
						this.furCoveredGraphic = null;
					}
				}
				if (ModsConfig.BiotechActive)
				{
					this.swaddledBabyGraphic = GraphicDatabase.Get<Graphic_Multi>("Things/Pawn/Humanlike/Apparel/SwaddledBaby/Swaddled_Child", ShaderDatabase.Cutout, Vector2.one, this.SwaddleColor());
				}
				if (this.pawn.style != null && ModsConfig.IdeologyActive)
				{
					if (ModLister.BiotechInstalled && this.pawn.genes != null)
					{
						if (this.pawn.genes.GenesListForReading.Any((Gene x) => x.def.graphicData != null && !x.def.graphicData.tattoosVisible && x.Active))
						{
							goto IL_2FA;
						}
					}
					Color skinColor = this.pawn.story.SkinColor;
					skinColor.a *= 0.8f;
					if (this.pawn.style.FaceTattoo != null && this.pawn.style.FaceTattoo != TattooDefOf.NoTattoo_Face)
					{
						this.faceTattooGraphic = GraphicDatabase.Get<Graphic_Multi>(this.pawn.style.FaceTattoo.texPath, ShaderDatabase.CutoutSkinOverlay, Vector2.one, skinColor, Color.white, null, this.pawn.story.headType.graphicPath);
					}
					else
					{
						this.faceTattooGraphic = null;
					}
					if (this.pawn.style.BodyTattoo != null && this.pawn.style.BodyTattoo != TattooDefOf.NoTattoo_Body)
					{
						this.bodyTattooGraphic = GraphicDatabase.Get<Graphic_Multi>(this.pawn.style.BodyTattoo.texPath, ShaderDatabase.CutoutSkinOverlay, Vector2.one, skinColor, Color.white, null, this.pawn.story.bodyType.bodyNakedGraphicPath);
					}
					else
					{
						this.bodyTattooGraphic = null;
					}
				}
				IL_2FA:
				this.headGraphic = this.pawn.story.headType.GetGraphic(this.pawn.story.SkinColor, false, this.pawn.story.SkinColorOverriden);
				this.desiccatedHeadGraphic = this.pawn.story.headType.GetGraphic(color, true, this.pawn.story.SkinColorOverriden);
				this.skullGraphic = HeadTypeDefOf.Skull.GetGraphic(Color.white, true, false);
				this.headStumpGraphic = HeadTypeDefOf.Stump.GetGraphic(this.pawn.story.SkinColor, false, this.pawn.story.SkinColorOverriden);
				this.desiccatedHeadStumpGraphic = HeadTypeDefOf.Stump.GetGraphic(color, true, this.pawn.story.SkinColorOverriden);
				this.CalculateHairMats();
				this.ResolveApparelGraphics();
				this.ResolveGeneGraphics();
				return;
			}
			PawnKindLifeStage curKindLifeStage = this.pawn.ageTracker.CurKindLifeStage;
			if (this.pawn.gender != Gender.Female || curKindLifeStage.femaleGraphicData == null)
			{
				this.nakedGraphic = curKindLifeStage.bodyGraphicData.Graphic;
			}
			else
			{
				this.nakedGraphic = curKindLifeStage.femaleGraphicData.Graphic;
			}
			if (this.pawn.RaceProps.packAnimal)
			{
				this.packGraphic = GraphicDatabase.Get<Graphic_Multi>(this.nakedGraphic.path + "Pack", ShaderDatabase.Cutout, this.nakedGraphic.drawSize, Color.white);
			}
			Shader newShader = (this.pawn.story == null) ? ShaderDatabase.CutoutSkin : ShaderUtility.GetSkinShader(this.pawn.story.SkinColorOverriden);
			if (curKindLifeStage.corpseGraphicData != null)
			{
				if (this.pawn.gender != Gender.Female || curKindLifeStage.femaleCorpseGraphicData == null)
				{
					this.corpseGraphic = curKindLifeStage.corpseGraphicData.Graphic.GetColoredVersion(curKindLifeStage.corpseGraphicData.Graphic.Shader, this.nakedGraphic.Color, this.nakedGraphic.ColorTwo);
					this.rottingGraphic = curKindLifeStage.corpseGraphicData.Graphic.GetColoredVersion(newShader, PawnGraphicSet.RottingColorDefault, PawnGraphicSet.RottingColorDefault);
				}
				else
				{
					this.corpseGraphic = curKindLifeStage.femaleCorpseGraphicData.Graphic.GetColoredVersion(curKindLifeStage.femaleCorpseGraphicData.Graphic.Shader, this.nakedGraphic.Color, this.nakedGraphic.ColorTwo);
					this.rottingGraphic = curKindLifeStage.femaleCorpseGraphicData.Graphic.GetColoredVersion(newShader, PawnGraphicSet.RottingColorDefault, PawnGraphicSet.RottingColorDefault);
				}
			}
			else
			{
				this.corpseGraphic = null;
				this.rottingGraphic = this.nakedGraphic.GetColoredVersion(newShader, PawnGraphicSet.RottingColorDefault, PawnGraphicSet.RottingColorDefault);
			}
			if (curKindLifeStage.dessicatedBodyGraphicData != null)
			{
				if (this.pawn.RaceProps.FleshType == FleshTypeDefOf.Insectoid)
				{
					if (this.pawn.gender != Gender.Female || curKindLifeStage.femaleDessicatedBodyGraphicData == null)
					{
						this.dessicatedGraphic = curKindLifeStage.dessicatedBodyGraphicData.Graphic.GetColoredVersion(ShaderDatabase.Cutout, PawnGraphicSet.DessicatedColorInsect, PawnGraphicSet.DessicatedColorInsect);
					}
					else
					{
						this.dessicatedGraphic = curKindLifeStage.femaleDessicatedBodyGraphicData.Graphic.GetColoredVersion(ShaderDatabase.Cutout, PawnGraphicSet.DessicatedColorInsect, PawnGraphicSet.DessicatedColorInsect);
					}
				}
				else if (this.pawn.gender != Gender.Female || curKindLifeStage.femaleDessicatedBodyGraphicData == null)
				{
					this.dessicatedGraphic = curKindLifeStage.dessicatedBodyGraphicData.GraphicColoredFor(this.pawn);
				}
				else
				{
					this.dessicatedGraphic = curKindLifeStage.femaleDessicatedBodyGraphicData.GraphicColoredFor(this.pawn);
				}
			}
			if (!this.pawn.kindDef.alternateGraphics.NullOrEmpty<AlternateGraphic>())
			{
				Rand.PushState(this.pawn.thingIDNumber ^ 46101);
				if (Rand.Value <= this.pawn.kindDef.alternateGraphicChance)
				{
					this.nakedGraphic = this.pawn.kindDef.alternateGraphics.RandomElementByWeight((AlternateGraphic x) => x.Weight).GetGraphic(this.nakedGraphic);
				}
				Rand.PopState();
			}
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x00074770 File Offset: 0x00072970
		public void SetAllGraphicsDirty()
		{
			if (this.AllResolved)
			{
				this.ResolveAllGraphics();
			}
			GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this.pawn);
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x0007478C File Offset: 0x0007298C
		public void ResolveApparelGraphics()
		{
			this.ClearCache();
			this.apparelGraphics.Clear();
			using (List<Apparel>.Enumerator enumerator = this.pawn.apparel.WornApparel.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ApparelGraphicRecord item;
					if (ApparelGraphicRecordGetter.TryGetGraphicApparel(enumerator.Current, this.pawn.story.bodyType, out item))
					{
						this.apparelGraphics.Add(item);
					}
				}
			}
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x00074818 File Offset: 0x00072A18
		public void CalculateHairMats()
		{
			if (this.pawn.story.hairDef != null)
			{
				this.hairGraphic = (this.pawn.story.hairDef.noGraphic ? null : GraphicDatabase.Get<Graphic_Multi>(this.pawn.story.hairDef.texPath, ShaderDatabase.Transparent, Vector2.one, this.pawn.story.HairColor));
			}
			if (this.pawn.style != null && this.pawn.style.beardDef != null && !this.pawn.style.beardDef.noGraphic)
			{
				this.beardGraphic = GraphicDatabase.Get<Graphic_Multi>(this.pawn.style.beardDef.texPath, ShaderDatabase.Transparent, Vector2.one, this.pawn.story.HairColor);
			}
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x000748FB File Offset: 0x00072AFB
		public void SetApparelGraphicsDirty()
		{
			if (this.AllResolved)
			{
				this.ResolveApparelGraphics();
			}
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x0007490C File Offset: 0x00072B0C
		public void ResolveGeneGraphics()
		{
			if (!ModsConfig.BiotechActive || this.pawn.genes == null)
			{
				return;
			}
			Color rottingColor = this.pawn.story.SkinColorOverriden ? (PawnGraphicSet.RottingColorDefault * this.pawn.story.SkinColor) : PawnGraphicSet.RottingColorDefault;
			Shader skinShader = ShaderUtility.GetSkinShader(this.pawn.story.SkinColorOverriden);
			this.geneGraphics.Clear();
			foreach (Gene gene in this.pawn.genes.GenesListForReading)
			{
				if (gene.def.HasGraphic && gene.Active)
				{
					ValueTuple<Graphic, Graphic> graphics = gene.def.graphicData.GetGraphics(this.pawn, skinShader, rottingColor);
					this.geneGraphics.Add(new GeneGraphicRecord(graphics.Item1, graphics.Item2, gene));
				}
			}
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x00074A1C File Offset: 0x00072C1C
		private Color SwaddleColor()
		{
			Rand.PushState(this.pawn.thingIDNumber);
			float num = Rand.Range(0.6f, 0.89f);
			float num2 = Rand.Range(-0.1f, 0.1f);
			float num3 = Rand.Range(-0.1f, 0.1f);
			float num4 = Rand.Range(-0.1f, 0.1f);
			Rand.PopState();
			return new Color(num + num2, num + num3, num + num4);
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x00074A8C File Offset: 0x00072C8C
		public Material GetOverlayMat(Material mat, Color color)
		{
			Material material;
			if (!PawnGraphicSet.overlayMats.TryGetValue(mat, out material))
			{
				material = MaterialAllocator.Create(mat);
				PawnGraphicSet.overlayMats.Add(mat, material);
			}
			material.SetColor(ShaderPropertyIDs.OverlayColor, color);
			material.SetFloat(ShaderPropertyIDs.OverlayOpacity, 0.5f);
			return material;
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x00074AD8 File Offset: 0x00072CD8
		public void SetGeneGraphicsDirty()
		{
			if (this.AllResolved)
			{
				this.ResolveGeneGraphics();
			}
		}

		// Token: 0x04000FF9 RID: 4089
		public Pawn pawn;

		// Token: 0x04000FFA RID: 4090
		public Graphic nakedGraphic;

		// Token: 0x04000FFB RID: 4091
		public Graphic rottingGraphic;

		// Token: 0x04000FFC RID: 4092
		public Graphic dessicatedGraphic;

		// Token: 0x04000FFD RID: 4093
		public Graphic corpseGraphic;

		// Token: 0x04000FFE RID: 4094
		public Graphic packGraphic;

		// Token: 0x04000FFF RID: 4095
		public DamageFlasher flasher;

		// Token: 0x04001000 RID: 4096
		private static Dictionary<Material, Material> overlayMats = new Dictionary<Material, Material>();

		// Token: 0x04001001 RID: 4097
		public Graphic headGraphic;

		// Token: 0x04001002 RID: 4098
		public Graphic desiccatedHeadGraphic;

		// Token: 0x04001003 RID: 4099
		public Graphic skullGraphic;

		// Token: 0x04001004 RID: 4100
		public Graphic headStumpGraphic;

		// Token: 0x04001005 RID: 4101
		public Graphic desiccatedHeadStumpGraphic;

		// Token: 0x04001006 RID: 4102
		public Graphic hairGraphic;

		// Token: 0x04001007 RID: 4103
		public Graphic beardGraphic;

		// Token: 0x04001008 RID: 4104
		public Graphic swaddledBabyGraphic;

		// Token: 0x04001009 RID: 4105
		public List<ApparelGraphicRecord> apparelGraphics = new List<ApparelGraphicRecord>();

		// Token: 0x0400100A RID: 4106
		public Graphic bodyTattooGraphic;

		// Token: 0x0400100B RID: 4107
		public Graphic faceTattooGraphic;

		// Token: 0x0400100C RID: 4108
		public List<GeneGraphicRecord> geneGraphics = new List<GeneGraphicRecord>();

		// Token: 0x0400100D RID: 4109
		public Graphic furCoveredGraphic;

		// Token: 0x0400100E RID: 4110
		private List<Material> cachedMatsBodyBase = new List<Material>();

		// Token: 0x0400100F RID: 4111
		private int cachedMatsBodyBaseHash = -1;

		// Token: 0x04001010 RID: 4112
		public static readonly Color RottingColorDefault = new Color(0.34f, 0.32f, 0.3f);

		// Token: 0x04001011 RID: 4113
		public static readonly Color DessicatedColorInsect = new Color(0.8f, 0.8f, 0.8f);

		// Token: 0x04001012 RID: 4114
		private const float TattooOpacity = 0.8f;

		// Token: 0x04001013 RID: 4115
		private const float SwaddleColorOffset = 0.1f;

		// Token: 0x04001014 RID: 4116
		private const string SwaddleGraphicPath = "Things/Pawn/Humanlike/Apparel/SwaddledBaby/Swaddled_Child";
	}
}
