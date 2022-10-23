using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200029A RID: 666
	[StaticConstructorOnStartup]
	public class PawnHeadOverlays
	{
		// Token: 0x06001318 RID: 4888 RVA: 0x00072531 File Offset: 0x00070731
		public PawnHeadOverlays(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x00072540 File Offset: 0x00070740
		public void RenderStatusOverlays(Vector3 bodyLoc, Quaternion quat, Mesh headMesh)
		{
			if (!this.pawn.IsColonistPlayerControlled)
			{
				return;
			}
			Vector3 headLoc = bodyLoc + new Vector3(0f, 0f, 0.32f);
			if (this.pawn.needs.mood != null && !this.pawn.Downed && this.pawn.HitPoints > 0)
			{
				if (this.pawn.mindState.mentalBreaker.BreakExtremeIsImminent)
				{
					if (Time.time % 1.2f < 0.4f)
					{
						this.DrawHeadGlow(headLoc, PawnHeadOverlays.MentalStateImminentMat);
						return;
					}
				}
				else if (this.pawn.mindState.mentalBreaker.BreakExtremeIsApproaching && Time.time % 1.2f < 0.4f)
				{
					this.DrawHeadGlow(headLoc, PawnHeadOverlays.UnhappyMat);
				}
			}
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x00072611 File Offset: 0x00070811
		private void DrawHeadGlow(Vector3 headLoc, Material mat)
		{
			Graphics.DrawMesh(MeshPool.plane20, headLoc, Quaternion.identity, mat, 0);
		}

		// Token: 0x04000FB7 RID: 4023
		private Pawn pawn;

		// Token: 0x04000FB8 RID: 4024
		private const float AngerBlinkPeriod = 1.2f;

		// Token: 0x04000FB9 RID: 4025
		private const float AngerBlinkLength = 0.4f;

		// Token: 0x04000FBA RID: 4026
		private static readonly Material UnhappyMat = MaterialPool.MatFrom("Things/Pawn/Effects/Unhappy");

		// Token: 0x04000FBB RID: 4027
		private static readonly Material MentalStateImminentMat = MaterialPool.MatFrom("Things/Pawn/Effects/MentalStateImminent");
	}
}
