using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000203 RID: 515
	public sealed class MapDrawer
	{
		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000EE6 RID: 3814 RVA: 0x00052D7C File Offset: 0x00050F7C
		private IntVec2 SectionCount
		{
			get
			{
				return new IntVec2
				{
					x = Mathf.CeilToInt((float)this.map.Size.x / 17f),
					z = Mathf.CeilToInt((float)this.map.Size.z / 17f)
				};
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000EE7 RID: 3815 RVA: 0x00052DD8 File Offset: 0x00050FD8
		private CellRect VisibleSections
		{
			get
			{
				CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
				CellRect sunShadowsViewRect = this.GetSunShadowsViewRect(currentViewRect);
				sunShadowsViewRect.ClipInsideMap(this.map);
				IntVec2 intVec = this.SectionCoordsAt(sunShadowsViewRect.BottomLeft);
				IntVec2 intVec2 = this.SectionCoordsAt(sunShadowsViewRect.TopRight);
				if (intVec2.x < intVec.x || intVec2.z < intVec.z)
				{
					return CellRect.Empty;
				}
				return CellRect.FromLimits(intVec.x, intVec.z, intVec2.x, intVec2.z);
			}
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x00052E61 File Offset: 0x00051061
		public MapDrawer(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x00052E70 File Offset: 0x00051070
		public void MapMeshDirty(IntVec3 loc, MapMeshFlag dirtyFlags)
		{
			bool regenAdjacentCells = (dirtyFlags & (MapMeshFlag.FogOfWar | MapMeshFlag.Buildings)) > MapMeshFlag.None;
			bool regenAdjacentSections = (dirtyFlags & MapMeshFlag.GroundGlow) > MapMeshFlag.None;
			this.MapMeshDirty(loc, dirtyFlags, regenAdjacentCells, regenAdjacentSections);
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x00052E98 File Offset: 0x00051098
		public void MapMeshDirty(IntVec3 loc, MapMeshFlag dirtyFlags, bool regenAdjacentCells, bool regenAdjacentSections)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			this.SectionAt(loc).dirtyFlags |= dirtyFlags;
			if (regenAdjacentCells)
			{
				for (int i = 0; i < 8; i++)
				{
					IntVec3 intVec = loc + GenAdj.AdjacentCells[i];
					if (intVec.InBounds(this.map))
					{
						this.SectionAt(intVec).dirtyFlags |= dirtyFlags;
					}
				}
			}
			if (regenAdjacentSections)
			{
				IntVec2 a = this.SectionCoordsAt(loc);
				for (int j = 0; j < 8; j++)
				{
					IntVec3 intVec2 = GenAdj.AdjacentCells[j];
					IntVec2 intVec3 = a + new IntVec2(intVec2.x, intVec2.z);
					IntVec2 sectionCount = this.SectionCount;
					if (intVec3.x >= 0 && intVec3.z >= 0 && intVec3.x <= sectionCount.x - 1 && intVec3.z <= sectionCount.z - 1)
					{
						this.sections[intVec3.x, intVec3.z].dirtyFlags |= dirtyFlags;
					}
				}
			}
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x00052FB4 File Offset: 0x000511B4
		public void MapMeshDrawerUpdate_First()
		{
			CellRect visibleSections = this.VisibleSections;
			bool flag = false;
			foreach (IntVec3 intVec in visibleSections)
			{
				Section sect = this.sections[intVec.x, intVec.z];
				if (this.TryUpdateSection(sect))
				{
					flag = true;
				}
			}
			if (!flag)
			{
				for (int i = 0; i < this.SectionCount.x; i++)
				{
					for (int j = 0; j < this.SectionCount.z; j++)
					{
						if (this.TryUpdateSection(this.sections[i, j]))
						{
							return;
						}
					}
				}
			}
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x0005307C File Offset: 0x0005127C
		private bool TryUpdateSection(Section sect)
		{
			if (sect.dirtyFlags == MapMeshFlag.None)
			{
				return false;
			}
			for (int i = 0; i < MapMeshFlagUtility.allFlags.Count; i++)
			{
				MapMeshFlag mapMeshFlag = MapMeshFlagUtility.allFlags[i];
				if ((sect.dirtyFlags & mapMeshFlag) != MapMeshFlag.None)
				{
					sect.RegenerateLayers(mapMeshFlag);
				}
			}
			sect.dirtyFlags = MapMeshFlag.None;
			return true;
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x000530D0 File Offset: 0x000512D0
		public void DrawMapMesh()
		{
			CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
			currentViewRect.minX -= 17;
			currentViewRect.minZ -= 17;
			foreach (IntVec3 intVec in this.VisibleSections)
			{
				Section section = this.sections[intVec.x, intVec.z];
				section.DrawSection(!currentViewRect.Contains(section.botLeft));
			}
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x00053178 File Offset: 0x00051378
		private IntVec2 SectionCoordsAt(IntVec3 loc)
		{
			return new IntVec2(Mathf.FloorToInt((float)(loc.x / 17)), Mathf.FloorToInt((float)(loc.z / 17)));
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x000531A0 File Offset: 0x000513A0
		public Section SectionAt(IntVec3 loc)
		{
			IntVec2 intVec = this.SectionCoordsAt(loc);
			return this.sections[intVec.x, intVec.z];
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x000531CC File Offset: 0x000513CC
		public void RegenerateEverythingNow()
		{
			if (this.sections == null)
			{
				this.sections = new Section[this.SectionCount.x, this.SectionCount.z];
			}
			for (int i = 0; i < this.SectionCount.x; i++)
			{
				for (int j = 0; j < this.SectionCount.z; j++)
				{
					if (this.sections[i, j] == null)
					{
						this.sections[i, j] = new Section(new IntVec3(i, 0, j), this.map);
					}
					this.sections[i, j].RegenerateAllLayers();
				}
			}
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x00053270 File Offset: 0x00051470
		public void WholeMapChanged(MapMeshFlag change)
		{
			for (int i = 0; i < this.SectionCount.x; i++)
			{
				for (int j = 0; j < this.SectionCount.z; j++)
				{
					this.sections[i, j].dirtyFlags |= change;
				}
			}
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x000532C4 File Offset: 0x000514C4
		private CellRect GetSunShadowsViewRect(CellRect rect)
		{
			GenCelestial.LightInfo lightSourceInfo = GenCelestial.GetLightSourceInfo(this.map, GenCelestial.LightType.Shadow);
			if (lightSourceInfo.vector.x < 0f)
			{
				rect.maxX -= Mathf.FloorToInt(lightSourceInfo.vector.x);
			}
			else
			{
				rect.minX -= Mathf.CeilToInt(lightSourceInfo.vector.x);
			}
			if (lightSourceInfo.vector.y < 0f)
			{
				rect.maxZ -= Mathf.FloorToInt(lightSourceInfo.vector.y);
			}
			else
			{
				rect.minZ -= Mathf.CeilToInt(lightSourceInfo.vector.y);
			}
			return rect;
		}

		// Token: 0x04000D70 RID: 3440
		private Map map;

		// Token: 0x04000D71 RID: 3441
		private Section[,] sections;
	}
}
