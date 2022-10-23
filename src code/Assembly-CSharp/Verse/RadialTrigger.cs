using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003C9 RID: 969
	public class RadialTrigger : PawnTrigger
	{
		// Token: 0x06001BB1 RID: 7089 RVA: 0x000AA24C File Offset: 0x000A844C
		public override void Tick()
		{
			if (this.IsHashIntervalTick(60))
			{
				Map map = base.Map;
				int num = GenRadial.NumCellsInRadius((float)this.maxRadius);
				for (int i = 0; i < num; i++)
				{
					IntVec3 c = base.Position + GenRadial.RadialPattern[i];
					if (c.InBounds(map))
					{
						List<Thing> thingList = c.GetThingList(map);
						for (int j = 0; j < thingList.Count; j++)
						{
							if (base.TriggeredBy(thingList[j]) && (!this.lineOfSight || GenSight.LineOfSightToThing(base.Position, thingList[j], map, false, null)))
							{
								base.ActivatedBy((Pawn)thingList[j]);
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x06001BB2 RID: 7090 RVA: 0x000AA312 File Offset: 0x000A8512
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.maxRadius, "maxRadius", 0, false);
			Scribe_Values.Look<bool>(ref this.lineOfSight, "lineOfSight", false, false);
		}

		// Token: 0x04001401 RID: 5121
		public int maxRadius;

		// Token: 0x04001402 RID: 5122
		public bool lineOfSight;
	}
}
