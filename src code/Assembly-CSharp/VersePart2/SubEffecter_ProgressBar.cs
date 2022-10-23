using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200007C RID: 124
	public class SubEffecter_ProgressBar : SubEffecter
	{
		// Token: 0x060004B6 RID: 1206 RVA: 0x0001A6A9 File Offset: 0x000188A9
		public SubEffecter_ProgressBar(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0001A898 File Offset: 0x00018A98
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (this.mote == null)
			{
				this.mote = (MoteProgressBar)MoteMaker.MakeInteractionOverlay(this.def.moteDef, A, B);
				this.mote.exactScale.x = 0.68f;
				this.mote.exactScale.z = 0.12f;
			}
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0001A8F4 File Offset: 0x00018AF4
		public override void SubCleanup()
		{
			if (this.mote != null && !this.mote.Destroyed)
			{
				this.mote.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0400021B RID: 539
		public MoteProgressBar mote;

		// Token: 0x0400021C RID: 540
		private const float Width = 0.68f;

		// Token: 0x0400021D RID: 541
		private const float Height = 0.12f;
	}
}
