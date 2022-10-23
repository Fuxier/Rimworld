using System;

namespace Verse
{
	// Token: 0x02000194 RID: 404
	public class LanguageWorker_Portuguese : LanguageWorker
	{
		// Token: 0x06000B1D RID: 2845 RVA: 0x0003CFED File Offset: 0x0003B1ED
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return ((gender == Gender.Female) ? "umas " : "uns ") + str;
			}
			return ((gender == Gender.Female) ? "uma " : "um ") + str;
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x0003D025 File Offset: 0x0003B225
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return ((gender == Gender.Female) ? "as " : "os ") + str;
			}
			return ((gender == Gender.Female) ? "a " : "o ") + str;
		}
	}
}
