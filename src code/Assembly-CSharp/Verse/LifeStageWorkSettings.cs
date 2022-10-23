using System;
using System.Xml;

namespace Verse
{
	// Token: 0x020000D9 RID: 217
	public class LifeStageWorkSettings
	{
		// Token: 0x06000642 RID: 1602 RVA: 0x0002218B File Offset: 0x0002038B
		public bool IsDisabled(Pawn pawn)
		{
			return pawn.ageTracker.AgeBiologicalYears < this.minAge;
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x000221A0 File Offset: 0x000203A0
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "workType", xmlRoot.Name, null, null, null);
			this.minAge = ParseHelper.FromString<int>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x0400043C RID: 1084
		public WorkTypeDef workType;

		// Token: 0x0400043D RID: 1085
		public int minAge;
	}
}
