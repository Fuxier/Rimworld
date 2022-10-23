using System;

namespace Verse
{
	// Token: 0x02000524 RID: 1316
	public struct AcceptanceReport
	{
		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x060027FD RID: 10237 RVA: 0x00104225 File Offset: 0x00102425
		public string Reason
		{
			get
			{
				return this.reasonTextInt;
			}
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x060027FE RID: 10238 RVA: 0x0010422D File Offset: 0x0010242D
		public bool Accepted
		{
			get
			{
				return this.acceptedInt;
			}
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x060027FF RID: 10239 RVA: 0x00104238 File Offset: 0x00102438
		public static AcceptanceReport WasAccepted
		{
			get
			{
				return new AcceptanceReport("")
				{
					acceptedInt = true
				};
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06002800 RID: 10240 RVA: 0x0010425C File Offset: 0x0010245C
		public static AcceptanceReport WasRejected
		{
			get
			{
				return new AcceptanceReport("")
				{
					acceptedInt = false
				};
			}
		}

		// Token: 0x06002801 RID: 10241 RVA: 0x0010427E File Offset: 0x0010247E
		public AcceptanceReport(string reasonText)
		{
			this.acceptedInt = false;
			this.reasonTextInt = reasonText;
		}

		// Token: 0x06002802 RID: 10242 RVA: 0x0010428E File Offset: 0x0010248E
		public static implicit operator bool(AcceptanceReport report)
		{
			return report.Accepted;
		}

		// Token: 0x06002803 RID: 10243 RVA: 0x00104297 File Offset: 0x00102497
		public static implicit operator AcceptanceReport(bool value)
		{
			if (value)
			{
				return AcceptanceReport.WasAccepted;
			}
			return AcceptanceReport.WasRejected;
		}

		// Token: 0x06002804 RID: 10244 RVA: 0x001042A7 File Offset: 0x001024A7
		public static implicit operator AcceptanceReport(string value)
		{
			return new AcceptanceReport(value);
		}

		// Token: 0x06002805 RID: 10245 RVA: 0x001042AF File Offset: 0x001024AF
		public static implicit operator AcceptanceReport(TaggedString value)
		{
			return new AcceptanceReport(value);
		}

		// Token: 0x04001A6C RID: 6764
		private string reasonTextInt;

		// Token: 0x04001A6D RID: 6765
		private bool acceptedInt;
	}
}
