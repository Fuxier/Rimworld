using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200025D RID: 605
	public class MapTemperature : ICellBoolGiver
	{
		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06001151 RID: 4433 RVA: 0x000650F2 File Offset: 0x000632F2
		public float OutdoorTemp
		{
			get
			{
				return Find.World.tileTemperatures.GetOutdoorTemp(this.map.Tile);
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06001152 RID: 4434 RVA: 0x0006510E File Offset: 0x0006330E
		public float SeasonalTemp
		{
			get
			{
				return Find.World.tileTemperatures.GetSeasonalTemp(this.map.Tile);
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06001153 RID: 4435 RVA: 0x0006512C File Offset: 0x0006332C
		public CellBoolDrawer Drawer
		{
			get
			{
				if (this.drawerInt == null)
				{
					this.drawerInt = new CellBoolDrawer(this, this.map.Size.x, this.map.Size.z, 3600, 0.33f);
				}
				return this.drawerInt;
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06001154 RID: 4436 RVA: 0x00020495 File Offset: 0x0001E695
		public Color Color
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x0006517D File Offset: 0x0006337D
		public MapTemperature(Map map)
		{
			this.map = map;
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x0006518C File Offset: 0x0006338C
		public void MapTemperatureTick()
		{
			if (Find.TickManager.TicksGame % 120 == 7 || DebugSettings.fastEcology)
			{
				List<Room> allRooms = this.map.regionGrid.allRooms;
				for (int i = 0; i < allRooms.Count; i++)
				{
					allRooms[i].TempTracker.EqualizeTemperature();
				}
			}
			if (Find.TickManager.TicksGame % 60 == 0)
			{
				this.Drawer.SetDirty();
			}
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x000651FD File Offset: 0x000633FD
		public void TemperatureUpdate()
		{
			if (Find.PlaySettings.showTemperatureOverlay)
			{
				this.Drawer.MarkForDraw();
			}
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x00065221 File Offset: 0x00063421
		public bool SeasonAcceptableFor(ThingDef animalRace)
		{
			return Find.World.tileTemperatures.SeasonAcceptableFor(this.map.Tile, animalRace);
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x0006523E File Offset: 0x0006343E
		public bool OutdoorTemperatureAcceptableFor(ThingDef animalRace)
		{
			return Find.World.tileTemperatures.OutdoorTemperatureAcceptableFor(this.map.Tile, animalRace);
		}

		// Token: 0x0600115A RID: 4442 RVA: 0x0006525B File Offset: 0x0006345B
		public bool SeasonAndOutdoorTemperatureAcceptableFor(ThingDef animalRace)
		{
			return Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(this.map.Tile, animalRace);
		}

		// Token: 0x0600115B RID: 4443 RVA: 0x00065278 File Offset: 0x00063478
		public bool LocalSeasonsAreMeaningful()
		{
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < 12; i++)
			{
				float num = Find.World.tileTemperatures.AverageTemperatureForTwelfth(this.map.Tile, (Twelfth)i);
				if (num > 0f)
				{
					flag2 = true;
				}
				if (num < 0f)
				{
					flag = true;
				}
			}
			return flag2 && flag;
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x000652C8 File Offset: 0x000634C8
		public bool GetCellBool(int index)
		{
			IntVec3 intVec = this.map.cellIndices.IndexToCell(index);
			return !intVec.Fogged(this.map) && intVec.GetRoom(this.map) != null;
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x00065308 File Offset: 0x00063508
		public Color GetCellExtraColor(int index)
		{
			float temperature = this.map.cellIndices.IndexToCell(index).GetTemperature(this.map);
			return this.GetColorForTemperature(temperature);
		}

		// Token: 0x0600115E RID: 4446 RVA: 0x0006533C File Offset: 0x0006353C
		private Color GetColorForTemperature(float temperature)
		{
			List<ValueTuple<float, Color>> temperatureColorMap = MapTemperature.TemperatureColorMap;
			if (temperature <= temperatureColorMap[0].Item1)
			{
				return temperatureColorMap[0].Item2;
			}
			if (temperature >= temperatureColorMap[temperatureColorMap.Count - 1].Item1)
			{
				return temperatureColorMap[temperatureColorMap.Count - 1].Item2;
			}
			int i = 1;
			while (i < temperatureColorMap.Count)
			{
				if (temperatureColorMap[i].Item1 > temperature)
				{
					if (i >= temperatureColorMap.Count - 1)
					{
						return temperatureColorMap[i].Item2;
					}
					int index = i - 1;
					int index2 = i;
					float item = temperatureColorMap[index].Item1;
					float item2 = temperatureColorMap[index2].Item1;
					float t = (temperature - item) / (item2 - item);
					return Color.Lerp(temperatureColorMap[index].Item2, temperatureColorMap[index2].Item2, t);
				}
				else
				{
					i++;
				}
			}
			Log.Error("Error when trying to determine correct color for temperature grid.");
			return Color.white;
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x0006542D File Offset: 0x0006362D
		public void Notify_ThingSpawned(Thing thing)
		{
			if (thing.def.AffectsRegions)
			{
				this.Drawer.SetDirty();
			}
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x00065448 File Offset: 0x00063648
		public void DebugLogTemps()
		{
			StringBuilder stringBuilder = new StringBuilder();
			float num = (Find.CurrentMap != null) ? Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile).y : 0f;
			stringBuilder.AppendLine("Latitude " + num);
			stringBuilder.AppendLine("-----Temperature for each hour this day------");
			stringBuilder.AppendLine("Hour    Temp    SunEffect");
			int num2 = Find.TickManager.TicksAbs - Find.TickManager.TicksAbs % 60000;
			for (int i = 0; i < 24; i++)
			{
				int absTick = num2 + i * 2500;
				stringBuilder.Append(i.ToString().PadRight(5));
				stringBuilder.Append(Find.World.tileTemperatures.OutdoorTemperatureAt(this.map.Tile, absTick).ToString("F2").PadRight(8));
				stringBuilder.Append(GenTemperature.OffsetFromSunCycle(absTick, this.map.Tile).ToString("F2"));
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("-----Temperature for each twelfth this year------");
			for (int j = 0; j < 12; j++)
			{
				Twelfth twelfth = (Twelfth)j;
				float num3 = Find.World.tileTemperatures.AverageTemperatureForTwelfth(this.map.Tile, twelfth);
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					twelfth.GetQuadrum(),
					"/",
					SeasonUtility.GetReportedSeason(twelfth.GetMiddleYearPct(), num),
					" - ",
					twelfth.ToString(),
					" ",
					num3.ToString("F2")
				}));
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("-----Temperature for each day this year------");
			stringBuilder.AppendLine("Tile avg: " + this.map.TileInfo.temperature + "°C");
			stringBuilder.AppendLine("Seasonal shift: " + GenTemperature.SeasonalShiftAmplitudeAt(this.map.Tile));
			stringBuilder.AppendLine("Equatorial distance: " + Find.WorldGrid.DistanceFromEquatorNormalized(this.map.Tile));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Day  Lo   Hi   OffsetFromSeason RandomDailyVariation");
			for (int k = 0; k < 60; k++)
			{
				int absTick2 = (int)((float)(k * 60000) + 15000f);
				int absTick3 = (int)((float)(k * 60000) + 45000f);
				stringBuilder.Append(k.ToString().PadRight(8));
				stringBuilder.Append(Find.World.tileTemperatures.OutdoorTemperatureAt(this.map.Tile, absTick2).ToString("F2").PadRight(11));
				stringBuilder.Append(Find.World.tileTemperatures.OutdoorTemperatureAt(this.map.Tile, absTick3).ToString("F2").PadRight(11));
				stringBuilder.Append(GenTemperature.OffsetFromSeasonCycle(absTick3, this.map.Tile).ToString("F2").PadRight(11));
				stringBuilder.Append(Find.World.tileTemperatures.OffsetFromDailyRandomVariation(this.map.Tile, absTick3).ToString("F2"));
				stringBuilder.AppendLine();
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x000657F4 File Offset: 0x000639F4
		public void DebugLogTemperatureOverlayColors()
		{
			int num = -50;
			int num2 = 150;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("-----Temperature overlay colors by temperature------");
			for (int i = num; i <= num2; i++)
			{
				stringBuilder.AppendLine(i + ": " + this.GetColorForTemperature((float)i).ToString());
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04000ECE RID: 3790
		private static readonly List<ValueTuple<float, Color>> TemperatureColorMap = new List<ValueTuple<float, Color>>
		{
			new ValueTuple<float, Color>(-25f, ColorLibrary.DarkBlue),
			new ValueTuple<float, Color>(0f, ColorLibrary.Blue),
			new ValueTuple<float, Color>(25f, ColorLibrary.Green),
			new ValueTuple<float, Color>(50f, ColorLibrary.Yellow),
			new ValueTuple<float, Color>(100f, ColorLibrary.Red)
		};

		// Token: 0x04000ECF RID: 3791
		private const int TemperatureOverlayUpdateInterval = 60;

		// Token: 0x04000ED0 RID: 3792
		private Map map;

		// Token: 0x04000ED1 RID: 3793
		private CellBoolDrawer drawerInt;
	}
}
