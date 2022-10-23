using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000202 RID: 514
	public static class MapMeshFlagUtility
	{
		// Token: 0x06000EE5 RID: 3813 RVA: 0x00052D08 File Offset: 0x00050F08
		static MapMeshFlagUtility()
		{
			foreach (object obj in Enum.GetValues(typeof(MapMeshFlag)))
			{
				MapMeshFlag mapMeshFlag = (MapMeshFlag)obj;
				if (mapMeshFlag != MapMeshFlag.None)
				{
					MapMeshFlagUtility.allFlags.Add(mapMeshFlag);
				}
			}
		}

		// Token: 0x04000D6F RID: 3439
		public static List<MapMeshFlag> allFlags = new List<MapMeshFlag>();
	}
}
