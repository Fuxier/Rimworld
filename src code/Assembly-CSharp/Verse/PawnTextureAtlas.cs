using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000383 RID: 899
	public class PawnTextureAtlas
	{
		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x060019EF RID: 6639 RVA: 0x0009C96D File Offset: 0x0009AB6D
		public RenderTexture RawTexture
		{
			get
			{
				return this.texture;
			}
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x060019F0 RID: 6640 RVA: 0x0009C975 File Offset: 0x0009AB75
		public int FreeCount
		{
			get
			{
				return this.freeFrameSets.Count;
			}
		}

		// Token: 0x060019F1 RID: 6641 RVA: 0x0009C984 File Offset: 0x0009AB84
		public PawnTextureAtlas()
		{
			this.texture = new RenderTexture(2048, 2048, 24, RenderTextureFormat.ARGB32, 0);
			this.texture.name = "PawnTextureAtlas_" + 2048;
			List<Rect> list = new List<Rect>();
			for (int i = 0; i < 2048; i += 128)
			{
				for (int j = 0; j < 2048; j += 128)
				{
					list.Add(new Rect((float)i / 2048f, (float)j / 2048f, 0.0625f, 0.0625f));
				}
			}
			while (list.Count >= 8)
			{
				PawnTextureAtlasFrameSet pawnTextureAtlasFrameSet = new PawnTextureAtlasFrameSet();
				pawnTextureAtlasFrameSet.uvRects = new Rect[]
				{
					list.Pop<Rect>(),
					list.Pop<Rect>(),
					list.Pop<Rect>(),
					list.Pop<Rect>(),
					list.Pop<Rect>(),
					list.Pop<Rect>(),
					list.Pop<Rect>(),
					list.Pop<Rect>()
				};
				pawnTextureAtlasFrameSet.meshes = (from u in pawnTextureAtlasFrameSet.uvRects
				select TextureAtlasHelper.CreateMeshForUV(u, 1f)).ToArray<Mesh>();
				pawnTextureAtlasFrameSet.atlas = this.texture;
				this.freeFrameSets.Add(pawnTextureAtlasFrameSet);
			}
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x0009CB10 File Offset: 0x0009AD10
		public bool ContainsFrameSet(Pawn pawn)
		{
			return this.frameAssignments.ContainsKey(pawn);
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x0009CB20 File Offset: 0x0009AD20
		public bool TryGetFrameSet(Pawn pawn, out PawnTextureAtlasFrameSet frameSet, out bool createdNew)
		{
			createdNew = false;
			if (this.frameAssignments.TryGetValue(pawn, out frameSet))
			{
				return true;
			}
			if (this.FreeCount == 0)
			{
				return false;
			}
			createdNew = true;
			frameSet = this.freeFrameSets.Pop<PawnTextureAtlasFrameSet>();
			for (int i = 0; i < frameSet.isDirty.Length; i++)
			{
				frameSet.isDirty[i] = true;
			}
			this.frameAssignments.Add(pawn, frameSet);
			return true;
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x0009CB88 File Offset: 0x0009AD88
		public void GC()
		{
			try
			{
				foreach (Pawn pawn in this.frameAssignments.Keys)
				{
					if (!pawn.SpawnedOrAnyParentSpawned)
					{
						PawnTextureAtlas.tmpPawnsToFree.Add(pawn);
					}
				}
				foreach (Pawn key in PawnTextureAtlas.tmpPawnsToFree)
				{
					this.freeFrameSets.Add(this.frameAssignments[key]);
					this.frameAssignments.Remove(key);
				}
			}
			finally
			{
				PawnTextureAtlas.tmpPawnsToFree.Clear();
			}
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x0009CC64 File Offset: 0x0009AE64
		public void Destroy()
		{
			foreach (PawnTextureAtlasFrameSet pawnTextureAtlasFrameSet in this.frameAssignments.Values.Concat(this.freeFrameSets))
			{
				Mesh[] meshes = pawnTextureAtlasFrameSet.meshes;
				for (int i = 0; i < meshes.Length; i++)
				{
					UnityEngine.Object.Destroy(meshes[i]);
				}
			}
			this.frameAssignments.Clear();
			this.freeFrameSets.Clear();
			UnityEngine.Object.Destroy(this.texture);
		}

		// Token: 0x040012E6 RID: 4838
		private RenderTexture texture;

		// Token: 0x040012E7 RID: 4839
		private Dictionary<Pawn, PawnTextureAtlasFrameSet> frameAssignments = new Dictionary<Pawn, PawnTextureAtlasFrameSet>();

		// Token: 0x040012E8 RID: 4840
		private List<PawnTextureAtlasFrameSet> freeFrameSets = new List<PawnTextureAtlasFrameSet>();

		// Token: 0x040012E9 RID: 4841
		private static List<Pawn> tmpPawnsToFree = new List<Pawn>();

		// Token: 0x040012EA RID: 4842
		private const int AtlasSize = 2048;

		// Token: 0x040012EB RID: 4843
		public const int FrameSize = 128;

		// Token: 0x040012EC RID: 4844
		private const int PawnsHeldPerAtlas = 32;

		// Token: 0x040012ED RID: 4845
		private const int FramesPerPawn = 8;
	}
}
