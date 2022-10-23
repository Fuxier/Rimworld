using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Verse
{
	// Token: 0x020001C4 RID: 452
	public class DrawBatch
	{
		// Token: 0x06000CAD RID: 3245 RVA: 0x000470D4 File Offset: 0x000452D4
		public DrawBatchPropertyBlock GetPropertyBlock()
		{
			DrawBatchPropertyBlock drawBatchPropertyBlock;
			if (this.propertyBlockCache.Count == 0)
			{
				drawBatchPropertyBlock = new DrawBatchPropertyBlock();
				this.myPropertyBlocks.Add(drawBatchPropertyBlock);
			}
			else
			{
				drawBatchPropertyBlock = this.propertyBlockCache.Pop<DrawBatchPropertyBlock>();
			}
			if (DrawBatch.PropertyBlockLeakDebug)
			{
				drawBatchPropertyBlock.leakDebugString = "Allocated from:\n\n---------------\n\n" + StackTraceUtility.ExtractStackTrace();
			}
			return drawBatchPropertyBlock;
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x0004712E File Offset: 0x0004532E
		public void ReturnPropertyBlock(DrawBatchPropertyBlock propertyBlock)
		{
			if (this.myPropertyBlocks.Contains(propertyBlock))
			{
				this.propertyBlockCache.Add(propertyBlock);
			}
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x0004714A File Offset: 0x0004534A
		public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Color? color = null, bool renderInstanced = false, DrawBatchPropertyBlock propertyBlock = null)
		{
			this.GetBatchDataForInsertion(new DrawBatch.BatchKey(mesh, material, layer, renderInstanced, propertyBlock)).Add(matrix, color);
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x00047167 File Offset: 0x00045367
		public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, bool renderInstanced = false)
		{
			this.GetBatchDataForInsertion(new DrawBatch.BatchKey(mesh, material, layer, renderInstanced, null)).Add(matrix);
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x00047184 File Offset: 0x00045384
		public void Flush(bool draw = true)
		{
			if (this.tmpPropertyBlock == null)
			{
				this.tmpPropertyBlock = new MaterialPropertyBlock();
			}
			this.tmpPropertyBlocks.Clear();
			this.tmpPropertyBlocks.AddRange(this.propertyBlockCache);
			try
			{
				foreach (KeyValuePair<DrawBatch.BatchKey, List<DrawBatch.BatchData>> keyValuePair in this.batches)
				{
					DrawBatch.BatchKey key = keyValuePair.Key;
					try
					{
						foreach (DrawBatch.BatchData batchData in keyValuePair.Value)
						{
							DrawBatch.BatchData batchData2 = batchData;
							if (draw)
							{
								this.tmpPropertyBlock.Clear();
								if (key.propertyBlock != null)
								{
									key.propertyBlock.Write(this.tmpPropertyBlock);
								}
								if (key.renderInstanced)
								{
									key.material.enableInstancing = true;
									if (batchData2.hasAnyColors)
									{
										this.tmpPropertyBlock.SetVectorArray("_Color", batchData2.colors);
									}
									Graphics.DrawMeshInstanced(key.mesh, 0, key.material, batchData.matrices, batchData.ptr, this.tmpPropertyBlock, ShadowCastingMode.On, true, key.layer);
								}
								else
								{
									for (int i = 0; i < batchData2.ptr; i++)
									{
										Matrix4x4 matrix = batchData2.matrices[i];
										Vector4 v = batchData2.colors[i];
										if (batchData2.hasAnyColors)
										{
											this.tmpPropertyBlock.SetColor("_Color", v);
										}
										Graphics.DrawMesh(key.mesh, matrix, key.material, key.layer, null, 0, this.tmpPropertyBlock);
									}
								}
							}
							batchData2.Clear();
							this.batchDataListCache.Add(batchData2);
						}
					}
					finally
					{
						if (key.propertyBlock != null && this.myPropertyBlocks.Contains(key.propertyBlock))
						{
							this.tmpPropertyBlocks.Add(key.propertyBlock);
							key.propertyBlock.Clear();
							this.propertyBlockCache.Add(key.propertyBlock);
						}
						this.batchListCache.Add(keyValuePair.Value);
						keyValuePair.Value.Clear();
					}
				}
			}
			finally
			{
				foreach (DrawBatchPropertyBlock drawBatchPropertyBlock in this.myPropertyBlocks)
				{
					if (!this.tmpPropertyBlocks.Contains(drawBatchPropertyBlock))
					{
						Log.Warning("Property block from FleckDrawBatch leaked!" + ((drawBatchPropertyBlock.leakDebugString == null) ? null : ("Leak debug information: \n" + drawBatchPropertyBlock.leakDebugString)));
					}
				}
				HashSet<DrawBatchPropertyBlock> hashSet = this.myPropertyBlocks;
				this.myPropertyBlocks = this.tmpPropertyBlocks;
				this.tmpPropertyBlocks = hashSet;
				this.batches.Clear();
				this.lastBatchKey = default(DrawBatch.BatchKey);
				this.lastBatchList = null;
			}
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x000474E0 File Offset: 0x000456E0
		private DrawBatch.BatchData GetBatchDataForInsertion(DrawBatch.BatchKey key)
		{
			List<DrawBatch.BatchData> list;
			if (this.lastBatchList != null && key.GetHashCode() == this.lastBatchKey.GetHashCode() && key.Equals(this.lastBatchKey))
			{
				list = this.lastBatchList;
			}
			else
			{
				if (!this.batches.TryGetValue(key, out list))
				{
					list = ((this.batchListCache.Count == 0) ? new List<DrawBatch.BatchData>() : this.batchListCache.Pop<List<DrawBatch.BatchData>>());
					this.batches.Add(key, list);
					list.Add((this.batchDataListCache.Count == 0) ? new DrawBatch.BatchData() : this.batchDataListCache.Pop<DrawBatch.BatchData>());
				}
				this.lastBatchList = list;
				this.lastBatchKey = key;
			}
			int index = list.Count - 1;
			if (list[index].ptr < 1023)
			{
				return list[index];
			}
			DrawBatch.BatchData batchData = (this.batchDataListCache.Count == 0) ? new DrawBatch.BatchData() : this.batchDataListCache.Pop<DrawBatch.BatchData>();
			list.Add(batchData);
			return batchData;
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x000475EC File Offset: 0x000457EC
		public void MergeWith(DrawBatch other)
		{
			foreach (KeyValuePair<DrawBatch.BatchKey, List<DrawBatch.BatchData>> keyValuePair in other.batches)
			{
				foreach (DrawBatch.BatchData batchData in keyValuePair.Value)
				{
					while (batchData.ptr > 0)
					{
						DrawBatch.BatchData batchDataForInsertion = this.GetBatchDataForInsertion(keyValuePair.Key);
						int num = Mathf.Min(batchData.ptr, 1023 - batchDataForInsertion.ptr);
						Array.Copy(batchData.matrices, 0, batchDataForInsertion.matrices, batchDataForInsertion.ptr, num);
						Array.Copy(batchData.colors, 0, batchDataForInsertion.colors, batchDataForInsertion.ptr, num);
						batchDataForInsertion.ptr += num;
						batchData.ptr -= num;
					}
				}
			}
		}

		// Token: 0x04000B89 RID: 2953
		private Dictionary<DrawBatch.BatchKey, List<DrawBatch.BatchData>> batches = new Dictionary<DrawBatch.BatchKey, List<DrawBatch.BatchData>>();

		// Token: 0x04000B8A RID: 2954
		private List<DrawBatch.BatchData> batchDataListCache = new List<DrawBatch.BatchData>();

		// Token: 0x04000B8B RID: 2955
		private List<List<DrawBatch.BatchData>> batchListCache = new List<List<DrawBatch.BatchData>>();

		// Token: 0x04000B8C RID: 2956
		private HashSet<DrawBatchPropertyBlock> myPropertyBlocks = new HashSet<DrawBatchPropertyBlock>();

		// Token: 0x04000B8D RID: 2957
		private List<DrawBatchPropertyBlock> propertyBlockCache = new List<DrawBatchPropertyBlock>();

		// Token: 0x04000B8E RID: 2958
		private MaterialPropertyBlock tmpPropertyBlock;

		// Token: 0x04000B8F RID: 2959
		private HashSet<DrawBatchPropertyBlock> tmpPropertyBlocks = new HashSet<DrawBatchPropertyBlock>();

		// Token: 0x04000B90 RID: 2960
		public const int MaxCountPerBatch = 1023;

		// Token: 0x04000B91 RID: 2961
		private static bool PropertyBlockLeakDebug;

		// Token: 0x04000B92 RID: 2962
		private DrawBatch.BatchKey lastBatchKey;

		// Token: 0x04000B93 RID: 2963
		private List<DrawBatch.BatchData> lastBatchList;

		// Token: 0x02001D53 RID: 7507
		private class BatchData
		{
			// Token: 0x0600B405 RID: 46085 RVA: 0x0040F513 File Offset: 0x0040D713
			public BatchData()
			{
				this.matrices = new Matrix4x4[1023];
				this.colors = new Vector4[1023];
				this.ptr = 0;
			}

			// Token: 0x0600B406 RID: 46086 RVA: 0x0040F542 File Offset: 0x0040D742
			public void Clear()
			{
				this.ptr = 0;
				this.hasAnyColors = false;
			}

			// Token: 0x0600B407 RID: 46087 RVA: 0x0040F552 File Offset: 0x0040D752
			public void Add(Matrix4x4 matrix)
			{
				this.matrices[this.ptr] = matrix;
				this.colors[this.ptr] = DrawBatch.BatchData.WhiteColor;
				this.ptr++;
			}

			// Token: 0x0600B408 RID: 46088 RVA: 0x0040F58C File Offset: 0x0040D78C
			public void Add(Matrix4x4 matrix, Color? color)
			{
				this.matrices[this.ptr] = matrix;
				this.colors[this.ptr] = (color ?? DrawBatch.BatchData.WhiteColor);
				this.ptr++;
				this.hasAnyColors = true;
			}

			// Token: 0x040073CA RID: 29642
			public Matrix4x4[] matrices;

			// Token: 0x040073CB RID: 29643
			public int ptr;

			// Token: 0x040073CC RID: 29644
			public Vector4[] colors;

			// Token: 0x040073CD RID: 29645
			public bool hasAnyColors;

			// Token: 0x040073CE RID: 29646
			private static readonly Vector4 WhiteColor = Color.white;
		}

		// Token: 0x02001D54 RID: 7508
		private struct BatchKey : IEquatable<DrawBatch.BatchKey>
		{
			// Token: 0x0600B40A RID: 46090 RVA: 0x0040F608 File Offset: 0x0040D808
			public BatchKey(Mesh mesh, Material material, int layer, bool renderInstanced, DrawBatchPropertyBlock propertyBlock)
			{
				this.mesh = mesh;
				this.material = material;
				this.layer = layer;
				this.renderInstanced = (renderInstanced && SystemInfo.supportsInstancing);
				this.propertyBlock = propertyBlock;
				this.hash = mesh.GetHashCode();
				this.hash = Gen.HashCombineInt(this.hash, material.GetHashCode());
				this.hash = Gen.HashCombineInt(this.hash, layer | (renderInstanced ? 1 : 0) << 8);
				this.hash = ((propertyBlock == null) ? this.hash : Gen.HashCombineInt(this.hash, propertyBlock.GetHashCode()));
			}

			// Token: 0x0600B40B RID: 46091 RVA: 0x0040F6A8 File Offset: 0x0040D8A8
			public override bool Equals(object obj)
			{
				if (obj != null && obj is DrawBatch.BatchKey)
				{
					DrawBatch.BatchKey other = (DrawBatch.BatchKey)obj;
					return this.Equals(other);
				}
				return false;
			}

			// Token: 0x0600B40C RID: 46092 RVA: 0x0040F6D4 File Offset: 0x0040D8D4
			public bool Equals(DrawBatch.BatchKey other)
			{
				return this.mesh == other.mesh && this.material == other.material && this.layer == other.layer && this.renderInstanced == other.renderInstanced && this.propertyBlock == other.propertyBlock;
			}

			// Token: 0x0600B40D RID: 46093 RVA: 0x0040F729 File Offset: 0x0040D929
			public override int GetHashCode()
			{
				return this.hash;
			}

			// Token: 0x040073CF RID: 29647
			public readonly Mesh mesh;

			// Token: 0x040073D0 RID: 29648
			public readonly Material material;

			// Token: 0x040073D1 RID: 29649
			public readonly int layer;

			// Token: 0x040073D2 RID: 29650
			public readonly bool renderInstanced;

			// Token: 0x040073D3 RID: 29651
			public readonly DrawBatchPropertyBlock propertyBlock;

			// Token: 0x040073D4 RID: 29652
			private int hash;
		}
	}
}
