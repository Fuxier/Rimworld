using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020005A1 RID: 1441
	public class SpecialThingFilterDef : Def
	{
		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06002BDF RID: 11231 RVA: 0x00116C38 File Offset: 0x00114E38
		public SpecialThingFilterWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (SpecialThingFilterWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x06002BE0 RID: 11232 RVA: 0x00116C5E File Offset: 0x00114E5E
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.workerClass == null)
			{
				yield return "SpecialThingFilterDef " + this.defName + " has no worker class.";
			}
			yield break;
			yield break;
		}

		// Token: 0x06002BE1 RID: 11233 RVA: 0x00116C6E File Offset: 0x00114E6E
		public static SpecialThingFilterDef Named(string defName)
		{
			return DefDatabase<SpecialThingFilterDef>.GetNamed(defName, true);
		}

		// Token: 0x04001CE2 RID: 7394
		public ThingCategoryDef parentCategory;

		// Token: 0x04001CE3 RID: 7395
		public string saveKey;

		// Token: 0x04001CE4 RID: 7396
		public bool allowedByDefault;

		// Token: 0x04001CE5 RID: 7397
		public bool configurable = true;

		// Token: 0x04001CE6 RID: 7398
		public Type workerClass;

		// Token: 0x04001CE7 RID: 7399
		[Unsaved(false)]
		private SpecialThingFilterWorker workerInt;
	}
}
