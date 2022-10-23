using System;
using System.Xml;

namespace Verse
{
	// Token: 0x020000DA RID: 218
	public class MechWorkTypePriority
	{
		// Token: 0x06000645 RID: 1605 RVA: 0x000221CC File Offset: 0x000203CC
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "def", xmlRoot.Name, null, null, null);
			this.priority = (xmlRoot.HasChildNodes ? ParseHelper.FromString<int>(xmlRoot.FirstChild.Value) : 3);
		}

		// Token: 0x0400043E RID: 1086
		public WorkTypeDef def;

		// Token: 0x0400043F RID: 1087
		public int priority;
	}
}
