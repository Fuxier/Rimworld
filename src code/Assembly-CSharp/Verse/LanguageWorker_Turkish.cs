using System;

namespace Verse
{
	// Token: 0x02000199 RID: 409
	public class LanguageWorker_Turkish : LanguageWorker
	{
		// Token: 0x06000B34 RID: 2868 RVA: 0x0003D6D8 File Offset: 0x0003B8D8
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			return str + " bir";
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x000282AB File Offset: 0x000264AB
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			return str;
		}
	}
}
