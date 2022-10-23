using System;

namespace Verse
{
	// Token: 0x02000257 RID: 599
	public static class MapExposeUtility
	{
		// Token: 0x06001124 RID: 4388 RVA: 0x00064154 File Offset: 0x00062354
		public static void ExposeUshort(Map map, Func<IntVec3, ushort> shortReader, Action<IntVec3, ushort> shortWriter, string label)
		{
			byte[] arr = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				arr = MapSerializeUtility.SerializeUshort(map, shortReader);
			}
			DataExposeUtility.ByteArray(ref arr, label);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				MapSerializeUtility.LoadUshort(arr, map, shortWriter);
			}
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x0006418C File Offset: 0x0006238C
		public static void ExposeInt(Map map, Func<IntVec3, int> intReader, Action<IntVec3, int> intWriter, string label)
		{
			byte[] arr = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				arr = MapSerializeUtility.SerializeInt(map, intReader);
			}
			DataExposeUtility.ByteArray(ref arr, label);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				MapSerializeUtility.LoadInt(arr, map, intWriter);
			}
		}
	}
}
