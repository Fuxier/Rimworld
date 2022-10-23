using System;

namespace Verse
{
	// Token: 0x02000116 RID: 278
	public class PawnCapacityDef : Def
	{
		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000744 RID: 1860 RVA: 0x00025E24 File Offset: 0x00024024
		public PawnCapacityWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnCapacityWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x00025E4A File Offset: 0x0002404A
		public string GetLabelFor(Pawn pawn)
		{
			return this.GetLabelFor(pawn.RaceProps.IsFlesh, pawn.RaceProps.Humanlike);
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x00025E68 File Offset: 0x00024068
		public string GetLabelFor(bool isFlesh, bool isHumanlike)
		{
			if (isHumanlike)
			{
				return this.label;
			}
			if (isFlesh)
			{
				if (!this.labelAnimals.NullOrEmpty())
				{
					return this.labelAnimals;
				}
				return this.label;
			}
			else
			{
				if (!this.labelMechanoids.NullOrEmpty())
				{
					return this.labelMechanoids;
				}
				return this.label;
			}
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x00025EB7 File Offset: 0x000240B7
		public bool CanShowOnPawn(Pawn p)
		{
			if (p.def.race.Humanlike)
			{
				return this.showOnHumanlikes;
			}
			if (p.def.race.Animal)
			{
				return this.showOnAnimals;
			}
			return this.showOnMechanoids;
		}

		// Token: 0x040006C8 RID: 1736
		public int listOrder;

		// Token: 0x040006C9 RID: 1737
		public Type workerClass = typeof(PawnCapacityWorker);

		// Token: 0x040006CA RID: 1738
		[MustTranslate]
		public string labelMechanoids = "";

		// Token: 0x040006CB RID: 1739
		[MustTranslate]
		public string labelAnimals = "";

		// Token: 0x040006CC RID: 1740
		public bool showOnHumanlikes = true;

		// Token: 0x040006CD RID: 1741
		public bool showOnAnimals = true;

		// Token: 0x040006CE RID: 1742
		public bool showOnMechanoids = true;

		// Token: 0x040006CF RID: 1743
		public bool lethalFlesh;

		// Token: 0x040006D0 RID: 1744
		public bool lethalMechanoids;

		// Token: 0x040006D1 RID: 1745
		public float minForCapable;

		// Token: 0x040006D2 RID: 1746
		public float minValue;

		// Token: 0x040006D3 RID: 1747
		public bool zeroIfCannotBeAwake;

		// Token: 0x040006D4 RID: 1748
		public bool showOnCaravanHealthTab;

		// Token: 0x040006D5 RID: 1749
		[Unsaved(false)]
		private PawnCapacityWorker workerInt;
	}
}
