using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020004F1 RID: 1265
	public class Dialog_RenameZone : Dialog_Rename
	{
		// Token: 0x0600269F RID: 9887 RVA: 0x000F86AE File Offset: 0x000F68AE
		public Dialog_RenameZone(Zone zone)
		{
			this.zone = zone;
			this.curName = zone.label;
		}

		// Token: 0x060026A0 RID: 9888 RVA: 0x000F86CC File Offset: 0x000F68CC
		protected override AcceptanceReport NameIsValid(string name)
		{
			AcceptanceReport result = base.NameIsValid(name);
			if (!result.Accepted)
			{
				return result;
			}
			if (this.zone.Map.zoneManager.AllZones.Any((Zone z) => z != this.zone && z.label == name))
			{
				return "NameIsInUse".Translate();
			}
			return true;
		}

		// Token: 0x060026A1 RID: 9889 RVA: 0x000F8743 File Offset: 0x000F6943
		protected override void SetName(string name)
		{
			this.zone.label = this.curName;
			Messages.Message("ZoneGainsName".Translate(this.curName), MessageTypeDefOf.TaskCompletion, false);
		}

		// Token: 0x04001942 RID: 6466
		private Zone zone;
	}
}
