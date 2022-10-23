using System;

namespace Verse
{
	// Token: 0x0200036E RID: 878
	public static class AutoBreastfeedModeExtension
	{
		// Token: 0x060018D9 RID: 6361 RVA: 0x000956DA File Offset: 0x000938DA
		public static TaggedString Translate(this AutofeedMode mode)
		{
			switch (mode)
			{
			case AutofeedMode.Never:
				return "AutofeedModeNever".Translate();
			case AutofeedMode.Childcare:
				return "AutofeedModeChildcare".Translate();
			case AutofeedMode.Urgent:
				return "AutofeedModeUrgent".Translate();
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x060018DA RID: 6362 RVA: 0x00095718 File Offset: 0x00093918
		public static TaggedString GetTooltip(this AutofeedMode mode, Pawn baby, Pawn feeder)
		{
			string key;
			switch (mode)
			{
			case AutofeedMode.Never:
				key = "AutofeedModeTooltipNever";
				break;
			case AutofeedMode.Childcare:
				key = "AutofeedModeTooltipChildcare";
				break;
			case AutofeedMode.Urgent:
				key = "AutofeedModeTooltipUrgent";
				break;
			default:
				throw new NotImplementedException();
			}
			return key.Translate(baby.Named("BABY"), feeder.Named("FEEDER"));
		}
	}
}
