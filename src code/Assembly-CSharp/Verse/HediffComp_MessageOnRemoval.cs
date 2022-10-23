using System;

namespace Verse
{
	// Token: 0x0200030B RID: 779
	public class HediffComp_MessageOnRemoval : HediffComp_MessageBase
	{
		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x0600154F RID: 5455 RVA: 0x0008010B File Offset: 0x0007E30B
		protected HediffCompProperties_MessageOnRemoval Props
		{
			get
			{
				return (HediffCompProperties_MessageOnRemoval)this.props;
			}
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x00080118 File Offset: 0x0007E318
		public override void CompPostPostRemoved()
		{
			base.CompPostPostRemoved();
			if ((this.Props.messageOnZeroSeverity && this.parent.Severity <= 0f) || (this.Props.messageOnNonZeroSeverity && this.parent.Severity > 0f))
			{
				this.Message();
			}
		}
	}
}
