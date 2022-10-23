using System;

namespace Verse
{
	// Token: 0x0200018C RID: 396
	public class LanguageWorker_Dutch : LanguageWorker
	{
		// Token: 0x06000AEC RID: 2796 RVA: 0x0003B88D File Offset: 0x00039A8D
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return str;
			}
			return "een " + str;
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x0003B8A5 File Offset: 0x00039AA5
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return "de " + str;
			}
			if (gender == Gender.Male || gender == Gender.Female)
			{
				return "de " + str;
			}
			return "het " + str;
		}
	}
}
