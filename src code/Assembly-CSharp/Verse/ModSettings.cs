using System;

namespace Verse
{
	// Token: 0x02000284 RID: 644
	public abstract class ModSettings : IExposable
	{
		// Token: 0x170003AD RID: 941
		// (get) Token: 0x060012B1 RID: 4785 RVA: 0x0006C658 File Offset: 0x0006A858
		// (set) Token: 0x060012B2 RID: 4786 RVA: 0x0006C660 File Offset: 0x0006A860
		public Mod Mod { get; internal set; }

		// Token: 0x060012B3 RID: 4787 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void ExposeData()
		{
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x0006C669 File Offset: 0x0006A869
		public void Write()
		{
			LoadedModManager.WriteModSettings(this.Mod.Content.FolderName, this.Mod.GetType().Name, this);
		}
	}
}
