using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200034E RID: 846
	public class Hediff_PsychicLove : HediffWithTarget
	{
		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x060016BB RID: 5819 RVA: 0x0008573C File Offset: 0x0008393C
		public override string LabelBase
		{
			get
			{
				string[] array = new string[5];
				array[0] = base.LabelBase;
				array[1] = " ";
				array[2] = this.def.targetPrefix;
				array[3] = " ";
				int num = 4;
				Thing target = this.target;
				array[num] = ((target != null) ? target.LabelShortCap : null);
				return string.Concat(array);
			}
		}

		// Token: 0x060016BC RID: 5820 RVA: 0x00085790 File Offset: 0x00083990
		public override void Notify_RelationAdded(Pawn otherPawn, PawnRelationDef relationDef)
		{
			if (otherPawn == this.target && (relationDef == PawnRelationDefOf.Lover || relationDef == PawnRelationDefOf.Fiance || relationDef == PawnRelationDefOf.Spouse))
			{
				this.pawn.health.RemoveHediff(this);
			}
		}
	}
}
