using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200024E RID: 590
	public sealed class RoomTempTracker
	{
		// Token: 0x17000347 RID: 839
		// (get) Token: 0x060010DF RID: 4319 RVA: 0x0006232D File Offset: 0x0006052D
		private Map Map
		{
			get
			{
				return this.room.Map;
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x060010E0 RID: 4320 RVA: 0x0006233A File Offset: 0x0006053A
		private float ThinRoofCoverage
		{
			get
			{
				return 1f - (this.thickRoofCoverage + this.noRoofCoverage);
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x060010E1 RID: 4321 RVA: 0x0006234F File Offset: 0x0006054F
		public List<IntVec3> EqualizeCellsForReading
		{
			get
			{
				return this.equalizeCells;
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x060010E2 RID: 4322 RVA: 0x00062357 File Offset: 0x00060557
		// (set) Token: 0x060010E3 RID: 4323 RVA: 0x0006235F File Offset: 0x0006055F
		public float Temperature
		{
			get
			{
				return this.temperatureInt;
			}
			set
			{
				this.temperatureInt = Mathf.Clamp(value, -273.15f, 1000f);
			}
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x00062377 File Offset: 0x00060577
		public RoomTempTracker(Room room, Map map)
		{
			this.room = room;
			this.Temperature = map.mapTemperature.OutdoorTemp;
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x000623A2 File Offset: 0x000605A2
		public void RoofChanged()
		{
			this.RegenerateEqualizationData();
		}

		// Token: 0x060010E6 RID: 4326 RVA: 0x000623AA File Offset: 0x000605AA
		public void RoomChanged()
		{
			if (this.Map != null)
			{
				this.Map.autoBuildRoofAreaSetter.ResolveQueuedGenerateRoofs();
			}
			this.RegenerateEqualizationData();
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x000623CC File Offset: 0x000605CC
		private void RegenerateEqualizationData()
		{
			this.thickRoofCoverage = 0f;
			this.noRoofCoverage = 0f;
			this.equalizeCells.Clear();
			if (this.room.DistrictCount == 0)
			{
				return;
			}
			Map map = this.Map;
			if (!this.room.UsesOutdoorTemperature)
			{
				int num = 0;
				foreach (IntVec3 c in this.room.Cells)
				{
					RoofDef roof = c.GetRoof(map);
					if (roof == null)
					{
						this.noRoofCoverage += 1f;
					}
					else if (roof.isThickRoof)
					{
						this.thickRoofCoverage += 1f;
					}
					num++;
				}
				this.thickRoofCoverage /= (float)num;
				this.noRoofCoverage /= (float)num;
				foreach (IntVec3 a in this.room.Cells)
				{
					int i = 0;
					while (i < 4)
					{
						IntVec3 intVec = a + GenAdj.CardinalDirections[i];
						IntVec3 intVec2 = a + GenAdj.CardinalDirections[i] * 2;
						if (!intVec.InBounds(map))
						{
							goto IL_1D8;
						}
						Region region = intVec.GetRegion(map, RegionType.Set_Passable);
						if (region == null)
						{
							goto IL_1D8;
						}
						if (region.type == RegionType.Portal)
						{
							bool flag = false;
							for (int j = 0; j < region.links.Count; j++)
							{
								Region regionA = region.links[j].RegionA;
								Region regionB = region.links[j].RegionB;
								if (regionA.Room != this.room && !regionA.IsDoorway)
								{
									flag = true;
									break;
								}
								if (regionB.Room != this.room && !regionB.IsDoorway)
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								goto IL_1D8;
							}
						}
						IL_23C:
						i++;
						continue;
						IL_1D8:
						if (!intVec2.InBounds(map) || intVec2.GetRoom(map) == this.room)
						{
							goto IL_23C;
						}
						bool flag2 = false;
						for (int k = 0; k < 4; k++)
						{
							if ((intVec2 + GenAdj.CardinalDirections[k]).GetRoom(map) == this.room)
							{
								flag2 = true;
								break;
							}
						}
						if (!flag2)
						{
							this.equalizeCells.Add(intVec2);
							goto IL_23C;
						}
						goto IL_23C;
					}
				}
				this.equalizeCells.Shuffle<IntVec3>();
			}
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x0006267C File Offset: 0x0006087C
		public void EqualizeTemperature()
		{
			if (this.room.UsesOutdoorTemperature)
			{
				this.Temperature = this.Map.mapTemperature.OutdoorTemp;
				return;
			}
			if (this.room.IsDoorway)
			{
				bool flag = true;
				IntVec3 anyCell = this.room.Districts[0].Regions[0].AnyCell;
				for (int i = 0; i < 4; i++)
				{
					IntVec3 intVec = anyCell + GenAdj.CardinalDirections[i];
					if (intVec.InBounds(this.Map))
					{
						Room room = intVec.GetRoom(this.Map);
						if (room != null && !room.IsDoorway)
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					this.room.Temperature += this.WallEqualizationTempChangePerInterval();
				}
				return;
			}
			float num = this.ThinRoofEqualizationTempChangePerInterval();
			float num2 = this.NoRoofEqualizationTempChangePerInterval();
			float num3 = this.WallEqualizationTempChangePerInterval();
			float num4 = this.DeepEqualizationTempChangePerInterval();
			this.Temperature += num + num2 + num3 + num4;
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x00062788 File Offset: 0x00060988
		private float WallEqualizationTempChangePerInterval()
		{
			if (this.equalizeCells.Count == 0)
			{
				return 0f;
			}
			float num = 0f;
			int num2 = Mathf.CeilToInt((float)this.equalizeCells.Count * 0.2f);
			for (int i = 0; i < num2; i++)
			{
				this.cycleIndex++;
				int index = this.cycleIndex % this.equalizeCells.Count;
				float num3;
				if (GenTemperature.TryGetDirectAirTemperatureForCell(this.equalizeCells[index], this.Map, out num3))
				{
					num += num3 - this.Temperature;
				}
				else
				{
					num += Mathf.Lerp(this.Temperature, this.Map.mapTemperature.OutdoorTemp, 0.5f) - this.Temperature;
				}
			}
			return num / (float)num2 * (float)this.equalizeCells.Count * 120f * 0.00017f / (float)this.room.CellCount;
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x00062870 File Offset: 0x00060A70
		private float TempDiffFromOutdoorsAdjusted()
		{
			float num = this.Map.mapTemperature.OutdoorTemp - this.temperatureInt;
			if (Mathf.Abs(num) < 100f)
			{
				return num;
			}
			return Mathf.Sign(num) * 100f + 5f * (num - Mathf.Sign(num) * 100f);
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x000628C5 File Offset: 0x00060AC5
		private float ThinRoofEqualizationTempChangePerInterval()
		{
			if (this.ThinRoofCoverage < 0.001f)
			{
				return 0f;
			}
			return this.TempDiffFromOutdoorsAdjusted() * this.ThinRoofCoverage * 5E-05f * 120f;
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x000628F3 File Offset: 0x00060AF3
		private float NoRoofEqualizationTempChangePerInterval()
		{
			if (this.noRoofCoverage < 0.001f)
			{
				return 0f;
			}
			return this.TempDiffFromOutdoorsAdjusted() * this.noRoofCoverage * 0.0007f * 120f;
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x00062924 File Offset: 0x00060B24
		private float DeepEqualizationTempChangePerInterval()
		{
			if (this.thickRoofCoverage < 0.001f)
			{
				return 0f;
			}
			float num = 15f - this.temperatureInt;
			if (num > 0f)
			{
				return 0f;
			}
			return num * this.thickRoofCoverage * 5E-05f * 120f;
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x00062974 File Offset: 0x00060B74
		public void DebugDraw()
		{
			foreach (IntVec3 c in this.equalizeCells)
			{
				CellRenderer.RenderCell(c, 0.5f);
			}
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x000629CC File Offset: 0x00060BCC
		internal string DebugString()
		{
			if (this.room.UsesOutdoorTemperature)
			{
				return "uses outdoor temperature";
			}
			if (Time.frameCount > RoomTempTracker.debugGetFrame + 120)
			{
				RoomTempTracker.debugWallEq = 0f;
				for (int i = 0; i < 40; i++)
				{
					RoomTempTracker.debugWallEq += this.WallEqualizationTempChangePerInterval();
				}
				RoomTempTracker.debugWallEq /= 40f;
				RoomTempTracker.debugGetFrame = Time.frameCount;
			}
			return string.Concat(new object[]
			{
				"  thick roof coverage: ",
				this.thickRoofCoverage.ToStringPercent("F0"),
				"\n  thin roof coverage: ",
				this.ThinRoofCoverage.ToStringPercent("F0"),
				"\n  no roof coverage: ",
				this.noRoofCoverage.ToStringPercent("F0"),
				"\n\n  wall equalization: ",
				RoomTempTracker.debugWallEq.ToStringTemperatureOffset("F3"),
				"\n  thin roof equalization: ",
				this.ThinRoofEqualizationTempChangePerInterval().ToStringTemperatureOffset("F3"),
				"\n  no roof equalization: ",
				this.NoRoofEqualizationTempChangePerInterval().ToStringTemperatureOffset("F3"),
				"\n  deep equalization: ",
				this.DeepEqualizationTempChangePerInterval().ToStringTemperatureOffset("F3"),
				"\n\n  temp diff from outdoors, adjusted: ",
				this.TempDiffFromOutdoorsAdjusted().ToStringTemperatureOffset("F3"),
				"\n  tempChange e=20 targ= 200C: ",
				GenTemperature.ControlTemperatureTempChange(this.room.Cells.First<IntVec3>(), this.room.Map, 20f, 200f),
				"\n  tempChange e=20 targ=-200C: ",
				GenTemperature.ControlTemperatureTempChange(this.room.Cells.First<IntVec3>(), this.room.Map, 20f, -200f),
				"\n  equalize interval ticks: ",
				120,
				"\n  equalize cells count:",
				this.equalizeCells.Count
			});
		}

		// Token: 0x04000EA1 RID: 3745
		private Room room;

		// Token: 0x04000EA2 RID: 3746
		private float temperatureInt;

		// Token: 0x04000EA3 RID: 3747
		private List<IntVec3> equalizeCells = new List<IntVec3>();

		// Token: 0x04000EA4 RID: 3748
		private float noRoofCoverage;

		// Token: 0x04000EA5 RID: 3749
		private float thickRoofCoverage;

		// Token: 0x04000EA6 RID: 3750
		public const float FractionWallEqualizeCells = 0.2f;

		// Token: 0x04000EA7 RID: 3751
		public const float WallEqualizeFactor = 0.00017f;

		// Token: 0x04000EA8 RID: 3752
		public const float EqualizationPowerOfFilledCells = 0.5f;

		// Token: 0x04000EA9 RID: 3753
		private int cycleIndex;

		// Token: 0x04000EAA RID: 3754
		private const float ThinRoofEqualizeRate = 5E-05f;

		// Token: 0x04000EAB RID: 3755
		private const float NoRoofEqualizeRate = 0.0007f;

		// Token: 0x04000EAC RID: 3756
		private const float DeepEqualizeFractionPerTick = 5E-05f;

		// Token: 0x04000EAD RID: 3757
		private static int debugGetFrame = -999;

		// Token: 0x04000EAE RID: 3758
		private static float debugWallEq;
	}
}
