using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200029B RID: 667
	public class PawnLeaner
	{
		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x0600131C RID: 4892 RVA: 0x00072645 File Offset: 0x00070845
		public Vector3 LeanOffset
		{
			get
			{
				return this.shootSourceOffset.ToVector3() * 0.5f * this.leanOffsetCurPct;
			}
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x00072667 File Offset: 0x00070867
		public PawnLeaner(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x00072684 File Offset: 0x00070884
		public void ProcessPostTickVisuals(int ticksPassed)
		{
			if (this.ShouldLean())
			{
				this.leanOffsetCurPct += 0.075f * (float)ticksPassed;
				if (this.leanOffsetCurPct > 1f)
				{
					this.leanOffsetCurPct = 1f;
					return;
				}
			}
			else
			{
				this.leanOffsetCurPct -= 0.075f * (float)ticksPassed;
				if (this.leanOffsetCurPct < 0f)
				{
					this.leanOffsetCurPct = 0f;
				}
			}
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x000726F4 File Offset: 0x000708F4
		public bool ShouldLean()
		{
			return this.pawn.stances.curStance is Stance_Busy && !(this.shootSourceOffset == new IntVec3(0, 0, 0));
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x00072727 File Offset: 0x00070927
		public void Notify_WarmingCastAlongLine(ShootLine newShootLine, IntVec3 ShootPosition)
		{
			this.shootSourceOffset = newShootLine.Source - this.pawn.Position;
		}

		// Token: 0x04000FBC RID: 4028
		private Pawn pawn;

		// Token: 0x04000FBD RID: 4029
		private IntVec3 shootSourceOffset = new IntVec3(0, 0, 0);

		// Token: 0x04000FBE RID: 4030
		private float leanOffsetCurPct;

		// Token: 0x04000FBF RID: 4031
		private const float LeanOffsetPctChangeRate = 0.075f;

		// Token: 0x04000FC0 RID: 4032
		private const float LeanOffsetDistanceMultiplier = 0.5f;
	}
}
