using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200027F RID: 639
	public class LoadFolder : IEquatable<LoadFolder>
	{
		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06001294 RID: 4756 RVA: 0x0006BD29 File Offset: 0x00069F29
		public bool ShouldLoad
		{
			get
			{
				return (this.requiredPackageIds.NullOrEmpty<string>() || ModLister.AnyFromListActive(this.requiredPackageIds)) && (this.disallowedPackageIds.NullOrEmpty<string>() || !ModLister.AnyFromListActive(this.disallowedPackageIds));
			}
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x0006BD64 File Offset: 0x00069F64
		public LoadFolder(string folderName, List<string> requiredPackageIds, List<string> disallowedPackageIds)
		{
			this.folderName = folderName;
			this.requiredPackageIds = requiredPackageIds;
			this.disallowedPackageIds = disallowedPackageIds;
			this.hashCodeCached = ((folderName != null) ? folderName.GetHashCode() : 0);
			this.hashCodeCached = Gen.HashCombine<int>(this.hashCodeCached, (requiredPackageIds != null) ? requiredPackageIds.GetHashCode() : 0);
			this.hashCodeCached = Gen.HashCombine<int>(this.hashCodeCached, (disallowedPackageIds != null) ? disallowedPackageIds.GetHashCode() : 0);
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x0006BDD8 File Offset: 0x00069FD8
		public bool Equals(LoadFolder other)
		{
			return other != null && this.hashCodeCached == other.GetHashCode();
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x0006BDF0 File Offset: 0x00069FF0
		public override bool Equals(object obj)
		{
			LoadFolder other;
			return (other = (obj as LoadFolder)) != null && this.Equals(other);
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x0006BE10 File Offset: 0x0006A010
		public override int GetHashCode()
		{
			return this.hashCodeCached;
		}

		// Token: 0x04000F75 RID: 3957
		public string folderName;

		// Token: 0x04000F76 RID: 3958
		public List<string> requiredPackageIds;

		// Token: 0x04000F77 RID: 3959
		public List<string> disallowedPackageIds;

		// Token: 0x04000F78 RID: 3960
		private readonly int hashCodeCached;
	}
}
