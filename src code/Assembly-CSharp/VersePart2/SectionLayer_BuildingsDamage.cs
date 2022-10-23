using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000209 RID: 521
	public class SectionLayer_BuildingsDamage : SectionLayer
	{
		// Token: 0x06000F0F RID: 3855 RVA: 0x000548E4 File Offset: 0x00052AE4
		public SectionLayer_BuildingsDamage(Section section) : base(section)
		{
			this.relevantChangeTypes = (MapMeshFlag.Buildings | MapMeshFlag.BuildingsDamage);
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x000548F8 File Offset: 0x00052AF8
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			foreach (IntVec3 intVec in this.section.CellRect)
			{
				List<Thing> list = base.Map.thingGrid.ThingsListAt(intVec);
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					Building building = list[i] as Building;
					if (building != null && building.def.useHitPoints && building.HitPoints < building.MaxHitPoints && building.def.drawDamagedOverlay && building.Position.x == intVec.x && building.Position.z == intVec.z)
					{
						this.PrintDamageVisualsFrom(building);
					}
				}
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x000549FC File Offset: 0x00052BFC
		private void PrintDamageVisualsFrom(Building b)
		{
			if (b.def.graphicData != null && b.def.graphicData.damageData != null && !b.def.graphicData.damageData.enabled)
			{
				return;
			}
			this.PrintScratches(b);
			this.PrintCornersAndEdges(b);
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x00054A50 File Offset: 0x00052C50
		private void PrintScratches(Building b)
		{
			int num = 0;
			List<DamageOverlay> overlays = BuildingsDamageSectionLayerUtility.GetOverlays(b);
			for (int i = 0; i < overlays.Count; i++)
			{
				if (overlays[i] == DamageOverlay.Scratch)
				{
					num++;
				}
			}
			if (num == 0)
			{
				return;
			}
			Rect rect = BuildingsDamageSectionLayerUtility.GetDamageRect(b);
			float num2 = Mathf.Min(0.5f * Mathf.Min(rect.width, rect.height), 1f);
			rect = rect.ContractedBy(num2 / 2f);
			if (rect.width <= 0f || rect.height <= 0f)
			{
				return;
			}
			float num3 = Mathf.Max(rect.width, rect.height) * 0.7f;
			SectionLayer_BuildingsDamage.scratches.Clear();
			Rand.PushState();
			Rand.Seed = b.thingIDNumber * 3697;
			for (int j = 0; j < num; j++)
			{
				this.AddScratch(b, rect.width, rect.height, ref num3);
			}
			Rand.PopState();
			float damageTexturesAltitude = this.GetDamageTexturesAltitude(b);
			IList<Material> scratchMats = BuildingsDamageSectionLayerUtility.GetScratchMats(b);
			Rand.PushState();
			Rand.Seed = b.thingIDNumber * 7;
			for (int k = 0; k < SectionLayer_BuildingsDamage.scratches.Count; k++)
			{
				float x = SectionLayer_BuildingsDamage.scratches[k].x;
				float y = SectionLayer_BuildingsDamage.scratches[k].y;
				float rot = Rand.Range(0f, 360f);
				float num4 = num2;
				if (rect.width > 0.95f && rect.height > 0.95f)
				{
					num4 *= Rand.Range(0.85f, 1f);
				}
				Vector3 center = new Vector3(rect.xMin + x, damageTexturesAltitude, rect.yMin + y);
				Material mat = scratchMats.RandomElement<Material>();
				Vector2[] uvs;
				Color32 color;
				Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Building, false, false, out mat, out uvs, out color);
				Printer_Plane.PrintPlane(this, center, new Vector2(num4, num4), mat, rot, false, uvs, null, 0f, 0f);
			}
			Rand.PopState();
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x00054C58 File Offset: 0x00052E58
		private void AddScratch(Building b, float rectWidth, float rectHeight, ref float minDist)
		{
			bool flag = false;
			float num = 0f;
			float num2 = 0f;
			while (!flag)
			{
				for (int i = 0; i < 5; i++)
				{
					num = Rand.Value * rectWidth;
					num2 = Rand.Value * rectHeight;
					float num3 = float.MaxValue;
					for (int j = 0; j < SectionLayer_BuildingsDamage.scratches.Count; j++)
					{
						float num4 = (num - SectionLayer_BuildingsDamage.scratches[j].x) * (num - SectionLayer_BuildingsDamage.scratches[j].x) + (num2 - SectionLayer_BuildingsDamage.scratches[j].y) * (num2 - SectionLayer_BuildingsDamage.scratches[j].y);
						if (num4 < num3)
						{
							num3 = num4;
						}
					}
					if (num3 >= minDist * minDist)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					minDist *= 0.85f;
					if (minDist < 0.001f)
					{
						break;
					}
				}
			}
			if (flag)
			{
				SectionLayer_BuildingsDamage.scratches.Add(new Vector2(num, num2));
			}
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x00054D57 File Offset: 0x00052F57
		private void PrintCornersAndEdges(Building b)
		{
			Rand.PushState();
			Rand.Seed = b.thingIDNumber * 3;
			if (BuildingsDamageSectionLayerUtility.UsesLinkableCornersAndEdges(b))
			{
				this.DrawLinkableCornersAndEdges(b);
			}
			else
			{
				this.DrawFullThingCorners(b);
			}
			Rand.PopState();
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x00054D88 File Offset: 0x00052F88
		private void DrawLinkableCornersAndEdges(Building b)
		{
			if (b.def.graphicData == null)
			{
				return;
			}
			DamageGraphicData damageData = b.def.graphicData.damageData;
			if (damageData == null)
			{
				return;
			}
			float damageTexturesAltitude = this.GetDamageTexturesAltitude(b);
			List<DamageOverlay> overlays = BuildingsDamageSectionLayerUtility.GetOverlays(b);
			IntVec3 position = b.Position;
			Vector3 vector = new Vector3((float)position.x + 0.5f, damageTexturesAltitude, (float)position.z + 0.5f);
			float x = Rand.Range(0.4f, 0.6f);
			float z = Rand.Range(0.4f, 0.6f);
			float x2 = Rand.Range(0.4f, 0.6f);
			float z2 = Rand.Range(0.4f, 0.6f);
			Vector2[] uvs = null;
			for (int i = 0; i < overlays.Count; i++)
			{
				switch (overlays[i])
				{
				case DamageOverlay.TopLeftCorner:
				{
					Material mat = damageData.cornerTLMat;
					Color32 color;
					Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Building, false, false, out mat, out uvs, out color);
					Printer_Plane.PrintPlane(this, vector, Vector2.one, mat, 0f, false, uvs, null, 0f, 0f);
					break;
				}
				case DamageOverlay.TopRightCorner:
				{
					Material mat = damageData.cornerTRMat;
					Color32 color;
					Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Building, false, false, out mat, out uvs, out color);
					Printer_Plane.PrintPlane(this, vector, Vector2.one, mat, 90f, false, uvs, null, 0f, 0f);
					break;
				}
				case DamageOverlay.BotLeftCorner:
				{
					Material mat = damageData.cornerBLMat;
					Color32 color;
					Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Building, false, false, out mat, out uvs, out color);
					Printer_Plane.PrintPlane(this, vector, Vector2.one, mat, 270f, false, uvs, null, 0f, 0f);
					break;
				}
				case DamageOverlay.BotRightCorner:
				{
					Material mat = damageData.cornerBRMat;
					Color32 color;
					Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Building, false, false, out mat, out uvs, out color);
					Printer_Plane.PrintPlane(this, vector, Vector2.one, mat, 180f, false, uvs, null, 0f, 0f);
					break;
				}
				case DamageOverlay.LeftEdge:
				{
					Material mat = damageData.edgeLeftMat;
					Color32 color;
					Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Building, false, false, out mat, out uvs, out color);
					Printer_Plane.PrintPlane(this, vector + new Vector3(0f, 0f, z2), Vector2.one, mat, 270f, false, uvs, null, 0f, 0f);
					break;
				}
				case DamageOverlay.RightEdge:
				{
					Material mat = damageData.edgeRightMat;
					Color32 color;
					Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Building, false, false, out mat, out uvs, out color);
					Printer_Plane.PrintPlane(this, vector + new Vector3(0f, 0f, z), Vector2.one, mat, 90f, false, uvs, null, 0f, 0f);
					break;
				}
				case DamageOverlay.TopEdge:
				{
					Material mat = damageData.edgeTopMat;
					Color32 color;
					Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Building, false, false, out mat, out uvs, out color);
					Printer_Plane.PrintPlane(this, vector + new Vector3(x, 0f, 0f), Vector2.one, mat, 0f, false, uvs, null, 0f, 0f);
					break;
				}
				case DamageOverlay.BotEdge:
				{
					Material mat = damageData.edgeBotMat;
					Color32 color;
					Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Building, false, false, out mat, out uvs, out color);
					Printer_Plane.PrintPlane(this, vector + new Vector3(x2, 0f, 0f), Vector2.one, mat, 180f, false, uvs, null, 0f, 0f);
					break;
				}
				}
			}
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x000550D8 File Offset: 0x000532D8
		private void DrawFullThingCorners(Building b)
		{
			if (b.def.graphicData == null)
			{
				return;
			}
			if (b.def.graphicData.damageData == null)
			{
				return;
			}
			Rect damageRect = BuildingsDamageSectionLayerUtility.GetDamageRect(b);
			float damageTexturesAltitude = this.GetDamageTexturesAltitude(b);
			float num = Mathf.Min(Mathf.Min(damageRect.width, damageRect.height), 1.5f);
			Material material;
			Material material2;
			Material material3;
			Material material4;
			BuildingsDamageSectionLayerUtility.GetCornerMats(out material, out material2, out material3, out material4, b);
			float num2 = num * Rand.Range(0.9f, 1f);
			float num3 = num * Rand.Range(0.9f, 1f);
			float num4 = num * Rand.Range(0.9f, 1f);
			float num5 = num * Rand.Range(0.9f, 1f);
			Vector2[] uvs = null;
			List<DamageOverlay> overlays = BuildingsDamageSectionLayerUtility.GetOverlays(b);
			for (int i = 0; i < overlays.Count; i++)
			{
				switch (overlays[i])
				{
				case DamageOverlay.TopLeftCorner:
				{
					Rect rect = new Rect(damageRect.xMin, damageRect.yMax - num2, num2, num2);
					Material mat = material;
					Color32 color;
					Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Building, false, false, out mat, out uvs, out color);
					Printer_Plane.PrintPlane(this, new Vector3(rect.center.x, damageTexturesAltitude, rect.center.y), rect.size, mat, 0f, false, uvs, null, 0f, 0f);
					break;
				}
				case DamageOverlay.TopRightCorner:
				{
					Rect rect2 = new Rect(damageRect.xMax - num3, damageRect.yMax - num3, num3, num3);
					Material mat = material2;
					Color32 color;
					Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Building, false, false, out mat, out uvs, out color);
					Printer_Plane.PrintPlane(this, new Vector3(rect2.center.x, damageTexturesAltitude, rect2.center.y), rect2.size, mat, 90f, false, uvs, null, 0f, 0f);
					break;
				}
				case DamageOverlay.BotLeftCorner:
				{
					Rect rect3 = new Rect(damageRect.xMin, damageRect.yMin, num5, num5);
					Material mat = material4;
					Color32 color;
					Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Building, false, false, out mat, out uvs, out color);
					Printer_Plane.PrintPlane(this, new Vector3(rect3.center.x, damageTexturesAltitude, rect3.center.y), rect3.size, mat, 270f, false, uvs, null, 0f, 0f);
					break;
				}
				case DamageOverlay.BotRightCorner:
				{
					Rect rect4 = new Rect(damageRect.xMax - num4, damageRect.yMin, num4, num4);
					Material mat = material3;
					Color32 color;
					Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Building, false, false, out mat, out uvs, out color);
					Printer_Plane.PrintPlane(this, new Vector3(rect4.center.x, damageTexturesAltitude, rect4.center.y), rect4.size, mat, 180f, false, uvs, null, 0f, 0f);
					break;
				}
				}
			}
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x000553A9 File Offset: 0x000535A9
		private float GetDamageTexturesAltitude(Building b)
		{
			return b.def.Altitude + 0.2027027f;
		}

		// Token: 0x04000D8B RID: 3467
		private static List<Vector2> scratches = new List<Vector2>();
	}
}
