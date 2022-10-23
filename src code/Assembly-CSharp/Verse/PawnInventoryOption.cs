using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200011A RID: 282
	public class PawnInventoryOption
	{
		// Token: 0x0600075C RID: 1884 RVA: 0x0002639E File Offset: 0x0002459E
		public IEnumerable<Thing> GenerateThings()
		{
			if (Rand.Value < this.skipChance)
			{
				yield break;
			}
			if (this.thingDef != null && this.countRange.max > 0)
			{
				Thing thing = ThingMaker.MakeThing(this.thingDef, null);
				thing.stackCount = this.countRange.RandomInRange;
				yield return thing;
			}
			if (this.subOptionsTakeAll != null)
			{
				foreach (PawnInventoryOption pawnInventoryOption in this.subOptionsTakeAll)
				{
					foreach (Thing thing2 in pawnInventoryOption.GenerateThings())
					{
						yield return thing2;
					}
					IEnumerator<Thing> enumerator2 = null;
				}
				List<PawnInventoryOption>.Enumerator enumerator = default(List<PawnInventoryOption>.Enumerator);
			}
			if (this.subOptionsChooseOne != null)
			{
				PawnInventoryOption pawnInventoryOption2 = this.subOptionsChooseOne.RandomElementByWeight((PawnInventoryOption o) => o.choiceChance);
				foreach (Thing thing3 in pawnInventoryOption2.GenerateThings())
				{
					yield return thing3;
				}
				IEnumerator<Thing> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x04000741 RID: 1857
		public ThingDef thingDef;

		// Token: 0x04000742 RID: 1858
		public IntRange countRange = IntRange.one;

		// Token: 0x04000743 RID: 1859
		public float choiceChance = 1f;

		// Token: 0x04000744 RID: 1860
		public float skipChance;

		// Token: 0x04000745 RID: 1861
		public List<PawnInventoryOption> subOptionsTakeAll;

		// Token: 0x04000746 RID: 1862
		public List<PawnInventoryOption> subOptionsChooseOne;
	}
}
