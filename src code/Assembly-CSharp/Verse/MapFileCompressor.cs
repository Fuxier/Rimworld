using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001B8 RID: 440
	public class MapFileCompressor : IExposable
	{
		// Token: 0x06000C5E RID: 3166 RVA: 0x00045578 File Offset: 0x00043778
		public MapFileCompressor(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x00045587 File Offset: 0x00043787
		public void ExposeData()
		{
			DataExposeUtility.ByteArray(ref this.compressedData, "compressedThingMap");
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x00045599 File Offset: 0x00043799
		public void BuildCompressedString()
		{
			this.compressibilityDecider = new CompressibilityDecider(this.map);
			this.compressibilityDecider.DetermineReferences();
			this.compressedData = MapSerializeUtility.SerializeUshort(this.map, new Func<IntVec3, ushort>(this.HashValueForSquare));
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x000455D4 File Offset: 0x000437D4
		private ushort HashValueForSquare(IntVec3 curSq)
		{
			ushort num = 0;
			foreach (Thing thing in this.map.thingGrid.ThingsAt(curSq))
			{
				if (thing.IsSaveCompressible())
				{
					if (num != 0)
					{
						Log.Error(string.Concat(new object[]
						{
							"Found two compressible things in ",
							curSq,
							". The last was ",
							thing
						}));
					}
					num = thing.def.shortHash;
				}
			}
			return num;
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x0004566C File Offset: 0x0004386C
		public IEnumerable<Thing> ThingsToSpawnAfterLoad()
		{
			Dictionary<ushort, ThingDef> thingDefsByShortHash = new Dictionary<ushort, ThingDef>();
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDefsByShortHash.ContainsKey(thingDef.shortHash))
				{
					Log.Error(string.Concat(new object[]
					{
						"Hash collision between ",
						thingDef,
						" and  ",
						thingDefsByShortHash[thingDef.shortHash],
						": both have short hash ",
						thingDef.shortHash
					}));
				}
				else
				{
					thingDefsByShortHash.Add(thingDef.shortHash, thingDef);
				}
			}
			int major = ScribeMetaHeaderUtility.loadedGameVersionMajor;
			int minor = ScribeMetaHeaderUtility.loadedGameVersionMinor;
			List<Thing> loadables = new List<Thing>();
			MapSerializeUtility.LoadUshort(this.compressedData, this.map, delegate(IntVec3 c, ushort val)
			{
				if (val == 0)
				{
					return;
				}
				ThingDef thingDef2 = BackCompatibility.BackCompatibleThingDefWithShortHash_Force(val, major, minor);
				if (thingDef2 == null)
				{
					try
					{
						thingDef2 = thingDefsByShortHash[val];
					}
					catch (KeyNotFoundException)
					{
						ThingDef thingDef3 = BackCompatibility.BackCompatibleThingDefWithShortHash(val);
						if (thingDef3 != null)
						{
							thingDef2 = thingDef3;
							thingDefsByShortHash.Add(val, thingDef3);
						}
						else
						{
							Log.Error("Map compressor decompression error: No thingDef with short hash " + val + ". Adding as null to dictionary.");
							thingDefsByShortHash.Add(val, null);
						}
					}
				}
				if (thingDef2 != null)
				{
					try
					{
						if (!thingDef2.saveCompressible)
						{
							Log.Warning("Tried loading non-compressible thing as compressed thing: " + thingDef2.defName);
						}
						else
						{
							Thing thing = ThingMaker.MakeThing(thingDef2, null);
							thing.SetPositionDirect(c);
							loadables.Add(thing);
						}
					}
					catch (Exception arg)
					{
						Log.Error("Could not instantiate compressed thing: " + arg);
					}
				}
			});
			return loadables;
		}

		// Token: 0x04000B5B RID: 2907
		private Map map;

		// Token: 0x04000B5C RID: 2908
		private byte[] compressedData;

		// Token: 0x04000B5D RID: 2909
		public CompressibilityDecider compressibilityDecider;
	}
}
