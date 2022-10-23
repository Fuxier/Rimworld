using System;

namespace Verse
{
	// Token: 0x020002E1 RID: 737
	public class HediffComp_Effecter : HediffComp
	{
		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x060014D0 RID: 5328 RVA: 0x0007E251 File Offset: 0x0007C451
		public HediffCompProperties_Effecter Props
		{
			get
			{
				return (HediffCompProperties_Effecter)this.props;
			}
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x0007E260 File Offset: 0x0007C460
		public EffecterDef CurrentStateEffecter()
		{
			if (this.parent.CurStageIndex >= this.Props.severityIndices.min && (this.Props.severityIndices.max < 0 || this.parent.CurStageIndex <= this.Props.severityIndices.max))
			{
				return this.Props.stateEffecter;
			}
			return null;
		}
	}
}
