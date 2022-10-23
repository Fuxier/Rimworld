using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x02000123 RID: 291
	public class SkillRequirement
	{
		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x000266F0 File Offset: 0x000248F0
		public string Summary
		{
			get
			{
				if (this.skill == null)
				{
					return "";
				}
				return string.Format("{0} ({1})", this.skill.LabelCap, this.minLevel);
			}
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x00026728 File Offset: 0x00024928
		public bool PawnSatisfies(Pawn pawn)
		{
			return (pawn.IsColonyMech && pawn.RaceProps.mechFixedSkillLevel >= this.minLevel) || (pawn.skills != null && pawn.skills.GetSkill(this.skill).Level >= this.minLevel);
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0002677D File Offset: 0x0002497D
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "skill", xmlRoot.Name, null, null, null);
			this.minLevel = ParseHelper.FromString<int>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x000267A9 File Offset: 0x000249A9
		public override string ToString()
		{
			if (this.skill == null)
			{
				return "null-skill-requirement";
			}
			return this.skill.defName + "-" + this.minLevel;
		}

		// Token: 0x04000775 RID: 1909
		public SkillDef skill;

		// Token: 0x04000776 RID: 1910
		public int minLevel;
	}
}
