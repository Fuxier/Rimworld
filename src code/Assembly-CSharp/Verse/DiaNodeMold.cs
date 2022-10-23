using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020004FC RID: 1276
	public class DiaNodeMold
	{
		// Token: 0x060026EC RID: 9964 RVA: 0x000FA020 File Offset: 0x000F8220
		public void PostLoad()
		{
			int num = 0;
			foreach (string text in this.texts.ListFullCopy<string>())
			{
				this.texts[num] = text.Replace("\\n", Environment.NewLine);
				num++;
			}
			foreach (DiaOptionMold diaOptionMold in this.optionList)
			{
				foreach (DiaNodeMold diaNodeMold in diaOptionMold.ChildNodes)
				{
					diaNodeMold.PostLoad();
				}
			}
		}

		// Token: 0x04001986 RID: 6534
		public string name = "Unnamed";

		// Token: 0x04001987 RID: 6535
		public bool unique;

		// Token: 0x04001988 RID: 6536
		public List<string> texts = new List<string>();

		// Token: 0x04001989 RID: 6537
		public List<DiaOptionMold> optionList = new List<DiaOptionMold>();

		// Token: 0x0400198A RID: 6538
		[Unsaved(false)]
		public bool isRoot = true;

		// Token: 0x0400198B RID: 6539
		[Unsaved(false)]
		public bool used;

		// Token: 0x0400198C RID: 6540
		[Unsaved(false)]
		public DiaNodeType nodeType;
	}
}
