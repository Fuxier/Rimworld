using System;

namespace Verse
{
	// Token: 0x02000574 RID: 1396
	public class WeakReference<T> : WeakReference where T : class
	{
		// Token: 0x06002AEB RID: 10987 RVA: 0x00112AE9 File Offset: 0x00110CE9
		public WeakReference(T target) : base(target)
		{
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x06002AEC RID: 10988 RVA: 0x00112AF7 File Offset: 0x00110CF7
		// (set) Token: 0x06002AED RID: 10989 RVA: 0x00112B04 File Offset: 0x00110D04
		public new T Target
		{
			get
			{
				return (T)((object)base.Target);
			}
			set
			{
				base.Target = value;
			}
		}
	}
}
