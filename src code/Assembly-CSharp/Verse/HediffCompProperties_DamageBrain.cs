using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002D2 RID: 722
	public class HediffCompProperties_DamageBrain : HediffCompProperties
	{
		// Token: 0x06001499 RID: 5273 RVA: 0x0007D61B File Offset: 0x0007B81B
		public HediffCompProperties_DamageBrain()
		{
			this.compClass = typeof(HediffComp_DamageBrain);
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x0007D63E File Offset: 0x0007B83E
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.damageAmount == IntRange.zero)
			{
				yield return "damageAmount is not defined";
			}
			if (this.mtbDaysPerStage == null)
			{
				yield return "mtbDaysPerStage is not defined";
			}
			else if (this.mtbDaysPerStage.Count != parentDef.stages.Count)
			{
				yield return "mtbDaysPerStage count doesn't match Hediffs number of stages";
			}
			yield break;
			yield break;
		}

		// Token: 0x040010BF RID: 4287
		public IntRange damageAmount = IntRange.zero;

		// Token: 0x040010C0 RID: 4288
		public List<float> mtbDaysPerStage;
	}
}
