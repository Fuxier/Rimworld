using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001CB RID: 459
	public abstract class FleckSystem : IExposable, ILoadReferenceable
	{
		// Token: 0x06000CD4 RID: 3284
		public abstract void Update();

		// Token: 0x06000CD5 RID: 3285
		public abstract void Tick();

		// Token: 0x06000CD6 RID: 3286
		public abstract void Draw(DrawBatch drawBatch);

		// Token: 0x06000CD7 RID: 3287 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void OnGUI()
		{
		}

		// Token: 0x06000CD8 RID: 3288
		public abstract void CreateFleck(FleckCreationData fleckData);

		// Token: 0x06000CD9 RID: 3289
		public abstract void ExposeData();

		// Token: 0x06000CDA RID: 3290 RVA: 0x00047CB3 File Offset: 0x00045EB3
		public string GetUniqueLoadID()
		{
			return this.parent.parent.GetUniqueLoadID() + "_FleckSystem_" + base.GetType().FullName;
		}

		// Token: 0x04000BC1 RID: 3009
		public List<FleckDef> handledDefs = new List<FleckDef>();

		// Token: 0x04000BC2 RID: 3010
		public FleckManager parent;
	}
}
