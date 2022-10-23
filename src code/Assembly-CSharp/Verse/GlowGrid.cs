using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001DB RID: 475
	public sealed class GlowGrid
	{
		// Token: 0x06000D31 RID: 3377 RVA: 0x0004A16C File Offset: 0x0004836C
		public GlowGrid(Map map)
		{
			this.map = map;
			this.glowGrid = new ColorInt[map.cellIndices.NumGridCells];
			this.glowGridNoCavePlants = new ColorInt[map.cellIndices.NumGridCells];
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x0004A1C8 File Offset: 0x000483C8
		public Color32 VisualGlowAt(IntVec3 c)
		{
			return this.glowGrid[this.map.cellIndices.CellToIndex(c)].ProjectToColor32;
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x0004A1EC File Offset: 0x000483EC
		public float GameGlowAt(IntVec3 c, bool ignoreCavePlants = false)
		{
			float num = 0f;
			if (!this.map.roofGrid.Roofed(c))
			{
				num = this.map.skyManager.CurSkyGlow;
				if (num == 1f)
				{
					return num;
				}
			}
			ColorInt colorInt = (ignoreCavePlants ? this.glowGridNoCavePlants : this.glowGrid)[this.map.cellIndices.CellToIndex(c)];
			if (colorInt.a == 1)
			{
				return 1f;
			}
			float b = (float)Mathf.Max(new int[]
			{
				colorInt.r,
				colorInt.g,
				colorInt.b
			}) / 255f * 3.6f;
			b = Mathf.Min(0.5f, b);
			return Mathf.Max(num, b);
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x0004A2AB File Offset: 0x000484AB
		public PsychGlow PsychGlowAt(IntVec3 c)
		{
			return GlowGrid.PsychGlowAtGlow(this.GameGlowAt(c, false));
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x0004A2BA File Offset: 0x000484BA
		public static PsychGlow PsychGlowAtGlow(float glow)
		{
			if (glow > 0.9f)
			{
				return PsychGlow.Overlit;
			}
			if (glow > 0.3f)
			{
				return PsychGlow.Lit;
			}
			return PsychGlow.Dark;
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x0004A2D1 File Offset: 0x000484D1
		public void RegisterGlower(CompGlower newGlow)
		{
			this.litGlowers.Add(newGlow);
			this.MarkGlowGridDirty(newGlow.parent.Position);
			if (Current.ProgramState != ProgramState.Playing)
			{
				this.initialGlowerLocs.Add(newGlow.parent.Position);
			}
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x0004A30F File Offset: 0x0004850F
		public void DeRegisterGlower(CompGlower oldGlow)
		{
			this.litGlowers.Remove(oldGlow);
			this.MarkGlowGridDirty(oldGlow.parent.Position);
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x0004A32F File Offset: 0x0004852F
		public void MarkGlowGridDirty(IntVec3 loc)
		{
			this.glowGridDirty = true;
			this.map.mapDrawer.MapMeshDirty(loc, MapMeshFlag.GroundGlow);
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x0004A34A File Offset: 0x0004854A
		public void GlowGridUpdate_First()
		{
			if (this.glowGridDirty)
			{
				this.RecalculateAllGlow();
				this.glowGridDirty = false;
			}
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x0004A364 File Offset: 0x00048564
		private void RecalculateAllGlow()
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			if (this.initialGlowerLocs != null)
			{
				foreach (IntVec3 loc in this.initialGlowerLocs)
				{
					this.MarkGlowGridDirty(loc);
				}
				this.initialGlowerLocs = null;
			}
			int numGridCells = this.map.cellIndices.NumGridCells;
			for (int i = 0; i < numGridCells; i++)
			{
				this.glowGrid[i] = new ColorInt(0, 0, 0, 0);
				this.glowGridNoCavePlants[i] = new ColorInt(0, 0, 0, 0);
			}
			foreach (CompGlower compGlower in this.litGlowers)
			{
				this.map.glowFlooder.AddFloodGlowFor(compGlower, this.glowGrid);
				if (compGlower.parent.def.category != ThingCategory.Plant || !compGlower.parent.def.plant.cavePlant)
				{
					this.map.glowFlooder.AddFloodGlowFor(compGlower, this.glowGridNoCavePlants);
				}
			}
		}

		// Token: 0x04000C0D RID: 3085
		private Map map;

		// Token: 0x04000C0E RID: 3086
		public ColorInt[] glowGrid;

		// Token: 0x04000C0F RID: 3087
		public ColorInt[] glowGridNoCavePlants;

		// Token: 0x04000C10 RID: 3088
		private bool glowGridDirty;

		// Token: 0x04000C11 RID: 3089
		private HashSet<CompGlower> litGlowers = new HashSet<CompGlower>();

		// Token: 0x04000C12 RID: 3090
		private List<IntVec3> initialGlowerLocs = new List<IntVec3>();

		// Token: 0x04000C13 RID: 3091
		public const int AlphaOfNotOverlit = 0;

		// Token: 0x04000C14 RID: 3092
		public const int AlphaOfOverlit = 1;

		// Token: 0x04000C15 RID: 3093
		private const float GameGlowLitThreshold = 0.3f;

		// Token: 0x04000C16 RID: 3094
		private const float GameGlowOverlitThreshold = 0.9f;

		// Token: 0x04000C17 RID: 3095
		private const float GroundGameGlowFactor = 3.6f;

		// Token: 0x04000C18 RID: 3096
		private const float MaxGameGlowFromNonOverlitGroundLights = 0.5f;
	}
}
