using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000361 RID: 865
	public class PawnCapacitiesHandler
	{
		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06001736 RID: 5942 RVA: 0x0008851B File Offset: 0x0008671B
		public bool CanBeAwake
		{
			get
			{
				return this.GetLevel(PawnCapacityDefOf.Consciousness) >= 0.3f;
			}
		}

		// Token: 0x06001737 RID: 5943 RVA: 0x00088532 File Offset: 0x00086732
		public PawnCapacitiesHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x00088541 File Offset: 0x00086741
		public void Clear()
		{
			this.cachedCapacityLevels = null;
		}

		// Token: 0x06001739 RID: 5945 RVA: 0x0008854C File Offset: 0x0008674C
		public float GetLevel(PawnCapacityDef capacity)
		{
			if (this.pawn.health.Dead)
			{
				return 0f;
			}
			if (this.cachedCapacityLevels == null)
			{
				this.Notify_CapacityLevelsDirty();
			}
			PawnCapacitiesHandler.CacheElement cacheElement = this.cachedCapacityLevels[capacity];
			if (cacheElement.status == PawnCapacitiesHandler.CacheStatus.Caching)
			{
				Log.Error(string.Format("Detected infinite stat recursion when evaluating {0}", capacity));
				return 0f;
			}
			if (cacheElement.status == PawnCapacitiesHandler.CacheStatus.Uncached)
			{
				cacheElement.status = PawnCapacitiesHandler.CacheStatus.Caching;
				try
				{
					cacheElement.value = PawnCapacityUtility.CalculateCapacityLevel(this.pawn.health.hediffSet, capacity, null, false);
				}
				finally
				{
					cacheElement.status = PawnCapacitiesHandler.CacheStatus.Cached;
				}
			}
			return cacheElement.value;
		}

		// Token: 0x0600173A RID: 5946 RVA: 0x000885F8 File Offset: 0x000867F8
		public bool CapableOf(PawnCapacityDef capacity)
		{
			return this.GetLevel(capacity) > capacity.minForCapable;
		}

		// Token: 0x0600173B RID: 5947 RVA: 0x0008860C File Offset: 0x0008680C
		public void Notify_CapacityLevelsDirty()
		{
			if (this.cachedCapacityLevels == null)
			{
				this.cachedCapacityLevels = new DefMap<PawnCapacityDef, PawnCapacitiesHandler.CacheElement>();
			}
			for (int i = 0; i < this.cachedCapacityLevels.Count; i++)
			{
				this.cachedCapacityLevels[i].status = PawnCapacitiesHandler.CacheStatus.Uncached;
			}
		}

		// Token: 0x040011D0 RID: 4560
		private Pawn pawn;

		// Token: 0x040011D1 RID: 4561
		private DefMap<PawnCapacityDef, PawnCapacitiesHandler.CacheElement> cachedCapacityLevels;

		// Token: 0x02001E24 RID: 7716
		private enum CacheStatus
		{
			// Token: 0x040076F2 RID: 30450
			Uncached,
			// Token: 0x040076F3 RID: 30451
			Caching,
			// Token: 0x040076F4 RID: 30452
			Cached
		}

		// Token: 0x02001E25 RID: 7717
		private class CacheElement
		{
			// Token: 0x040076F5 RID: 30453
			public PawnCapacitiesHandler.CacheStatus status;

			// Token: 0x040076F6 RID: 30454
			public float value;
		}
	}
}
