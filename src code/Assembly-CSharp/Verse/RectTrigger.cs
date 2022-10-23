using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003CA RID: 970
	public class RectTrigger : PawnTrigger
	{
		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06001BB4 RID: 7092 RVA: 0x000AA346 File Offset: 0x000A8546
		// (set) Token: 0x06001BB5 RID: 7093 RVA: 0x000AA34E File Offset: 0x000A854E
		public CellRect Rect
		{
			get
			{
				return this.rect;
			}
			set
			{
				this.rect = value;
				if (base.Spawned)
				{
					this.rect.ClipInsideMap(base.Map);
				}
			}
		}

		// Token: 0x06001BB6 RID: 7094 RVA: 0x000AA371 File Offset: 0x000A8571
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.rect.ClipInsideMap(base.Map);
		}

		// Token: 0x06001BB7 RID: 7095 RVA: 0x000AA390 File Offset: 0x000A8590
		public override void Tick()
		{
			if (this.destroyIfUnfogged && !this.rect.CenterCell.Fogged(base.Map))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (this.IsHashIntervalTick(60) && !base.Destroyed)
			{
				Map map = base.Map;
				for (int i = this.rect.minZ; i <= this.rect.maxZ; i++)
				{
					for (int j = this.rect.minX; j <= this.rect.maxX; j++)
					{
						List<Thing> thingList = new IntVec3(j, 0, i).GetThingList(map);
						for (int k = 0; k < thingList.Count; k++)
						{
							if (base.TriggeredBy(thingList[k]))
							{
								base.ActivatedBy((Pawn)thingList[k]);
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x06001BB8 RID: 7096 RVA: 0x000AA46C File Offset: 0x000A866C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<CellRect>(ref this.rect, "rect", default(CellRect), false);
			Scribe_Values.Look<bool>(ref this.destroyIfUnfogged, "destroyIfUnfogged", false, false);
			Scribe_Values.Look<bool>(ref this.activateOnExplosion, "activateOnExplosion", false, false);
		}

		// Token: 0x04001403 RID: 5123
		private CellRect rect;

		// Token: 0x04001404 RID: 5124
		public bool destroyIfUnfogged;

		// Token: 0x04001405 RID: 5125
		public bool activateOnExplosion;
	}
}
