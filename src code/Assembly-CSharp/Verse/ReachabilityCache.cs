using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000236 RID: 566
	public class ReachabilityCache
	{
		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000FF3 RID: 4083 RVA: 0x0005CAD0 File Offset: 0x0005ACD0
		public int Count
		{
			get
			{
				return this.cacheDict.Count;
			}
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0005CAE0 File Offset: 0x0005ACE0
		public BoolUnknown CachedResultFor(District A, District B, TraverseParms traverseParams)
		{
			bool flag;
			if (!this.cacheDict.TryGetValue(new ReachabilityCache.CachedEntry(A.ID, B.ID, traverseParams), out flag))
			{
				return BoolUnknown.Unknown;
			}
			if (!flag)
			{
				return BoolUnknown.False;
			}
			return BoolUnknown.True;
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x0005CB18 File Offset: 0x0005AD18
		public void AddCachedResult(District A, District B, TraverseParms traverseParams, bool reachable)
		{
			ReachabilityCache.CachedEntry key = new ReachabilityCache.CachedEntry(A.ID, B.ID, traverseParams);
			if (!this.cacheDict.ContainsKey(key))
			{
				this.cacheDict.Add(key, reachable);
			}
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x0005CB55 File Offset: 0x0005AD55
		public void Clear()
		{
			this.cacheDict.Clear();
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x0005CB64 File Offset: 0x0005AD64
		public void ClearFor(Pawn p)
		{
			ReachabilityCache.tmpCachedEntries.Clear();
			foreach (KeyValuePair<ReachabilityCache.CachedEntry, bool> keyValuePair in this.cacheDict)
			{
				if (keyValuePair.Key.TraverseParms.pawn == p)
				{
					ReachabilityCache.tmpCachedEntries.Add(keyValuePair.Key);
				}
			}
			for (int i = 0; i < ReachabilityCache.tmpCachedEntries.Count; i++)
			{
				this.cacheDict.Remove(ReachabilityCache.tmpCachedEntries[i]);
			}
			ReachabilityCache.tmpCachedEntries.Clear();
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x0005CC18 File Offset: 0x0005AE18
		public void ClearForHostile(Thing hostileTo)
		{
			ReachabilityCache.tmpCachedEntries.Clear();
			foreach (KeyValuePair<ReachabilityCache.CachedEntry, bool> keyValuePair in this.cacheDict)
			{
				Pawn pawn = keyValuePair.Key.TraverseParms.pawn;
				if (pawn != null && pawn.HostileTo(hostileTo))
				{
					ReachabilityCache.tmpCachedEntries.Add(keyValuePair.Key);
				}
			}
			for (int i = 0; i < ReachabilityCache.tmpCachedEntries.Count; i++)
			{
				this.cacheDict.Remove(ReachabilityCache.tmpCachedEntries[i]);
			}
			ReachabilityCache.tmpCachedEntries.Clear();
		}

		// Token: 0x04000E2A RID: 3626
		private Dictionary<ReachabilityCache.CachedEntry, bool> cacheDict = new Dictionary<ReachabilityCache.CachedEntry, bool>();

		// Token: 0x04000E2B RID: 3627
		private static List<ReachabilityCache.CachedEntry> tmpCachedEntries = new List<ReachabilityCache.CachedEntry>();

		// Token: 0x02001D91 RID: 7569
		private struct CachedEntry : IEquatable<ReachabilityCache.CachedEntry>
		{
			// Token: 0x17001E4B RID: 7755
			// (get) Token: 0x0600B4EA RID: 46314 RVA: 0x00411FB7 File Offset: 0x004101B7
			// (set) Token: 0x0600B4EB RID: 46315 RVA: 0x00411FBF File Offset: 0x004101BF
			public int FirstID { get; private set; }

			// Token: 0x17001E4C RID: 7756
			// (get) Token: 0x0600B4EC RID: 46316 RVA: 0x00411FC8 File Offset: 0x004101C8
			// (set) Token: 0x0600B4ED RID: 46317 RVA: 0x00411FD0 File Offset: 0x004101D0
			public int SecondID { get; private set; }

			// Token: 0x17001E4D RID: 7757
			// (get) Token: 0x0600B4EE RID: 46318 RVA: 0x00411FD9 File Offset: 0x004101D9
			// (set) Token: 0x0600B4EF RID: 46319 RVA: 0x00411FE1 File Offset: 0x004101E1
			public TraverseParms TraverseParms { get; private set; }

			// Token: 0x0600B4F0 RID: 46320 RVA: 0x00411FEA File Offset: 0x004101EA
			public CachedEntry(int firstID, int secondID, TraverseParms traverseParms)
			{
				this = default(ReachabilityCache.CachedEntry);
				if (firstID < secondID)
				{
					this.FirstID = firstID;
					this.SecondID = secondID;
				}
				else
				{
					this.FirstID = secondID;
					this.SecondID = firstID;
				}
				this.TraverseParms = traverseParms;
			}

			// Token: 0x0600B4F1 RID: 46321 RVA: 0x0041201C File Offset: 0x0041021C
			public static bool operator ==(ReachabilityCache.CachedEntry lhs, ReachabilityCache.CachedEntry rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x0600B4F2 RID: 46322 RVA: 0x00412026 File Offset: 0x00410226
			public static bool operator !=(ReachabilityCache.CachedEntry lhs, ReachabilityCache.CachedEntry rhs)
			{
				return !lhs.Equals(rhs);
			}

			// Token: 0x0600B4F3 RID: 46323 RVA: 0x00412033 File Offset: 0x00410233
			public override bool Equals(object obj)
			{
				return obj is ReachabilityCache.CachedEntry && this.Equals((ReachabilityCache.CachedEntry)obj);
			}

			// Token: 0x0600B4F4 RID: 46324 RVA: 0x0041204B File Offset: 0x0041024B
			public bool Equals(ReachabilityCache.CachedEntry other)
			{
				return this.FirstID == other.FirstID && this.SecondID == other.SecondID && this.TraverseParms == other.TraverseParms;
			}

			// Token: 0x0600B4F5 RID: 46325 RVA: 0x0041207F File Offset: 0x0041027F
			public override int GetHashCode()
			{
				return Gen.HashCombineStruct<TraverseParms>(Gen.HashCombineInt(this.FirstID, this.SecondID), this.TraverseParms);
			}
		}
	}
}
