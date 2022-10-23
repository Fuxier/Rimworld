using System;
using System.Xml;

namespace Verse
{
	// Token: 0x0200006C RID: 108
	public class DamageFactor
	{
		// Token: 0x06000475 RID: 1141 RVA: 0x00019A27 File Offset: 0x00017C27
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "damageDef", xmlRoot.Name, null, null, null);
			this.factor = (xmlRoot.HasChildNodes ? ParseHelper.FromString<float>(xmlRoot.FirstChild.Value) : 1f);
		}

		// Token: 0x040001E9 RID: 489
		public DamageDef damageDef;

		// Token: 0x040001EA RID: 490
		public float factor = 1f;
	}
}
