using System;

namespace Verse
{
	// Token: 0x02000324 RID: 804
	public class Hediff_HemogenCraving : HediffWithComps
	{
		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06001596 RID: 5526 RVA: 0x00080D34 File Offset: 0x0007EF34
		public override string SeverityLabel
		{
			get
			{
				if (this.Severity == 0f)
				{
					return null;
				}
				return this.Severity.ToStringPercent();
			}
		}
	}
}
