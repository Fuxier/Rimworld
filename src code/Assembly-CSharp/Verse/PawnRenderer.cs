using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020002AA RID: 682
	public class PawnRenderer
	{
		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06001364 RID: 4964 RVA: 0x00074B2E File Offset: 0x00072D2E
		private RotDrawMode CurRotDrawMode
		{
			get
			{
				if (this.pawn.Dead && this.pawn.Corpse != null)
				{
					return this.pawn.Corpse.CurRotDrawMode;
				}
				return RotDrawMode.Fresh;
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06001365 RID: 4965 RVA: 0x00074B5C File Offset: 0x00072D5C
		public PawnWoundDrawer WoundOverlays
		{
			get
			{
				return this.woundOverlays;
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06001366 RID: 4966 RVA: 0x00074B64 File Offset: 0x00072D64
		public PawnFirefoamDrawer FirefoamOverlays
		{
			get
			{
				return this.firefoamOverlays;
			}
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x00074B6C File Offset: 0x00072D6C
		public PawnRenderer(Pawn pawn)
		{
			this.pawn = pawn;
			this.wiggler = new PawnDownedWiggler(pawn);
			this.statusOverlays = new PawnHeadOverlays(pawn);
			this.woundOverlays = new PawnWoundDrawer(pawn);
			this.firefoamOverlays = new PawnFirefoamDrawer(pawn);
			this.graphics = new PawnGraphicSet(pawn);
			this.effecters = new PawnStatusEffecters(pawn);
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x00074BD0 File Offset: 0x00072DD0
		private PawnRenderFlags GetDefaultRenderFlags(Pawn pawn)
		{
			PawnRenderFlags pawnRenderFlags = PawnRenderFlags.None;
			if (pawn.IsInvisible())
			{
				pawnRenderFlags |= PawnRenderFlags.Invisible;
			}
			if (!pawn.health.hediffSet.HasHead)
			{
				pawnRenderFlags |= PawnRenderFlags.HeadStump;
			}
			return pawnRenderFlags;
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x00074C04 File Offset: 0x00072E04
		private Mesh GetBlitMeshUpdatedFrame(PawnTextureAtlasFrameSet frameSet, Rot4 rotation, PawnDrawMode drawMode)
		{
			int index = frameSet.GetIndex(rotation, drawMode);
			if (frameSet.isDirty[index])
			{
				Find.PawnCacheCamera.rect = frameSet.uvRects[index];
				Find.PawnCacheRenderer.RenderPawn(this.pawn, frameSet.atlas, Vector3.zero, 1f, 0f, rotation, true, drawMode == PawnDrawMode.BodyAndHead, true, true, false, default(Vector3), null, null, false);
				Find.PawnCacheCamera.rect = new Rect(0f, 0f, 1f, 1f);
				frameSet.isDirty[index] = false;
			}
			return frameSet.meshes[index];
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x00074CB4 File Offset: 0x00072EB4
		public static void CalculateCarriedDrawPos(Pawn pawn, Thing carriedThing, ref Vector3 carryDrawPos, out bool behind, out bool flip)
		{
			behind = false;
			flip = false;
			if (pawn.CurJob == null || !pawn.jobs.curDriver.ModifyCarriedThingDrawPos(ref carryDrawPos, ref behind, ref flip))
			{
				if (carriedThing is Pawn || carriedThing is Corpse)
				{
					Pawn pawn2;
					if ((pawn2 = (carriedThing as Pawn)) != null && pawn2.RaceProps.Humanlike && pawn2.DevelopmentalStage.Baby())
					{
						Vector2 vector = new Vector2(-0.1f, -0.28f).RotatedBy(pawn.Drawer.renderer.BodyAngle() * -1f);
						carryDrawPos += new Vector3(vector.x, 0f, vector.y);
						return;
					}
					carryDrawPos += new Vector3(0.44f, 0f, 0f);
					return;
				}
				else
				{
					carryDrawPos += new Vector3(0.18f, 0f, 0.05f);
				}
			}
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x00074DC0 File Offset: 0x00072FC0
		private void DrawCarriedThing(Vector3 drawLoc)
		{
			Pawn_CarryTracker carryTracker = this.pawn.carryTracker;
			Thing carriedThing;
			if ((carriedThing = ((carryTracker != null) ? carryTracker.CarriedThing : null)) != null)
			{
				PawnRenderer.DrawCarriedThing(this.pawn, drawLoc, carriedThing);
			}
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x00074DF8 File Offset: 0x00072FF8
		public static void DrawCarriedThing(Pawn pawn, Vector3 drawLoc, Thing carriedThing)
		{
			Vector3 drawLoc2 = drawLoc;
			bool flag;
			bool flip;
			PawnRenderer.CalculateCarriedDrawPos(pawn, carriedThing, ref drawLoc2, out flag, out flip);
			if (flag)
			{
				drawLoc2.y -= 0.03474903f;
			}
			else
			{
				drawLoc2.y += 0.03474903f;
			}
			carriedThing.DrawAt(drawLoc2, flip);
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x00074E44 File Offset: 0x00073044
		private void DrawInvisibleShadow(Vector3 drawLoc)
		{
			if (this.pawn.def.race.specialShadowData != null)
			{
				if (this.shadowGraphic == null)
				{
					this.shadowGraphic = new Graphic_Shadow(this.pawn.def.race.specialShadowData);
				}
				this.shadowGraphic.Draw(drawLoc, Rot4.North, this.pawn, 0f);
			}
			Graphic nakedGraphic = this.graphics.nakedGraphic;
			if (nakedGraphic == null)
			{
				return;
			}
			Graphic_Shadow graphic_Shadow = nakedGraphic.ShadowGraphic;
			if (graphic_Shadow == null)
			{
				return;
			}
			graphic_Shadow.Draw(drawLoc, Rot4.North, this.pawn, 0f);
		}

		// Token: 0x0600136E RID: 4974 RVA: 0x00074EDC File Offset: 0x000730DC
		private Vector3 GetBodyPos(Vector3 drawLoc, out bool showBody)
		{
			Building_Bed building_Bed = this.pawn.CurrentBed();
			Vector3 result;
			if (building_Bed != null && this.pawn.RaceProps.Humanlike)
			{
				showBody = building_Bed.def.building.bed_showSleeperBody;
				AltitudeLayer altLayer = (AltitudeLayer)Mathf.Max((int)building_Bed.def.altitudeLayer, 18);
				Vector3 a = this.pawn.Position.ToVector3ShiftedWithAltitude(altLayer);
				Rot4 rotation = building_Bed.Rotation;
				rotation.AsInt += 2;
				float d = this.BaseHeadOffsetAt(Rot4.South).z + this.pawn.story.bodyType.bedOffset + building_Bed.def.building.bed_pawnDrawOffset;
				Vector3 a2 = rotation.FacingCell.ToVector3();
				result = a - a2 * d;
				result.y += 0.008687258f;
			}
			else
			{
				showBody = true;
				result = drawLoc;
				IThingHolderWithDrawnPawn thingHolderWithDrawnPawn;
				if ((thingHolderWithDrawnPawn = (this.pawn.ParentHolder as IThingHolderWithDrawnPawn)) != null)
				{
					result.y = thingHolderWithDrawnPawn.HeldPawnDrawPos_Y;
				}
				else if (!this.pawn.Dead && this.pawn.CarriedBy == null)
				{
					result.y = AltitudeLayer.LayingPawn.AltitudeFor() + 0.008687258f;
				}
			}
			Pawn_MindState mindState = this.pawn.mindState;
			bool? flag;
			if (mindState == null)
			{
				flag = null;
			}
			else
			{
				PawnDuty duty = mindState.duty;
				if (duty == null)
				{
					flag = null;
				}
				else
				{
					DutyDef def = duty.def;
					flag = ((def != null) ? def.drawBodyOverride : null);
				}
			}
			showBody = (flag ?? showBody);
			return result;
		}

		// Token: 0x0600136F RID: 4975 RVA: 0x00075088 File Offset: 0x00073288
		public GraphicMeshSet GetBodyOverlayMeshSet()
		{
			if (!this.pawn.RaceProps.Humanlike)
			{
				return HumanlikeMeshPoolUtility.GetHumanlikeBodySetForPawn(this.pawn);
			}
			BodyTypeDef bodyType = this.pawn.story.bodyType;
			if (bodyType == BodyTypeDefOf.Male)
			{
				return MeshPool.humanlikeBodySet_Male;
			}
			if (bodyType == BodyTypeDefOf.Female)
			{
				return MeshPool.humanlikeBodySet_Female;
			}
			if (bodyType == BodyTypeDefOf.Thin)
			{
				return MeshPool.humanlikeBodySet_Thin;
			}
			if (bodyType == BodyTypeDefOf.Fat)
			{
				return MeshPool.humanlikeBodySet_Fat;
			}
			if (bodyType == BodyTypeDefOf.Hulk)
			{
				return MeshPool.humanlikeBodySet_Hulk;
			}
			return HumanlikeMeshPoolUtility.GetHumanlikeBodySetForPawn(this.pawn);
		}

		// Token: 0x06001370 RID: 4976 RVA: 0x00075118 File Offset: 0x00073318
		public void RenderPawnAt(Vector3 drawLoc, Rot4? rotOverride = null, bool neverAimWeapon = false)
		{
			if (!this.graphics.AllResolved)
			{
				this.graphics.ResolveAllGraphics();
			}
			Rot4 rot = rotOverride ?? this.pawn.Rotation;
			PawnRenderFlags pawnRenderFlags = this.GetDefaultRenderFlags(this.pawn);
			pawnRenderFlags |= PawnRenderFlags.Clothes;
			pawnRenderFlags |= PawnRenderFlags.Headgear;
			if (neverAimWeapon)
			{
				pawnRenderFlags |= PawnRenderFlags.NeverAimWeapon;
			}
			RotDrawMode curRotDrawMode = this.CurRotDrawMode;
			bool flag = this.pawn.RaceProps.Humanlike && Find.CameraDriver.ZoomRootSize > 18f && curRotDrawMode != RotDrawMode.Dessicated && !this.pawn.IsInvisible() && !pawnRenderFlags.FlagSet(PawnRenderFlags.Portrait);
			PawnTextureAtlasFrameSet pawnTextureAtlasFrameSet = null;
			bool flag2;
			if (flag && !GlobalTextureAtlasManager.TryGetPawnFrameSet(this.pawn, out pawnTextureAtlasFrameSet, out flag2, true))
			{
				flag = false;
			}
			if (this.pawn.GetPosture() == PawnPosture.Standing)
			{
				if (flag)
				{
					Material material = MaterialPool.MatFrom(new MaterialRequest(pawnTextureAtlasFrameSet.atlas, ShaderDatabase.Cutout));
					material = this.OverrideMaterialIfNeeded(material, this.pawn, false);
					GenDraw.DrawMeshNowOrLater(this.GetBlitMeshUpdatedFrame(pawnTextureAtlasFrameSet, rot, PawnDrawMode.BodyAndHead), drawLoc, Quaternion.AngleAxis(0f, Vector3.up), material, false);
					this.DrawDynamicParts(drawLoc, 0f, rot, pawnRenderFlags);
				}
				else
				{
					this.RenderPawnInternal(drawLoc, 0f, true, rot, curRotDrawMode, pawnRenderFlags);
				}
				this.DrawCarriedThing(drawLoc);
				if (!pawnRenderFlags.FlagSet(PawnRenderFlags.Invisible))
				{
					this.DrawInvisibleShadow(drawLoc);
				}
			}
			else
			{
				bool flag3;
				Vector3 bodyPos = this.GetBodyPos(drawLoc, out flag3);
				float angle = this.BodyAngle();
				Rot4 rot2 = this.LayingFacing();
				if (flag)
				{
					Material material2 = MaterialPool.MatFrom(new MaterialRequest(pawnTextureAtlasFrameSet.atlas, ShaderDatabase.Cutout));
					material2 = this.OverrideMaterialIfNeeded(material2, this.pawn, false);
					GenDraw.DrawMeshNowOrLater(this.GetBlitMeshUpdatedFrame(pawnTextureAtlasFrameSet, rot2, flag3 ? PawnDrawMode.BodyAndHead : PawnDrawMode.HeadOnly), bodyPos, Quaternion.AngleAxis(angle, Vector3.up), material2, false);
					this.DrawDynamicParts(bodyPos, angle, rot, pawnRenderFlags);
				}
				else
				{
					this.RenderPawnInternal(bodyPos, angle, flag3, rot2, curRotDrawMode, pawnRenderFlags);
				}
				this.DrawCarriedThing(bodyPos);
			}
			if (this.pawn.Spawned && !this.pawn.Dead)
			{
				this.pawn.stances.StanceTrackerDraw();
				this.pawn.pather.PatherDraw();
				this.pawn.roping.RopingDraw();
			}
			this.DrawDebug();
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x00075368 File Offset: 0x00073568
		public void RenderCache(Rot4 rotation, float angle, Vector3 positionOffset, bool renderHead, bool renderBody, bool portrait, bool renderHeadgear, bool renderClothes, IReadOnlyDictionary<Apparel, Color> overrideApparelColor = null, Color? overrideHairColor = null, bool stylingStation = false)
		{
			Vector3 zero = Vector3.zero;
			PawnRenderFlags pawnRenderFlags = this.GetDefaultRenderFlags(this.pawn);
			if (portrait)
			{
				pawnRenderFlags |= PawnRenderFlags.Portrait;
			}
			pawnRenderFlags |= PawnRenderFlags.Cache;
			pawnRenderFlags |= PawnRenderFlags.DrawNow;
			if (!renderHead)
			{
				pawnRenderFlags |= PawnRenderFlags.HeadStump;
			}
			if (renderHeadgear)
			{
				pawnRenderFlags |= PawnRenderFlags.Headgear;
			}
			if (renderClothes)
			{
				pawnRenderFlags |= PawnRenderFlags.Clothes;
			}
			if (stylingStation)
			{
				pawnRenderFlags |= PawnRenderFlags.StylingStation;
			}
			PawnRenderer.tmpOriginalColors.Clear();
			try
			{
				if (overrideApparelColor != null)
				{
					foreach (KeyValuePair<Apparel, Color> keyValuePair in overrideApparelColor)
					{
						Apparel key = keyValuePair.Key;
						CompColorable compColorable = key.TryGetComp<CompColorable>();
						if (compColorable != null)
						{
							PawnRenderer.tmpOriginalColors.Add(key, new ValueTuple<Color, bool>(compColorable.Color, compColorable.Active));
							key.SetColor(keyValuePair.Value, true);
						}
					}
				}
				Color hairColor = Color.white;
				if (this.pawn.story != null)
				{
					hairColor = this.pawn.story.HairColor;
					if (overrideHairColor != null)
					{
						this.pawn.story.HairColor = overrideHairColor.Value;
						this.pawn.Drawer.renderer.graphics.ResolveAllGraphics();
					}
				}
				this.RenderPawnInternal(zero + positionOffset, angle, renderBody, rotation, this.CurRotDrawMode, pawnRenderFlags);
				foreach (KeyValuePair<Apparel, ValueTuple<Color, bool>> keyValuePair2 in PawnRenderer.tmpOriginalColors)
				{
					if (!keyValuePair2.Value.Item2)
					{
						keyValuePair2.Key.TryGetComp<CompColorable>().Disable();
					}
					else
					{
						keyValuePair2.Key.SetColor(keyValuePair2.Value.Item1, true);
					}
				}
				if (this.pawn.story != null && overrideHairColor != null)
				{
					this.pawn.story.HairColor = hairColor;
					this.pawn.Drawer.renderer.graphics.ResolveAllGraphics();
				}
			}
			catch (Exception arg)
			{
				Log.Error("Error rendering pawn portrait: " + arg);
			}
			finally
			{
				PawnRenderer.tmpOriginalColors.Clear();
			}
		}

		// Token: 0x06001372 RID: 4978 RVA: 0x000755E0 File Offset: 0x000737E0
		private void RenderPawnInternal(Vector3 rootLoc, float angle, bool renderBody, Rot4 bodyFacing, RotDrawMode bodyDrawType, PawnRenderFlags flags)
		{
			if (!this.graphics.AllResolved)
			{
				this.graphics.ResolveAllGraphics();
			}
			Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.up);
			Vector3 vector = rootLoc;
			if (this.pawn.ageTracker.CurLifeStage.bodyDrawOffset != null)
			{
				vector += this.pawn.ageTracker.CurLifeStage.bodyDrawOffset.Value;
			}
			Vector3 vector2 = vector;
			Vector3 a = vector;
			if (bodyFacing != Rot4.North)
			{
				a.y += 0.023166021f;
				vector2.y += 0.02027027f;
			}
			else
			{
				a.y += 0.02027027f;
				vector2.y += 0.023166021f;
			}
			Vector3 utilityLoc = vector;
			utilityLoc.y += ((bodyFacing == Rot4.South) ? 0.0057915053f : 0.028957527f);
			Mesh mesh = null;
			Vector3 drawLoc;
			if (renderBody)
			{
				this.DrawPawnBody(vector, angle, bodyFacing, bodyDrawType, flags, out mesh);
				if (bodyDrawType == RotDrawMode.Fresh && this.graphics.furCoveredGraphic != null)
				{
					Vector3 shellLoc = vector;
					shellLoc.y += 0.009187258f;
					this.DrawPawnFur(shellLoc, bodyFacing, quaternion, flags);
				}
				drawLoc = vector;
				drawLoc.y += 0.009687258f;
				if (bodyDrawType == RotDrawMode.Fresh)
				{
					this.woundOverlays.RenderPawnOverlay(drawLoc, mesh, quaternion, flags.FlagSet(PawnRenderFlags.DrawNow), PawnOverlayDrawer.OverlayLayer.Body, bodyFacing, new bool?(false));
				}
				if (flags.FlagSet(PawnRenderFlags.Clothes))
				{
					this.DrawBodyApparel(vector2, utilityLoc, mesh, angle, bodyFacing, flags);
				}
				if (this.pawn.SwaddleBaby())
				{
					this.SwaddleBaby(vector2, bodyFacing, quaternion, flags);
				}
				if (ModLister.BiotechInstalled && this.pawn.genes != null)
				{
					this.DrawBodyGenes(vector, quaternion, angle, bodyFacing, bodyDrawType, flags);
				}
				drawLoc = vector;
				drawLoc.y += 0.022166021f;
				if (bodyDrawType == RotDrawMode.Fresh)
				{
					this.woundOverlays.RenderPawnOverlay(drawLoc, mesh, quaternion, flags.FlagSet(PawnRenderFlags.DrawNow), PawnOverlayDrawer.OverlayLayer.Body, bodyFacing, new bool?(true));
				}
			}
			Vector3 vector3 = Vector3.zero;
			drawLoc = vector;
			drawLoc.y += 0.028957527f;
			Mesh mesh2 = null;
			if (this.graphics.headGraphic != null)
			{
				vector3 = quaternion * this.BaseHeadOffsetAt(bodyFacing);
				Material material = this.graphics.HeadMatAt(bodyFacing, bodyDrawType, flags.FlagSet(PawnRenderFlags.HeadStump), flags.FlagSet(PawnRenderFlags.Portrait), !flags.FlagSet(PawnRenderFlags.Cache));
				if (material != null)
				{
					mesh2 = HumanlikeMeshPoolUtility.GetHumanlikeHeadSetForPawn(this.pawn).MeshAt(bodyFacing);
					GenDraw.DrawMeshNowOrLater(mesh2, a + vector3, quaternion, material, flags.FlagSet(PawnRenderFlags.DrawNow));
				}
			}
			if (bodyDrawType == RotDrawMode.Fresh)
			{
				this.woundOverlays.RenderPawnOverlay(drawLoc, mesh, quaternion, flags.FlagSet(PawnRenderFlags.DrawNow), PawnOverlayDrawer.OverlayLayer.Head, bodyFacing, null);
			}
			if (this.graphics.headGraphic != null)
			{
				this.DrawHeadHair(vector, vector3, angle, bodyFacing, bodyFacing, bodyDrawType, flags, renderBody);
			}
			if (!flags.FlagSet(PawnRenderFlags.Portrait) && this.pawn.RaceProps.Animal && this.pawn.inventory != null && this.pawn.inventory.innerContainer.Count > 0 && this.graphics.packGraphic != null)
			{
				GenDraw.DrawMeshNowOrLater(mesh, Matrix4x4.TRS(vector2, quaternion, Vector3.one), this.graphics.packGraphic.MatAt(bodyFacing, null), flags.FlagSet(PawnRenderFlags.DrawNow));
			}
			if (this.firefoamOverlays.IsCoveredInFoam)
			{
				Vector3 drawLoc2 = vector;
				drawLoc2.y += 0.033301156f;
				if (renderBody)
				{
					this.firefoamOverlays.RenderPawnOverlay(drawLoc2, mesh, quaternion, flags.FlagSet(PawnRenderFlags.DrawNow), PawnOverlayDrawer.OverlayLayer.Body, bodyFacing, null);
				}
				if (mesh2 != null)
				{
					drawLoc2 = a + vector3;
					drawLoc2.y += 0.033301156f;
					this.firefoamOverlays.RenderPawnOverlay(drawLoc2, mesh2, quaternion, flags.FlagSet(PawnRenderFlags.DrawNow), PawnOverlayDrawer.OverlayLayer.Head, bodyFacing, null);
				}
			}
			if (!flags.FlagSet(PawnRenderFlags.Portrait) && !flags.FlagSet(PawnRenderFlags.Cache))
			{
				this.DrawDynamicParts(vector, angle, bodyFacing, flags);
			}
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x000759FC File Offset: 0x00073BFC
		private void DrawPawnBody(Vector3 rootLoc, float angle, Rot4 facing, RotDrawMode bodyDrawType, PawnRenderFlags flags, out Mesh bodyMesh)
		{
			Quaternion quat = Quaternion.AngleAxis(angle, Vector3.up);
			Vector3 vector = rootLoc;
			vector.y += 0.008687258f;
			Vector3 loc = vector;
			loc.y += 0.0014478763f;
			bodyMesh = null;
			if (bodyDrawType == RotDrawMode.Dessicated && !this.pawn.RaceProps.Humanlike && this.graphics.dessicatedGraphic != null && !flags.FlagSet(PawnRenderFlags.Portrait))
			{
				this.graphics.dessicatedGraphic.Draw(vector, facing, this.pawn, angle);
				return;
			}
			if (this.pawn.RaceProps.Humanlike)
			{
				bodyMesh = HumanlikeMeshPoolUtility.GetHumanlikeBodySetForPawn(this.pawn).MeshAt(facing);
			}
			else
			{
				bodyMesh = this.graphics.nakedGraphic.MeshAt(facing);
			}
			List<Material> list = this.graphics.MatsBodyBaseAt(facing, this.pawn.Dead, bodyDrawType, flags.FlagSet(PawnRenderFlags.Clothes));
			for (int i = 0; i < list.Count; i++)
			{
				Material material = (this.pawn.RaceProps.IsMechanoid && this.pawn.Faction != null && this.pawn.Faction != Faction.OfMechanoids) ? this.graphics.GetOverlayMat(list[i], this.pawn.Faction.MechColor) : list[i];
				Material mat = flags.FlagSet(PawnRenderFlags.Cache) ? material : this.OverrideMaterialIfNeeded(material, this.pawn, flags.FlagSet(PawnRenderFlags.Portrait));
				GenDraw.DrawMeshNowOrLater(bodyMesh, vector, quat, mat, flags.FlagSet(PawnRenderFlags.DrawNow));
				vector.y += 0.0028957527f;
			}
			if (ModsConfig.IdeologyActive && this.graphics.bodyTattooGraphic != null && bodyDrawType != RotDrawMode.Dessicated && (facing != Rot4.North || this.pawn.style.BodyTattoo.visibleNorth))
			{
				GenDraw.DrawMeshNowOrLater(this.GetBodyOverlayMeshSet().MeshAt(facing), loc, quat, this.graphics.bodyTattooGraphic.MatAt(facing, null), flags.FlagSet(PawnRenderFlags.DrawNow));
			}
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x00075C1C File Offset: 0x00073E1C
		private void DrawPawnFur(Vector3 shellLoc, Rot4 facing, Quaternion quat, PawnRenderFlags flags)
		{
			Mesh mesh = HumanlikeMeshPoolUtility.GetHumanlikeBodySetForPawn(this.pawn).MeshAt(facing);
			Material mat = this.graphics.FurMatAt(facing, flags.FlagSet(PawnRenderFlags.Portrait), flags.FlagSet(PawnRenderFlags.Cache));
			GenDraw.DrawMeshNowOrLater(mesh, shellLoc, quat, mat, flags.FlagSet(PawnRenderFlags.DrawNow));
		}

		// Token: 0x06001375 RID: 4981 RVA: 0x00075C68 File Offset: 0x00073E68
		private void SwaddleBaby(Vector3 shellLoc, Rot4 facing, Quaternion quat, PawnRenderFlags flags)
		{
			Mesh mesh = HumanlikeMeshPoolUtility.GetSwaddledBabySet().MeshAt(facing);
			Material material = this.graphics.SwaddledBabyMatAt(facing, flags.FlagSet(PawnRenderFlags.Portrait), flags.FlagSet(PawnRenderFlags.Cache));
			if (material != null)
			{
				GenDraw.DrawMeshNowOrLater(mesh, shellLoc, quat, material, flags.FlagSet(PawnRenderFlags.DrawNow));
			}
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x00075CBC File Offset: 0x00073EBC
		private void DrawHeadHair(Vector3 rootLoc, Vector3 headOffset, float angle, Rot4 bodyFacing, Rot4 headFacing, RotDrawMode bodyDrawType, PawnRenderFlags flags, bool bodyDrawn)
		{
			PawnRenderer.<>c__DisplayClass54_0 CS$<>8__locals1 = new PawnRenderer.<>c__DisplayClass54_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.headFacing = headFacing;
			CS$<>8__locals1.bodyFacing = bodyFacing;
			CS$<>8__locals1.flags = flags;
			CS$<>8__locals1.rootLoc = rootLoc;
			CS$<>8__locals1.headOffset = headOffset;
			CS$<>8__locals1.bodyDrawType = bodyDrawType;
			if (this.ShellFullyCoversHead(CS$<>8__locals1.flags) && bodyDrawn)
			{
				return;
			}
			CS$<>8__locals1.onHeadLoc = CS$<>8__locals1.rootLoc + CS$<>8__locals1.headOffset;
			PawnRenderer.<>c__DisplayClass54_0 CS$<>8__locals2 = CS$<>8__locals1;
			CS$<>8__locals2.onHeadLoc.y = CS$<>8__locals2.onHeadLoc.y + 0.028957527f;
			List<ApparelGraphicRecord> apparelGraphics = this.graphics.apparelGraphics;
			CS$<>8__locals1.geneGraphics = this.graphics.geneGraphics;
			CS$<>8__locals1.quat = Quaternion.AngleAxis(angle, Vector3.up);
			bool flag = this.pawn.DevelopmentalStage.Baby() || CS$<>8__locals1.bodyDrawType == RotDrawMode.Dessicated || CS$<>8__locals1.flags.FlagSet(PawnRenderFlags.HeadStump);
			bool flag2;
			if (flag)
			{
				Pawn_StoryTracker story = this.pawn.story;
				if (((story != null) ? story.hairDef : null) != null)
				{
					flag2 = this.pawn.story.hairDef.noGraphic;
					goto IL_104;
				}
			}
			flag2 = true;
			IL_104:
			bool flag3 = flag2;
			bool flag4;
			if (!flag && CS$<>8__locals1.bodyFacing != Rot4.North && this.pawn.DevelopmentalStage.Adult())
			{
				Pawn_StyleTracker style = this.pawn.style;
				flag4 = ((((style != null) ? style.beardDef : null) ?? BeardDefOf.NoBeard) != BeardDefOf.NoBeard);
			}
			else
			{
				flag4 = false;
			}
			bool flag5 = flag4;
			CS$<>8__locals1.allFaceCovered = false;
			CS$<>8__locals1.drawEyes = true;
			CS$<>8__locals1.middleFaceCovered = false;
			bool flag6 = this.pawn.CurrentBed() != null && !this.pawn.CurrentBed().def.building.bed_showSleeperBody;
			bool flag7 = !CS$<>8__locals1.flags.FlagSet(PawnRenderFlags.Portrait) && flag6;
			bool flag8 = CS$<>8__locals1.flags.FlagSet(PawnRenderFlags.Headgear) && (!CS$<>8__locals1.flags.FlagSet(PawnRenderFlags.Portrait) || !Prefs.HatsOnlyOnMap || CS$<>8__locals1.flags.FlagSet(PawnRenderFlags.StylingStation));
			if (this.leftEyeCached == null)
			{
				this.leftEyeCached = this.pawn.def.race.body.AllParts.FirstOrDefault((BodyPartRecord p) => p.woundAnchorTag == "LeftEye");
			}
			if (this.rightEyeCached == null)
			{
				this.rightEyeCached = this.pawn.def.race.body.AllParts.FirstOrDefault((BodyPartRecord p) => p.woundAnchorTag == "RightEye");
			}
			CS$<>8__locals1.hasLeftEye = (this.leftEyeCached != null && !this.pawn.health.hediffSet.PartIsMissing(this.leftEyeCached));
			CS$<>8__locals1.hasRightEye = (this.rightEyeCached != null && !this.pawn.health.hediffSet.PartIsMissing(this.rightEyeCached));
			if (flag8)
			{
				for (int i = 0; i < apparelGraphics.Count; i++)
				{
					if ((!flag7 || apparelGraphics[i].sourceApparel.def.apparel.hatRenderedFrontOfFace) && (apparelGraphics[i].sourceApparel.def.apparel.LastLayer == ApparelLayerDefOf.Overhead || apparelGraphics[i].sourceApparel.def.apparel.LastLayer == ApparelLayerDefOf.EyeCover))
					{
						if (apparelGraphics[i].sourceApparel.def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.FullHead))
						{
							flag5 = false;
							CS$<>8__locals1.allFaceCovered = true;
							if (!apparelGraphics[i].sourceApparel.def.apparel.forceEyesVisibleForRotations.Contains(CS$<>8__locals1.headFacing.AsInt))
							{
								CS$<>8__locals1.drawEyes = false;
							}
						}
						if (!apparelGraphics[i].sourceApparel.def.apparel.hatRenderedFrontOfFace && !apparelGraphics[i].sourceApparel.def.apparel.forceRenderUnderHair)
						{
							flag3 = false;
						}
						if (apparelGraphics[i].sourceApparel.def.apparel.coversHeadMiddle)
						{
							CS$<>8__locals1.middleFaceCovered = true;
						}
					}
				}
			}
			CS$<>8__locals1.<DrawHeadHair>g__TryDrawGenes|3(GeneDrawLayer.PostSkin);
			if (ModsConfig.IdeologyActive && this.graphics.faceTattooGraphic != null && CS$<>8__locals1.bodyDrawType != RotDrawMode.Dessicated && !CS$<>8__locals1.flags.FlagSet(PawnRenderFlags.HeadStump) && (CS$<>8__locals1.bodyFacing != Rot4.North || this.pawn.style.FaceTattoo.visibleNorth))
			{
				Vector3 loc = CS$<>8__locals1.rootLoc + CS$<>8__locals1.headOffset;
				loc.y += 0.023166021f;
				if (CS$<>8__locals1.bodyFacing == Rot4.North)
				{
					loc.y -= 0.001f;
				}
				else
				{
					loc.y += 0.001f;
				}
				GenDraw.DrawMeshNowOrLater(this.graphics.HairMeshSet.MeshAt(CS$<>8__locals1.headFacing), loc, CS$<>8__locals1.quat, this.graphics.faceTattooGraphic.MatAt(CS$<>8__locals1.headFacing, null), CS$<>8__locals1.flags.FlagSet(PawnRenderFlags.DrawNow));
			}
			CS$<>8__locals1.<DrawHeadHair>g__TryDrawGenes|3(GeneDrawLayer.PostTattoo);
			if (CS$<>8__locals1.headFacing != Rot4.North && (!CS$<>8__locals1.allFaceCovered | CS$<>8__locals1.drawEyes))
			{
				foreach (Hediff hediff in this.pawn.health.hediffSet.hediffs)
				{
					if (hediff.def.eyeGraphicSouth != null && hediff.def.eyeGraphicEast != null)
					{
						GraphicData graphicData = CS$<>8__locals1.headFacing.IsHorizontal ? hediff.def.eyeGraphicEast : hediff.def.eyeGraphicSouth;
						bool flag9 = hediff.Part.woundAnchorTag == "LeftEye";
						CS$<>8__locals1.<DrawHeadHair>g__DrawExtraEyeGraphic|6(graphicData.Graphic, hediff.def.eyeGraphicScale * this.pawn.ageTracker.CurLifeStage.eyeSizeFactor.GetValueOrDefault(1f), 0.0014f, flag9, !flag9);
					}
				}
			}
			if (flag5)
			{
				Vector3 loc2 = this.OffsetBeardLocationForHead(this.pawn.style.beardDef, this.pawn.story.headType, CS$<>8__locals1.headFacing, CS$<>8__locals1.rootLoc + CS$<>8__locals1.headOffset);
				Mesh mesh = this.graphics.BeardMeshSet.MeshAt(CS$<>8__locals1.headFacing);
				Material material = this.graphics.BeardMatAt(CS$<>8__locals1.headFacing, CS$<>8__locals1.flags.FlagSet(PawnRenderFlags.Portrait), CS$<>8__locals1.flags.FlagSet(PawnRenderFlags.Cache));
				if (material != null)
				{
					GenDraw.DrawMeshNowOrLater(mesh, loc2, CS$<>8__locals1.quat, material, CS$<>8__locals1.flags.FlagSet(PawnRenderFlags.DrawNow));
				}
			}
			if (flag8)
			{
				for (int j = 0; j < apparelGraphics.Count; j++)
				{
					if ((!flag7 || apparelGraphics[j].sourceApparel.def.apparel.hatRenderedFrontOfFace) && apparelGraphics[j].sourceApparel.def.apparel.forceRenderUnderHair)
					{
						CS$<>8__locals1.<DrawHeadHair>g__DrawApparel|2(apparelGraphics[j]);
					}
				}
			}
			if (flag3)
			{
				Mesh mesh2 = this.graphics.HairMeshSet.MeshAt(CS$<>8__locals1.headFacing);
				Material material2 = this.graphics.HairMatAt(CS$<>8__locals1.headFacing, CS$<>8__locals1.flags.FlagSet(PawnRenderFlags.Portrait), CS$<>8__locals1.flags.FlagSet(PawnRenderFlags.Cache));
				if (material2 != null)
				{
					GenDraw.DrawMeshNowOrLater(mesh2, CS$<>8__locals1.onHeadLoc, CS$<>8__locals1.quat, material2, CS$<>8__locals1.flags.FlagSet(PawnRenderFlags.DrawNow));
				}
			}
			CS$<>8__locals1.<DrawHeadHair>g__TryDrawGenes|3(GeneDrawLayer.PostHair);
			if (flag8)
			{
				for (int k = 0; k < apparelGraphics.Count; k++)
				{
					if ((!flag7 || apparelGraphics[k].sourceApparel.def.apparel.hatRenderedFrontOfFace) && (apparelGraphics[k].sourceApparel.def.apparel.LastLayer == ApparelLayerDefOf.Overhead || apparelGraphics[k].sourceApparel.def.apparel.LastLayer == ApparelLayerDefOf.EyeCover) && !apparelGraphics[k].sourceApparel.def.apparel.forceRenderUnderHair)
					{
						CS$<>8__locals1.<DrawHeadHair>g__DrawApparel|2(apparelGraphics[k]);
					}
				}
			}
			CS$<>8__locals1.<DrawHeadHair>g__TryDrawGenes|3(GeneDrawLayer.PostHeadgear);
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x00076588 File Offset: 0x00074788
		private bool ShellFullyCoversHead(PawnRenderFlags flags)
		{
			if (!flags.FlagSet(PawnRenderFlags.Clothes))
			{
				return false;
			}
			List<ApparelGraphicRecord> apparelGraphics = this.graphics.apparelGraphics;
			for (int i = 0; i < apparelGraphics.Count; i++)
			{
				if (apparelGraphics[i].sourceApparel.def.apparel.LastLayer == ApparelLayerDefOf.Shell && apparelGraphics[i].sourceApparel.def.apparel.shellCoversHead)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x00076600 File Offset: 0x00074800
		private Vector3 OffsetBeardLocationForHead(BeardDef beardDef, HeadTypeDef head, Rot4 headFacing, Vector3 beardLoc)
		{
			if (headFacing == Rot4.East)
			{
				beardLoc += Vector3.right * head.beardOffsetXEast;
			}
			else if (headFacing == Rot4.West)
			{
				beardLoc += Vector3.left * head.beardOffsetXEast;
			}
			beardLoc.y += 0.026061773f;
			beardLoc += head.beardOffset;
			beardLoc += this.pawn.style.beardDef.GetOffset(this.pawn.story.headType, headFacing);
			return beardLoc;
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x000766AC File Offset: 0x000748AC
		private Vector3 HeadGeneDrawLocation(GeneDef geneDef, HeadTypeDef head, Rot4 headFacing, Vector3 geneLoc, GeneDrawLayer layer)
		{
			if (layer != GeneDrawLayer.PostSkin)
			{
				if (layer - GeneDrawLayer.PostHair <= 1)
				{
					geneLoc.y += 0.03335328f;
				}
				else
				{
					geneLoc.y += 0.028957527f;
				}
			}
			else
			{
				geneLoc.y += 0.026061773f;
			}
			geneLoc += geneDef.graphicData.DrawOffsetAt(headFacing);
			float narrowCrownHorizontalOffset = geneDef.graphicData.narrowCrownHorizontalOffset;
			if (narrowCrownHorizontalOffset != 0f && head.narrow && headFacing.IsHorizontal)
			{
				if (headFacing == Rot4.East)
				{
					geneLoc += Vector3.right * -narrowCrownHorizontalOffset;
				}
				else if (headFacing == Rot4.West)
				{
					geneLoc += Vector3.right * narrowCrownHorizontalOffset;
				}
				geneLoc += Vector3.forward * -narrowCrownHorizontalOffset;
			}
			return geneLoc;
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x00076790 File Offset: 0x00074990
		private void DrawBodyGenes(Vector3 rootLoc, Quaternion quat, float angle, Rot4 bodyFacing, RotDrawMode bodyDrawType, PawnRenderFlags flags)
		{
			Vector2 bodyGraphicScale = this.pawn.story.bodyType.bodyGraphicScale;
			float num = (bodyGraphicScale.x + bodyGraphicScale.y) / 2f;
			foreach (GeneGraphicRecord geneGraphicRecord in this.graphics.geneGraphics)
			{
				GeneGraphicData graphicData = geneGraphicRecord.sourceGene.def.graphicData;
				if (graphicData.drawLoc == GeneDrawLoc.Tailbone && (bodyDrawType != RotDrawMode.Dessicated || geneGraphicRecord.sourceGene.def.graphicData.drawWhileDessicated))
				{
					Vector3 v = graphicData.DrawOffsetAt(bodyFacing);
					v.x *= bodyGraphicScale.x;
					v.z *= bodyGraphicScale.y;
					Vector3 s = new Vector3(graphicData.drawScale * num, 1f, graphicData.drawScale * num);
					Matrix4x4 matrix = Matrix4x4.TRS(rootLoc + v.RotatedBy(angle), quat, s);
					Material material = geneGraphicRecord.graphic.MatAt(bodyFacing, null);
					material = (flags.FlagSet(PawnRenderFlags.Cache) ? material : this.OverrideMaterialIfNeeded(material, this.pawn, flags.FlagSet(PawnRenderFlags.Portrait)));
					GenDraw.DrawMeshNowOrLater((bodyFacing == Rot4.West) ? MeshPool.GridPlaneFlip(Vector2.one) : MeshPool.GridPlane(Vector2.one), matrix, material, flags.FlagSet(PawnRenderFlags.DrawNow));
				}
			}
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x0007692C File Offset: 0x00074B2C
		private void DrawBodyApparel(Vector3 shellLoc, Vector3 utilityLoc, Mesh bodyMesh, float angle, Rot4 bodyFacing, PawnRenderFlags flags)
		{
			List<ApparelGraphicRecord> apparelGraphics = this.graphics.apparelGraphics;
			Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.up);
			for (int i = 0; i < apparelGraphics.Count; i++)
			{
				ApparelGraphicRecord apparelGraphicRecord = apparelGraphics[i];
				if (apparelGraphicRecord.sourceApparel.def.apparel.LastLayer == ApparelLayerDefOf.Shell && !apparelGraphicRecord.sourceApparel.def.apparel.shellRenderedBehindHead)
				{
					Material material = apparelGraphicRecord.graphic.MatAt(bodyFacing, null);
					material = (flags.FlagSet(PawnRenderFlags.Cache) ? material : this.OverrideMaterialIfNeeded(material, this.pawn, flags.FlagSet(PawnRenderFlags.Portrait)));
					Vector3 loc = shellLoc;
					if (apparelGraphicRecord.sourceApparel.def.apparel.shellCoversHead)
					{
						loc.y += 0.0028957527f;
					}
					GenDraw.DrawMeshNowOrLater(bodyMesh, loc, quaternion, material, flags.FlagSet(PawnRenderFlags.DrawNow));
				}
				if (PawnRenderer.RenderAsPack(apparelGraphicRecord.sourceApparel))
				{
					Material material2 = apparelGraphicRecord.graphic.MatAt(bodyFacing, null);
					material2 = (flags.FlagSet(PawnRenderFlags.Cache) ? material2 : this.OverrideMaterialIfNeeded(material2, this.pawn, flags.FlagSet(PawnRenderFlags.Portrait)));
					if (apparelGraphicRecord.sourceApparel.def.apparel.wornGraphicData != null)
					{
						Vector2 vector = apparelGraphicRecord.sourceApparel.def.apparel.wornGraphicData.BeltOffsetAt(bodyFacing, this.pawn.story.bodyType);
						Vector2 vector2 = apparelGraphicRecord.sourceApparel.def.apparel.wornGraphicData.BeltScaleAt(bodyFacing, this.pawn.story.bodyType);
						Matrix4x4 matrix = Matrix4x4.Translate(utilityLoc) * Matrix4x4.Rotate(quaternion) * Matrix4x4.Translate(new Vector3(vector.x, 0f, vector.y)) * Matrix4x4.Scale(new Vector3(vector2.x, 1f, vector2.y));
						GenDraw.DrawMeshNowOrLater(bodyMesh, matrix, material2, flags.FlagSet(PawnRenderFlags.DrawNow));
					}
					else
					{
						GenDraw.DrawMeshNowOrLater(bodyMesh, shellLoc, quaternion, material2, flags.FlagSet(PawnRenderFlags.DrawNow));
					}
				}
			}
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x00076B58 File Offset: 0x00074D58
		private void DrawDynamicParts(Vector3 rootLoc, float angle, Rot4 pawnRotation, PawnRenderFlags flags)
		{
			Quaternion quat = Quaternion.AngleAxis(angle, Vector3.up);
			this.DrawEquipment(rootLoc, pawnRotation, flags);
			if (this.pawn.apparel != null)
			{
				List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					wornApparel[i].DrawWornExtras();
				}
			}
			Vector3 bodyLoc = rootLoc;
			bodyLoc.y += 0.037644785f;
			this.statusOverlays.RenderStatusOverlays(bodyLoc, quat, HumanlikeMeshPoolUtility.GetHumanlikeHeadSetForPawn(this.pawn).MeshAt(pawnRotation));
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x00076BE8 File Offset: 0x00074DE8
		private void DrawEquipment(Vector3 rootLoc, Rot4 pawnRotation, PawnRenderFlags flags)
		{
			if (this.pawn.Dead || !this.pawn.Spawned)
			{
				return;
			}
			if (this.pawn.equipment == null || this.pawn.equipment.Primary == null)
			{
				return;
			}
			if (this.pawn.CurJob != null && this.pawn.CurJob.def.neverShowWeapon)
			{
				return;
			}
			Vector3 vector = new Vector3(0f, (pawnRotation == Rot4.North) ? -0.0028957527f : 0.03474903f, 0f);
			Stance_Busy stance_Busy = this.pawn.stances.curStance as Stance_Busy;
			float equipmentDrawDistanceFactor = this.pawn.ageTracker.CurLifeStage.equipmentDrawDistanceFactor;
			if (stance_Busy != null && !stance_Busy.neverAimWeapon && stance_Busy.focusTarg.IsValid && (flags & PawnRenderFlags.NeverAimWeapon) == PawnRenderFlags.None)
			{
				Vector3 a;
				if (stance_Busy.focusTarg.HasThing)
				{
					a = stance_Busy.focusTarg.Thing.DrawPos;
				}
				else
				{
					a = stance_Busy.focusTarg.Cell.ToVector3Shifted();
				}
				float num = 0f;
				if ((a - this.pawn.DrawPos).MagnitudeHorizontalSquared() > 0.001f)
				{
					num = (a - this.pawn.DrawPos).AngleFlat();
				}
				Verb currentEffectiveVerb = this.pawn.CurrentEffectiveVerb;
				if (currentEffectiveVerb != null && currentEffectiveVerb.AimAngleOverride != null)
				{
					num = currentEffectiveVerb.AimAngleOverride.Value;
				}
				vector += rootLoc + new Vector3(0f, 0f, 0.4f + this.pawn.equipment.Primary.def.equippedDistanceOffset).RotatedBy(num) * equipmentDrawDistanceFactor;
				this.DrawEquipmentAiming(this.pawn.equipment.Primary, vector, num);
				return;
			}
			if (this.CarryWeaponOpenly())
			{
				if (pawnRotation == Rot4.South)
				{
					vector += rootLoc + new Vector3(0f, 0f, -0.22f) * equipmentDrawDistanceFactor;
					this.DrawEquipmentAiming(this.pawn.equipment.Primary, vector, 143f);
					return;
				}
				if (pawnRotation == Rot4.North)
				{
					vector += rootLoc + new Vector3(0f, 0f, -0.11f) * equipmentDrawDistanceFactor;
					this.DrawEquipmentAiming(this.pawn.equipment.Primary, vector, 143f);
					return;
				}
				if (pawnRotation == Rot4.East)
				{
					vector += rootLoc + new Vector3(0.2f, 0f, -0.22f) * equipmentDrawDistanceFactor;
					this.DrawEquipmentAiming(this.pawn.equipment.Primary, vector, 143f);
					return;
				}
				if (pawnRotation == Rot4.West)
				{
					vector += rootLoc + new Vector3(-0.2f, 0f, -0.22f) * equipmentDrawDistanceFactor;
					this.DrawEquipmentAiming(this.pawn.equipment.Primary, vector, 217f);
				}
			}
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x00076F30 File Offset: 0x00075130
		public void DrawEquipmentAiming(Thing eq, Vector3 drawLoc, float aimAngle)
		{
			float num = aimAngle - 90f;
			Mesh mesh;
			if (aimAngle > 20f && aimAngle < 160f)
			{
				mesh = MeshPool.plane10;
				num += eq.def.equippedAngleOffset;
			}
			else if (aimAngle > 200f && aimAngle < 340f)
			{
				mesh = MeshPool.plane10Flip;
				num -= 180f;
				num -= eq.def.equippedAngleOffset;
			}
			else
			{
				mesh = MeshPool.plane10;
				num += eq.def.equippedAngleOffset;
			}
			num %= 360f;
			CompEquippable compEquippable = eq.TryGetComp<CompEquippable>();
			if (compEquippable != null)
			{
				Vector3 b;
				float num2;
				EquipmentUtility.Recoil(eq.def, EquipmentUtility.GetRecoilVerb(compEquippable.AllVerbs), out b, out num2, aimAngle);
				drawLoc += b;
				num += num2;
			}
			Graphic_StackCount graphic_StackCount = eq.Graphic as Graphic_StackCount;
			Material material;
			if (graphic_StackCount != null)
			{
				material = graphic_StackCount.SubGraphicForStackCount(1, eq.def).MatSingleFor(eq);
			}
			else
			{
				material = eq.Graphic.MatSingleFor(eq);
			}
			Vector3 s = new Vector3(eq.Graphic.drawSize.x, 0f, eq.Graphic.drawSize.y);
			Matrix4x4 matrix = Matrix4x4.TRS(drawLoc, Quaternion.AngleAxis(num, Vector3.up), s);
			Graphics.DrawMesh(mesh, matrix, material, 0);
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x00077070 File Offset: 0x00075270
		private Material OverrideMaterialIfNeeded(Material original, Pawn pawn, bool portrait = false)
		{
			Material baseMat = (!portrait && pawn.IsInvisible()) ? InvisibilityMatPool.GetInvisibleMat(original) : original;
			return this.graphics.flasher.GetDamagedMat(baseMat);
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x000770A8 File Offset: 0x000752A8
		private bool CarryWeaponOpenly()
		{
			if (this.pawn.carryTracker != null && this.pawn.carryTracker.CarriedThing != null)
			{
				return false;
			}
			if (this.pawn.Drafted)
			{
				return true;
			}
			if (this.pawn.CurJob != null && this.pawn.CurJob.def.alwaysShowWeapon)
			{
				return true;
			}
			if (this.pawn.mindState.duty != null && this.pawn.mindState.duty.def.alwaysShowWeapon)
			{
				return true;
			}
			Lord lord = this.pawn.GetLord();
			return lord != null && lord.LordJob != null && lord.LordJob.AlwaysShowWeapon;
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x00077164 File Offset: 0x00075364
		private Rot4 RotationForcedByJob()
		{
			if (this.pawn.jobs != null && this.pawn.jobs.curDriver != null && this.pawn.jobs.curDriver.ForcedLayingRotation.IsValid)
			{
				return this.pawn.jobs.curDriver.ForcedLayingRotation;
			}
			return Rot4.Invalid;
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x000771CC File Offset: 0x000753CC
		public Rot4 LayingFacing()
		{
			Rot4 result = this.RotationForcedByJob();
			if (result.IsValid)
			{
				return result;
			}
			PawnPosture posture = this.pawn.GetPosture();
			if (posture == PawnPosture.LayingOnGroundFaceUp || this.pawn.Deathresting)
			{
				return Rot4.South;
			}
			if (this.pawn.RaceProps.Humanlike)
			{
				Pawn_CarryTracker pawn_CarryTracker;
				if (this.pawn.DevelopmentalStage.Baby() && (pawn_CarryTracker = (this.pawn.ParentHolder as Pawn_CarryTracker)) != null)
				{
					if (!(pawn_CarryTracker.pawn.Rotation == Rot4.West))
					{
						return Rot4.West;
					}
					return Rot4.East;
				}
				else
				{
					if (posture.FaceUp() && this.pawn.CurrentBed() != null)
					{
						return Rot4.South;
					}
					switch (this.pawn.thingIDNumber % 4)
					{
					case 0:
						return Rot4.South;
					case 1:
						return Rot4.South;
					case 2:
						return Rot4.East;
					case 3:
						return Rot4.West;
					}
				}
			}
			else
			{
				switch (this.pawn.thingIDNumber % 4)
				{
				case 0:
					return Rot4.South;
				case 1:
					return Rot4.East;
				case 2:
					return Rot4.West;
				case 3:
					return Rot4.West;
				}
			}
			return Rot4.Random;
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x00077308 File Offset: 0x00075508
		public float BodyAngle()
		{
			if (this.pawn.GetPosture() == PawnPosture.Standing)
			{
				return 0f;
			}
			Building_Bed building_Bed = this.pawn.CurrentBed();
			if (building_Bed != null && this.pawn.RaceProps.Humanlike)
			{
				Rot4 rotation = building_Bed.Rotation;
				rotation.AsInt += 2;
				return rotation.AsAngle;
			}
			IThingHolderWithDrawnPawn thingHolderWithDrawnPawn;
			if ((thingHolderWithDrawnPawn = (this.pawn.ParentHolder as IThingHolderWithDrawnPawn)) != null)
			{
				return thingHolderWithDrawnPawn.HeldPawnBodyAngle;
			}
			Pawn_CarryTracker pawn_CarryTracker;
			if (this.pawn.RaceProps.Humanlike && this.pawn.DevelopmentalStage.Baby() && (pawn_CarryTracker = (this.pawn.ParentHolder as Pawn_CarryTracker)) != null)
			{
				return ((pawn_CarryTracker.pawn.Rotation == Rot4.West) ? 290f : 70f) + pawn_CarryTracker.pawn.Drawer.renderer.BodyAngle();
			}
			if (this.pawn.Downed || this.pawn.Dead)
			{
				return this.wiggler.downedAngle;
			}
			if (this.pawn.RaceProps.Humanlike)
			{
				return this.LayingFacing().AsAngle;
			}
			if (this.RotationForcedByJob().IsValid)
			{
				return 0f;
			}
			Rot4 rot = Rot4.West;
			int num = this.pawn.thingIDNumber % 2;
			if (num != 0)
			{
				if (num == 1)
				{
					rot = Rot4.East;
				}
			}
			else
			{
				rot = Rot4.West;
			}
			return rot.AsAngle;
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x0007748C File Offset: 0x0007568C
		public Vector3 BaseHeadOffsetAt(Rot4 rotation)
		{
			Vector2 vector = this.pawn.story.bodyType.headOffset * Mathf.Sqrt(this.pawn.ageTracker.CurLifeStage.bodySizeFactor);
			switch (rotation.AsInt)
			{
			case 0:
				return new Vector3(0f, 0f, vector.y);
			case 1:
				return new Vector3(vector.x, 0f, vector.y);
			case 2:
				return new Vector3(0f, 0f, vector.y);
			case 3:
				return new Vector3(-vector.x, 0f, vector.y);
			default:
				Log.Error("BaseHeadOffsetAt error in " + this.pawn);
				return Vector3.zero;
			}
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x00077563 File Offset: 0x00075763
		public void Notify_DamageApplied(DamageInfo dam)
		{
			this.graphics.flasher.Notify_DamageApplied(dam);
			this.wiggler.Notify_DamageApplied(dam);
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x00077582 File Offset: 0x00075782
		public void ProcessPostTickVisuals(int ticksPassed)
		{
			this.wiggler.ProcessPostTickVisuals(ticksPassed);
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x00077590 File Offset: 0x00075790
		public void EffectersTick(bool suspended)
		{
			this.effecters.EffectersTick(suspended);
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x000775A0 File Offset: 0x000757A0
		public static bool RenderAsPack(Apparel apparel)
		{
			return apparel.def.apparel.LastLayer.IsUtilityLayer && (apparel.def.apparel.wornGraphicData == null || apparel.def.apparel.wornGraphicData.renderUtilityAsPack);
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x000775F0 File Offset: 0x000757F0
		private void DrawDebug()
		{
			if (DebugViewSettings.drawDuties && Find.Selector.IsSelected(this.pawn) && this.pawn.mindState != null && this.pawn.mindState.duty != null)
			{
				this.pawn.mindState.duty.DrawDebug(this.pawn);
			}
		}

		// Token: 0x04001020 RID: 4128
		private Pawn pawn;

		// Token: 0x04001021 RID: 4129
		public PawnGraphicSet graphics;

		// Token: 0x04001022 RID: 4130
		public PawnDownedWiggler wiggler;

		// Token: 0x04001023 RID: 4131
		private PawnHeadOverlays statusOverlays;

		// Token: 0x04001024 RID: 4132
		private PawnStatusEffecters effecters;

		// Token: 0x04001025 RID: 4133
		private PawnWoundDrawer woundOverlays;

		// Token: 0x04001026 RID: 4134
		private PawnFirefoamDrawer firefoamOverlays;

		// Token: 0x04001027 RID: 4135
		private Graphic_Shadow shadowGraphic;

		// Token: 0x04001028 RID: 4136
		private BodyPartRecord leftEyeCached;

		// Token: 0x04001029 RID: 4137
		private BodyPartRecord rightEyeCached;

		// Token: 0x0400102A RID: 4138
		private const float CarriedThingDrawAngle = 16f;

		// Token: 0x0400102B RID: 4139
		private const float CarriedBabyDrawAngle = 70f;

		// Token: 0x0400102C RID: 4140
		private const float SubInterval = 0.0028957527f;

		// Token: 0x0400102D RID: 4141
		private const float YOffset_PrimaryEquipmentUnder = -0.0028957527f;

		// Token: 0x0400102E RID: 4142
		private const float YOffset_CarriedThingUnder = -0.0028957527f;

		// Token: 0x0400102F RID: 4143
		private const float YOffset_Behind = 0.0028957527f;

		// Token: 0x04001030 RID: 4144
		private const float YOffset_Utility_South = 0.0057915053f;

		// Token: 0x04001031 RID: 4145
		private const float YOffset_Body = 0.008687258f;

		// Token: 0x04001032 RID: 4146
		private const float YOffsetInterval_Clothes = 0.0028957527f;

		// Token: 0x04001033 RID: 4147
		private const float YOffset_Shell = 0.02027027f;

		// Token: 0x04001034 RID: 4148
		private const float YOffset_Head = 0.023166021f;

		// Token: 0x04001035 RID: 4149
		private const float YOffset_OnHeadDirect = 0.026061773f;

		// Token: 0x04001036 RID: 4150
		private const float YOffset_OnHead = 0.028957527f;

		// Token: 0x04001037 RID: 4151
		private const float YOffset_Utility = 0.028957527f;

		// Token: 0x04001038 RID: 4152
		private const float YOffset_PostHead = 0.03185328f;

		// Token: 0x04001039 RID: 4153
		private const float YOffset_CoveredInOverlay = 0.033301156f;

		// Token: 0x0400103A RID: 4154
		private const float YOffset_CarriedThing = 0.03474903f;

		// Token: 0x0400103B RID: 4155
		private const float YOffset_PrimaryEquipmentOver = 0.03474903f;

		// Token: 0x0400103C RID: 4156
		private const float YOffset_Status = 0.037644785f;

		// Token: 0x0400103D RID: 4157
		private const float CachedPawnTextureMinCameraZoom = 18f;

		// Token: 0x0400103E RID: 4158
		private const string LeftEyeWoundAnchorTag = "LeftEye";

		// Token: 0x0400103F RID: 4159
		private const string RightEyeWoundAnchorTag = "RightEye";

		// Token: 0x04001040 RID: 4160
		private static Dictionary<Apparel, ValueTuple<Color, bool>> tmpOriginalColors = new Dictionary<Apparel, ValueTuple<Color, bool>>();
	}
}
