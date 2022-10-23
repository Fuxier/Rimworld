using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001E1 RID: 481
	public sealed class DeepResourceGrid : ICellBoolGiver, IExposable
	{
		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000D70 RID: 3440 RVA: 0x00020495 File Offset: 0x0001E695
		public Color Color
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x0004ADDC File Offset: 0x00048FDC
		public DeepResourceGrid(Map map)
		{
			this.map = map;
			this.defGrid = new ushort[map.cellIndices.NumGridCells];
			this.countGrid = new ushort[map.cellIndices.NumGridCells];
			this.drawer = new CellBoolDrawer(this, map.Size.x, map.Size.z, 3640, 1f);
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x0004AE50 File Offset: 0x00049050
		public void ExposeData()
		{
			MapExposeUtility.ExposeUshort(this.map, (IntVec3 c) => this.defGrid[this.map.cellIndices.CellToIndex(c)], delegate(IntVec3 c, ushort val)
			{
				this.defGrid[this.map.cellIndices.CellToIndex(c)] = val;
			}, "defGrid");
			MapExposeUtility.ExposeUshort(this.map, (IntVec3 c) => this.countGrid[this.map.cellIndices.CellToIndex(c)], delegate(IntVec3 c, ushort val)
			{
				this.countGrid[this.map.cellIndices.CellToIndex(c)] = val;
			}, "countGrid");
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x0004AEAD File Offset: 0x000490AD
		public ThingDef ThingDefAt(IntVec3 c)
		{
			return DefDatabase<ThingDef>.GetByShortHash(this.defGrid[this.map.cellIndices.CellToIndex(c)]);
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x0004AECC File Offset: 0x000490CC
		public int CountAt(IntVec3 c)
		{
			return (int)this.countGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x0004AEE8 File Offset: 0x000490E8
		public void SetAt(IntVec3 c, ThingDef def, int count)
		{
			if (count == 0)
			{
				def = null;
			}
			ushort num;
			if (def == null)
			{
				num = 0;
			}
			else
			{
				num = def.shortHash;
			}
			ushort num2 = (ushort)count;
			if (count > 65535)
			{
				Log.Error("Cannot store count " + count + " in DeepResourceGrid: out of ushort range.");
				num2 = ushort.MaxValue;
			}
			if (count < 0)
			{
				Log.Error("Cannot store count " + count + " in DeepResourceGrid: out of ushort range.");
				num2 = 0;
			}
			int num3 = this.map.cellIndices.CellToIndex(c);
			if (this.defGrid[num3] == num && this.countGrid[num3] == num2)
			{
				return;
			}
			this.defGrid[num3] = num;
			this.countGrid[num3] = num2;
			this.drawer.SetDirty();
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x0004AF9A File Offset: 0x0004919A
		public void DeepResourceGridUpdate()
		{
			this.drawer.CellBoolDrawerUpdate();
			if (DebugViewSettings.drawDeepResources)
			{
				this.MarkForDraw();
			}
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x0004AFB4 File Offset: 0x000491B4
		public void MarkForDraw()
		{
			if (this.map == Find.CurrentMap)
			{
				this.drawer.MarkForDraw();
			}
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x0004AFD0 File Offset: 0x000491D0
		public void DrawPlacingMouseAttachments(BuildableDef placingDef)
		{
			ThingDef thingDef;
			if ((thingDef = (placingDef as ThingDef)) != null && thingDef.CompDefFor<CompDeepDrill>() != null && this.AnyActiveDeepScannersOnMap())
			{
				this.RenderMouseAttachments();
			}
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x0004B000 File Offset: 0x00049200
		public void DeepResourcesOnGUI()
		{
			Thing singleSelectedThing = Find.Selector.SingleSelectedThing;
			if (singleSelectedThing == null)
			{
				return;
			}
			CompDeepScanner compDeepScanner = singleSelectedThing.TryGetComp<CompDeepScanner>();
			CompDeepDrill compDeepDrill = singleSelectedThing.TryGetComp<CompDeepDrill>();
			if (compDeepScanner == null && compDeepDrill == null)
			{
				return;
			}
			if (!this.AnyActiveDeepScannersOnMap())
			{
				return;
			}
			this.RenderMouseAttachments();
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x0004B040 File Offset: 0x00049240
		public bool AnyActiveDeepScannersOnMap()
		{
			foreach (Building thing in this.map.listerBuildings.allBuildingsColonist)
			{
				CompDeepScanner compDeepScanner = thing.TryGetComp<CompDeepScanner>();
				if (compDeepScanner != null && compDeepScanner.ShouldShowDeepResourceOverlay())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x0004B0B0 File Offset: 0x000492B0
		private void RenderMouseAttachments()
		{
			IntVec3 c = UI.MouseCell();
			if (!c.InBounds(this.map))
			{
				return;
			}
			ThingDef thingDef = this.map.deepResourceGrid.ThingDefAt(c);
			if (thingDef == null)
			{
				return;
			}
			int num = this.map.deepResourceGrid.CountAt(c);
			if (num <= 0)
			{
				return;
			}
			Vector2 vector = c.ToVector3().MapToUIPosition();
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleLeft;
			float num2 = (UI.CurUICellSize() - 27f) / 2f;
			Rect rect = new Rect(vector.x + num2, vector.y - UI.CurUICellSize() + num2, 27f, 27f);
			Widgets.ThingIcon(rect, thingDef, null, null, 1f, null, null);
			Widgets.Label(new Rect(rect.xMax + 4f, rect.y, 999f, 29f), "DeepResourceRemaining".Translate(thingDef.Named("RESOURCE"), num.Named("COUNT")));
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x0004B1D4 File Offset: 0x000493D4
		public bool GetCellBool(int index)
		{
			return this.CountAt(this.map.cellIndices.IndexToCell(index)) > 0;
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x0004B1F0 File Offset: 0x000493F0
		public Color GetCellExtraColor(int index)
		{
			IntVec3 c = this.map.cellIndices.IndexToCell(index);
			float num = (float)this.CountAt(c);
			ThingDef thingDef = this.ThingDefAt(c);
			return DebugMatsSpectrum.Mat(Mathf.RoundToInt(num / (float)thingDef.deepCountPerCell / 2f * 100f) % 100, true).color;
		}

		// Token: 0x04000C29 RID: 3113
		private const float LineSpacing = 29f;

		// Token: 0x04000C2A RID: 3114
		private const float IconPaddingRight = 4f;

		// Token: 0x04000C2B RID: 3115
		private const float IconSize = 27f;

		// Token: 0x04000C2C RID: 3116
		private Map map;

		// Token: 0x04000C2D RID: 3117
		private CellBoolDrawer drawer;

		// Token: 0x04000C2E RID: 3118
		private ushort[] defGrid;

		// Token: 0x04000C2F RID: 3119
		private ushort[] countGrid;
	}
}
