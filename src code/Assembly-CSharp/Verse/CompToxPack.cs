using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200040F RID: 1039
	public class CompToxPack : CompAIUsablePack
	{
		// Token: 0x06001E8A RID: 7818 RVA: 0x000B6E3C File Offset: 0x000B503C
		protected override float ChanceToUse(Pawn wearer)
		{
			if (!ModsConfig.BiotechActive)
			{
				return 0f;
			}
			int num = GenRadial.NumCellsInRadius(1.9f);
			float num2 = 0f;
			for (int i = 0; i < num; i++)
			{
				IntVec3 c = wearer.Position + GenRadial.RadialPattern[i];
				if (c.InBounds(wearer.Map))
				{
					List<Thing> thingList = c.GetThingList(wearer.Map);
					for (int j = 0; j < thingList.Count; j++)
					{
						Pawn pawn;
						if ((pawn = (thingList[j] as Pawn)) != null && pawn != wearer && pawn.HostileTo(wearer))
						{
							num2 += pawn.BodySize;
							if (num2 >= 1f)
							{
								break;
							}
						}
					}
				}
			}
			return num2;
		}

		// Token: 0x06001E8B RID: 7819 RVA: 0x000B6EF7 File Offset: 0x000B50F7
		protected override void UsePack(Pawn wearer)
		{
			Verb_DeployToxPack.TryDeploy(this.parent.TryGetComp<CompReloadable>(), this.parent.TryGetComp<CompReleaseGas>());
		}
	}
}
