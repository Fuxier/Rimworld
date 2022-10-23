using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001C3 RID: 451
	public sealed class DynamicDrawManager
	{
		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000CA7 RID: 3239 RVA: 0x00046E82 File Offset: 0x00045082
		public HashSet<Thing> DrawThingsForReading
		{
			get
			{
				return this.drawThings;
			}
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x00046E8A File Offset: 0x0004508A
		public DynamicDrawManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x00046EA4 File Offset: 0x000450A4
		public void RegisterDrawable(Thing t)
		{
			if (t.def.drawerType != DrawerType.None)
			{
				if (this.drawingNow)
				{
					Log.Warning("Cannot register drawable " + t + " while drawing is in progress. Things shouldn't be spawned in Draw methods.");
				}
				this.drawThings.Add(t);
			}
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x00046EDD File Offset: 0x000450DD
		public void DeRegisterDrawable(Thing t)
		{
			if (t.def.drawerType != DrawerType.None)
			{
				if (this.drawingNow)
				{
					Log.Warning("Cannot deregister drawable " + t + " while drawing is in progress. Things shouldn't be despawned in Draw methods.");
				}
				this.drawThings.Remove(t);
			}
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x00046F18 File Offset: 0x00045118
		public void DrawDynamicThings()
		{
			if (!DebugViewSettings.drawThingsDynamic)
			{
				return;
			}
			this.drawingNow = true;
			try
			{
				bool[] fogGrid = this.map.fogGrid.fogGrid;
				CellRect cellRect = Find.CameraDriver.CurrentViewRect;
				cellRect.ClipInsideMap(this.map);
				cellRect = cellRect.ExpandedBy(1);
				CellIndices cellIndices = this.map.cellIndices;
				foreach (Thing thing in this.drawThings)
				{
					IntVec3 position = thing.Position;
					if ((cellRect.Contains(position) || thing.def.drawOffscreen) && (!fogGrid[cellIndices.CellToIndex(position)] || thing.def.seeThroughFog) && (thing.def.hideAtSnowDepth >= 1f || this.map.snowGrid.GetDepth(position) <= thing.def.hideAtSnowDepth))
					{
						try
						{
							thing.Draw();
						}
						catch (Exception ex)
						{
							Log.Error(string.Concat(new object[]
							{
								"Exception drawing ",
								thing,
								": ",
								ex.ToString()
							}));
						}
					}
				}
			}
			catch (Exception arg)
			{
				Log.Error("Exception drawing dynamic things: " + arg);
			}
			this.drawingNow = false;
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x000470C0 File Offset: 0x000452C0
		public void LogDynamicDrawThings()
		{
			Log.Message(DebugLogsUtility.ThingListToUniqueCountString(this.drawThings));
		}

		// Token: 0x04000B86 RID: 2950
		private Map map;

		// Token: 0x04000B87 RID: 2951
		private HashSet<Thing> drawThings = new HashSet<Thing>();

		// Token: 0x04000B88 RID: 2952
		private bool drawingNow;
	}
}
