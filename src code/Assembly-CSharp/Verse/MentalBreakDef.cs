using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200010F RID: 271
	public class MentalBreakDef : Def
	{
		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x00025BBA File Offset: 0x00023DBA
		public MentalBreakWorker Worker
		{
			get
			{
				if (this.workerInt == null && this.workerClass != null)
				{
					this.workerInt = (MentalBreakWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x00025BFA File Offset: 0x00023DFA
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.intensity == MentalBreakIntensity.None)
			{
				yield return "intensity not set";
			}
			yield break;
			yield break;
		}

		// Token: 0x0400068F RID: 1679
		public Type workerClass = typeof(MentalBreakWorker);

		// Token: 0x04000690 RID: 1680
		public MentalStateDef mentalState;

		// Token: 0x04000691 RID: 1681
		public float baseCommonality;

		// Token: 0x04000692 RID: 1682
		public SimpleCurve commonalityFactorPerPopulationCurve;

		// Token: 0x04000693 RID: 1683
		public MentalBreakIntensity intensity;

		// Token: 0x04000694 RID: 1684
		public TraitDef requiredTrait;

		// Token: 0x04000695 RID: 1685
		public GeneDef requiredGene;

		// Token: 0x04000696 RID: 1686
		private MentalBreakWorker workerInt;
	}
}
