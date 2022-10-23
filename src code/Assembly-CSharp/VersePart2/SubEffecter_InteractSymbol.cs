using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200054C RID: 1356
	public class SubEffecter_InteractSymbol : SubEffecter
	{
		// Token: 0x06002985 RID: 10629 RVA: 0x0001A6A9 File Offset: 0x000188A9
		public SubEffecter_InteractSymbol(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06002986 RID: 10630 RVA: 0x001095E2 File Offset: 0x001077E2
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (this.interactMote == null)
			{
				this.interactMote = MoteMaker.MakeInteractionOverlay(this.def.moteDef, A, B);
			}
		}

		// Token: 0x06002987 RID: 10631 RVA: 0x00109604 File Offset: 0x00107804
		public override void SubCleanup()
		{
			if (this.interactMote != null && !this.interactMote.Destroyed)
			{
				this.interactMote.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x04001B71 RID: 7025
		private Mote interactMote;
	}
}
