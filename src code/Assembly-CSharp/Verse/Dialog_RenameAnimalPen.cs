using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020004F3 RID: 1267
	public class Dialog_RenameAnimalPen : Dialog_Rename
	{
		// Token: 0x060026A5 RID: 9893 RVA: 0x000F8822 File Offset: 0x000F6A22
		public Dialog_RenameAnimalPen(Map map, CompAnimalPenMarker marker)
		{
			this.map = map;
			this.marker = marker;
			this.curName = marker.label;
		}

		// Token: 0x060026A6 RID: 9894 RVA: 0x000F8844 File Offset: 0x000F6A44
		protected override AcceptanceReport NameIsValid(string name)
		{
			AcceptanceReport result = base.NameIsValid(name);
			if (!result.Accepted)
			{
				return result;
			}
			if (name != this.marker.label && this.map.animalPenManager.GetPenNamed(name) != null)
			{
				return "NameIsInUse".Translate();
			}
			return true;
		}

		// Token: 0x060026A7 RID: 9895 RVA: 0x000F88A0 File Offset: 0x000F6AA0
		protected override void SetName(string name)
		{
			this.marker.label = name;
		}

		// Token: 0x04001944 RID: 6468
		private readonly Map map;

		// Token: 0x04001945 RID: 6469
		private readonly CompAnimalPenMarker marker;
	}
}
