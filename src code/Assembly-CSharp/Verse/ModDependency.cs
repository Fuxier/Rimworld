using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000282 RID: 642
	public class ModDependency : ModRequirement
	{
		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x060012A5 RID: 4773 RVA: 0x0006C421 File Offset: 0x0006A621
		public override string RequirementTypeLabel
		{
			get
			{
				return "ModDependsOn".Translate("");
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x060012A6 RID: 4774 RVA: 0x0006C43C File Offset: 0x0006A63C
		public override bool IsSatisfied
		{
			get
			{
				return ModLister.GetActiveModWithIdentifier(this.packageId, false) != null;
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x060012A7 RID: 4775 RVA: 0x0006C450 File Offset: 0x0006A650
		public override Texture2D StatusIcon
		{
			get
			{
				ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.packageId, true);
				if (modWithIdentifier == null)
				{
					return ModRequirement.NotInstalled;
				}
				if (!modWithIdentifier.Active)
				{
					return ModRequirement.Installed;
				}
				return ModRequirement.Resolved;
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x060012A8 RID: 4776 RVA: 0x0006C488 File Offset: 0x0006A688
		public override string Tooltip
		{
			get
			{
				ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.packageId, true);
				if (modWithIdentifier == null)
				{
					return base.Tooltip + "\n" + "ContentNotInstalled".Translate() + "\n\n" + "ModClickToGoToWebsite".Translate();
				}
				if (!modWithIdentifier.Active)
				{
					return base.Tooltip + "\n" + "ContentInstalledButNotActive".Translate() + "\n\n" + "ModClickToSelect".Translate();
				}
				return base.Tooltip;
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x060012A9 RID: 4777 RVA: 0x0006C52F File Offset: 0x0006A72F
		public string Url
		{
			get
			{
				return this.steamWorkshopUrl ?? this.downloadUrl;
			}
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x0006C544 File Offset: 0x0006A744
		public override void OnClicked(Page_ModsConfig window)
		{
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.packageId, true);
			if (modWithIdentifier == null)
			{
				if (!this.Url.NullOrEmpty())
				{
					SteamUtility.OpenUrl(this.Url);
					return;
				}
			}
			else if (!modWithIdentifier.Active)
			{
				window.SelectMod(modWithIdentifier);
			}
		}

		// Token: 0x04000F81 RID: 3969
		public string downloadUrl;

		// Token: 0x04000F82 RID: 3970
		public string steamWorkshopUrl;
	}
}
