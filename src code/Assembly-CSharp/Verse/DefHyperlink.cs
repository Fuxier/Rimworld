using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x020000A6 RID: 166
	public class DefHyperlink
	{
		// Token: 0x0600058D RID: 1421 RVA: 0x00003724 File Offset: 0x00001924
		public DefHyperlink()
		{
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0001F3BF File Offset: 0x0001D5BF
		public DefHyperlink(Def def)
		{
			this.def = def;
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0001F3CE File Offset: 0x0001D5CE
		public DefHyperlink(RoyalTitleDef def, Faction faction)
		{
			this.def = def;
			this.faction = faction;
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0001F3E4 File Offset: 0x0001D5E4
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured DefHyperlink: " + xmlRoot.OuterXml);
				return;
			}
			Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(xmlRoot.Name, null);
			if (typeInAnyAssembly == null)
			{
				Log.Error("Misconfigured DefHyperlink. Could not find def of type " + xmlRoot.Name);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "def", xmlRoot.FirstChild.Value, null, null, typeInAnyAssembly);
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0001F45A File Offset: 0x0001D65A
		public static implicit operator DefHyperlink(Def def)
		{
			return new DefHyperlink
			{
				def = def
			};
		}

		// Token: 0x0400029A RID: 666
		public Def def;

		// Token: 0x0400029B RID: 667
		public Faction faction;
	}
}
