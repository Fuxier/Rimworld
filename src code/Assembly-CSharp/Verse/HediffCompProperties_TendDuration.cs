using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000329 RID: 809
	public class HediffCompProperties_TendDuration : HediffCompProperties
	{
		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x060015A2 RID: 5538 RVA: 0x00080EA3 File Offset: 0x0007F0A3
		public bool TendIsPermanent
		{
			get
			{
				return this.baseTendDurationHours < 0f;
			}
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x060015A3 RID: 5539 RVA: 0x00080EB2 File Offset: 0x0007F0B2
		public int TendTicksFull
		{
			get
			{
				if (this.TendIsPermanent)
				{
					Log.ErrorOnce("Queried TendTicksFull on permanent-tend Hediff.", 6163263);
				}
				return Mathf.RoundToInt((this.baseTendDurationHours + this.tendOverlapHours) * 2500f);
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x060015A4 RID: 5540 RVA: 0x00080EE3 File Offset: 0x0007F0E3
		public int TendTicksBase
		{
			get
			{
				if (this.TendIsPermanent)
				{
					Log.ErrorOnce("Queried TendTicksBase on permanent-tend Hediff.", 61621263);
				}
				return Mathf.RoundToInt(this.baseTendDurationHours * 2500f);
			}
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x060015A5 RID: 5541 RVA: 0x00080F0D File Offset: 0x0007F10D
		public int TendTicksOverlap
		{
			get
			{
				if (this.TendIsPermanent)
				{
					Log.ErrorOnce("Queried TendTicksOverlap on permanent-tend Hediff.", 1963263);
				}
				return Mathf.RoundToInt(this.tendOverlapHours * 2500f);
			}
		}

		// Token: 0x060015A6 RID: 5542 RVA: 0x00080F37 File Offset: 0x0007F137
		public HediffCompProperties_TendDuration()
		{
			this.compClass = typeof(HediffComp_TendDuration);
		}

		// Token: 0x04001152 RID: 4434
		private float baseTendDurationHours = -1f;

		// Token: 0x04001153 RID: 4435
		private float tendOverlapHours = 3f;

		// Token: 0x04001154 RID: 4436
		public bool tendAllAtOnce;

		// Token: 0x04001155 RID: 4437
		public int disappearsAtTotalTendQuality = -1;

		// Token: 0x04001156 RID: 4438
		public float severityPerDayTended;

		// Token: 0x04001157 RID: 4439
		public bool showTendQuality = true;

		// Token: 0x04001158 RID: 4440
		[LoadAlias("labelTreatedWell")]
		public string labelTendedWell;

		// Token: 0x04001159 RID: 4441
		[LoadAlias("labelTreatedWellInner")]
		public string labelTendedWellInner;

		// Token: 0x0400115A RID: 4442
		[LoadAlias("labelSolidTreatedWell")]
		public string labelSolidTendedWell;
	}
}
