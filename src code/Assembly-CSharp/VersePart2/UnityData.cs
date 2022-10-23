using System;
using System.Threading;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000043 RID: 67
	public static class UnityData
	{
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x00014331 File Offset: 0x00012531
		public static bool IsInMainThread
		{
			get
			{
				return UnityData.mainThreadId == Thread.CurrentThread.ManagedThreadId;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x00014344 File Offset: 0x00012544
		public static bool Is32BitBuild
		{
			get
			{
				return IntPtr.Size == 4;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x0001434E File Offset: 0x0001254E
		public static bool Is64BitBuild
		{
			get
			{
				return IntPtr.Size == 8;
			}
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00014358 File Offset: 0x00012558
		static UnityData()
		{
			if (!UnityData.initialized && !UnityDataInitializer.initializing)
			{
				Log.Warning("Used UnityData before it's initialized.");
			}
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00014372 File Offset: 0x00012572
		public static void CopyUnityData()
		{
			UnityData.mainThreadId = Thread.CurrentThread.ManagedThreadId;
			UnityData.isEditor = Application.isEditor;
			UnityData.dataPath = Application.dataPath;
			UnityData.platform = Application.platform;
			UnityData.persistentDataPath = Application.persistentDataPath;
			UnityData.initialized = true;
		}

		// Token: 0x040000DE RID: 222
		private static bool initialized;

		// Token: 0x040000DF RID: 223
		public static bool isEditor;

		// Token: 0x040000E0 RID: 224
		public static string dataPath;

		// Token: 0x040000E1 RID: 225
		public static RuntimePlatform platform;

		// Token: 0x040000E2 RID: 226
		public static string persistentDataPath;

		// Token: 0x040000E3 RID: 227
		private static int mainThreadId;
	}
}
