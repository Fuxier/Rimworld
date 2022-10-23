using System;

namespace Verse
{
	// Token: 0x020001FA RID: 506
	public abstract class MapComponent : IExposable
	{
		// Token: 0x06000EC3 RID: 3779 RVA: 0x00051C3B File Offset: 0x0004FE3B
		public MapComponent(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void MapComponentUpdate()
		{
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void MapComponentTick()
		{
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void MapComponentOnGUI()
		{
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void ExposeData()
		{
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void FinalizeInit()
		{
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void MapGenerated()
		{
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void MapRemoved()
		{
		}

		// Token: 0x04000D47 RID: 3399
		public Map map;
	}
}
