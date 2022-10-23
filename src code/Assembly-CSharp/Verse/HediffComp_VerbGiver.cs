using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200032C RID: 812
	public class HediffComp_VerbGiver : HediffComp, IVerbOwner
	{
		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x060015B9 RID: 5561 RVA: 0x00081735 File Offset: 0x0007F935
		public HediffCompProperties_VerbGiver Props
		{
			get
			{
				return (HediffCompProperties_VerbGiver)this.props;
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x060015BA RID: 5562 RVA: 0x00081742 File Offset: 0x0007F942
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x060015BB RID: 5563 RVA: 0x0008174A File Offset: 0x0007F94A
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.Props.verbs;
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x060015BC RID: 5564 RVA: 0x00081757 File Offset: 0x0007F957
		public List<Tool> Tools
		{
			get
			{
				return this.Props.tools;
			}
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x060015BD RID: 5565 RVA: 0x00081764 File Offset: 0x0007F964
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return base.Pawn;
			}
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x060015BE RID: 5566 RVA: 0x0008176C File Offset: 0x0007F96C
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.Hediff;
			}
		}

		// Token: 0x060015BF RID: 5567 RVA: 0x00081773 File Offset: 0x0007F973
		public HediffComp_VerbGiver()
		{
			this.verbTracker = new VerbTracker(this);
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x00081787 File Offset: 0x0007F987
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.verbTracker == null)
			{
				this.verbTracker = new VerbTracker(this);
			}
		}

		// Token: 0x060015C1 RID: 5569 RVA: 0x000817C5 File Offset: 0x0007F9C5
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			this.verbTracker.VerbsTick();
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x000817D9 File Offset: 0x0007F9D9
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return this.parent.GetUniqueLoadID() + "_" + this.parent.comps.IndexOf(this);
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x00081806 File Offset: 0x0007FA06
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			return p.health.hediffSet.hediffs.Contains(this.parent);
		}

		// Token: 0x04001165 RID: 4453
		public VerbTracker verbTracker;
	}
}
