using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000071 RID: 113
	public class AbilityCompProperties
	{
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual bool OverridesPsyfocusCost
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000480 RID: 1152 RVA: 0x00019CCE File Offset: 0x00017ECE
		public virtual FloatRange PsyfocusCostRange
		{
			get
			{
				return FloatRange.ZeroToOne;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000481 RID: 1153 RVA: 0x00019CD5 File Offset: 0x00017ED5
		public virtual string PsyfocusCostExplanation
		{
			get
			{
				return "";
			}
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00019CDC File Offset: 0x00017EDC
		public virtual IEnumerable<string> ExtraStatSummary()
		{
			return Enumerable.Empty<string>();
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00019CE3 File Offset: 0x00017EE3
		public virtual IEnumerable<string> ConfigErrors(AbilityDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return "compClass is null";
			}
			int num;
			for (int i = 0; i < parentDef.comps.Count; i = num + 1)
			{
				if (parentDef.comps[i] != this && parentDef.comps[i].compClass == this.compClass)
				{
					yield return "two comps with same compClass: " + this.compClass;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0400020B RID: 523
		[TranslationHandle]
		public Type compClass;
	}
}
