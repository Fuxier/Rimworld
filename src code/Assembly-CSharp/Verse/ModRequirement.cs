using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000281 RID: 641
	[StaticConstructorOnStartup]
	public abstract class ModRequirement
	{
		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x0600129E RID: 4766
		public abstract bool IsSatisfied { get; }

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x0600129F RID: 4767
		public abstract string RequirementTypeLabel { get; }

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x060012A0 RID: 4768 RVA: 0x0006C397 File Offset: 0x0006A597
		public virtual string Tooltip
		{
			get
			{
				return "ModPackageId".Translate() + ": " + this.packageId;
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x060012A1 RID: 4769 RVA: 0x0006C3BD File Offset: 0x0006A5BD
		public virtual Texture2D StatusIcon
		{
			get
			{
				if (!this.IsSatisfied)
				{
					return ModRequirement.NotResolved;
				}
				return ModRequirement.Resolved;
			}
		}

		// Token: 0x060012A2 RID: 4770
		public abstract void OnClicked(Page_ModsConfig window);

		// Token: 0x04000F7B RID: 3963
		public string packageId;

		// Token: 0x04000F7C RID: 3964
		public string displayName;

		// Token: 0x04000F7D RID: 3965
		public static Texture2D NotResolved = ContentFinder<Texture2D>.Get("UI/Icons/ModRequirements/NotResolved", true);

		// Token: 0x04000F7E RID: 3966
		public static Texture2D NotInstalled = ContentFinder<Texture2D>.Get("UI/Icons/ModRequirements/NotInstalled", true);

		// Token: 0x04000F7F RID: 3967
		public static Texture2D Installed = ContentFinder<Texture2D>.Get("UI/Icons/ModRequirements/Installed", true);

		// Token: 0x04000F80 RID: 3968
		public static Texture2D Resolved = ContentFinder<Texture2D>.Get("UI/Widgets/CheckOn", true);
	}
}
