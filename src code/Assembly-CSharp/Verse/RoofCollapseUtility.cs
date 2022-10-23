using System;

namespace Verse
{
	// Token: 0x02000253 RID: 595
	public static class RoofCollapseUtility
	{
		// Token: 0x06001105 RID: 4357 RVA: 0x000636AC File Offset: 0x000618AC
		public static bool WithinRangeOfRoofHolder(IntVec3 c, Map map, bool assumeNonNoRoofCellsAreRoofed = false)
		{
			bool connected = false;
			map.floodFiller.FloodFill(c, (IntVec3 x) => (x.Roofed(map) || x == c || (assumeNonNoRoofCellsAreRoofed && !map.areaManager.NoRoof[x])) && x.InHorDistOf(c, 6.9f), delegate(IntVec3 x)
			{
				for (int i = 0; i < 5; i++)
				{
					IntVec3 c2 = x + GenAdj.CardinalDirectionsAndInside[i];
					if (c2.InBounds(map) && c2.InHorDistOf(c, 6.9f))
					{
						Building edifice = c2.GetEdifice(map);
						if (edifice != null && edifice.def.holdsRoof)
						{
							connected = true;
							return true;
						}
					}
				}
				return false;
			}, int.MaxValue, false, null);
			return connected;
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x00063718 File Offset: 0x00061918
		public static bool ConnectedToRoofHolder(IntVec3 c, Map map, bool assumeRoofAtRoot)
		{
			bool connected = false;
			map.floodFiller.FloodFill(c, (IntVec3 x) => (x.Roofed(map) || (x == c & assumeRoofAtRoot)) && !connected, delegate(IntVec3 x)
			{
				for (int i = 0; i < 5; i++)
				{
					IntVec3 c2 = x + GenAdj.CardinalDirectionsAndInside[i];
					if (c2.InBounds(map))
					{
						Building edifice = c2.GetEdifice(map);
						if (edifice != null && edifice.def.holdsRoof)
						{
							connected = true;
							return;
						}
					}
				}
			}, int.MaxValue, false, null);
			return connected;
		}

		// Token: 0x04000EBA RID: 3770
		public const float RoofMaxSupportDistance = 6.9f;

		// Token: 0x04000EBB RID: 3771
		public static readonly int RoofSupportRadialCellsCount = GenRadial.NumCellsInRadius(6.9f);
	}
}
