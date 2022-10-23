using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000110 RID: 272
	public class MentalFitDef : Def
	{
		// Token: 0x06000733 RID: 1843 RVA: 0x00025C22 File Offset: 0x00023E22
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.mentalState == null)
			{
				yield return "mentalState not set.";
			}
			if (this.mtbDaysMoodCurve == null)
			{
				yield return "mtbDaysMoodCurve not set.";
			}
			yield break;
			yield break;
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00025C34 File Offset: 0x00023E34
		public float CalculateMTBDays(Pawn pawn)
		{
			if (!this.developmentalStageFilter.Has(pawn.DevelopmentalStage))
			{
				return float.PositiveInfinity;
			}
			if (pawn.needs.mood == null)
			{
				return float.PositiveInfinity;
			}
			SimpleCurve simpleCurve = this.mtbDaysMoodCurve;
			if (simpleCurve == null)
			{
				return float.PositiveInfinity;
			}
			return simpleCurve.Evaluate(pawn.needs.mood.CurLevelPercentage);
		}

		// Token: 0x04000697 RID: 1687
		public MentalStateDef mentalState;

		// Token: 0x04000698 RID: 1688
		public SimpleCurve mtbDaysMoodCurve;

		// Token: 0x04000699 RID: 1689
		public DevelopmentalStage developmentalStageFilter = DevelopmentalStage.Child | DevelopmentalStage.Adult;
	}
}
