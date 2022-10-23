using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000564 RID: 1380
	[StaticConstructorOnStartup]
	public static class GenDraw
	{
		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x06002A39 RID: 10809 RVA: 0x0010D886 File Offset: 0x0010BA86
		public static Material CurTargetingMat
		{
			get
			{
				GenDraw.TargetSquareMatSingle.color = GenDraw.CurTargetingColor;
				return GenDraw.TargetSquareMatSingle;
			}
		}

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x06002A3A RID: 10810 RVA: 0x0010D89C File Offset: 0x0010BA9C
		public static Color CurTargetingColor
		{
			get
			{
				float num = (float)Math.Sin((double)(Time.time * 8f));
				num *= 0.2f;
				num += 0.8f;
				return new Color(1f, num, num);
			}
		}

		// Token: 0x06002A3B RID: 10811 RVA: 0x0010D8D8 File Offset: 0x0010BAD8
		public static void DrawNoBuildEdgeLines()
		{
			GenDraw.DrawMapEdgeLines(10);
		}

		// Token: 0x06002A3C RID: 10812 RVA: 0x0010D8E1 File Offset: 0x0010BAE1
		public static void DrawNoZoneEdgeLines()
		{
			GenDraw.DrawMapEdgeLines(5);
		}

		// Token: 0x06002A3D RID: 10813 RVA: 0x0010D8EC File Offset: 0x0010BAEC
		private static void DrawMapEdgeLines(int edgeDist)
		{
			float y = AltitudeLayer.MetaOverlays.AltitudeFor();
			IntVec3 size = Find.CurrentMap.Size;
			Vector3 vector = new Vector3((float)edgeDist, y, (float)edgeDist);
			Vector3 vector2 = new Vector3((float)edgeDist, y, (float)(size.z - edgeDist));
			Vector3 vector3 = new Vector3((float)(size.x - edgeDist), y, (float)(size.z - edgeDist));
			Vector3 vector4 = new Vector3((float)(size.x - edgeDist), y, (float)edgeDist);
			GenDraw.DrawLineBetween(vector, vector2, GenDraw.LineMatMetaOverlay, 0.2f);
			GenDraw.DrawLineBetween(vector2, vector3, GenDraw.LineMatMetaOverlay, 0.2f);
			GenDraw.DrawLineBetween(vector3, vector4, GenDraw.LineMatMetaOverlay, 0.2f);
			GenDraw.DrawLineBetween(vector4, vector, GenDraw.LineMatMetaOverlay, 0.2f);
		}

		// Token: 0x06002A3E RID: 10814 RVA: 0x0010D9A0 File Offset: 0x0010BBA0
		public static void DrawLineBetween(Vector3 A, Vector3 B)
		{
			GenDraw.DrawLineBetween(A, B, GenDraw.LineMatWhite, 0.2f);
		}

		// Token: 0x06002A3F RID: 10815 RVA: 0x0010D9B3 File Offset: 0x0010BBB3
		public static void DrawLineBetween(Vector3 A, Vector3 B, float layer)
		{
			GenDraw.DrawLineBetween(A, B, layer, GenDraw.LineMatWhite, 0.2f);
		}

		// Token: 0x06002A40 RID: 10816 RVA: 0x0010D9C7 File Offset: 0x0010BBC7
		public static void DrawLineBetween(Vector3 A, Vector3 B, float layer, Material mat, float lineWidth = 0.2f)
		{
			GenDraw.DrawLineBetween(A + Vector3.up * layer, B + Vector3.up * layer, mat, lineWidth);
		}

		// Token: 0x06002A41 RID: 10817 RVA: 0x0010D9F3 File Offset: 0x0010BBF3
		public static void DrawLineBetween(Vector3 A, Vector3 B, SimpleColor color, float lineWidth = 0.2f)
		{
			GenDraw.DrawLineBetween(A, B, GenDraw.GetLineMat(color), lineWidth);
		}

		// Token: 0x06002A42 RID: 10818 RVA: 0x0010DA04 File Offset: 0x0010BC04
		public static void DrawLineBetween(Vector3 A, Vector3 B, Material mat, float lineWidth = 0.2f)
		{
			if (Mathf.Abs(A.x - B.x) < 0.01f && Mathf.Abs(A.z - B.z) < 0.01f)
			{
				return;
			}
			Vector3 pos = (A + B) / 2f;
			if (A == B)
			{
				return;
			}
			A.y = B.y;
			float z = (A - B).MagnitudeHorizontal();
			Quaternion q = Quaternion.LookRotation(A - B);
			Vector3 s = new Vector3(lineWidth, 1f, z);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(pos, q, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, mat, 0);
		}

		// Token: 0x06002A43 RID: 10819 RVA: 0x0010DAB5 File Offset: 0x0010BCB5
		public static void DrawCircleOutline(Vector3 center, float radius)
		{
			GenDraw.DrawCircleOutline(center, radius, GenDraw.LineMatWhite);
		}

		// Token: 0x06002A44 RID: 10820 RVA: 0x0010DAC3 File Offset: 0x0010BCC3
		public static void DrawCircleOutline(Vector3 center, float radius, SimpleColor color)
		{
			GenDraw.DrawCircleOutline(center, radius, GenDraw.GetLineMat(color));
		}

		// Token: 0x06002A45 RID: 10821 RVA: 0x0010DAD4 File Offset: 0x0010BCD4
		public static void DrawCircleOutline(Vector3 center, float radius, Material material)
		{
			int num = Mathf.Clamp(Mathf.RoundToInt(24f * radius), 12, 48);
			float num2 = 0f;
			float num3 = 6.2831855f / (float)num;
			Vector3 vector = center;
			Vector3 a = center;
			for (int i = 0; i < num + 2; i++)
			{
				if (i >= 2)
				{
					GenDraw.DrawLineBetween(a, vector, material, 0.2f);
				}
				a = vector;
				vector = center;
				vector.x += Mathf.Cos(num2) * radius;
				vector.z += Mathf.Sin(num2) * radius;
				num2 += num3;
			}
		}

		// Token: 0x06002A46 RID: 10822 RVA: 0x0010DB60 File Offset: 0x0010BD60
		private static Material GetLineMat(SimpleColor color)
		{
			switch (color)
			{
			case SimpleColor.White:
				return GenDraw.LineMatWhite;
			case SimpleColor.Red:
				return GenDraw.LineMatRed;
			case SimpleColor.Green:
				return GenDraw.LineMatGreen;
			case SimpleColor.Blue:
				return GenDraw.LineMatBlue;
			case SimpleColor.Magenta:
				return GenDraw.LineMatMagenta;
			case SimpleColor.Yellow:
				return GenDraw.LineMatYellow;
			case SimpleColor.Cyan:
				return GenDraw.LineMatCyan;
			case SimpleColor.Orange:
				return GenDraw.LineMatOrange;
			default:
				return GenDraw.LineMatWhite;
			}
		}

		// Token: 0x06002A47 RID: 10823 RVA: 0x0010DBCA File Offset: 0x0010BDCA
		public static void DrawWorldLineBetween(Vector3 A, Vector3 B)
		{
			GenDraw.DrawWorldLineBetween(A, B, GenDraw.WorldLineMatWhite, 1f);
		}

		// Token: 0x06002A48 RID: 10824 RVA: 0x0010DBE0 File Offset: 0x0010BDE0
		public static void DrawWorldLineBetween(Vector3 A, Vector3 B, Material material, float widthFactor = 1f)
		{
			if (Mathf.Abs(A.x - B.x) < 0.005f && Mathf.Abs(A.y - B.y) < 0.005f && Mathf.Abs(A.z - B.z) < 0.005f)
			{
				return;
			}
			Vector3 pos = (A + B) / 2f;
			float magnitude = (A - B).magnitude;
			Quaternion q = Quaternion.LookRotation(A - B, pos.normalized);
			Vector3 s = new Vector3(0.2f * Find.WorldGrid.averageTileSize * widthFactor, 1f, magnitude);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(pos, q, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, material, WorldCameraManager.WorldLayer);
		}

		// Token: 0x06002A49 RID: 10825 RVA: 0x0010DCB4 File Offset: 0x0010BEB4
		public static void DrawWorldRadiusRing(int center, int radius)
		{
			if (radius < 0)
			{
				return;
			}
			if (GenDraw.cachedEdgeTilesForCenter != center || GenDraw.cachedEdgeTilesForRadius != radius || GenDraw.cachedEdgeTilesForWorldSeed != Find.World.info.Seed)
			{
				GenDraw.cachedEdgeTilesForCenter = center;
				GenDraw.cachedEdgeTilesForRadius = radius;
				GenDraw.cachedEdgeTilesForWorldSeed = Find.World.info.Seed;
				GenDraw.cachedEdgeTiles.Clear();
				Find.WorldFloodFiller.FloodFill(center, (int tile) => true, delegate(int tile, int dist)
				{
					if (dist > radius + 1)
					{
						return true;
					}
					if (dist == radius + 1)
					{
						GenDraw.cachedEdgeTiles.Add(tile);
					}
					return false;
				}, int.MaxValue, null);
				WorldGrid worldGrid = Find.WorldGrid;
				Vector3 c = worldGrid.GetTileCenter(center);
				Vector3 n = c.normalized;
				GenDraw.cachedEdgeTiles.Sort(delegate(int a, int b)
				{
					float num = Vector3.Dot(n, Vector3.Cross(worldGrid.GetTileCenter(a) - c, worldGrid.GetTileCenter(b) - c));
					if (Mathf.Abs(num) < 0.0001f)
					{
						return 0;
					}
					if (num < 0f)
					{
						return -1;
					}
					return 1;
				});
			}
			GenDraw.DrawWorldLineStrip(GenDraw.cachedEdgeTiles, GenDraw.OneSidedWorldLineMatWhite, 5f);
		}

		// Token: 0x06002A4A RID: 10826 RVA: 0x0010DDD0 File Offset: 0x0010BFD0
		public static void DrawWorldLineStrip(List<int> edgeTiles, Material material, float widthFactor)
		{
			if (edgeTiles.Count < 3)
			{
				return;
			}
			WorldGrid worldGrid = Find.WorldGrid;
			float d = 0.05f;
			for (int i = 0; i < edgeTiles.Count; i++)
			{
				int index = (i == 0) ? (edgeTiles.Count - 1) : (i - 1);
				int num = edgeTiles[index];
				int num2 = edgeTiles[i];
				if (worldGrid.IsNeighbor(num, num2))
				{
					Vector3 a = worldGrid.GetTileCenter(num);
					Vector3 vector = worldGrid.GetTileCenter(num2);
					a += a.normalized * d;
					vector += vector.normalized * d;
					GenDraw.DrawWorldLineBetween(a, vector, material, widthFactor);
				}
			}
		}

		// Token: 0x06002A4B RID: 10827 RVA: 0x0010DE81 File Offset: 0x0010C081
		public static void DrawTargetHighlight(LocalTargetInfo targ)
		{
			if (targ.Thing != null)
			{
				GenDraw.DrawTargetingHighlight_Thing(targ.Thing);
				return;
			}
			GenDraw.DrawTargetingHighlight_Cell(targ.Cell);
		}

		// Token: 0x06002A4C RID: 10828 RVA: 0x0010DEA5 File Offset: 0x0010C0A5
		private static void DrawTargetingHighlight_Cell(IntVec3 c)
		{
			GenDraw.DrawTargetHighlightWithLayer(c, AltitudeLayer.Building);
		}

		// Token: 0x06002A4D RID: 10829 RVA: 0x0010DEB0 File Offset: 0x0010C0B0
		public static void DrawTargetHighlightWithLayer(IntVec3 c, AltitudeLayer layer)
		{
			Vector3 position = c.ToVector3ShiftedWithAltitude(layer);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, GenDraw.CurTargetingMat, 0);
		}

		// Token: 0x06002A4E RID: 10830 RVA: 0x0010DEDC File Offset: 0x0010C0DC
		public static void DrawTargetHighlightWithLayer(Vector3 c, AltitudeLayer layer)
		{
			Vector3 position = new Vector3(c.x, layer.AltitudeFor(), c.z);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, GenDraw.CurTargetingMat, 0);
		}

		// Token: 0x06002A4F RID: 10831 RVA: 0x0010DF18 File Offset: 0x0010C118
		private static void DrawTargetingHighlight_Thing(Thing t)
		{
			Graphics.DrawMesh(MeshPool.plane10, t.TrueCenter() + Altitudes.AltIncVect, t.Rotation.AsQuat, GenDraw.CurTargetingMat, 0);
		}

		// Token: 0x06002A50 RID: 10832 RVA: 0x0010DF54 File Offset: 0x0010C154
		public static void DrawStencilCell(Vector3 c, Material material, float width = 1f, float height = 1f)
		{
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(new Vector3(c.x, -1f, c.z), Quaternion.identity, new Vector3(width, 1f, height));
			Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0);
		}

		// Token: 0x06002A51 RID: 10833 RVA: 0x0010DFA4 File Offset: 0x0010C1A4
		public static void DrawTargetingHightlight_Explosion(IntVec3 c, float Radius)
		{
			GenDraw.DrawRadiusRing(c, Radius);
		}

		// Token: 0x06002A52 RID: 10834 RVA: 0x0010DFB0 File Offset: 0x0010C1B0
		public static void DrawInteractionCells(ThingDef tDef, IntVec3 center, Rot4 placingRot)
		{
			if (!tDef.multipleInteractionCellOffsets.NullOrEmpty<IntVec3>())
			{
				using (List<IntVec3>.Enumerator enumerator = tDef.multipleInteractionCellOffsets.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IntVec3 interactionOffset = enumerator.Current;
						GenDraw.DrawInteractionCell(tDef, interactionOffset, center, placingRot);
					}
					return;
				}
			}
			if (tDef.hasInteractionCell)
			{
				GenDraw.DrawInteractionCell(tDef, tDef.interactionCellOffset, center, placingRot);
			}
		}

		// Token: 0x06002A53 RID: 10835 RVA: 0x0010E028 File Offset: 0x0010C228
		private static void DrawInteractionCell(ThingDef tDef, IntVec3 interactionOffset, IntVec3 center, Rot4 placingRot)
		{
			IntVec3 c = ThingUtility.InteractionCell(interactionOffset, center, placingRot);
			Vector3 vector = c.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			if (c.InBounds(Find.CurrentMap))
			{
				Building edifice = c.GetEdifice(Find.CurrentMap);
				if (edifice != null && edifice.def.building != null && edifice.def.building.isSittable)
				{
					return;
				}
			}
			if (tDef.interactionCellGraphic == null && tDef.interactionCellIcon != null)
			{
				ThingDef thingDef = tDef.interactionCellIcon;
				if (thingDef.blueprintDef != null)
				{
					thingDef = thingDef.blueprintDef;
				}
				tDef.interactionCellGraphic = thingDef.graphic.GetColoredVersion(ShaderTypeDefOf.EdgeDetect.Shader, GenDraw.InteractionCellIntensity, Color.white);
			}
			if (tDef.interactionCellGraphic != null)
			{
				Rot4 rot = tDef.interactionCellIconReverse ? placingRot.Opposite : placingRot;
				tDef.interactionCellGraphic.DrawFromDef(vector, rot, tDef.interactionCellIcon, 0f);
				return;
			}
			Graphics.DrawMesh(MeshPool.plane10, vector, Quaternion.identity, GenDraw.InteractionCellMaterial, 0);
		}

		// Token: 0x06002A54 RID: 10836 RVA: 0x0010E11C File Offset: 0x0010C31C
		public static void DrawRadiusRing(IntVec3 center, float radius, Color color, Func<IntVec3, bool> predicate = null)
		{
			if (radius > GenRadial.MaxRadialPatternRadius)
			{
				if (!GenDraw.maxRadiusMessaged)
				{
					Log.Error("Cannot draw radius ring of radius " + radius + ": not enough squares in the precalculated list.");
					GenDraw.maxRadiusMessaged = true;
				}
				return;
			}
			GenDraw.ringDrawCells.Clear();
			int num = GenRadial.NumCellsInRadius(radius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[i];
				if (predicate == null || predicate(intVec))
				{
					GenDraw.ringDrawCells.Add(intVec);
				}
			}
			GenDraw.DrawFieldEdges(GenDraw.ringDrawCells, color, null);
		}

		// Token: 0x06002A55 RID: 10837 RVA: 0x0010E1B3 File Offset: 0x0010C3B3
		public static void DrawRadiusRing(IntVec3 center, float radius)
		{
			GenDraw.DrawRadiusRing(center, radius, Color.white, null);
		}

		// Token: 0x06002A56 RID: 10838 RVA: 0x0010E1C4 File Offset: 0x0010C3C4
		public static void DrawFieldEdges(List<IntVec3> cells)
		{
			GenDraw.DrawFieldEdges(cells, Color.white, null);
		}

		// Token: 0x06002A57 RID: 10839 RVA: 0x0010E1E8 File Offset: 0x0010C3E8
		public static void DrawFieldEdges(List<IntVec3> cells, Color color, float? altOffset = null)
		{
			Map currentMap = Find.CurrentMap;
			Material material = MaterialPool.MatFrom(new MaterialRequest
			{
				shader = ShaderDatabase.Transparent,
				color = color,
				BaseTexPath = "UI/Overlays/TargetHighlight_Side"
			});
			material.GetTexture("_MainTex").wrapMode = TextureWrapMode.Clamp;
			if (GenDraw.fieldGrid == null)
			{
				GenDraw.fieldGrid = new BoolGrid(currentMap);
			}
			else
			{
				GenDraw.fieldGrid.ClearAndResizeTo(currentMap);
			}
			int x = currentMap.Size.x;
			int z = currentMap.Size.z;
			int count = cells.Count;
			float y = altOffset ?? (Rand.ValueSeeded(color.ToOpaque().GetHashCode()) * 0.04054054f / 10f);
			for (int i = 0; i < count; i++)
			{
				if (cells[i].InBounds(currentMap))
				{
					GenDraw.fieldGrid[cells[i].x, cells[i].z] = true;
				}
			}
			for (int j = 0; j < count; j++)
			{
				IntVec3 intVec = cells[j];
				if (intVec.InBounds(currentMap))
				{
					GenDraw.rotNeeded[0] = (intVec.z < z - 1 && !GenDraw.fieldGrid[intVec.x, intVec.z + 1]);
					GenDraw.rotNeeded[1] = (intVec.x < x - 1 && !GenDraw.fieldGrid[intVec.x + 1, intVec.z]);
					GenDraw.rotNeeded[2] = (intVec.z > 0 && !GenDraw.fieldGrid[intVec.x, intVec.z - 1]);
					GenDraw.rotNeeded[3] = (intVec.x > 0 && !GenDraw.fieldGrid[intVec.x - 1, intVec.z]);
					for (int k = 0; k < 4; k++)
					{
						if (GenDraw.rotNeeded[k])
						{
							Graphics.DrawMesh(MeshPool.plane10, intVec.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays) + new Vector3(0f, y, 0f), new Rot4(k).AsQuat, material, 0);
						}
					}
				}
			}
		}

		// Token: 0x06002A58 RID: 10840 RVA: 0x0010E44C File Offset: 0x0010C64C
		public static void DrawAimPie(Thing shooter, LocalTargetInfo target, int degreesWide, float offsetDist)
		{
			float facing = 0f;
			if (target.Cell != shooter.Position)
			{
				if (target.Thing != null)
				{
					facing = (target.Thing.DrawPos - shooter.Position.ToVector3Shifted()).AngleFlat();
				}
				else
				{
					facing = (target.Cell - shooter.Position).AngleFlat;
				}
			}
			GenDraw.DrawAimPieRaw(shooter.DrawPos + new Vector3(0f, offsetDist, 0f), facing, degreesWide);
		}

		// Token: 0x06002A59 RID: 10841 RVA: 0x0010E4E0 File Offset: 0x0010C6E0
		public static void DrawAimPieRaw(Vector3 center, float facing, int degreesWide)
		{
			if (degreesWide <= 0)
			{
				return;
			}
			if (degreesWide > 360)
			{
				degreesWide = 360;
			}
			center += Quaternion.AngleAxis(facing, Vector3.up) * Vector3.forward * 0.8f;
			Graphics.DrawMesh(MeshPool.pies[degreesWide], center, Quaternion.AngleAxis(facing + (float)(degreesWide / 2) - 90f, Vector3.up), GenDraw.AimPieMaterial, 0);
		}

		// Token: 0x06002A5A RID: 10842 RVA: 0x0010E554 File Offset: 0x0010C754
		public static void DrawCooldownCircle(Vector3 center, float radius)
		{
			Vector3 s = new Vector3(radius, 1f, radius);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(center, Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.circle, matrix, GenDraw.AimPieMaterial, 0);
		}

		// Token: 0x06002A5B RID: 10843 RVA: 0x0010E598 File Offset: 0x0010C798
		public static void DrawFillableBar(GenDraw.FillableBarRequest r)
		{
			Vector2 vector = r.preRotationOffset.RotatedBy(r.rotation.AsAngle);
			r.center += new Vector3(vector.x, 0f, vector.y);
			if (r.rotation == Rot4.South)
			{
				r.rotation = Rot4.North;
			}
			if (r.rotation == Rot4.West)
			{
				r.rotation = Rot4.East;
			}
			Vector3 s = new Vector3(r.size.x + r.margin, 1f, r.size.y + r.margin);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(r.center, r.rotation.AsQuat, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, r.unfilledMat, 0);
			if (r.fillPercent > 0.001f)
			{
				s = new Vector3(r.size.x * r.fillPercent, 1f, r.size.y);
				matrix = default(Matrix4x4);
				Vector3 pos = r.center + Vector3.up * 0.01f;
				if (!r.rotation.IsHorizontal)
				{
					pos.x -= r.size.x * 0.5f;
					pos.x += 0.5f * r.size.x * r.fillPercent;
				}
				else
				{
					pos.z -= r.size.x * 0.5f;
					pos.z += 0.5f * r.size.x * r.fillPercent;
				}
				matrix.SetTRS(pos, r.rotation.AsQuat, s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, r.filledMat, 0);
			}
		}

		// Token: 0x06002A5C RID: 10844 RVA: 0x0010E79C File Offset: 0x0010C99C
		public static void DrawMeshNowOrLater(Mesh mesh, Vector3 loc, Quaternion quat, Material mat, bool drawNow)
		{
			if (drawNow)
			{
				if (mat == null || !mat.SetPass(0))
				{
					string str = "SetPass(0) call failed on material ";
					string str2 = (mat != null) ? mat.name : null;
					string str3 = " with shader ";
					string str4;
					if (mat == null)
					{
						str4 = null;
					}
					else
					{
						Shader shader = mat.shader;
						str4 = ((shader != null) ? shader.name : null);
					}
					Log.Error(str + str2 + str3 + str4);
				}
				Graphics.DrawMeshNow(mesh, loc, quat);
				return;
			}
			Graphics.DrawMesh(mesh, loc, quat, mat, 0);
		}

		// Token: 0x06002A5D RID: 10845 RVA: 0x0010E80A File Offset: 0x0010CA0A
		public static void DrawMeshNowOrLater(Mesh mesh, Matrix4x4 matrix, Material mat, bool drawNow)
		{
			if (drawNow)
			{
				mat.SetPass(0);
				Graphics.DrawMeshNow(mesh, matrix);
				return;
			}
			Graphics.DrawMesh(mesh, matrix, mat, 0);
		}

		// Token: 0x06002A5E RID: 10846 RVA: 0x0010E828 File Offset: 0x0010CA28
		public static void DrawArrowPointingAt(Vector3 mapTarget, bool offscreenOnly = false)
		{
			Vector3 vector = UI.UIToMapPosition((float)(UI.screenWidth / 2), (float)(UI.screenHeight / 2));
			if ((vector - mapTarget).MagnitudeHorizontalSquared() < 81f)
			{
				if (!offscreenOnly)
				{
					Vector3 position = mapTarget;
					position.y = AltitudeLayer.MetaOverlays.AltitudeFor();
					position.z -= 1.5f;
					Graphics.DrawMesh(MeshPool.plane20, position, Quaternion.identity, GenDraw.ArrowMatWhite, 0);
					return;
				}
			}
			else
			{
				Vector3 normalized = (mapTarget - vector).Yto0().normalized;
				Vector3 position2 = vector + normalized * 7f;
				position2.y = AltitudeLayer.MetaOverlays.AltitudeFor();
				Quaternion rotation = Quaternion.LookRotation(normalized);
				Graphics.DrawMesh(MeshPool.plane20, position2, rotation, GenDraw.ArrowMatWhite, 0);
			}
		}

		// Token: 0x06002A5F RID: 10847 RVA: 0x0010E8EC File Offset: 0x0010CAEC
		public static void DrawArrowRotated(Vector3 pos, float rotationAngle, bool ghost)
		{
			Quaternion rotation = Quaternion.AngleAxis(rotationAngle, new Vector3(0f, 1f, 0f));
			Vector3 position = pos;
			position.y = AltitudeLayer.MetaOverlays.AltitudeFor();
			Graphics.DrawMesh(MeshPool.plane10, position, rotation, ghost ? GenDraw.ArrowMatGhost : GenDraw.ArrowMatWhite, 0);
		}

		// Token: 0x04001BAD RID: 7085
		private static readonly Material TargetSquareMatSingle = MaterialPool.MatFrom("UI/Overlays/TargetHighlight_Square", ShaderDatabase.Transparent);

		// Token: 0x04001BAE RID: 7086
		private const float TargetPulseFrequency = 8f;

		// Token: 0x04001BAF RID: 7087
		public static readonly string LineTexPath = "UI/Overlays/ThingLine";

		// Token: 0x04001BB0 RID: 7088
		public static readonly string OneSidedLineTexPath = "UI/Overlays/OneSidedLine";

		// Token: 0x04001BB1 RID: 7089
		private static readonly Material LineMatWhite = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.white);

		// Token: 0x04001BB2 RID: 7090
		private static readonly Material LineMatRed = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.red);

		// Token: 0x04001BB3 RID: 7091
		private static readonly Material LineMatGreen = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.green);

		// Token: 0x04001BB4 RID: 7092
		private static readonly Material LineMatBlue = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.blue);

		// Token: 0x04001BB5 RID: 7093
		private static readonly Material LineMatMagenta = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.magenta);

		// Token: 0x04001BB6 RID: 7094
		private static readonly Material LineMatYellow = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.yellow);

		// Token: 0x04001BB7 RID: 7095
		private static readonly Material LineMatCyan = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.cyan);

		// Token: 0x04001BB8 RID: 7096
		private static readonly Material LineMatOrange = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, ColorLibrary.Orange);

		// Token: 0x04001BB9 RID: 7097
		private static readonly Material LineMatMetaOverlay = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.MetaOverlay);

		// Token: 0x04001BBA RID: 7098
		private static readonly Material WorldLineMatWhite = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.WorldOverlayTransparent, Color.white, WorldMaterials.WorldLineRenderQueue);

		// Token: 0x04001BBB RID: 7099
		private static readonly Material OneSidedWorldLineMatWhite = MaterialPool.MatFrom(GenDraw.OneSidedLineTexPath, ShaderDatabase.WorldOverlayTransparent, Color.white, WorldMaterials.WorldLineRenderQueue);

		// Token: 0x04001BBC RID: 7100
		public static readonly Material RitualStencilMat = MaterialPool.MatFrom(ShaderDatabase.RitualStencil);

		// Token: 0x04001BBD RID: 7101
		private const float LineWidth = 0.2f;

		// Token: 0x04001BBE RID: 7102
		private const float BaseWorldLineWidth = 0.2f;

		// Token: 0x04001BBF RID: 7103
		public static readonly Material InteractionCellMaterial = MaterialPool.MatFrom("UI/Overlays/InteractionCell", ShaderDatabase.Transparent);

		// Token: 0x04001BC0 RID: 7104
		private static readonly Color InteractionCellIntensity = new Color(1f, 1f, 1f, 0.3f);

		// Token: 0x04001BC1 RID: 7105
		public const float MultiItemsPerCellDrawSizeFactor = 0.8f;

		// Token: 0x04001BC2 RID: 7106
		private static List<int> cachedEdgeTiles = new List<int>();

		// Token: 0x04001BC3 RID: 7107
		private static int cachedEdgeTilesForCenter = -1;

		// Token: 0x04001BC4 RID: 7108
		private static int cachedEdgeTilesForRadius = -1;

		// Token: 0x04001BC5 RID: 7109
		private static int cachedEdgeTilesForWorldSeed = -1;

		// Token: 0x04001BC6 RID: 7110
		private static List<IntVec3> ringDrawCells = new List<IntVec3>();

		// Token: 0x04001BC7 RID: 7111
		private static bool maxRadiusMessaged = false;

		// Token: 0x04001BC8 RID: 7112
		private static BoolGrid fieldGrid;

		// Token: 0x04001BC9 RID: 7113
		private static bool[] rotNeeded = new bool[4];

		// Token: 0x04001BCA RID: 7114
		private static readonly Material AimPieMaterial = SolidColorMaterials.SimpleSolidColorMaterial(new Color(1f, 1f, 1f, 0.3f), false);

		// Token: 0x04001BCB RID: 7115
		private static readonly Material ArrowMatWhite = MaterialPool.MatFrom("UI/Overlays/Arrow", ShaderDatabase.CutoutFlying, Color.white);

		// Token: 0x04001BCC RID: 7116
		private static readonly Material ArrowMatGhost = MaterialPool.MatFrom("UI/Overlays/ArrowGhost", ShaderDatabase.Transparent, Color.white);

		// Token: 0x02002127 RID: 8487
		public struct FillableBarRequest
		{
			// Token: 0x04008365 RID: 33637
			public Vector3 center;

			// Token: 0x04008366 RID: 33638
			public Vector2 size;

			// Token: 0x04008367 RID: 33639
			public float fillPercent;

			// Token: 0x04008368 RID: 33640
			public Material filledMat;

			// Token: 0x04008369 RID: 33641
			public Material unfilledMat;

			// Token: 0x0400836A RID: 33642
			public float margin;

			// Token: 0x0400836B RID: 33643
			public Rot4 rotation;

			// Token: 0x0400836C RID: 33644
			public Vector2 preRotationOffset;
		}
	}
}
