using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000089 RID: 137
	public class PhysicalInteractionReservationManager : IExposable
	{
		// Token: 0x060004E8 RID: 1256 RVA: 0x0001AFD4 File Offset: 0x000191D4
		public void Reserve(Pawn claimant, Job job, LocalTargetInfo target)
		{
			if (this.IsReservedBy(claimant, target))
			{
				return;
			}
			PhysicalInteractionReservationManager.PhysicalInteractionReservation physicalInteractionReservation = new PhysicalInteractionReservationManager.PhysicalInteractionReservation();
			physicalInteractionReservation.target = target;
			physicalInteractionReservation.claimant = claimant;
			physicalInteractionReservation.job = job;
			this.reservations.Add(physicalInteractionReservation);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0001B014 File Offset: 0x00019214
		public void Release(Pawn claimant, Job job, LocalTargetInfo target)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				PhysicalInteractionReservationManager.PhysicalInteractionReservation physicalInteractionReservation = this.reservations[i];
				if (physicalInteractionReservation.target == target && physicalInteractionReservation.claimant == claimant && physicalInteractionReservation.job == job)
				{
					this.reservations.RemoveAt(i);
					return;
				}
			}
			Log.Warning(string.Concat(new object[]
			{
				claimant,
				" tried to release reservation on target ",
				target,
				", but it's not reserved by him."
			}));
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0001B0A0 File Offset: 0x000192A0
		public bool IsReservedBy(Pawn claimant, LocalTargetInfo target)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				PhysicalInteractionReservationManager.PhysicalInteractionReservation physicalInteractionReservation = this.reservations[i];
				if (physicalInteractionReservation.target == target && physicalInteractionReservation.claimant == claimant)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001B0EA File Offset: 0x000192EA
		public bool IsReserved(LocalTargetInfo target)
		{
			return this.FirstReserverOf(target) != null;
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0001B0F8 File Offset: 0x000192F8
		public Pawn FirstReserverOf(LocalTargetInfo target)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				PhysicalInteractionReservationManager.PhysicalInteractionReservation physicalInteractionReservation = this.reservations[i];
				if (physicalInteractionReservation.target == target)
				{
					return physicalInteractionReservation.claimant;
				}
			}
			return null;
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0001B140 File Offset: 0x00019340
		public void ReserversOf(LocalTargetInfo target, HashSet<Pawn> reserversOut)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				PhysicalInteractionReservationManager.PhysicalInteractionReservation physicalInteractionReservation = this.reservations[i];
				if (physicalInteractionReservation.target == target)
				{
					reserversOut.Add(physicalInteractionReservation.claimant);
				}
			}
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0001B18C File Offset: 0x0001938C
		public LocalTargetInfo FirstReservationFor(Pawn claimant)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].claimant == claimant)
				{
					return this.reservations[i].target;
				}
			}
			return LocalTargetInfo.Invalid;
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0001B1DC File Offset: 0x000193DC
		public void ReleaseAllForTarget(LocalTargetInfo target)
		{
			this.reservations.RemoveAll((PhysicalInteractionReservationManager.PhysicalInteractionReservation x) => x.target == target);
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0001B210 File Offset: 0x00019410
		public void ReleaseClaimedBy(Pawn claimant, Job job)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].claimant == claimant && this.reservations[i].job == job)
				{
					this.reservations.RemoveAt(i);
				}
			}
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0001B26C File Offset: 0x0001946C
		public void ReleaseAllClaimedBy(Pawn claimant)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].claimant == claimant)
				{
					this.reservations.RemoveAt(i);
				}
			}
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0001B2B4 File Offset: 0x000194B4
		public void ExposeData()
		{
			Scribe_Collections.Look<PhysicalInteractionReservationManager.PhysicalInteractionReservation>(ref this.reservations, "reservations", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.reservations.RemoveAll((PhysicalInteractionReservationManager.PhysicalInteractionReservation x) => x.claimant.DestroyedOrNull()) != 0)
				{
					Log.Warning("Some physical interaction reservations had null or destroyed claimant.");
				}
			}
		}

		// Token: 0x04000230 RID: 560
		private List<PhysicalInteractionReservationManager.PhysicalInteractionReservation> reservations = new List<PhysicalInteractionReservationManager.PhysicalInteractionReservation>();

		// Token: 0x02001CA2 RID: 7330
		public class PhysicalInteractionReservation : IExposable
		{
			// Token: 0x0600B025 RID: 45093 RVA: 0x003FF7FF File Offset: 0x003FD9FF
			public void ExposeData()
			{
				Scribe_TargetInfo.Look(ref this.target, "target");
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
			}

			// Token: 0x040070D0 RID: 28880
			public LocalTargetInfo target;

			// Token: 0x040070D1 RID: 28881
			public Pawn claimant;

			// Token: 0x040070D2 RID: 28882
			public Job job;
		}
	}
}
