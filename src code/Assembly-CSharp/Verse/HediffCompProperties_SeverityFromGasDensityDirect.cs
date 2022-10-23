using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000322 RID: 802
	public class HediffCompProperties_SeverityFromGasDensityDirect : HediffCompProperties
	{
		// Token: 0x0600158D RID: 5517 RVA: 0x00080BCA File Offset: 0x0007EDCA
		public HediffCompProperties_SeverityFromGasDensityDirect()
		{
			this.compClass = typeof(HediffComp_SeverityFromGasDensityDirect);
		}

		// Token: 0x0600158E RID: 5518 RVA: 0x00080BF5 File Offset: 0x0007EDF5
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.densityStages.NullOrEmpty<float>())
			{
				yield return "densityStages is empty";
			}
			else if (parentDef.stages.NullOrEmpty<HediffStage>())
			{
				yield return "has no stages";
			}
			else if (this.densityStages.Count != parentDef.stages.Count)
			{
				yield return "densityStages count doesn't match stages count";
			}
			yield break;
			yield break;
		}

		// Token: 0x0400114B RID: 4427
		public GasType gasType;

		// Token: 0x0400114C RID: 4428
		public int intervalTicks = 60;

		// Token: 0x0400114D RID: 4429
		public List<float> densityStages = new List<float>();
	}
}
