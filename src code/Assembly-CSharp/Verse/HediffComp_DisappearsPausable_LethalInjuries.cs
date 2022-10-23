using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002D7 RID: 727
	public class HediffComp_DisappearsPausable_LethalInjuries : HediffComp_DisappearsPausable
	{
		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x060014B1 RID: 5297 RVA: 0x0007DA28 File Offset: 0x0007BC28
		protected override bool Paused
		{
			get
			{
				return SanguophageUtility.ShouldBeDeathrestingOrInComaInsteadOfDead(base.Pawn);
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x060014B2 RID: 5298 RVA: 0x0007DA35 File Offset: 0x0007BC35
		public override string CompTipStringExtra
		{
			get
			{
				if (this.Paused)
				{
					return "PawnWillKeepRegeneratingLethalInjuries".Translate(base.Pawn.Named("PAWN")).Colorize(ColorLibrary.RedReadable);
				}
				return base.CompTipStringExtra;
			}
		}
	}
}
