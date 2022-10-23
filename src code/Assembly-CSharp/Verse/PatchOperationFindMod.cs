using System;
using System.Collections.Generic;
using System.Xml;

namespace Verse
{
	// Token: 0x02000296 RID: 662
	public class PatchOperationFindMod : PatchOperation
	{
		// Token: 0x060012F9 RID: 4857 RVA: 0x0006E430 File Offset: 0x0006C630
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool flag = false;
			for (int i = 0; i < this.mods.Count; i++)
			{
				if (ModLister.HasActiveModWithName(this.mods[i]))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				if (this.match != null)
				{
					return this.match.Apply(xml);
				}
			}
			else if (this.nomatch != null)
			{
				return this.nomatch.Apply(xml);
			}
			return true;
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x0006E49A File Offset: 0x0006C69A
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.mods.ToCommaList(false, false));
		}

		// Token: 0x04000FA1 RID: 4001
		private List<string> mods;

		// Token: 0x04000FA2 RID: 4002
		private PatchOperation match;

		// Token: 0x04000FA3 RID: 4003
		private PatchOperation nomatch;
	}
}
