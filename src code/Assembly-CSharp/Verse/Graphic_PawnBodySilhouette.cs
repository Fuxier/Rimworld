using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003E7 RID: 999
	public class Graphic_PawnBodySilhouette : Graphic_Mote
	{
		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06001C70 RID: 7280 RVA: 0x00002662 File Offset: 0x00000862
		protected override bool ForcePropertyBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x000AD748 File Offset: 0x000AB948
		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			this.path = req.path;
			this.maskPath = req.maskPath;
			this.color = req.color;
			this.colorTwo = req.colorTwo;
			this.drawSize = req.drawSize;
			this.request = req;
		}

		// Token: 0x06001C72 RID: 7282 RVA: 0x000AD7A4 File Offset: 0x000AB9A4
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Mote mote = (Mote)thing;
			Color color = this.color;
			color.a *= mote.Alpha;
			Corpse corpse = mote.link1.Target.Thing as Corpse;
			Pawn pawn = mote.link1.Target.Thing as Pawn;
			Pawn pawn2 = ((corpse != null) ? corpse.InnerPawn : null) ?? pawn;
			if (pawn2 == null)
			{
				pawn2 = this.lastPawn;
			}
			PawnRenderer renderer = pawn2.Drawer.renderer;
			if (!renderer.graphics.AllResolved)
			{
				return;
			}
			Rot4 rot2 = (pawn2.GetPosture() == PawnPosture.Standing) ? pawn2.Rotation : renderer.LayingFacing();
			Vector3 vector = pawn2.DrawPos;
			Building_Bed building_Bed = pawn2.CurrentBed();
			if (building_Bed != null)
			{
				Rot4 rotation = building_Bed.Rotation;
				rotation.AsInt += 2;
				vector -= rotation.FacingCell.ToVector3() * (pawn2.story.bodyType.bedOffset + pawn2.Drawer.renderer.BaseHeadOffsetAt(Rot4.South).z);
			}
			bool posture = pawn2.GetPosture() != PawnPosture.Standing;
			vector.y = mote.def.Altitude;
			if (this.lastPawn != pawn2 || this.lastFacing != rot2)
			{
				this.bodyMaterial = this.MakeMatFrom(this.request, renderer.graphics.MatsBodyBaseAt(rot2, pawn2.Dead, RotDrawMode.Fresh, false)[0].mainTexture);
			}
			Mesh mesh;
			if (pawn2.RaceProps.Humanlike)
			{
				mesh = HumanlikeMeshPoolUtility.GetHumanlikeBodySetForPawn(pawn2).MeshAt(rot2);
			}
			else
			{
				mesh = renderer.graphics.nakedGraphic.MeshAt(rot2);
			}
			this.bodyMaterial.SetVector("_pawnCenterWorld", new Vector4(vector.x, vector.z, 0f, 0f));
			this.bodyMaterial.SetVector("_pawnDrawSizeWorld", new Vector4(mesh.bounds.size.x, mesh.bounds.size.z, 0f, 0f));
			this.bodyMaterial.SetFloat(ShaderPropertyIDs.AgeSecs, mote.AgeSecs);
			this.bodyMaterial.SetColor(ShaderPropertyIDs.Color, color);
			Quaternion quaternion = Quaternion.AngleAxis((!posture) ? 0f : renderer.BodyAngle(), Vector3.up);
			if (building_Bed == null || building_Bed.def.building.bed_showSleeperBody)
			{
				GenDraw.DrawMeshNowOrLater(mesh, vector, quaternion, this.bodyMaterial, false);
			}
			if (pawn2.RaceProps.Humanlike)
			{
				if (this.lastPawn != pawn2 || this.lastFacing != rot2)
				{
					this.headMaterial = this.MakeMatFrom(this.request, renderer.graphics.headGraphic.MatAt(rot2, null).mainTexture);
				}
				Vector3 b = quaternion * renderer.BaseHeadOffsetAt(rot2) + new Vector3(0f, 0.001f, 0f);
				Mesh mesh2 = HumanlikeMeshPoolUtility.GetHumanlikeHeadSetForPawn(pawn2).MeshAt(rot2);
				this.headMaterial.SetVector("_pawnCenterWorld", new Vector4(vector.x, vector.z, 0f, 0f));
				this.headMaterial.SetVector("_pawnDrawSizeWorld", new Vector4(mesh2.bounds.size.x, mesh.bounds.size.z, 0f, 0f));
				this.headMaterial.SetFloat(ShaderPropertyIDs.AgeSecs, mote.AgeSecs);
				this.headMaterial.SetColor(ShaderPropertyIDs.Color, color);
				GenDraw.DrawMeshNowOrLater(mesh2, vector + b, quaternion, this.headMaterial, false);
			}
			if (pawn2 != null)
			{
				this.lastPawn = pawn2;
			}
			this.lastFacing = rot2;
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x000ADB9C File Offset: 0x000ABD9C
		private Material MakeMatFrom(GraphicRequest req, Texture mainTex)
		{
			return MaterialPool.MatFrom(new MaterialRequest
			{
				mainTex = mainTex,
				shader = req.shader,
				color = this.color,
				colorTwo = this.colorTwo,
				renderQueue = req.renderQueue,
				shaderParameters = req.shaderParameters
			});
		}

		// Token: 0x06001C74 RID: 7284 RVA: 0x000ADC00 File Offset: 0x000ABE00
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04001449 RID: 5193
		private GraphicRequest request;

		// Token: 0x0400144A RID: 5194
		private Pawn lastPawn;

		// Token: 0x0400144B RID: 5195
		private Rot4 lastFacing;

		// Token: 0x0400144C RID: 5196
		private Material bodyMaterial;

		// Token: 0x0400144D RID: 5197
		private Material headMaterial;
	}
}
