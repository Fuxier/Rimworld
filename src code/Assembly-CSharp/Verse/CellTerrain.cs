using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000212 RID: 530
	public struct CellTerrain
	{
		// Token: 0x06000F46 RID: 3910 RVA: 0x00057D30 File Offset: 0x00055F30
		public CellTerrain(TerrainDef def, bool polluted, float snowCoverage, ColorDef color)
		{
			this.def = def;
			this.polluted = polluted;
			this.snowCoverage = snowCoverage;
			this.color = color;
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x00057D50 File Offset: 0x00055F50
		public override bool Equals(object obj)
		{
			if (obj is CellTerrain)
			{
				CellTerrain terrain = (CellTerrain)obj;
				return this.Equals(terrain);
			}
			return false;
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x00057D78 File Offset: 0x00055F78
		public bool Equals(CellTerrain terrain)
		{
			return terrain.def == this.def && terrain.color == this.color && terrain.polluted == this.polluted && Mathf.Abs(terrain.snowCoverage - this.snowCoverage) < float.Epsilon;
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x00057DCA File Offset: 0x00055FCA
		public override int GetHashCode()
		{
			return Gen.HashCombine<ColorDef>(Gen.HashCombine<float>(Gen.HashCombine<bool>(Gen.HashCombine<TerrainDef>(0, this.def), this.polluted), this.snowCoverage), this.color);
		}

		// Token: 0x04000DA9 RID: 3497
		public TerrainDef def;

		// Token: 0x04000DAA RID: 3498
		public bool polluted;

		// Token: 0x04000DAB RID: 3499
		public float snowCoverage;

		// Token: 0x04000DAC RID: 3500
		public ColorDef color;
	}
}
