using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002E3 RID: 739
	public class HediffComp_EntropyLink : HediffComp
	{
		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x060014D4 RID: 5332 RVA: 0x0007E2CF File Offset: 0x0007C4CF
		public HediffCompProperties_EntropyLink Props
		{
			get
			{
				return (HediffCompProperties_EntropyLink)this.props;
			}
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x0007E2DC File Offset: 0x0007C4DC
		public override void Notify_EntropyGained(float baseAmount, float finalAmount, Thing source = null)
		{
			base.Notify_EntropyGained(baseAmount, finalAmount, source);
			HediffComp_Link hediffComp_Link = this.parent.TryGetComp<HediffComp_Link>();
			if (hediffComp_Link != null && hediffComp_Link.other != source && hediffComp_Link.OtherPawn.psychicEntropy != null)
			{
				hediffComp_Link.OtherPawn.psychicEntropy.TryAddEntropy(baseAmount * this.Props.entropyTransferAmount, this.parent.pawn, false, false);
				MoteMaker.MakeInteractionOverlay(ThingDefOf.Mote_PsychicLinkPulse, this.parent.pawn, hediffComp_Link.other);
			}
		}
	}
}
