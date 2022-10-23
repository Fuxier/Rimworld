using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000337 RID: 823
	public class HediffComp_SeverityPerDay : HediffComp_SeverityModifierBase
	{
		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x060015F2 RID: 5618 RVA: 0x0008218C File Offset: 0x0008038C
		private HediffCompProperties_SeverityPerDay Props
		{
			get
			{
				return (HediffCompProperties_SeverityPerDay)this.props;
			}
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x00082199 File Offset: 0x00080399
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.severityPerDay = this.Props.CalculateSeverityPerDay();
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x000821B3 File Offset: 0x000803B3
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<float>(ref this.severityPerDay, "severityPerDay", 0f, false);
		}

		// Token: 0x060015F5 RID: 5621 RVA: 0x000821D4 File Offset: 0x000803D4
		public override float SeverityChangePerDay()
		{
			if (base.Pawn.ageTracker.AgeBiologicalYearsFloat < this.Props.minAge)
			{
				return 0f;
			}
			float num = this.severityPerDay;
			if (ModsConfig.BiotechActive && MechanitorUtility.IsMechanitor(base.Pawn))
			{
				num *= this.Props.mechanitorFactor;
			}
			return num;
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x060015F6 RID: 5622 RVA: 0x00082230 File Offset: 0x00080430
		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (this.Props.showHoursToRecover && this.SeverityChangePerDay() < 0f)
				{
					return Mathf.RoundToInt(this.parent.Severity / Mathf.Abs(this.SeverityChangePerDay()) * 24f) + "LetterHour".Translate();
				}
				return null;
			}
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x060015F7 RID: 5623 RVA: 0x00082294 File Offset: 0x00080494
		public override string CompTipStringExtra
		{
			get
			{
				if (this.Props.showDaysToRecover && this.SeverityChangePerDay() < 0f)
				{
					return "DaysToRecover".Translate((this.parent.Severity / Mathf.Abs(this.SeverityChangePerDay())).ToString("0.0"));
				}
				return null;
			}
		}

		// Token: 0x0400117D RID: 4477
		protected float severityPerDay;
	}
}
