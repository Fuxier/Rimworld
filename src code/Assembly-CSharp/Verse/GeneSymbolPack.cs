using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200006D RID: 109
	public class GeneSymbolPack
	{
		// Token: 0x040001EB RID: 491
		public List<GeneSymbolPack.WeightedSymbol> prefixSymbols;

		// Token: 0x040001EC RID: 492
		public List<GeneSymbolPack.WeightedSymbol> suffixSymbols;

		// Token: 0x040001ED RID: 493
		public List<GeneSymbolPack.WeightedSymbol> wholeNameSymbols;

		// Token: 0x02001C99 RID: 7321
		public class WeightedSymbol
		{
			// Token: 0x040070B6 RID: 28854
			[MustTranslate]
			public string symbol;

			// Token: 0x040070B7 RID: 28855
			public float weight = 1f;
		}
	}
}
