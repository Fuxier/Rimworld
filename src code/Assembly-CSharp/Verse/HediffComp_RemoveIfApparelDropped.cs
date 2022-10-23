using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000319 RID: 793
	public class HediffComp_RemoveIfApparelDropped : HediffComp
	{
		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06001576 RID: 5494 RVA: 0x00080886 File Offset: 0x0007EA86
		public HediffCompProperties_RemoveIfApparelDropped Props
		{
			get
			{
				return (HediffCompProperties_RemoveIfApparelDropped)this.props;
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06001577 RID: 5495 RVA: 0x00080893 File Offset: 0x0007EA93
		public override bool CompShouldRemove
		{
			get
			{
				return !this.parent.pawn.apparel.Wearing(this.wornApparel);
			}
		}

		// Token: 0x06001578 RID: 5496 RVA: 0x000808B3 File Offset: 0x0007EAB3
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_References.Look<Apparel>(ref this.wornApparel, "wornApparel", false);
		}

		// Token: 0x0400113E RID: 4414
		public Apparel wornApparel;
	}
}
