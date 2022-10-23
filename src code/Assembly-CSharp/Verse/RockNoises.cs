using System;
using System.Collections.Generic;
using Verse.Noise;

namespace Verse
{
	// Token: 0x02000229 RID: 553
	public static class RockNoises
	{
		// Token: 0x06000FB1 RID: 4017 RVA: 0x0005B49C File Offset: 0x0005969C
		public static void Init(Map map)
		{
			RockNoises.rockNoises = new List<RockNoises.RockNoise>();
			foreach (ThingDef rockDef in Find.World.NaturalRockTypesIn(map.Tile))
			{
				RockNoises.RockNoise rockNoise = new RockNoises.RockNoise();
				rockNoise.rockDef = rockDef;
				rockNoise.noise = new Perlin(0.004999999888241291, 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.Medium);
				RockNoises.rockNoises.Add(rockNoise);
				NoiseDebugUI.StoreNoiseRender(rockNoise.noise, rockNoise.rockDef + " score", map.Size.ToIntVec2);
			}
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x0005B570 File Offset: 0x00059770
		public static void Reset()
		{
			RockNoises.rockNoises = null;
		}

		// Token: 0x04000DFF RID: 3583
		public static List<RockNoises.RockNoise> rockNoises;

		// Token: 0x04000E00 RID: 3584
		private const float RockNoiseFreq = 0.005f;

		// Token: 0x02001D86 RID: 7558
		public class RockNoise
		{
			// Token: 0x04007489 RID: 29833
			public ThingDef rockDef;

			// Token: 0x0400748A RID: 29834
			public ModuleBase noise;
		}
	}
}
