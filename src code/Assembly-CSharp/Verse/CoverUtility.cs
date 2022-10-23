using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200059B RID: 1435
	public static class CoverUtility
	{
		// Token: 0x06002BB8 RID: 11192 RVA: 0x001155D4 File Offset: 0x001137D4
		public static List<CoverInfo> CalculateCoverGiverSet(LocalTargetInfo target, IntVec3 shooterLoc, Map map)
		{
			IntVec3 cell = target.Cell;
			List<CoverInfo> list = new List<CoverInfo>();
			if (target.HasThing && !target.Thing.def.CanBenefitFromCover)
			{
				return list;
			}
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec = cell + GenAdj.AdjacentCells[i];
				CoverInfo item;
				if (intVec.InBounds(map) && CoverUtility.TryFindAdjustedCoverInCell(shooterLoc, target, intVec, map, out item) && item.BlockChance > 0f)
				{
					list.Add(item);
				}
			}
			return list;
		}

		// Token: 0x06002BB9 RID: 11193 RVA: 0x00115658 File Offset: 0x00113858
		public static float CalculateOverallBlockChance(LocalTargetInfo target, IntVec3 shooterLoc, Map map)
		{
			IntVec3 cell = target.Cell;
			float num = 0f;
			if (target.HasThing && !target.Thing.def.CanBenefitFromCover)
			{
				return num;
			}
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec = cell + GenAdj.AdjacentCells[i];
				CoverInfo coverInfo;
				if (intVec.InBounds(map) && !(shooterLoc == intVec) && CoverUtility.TryFindAdjustedCoverInCell(shooterLoc, target, intVec, map, out coverInfo))
				{
					num += (1f - num) * coverInfo.BlockChance;
				}
			}
			return num;
		}

		// Token: 0x06002BBA RID: 11194 RVA: 0x001156E4 File Offset: 0x001138E4
		private static bool TryFindAdjustedCoverInCell(IntVec3 shooterLoc, LocalTargetInfo target, IntVec3 adjCell, Map map, out CoverInfo result)
		{
			IntVec3 cell = target.Cell;
			Thing cover = adjCell.GetCover(map);
			if (cover == null || cover == target.Thing || shooterLoc == cell)
			{
				result = CoverInfo.Invalid;
				return false;
			}
			float angleFlat = (shooterLoc - cell).AngleFlat;
			float num = GenGeo.AngleDifferenceBetween((adjCell - cell).AngleFlat, angleFlat);
			if (!cell.AdjacentToCardinal(adjCell))
			{
				num *= 1.75f;
			}
			float num2 = cover.BaseBlockChance();
			if (num < 15f)
			{
				num2 *= 1f;
			}
			else if (num < 27f)
			{
				num2 *= 0.8f;
			}
			else if (num < 40f)
			{
				num2 *= 0.6f;
			}
			else if (num < 52f)
			{
				num2 *= 0.4f;
			}
			else
			{
				if (num >= 65f)
				{
					result = CoverInfo.Invalid;
					return false;
				}
				num2 *= 0.2f;
			}
			float lengthHorizontal = (shooterLoc - adjCell).LengthHorizontal;
			if (lengthHorizontal < 1.9f)
			{
				num2 *= 0.3333f;
			}
			else if (lengthHorizontal < 2.9f)
			{
				num2 *= 0.66666f;
			}
			result = new CoverInfo(cover, num2);
			return true;
		}

		// Token: 0x06002BBB RID: 11195 RVA: 0x00115823 File Offset: 0x00113A23
		public static float BaseBlockChance(this ThingDef def)
		{
			if (def.Fillage == FillCategory.Full)
			{
				return 0.75f;
			}
			return def.fillPercent;
		}

		// Token: 0x06002BBC RID: 11196 RVA: 0x0011583C File Offset: 0x00113A3C
		public static float BaseBlockChance(this Thing thing)
		{
			Building_Door building_Door = thing as Building_Door;
			if (building_Door != null && building_Door.Open)
			{
				return 0f;
			}
			return thing.def.BaseBlockChance();
		}

		// Token: 0x06002BBD RID: 11197 RVA: 0x0011586C File Offset: 0x00113A6C
		public static float TotalSurroundingCoverScore(IntVec3 c, Map map)
		{
			float num = 0f;
			for (int i = 0; i < 8; i++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCells[i];
				if (c2.InBounds(map))
				{
					Thing cover = c2.GetCover(map);
					if (cover != null)
					{
						num += cover.BaseBlockChance();
					}
				}
			}
			return num;
		}

		// Token: 0x06002BBE RID: 11198 RVA: 0x001158BC File Offset: 0x00113ABC
		public static bool ThingCovered(Thing thing, Map map)
		{
			foreach (IntVec3 c in thing.OccupiedRect())
			{
				List<Thing> thingList = c.GetThingList(map);
				bool flag = false;
				for (int i = 0; i < thingList.Count; i++)
				{
					if (thingList[i] != thing && thingList[i].def.Fillage == FillCategory.Full && thingList[i].def.Altitude >= thing.def.Altitude)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04001CB4 RID: 7348
		public const float CoverPercent_Corner = 0.75f;
	}
}
