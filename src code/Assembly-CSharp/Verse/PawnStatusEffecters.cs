using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200029D RID: 669
	internal struct PawnStatusEffecters
	{
		// Token: 0x06001324 RID: 4900 RVA: 0x00072AD0 File Offset: 0x00070CD0
		public PawnStatusEffecters(Pawn pawn)
		{
			this.pawn = pawn;
			this.pairs = new List<PawnStatusEffecters.LiveEffecter>();
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x00072AE4 File Offset: 0x00070CE4
		public void EffectersTick(bool suspended)
		{
			if (!suspended)
			{
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					HediffComp_Effecter hediffComp_Effecter = hediffs[i].TryGetComp<HediffComp_Effecter>();
					if (hediffComp_Effecter != null)
					{
						EffecterDef effecterDef = hediffComp_Effecter.CurrentStateEffecter();
						if (effecterDef != null)
						{
							this.AddOrMaintain(effecterDef);
						}
					}
				}
				Pawn_MindState mindState = this.pawn.mindState;
				if (((mindState != null) ? mindState.mentalStateHandler.CurState : null) != null)
				{
					EffecterDef effecterDef2 = this.pawn.mindState.mentalStateHandler.CurState.CurrentStateEffecter();
					if (effecterDef2 != null)
					{
						this.AddOrMaintain(effecterDef2);
					}
				}
			}
			for (int j = this.pairs.Count - 1; j >= 0; j--)
			{
				if (this.pairs[j].Expired)
				{
					this.pairs[j].Cleanup();
					this.pairs.RemoveAt(j);
				}
				else
				{
					this.pairs[j].Tick(this.pawn);
				}
			}
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x00072BF0 File Offset: 0x00070DF0
		private void AddOrMaintain(EffecterDef def)
		{
			for (int i = 0; i < this.pairs.Count; i++)
			{
				if (this.pairs[i].def == def)
				{
					this.pairs[i].Maintain();
					return;
				}
			}
			PawnStatusEffecters.LiveEffecter liveEffecter = FullPool<PawnStatusEffecters.LiveEffecter>.Get();
			liveEffecter.def = def;
			liveEffecter.Maintain();
			this.pairs.Add(liveEffecter);
		}

		// Token: 0x04000FCD RID: 4045
		public Pawn pawn;

		// Token: 0x04000FCE RID: 4046
		private List<PawnStatusEffecters.LiveEffecter> pairs;

		// Token: 0x02001DE3 RID: 7651
		private class LiveEffecter : IFullPoolable
		{
			// Token: 0x17001E7D RID: 7805
			// (get) Token: 0x0600B668 RID: 46696 RVA: 0x004162C0 File Offset: 0x004144C0
			public bool Expired
			{
				get
				{
					return Find.TickManager.TicksGame > this.lastMaintainTick;
				}
			}

			// Token: 0x0600B66A RID: 46698 RVA: 0x004162D4 File Offset: 0x004144D4
			public void Cleanup()
			{
				if (this.effecter != null)
				{
					this.effecter.Cleanup();
				}
				FullPool<PawnStatusEffecters.LiveEffecter>.Return(this);
			}

			// Token: 0x0600B66B RID: 46699 RVA: 0x004162EF File Offset: 0x004144EF
			public void Reset()
			{
				this.def = null;
				this.effecter = null;
				this.lastMaintainTick = -1;
			}

			// Token: 0x0600B66C RID: 46700 RVA: 0x00416306 File Offset: 0x00414506
			public void Maintain()
			{
				this.lastMaintainTick = Find.TickManager.TicksGame;
			}

			// Token: 0x0600B66D RID: 46701 RVA: 0x00416318 File Offset: 0x00414518
			public void Tick(Pawn pawn)
			{
				if (this.effecter == null)
				{
					this.effecter = this.def.SpawnAttached(pawn, pawn.MapHeld, 1f);
				}
				this.effecter.EffectTick(pawn, pawn);
			}

			// Token: 0x040075EF RID: 30191
			public EffecterDef def;

			// Token: 0x040075F0 RID: 30192
			public Effecter effecter;

			// Token: 0x040075F1 RID: 30193
			public int lastMaintainTick;
		}
	}
}
