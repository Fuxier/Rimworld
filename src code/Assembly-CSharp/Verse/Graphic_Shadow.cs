using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003EA RID: 1002
	public class Graphic_Shadow : Graphic
	{
		// Token: 0x06001C8B RID: 7307 RVA: 0x000ADFA6 File Offset: 0x000AC1A6
		public Graphic_Shadow(ShadowData shadowInfo)
		{
			this.shadowInfo = shadowInfo;
			if (shadowInfo == null)
			{
				throw new ArgumentNullException("shadowInfo");
			}
			this.shadowMesh = ShadowMeshPool.GetShadowMesh(shadowInfo);
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x000ADFD0 File Offset: 0x000AC1D0
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			if (this.shadowMesh != null && thingDef != null && this.shadowInfo != null && (Find.CurrentMap == null || !loc.ToIntVec3().InBounds(Find.CurrentMap) || !Find.CurrentMap.roofGrid.Roofed(loc.ToIntVec3())) && DebugViewSettings.drawShadows)
			{
				Vector3 position = loc + this.shadowInfo.offset;
				position.y = AltitudeLayer.Shadows.AltitudeFor();
				Graphics.DrawMesh(this.shadowMesh, position, rot.AsQuat, MatBases.SunShadowFade, 0);
			}
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x000AE068 File Offset: 0x000AC268
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			Vector3 center = thing.TrueCenter() + (this.shadowInfo.offset + new Vector3(Graphic_Shadow.GlobalShadowPosOffsetX, 0f, Graphic_Shadow.GlobalShadowPosOffsetZ)).RotatedBy(thing.Rotation);
			center.y = AltitudeLayer.Shadows.AltitudeFor();
			Printer_Shadow.PrintShadow(layer, center, this.shadowInfo, thing.Rotation);
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x000AE0D1 File Offset: 0x000AC2D1
		public override string ToString()
		{
			return "Graphic_Shadow(" + this.shadowInfo + ")";
		}

		// Token: 0x04001450 RID: 5200
		private Mesh shadowMesh;

		// Token: 0x04001451 RID: 5201
		private ShadowData shadowInfo;

		// Token: 0x04001452 RID: 5202
		[TweakValue("Graphics_Shadow", -5f, 5f)]
		private static float GlobalShadowPosOffsetX;

		// Token: 0x04001453 RID: 5203
		[TweakValue("Graphics_Shadow", -5f, 5f)]
		private static float GlobalShadowPosOffsetZ;
	}
}
