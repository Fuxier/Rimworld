using System;
using RimWorld.IO;

namespace Verse
{
	// Token: 0x0200027A RID: 634
	public class LoadedContentItem<T> where T : class
	{
		// Token: 0x06001213 RID: 4627 RVA: 0x00069BF7 File Offset: 0x00067DF7
		public LoadedContentItem(VirtualFile internalFile, T contentItem, IDisposable extraDisposable = null)
		{
			this.internalFile = internalFile;
			this.contentItem = contentItem;
			this.extraDisposable = extraDisposable;
		}

		// Token: 0x04000F3A RID: 3898
		public VirtualFile internalFile;

		// Token: 0x04000F3B RID: 3899
		public T contentItem;

		// Token: 0x04000F3C RID: 3900
		public IDisposable extraDisposable;
	}
}
