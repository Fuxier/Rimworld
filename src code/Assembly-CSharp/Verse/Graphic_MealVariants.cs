using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003E1 RID: 993
	public class Graphic_MealVariants : Graphic_StackCount
	{
		// Token: 0x06001C47 RID: 7239 RVA: 0x000ACD36 File Offset: 0x000AAF36
		public override Graphic SubGraphicFor(Thing thing)
		{
			return this.subGraphics[this.SubGraphicTypeIndex(thing) + this.SubGraphicIndexOffset(thing)];
		}

		// Token: 0x06001C48 RID: 7240 RVA: 0x000ACD50 File Offset: 0x000AAF50
		public int SubGraphicTypeIndex(Thing thing)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return 0;
			}
			FoodKind foodKind = FoodUtility.GetFoodKind(thing);
			if (foodKind == FoodKind.Meat)
			{
				return this.subGraphics.Length / 3;
			}
			if (foodKind != FoodKind.NonMeat)
			{
				return 0;
			}
			return this.subGraphics.Length / 3 * 2;
		}

		// Token: 0x06001C49 RID: 7241 RVA: 0x000ACD90 File Offset: 0x000AAF90
		public int SubGraphicIndexOffset(Thing thing)
		{
			if (thing == null)
			{
				return 0;
			}
			switch (this.subGraphics.Length / 3)
			{
			case 1:
				return 0;
			case 2:
				if (thing.stackCount == 1)
				{
					return 0;
				}
				return 1;
			case 3:
				if (thing.stackCount == 1)
				{
					return 0;
				}
				if (thing.stackCount == thing.def.stackLimit)
				{
					return 2;
				}
				return 1;
			default:
				throw new NotImplementedException("More than 3 different stack size meal graphics per meal type not supported");
			}
		}
	}
}
