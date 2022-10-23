using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200040D RID: 1037
	public abstract class CompAIUsablePack : ThingComp
	{
		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x06001E81 RID: 7809 RVA: 0x000B6CB1 File Offset: 0x000B4EB1
		protected CompProperties_AIUSablePack Props
		{
			get
			{
				return (CompProperties_AIUSablePack)this.props;
			}
		}

		// Token: 0x06001E82 RID: 7810 RVA: 0x000B6CC0 File Offset: 0x000B4EC0
		public override void CompTick()
		{
			Pawn wearer = ((Apparel)this.parent).Wearer;
			if (this.CanOpportunisticallyUseNow(wearer))
			{
				this.UsePack(wearer);
			}
		}

		// Token: 0x06001E83 RID: 7811 RVA: 0x000B6CF0 File Offset: 0x000B4EF0
		private bool CanOpportunisticallyUseNow(Pawn wearer)
		{
			return wearer != null && !wearer.Dead && wearer.Spawned && wearer.IsHashIntervalTick(this.Props.checkInterval) && !wearer.Downed && wearer.Awake() && !wearer.IsColonistPlayerControlled && !wearer.InMentalState && Rand.Value < this.ChanceToUse(wearer);
		}

		// Token: 0x06001E84 RID: 7812
		protected abstract float ChanceToUse(Pawn wearer);

		// Token: 0x06001E85 RID: 7813
		protected abstract void UsePack(Pawn wearer);
	}
}
