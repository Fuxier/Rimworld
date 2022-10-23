using System;

namespace Verse
{
	// Token: 0x02000155 RID: 341
	public abstract class WorldGenStep
	{
		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060008E3 RID: 2275
		public abstract int SeedPart { get; }

		// Token: 0x060008E4 RID: 2276
		public abstract void GenerateFresh(string seed);

		// Token: 0x060008E5 RID: 2277 RVA: 0x0002B857 File Offset: 0x00029A57
		public virtual void GenerateWithoutWorldData(string seed)
		{
			this.GenerateFresh(seed);
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void GenerateFromScribe(string seed)
		{
		}

		// Token: 0x04000981 RID: 2433
		public WorldGenStepDef def;
	}
}
