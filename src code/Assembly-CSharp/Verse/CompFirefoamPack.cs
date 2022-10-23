using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200040E RID: 1038
	public class CompFirefoamPack : CompAIUsablePack
	{
		// Token: 0x06001E87 RID: 7815 RVA: 0x000B6D5C File Offset: 0x000B4F5C
		protected override float ChanceToUse(Pawn wearer)
		{
			if (wearer.GetAttachment(ThingDefOf.Fire) != null)
			{
				return 1f;
			}
			int num = GenRadial.NumCellsInRadius(1.9f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 c = wearer.Position + GenRadial.RadialPattern[i];
				if (c.InBounds(wearer.Map))
				{
					List<Thing> thingList = c.GetThingList(wearer.Map);
					int j = 0;
					while (j < thingList.Count)
					{
						if (thingList[j] is Fire || thingList[j].HasAttachment(ThingDefOf.Fire))
						{
							if (i == 0)
							{
								return 1f;
							}
							return 0.2f;
						}
						else
						{
							j++;
						}
					}
				}
			}
			return 0f;
		}

		// Token: 0x06001E88 RID: 7816 RVA: 0x000B6E14 File Offset: 0x000B5014
		protected override void UsePack(Pawn wearer)
		{
			Verb_FirefoamPop.Pop(wearer, this.parent.TryGetComp<CompExplosive>(), this.parent.TryGetComp<CompReloadable>());
		}

		// Token: 0x040014DF RID: 5343
		private const float ChanceToUseWithNearbyFire = 0.2f;
	}
}
