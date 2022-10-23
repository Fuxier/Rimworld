using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000368 RID: 872
	[StaticConstructorOnStartup]
	public sealed class PawnDestinationReservationManager : IExposable
	{
		// Token: 0x06001825 RID: 6181 RVA: 0x0008F2C4 File Offset: 0x0008D4C4
		public PawnDestinationReservationManager.PawnDestinationSet GetPawnDestinationSetFor(Faction faction)
		{
			if (!this.reservedDestinations.ContainsKey(faction))
			{
				this.reservedDestinations.Add(faction, new PawnDestinationReservationManager.PawnDestinationSet());
			}
			return this.reservedDestinations[faction];
		}

		// Token: 0x06001826 RID: 6182 RVA: 0x0008F2F4 File Offset: 0x0008D4F4
		public void Reserve(Pawn p, Job job, IntVec3 loc)
		{
			if (p.Faction == null)
			{
				return;
			}
			Pawn pawn;
			if (p.Drafted && p.Faction == Faction.OfPlayer && this.IsReserved(loc, out pawn) && pawn != p && !pawn.HostileTo(p) && pawn.Faction != p.Faction)
			{
				MentalStateDef mentalStateDef = pawn.MentalStateDef;
				if (mentalStateDef == null || mentalStateDef.category != MentalStateCategory.Aggro)
				{
					MentalStateDef mentalStateDef2 = pawn.MentalStateDef;
					if (mentalStateDef2 == null || mentalStateDef2.category != MentalStateCategory.Malicious)
					{
						pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
			}
			this.ObsoleteAllClaimedBy(p);
			this.GetPawnDestinationSetFor(p.Faction).list.Add(new PawnDestinationReservationManager.PawnDestinationReservation
			{
				target = loc,
				claimant = p,
				job = job
			});
		}

		// Token: 0x06001827 RID: 6183 RVA: 0x0008F3C0 File Offset: 0x0008D5C0
		public PawnDestinationReservationManager.PawnDestinationReservation MostRecentReservationFor(Pawn p)
		{
			if (p.Faction == null)
			{
				return null;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(p.Faction).list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].claimant == p && !list[i].obsolete)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x06001828 RID: 6184 RVA: 0x0008F420 File Offset: 0x0008D620
		public IntVec3 FirstObsoleteReservationFor(Pawn p)
		{
			if (p.Faction == null)
			{
				return IntVec3.Invalid;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(p.Faction).list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].claimant == p && list[i].obsolete)
				{
					return list[i].target;
				}
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06001829 RID: 6185 RVA: 0x0008F490 File Offset: 0x0008D690
		public Job FirstObsoleteReservationJobFor(Pawn p)
		{
			if (p.Faction == null)
			{
				return null;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(p.Faction).list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].claimant == p && list[i].obsolete)
				{
					return list[i].job;
				}
			}
			return null;
		}

		// Token: 0x0600182A RID: 6186 RVA: 0x0008F4F8 File Offset: 0x0008D6F8
		public bool IsReserved(IntVec3 loc)
		{
			Pawn pawn;
			return this.IsReserved(loc, out pawn);
		}

		// Token: 0x0600182B RID: 6187 RVA: 0x0008F510 File Offset: 0x0008D710
		public bool IsReserved(IntVec3 loc, out Pawn claimant)
		{
			foreach (KeyValuePair<Faction, PawnDestinationReservationManager.PawnDestinationSet> keyValuePair in this.reservedDestinations)
			{
				List<PawnDestinationReservationManager.PawnDestinationReservation> list = keyValuePair.Value.list;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].target == loc)
					{
						claimant = list[i].claimant;
						return true;
					}
				}
			}
			claimant = null;
			return false;
		}

		// Token: 0x0600182C RID: 6188 RVA: 0x0008F5A8 File Offset: 0x0008D7A8
		public bool CanReserve(IntVec3 c, Pawn searcher, bool draftedOnly = false)
		{
			if (searcher.Faction == null)
			{
				return true;
			}
			if (searcher.Faction == Faction.OfPlayer)
			{
				return this.CanReserveInt(c, searcher.Faction, searcher, draftedOnly);
			}
			foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
			{
				if (!faction.HostileTo(searcher.Faction) && !this.CanReserveInt(c, faction, searcher, draftedOnly))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600182D RID: 6189 RVA: 0x0008F640 File Offset: 0x0008D840
		private bool CanReserveInt(IntVec3 c, Faction faction, Pawn ignoreClaimant = null, bool draftedOnly = false)
		{
			if (faction == null)
			{
				return true;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(faction).list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].target == c && (ignoreClaimant == null || list[i].claimant != ignoreClaimant) && (!draftedOnly || list[i].claimant.Drafted))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600182E RID: 6190 RVA: 0x0008F6B0 File Offset: 0x0008D8B0
		public Pawn FirstReserverOf(IntVec3 c, Faction faction)
		{
			if (faction == null)
			{
				return null;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(faction).list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].target == c)
				{
					return list[i].claimant;
				}
			}
			return null;
		}

		// Token: 0x0600182F RID: 6191 RVA: 0x0008F704 File Offset: 0x0008D904
		public void ReleaseAllObsoleteClaimedBy(Pawn p)
		{
			if (p.Faction == null)
			{
				return;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(p.Faction).list;
			int i = 0;
			while (i < list.Count)
			{
				if (list[i].claimant == p && list[i].obsolete)
				{
					list[i] = list[list.Count - 1];
					list.RemoveLast<PawnDestinationReservationManager.PawnDestinationReservation>();
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06001830 RID: 6192 RVA: 0x0008F778 File Offset: 0x0008D978
		public void ReleaseAllClaimedBy(Pawn p)
		{
			if (p.Faction == null)
			{
				return;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(p.Faction).list;
			int i = 0;
			while (i < list.Count)
			{
				if (list[i].claimant == p)
				{
					list[i] = list[list.Count - 1];
					list.RemoveLast<PawnDestinationReservationManager.PawnDestinationReservation>();
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x0008F7E0 File Offset: 0x0008D9E0
		public void ReleaseClaimedBy(Pawn p, Job job)
		{
			if (p.Faction == null)
			{
				return;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(p.Faction).list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].claimant == p && list[i].job == job)
				{
					list[i].job = null;
					if (list[i].obsolete)
					{
						list[i] = list[list.Count - 1];
						list.RemoveLast<PawnDestinationReservationManager.PawnDestinationReservation>();
						i--;
					}
				}
			}
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x0008F871 File Offset: 0x0008DA71
		public void Notify_FactionRemoved(Faction faction)
		{
			if (this.reservedDestinations.ContainsKey(faction))
			{
				this.reservedDestinations.Remove(faction);
			}
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x0008F890 File Offset: 0x0008DA90
		public void ObsoleteAllClaimedBy(Pawn p)
		{
			if (p.Faction == null)
			{
				return;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(p.Faction).list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].claimant == p)
				{
					list[i].obsolete = true;
					if (list[i].job == null)
					{
						list[i] = list[list.Count - 1];
						list.RemoveLast<PawnDestinationReservationManager.PawnDestinationReservation>();
						i--;
					}
				}
			}
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x0008F914 File Offset: 0x0008DB14
		public void DebugDrawDestinations()
		{
			foreach (PawnDestinationReservationManager.PawnDestinationReservation pawnDestinationReservation in this.GetPawnDestinationSetFor(Faction.OfPlayer).list)
			{
				if (!(pawnDestinationReservation.claimant.Position == pawnDestinationReservation.target))
				{
					IntVec3 target = pawnDestinationReservation.target;
					Vector3 s = new Vector3(1f, 1f, 1f);
					Matrix4x4 matrix = default(Matrix4x4);
					matrix.SetTRS(target.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays), Quaternion.identity, s);
					Graphics.DrawMesh(MeshPool.plane10, matrix, PawnDestinationReservationManager.DestinationMat, 0);
					if (Find.Selector.IsSelected(pawnDestinationReservation.claimant))
					{
						Graphics.DrawMesh(MeshPool.plane10, matrix, PawnDestinationReservationManager.DestinationSelectionMat, 0);
					}
				}
			}
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x0008F9FC File Offset: 0x0008DBFC
		public void DebugDrawReservations()
		{
			foreach (KeyValuePair<Faction, PawnDestinationReservationManager.PawnDestinationSet> keyValuePair in this.reservedDestinations)
			{
				foreach (PawnDestinationReservationManager.PawnDestinationReservation pawnDestinationReservation in keyValuePair.Value.list)
				{
					IntVec3 target = pawnDestinationReservation.target;
					MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
					materialPropertyBlock.SetColor("_Color", keyValuePair.Key.Color);
					Vector3 s = new Vector3(1f, 1f, 1f);
					Matrix4x4 matrix = default(Matrix4x4);
					matrix.SetTRS(target.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays), Quaternion.identity, s);
					Graphics.DrawMesh(MeshPool.plane10, matrix, PawnDestinationReservationManager.DestinationMat, 0, Camera.main, 0, materialPropertyBlock);
					if (Find.Selector.IsSelected(pawnDestinationReservation.claimant))
					{
						Graphics.DrawMesh(MeshPool.plane10, matrix, PawnDestinationReservationManager.DestinationSelectionMat, 0);
					}
				}
			}
		}

		// Token: 0x06001836 RID: 6198 RVA: 0x0008FB30 File Offset: 0x0008DD30
		public void ExposeData()
		{
			Scribe_Collections.Look<Faction, PawnDestinationReservationManager.PawnDestinationSet>(ref this.reservedDestinations, "reservedDestinations", LookMode.Reference, LookMode.Deep, ref this.reservedDestinationsKeysWorkingList, ref this.reservedDestinationsValuesWorkingList, true);
		}

		// Token: 0x04001223 RID: 4643
		private Dictionary<Faction, PawnDestinationReservationManager.PawnDestinationSet> reservedDestinations = new Dictionary<Faction, PawnDestinationReservationManager.PawnDestinationSet>();

		// Token: 0x04001224 RID: 4644
		private static readonly Material DestinationMat = MaterialPool.MatFrom("UI/Overlays/ReservedDestination");

		// Token: 0x04001225 RID: 4645
		private static readonly Material DestinationSelectionMat = MaterialPool.MatFrom("UI/Overlays/ReservedDestinationSelection");

		// Token: 0x04001226 RID: 4646
		private List<Faction> reservedDestinationsKeysWorkingList;

		// Token: 0x04001227 RID: 4647
		private List<PawnDestinationReservationManager.PawnDestinationSet> reservedDestinationsValuesWorkingList;

		// Token: 0x02001E37 RID: 7735
		public class PawnDestinationReservation : IExposable
		{
			// Token: 0x0600B828 RID: 47144 RVA: 0x0041C5E0 File Offset: 0x0041A7E0
			public void ExposeData()
			{
				Scribe_Values.Look<IntVec3>(ref this.target, "target", default(IntVec3), false);
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
				Scribe_Values.Look<bool>(ref this.obsolete, "obsolete", false, false);
			}

			// Token: 0x0400772E RID: 30510
			public IntVec3 target;

			// Token: 0x0400772F RID: 30511
			public Pawn claimant;

			// Token: 0x04007730 RID: 30512
			public Job job;

			// Token: 0x04007731 RID: 30513
			public bool obsolete;
		}

		// Token: 0x02001E38 RID: 7736
		public class PawnDestinationSet : IExposable
		{
			// Token: 0x0600B82A RID: 47146 RVA: 0x0041C63C File Offset: 0x0041A83C
			public void ExposeData()
			{
				Scribe_Collections.Look<PawnDestinationReservationManager.PawnDestinationReservation>(ref this.list, "list", LookMode.Deep, Array.Empty<object>());
				if (Scribe.mode == LoadSaveMode.PostLoadInit)
				{
					if (this.list.RemoveAll((PawnDestinationReservationManager.PawnDestinationReservation x) => x.claimant.DestroyedOrNull()) != 0)
					{
						Log.Warning("Some destination reservations had null or destroyed claimant.");
					}
				}
			}

			// Token: 0x04007732 RID: 30514
			public List<PawnDestinationReservationManager.PawnDestinationReservation> list = new List<PawnDestinationReservationManager.PawnDestinationReservation>();
		}
	}
}
