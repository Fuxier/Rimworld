using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020002FA RID: 762
	public class HediffComp_HealPermanentWounds : HediffComp
	{
		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06001512 RID: 5394 RVA: 0x0007F31F File Offset: 0x0007D51F
		public HediffCompProperties_HealPermanentWounds Props
		{
			get
			{
				return (HediffCompProperties_HealPermanentWounds)this.props;
			}
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x0007F32C File Offset: 0x0007D52C
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.ResetTicksToHeal();
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x0007F33A File Offset: 0x0007D53A
		private void ResetTicksToHeal()
		{
			this.ticksToHeal = Rand.Range(15, 30) * 60000;
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x0007F351 File Offset: 0x0007D551
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksToHeal--;
			if (this.ticksToHeal <= 0)
			{
				HediffComp_HealPermanentWounds.TryHealRandomPermanentWound(base.Pawn, this.parent.LabelCap);
				this.ResetTicksToHeal();
			}
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x0007F388 File Offset: 0x0007D588
		public static void TryHealRandomPermanentWound(Pawn pawn, string cause)
		{
			Hediff hediff;
			if (!(from hd in pawn.health.hediffSet.hediffs
			where hd.IsPermanent() || hd.def.chronic
			select hd).TryRandomElement(out hediff))
			{
				return;
			}
			HealthUtility.Cure(hediff);
			if (PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				Messages.Message("MessagePermanentWoundHealed".Translate(cause, pawn.LabelShort, hediff.Label, pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent, true);
			}
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x0007F429 File Offset: 0x0007D629
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksToHeal, "ticksToHeal", 0, false);
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x0007F43D File Offset: 0x0007D63D
		public override string CompDebugString()
		{
			return "ticksToHeal: " + this.ticksToHeal;
		}

		// Token: 0x04001106 RID: 4358
		private int ticksToHeal;
	}
}
