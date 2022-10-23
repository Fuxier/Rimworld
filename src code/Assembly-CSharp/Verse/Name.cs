using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000364 RID: 868
	public abstract class Name : IExposable
	{
		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06001746 RID: 5958
		public abstract string ToStringFull { get; }

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06001747 RID: 5959
		public abstract string ToStringShort { get; }

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06001748 RID: 5960
		public abstract bool IsValid { get; }

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06001749 RID: 5961 RVA: 0x00088F98 File Offset: 0x00087198
		public bool UsedThisGame
		{
			get
			{
				using (IEnumerator<Name> enumerator = NameUseChecker.AllPawnsNamesEverUsed.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.ConfusinglySimilarTo(this))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x0600174A RID: 5962
		public abstract bool ConfusinglySimilarTo(Name other);

		// Token: 0x0600174B RID: 5963
		public abstract void ExposeData();

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x0600174C RID: 5964
		public abstract bool Numerical { get; }
	}
}
