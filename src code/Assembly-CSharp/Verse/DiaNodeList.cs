using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020004FD RID: 1277
	public class DiaNodeList
	{
		// Token: 0x060026EE RID: 9966 RVA: 0x000FA13C File Offset: 0x000F833C
		public DiaNodeMold RandomNodeFromList()
		{
			List<DiaNodeMold> list = this.Nodes.ListFullCopy<DiaNodeMold>();
			foreach (string nodeName in this.NodeNames)
			{
				list.Add(DialogDatabase.GetNodeNamed(nodeName));
			}
			foreach (DiaNodeMold diaNodeMold in list)
			{
				if (diaNodeMold.unique && diaNodeMold.used)
				{
					list.Remove(diaNodeMold);
				}
			}
			return list.RandomElement<DiaNodeMold>();
		}

		// Token: 0x0400198D RID: 6541
		public string Name = "NeedsName";

		// Token: 0x0400198E RID: 6542
		public List<DiaNodeMold> Nodes = new List<DiaNodeMold>();

		// Token: 0x0400198F RID: 6543
		public List<string> NodeNames = new List<string>();
	}
}
