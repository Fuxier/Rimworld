using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x020001AA RID: 426
	public class AnimalPenBalanceCalculator
	{
		// Token: 0x06000BCC RID: 3020 RVA: 0x00042741 File Offset: 0x00040941
		public AnimalPenBalanceCalculator(Map map, bool considerInProgressMovement)
		{
			this.map = map;
			this.considerInProgressMovement = considerInProgressMovement;
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x00042769 File Offset: 0x00040969
		public void MarkDirty()
		{
			this.dirty = true;
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x00042774 File Offset: 0x00040974
		public bool IsBetterPen(CompAnimalPenMarker markerA, CompAnimalPenMarker markerB, bool leavingMarkerB, Pawn animal)
		{
			this.RecalculateIfDirty();
			District district = markerA.parent.GetDistrict(RegionType.Set_Passable);
			District district2 = markerB.parent.GetDistrict(RegionType.Set_Passable);
			if (district == district2)
			{
				return false;
			}
			float bodySize = animal.BodySize;
			float num = this.TotalBodySizeIn(district) + bodySize;
			float num2 = this.TotalBodySizeIn(district2) + (leavingMarkerB ? (-bodySize) : bodySize);
			float num3 = num / (float)district.CellCount;
			float num4 = num2 / (float)district2.CellCount;
			return num3 * 1.2f < num4;
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x000427E8 File Offset: 0x000409E8
		public float TotalBodySizeIn(District district)
		{
			this.RecalculateIfDirty();
			float num = 0f;
			foreach (AnimalPenBalanceCalculator.AnimalMembershipInfo animalMembershipInfo in this.membership)
			{
				if (animalMembershipInfo.pen == district)
				{
					num += animalMembershipInfo.animal.BodySize;
				}
			}
			return num;
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x00042858 File Offset: 0x00040A58
		private void RecalculateIfDirty()
		{
			if (!this.dirty)
			{
				return;
			}
			this.dirty = false;
			this.membership.Clear();
			foreach (Pawn pawn in this.map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer))
			{
				if (AnimalPenUtility.NeedsToBeManagedByRope(pawn))
				{
					District district = null;
					if (this.considerInProgressMovement && pawn.roping.IsRopedByPawn && pawn.roping.RopedByPawn.jobs.curDriver is JobDriver_RopeToPen)
					{
						Thing thing = pawn.roping.RopedByPawn.CurJob.GetTarget(TargetIndex.C).Thing;
						District district2 = (thing != null) ? thing.GetDistrict(RegionType.Set_Passable) : null;
						if (district2 != null && !district2.TouchesMapEdge)
						{
							district = district2;
						}
					}
					if (district == null)
					{
						CompAnimalPenMarker currentPenOf = AnimalPenUtility.GetCurrentPenOf(pawn, false);
						district = ((currentPenOf != null) ? currentPenOf.parent.GetDistrict(RegionType.Set_Passable) : null);
					}
					this.membership.Add(new AnimalPenBalanceCalculator.AnimalMembershipInfo
					{
						animal = pawn,
						pen = district
					});
				}
			}
		}

		// Token: 0x04000B04 RID: 2820
		private const float DensityTolerance = 0.2f;

		// Token: 0x04000B05 RID: 2821
		private readonly Map map;

		// Token: 0x04000B06 RID: 2822
		private bool considerInProgressMovement;

		// Token: 0x04000B07 RID: 2823
		private readonly List<AnimalPenBalanceCalculator.AnimalMembershipInfo> membership = new List<AnimalPenBalanceCalculator.AnimalMembershipInfo>();

		// Token: 0x04000B08 RID: 2824
		private bool dirty = true;

		// Token: 0x02001D40 RID: 7488
		private struct AnimalMembershipInfo
		{
			// Token: 0x04007391 RID: 29585
			public Pawn animal;

			// Token: 0x04007392 RID: 29586
			public District pen;
		}
	}
}
