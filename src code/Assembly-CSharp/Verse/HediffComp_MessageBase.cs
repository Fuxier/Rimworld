using System;

namespace Verse
{
	// Token: 0x02000309 RID: 777
	public class HediffComp_MessageBase : HediffComp
	{
		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x0600154B RID: 5451 RVA: 0x00080064 File Offset: 0x0007E264
		private HediffCompProperties_MessageBase Props
		{
			get
			{
				return (HediffCompProperties_MessageBase)this.props;
			}
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x00080074 File Offset: 0x0007E274
		protected virtual void Message()
		{
			if (this.Props.onlyMessageForColonistsOrPrisoners && !base.Pawn.IsColonist && !base.Pawn.IsPrisonerOfColony)
			{
				return;
			}
			Messages.Message(this.Props.message.Formatted(base.Pawn), base.Pawn, this.Props.messageType, true);
		}
	}
}
