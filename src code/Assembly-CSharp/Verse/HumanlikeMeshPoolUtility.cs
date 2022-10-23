using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000397 RID: 919
	public static class HumanlikeMeshPoolUtility
	{
		// Token: 0x06001A56 RID: 6742 RVA: 0x0009EE57 File Offset: 0x0009D057
		public static float HumanlikeBodyWidthForPawn(Pawn pawn)
		{
			if (ModsConfig.BiotechActive && pawn.ageTracker.CurLifeStage.bodyWidth != null)
			{
				return pawn.ageTracker.CurLifeStage.bodyWidth.Value;
			}
			return 1.5f;
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x0009EE92 File Offset: 0x0009D092
		public static GraphicMeshSet GetHumanlikeBodySetForPawn(Pawn pawn)
		{
			if (ModsConfig.BiotechActive && pawn.ageTracker.CurLifeStage.bodyWidth != null)
			{
				return MeshPool.GetMeshSetForWidth(pawn.ageTracker.CurLifeStage.bodyWidth.Value);
			}
			return MeshPool.humanlikeBodySet;
		}

		// Token: 0x06001A58 RID: 6744 RVA: 0x0009EED2 File Offset: 0x0009D0D2
		public static GraphicMeshSet GetHumanlikeHeadSetForPawn(Pawn pawn)
		{
			if (ModsConfig.BiotechActive && pawn.ageTracker.CurLifeStage.bodyWidth != null)
			{
				return MeshPool.GetMeshSetForWidth(pawn.ageTracker.CurLifeStage.bodyWidth.Value);
			}
			return MeshPool.humanlikeHeadSet;
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x0009EF14 File Offset: 0x0009D114
		public static GraphicMeshSet GetHumanlikeHairSetForPawn(Pawn pawn)
		{
			Vector2 vector = pawn.story.headType.hairMeshSize;
			if (ModsConfig.BiotechActive && pawn.ageTracker.CurLifeStage.headSizeFactor != null)
			{
				vector *= pawn.ageTracker.CurLifeStage.headSizeFactor.Value;
			}
			return MeshPool.GetMeshSetForWidth(vector.x, vector.y);
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x0009EF80 File Offset: 0x0009D180
		public static GraphicMeshSet GetHumanlikeBeardSetForPawn(Pawn pawn)
		{
			Vector2 vector = pawn.story.headType.beardMeshSize;
			if (ModsConfig.BiotechActive && pawn.ageTracker.CurLifeStage.headSizeFactor != null)
			{
				vector *= pawn.ageTracker.CurLifeStage.headSizeFactor.Value;
			}
			return MeshPool.GetMeshSetForWidth(vector.x, vector.y);
		}

		// Token: 0x06001A5B RID: 6747 RVA: 0x0009EFE9 File Offset: 0x0009D1E9
		public static GraphicMeshSet GetSwaddledBabySet()
		{
			return MeshPool.humanlikeSwaddledBaby;
		}
	}
}
