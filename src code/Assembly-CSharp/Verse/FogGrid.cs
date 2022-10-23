using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020001D5 RID: 469
	public sealed class FogGrid : IExposable
	{
		// Token: 0x06000D0D RID: 3341 RVA: 0x00049217 File Offset: 0x00047417
		public FogGrid(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x00049226 File Offset: 0x00047426
		public void ExposeData()
		{
			DataExposeUtility.BoolArray(ref this.fogGrid, this.map.Area, "fogGrid");
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x00049244 File Offset: 0x00047444
		public void Unfog(IntVec3 c)
		{
			this.UnfogWorker(c);
			List<Thing> thingList = c.GetThingList(this.map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (thing.def.Fillage == FillCategory.Full)
				{
					foreach (IntVec3 c2 in thing.OccupiedRect().Cells)
					{
						this.UnfogWorker(c2);
					}
				}
			}
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x000492D8 File Offset: 0x000474D8
		private void UnfogWorker(IntVec3 c)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			if (!this.fogGrid[num])
			{
				return;
			}
			this.fogGrid[num] = false;
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Things | MapMeshFlag.FogOfWar);
			}
			Designation designation = this.map.designationManager.DesignationAt(c, DesignationDefOf.Mine);
			if (designation != null && c.GetFirstMineable(this.map) == null)
			{
				designation.Delete();
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.map.roofGrid.Drawer.SetDirty();
				this.map.mapTemperature.Drawer.SetDirty();
			}
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x00049385 File Offset: 0x00047585
		public bool IsFogged(IntVec3 c)
		{
			return c.InBounds(this.map) && this.fogGrid != null && this.fogGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x000493B7 File Offset: 0x000475B7
		public bool IsFogged(int index)
		{
			return this.fogGrid[index];
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x000493C4 File Offset: 0x000475C4
		public void ClearAllFog()
		{
			for (int i = 0; i < this.map.Size.x; i++)
			{
				for (int j = 0; j < this.map.Size.z; j++)
				{
					this.Unfog(new IntVec3(i, 0, j));
				}
			}
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x00049418 File Offset: 0x00047618
		public void Notify_FogBlockerRemoved(IntVec3 c)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			bool flag = false;
			for (int i = 0; i < 8; i++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCells[i];
				if (c2.InBounds(this.map) && !this.IsFogged(c2))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return;
			}
			this.FloodUnfogAdjacent(c, true);
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x00049475 File Offset: 0x00047675
		public void Notify_PawnEnteringDoor(Building_Door door, Pawn pawn)
		{
			if (pawn.Faction != Faction.OfPlayer && pawn.HostFaction != Faction.OfPlayer)
			{
				return;
			}
			this.FloodUnfogAdjacent(door.Position, false);
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x000494A0 File Offset: 0x000476A0
		internal void SetAllFogged()
		{
			CellIndices cellIndices = this.map.cellIndices;
			if (this.fogGrid == null)
			{
				this.fogGrid = new bool[cellIndices.NumGridCells];
			}
			foreach (IntVec3 c in this.map.AllCells)
			{
				this.fogGrid[cellIndices.CellToIndex(c)] = true;
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.map.roofGrid.Drawer.SetDirty();
			}
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x0004953C File Offset: 0x0004773C
		private void FloodUnfogAdjacent(IntVec3 c, bool sendLetters = true)
		{
			this.Unfog(c);
			bool flag = false;
			FloodUnfogResult floodUnfogResult = default(FloodUnfogResult);
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec = c + GenAdj.CardinalDirections[i];
				if (intVec.InBounds(this.map) && intVec.Fogged(this.map))
				{
					Building edifice = intVec.GetEdifice(this.map);
					if (edifice == null || !edifice.def.MakeFog)
					{
						flag = true;
						floodUnfogResult = FloodFillerFog.FloodUnfog(intVec, this.map);
					}
					else
					{
						this.Unfog(intVec);
					}
				}
			}
			for (int j = 0; j < 8; j++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCells[j];
				if (c2.InBounds(this.map))
				{
					Building edifice2 = c2.GetEdifice(this.map);
					if (edifice2 != null && edifice2.def.MakeFog)
					{
						this.Unfog(c2);
					}
				}
			}
			if (flag && sendLetters)
			{
				if (floodUnfogResult.mechanoidFound)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelAreaRevealed".Translate(), "AreaRevealedWithMechanoids".Translate(), LetterDefOf.ThreatBig, new TargetInfo(c, this.map, false), null, null, null, null);
					return;
				}
				if (!floodUnfogResult.allOnScreen || floodUnfogResult.cellsUnfogged >= 600)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelAreaRevealed".Translate(), "AreaRevealed".Translate(), LetterDefOf.NeutralEvent, new TargetInfo(c, this.map, false), null, null, null, null);
				}
			}
		}

		// Token: 0x04000BF0 RID: 3056
		private Map map;

		// Token: 0x04000BF1 RID: 3057
		public bool[] fogGrid;

		// Token: 0x04000BF2 RID: 3058
		private const int AlwaysSendLetterIfUnfoggedMoreCellsThan = 600;
	}
}
