using System;

namespace Verse
{
	// Token: 0x02000044 RID: 68
	public static class UnityDataInitializer
	{
		// Token: 0x060003A6 RID: 934 RVA: 0x000143B4 File Offset: 0x000125B4
		public static void CopyUnityData()
		{
			UnityDataInitializer.initializing = true;
			try
			{
				UnityData.CopyUnityData();
			}
			finally
			{
				UnityDataInitializer.initializing = false;
			}
		}

		// Token: 0x040000E4 RID: 228
		public static bool initializing;
	}
}
