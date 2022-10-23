using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x02000124 RID: 292
	public class SkillGain
	{
		// Token: 0x0600077F RID: 1919 RVA: 0x00003724 File Offset: 0x00001924
		public SkillGain()
		{
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x000267D9 File Offset: 0x000249D9
		public SkillGain(SkillDef skill, int xp)
		{
			this.skill = skill;
			this.xp = xp;
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x000267F0 File Offset: 0x000249F0
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured SkillGain: " + xmlRoot.OuterXml);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "skill", xmlRoot.Name, null, null, null);
			this.xp = ParseHelper.FromString<int>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04000777 RID: 1911
		public SkillDef skill;

		// Token: 0x04000778 RID: 1912
		public int xp;
	}
}
