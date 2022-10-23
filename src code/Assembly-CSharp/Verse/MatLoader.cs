using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000581 RID: 1409
	public static class MatLoader
	{
		// Token: 0x06002B1F RID: 11039 RVA: 0x00113B7C File Offset: 0x00111D7C
		public static Material LoadMat(string matPath, int renderQueue = -1)
		{
			Material material = (Material)Resources.Load("Materials/" + matPath, typeof(Material));
			if (material == null)
			{
				Log.Warning("Could not load material " + matPath);
			}
			MatLoader.Request key = new MatLoader.Request
			{
				path = matPath,
				renderQueue = renderQueue
			};
			Material material2;
			if (!MatLoader.dict.TryGetValue(key, out material2))
			{
				material2 = MaterialAllocator.Create(material);
				if (renderQueue != -1)
				{
					material2.renderQueue = renderQueue;
				}
				MatLoader.dict.Add(key, material2);
			}
			return material2;
		}

		// Token: 0x04001C27 RID: 7207
		private static Dictionary<MatLoader.Request, Material> dict = new Dictionary<MatLoader.Request, Material>();

		// Token: 0x02002146 RID: 8518
		private struct Request
		{
			// Token: 0x0600C687 RID: 50823 RVA: 0x0043ED1B File Offset: 0x0043CF1B
			public override int GetHashCode()
			{
				return Gen.HashCombineInt(Gen.HashCombine<string>(0, this.path), this.renderQueue);
			}

			// Token: 0x0600C688 RID: 50824 RVA: 0x0043ED34 File Offset: 0x0043CF34
			public override bool Equals(object obj)
			{
				return obj is MatLoader.Request && this.Equals((MatLoader.Request)obj);
			}

			// Token: 0x0600C689 RID: 50825 RVA: 0x0043ED4C File Offset: 0x0043CF4C
			public bool Equals(MatLoader.Request other)
			{
				return other.path == this.path && other.renderQueue == this.renderQueue;
			}

			// Token: 0x0600C68A RID: 50826 RVA: 0x0043ED71 File Offset: 0x0043CF71
			public static bool operator ==(MatLoader.Request lhs, MatLoader.Request rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x0600C68B RID: 50827 RVA: 0x0043ED7B File Offset: 0x0043CF7B
			public static bool operator !=(MatLoader.Request lhs, MatLoader.Request rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x0600C68C RID: 50828 RVA: 0x0043ED87 File Offset: 0x0043CF87
			public override string ToString()
			{
				return string.Concat(new object[]
				{
					"MatLoader.Request(",
					this.path,
					", ",
					this.renderQueue,
					")"
				});
			}

			// Token: 0x040083FA RID: 33786
			public string path;

			// Token: 0x040083FB RID: 33787
			public int renderQueue;
		}
	}
}
