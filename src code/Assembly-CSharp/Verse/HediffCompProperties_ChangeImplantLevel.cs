using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002CC RID: 716
	public class HediffCompProperties_ChangeImplantLevel : HediffCompProperties
	{
		// Token: 0x06001481 RID: 5249 RVA: 0x0007D20C File Offset: 0x0007B40C
		public HediffCompProperties_ChangeImplantLevel()
		{
			this.compClass = typeof(HediffComp_ChangeImplantLevel);
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x0007D224 File Offset: 0x0007B424
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.implant == null)
			{
				yield return "implant is null";
			}
			else if (!typeof(Hediff_Level).IsAssignableFrom(this.implant.hediffClass))
			{
				yield return "implant is not Hediff_Level";
			}
			if (this.levelOffset == 0)
			{
				yield return "levelOffset is 0";
			}
			if (this.probabilityPerStage == null)
			{
				yield return "probabilityPerStage is not defined";
			}
			else if (this.probabilityPerStage.Count != parentDef.stages.Count)
			{
				yield return "probabilityPerStage count doesn't match Hediffs number of stages";
			}
			yield break;
			yield break;
		}

		// Token: 0x040010B2 RID: 4274
		public HediffDef implant;

		// Token: 0x040010B3 RID: 4275
		public int levelOffset;

		// Token: 0x040010B4 RID: 4276
		public List<ChangeImplantLevel_Probability> probabilityPerStage;
	}
}
