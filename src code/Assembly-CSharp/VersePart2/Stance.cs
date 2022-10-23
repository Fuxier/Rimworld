using System;

namespace Verse
{
	// Token: 0x02000378 RID: 888
	public abstract class Stance : IExposable
	{
		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x060019BB RID: 6587 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual bool StanceBusy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x060019BC RID: 6588 RVA: 0x0009B4B2 File Offset: 0x000996B2
		protected Pawn Pawn
		{
			get
			{
				return this.stanceTracker.pawn;
			}
		}

		// Token: 0x060019BD RID: 6589 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void StanceTick()
		{
		}

		// Token: 0x060019BE RID: 6590 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void StanceDraw()
		{
		}

		// Token: 0x060019BF RID: 6591 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void ExposeData()
		{
		}

		// Token: 0x040012C7 RID: 4807
		public Pawn_StanceTracker stanceTracker;
	}
}
