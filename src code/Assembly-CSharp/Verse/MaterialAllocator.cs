using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200038B RID: 907
	internal static class MaterialAllocator
	{
		// Token: 0x06001A13 RID: 6675 RVA: 0x0009D5E0 File Offset: 0x0009B7E0
		public static Material Create(Material material)
		{
			Material material2 = new Material(material);
			MaterialAllocator.references[material2] = new MaterialAllocator.MaterialInfo
			{
				stackTrace = (MaterialAllocator.trackMaterialStackTrace ? Environment.StackTrace : "(unavailable)")
			};
			MaterialAllocator.TryReport();
			return material2;
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x0009D628 File Offset: 0x0009B828
		public static Material Create(Shader shader)
		{
			Material material = new Material(shader);
			MaterialAllocator.references[material] = new MaterialAllocator.MaterialInfo
			{
				stackTrace = (MaterialAllocator.trackMaterialStackTrace ? Environment.StackTrace : "(unavailable)")
			};
			MaterialAllocator.TryReport();
			return material;
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x0009D670 File Offset: 0x0009B870
		public static void Destroy(Material material)
		{
			if (!MaterialAllocator.references.ContainsKey(material))
			{
				Log.Error(string.Format("Destroying material {0}, but that material was not created through the MaterialTracker", material));
			}
			MaterialAllocator.references.Remove(material);
			UnityEngine.Object.Destroy(material);
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x0009D6A4 File Offset: 0x0009B8A4
		public static void TryReport()
		{
			if (MaterialAllocator.MaterialWarningThreshold() > MaterialAllocator.nextWarningThreshold)
			{
				MaterialAllocator.nextWarningThreshold = MaterialAllocator.MaterialWarningThreshold();
			}
			if (MaterialAllocator.references.Count > MaterialAllocator.nextWarningThreshold)
			{
				Log.Error(string.Format("Material allocator has allocated {0} materials; this may be a sign of a material leak", MaterialAllocator.references.Count));
				if (Prefs.DevMode)
				{
					MaterialAllocator.MaterialReport();
				}
				MaterialAllocator.nextWarningThreshold *= 2;
			}
		}

		// Token: 0x06001A17 RID: 6679 RVA: 0x0009D70E File Offset: 0x0009B90E
		public static int MaterialWarningThreshold()
		{
			return int.MaxValue;
		}

		// Token: 0x06001A18 RID: 6680 RVA: 0x0009D718 File Offset: 0x0009B918
		[DebugOutput("System", false)]
		public static void MaterialReport()
		{
			foreach (string text in (from kvp in MaterialAllocator.references
			group kvp by kvp.Value.stackTrace into g
			orderby g.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>() descending
			select string.Format("{0}: {1}", g.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>(), g.FirstOrDefault<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>().Value.stackTrace)).Take(20))
			{
				Log.Error(text);
			}
		}

		// Token: 0x06001A19 RID: 6681 RVA: 0x0009D7D4 File Offset: 0x0009B9D4
		[DebugOutput("System", false)]
		public static void MaterialSnapshot()
		{
			MaterialAllocator.snapshot = new Dictionary<string, int>();
			foreach (IGrouping<string, KeyValuePair<Material, MaterialAllocator.MaterialInfo>> grouping in from kvp in MaterialAllocator.references
			group kvp by kvp.Value.stackTrace)
			{
				MaterialAllocator.snapshot[grouping.Key] = grouping.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>();
			}
		}

		// Token: 0x06001A1A RID: 6682 RVA: 0x0009D860 File Offset: 0x0009BA60
		[DebugOutput("System", false)]
		public static void MaterialDelta()
		{
			IEnumerable<string> source = (from v in MaterialAllocator.references.Values
			select v.stackTrace).Concat(MaterialAllocator.snapshot.Keys).Distinct<string>();
			Dictionary<string, int> currentSnapshot = new Dictionary<string, int>();
			foreach (IGrouping<string, KeyValuePair<Material, MaterialAllocator.MaterialInfo>> grouping in from kvp in MaterialAllocator.references
			group kvp by kvp.Value.stackTrace)
			{
				currentSnapshot[grouping.Key] = grouping.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>();
			}
			foreach (string text in (from k in source
			select new KeyValuePair<string, int>(k, currentSnapshot.TryGetValue(k, 0) - MaterialAllocator.snapshot.TryGetValue(k, 0)) into kvp
			orderby kvp.Value descending
			select kvp into g
			select string.Format("{0}: {1}", g.Value, g.Key)).Take(20))
			{
				Log.Error(text);
			}
		}

		// Token: 0x0400130C RID: 4876
		private static bool trackMaterialStackTrace = false;

		// Token: 0x0400130D RID: 4877
		private static Dictionary<Material, MaterialAllocator.MaterialInfo> references = new Dictionary<Material, MaterialAllocator.MaterialInfo>();

		// Token: 0x0400130E RID: 4878
		public static int nextWarningThreshold;

		// Token: 0x0400130F RID: 4879
		private static Dictionary<string, int> snapshot = new Dictionary<string, int>();

		// Token: 0x02001E65 RID: 7781
		private struct MaterialInfo
		{
			// Token: 0x040077CA RID: 30666
			public string stackTrace;
		}
	}
}
