using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200029E RID: 670
	public class PawnTweener
	{
		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06001327 RID: 4903 RVA: 0x00072C58 File Offset: 0x00070E58
		public Vector3 TweenedPos
		{
			get
			{
				return this.tweenedPos;
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06001328 RID: 4904 RVA: 0x00072C60 File Offset: 0x00070E60
		public Vector3 LastTickTweenedVelocity
		{
			get
			{
				return this.TweenedPos - this.lastTickSpringPos;
			}
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x00072C73 File Offset: 0x00070E73
		public PawnTweener(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x00072CA4 File Offset: 0x00070EA4
		public void PreDrawPosCalculation()
		{
			if (this.lastDrawFrame == RealTime.frameCount)
			{
				return;
			}
			if (this.lastDrawFrame < RealTime.frameCount - 1)
			{
				this.ResetTweenedPosToRoot();
			}
			else
			{
				this.lastTickSpringPos = this.tweenedPos;
				float tickRateMultiplier = Find.TickManager.TickRateMultiplier;
				if (tickRateMultiplier < 5f)
				{
					Vector3 a = this.TweenedPosRoot() - this.tweenedPos;
					float num = 0.09f * (RealTime.deltaTime * 60f * tickRateMultiplier);
					if (RealTime.deltaTime > 0.05f)
					{
						num = Mathf.Min(num, 1f);
					}
					this.tweenedPos += a * num;
				}
				else
				{
					this.tweenedPos = this.TweenedPosRoot();
				}
			}
			this.lastDrawFrame = RealTime.frameCount;
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x00072D67 File Offset: 0x00070F67
		public void ResetTweenedPosToRoot()
		{
			this.tweenedPos = this.TweenedPosRoot();
			this.lastTickSpringPos = this.tweenedPos;
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x00072D84 File Offset: 0x00070F84
		private Vector3 TweenedPosRoot()
		{
			if (!this.pawn.Spawned)
			{
				return this.pawn.Position.ToVector3Shifted();
			}
			float z = 0f;
			if (this.pawn.Spawned && this.pawn.ageTracker.CurLifeStage.sittingOffset != null && !this.pawn.pather.MovingNow && this.pawn.GetPosture() == PawnPosture.Standing)
			{
				Building edifice = this.pawn.Position.GetEdifice(this.pawn.Map);
				if (edifice != null && edifice.def.building != null && edifice.def.building.isSittable)
				{
					z = this.pawn.ageTracker.CurLifeStage.sittingOffset.Value;
				}
			}
			float num = this.MovedPercent();
			return this.pawn.pather.nextCell.ToVector3Shifted() * num + this.pawn.Position.ToVector3Shifted() * (1f - num) + new Vector3(0f, 0f, z) + PawnCollisionTweenerUtility.PawnCollisionPosOffsetFor(this.pawn);
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x00072EC8 File Offset: 0x000710C8
		private float MovedPercent()
		{
			if (!this.pawn.pather.Moving)
			{
				return 0f;
			}
			if (this.pawn.stances.FullBodyBusy)
			{
				return 0f;
			}
			if (this.pawn.pather.BuildingBlockingNextPathCell() != null)
			{
				return 0f;
			}
			if (this.pawn.pather.NextCellDoorToWaitForOrManuallyOpen() != null)
			{
				return 0f;
			}
			if (this.pawn.pather.WillCollideWithPawnOnNextPathCell())
			{
				return 0f;
			}
			return 1f - this.pawn.pather.nextCellCostLeft / this.pawn.pather.nextCellCostTotal;
		}

		// Token: 0x04000FCF RID: 4047
		private Pawn pawn;

		// Token: 0x04000FD0 RID: 4048
		private Vector3 tweenedPos = new Vector3(0f, 0f, 0f);

		// Token: 0x04000FD1 RID: 4049
		private int lastDrawFrame = -1;

		// Token: 0x04000FD2 RID: 4050
		private Vector3 lastTickSpringPos;

		// Token: 0x04000FD3 RID: 4051
		private const float SpringTightness = 0.09f;
	}
}
