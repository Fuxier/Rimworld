using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200025B RID: 603
	public class StorageGroupManager : IExposable
	{
		// Token: 0x0600112E RID: 4398 RVA: 0x0006440F File Offset: 0x0006260F
		public StorageGroupManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x0006442C File Offset: 0x0006262C
		public StorageGroup NewGroup()
		{
			StorageGroup storageGroup = new StorageGroup(this.map);
			storageGroup.loadID = Find.UniqueIDsManager.GetNextStorageGroupID();
			this.groups.Add(storageGroup);
			return storageGroup;
		}

		// Token: 0x06001130 RID: 4400 RVA: 0x00064464 File Offset: 0x00062664
		public void Notify_MemberRemoved(StorageGroup group)
		{
			if (group.MemberCount <= 1)
			{
				for (int i = group.MemberCount - 1; i >= 0; i--)
				{
					group.members[i].SetStorageGroup(null);
				}
				this.groups.Remove(group);
			}
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x000644AC File Offset: 0x000626AC
		public void ExposeData()
		{
			Scribe_Collections.Look<StorageGroup>(ref this.groups, "groups", LookMode.Deep, Array.Empty<object>());
			Scribe_References.Look<Map>(ref this.map, "map", false);
		}

		// Token: 0x04000EC6 RID: 3782
		public Map map;

		// Token: 0x04000EC7 RID: 3783
		private List<StorageGroup> groups = new List<StorageGroup>();
	}
}
