using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002F4 RID: 756
	public class HediffComp_GiveRandomSituationalThought : HediffComp
	{
		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06001500 RID: 5376 RVA: 0x0007EE2B File Offset: 0x0007D02B
		public HediffCompProperties_GiveRandomSituationalThought Props
		{
			get
			{
				return (HediffCompProperties_GiveRandomSituationalThought)this.props;
			}
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x0007EE38 File Offset: 0x0007D038
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.selectedThought = this.Props.thoughtDefs.RandomElement<ThoughtDef>();
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06001502 RID: 5378 RVA: 0x0007EE56 File Offset: 0x0007D056
		public override string CompLabelInBracketsExtra
		{
			get
			{
				ThoughtDef thoughtDef = this.selectedThought;
				if (thoughtDef == null)
				{
					return null;
				}
				return thoughtDef.labelInBracketsExtraForHediff;
			}
		}

		// Token: 0x06001503 RID: 5379 RVA: 0x0007EE69 File Offset: 0x0007D069
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Defs.Look<ThoughtDef>(ref this.selectedThought, "selectedThought");
		}

		// Token: 0x040010F8 RID: 4344
		public ThoughtDef selectedThought;
	}
}
