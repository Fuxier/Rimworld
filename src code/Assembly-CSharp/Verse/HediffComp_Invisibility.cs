using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002FD RID: 765
	public class HediffComp_Invisibility : HediffComp
	{
		// Token: 0x06001524 RID: 5412 RVA: 0x0007F829 File Offset: 0x0007DA29
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.UpdateTarget();
		}

		// Token: 0x06001525 RID: 5413 RVA: 0x0007F838 File Offset: 0x0007DA38
		public override void CompPostPostRemoved()
		{
			base.CompPostPostRemoved();
			this.UpdateTarget();
		}

		// Token: 0x06001526 RID: 5414 RVA: 0x0007F848 File Offset: 0x0007DA48
		private void UpdateTarget()
		{
			if (!ModLister.CheckRoyalty("Invisibility hediff"))
			{
				return;
			}
			Pawn pawn = this.parent.pawn;
			if (pawn.Spawned)
			{
				pawn.Map.attackTargetsCache.UpdateTarget(pawn);
			}
			PortraitsCache.SetDirty(pawn);
			GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(pawn);
		}
	}
}
