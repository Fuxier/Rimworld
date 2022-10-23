using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000301 RID: 769
	[StaticConstructorOnStartup]
	public class HediffComp_Lactating : HediffComp_Chargeable
	{
		// Token: 0x06001530 RID: 5424 RVA: 0x0007F990 File Offset: 0x0007DB90
		public override void TryCharge(float desiredChargeAmount)
		{
			Pawn pawn = base.Pawn;
			bool flag;
			if (pawn == null)
			{
				flag = (null != null);
			}
			else
			{
				Pawn_NeedsTracker needs = pawn.needs;
				flag = (((needs != null) ? needs.food : null) != null);
			}
			if (!flag)
			{
				return;
			}
			desiredChargeAmount *= PawnUtility.BodyResourceGrowthSpeed(base.Pawn);
			float num = Mathf.Min(desiredChargeAmount, base.Pawn.needs.food.CurLevel);
			base.Pawn.needs.food.CurLevel -= num;
			base.TryCharge(num);
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x0007FA0D File Offset: 0x0007DC0D
		public float AddedNutritionPerDay()
		{
			return base.Props.fullChargeAmount * 60000f / (float)base.Props.ticksToFullCharge;
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06001532 RID: 5426 RVA: 0x0007FA30 File Offset: 0x0007DC30
		public override string CompTipStringExtra
		{
			get
			{
				if (base.Charge >= base.Props.fullChargeAmount)
				{
					return "LactatingStoppedBecauseFull".Translate();
				}
				float num = PawnUtility.BodyResourceGrowthSpeed(base.Pawn);
				if (num == 0f)
				{
					return "LactatingStoppedBecauseHungry".Translate().Colorize(ColorLibrary.RedReadable);
				}
				float f = this.AddedNutritionPerDay() * num;
				return "LactatingAddedNutritionPerDay".Translate(f.ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute), num);
			}
		}
	}
}
