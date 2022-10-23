using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000219 RID: 537
	public abstract class SectionLayer_Things : SectionLayer
	{
		// Token: 0x06000F5E RID: 3934 RVA: 0x00059156 File Offset: 0x00057356
		public SectionLayer_Things(Section section) : base(section)
		{
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x0005915F File Offset: 0x0005735F
		public override void DrawLayer()
		{
			if (!DebugViewSettings.drawThingsPrinted)
			{
				return;
			}
			base.DrawLayer();
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x00059170 File Offset: 0x00057370
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			foreach (IntVec3 intVec in this.section.CellRect)
			{
				List<Thing> list = base.Map.thingGrid.ThingsListAt(intVec);
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					Thing thing = list[i];
					if ((thing.def.seeThroughFog || !base.Map.fogGrid.fogGrid[CellIndicesUtility.CellToIndex(thing.Position, base.Map.Size.x)]) && thing.def.drawerType != DrawerType.None && (thing.def.drawerType != DrawerType.RealtimeOnly || !this.requireAddToMapMesh) && (thing.def.hideAtSnowDepth >= 1f || base.Map.snowGrid.GetDepth(thing.Position) <= thing.def.hideAtSnowDepth) && thing.Position.x == intVec.x && thing.Position.z == intVec.z)
					{
						this.TakePrintFrom(thing);
					}
				}
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x06000F61 RID: 3937
		protected abstract void TakePrintFrom(Thing t);

		// Token: 0x04000DBA RID: 3514
		protected bool requireAddToMapMesh;
	}
}
