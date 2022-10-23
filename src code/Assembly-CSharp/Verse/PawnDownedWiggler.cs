using System;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020002A4 RID: 676
	public class PawnDownedWiggler
	{
		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06001347 RID: 4935 RVA: 0x00073938 File Offset: 0x00071B38
		private static float RandomDownedAngle
		{
			get
			{
				float num = Rand.Range(45f, 135f);
				if (Rand.Value < 0.5f)
				{
					num += 180f;
				}
				return num;
			}
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x0007396A File Offset: 0x00071B6A
		public PawnDownedWiggler(Pawn pawn)
		{
			this.pawn = pawn;
			this.wiggleOffset = Mathf.Abs(pawn.HashOffset()) % 600;
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x0007399C File Offset: 0x00071B9C
		public void ProcessPostTickVisuals(int ticksPassed)
		{
			if (this.pawn.Downed && this.pawn.Spawned && !this.pawn.InBed())
			{
				this.ticksToIncapIcon -= ticksPassed;
				if (this.ticksToIncapIcon <= 0)
				{
					this.ticksToIncapIcon = 200 + this.ticksToIncapIcon;
					if (HealthAIUtility.WantsToBeRescuedIfDowned(this.pawn))
					{
						FleckMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, FleckDefOf.IncapIcon, 0.42f);
					}
				}
				if (this.pawn.Awake())
				{
					if (ModsConfig.BiotechActive)
					{
						Job curJob = this.pawn.CurJob;
						if (((curJob != null) ? curJob.def : null) == JobDefOf.Breastfeed)
						{
							return;
						}
					}
					if (ticksPassed > 600)
					{
						Log.Warning("Too many ticks passed during a single frame for sensical wiggling");
					}
					int num = (Find.TickManager.TicksGame + this.wiggleOffset) % 600;
					this.ProcessWigglePeriod(num, ticksPassed, true);
					this.ProcessWigglePeriod((num + 300) % 600, ticksPassed, false);
				}
			}
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x00073AB0 File Offset: 0x00071CB0
		private void ProcessWigglePeriod(int wigglePeriodTick, int ticksPassed, bool positiveAngleWiggle)
		{
			int a = 0;
			int a2 = 90;
			int b = wigglePeriodTick - ticksPassed;
			int num = Mathf.Min(a2, wigglePeriodTick) - Mathf.Max(a, b);
			if (num > 0)
			{
				this.downedAngle += 0.35f * (float)num * (float)(positiveAngleWiggle ? 1 : -1);
			}
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x00073AF6 File Offset: 0x00071CF6
		public void SetToCustomRotation(float rot)
		{
			this.downedAngle = rot;
			this.usingCustomRotation = true;
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x00073B08 File Offset: 0x00071D08
		public void Notify_DamageApplied(DamageInfo dam)
		{
			if ((this.pawn.Downed || this.pawn.Dead) && dam.Def.hasForcefulImpact)
			{
				this.downedAngle += 10f * Rand.Range(-1f, 1f);
				if (!this.usingCustomRotation)
				{
					if (this.downedAngle > 315f)
					{
						this.downedAngle = 315f;
					}
					if (this.downedAngle < 45f)
					{
						this.downedAngle = 45f;
					}
					if (this.downedAngle > 135f && this.downedAngle < 225f)
					{
						if (this.downedAngle > 180f)
						{
							this.downedAngle = 225f;
							return;
						}
						this.downedAngle = 135f;
						return;
					}
				}
				else
				{
					if (this.downedAngle >= 360f)
					{
						this.downedAngle -= 360f;
					}
					if (this.downedAngle < 0f)
					{
						this.downedAngle += 360f;
					}
				}
			}
		}

		// Token: 0x04000FE7 RID: 4071
		private Pawn pawn;

		// Token: 0x04000FE8 RID: 4072
		public float downedAngle = PawnDownedWiggler.RandomDownedAngle;

		// Token: 0x04000FE9 RID: 4073
		public int ticksToIncapIcon;

		// Token: 0x04000FEA RID: 4074
		private bool usingCustomRotation;

		// Token: 0x04000FEB RID: 4075
		private int wiggleOffset;

		// Token: 0x04000FEC RID: 4076
		private const float DownedAngleWidth = 45f;

		// Token: 0x04000FED RID: 4077
		private const float DamageTakenDownedAngleShift = 10f;

		// Token: 0x04000FEE RID: 4078
		private const int IncapWigglePeriod = 300;

		// Token: 0x04000FEF RID: 4079
		private const int IncapWiggleLength = 90;

		// Token: 0x04000FF0 RID: 4080
		private const float IncapWiggleSpeed = 0.35f;

		// Token: 0x04000FF1 RID: 4081
		private const int TicksBetweenIncapIcons = 200;
	}
}
