using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200021F RID: 543
	public class GenStep_RandomSelector : GenStep
	{
		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000F72 RID: 3954 RVA: 0x00059653 File Offset: 0x00057853
		public override int SeedPart
		{
			get
			{
				return 174742427;
			}
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x0005965C File Offset: 0x0005785C
		public override void Generate(Map map, GenStepParams parms)
		{
			RandomGenStepSelectorOption randomGenStepSelectorOption = this.options.RandomElementByWeight((RandomGenStepSelectorOption opt) => opt.weight);
			if (randomGenStepSelectorOption.genStep != null)
			{
				randomGenStepSelectorOption.genStep.Generate(map, parms);
			}
			if (randomGenStepSelectorOption.def != null)
			{
				randomGenStepSelectorOption.def.genStep.Generate(map, parms);
			}
		}

		// Token: 0x04000DC0 RID: 3520
		public List<RandomGenStepSelectorOption> options;
	}
}
