using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002A1 RID: 673
	public class Pawn_DrawTracker
	{
		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06001334 RID: 4916 RVA: 0x0007355C File Offset: 0x0007175C
		public Vector3 DrawPos
		{
			get
			{
				this.tweener.PreDrawPosCalculation();
				Vector3 vector = this.tweener.TweenedPos;
				vector += this.jitterer.CurrentOffset;
				vector += this.leaner.LeanOffset;
				vector += this.OffsetForcedByJob();
				vector.y = this.pawn.def.Altitude;
				return vector;
			}
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x000735CC File Offset: 0x000717CC
		public Pawn_DrawTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.tweener = new PawnTweener(pawn);
			this.jitterer = new JitterHandler();
			this.leaner = new PawnLeaner(pawn);
			this.renderer = new PawnRenderer(pawn);
			this.ui = new PawnUIOverlay(pawn);
			this.footprintMaker = new PawnFootprintMaker(pawn);
			this.breathMoteMaker = new PawnBreathMoteMaker(pawn);
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x0007363C File Offset: 0x0007183C
		public void ProcessPostTickVisuals(int ticksPassed)
		{
			if (!this.pawn.Spawned)
			{
				return;
			}
			this.jitterer.ProcessPostTickVisuals(ticksPassed);
			this.footprintMaker.ProcessPostTickVisuals(ticksPassed);
			this.breathMoteMaker.ProcessPostTickVisuals(ticksPassed);
			this.leaner.ProcessPostTickVisuals(ticksPassed);
			this.renderer.ProcessPostTickVisuals(ticksPassed);
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x00073694 File Offset: 0x00071894
		public void DrawAt(Vector3 loc)
		{
			this.renderer.RenderPawnAt(loc, null, false);
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x000736B7 File Offset: 0x000718B7
		private Vector3 OffsetForcedByJob()
		{
			if (this.pawn.jobs != null && this.pawn.jobs.curDriver != null)
			{
				return this.pawn.jobs.curDriver.ForcedBodyOffset;
			}
			return Vector3.zero;
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x000736F3 File Offset: 0x000718F3
		public void Notify_Spawned()
		{
			this.tweener.ResetTweenedPosToRoot();
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x00073700 File Offset: 0x00071900
		public void Notify_WarmingCastAlongLine(ShootLine newShootLine, IntVec3 ShootPosition)
		{
			this.leaner.Notify_WarmingCastAlongLine(newShootLine, ShootPosition);
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x0007370F File Offset: 0x0007190F
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (this.pawn.Destroyed || !this.pawn.Spawned)
			{
				return;
			}
			this.jitterer.Notify_DamageApplied(dinfo);
			this.renderer.Notify_DamageApplied(dinfo);
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x00073744 File Offset: 0x00071944
		public void Notify_DamageDeflected(DamageInfo dinfo)
		{
			if (this.pawn.Destroyed)
			{
				return;
			}
			this.jitterer.Notify_DamageDeflected(dinfo);
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x00073760 File Offset: 0x00071960
		public void Notify_MeleeAttackOn(Thing Target)
		{
			if (Target.Position != this.pawn.Position)
			{
				this.jitterer.AddOffset(0.5f, (Target.Position - this.pawn.Position).AngleFlat);
				return;
			}
			if (Target.DrawPos != this.pawn.DrawPos)
			{
				this.jitterer.AddOffset(0.25f, (Target.DrawPos - this.pawn.DrawPos).AngleFlat());
			}
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x000737F8 File Offset: 0x000719F8
		public void Notify_DebugAffected()
		{
			for (int i = 0; i < 10; i++)
			{
				FleckMaker.ThrowAirPuffUp(this.pawn.DrawPosHeld.Value, this.pawn.MapHeld);
			}
			this.jitterer.AddOffset(0.05f, (float)Rand.Range(0, 360));
		}

		// Token: 0x04000FDA RID: 4058
		private Pawn pawn;

		// Token: 0x04000FDB RID: 4059
		public PawnTweener tweener;

		// Token: 0x04000FDC RID: 4060
		private JitterHandler jitterer;

		// Token: 0x04000FDD RID: 4061
		public PawnLeaner leaner;

		// Token: 0x04000FDE RID: 4062
		public PawnRenderer renderer;

		// Token: 0x04000FDF RID: 4063
		public PawnUIOverlay ui;

		// Token: 0x04000FE0 RID: 4064
		private PawnFootprintMaker footprintMaker;

		// Token: 0x04000FE1 RID: 4065
		private PawnBreathMoteMaker breathMoteMaker;

		// Token: 0x04000FE2 RID: 4066
		private const float MeleeJitterDistance = 0.5f;
	}
}
