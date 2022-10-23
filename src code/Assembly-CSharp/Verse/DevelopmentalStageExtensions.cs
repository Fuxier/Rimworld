using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000546 RID: 1350
	public static class DevelopmentalStageExtensions
	{
		// Token: 0x0600295F RID: 10591 RVA: 0x00108CA8 File Offset: 0x00106EA8
		public static CachedTexture Icon(this DevelopmentalStage developmentalStage)
		{
			if (!DevelopmentalStageExtensions.ExactlyOneDevelopmentalStageSet(developmentalStage))
			{
				throw new ArgumentException(string.Format("Exactly one developmental stage may be set to get an icon, but was {0}.", developmentalStage));
			}
			if (developmentalStage == DevelopmentalStage.Baby)
			{
				return DevelopmentalStageExtensions.BabyTex;
			}
			if (developmentalStage == DevelopmentalStage.Child)
			{
				return DevelopmentalStageExtensions.ChildTex;
			}
			if (developmentalStage != DevelopmentalStage.Adult)
			{
				throw new NotImplementedException();
			}
			return DevelopmentalStageExtensions.AdultTex;
		}

		// Token: 0x06002960 RID: 10592 RVA: 0x00108CF8 File Offset: 0x00106EF8
		public static bool ExactlyOneDevelopmentalStageSet(DevelopmentalStage developmentalStage)
		{
			return developmentalStage != DevelopmentalStage.None && (developmentalStage & developmentalStage - 1U) == DevelopmentalStage.None;
		}

		// Token: 0x06002961 RID: 10593 RVA: 0x00108D14 File Offset: 0x00106F14
		public static bool Newborn(this DevelopmentalStage developmentalStage)
		{
			return (developmentalStage & DevelopmentalStage.Newborn) > DevelopmentalStage.None;
		}

		// Token: 0x06002962 RID: 10594 RVA: 0x00108D1C File Offset: 0x00106F1C
		public static bool Baby(this DevelopmentalStage developmentalStage)
		{
			return (developmentalStage & DevelopmentalStage.Baby) > DevelopmentalStage.None;
		}

		// Token: 0x06002963 RID: 10595 RVA: 0x00108D24 File Offset: 0x00106F24
		public static bool Child(this DevelopmentalStage developmentalStage)
		{
			return (developmentalStage & DevelopmentalStage.Child) > DevelopmentalStage.None;
		}

		// Token: 0x06002964 RID: 10596 RVA: 0x00108D2C File Offset: 0x00106F2C
		public static bool Adult(this DevelopmentalStage developmentalStage)
		{
			return (developmentalStage & DevelopmentalStage.Adult) > DevelopmentalStage.None;
		}

		// Token: 0x06002965 RID: 10597 RVA: 0x00108D34 File Offset: 0x00106F34
		public static bool Juvenile(this DevelopmentalStage developmentalStage)
		{
			return developmentalStage.Baby() || developmentalStage.Child();
		}

		// Token: 0x06002966 RID: 10598 RVA: 0x00108D46 File Offset: 0x00106F46
		public static bool Has(this DevelopmentalStage developmentalStage, DevelopmentalStage query)
		{
			if (!DevelopmentalStageExtensions.ExactlyOneDevelopmentalStageSet(query))
			{
				Log.ErrorOnce("A single DevelopmentalStage was expected but multiple were set.", 845116642);
			}
			return developmentalStage.HasAny(query);
		}

		// Token: 0x06002967 RID: 10599 RVA: 0x00074B26 File Offset: 0x00072D26
		public static bool HasAny(this DevelopmentalStage developmentalStage, DevelopmentalStage query)
		{
			return (developmentalStage & query) > DevelopmentalStage.None;
		}

		// Token: 0x06002968 RID: 10600 RVA: 0x00108D66 File Offset: 0x00106F66
		public static bool HasAll(this DevelopmentalStage developmentalStage, DevelopmentalStage query)
		{
			return (developmentalStage & query) == query;
		}

		// Token: 0x06002969 RID: 10601 RVA: 0x00108D70 File Offset: 0x00106F70
		public static string ToCommaListOr(this DevelopmentalStage developmentalStages)
		{
			DevelopmentalStageExtensions.developmentalStageStrings.Clear();
			foreach (DevelopmentalStage developmentalStage in developmentalStages.Enumerate())
			{
				DevelopmentalStageExtensions.developmentalStageStrings.Add(developmentalStage.ToString().Translate());
			}
			return DevelopmentalStageExtensions.developmentalStageStrings.ToCommaListOr(false);
		}

		// Token: 0x0600296A RID: 10602 RVA: 0x00108DD8 File Offset: 0x00106FD8
		public static string ToCommaList(this DevelopmentalStage developmentalStages, bool capitalize = false)
		{
			DevelopmentalStageExtensions.developmentalStageStrings.Clear();
			foreach (DevelopmentalStage developmentalStage in developmentalStages.Enumerate())
			{
				TaggedString taggedString = developmentalStage.ToString().Translate();
				DevelopmentalStageExtensions.developmentalStageStrings.Add(capitalize ? taggedString.CapitalizeFirst() : taggedString);
			}
			return DevelopmentalStageExtensions.developmentalStageStrings.ToCommaList(false, false);
		}

		// Token: 0x0600296B RID: 10603 RVA: 0x00108E4C File Offset: 0x0010704C
		public static DevelopmentalStageExtensions.EnumerationCodebehind Enumerate(this DevelopmentalStage developmentalStages)
		{
			return new DevelopmentalStageExtensions.EnumerationCodebehind(developmentalStages);
		}

		// Token: 0x04001B5D RID: 7005
		private static List<string> developmentalStageStrings = new List<string>();

		// Token: 0x04001B5E RID: 7006
		private const DevelopmentalStage lastdevelopmentalStage = DevelopmentalStage.Adult;

		// Token: 0x04001B5F RID: 7007
		public static CachedTexture BabyTex = new CachedTexture("UI/Icons/DevelopmentalStages/Baby");

		// Token: 0x04001B60 RID: 7008
		public static CachedTexture ChildTex = new CachedTexture("UI/Icons/DevelopmentalStages/Child");

		// Token: 0x04001B61 RID: 7009
		public static CachedTexture AdultTex = new CachedTexture("UI/Icons/DevelopmentalStages/Adult");

		// Token: 0x02002107 RID: 8455
		public struct EnumerationCodebehind
		{
			// Token: 0x0600C5C1 RID: 50625 RVA: 0x0043CB35 File Offset: 0x0043AD35
			public EnumerationCodebehind(DevelopmentalStage stages)
			{
				this.bit = 0U;
				this.stages = (uint)stages;
			}

			// Token: 0x17001F38 RID: 7992
			// (get) Token: 0x0600C5C2 RID: 50626 RVA: 0x0043CB45 File Offset: 0x0043AD45
			public DevelopmentalStage Current
			{
				get
				{
					return (DevelopmentalStage)this.bit;
				}
			}

			// Token: 0x0600C5C3 RID: 50627 RVA: 0x0043CB50 File Offset: 0x0043AD50
			public bool MoveNext()
			{
				if (this.bit == 0U)
				{
					this.bit = 1U;
				}
				else
				{
					this.bit <<= 1;
				}
				while (this.bit <= 8U)
				{
					if ((this.bit & this.stages) != 0U)
					{
						return true;
					}
					this.bit <<= 1;
				}
				return false;
			}

			// Token: 0x0600C5C4 RID: 50628 RVA: 0x0043CBA7 File Offset: 0x0043ADA7
			public DevelopmentalStageExtensions.EnumerationCodebehind GetEnumerator()
			{
				return this;
			}

			// Token: 0x0600C5C5 RID: 50629 RVA: 0x000034B7 File Offset: 0x000016B7
			public void Dispose()
			{
			}

			// Token: 0x040082E7 RID: 33511
			private uint bit;

			// Token: 0x040082E8 RID: 33512
			private uint stages;
		}
	}
}
