using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200025C RID: 604
	public static class GenTemperature
	{
		// Token: 0x06001132 RID: 4402 RVA: 0x000644D8 File Offset: 0x000626D8
		public static float AverageTemperatureAtTileForTwelfth(int tile, Twelfth twelfth)
		{
			int num = 30000;
			int num2 = 300000 * (int)twelfth;
			float num3 = 0f;
			for (int i = 0; i < 120; i++)
			{
				int absTick = num2 + num + Mathf.RoundToInt((float)i / 120f * 300000f);
				num3 += GenTemperature.GetTemperatureFromSeasonAtTile(absTick, tile);
			}
			return num3 / 120f;
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x00064534 File Offset: 0x00062734
		public static float MinTemperatureAtTile(int tile)
		{
			float num = float.MaxValue;
			for (int i = 0; i < 3600000; i += 27000)
			{
				num = Mathf.Min(num, GenTemperature.GetTemperatureFromSeasonAtTile(i, tile));
			}
			return num;
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x0006456C File Offset: 0x0006276C
		public static float MaxTemperatureAtTile(int tile)
		{
			float num = float.MinValue;
			for (int i = 0; i < 3600000; i += 27000)
			{
				num = Mathf.Max(num, GenTemperature.GetTemperatureFromSeasonAtTile(i, tile));
			}
			return num;
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x000645A2 File Offset: 0x000627A2
		public static FloatRange ComfortableTemperatureRange(this Pawn p)
		{
			return new FloatRange(p.GetStatValue(StatDefOf.ComfyTemperatureMin, true, 1), p.GetStatValue(StatDefOf.ComfyTemperatureMax, true, 1));
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x000645C4 File Offset: 0x000627C4
		public static FloatRange ComfortableTemperatureRange(ThingDef raceDef, List<ThingStuffPair> apparel = null)
		{
			FloatRange result = new FloatRange(raceDef.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null), raceDef.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null));
			if (apparel != null)
			{
				result.min -= apparel.Sum((ThingStuffPair x) => x.InsulationCold);
				result.max += apparel.Sum((ThingStuffPair x) => x.InsulationHeat);
			}
			return result;
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x00064654 File Offset: 0x00062854
		public static FloatRange SafeTemperatureRange(this Pawn p)
		{
			FloatRange result = p.ComfortableTemperatureRange();
			result.min -= 10f;
			result.max += 10f;
			return result;
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x0006468C File Offset: 0x0006288C
		public static bool SafeTemperatureAtCell(this Pawn p, IntVec3 cell, Map map)
		{
			return p.SafeTemperatureRange().Includes(GenTemperature.GetTemperatureForCell(cell, map));
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x000646B0 File Offset: 0x000628B0
		public static bool ComfortableTemperatureAtCell(this Pawn p, IntVec3 cell, Map map)
		{
			return p.ComfortableTemperatureRange().Includes(GenTemperature.GetTemperatureForCell(cell, map));
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x000646D4 File Offset: 0x000628D4
		public static FloatRange SafeTemperatureRange(ThingDef raceDef, List<ThingStuffPair> apparel = null)
		{
			FloatRange result = GenTemperature.ComfortableTemperatureRange(raceDef, apparel);
			result.min -= 10f;
			result.max += 10f;
			return result;
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x0006470C File Offset: 0x0006290C
		public static float GetTemperatureForCell(IntVec3 c, Map map)
		{
			float result;
			GenTemperature.TryGetTemperatureForCell(c, map, out result);
			return result;
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x00064724 File Offset: 0x00062924
		public static bool TryGetTemperatureForCell(IntVec3 c, Map map, out float tempResult)
		{
			if (map == null)
			{
				Log.Error("Got temperature for null map.");
				tempResult = 21f;
				return true;
			}
			if (!c.InBounds(map))
			{
				tempResult = 21f;
				return false;
			}
			if (GenTemperature.TryGetDirectAirTemperatureForCell(c, map, out tempResult))
			{
				return true;
			}
			List<Thing> list = map.thingGrid.ThingsListAtFast(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.passability == Traversability.Impassable)
				{
					return GenTemperature.TryGetAirTemperatureAroundThing(list[i], out tempResult);
				}
			}
			return false;
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x000647A8 File Offset: 0x000629A8
		public static bool TryGetDirectAirTemperatureForCell(IntVec3 c, Map map, out float temperature)
		{
			if (!c.InBounds(map))
			{
				temperature = 21f;
				return false;
			}
			Room room = c.GetRoom(map);
			if (room == null)
			{
				temperature = 21f;
				return false;
			}
			temperature = room.Temperature;
			return true;
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x000647E4 File Offset: 0x000629E4
		public static bool TryGetAirTemperatureAroundThing(Thing t, out float temperature)
		{
			float num = 0f;
			int num2 = 0;
			List<IntVec3> list = GenAdjFast.AdjacentCells8Way(t);
			for (int i = 0; i < list.Count; i++)
			{
				float num3;
				if (list[i].InBounds(t.Map) && GenTemperature.TryGetDirectAirTemperatureForCell(list[i], t.Map, out num3))
				{
					num += num3;
					num2++;
				}
			}
			if (num2 > 0)
			{
				temperature = num / (float)num2;
				return true;
			}
			temperature = 21f;
			return false;
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x00064860 File Offset: 0x00062A60
		public static float OffsetFromSunCycle(int absTick, int tile)
		{
			float num = GenDate.DayPercent((long)absTick, Find.WorldGrid.LongLatOf(tile).x);
			return Mathf.Cos(6.2831855f * (num + 0.32f)) * 7f;
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x000648A0 File Offset: 0x00062AA0
		public static float OffsetFromSeasonCycle(int absTick, int tile)
		{
			float num = (float)(absTick / 60000 % 60) / 60f;
			return Mathf.Cos(6.2831855f * (num - Season.Winter.GetMiddleTwelfth(0f).GetBeginningYearPct())) * -GenTemperature.SeasonalShiftAmplitudeAt(tile);
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x000648E4 File Offset: 0x00062AE4
		public static float GetTemperatureFromSeasonAtTile(int absTick, int tile)
		{
			if (absTick == 0)
			{
				absTick = 1;
			}
			return Find.WorldGrid[tile].temperature + GenTemperature.OffsetFromSeasonCycle(absTick, tile);
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x00064904 File Offset: 0x00062B04
		public static float GetTemperatureAtTile(int tile)
		{
			Map map = Current.Game.FindMap(tile);
			if (map != null)
			{
				return map.mapTemperature.OutdoorTemp;
			}
			return GenTemperature.GetTemperatureFromSeasonAtTile(GenTicks.TicksAbs, tile);
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x00064938 File Offset: 0x00062B38
		public static float SeasonalShiftAmplitudeAt(int tile)
		{
			if (Find.WorldGrid.LongLatOf(tile).y >= 0f)
			{
				return TemperatureTuning.SeasonalTempVariationCurve.Evaluate(Find.WorldGrid.DistanceFromEquatorNormalized(tile));
			}
			return -TemperatureTuning.SeasonalTempVariationCurve.Evaluate(Find.WorldGrid.DistanceFromEquatorNormalized(tile));
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x00064988 File Offset: 0x00062B88
		public static List<Twelfth> TwelfthsInAverageTemperatureRange(int tile, float minTemp, float maxTemp)
		{
			List<Twelfth> twelfths = new List<Twelfth>();
			for (int i = 0; i < 12; i++)
			{
				float num = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, (Twelfth)i);
				if (num >= minTemp && num <= maxTemp)
				{
					twelfths.Add((Twelfth)i);
				}
			}
			if (twelfths.Count <= 1 || twelfths.Count == 12)
			{
				return twelfths;
			}
			if (twelfths.Contains(Twelfth.Twelfth) && twelfths.Contains(Twelfth.First))
			{
				int num2 = (int)twelfths.First((Twelfth m) => !twelfths.Contains((Twelfth)(m - Twelfth.Second)));
				List<Twelfth> list = new List<Twelfth>();
				int num3 = num2;
				while (num3 < 12 && twelfths.Contains((Twelfth)num3))
				{
					list.Add((Twelfth)num3);
					num3++;
				}
				int num4 = 0;
				while (num4 < 12 && twelfths.Contains((Twelfth)num4))
				{
					list.Add((Twelfth)num4);
					num4++;
				}
			}
			return twelfths;
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x00064A88 File Offset: 0x00062C88
		public static Twelfth EarliestTwelfthInAverageTemperatureRange(int tile, float minTemp, float maxTemp)
		{
			int i = 0;
			while (i < 12)
			{
				float num = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, (Twelfth)i);
				if (num >= minTemp && num <= maxTemp)
				{
					if (i != 0)
					{
						return (Twelfth)i;
					}
					Twelfth twelfth = (Twelfth)i;
					for (int j = 0; j < 12; j++)
					{
						float num2 = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, twelfth.PreviousTwelfth());
						if (num2 < minTemp || num2 > maxTemp)
						{
							return twelfth;
						}
						twelfth = twelfth.PreviousTwelfth();
					}
					return (Twelfth)i;
				}
				else
				{
					i++;
				}
			}
			return Twelfth.Undefined;
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x00064AF0 File Offset: 0x00062CF0
		public static bool PushHeat(IntVec3 c, Map map, float energy)
		{
			if (map == null)
			{
				Log.Error("Added heat to null map.");
				return false;
			}
			Room room = c.GetRoom(map);
			if (room != null)
			{
				return room.PushHeat(energy);
			}
			GenTemperature.neighRooms.Clear();
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec = c + GenAdj.AdjacentCells[i];
				if (intVec.InBounds(map))
				{
					room = intVec.GetRoom(map);
					if (room != null)
					{
						GenTemperature.neighRooms.Add(room);
					}
				}
			}
			float energy2 = energy / (float)GenTemperature.neighRooms.Count;
			for (int j = 0; j < GenTemperature.neighRooms.Count; j++)
			{
				GenTemperature.neighRooms[j].PushHeat(energy2);
			}
			bool result = GenTemperature.neighRooms.Count > 0;
			GenTemperature.neighRooms.Clear();
			return result;
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x00064BB8 File Offset: 0x00062DB8
		public static void PushHeat(Thing t, float energy)
		{
			if (t.GetRoom(RegionType.Set_All) != null)
			{
				GenTemperature.PushHeat(t.Position, t.Map, energy);
				return;
			}
			IntVec3 c;
			if (GenAdj.TryFindRandomAdjacentCell8WayWithRoom(t, out c))
			{
				GenTemperature.PushHeat(c, t.Map, energy);
			}
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x00064BFC File Offset: 0x00062DFC
		public static float ControlTemperatureTempChange(IntVec3 cell, Map map, float energyLimit, float targetTemperature)
		{
			Room room = cell.GetRoom(map);
			if (room == null || room.UsesOutdoorTemperature)
			{
				return 0f;
			}
			float b = energyLimit / (float)room.CellCount;
			float a = targetTemperature - room.Temperature;
			float num;
			if (energyLimit > 0f)
			{
				num = Mathf.Min(a, b);
				num = Mathf.Max(num, 0f);
			}
			else
			{
				num = Mathf.Max(a, b);
				num = Mathf.Min(num, 0f);
			}
			return num;
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x00064C70 File Offset: 0x00062E70
		public static void EqualizeTemperaturesThroughBuilding(Building b, float rate, bool twoWay)
		{
			int num = 0;
			float num2 = 0f;
			if (twoWay)
			{
				for (int i = 0; i < 2; i++)
				{
					IntVec3 intVec = (i == 0) ? (b.Position + b.Rotation.FacingCell) : (b.Position - b.Rotation.FacingCell);
					if (intVec.InBounds(b.Map))
					{
						Room room = intVec.GetRoom(b.Map);
						if (room != null)
						{
							num2 += room.Temperature;
							GenTemperature.beqRooms[num] = room;
							num++;
						}
					}
				}
			}
			else
			{
				for (int j = 0; j < 4; j++)
				{
					IntVec3 intVec2 = b.Position + GenAdj.CardinalDirections[j];
					if (intVec2.InBounds(b.Map))
					{
						Room room2 = intVec2.GetRoom(b.Map);
						if (room2 != null)
						{
							num2 += room2.Temperature;
							GenTemperature.beqRooms[num] = room2;
							num++;
						}
					}
				}
			}
			if (num == 0)
			{
				return;
			}
			float num3 = num2 / (float)num;
			Room room3 = b.GetRoom(RegionType.Set_All);
			if (room3 != null)
			{
				room3.Temperature = num3;
			}
			if (num == 1)
			{
				return;
			}
			float num4 = 1f;
			for (int k = 0; k < num; k++)
			{
				if (!GenTemperature.beqRooms[k].UsesOutdoorTemperature)
				{
					float temperature = GenTemperature.beqRooms[k].Temperature;
					float num5 = (num3 - temperature) * rate;
					float num6 = num5 / (float)GenTemperature.beqRooms[k].CellCount;
					float num7 = GenTemperature.beqRooms[k].Temperature + num6;
					if (num5 > 0f && num7 > num3)
					{
						num7 = num3;
					}
					else if (num5 < 0f && num7 < num3)
					{
						num7 = num3;
					}
					float num8 = Mathf.Abs((num7 - temperature) * (float)GenTemperature.beqRooms[k].CellCount / num5);
					if (num8 < num4)
					{
						num4 = num8;
					}
				}
			}
			for (int l = 0; l < num; l++)
			{
				if (!GenTemperature.beqRooms[l].UsesOutdoorTemperature)
				{
					float temperature2 = GenTemperature.beqRooms[l].Temperature;
					float num9 = (num3 - temperature2) * rate * num4 / (float)GenTemperature.beqRooms[l].CellCount;
					GenTemperature.beqRooms[l].Temperature += num9;
				}
			}
			for (int m = 0; m < GenTemperature.beqRooms.Length; m++)
			{
				GenTemperature.beqRooms[m] = null;
			}
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x00064EC9 File Offset: 0x000630C9
		public static float RotRateAtTemperature(float temperature)
		{
			if (temperature < 0f)
			{
				return 0f;
			}
			if (temperature >= 10f)
			{
				return 1f;
			}
			return (temperature - 0f) / 10f;
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x00064EF4 File Offset: 0x000630F4
		public static bool FactionOwnsPassableRoomInTemperatureRange(Faction faction, FloatRange tempRange, Map map)
		{
			if (faction == Faction.OfPlayer)
			{
				List<Room> allRooms = map.regionGrid.allRooms;
				for (int i = 0; i < allRooms.Count; i++)
				{
					Room room = allRooms[i];
					if (room.AnyPassable && !room.Fogged && tempRange.Includes(room.Temperature))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x00064F54 File Offset: 0x00063154
		public static string GetAverageTemperatureLabel(int tile)
		{
			return Find.WorldGrid[tile].temperature.ToStringTemperature("F1") + " " + string.Format("({0} {1} {2})", GenTemperature.MinTemperatureAtTile(tile).ToStringTemperature("F0"), "RangeTo".Translate(), GenTemperature.MaxTemperatureAtTile(tile).ToStringTemperature("F0"));
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x00064FBE File Offset: 0x000631BE
		public static float CelsiusTo(float temp, TemperatureDisplayMode oldMode)
		{
			switch (oldMode)
			{
			case TemperatureDisplayMode.Celsius:
				return temp;
			case TemperatureDisplayMode.Fahrenheit:
				return temp * 1.8f + 32f;
			case TemperatureDisplayMode.Kelvin:
				return temp + 273.15f;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x00064FF1 File Offset: 0x000631F1
		public static float CelsiusToOffset(float temp, TemperatureDisplayMode oldMode)
		{
			switch (oldMode)
			{
			case TemperatureDisplayMode.Celsius:
				return temp;
			case TemperatureDisplayMode.Fahrenheit:
				return temp * 1.8f;
			case TemperatureDisplayMode.Kelvin:
				return temp;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x00065018 File Offset: 0x00063218
		public static float ConvertTemperatureOffset(float temp, TemperatureDisplayMode oldMode, TemperatureDisplayMode newMode)
		{
			switch (oldMode)
			{
			case TemperatureDisplayMode.Fahrenheit:
				temp /= 1.8f;
				break;
			}
			switch (newMode)
			{
			case TemperatureDisplayMode.Fahrenheit:
				temp *= 1.8f;
				break;
			}
			return temp;
		}

		// Token: 0x04000EC8 RID: 3784
		public static readonly Color ColorSpotHot = new Color(1f, 0f, 0f, 0.6f);

		// Token: 0x04000EC9 RID: 3785
		public static readonly Color ColorSpotCold = new Color(0f, 0f, 1f, 0.6f);

		// Token: 0x04000ECA RID: 3786
		public static readonly Color ColorRoomHot = new Color(1f, 0f, 0f, 0.3f);

		// Token: 0x04000ECB RID: 3787
		public static readonly Color ColorRoomCold = new Color(0f, 0f, 1f, 0.3f);

		// Token: 0x04000ECC RID: 3788
		private static List<Room> neighRooms = new List<Room>();

		// Token: 0x04000ECD RID: 3789
		private static Room[] beqRooms = new Room[4];
	}
}
