using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000283 RID: 643
	public class ModIncompatibility : ModRequirement
	{
		// Token: 0x170003AA RID: 938
		// (get) Token: 0x060012AC RID: 4780 RVA: 0x0006C591 File Offset: 0x0006A791
		public override string RequirementTypeLabel
		{
			get
			{
				return "ModIncompatibleWith".Translate("");
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x060012AD RID: 4781 RVA: 0x0006C5AC File Offset: 0x0006A7AC
		public override bool IsSatisfied
		{
			get
			{
				return ModLister.GetActiveModWithIdentifier(this.packageId, false) == null;
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x060012AE RID: 4782 RVA: 0x0006C5C0 File Offset: 0x0006A7C0
		public override string Tooltip
		{
			get
			{
				ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.packageId, true);
				if (modWithIdentifier != null && modWithIdentifier.Active)
				{
					return base.Tooltip + "\n" + "ContentActive".Translate() + "\n\n" + "ModClickToSelect".Translate();
				}
				return base.Tooltip;
			}
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x0006C62C File Offset: 0x0006A82C
		public override void OnClicked(Page_ModsConfig window)
		{
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.packageId, true);
			if (modWithIdentifier != null && modWithIdentifier.Active)
			{
				window.SelectMod(modWithIdentifier);
			}
		}
	}
}
