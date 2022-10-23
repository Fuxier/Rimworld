using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x0200038A RID: 906
	public static class FadedMaterialPool
	{
		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06001A0E RID: 6670 RVA: 0x0009D4C6 File Offset: 0x0009B6C6
		public static int TotalMaterialCount
		{
			get
			{
				return FadedMaterialPool.cachedMats.Count;
			}
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06001A0F RID: 6671 RVA: 0x0009D4D4 File Offset: 0x0009B6D4
		public static long TotalMaterialBytes
		{
			get
			{
				long num = 0L;
				foreach (KeyValuePair<FadedMaterialPool.FadedMatRequest, Material> keyValuePair in FadedMaterialPool.cachedMats)
				{
					num += Profiler.GetRuntimeMemorySizeLong(keyValuePair.Value);
				}
				return num;
			}
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x0009D534 File Offset: 0x0009B734
		public static Material FadedVersionOf(Material sourceMat, float alpha)
		{
			int num = FadedMaterialPool.IndexFromAlpha(alpha);
			if (num == 0)
			{
				return BaseContent.ClearMat;
			}
			if (num == 29)
			{
				return sourceMat;
			}
			FadedMaterialPool.FadedMatRequest key = new FadedMaterialPool.FadedMatRequest(sourceMat, num);
			Material material;
			if (!FadedMaterialPool.cachedMats.TryGetValue(key, out material))
			{
				material = MaterialAllocator.Create(sourceMat);
				material.color = new Color(1f, 1f, 1f, (float)FadedMaterialPool.IndexFromAlpha(alpha) / 30f);
				FadedMaterialPool.cachedMats.Add(key, material);
			}
			return material;
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x0009D5AC File Offset: 0x0009B7AC
		private static int IndexFromAlpha(float alpha)
		{
			int num = Mathf.FloorToInt(alpha * 30f);
			if (num == 30)
			{
				num = 29;
			}
			return num;
		}

		// Token: 0x0400130A RID: 4874
		private static Dictionary<FadedMaterialPool.FadedMatRequest, Material> cachedMats = new Dictionary<FadedMaterialPool.FadedMatRequest, Material>(FadedMaterialPool.FadedMatRequestComparer.Instance);

		// Token: 0x0400130B RID: 4875
		private const int NumFadeSteps = 30;

		// Token: 0x02001E63 RID: 7779
		private struct FadedMatRequest : IEquatable<FadedMaterialPool.FadedMatRequest>
		{
			// Token: 0x0600B8DD RID: 47325 RVA: 0x0041DDDF File Offset: 0x0041BFDF
			public FadedMatRequest(Material mat, int alphaIndex)
			{
				this.mat = mat;
				this.alphaIndex = alphaIndex;
			}

			// Token: 0x0600B8DE RID: 47326 RVA: 0x0041DDEF File Offset: 0x0041BFEF
			public override bool Equals(object obj)
			{
				return obj != null && obj is FadedMaterialPool.FadedMatRequest && this.Equals((FadedMaterialPool.FadedMatRequest)obj);
			}

			// Token: 0x0600B8DF RID: 47327 RVA: 0x0041DE0A File Offset: 0x0041C00A
			public bool Equals(FadedMaterialPool.FadedMatRequest other)
			{
				return this.mat == other.mat && this.alphaIndex == other.alphaIndex;
			}

			// Token: 0x0600B8E0 RID: 47328 RVA: 0x0041DE2F File Offset: 0x0041C02F
			public override int GetHashCode()
			{
				return Gen.HashCombineInt(this.mat.GetHashCode(), this.alphaIndex);
			}

			// Token: 0x0600B8E1 RID: 47329 RVA: 0x0041DE47 File Offset: 0x0041C047
			public static bool operator ==(FadedMaterialPool.FadedMatRequest lhs, FadedMaterialPool.FadedMatRequest rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x0600B8E2 RID: 47330 RVA: 0x0041DE51 File Offset: 0x0041C051
			public static bool operator !=(FadedMaterialPool.FadedMatRequest lhs, FadedMaterialPool.FadedMatRequest rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x040077C7 RID: 30663
			private Material mat;

			// Token: 0x040077C8 RID: 30664
			private int alphaIndex;
		}

		// Token: 0x02001E64 RID: 7780
		private class FadedMatRequestComparer : IEqualityComparer<FadedMaterialPool.FadedMatRequest>
		{
			// Token: 0x0600B8E3 RID: 47331 RVA: 0x0041DE5D File Offset: 0x0041C05D
			public bool Equals(FadedMaterialPool.FadedMatRequest x, FadedMaterialPool.FadedMatRequest y)
			{
				return x.Equals(y);
			}

			// Token: 0x0600B8E4 RID: 47332 RVA: 0x0041DE67 File Offset: 0x0041C067
			public int GetHashCode(FadedMaterialPool.FadedMatRequest obj)
			{
				return obj.GetHashCode();
			}

			// Token: 0x040077C9 RID: 30665
			public static readonly FadedMaterialPool.FadedMatRequestComparer Instance = new FadedMaterialPool.FadedMatRequestComparer();
		}
	}
}
