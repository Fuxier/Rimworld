using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200037F RID: 895
	public static class GlobalTextureAtlasManager
	{
		// Token: 0x060019DC RID: 6620 RVA: 0x0009BEEA File Offset: 0x0009A0EA
		public static void ClearStaticAtlasBuildQueue()
		{
			GlobalTextureAtlasManager.buildQueue.Clear();
		}

		// Token: 0x060019DD RID: 6621 RVA: 0x0009BEF8 File Offset: 0x0009A0F8
		public static bool TryInsertStatic(TextureAtlasGroup group, Texture2D texture, Texture2D mask = null)
		{
			if (texture.width >= 512 || texture.height >= 512)
			{
				return false;
			}
			if (mask != null && (texture.width != mask.width || texture.height != mask.height))
			{
				Log.Warning(string.Concat(new object[]
				{
					"Texture ",
					texture.name,
					" has dimensions of ",
					texture.width,
					" x ",
					texture.height,
					", but its mask has ",
					mask.width,
					" x ",
					mask.height,
					". This is not supported, texture will be excluded from atlas"
				}));
				return false;
			}
			TextureAtlasGroupKey key = new TextureAtlasGroupKey
			{
				group = group,
				hasMask = (mask != null)
			};
			ValueTuple<List<Texture2D>, HashSet<Texture2D>> valueTuple;
			if (!GlobalTextureAtlasManager.buildQueue.TryGetValue(key, out valueTuple))
			{
				valueTuple = new ValueTuple<List<Texture2D>, HashSet<Texture2D>>(new List<Texture2D>(), new HashSet<Texture2D>());
				GlobalTextureAtlasManager.buildQueue.Add(key, valueTuple);
			}
			if (valueTuple.Item2.Add(texture))
			{
				valueTuple.Item1.Add(texture);
			}
			if (mask != null)
			{
				if (GlobalTextureAtlasManager.buildQueueMasks.ContainsKey(texture))
				{
					if (GlobalTextureAtlasManager.buildQueueMasks[texture] != mask)
					{
						Log.Error(string.Concat(new string[]
						{
							"Same texture with 2 different masks inserted into texture atlas manager (",
							texture.name,
							") - ",
							mask.name,
							" | ",
							GlobalTextureAtlasManager.buildQueueMasks[texture].name,
							"!"
						}));
					}
				}
				else
				{
					GlobalTextureAtlasManager.buildQueueMasks.Add(texture, mask);
				}
			}
			return true;
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x0009C0C8 File Offset: 0x0009A2C8
		public static void BakeStaticAtlases()
		{
			BuildingsDamageSectionLayerUtility.TryInsertIntoAtlas();
			MinifiedThing.TryInsertIntoAtlas();
			GlobalTextureAtlasManager.<>c__DisplayClass7_0 CS$<>8__locals1;
			CS$<>8__locals1.pixels = 0;
			CS$<>8__locals1.currentBatch = new List<Texture2D>();
			foreach (KeyValuePair<TextureAtlasGroupKey, ValueTuple<List<Texture2D>, HashSet<Texture2D>>> keyValuePair in GlobalTextureAtlasManager.buildQueue)
			{
				foreach (Texture2D texture2D in keyValuePair.Value.Item1)
				{
					int num = texture2D.width * texture2D.height;
					if (num + CS$<>8__locals1.pixels > StaticTextureAtlas.MaxPixelsPerAtlas)
					{
						GlobalTextureAtlasManager.<BakeStaticAtlases>g__FlushBatch|7_0(keyValuePair.Key, ref CS$<>8__locals1);
					}
					CS$<>8__locals1.pixels += num;
					CS$<>8__locals1.currentBatch.Add(texture2D);
				}
				GlobalTextureAtlasManager.<BakeStaticAtlases>g__FlushBatch|7_0(keyValuePair.Key, ref CS$<>8__locals1);
			}
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x0009C1D4 File Offset: 0x0009A3D4
		public static bool TryGetStaticTile(TextureAtlasGroup group, Texture2D texture, out StaticTextureAtlasTile tile, bool ignoreFoundInOtherAtlas = false)
		{
			foreach (StaticTextureAtlas staticTextureAtlas in GlobalTextureAtlasManager.staticTextureAtlases)
			{
				if (staticTextureAtlas.groupKey.group == group && staticTextureAtlas.TryGetTile(texture, out tile))
				{
					return true;
				}
			}
			foreach (StaticTextureAtlas staticTextureAtlas2 in GlobalTextureAtlasManager.staticTextureAtlases)
			{
				if (staticTextureAtlas2.TryGetTile(texture, out tile))
				{
					if (!ignoreFoundInOtherAtlas)
					{
						Log.Warning(string.Concat(new object[]
						{
							"Found texture ",
							texture.name,
							" in another atlas group than requested (found in ",
							staticTextureAtlas2.groupKey,
							", requested in ",
							group,
							")!"
						}));
					}
					return true;
				}
			}
			tile = null;
			return false;
		}

		// Token: 0x060019E0 RID: 6624 RVA: 0x0009C2E0 File Offset: 0x0009A4E0
		public static bool TryGetPawnFrameSet(Pawn pawn, out PawnTextureAtlasFrameSet frameSet, out bool createdNew, bool allowCreatingNew = true)
		{
			using (List<PawnTextureAtlas>.Enumerator enumerator = GlobalTextureAtlasManager.pawnTextureAtlases.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.TryGetFrameSet(pawn, out frameSet, out createdNew))
					{
						return true;
					}
				}
			}
			if (allowCreatingNew)
			{
				PawnTextureAtlas pawnTextureAtlas = new PawnTextureAtlas();
				GlobalTextureAtlasManager.pawnTextureAtlases.Add(pawnTextureAtlas);
				return pawnTextureAtlas.TryGetFrameSet(pawn, out frameSet, out createdNew);
			}
			createdNew = false;
			frameSet = null;
			return false;
		}

		// Token: 0x060019E1 RID: 6625 RVA: 0x0009C360 File Offset: 0x0009A560
		public static bool TryMarkPawnFrameSetDirty(Pawn pawn)
		{
			PawnTextureAtlasFrameSet pawnTextureAtlasFrameSet;
			bool flag;
			if (!GlobalTextureAtlasManager.TryGetPawnFrameSet(pawn, out pawnTextureAtlasFrameSet, out flag, false))
			{
				return false;
			}
			for (int i = 0; i < pawnTextureAtlasFrameSet.isDirty.Length; i++)
			{
				pawnTextureAtlasFrameSet.isDirty[i] = true;
			}
			return true;
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x0009C39C File Offset: 0x0009A59C
		public static void GlobalTextureAtlasManagerUpdate()
		{
			if (GlobalTextureAtlasManager.rebakeAtlas)
			{
				GlobalTextureAtlasManager.FreeAllRuntimeAtlases();
				PortraitsCache.Clear();
				GlobalTextureAtlasManager.rebakeAtlas = false;
			}
			foreach (PawnTextureAtlas pawnTextureAtlas in GlobalTextureAtlasManager.pawnTextureAtlases)
			{
				pawnTextureAtlas.GC();
			}
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x0009C404 File Offset: 0x0009A604
		public static void FreeAllRuntimeAtlases()
		{
			foreach (PawnTextureAtlas pawnTextureAtlas in GlobalTextureAtlasManager.pawnTextureAtlases)
			{
				pawnTextureAtlas.Destroy();
			}
			GlobalTextureAtlasManager.pawnTextureAtlases.Clear();
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x0009C460 File Offset: 0x0009A660
		public static void DumpPawnAtlases(string folder)
		{
			foreach (PawnTextureAtlas pawnTextureAtlas in GlobalTextureAtlasManager.pawnTextureAtlases)
			{
				TextureAtlasHelper.WriteDebugPNG(pawnTextureAtlas.RawTexture, string.Concat(new object[]
				{
					folder,
					"\\dump_",
					pawnTextureAtlas.RawTexture.GetInstanceID(),
					".png"
				}));
			}
			Log.Message("Atlases have been dumped to " + folder);
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x0009C4F8 File Offset: 0x0009A6F8
		public static void DumpStaticAtlases(string folder)
		{
			foreach (StaticTextureAtlas staticTextureAtlas in GlobalTextureAtlasManager.staticTextureAtlases)
			{
				TextureAtlasHelper.WriteDebugPNG(staticTextureAtlas.ColorTexture, folder + "\\" + staticTextureAtlas.ColorTexture.name + ".png");
				if (staticTextureAtlas.MaskTexture != null)
				{
					TextureAtlasHelper.WriteDebugPNG(staticTextureAtlas.MaskTexture, folder + "\\" + staticTextureAtlas.MaskTexture.name + ".png");
				}
			}
			Log.Message("Atlases have been dumped to " + folder);
		}

		// Token: 0x060019E7 RID: 6631 RVA: 0x0009C5E0 File Offset: 0x0009A7E0
		[CompilerGenerated]
		internal static void <BakeStaticAtlases>g__FlushBatch|7_0(TextureAtlasGroupKey groupKey, ref GlobalTextureAtlasManager.<>c__DisplayClass7_0 A_1)
		{
			StaticTextureAtlas staticTextureAtlas = new StaticTextureAtlas(groupKey);
			foreach (Texture2D texture2D in A_1.currentBatch)
			{
				Texture2D mask;
				if (!groupKey.hasMask || !GlobalTextureAtlasManager.buildQueueMasks.TryGetValue(texture2D, out mask))
				{
					mask = null;
				}
				staticTextureAtlas.Insert(texture2D, mask);
			}
			staticTextureAtlas.Bake(false);
			GlobalTextureAtlasManager.staticTextureAtlases.Add(staticTextureAtlas);
			A_1.pixels = 0;
			A_1.currentBatch.Clear();
		}

		// Token: 0x040012DA RID: 4826
		public static bool rebakeAtlas = false;

		// Token: 0x040012DB RID: 4827
		private static List<PawnTextureAtlas> pawnTextureAtlases = new List<PawnTextureAtlas>();

		// Token: 0x040012DC RID: 4828
		private static List<StaticTextureAtlas> staticTextureAtlases = new List<StaticTextureAtlas>();

		// Token: 0x040012DD RID: 4829
		private static Dictionary<TextureAtlasGroupKey, ValueTuple<List<Texture2D>, HashSet<Texture2D>>> buildQueue = new Dictionary<TextureAtlasGroupKey, ValueTuple<List<Texture2D>, HashSet<Texture2D>>>();

		// Token: 0x040012DE RID: 4830
		private static Dictionary<Texture2D, Texture2D> buildQueueMasks = new Dictionary<Texture2D, Texture2D>();
	}
}
