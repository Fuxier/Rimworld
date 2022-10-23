using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000275 RID: 629
	public abstract class Mod
	{
		// Token: 0x1700036A RID: 874
		// (get) Token: 0x060011FD RID: 4605 RVA: 0x00069450 File Offset: 0x00067650
		public ModContentPack Content
		{
			get
			{
				return this.intContent;
			}
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x00069458 File Offset: 0x00067658
		public Mod(ModContentPack content)
		{
			this.intContent = content;
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x00069468 File Offset: 0x00067668
		public T GetSettings<T>() where T : ModSettings, new()
		{
			if (this.modSettings != null && this.modSettings.GetType() != typeof(T))
			{
				Log.Error(string.Format("Mod {0} attempted to read two different settings classes (was {1}, is now {2})", this.Content.Name, this.modSettings.GetType(), typeof(T)));
				return default(T);
			}
			if (this.modSettings != null)
			{
				return (T)((object)this.modSettings);
			}
			this.modSettings = LoadedModManager.ReadModSettings<T>(this.intContent.FolderName, base.GetType().Name);
			this.modSettings.Mod = this;
			return this.modSettings as T;
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x00069528 File Offset: 0x00067728
		public virtual void WriteSettings()
		{
			if (this.modSettings != null)
			{
				this.modSettings.Write();
			}
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void DoSettingsWindowContents(Rect inRect)
		{
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x00019CD5 File Offset: 0x00017ED5
		public virtual string SettingsCategory()
		{
			return "";
		}

		// Token: 0x04000F2D RID: 3885
		private ModSettings modSettings;

		// Token: 0x04000F2E RID: 3886
		private ModContentPack intContent;
	}
}
