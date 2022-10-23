using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x0200032D RID: 813
	public class Hediff_VatLearning : HediffWithComps
	{
		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x060015C4 RID: 5572 RVA: 0x00081823 File Offset: 0x0007FA23
		public override bool ShouldRemove
		{
			get
			{
				return this.pawn.Spawned || !(this.pawn.ParentHolder is Building_GrowthVat);
			}
		}

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x060015C5 RID: 5573 RVA: 0x0008184A File Offset: 0x0007FA4A
		public override string LabelInBrackets
		{
			get
			{
				return this.Severity.ToStringPercent();
			}
		}

		// Token: 0x060015C6 RID: 5574 RVA: 0x00081857 File Offset: 0x0007FA57
		public override void PostTick()
		{
			base.PostTick();
			if (this.Severity >= this.def.maxSeverity)
			{
				this.Learn();
			}
		}

		// Token: 0x060015C7 RID: 5575 RVA: 0x00081878 File Offset: 0x0007FA78
		public void Learn()
		{
			if (this.pawn.skills != null)
			{
				SkillRecord skillRecord;
				if ((from x in this.pawn.skills.skills
				where !x.TotallyDisabled
				select x).TryRandomElement(out skillRecord))
				{
					skillRecord.Learn(8000f, true);
				}
			}
			this.Severity = this.def.initialSeverity;
		}

		// Token: 0x04001166 RID: 4454
		private const float XPToAward = 8000f;
	}
}
