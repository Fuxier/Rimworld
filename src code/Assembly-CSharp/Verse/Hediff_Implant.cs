using System;

namespace Verse
{
	// Token: 0x02000344 RID: 836
	public class Hediff_Implant : HediffWithComps
	{
		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x0600164D RID: 5709 RVA: 0x0000249D File Offset: 0x0000069D
		public override bool ShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x000836D8 File Offset: 0x000818D8
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (base.Part == null)
			{
				Log.Error(this.def.defName + " has null Part. It should be set before PostAdd.");
				return;
			}
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x00083704 File Offset: 0x00081904
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Part == null)
			{
				Log.Error(base.GetType().Name + " has null part after loading.");
				this.pawn.health.hediffSet.hediffs.Remove(this);
				return;
			}
		}
	}
}
