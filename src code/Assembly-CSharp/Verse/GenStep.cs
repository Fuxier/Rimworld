using System;

namespace Verse
{
	// Token: 0x020000F5 RID: 245
	public abstract class GenStep
	{
		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060006E4 RID: 1764
		public abstract int SeedPart { get; }

		// Token: 0x060006E5 RID: 1765
		public abstract void Generate(Map map, GenStepParams parms);

		// Token: 0x040005AF RID: 1455
		public GenStepDef def;
	}
}
