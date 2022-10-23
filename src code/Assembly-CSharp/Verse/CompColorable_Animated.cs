using System;

namespace Verse
{
	// Token: 0x02000416 RID: 1046
	public class CompColorable_Animated : CompColorable
	{
		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06001EAD RID: 7853 RVA: 0x000B74F0 File Offset: 0x000B56F0
		public CompProperties_ColorableAnimated Props
		{
			get
			{
				return (CompProperties_ColorableAnimated)this.props;
			}
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x000B7500 File Offset: 0x000B5700
		public override void Initialize(CompProperties props)
		{
			this.props = props;
			if (this.Props.startWithRandom)
			{
				this.colorOffset = Rand.RangeInclusive(0, this.Props.colors.Count - 1);
			}
			base.SetColor(this.Props.colors[this.colorOffset % this.Props.colors.Count]);
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x000B756C File Offset: 0x000B576C
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(this.Props.changeInterval))
			{
				base.SetColor(this.Props.colors[this.colorOffset % this.Props.colors.Count]);
				this.colorOffset++;
			}
		}

		// Token: 0x06001EB0 RID: 7856 RVA: 0x000B75D2 File Offset: 0x000B57D2
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.colorOffset, "colorOffset", 0, false);
		}

		// Token: 0x040014EC RID: 5356
		public int colorOffset;
	}
}
