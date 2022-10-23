using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000100 RID: 256
	public class IdeoStoryPatternDef : Def
	{
		// Token: 0x06000706 RID: 1798 RVA: 0x000254F0 File Offset: 0x000236F0
		public override IEnumerable<string> ConfigErrors()
		{
			if (!this.segments.Any<string>())
			{
				yield return "Pattern must have at least one segment";
			}
			yield break;
		}

		// Token: 0x04000636 RID: 1590
		[NoTranslate]
		public List<string> segments = new List<string>();

		// Token: 0x04000637 RID: 1591
		public List<string> noCapitalizeFirstSentence = new List<string>();

		// Token: 0x04000638 RID: 1592
		public RulePack rules;
	}
}
