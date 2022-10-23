using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000363 RID: 867
	public class SummaryHealthHandler
	{
		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06001743 RID: 5955 RVA: 0x00088E70 File Offset: 0x00087070
		public float SummaryHealthPercent
		{
			get
			{
				if (this.pawn.Dead)
				{
					return 0f;
				}
				if (this.dirty)
				{
					List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
					float num = 1f;
					for (int i = 0; i < hediffs.Count; i++)
					{
						if (!(hediffs[i] is Hediff_MissingPart))
						{
							float num2 = Mathf.Min(hediffs[i].SummaryHealthPercentImpact, 0.95f);
							num *= 1f - num2;
						}
					}
					List<Hediff_MissingPart> missingPartsCommonAncestors = this.pawn.health.hediffSet.GetMissingPartsCommonAncestors();
					for (int j = 0; j < missingPartsCommonAncestors.Count; j++)
					{
						float num3 = Mathf.Min(missingPartsCommonAncestors[j].SummaryHealthPercentImpact, 0.95f);
						num *= 1f - num3;
					}
					this.cachedSummaryHealthPercent = Mathf.Clamp(num, 0.05f, 1f);
					this.dirty = false;
				}
				return this.cachedSummaryHealthPercent;
			}
		}

		// Token: 0x06001744 RID: 5956 RVA: 0x00088F6B File Offset: 0x0008716B
		public SummaryHealthHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06001745 RID: 5957 RVA: 0x00088F8C File Offset: 0x0008718C
		public void Notify_HealthChanged()
		{
			this.dirty = true;
		}

		// Token: 0x040011D2 RID: 4562
		private Pawn pawn;

		// Token: 0x040011D3 RID: 4563
		private float cachedSummaryHealthPercent = 1f;

		// Token: 0x040011D4 RID: 4564
		private bool dirty = true;
	}
}
