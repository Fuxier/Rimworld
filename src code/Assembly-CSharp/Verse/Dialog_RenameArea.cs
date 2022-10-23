using System;

namespace Verse
{
	// Token: 0x020004F2 RID: 1266
	public class Dialog_RenameArea : Dialog_Rename
	{
		// Token: 0x060026A2 RID: 9890 RVA: 0x000F877B File Offset: 0x000F697B
		public Dialog_RenameArea(Area area)
		{
			this.area = area;
			this.curName = area.Label;
		}

		// Token: 0x060026A3 RID: 9891 RVA: 0x000F8798 File Offset: 0x000F6998
		protected override AcceptanceReport NameIsValid(string name)
		{
			AcceptanceReport result = base.NameIsValid(name);
			if (!result.Accepted)
			{
				return result;
			}
			if (this.area.Map.areaManager.AllAreas.Any((Area a) => a != this.area && a.Label == name))
			{
				return "NameIsInUse".Translate();
			}
			return true;
		}

		// Token: 0x060026A4 RID: 9892 RVA: 0x000F880F File Offset: 0x000F6A0F
		protected override void SetName(string name)
		{
			this.area.SetLabel(this.curName);
		}

		// Token: 0x04001943 RID: 6467
		private Area area;
	}
}
