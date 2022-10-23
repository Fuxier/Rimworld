using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001BD RID: 445
	public static class DebugSolidColorMats
	{
		// Token: 0x06000C73 RID: 3187 RVA: 0x00045B94 File Offset: 0x00043D94
		public static Material MaterialOf(Color col)
		{
			Material material;
			if (DebugSolidColorMats.colorMatDict.TryGetValue(col, out material))
			{
				return material;
			}
			material = SolidColorMaterials.SimpleSolidColorMaterial(col, false);
			DebugSolidColorMats.colorMatDict.Add(col, material);
			return material;
		}

		// Token: 0x04000B6C RID: 2924
		private static Dictionary<Color, Material> colorMatDict = new Dictionary<Color, Material>();
	}
}
