using System;

namespace Verse
{
	// Token: 0x02000172 RID: 370
	public abstract class Entity
	{
		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000A0C RID: 2572
		public abstract string LabelCap { get; }

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000A0D RID: 2573
		public abstract string Label { get; }

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000A0E RID: 2574 RVA: 0x000311F8 File Offset: 0x0002F3F8
		public virtual string LabelShort
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000A0F RID: 2575 RVA: 0x000311F8 File Offset: 0x0002F3F8
		public virtual string LabelMouseover
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000A10 RID: 2576 RVA: 0x00031200 File Offset: 0x0002F400
		public virtual string LabelShortCap
		{
			get
			{
				return this.LabelShort.CapitalizeFirst();
			}
		}

		// Token: 0x06000A11 RID: 2577
		public abstract void SpawnSetup(Map map, bool respawningAfterLoad);

		// Token: 0x06000A12 RID: 2578
		public abstract void DeSpawn(DestroyMode mode = DestroyMode.Vanish);

		// Token: 0x06000A13 RID: 2579 RVA: 0x0003120D File Offset: 0x0002F40D
		public virtual void Tick()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x0003120D File Offset: 0x0002F40D
		public virtual void TickRare()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x0003120D File Offset: 0x0002F40D
		public virtual void TickLong()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x000311F8 File Offset: 0x0002F3F8
		public override string ToString()
		{
			return this.LabelCap;
		}
	}
}
