using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020004FA RID: 1274
	public class DiaNode
	{
		// Token: 0x060026E9 RID: 9961 RVA: 0x000F9F30 File Offset: 0x000F8130
		public DiaNode(TaggedString text)
		{
			this.text = text;
		}

		// Token: 0x060026EA RID: 9962 RVA: 0x000F9F4C File Offset: 0x000F814C
		public DiaNode(DiaNodeMold newDef)
		{
			this.def = newDef;
			this.def.used = true;
			this.text = this.def.texts.RandomElement<string>();
			if (this.def.optionList.Count > 0)
			{
				using (List<DiaOptionMold>.Enumerator enumerator = this.def.optionList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DiaOptionMold diaOptionMold = enumerator.Current;
						this.options.Add(new DiaOption(diaOptionMold));
					}
					return;
				}
			}
			this.options.Add(new DiaOption("OK".Translate()));
		}

		// Token: 0x060026EB RID: 9963 RVA: 0x000034B7 File Offset: 0x000016B7
		public void PreClose()
		{
		}

		// Token: 0x0400197E RID: 6526
		public TaggedString text;

		// Token: 0x0400197F RID: 6527
		public List<DiaOption> options = new List<DiaOption>();

		// Token: 0x04001980 RID: 6528
		protected DiaNodeMold def;
	}
}
