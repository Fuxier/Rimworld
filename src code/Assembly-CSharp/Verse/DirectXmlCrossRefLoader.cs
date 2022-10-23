using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000398 RID: 920
	public static class DirectXmlCrossRefLoader
	{
		// Token: 0x06001A5C RID: 6748 RVA: 0x0009EFF0 File Offset: 0x0009D1F0
		public static bool MistypedMayRequire(string mayRequireMod)
		{
			if (!Application.isEditor)
			{
				return false;
			}
			if (mayRequireMod.NullOrEmpty())
			{
				return false;
			}
			if (mayRequireMod.Contains(','))
			{
				string[] array = mayRequireMod.Split(new char[]
				{
					','
				});
				for (int i = 0; i < array.Length; i++)
				{
					if (!DirectXmlCrossRefLoader.<MistypedMayRequire>g__ExpansionFound|0_0(array[i]))
					{
						return true;
					}
				}
			}
			else if (!DirectXmlCrossRefLoader.<MistypedMayRequire>g__ExpansionFound|0_0(mayRequireMod))
			{
				return true;
			}
			return false;
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06001A5D RID: 6749 RVA: 0x0009F053 File Offset: 0x0009D253
		public static bool LoadingInProgress
		{
			get
			{
				return DirectXmlCrossRefLoader.wantedRefs.Count > 0;
			}
		}

		// Token: 0x06001A5E RID: 6750 RVA: 0x0009F064 File Offset: 0x0009D264
		public static void RegisterObjectWantsCrossRef(object wanter, FieldInfo fi, string targetDefName, string mayRequireMod = null, string mayRequireAnyMod = null, Type assumeFieldType = null)
		{
			DeepProfiler.Start("RegisterObjectWantsCrossRef (object, FieldInfo, string)");
			try
			{
				DirectXmlCrossRefLoader.WantedRefForObject item = new DirectXmlCrossRefLoader.WantedRefForObject(wanter, fi, targetDefName, mayRequireMod, mayRequireAnyMod, assumeFieldType);
				DirectXmlCrossRefLoader.wantedRefs.Add(item);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06001A5F RID: 6751 RVA: 0x0009F0AC File Offset: 0x0009D2AC
		public static void RegisterObjectWantsCrossRef(object wanter, string fieldName, string targetDefName, string mayRequireMod = null, string mayRequireAnyMod = null, Type overrideFieldType = null)
		{
			DeepProfiler.Start("RegisterObjectWantsCrossRef (object,string,string)");
			try
			{
				DirectXmlCrossRefLoader.WantedRefForObject item = new DirectXmlCrossRefLoader.WantedRefForObject(wanter, wanter.GetType().GetField(fieldName), targetDefName, mayRequireMod, mayRequireAnyMod, overrideFieldType);
				DirectXmlCrossRefLoader.wantedRefs.Add(item);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x0009F100 File Offset: 0x0009D300
		public static void RegisterObjectWantsCrossRef(object wanter, string fieldName, XmlNode parentNode, string mayRequireMod = null, string mayRequireAnyMod = null, Type overrideFieldType = null)
		{
			DeepProfiler.Start("RegisterObjectWantsCrossRef (object,string,XmlNode)");
			try
			{
				string text = mayRequireMod;
				if (mayRequireMod == null)
				{
					XmlAttributeCollection attributes = parentNode.Attributes;
					if (attributes == null)
					{
						text = null;
					}
					else
					{
						XmlAttribute xmlAttribute = attributes["MayRequire"];
						text = ((xmlAttribute != null) ? xmlAttribute.Value.ToLower() : null);
					}
				}
				string mayRequireMod2 = text;
				string text2 = mayRequireAnyMod;
				if (mayRequireAnyMod == null)
				{
					XmlAttributeCollection attributes2 = parentNode.Attributes;
					if (attributes2 == null)
					{
						text2 = null;
					}
					else
					{
						XmlAttribute xmlAttribute2 = attributes2["MayRequireAnyOf"];
						text2 = ((xmlAttribute2 != null) ? xmlAttribute2.Value.ToLower() : null);
					}
				}
				string mayRequireAnyMod2 = text2;
				DirectXmlCrossRefLoader.WantedRefForObject item = new DirectXmlCrossRefLoader.WantedRefForObject(wanter, wanter.GetType().GetField(fieldName), parentNode.Name, mayRequireMod2, mayRequireAnyMod2, overrideFieldType);
				DirectXmlCrossRefLoader.wantedRefs.Add(item);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06001A61 RID: 6753 RVA: 0x0009F1B4 File Offset: 0x0009D3B4
		public static void RegisterListWantsCrossRef<T>(List<T> wanterList, string targetDefName, object debugWanterInfo = null, string mayRequireMod = null, string mayRequireAnyMod = null)
		{
			DeepProfiler.Start("RegisterListWantsCrossRef");
			try
			{
				DirectXmlCrossRefLoader.WantedRef wantedRef;
				DirectXmlCrossRefLoader.WantedRefForList<T> wantedRefForList;
				if (!DirectXmlCrossRefLoader.wantedListDictRefs.TryGetValue(wanterList, out wantedRef))
				{
					wantedRefForList = new DirectXmlCrossRefLoader.WantedRefForList<T>(wanterList, debugWanterInfo);
					DirectXmlCrossRefLoader.wantedListDictRefs.Add(wanterList, wantedRefForList);
					DirectXmlCrossRefLoader.wantedRefs.Add(wantedRefForList);
				}
				else
				{
					wantedRefForList = (DirectXmlCrossRefLoader.WantedRefForList<T>)wantedRef;
				}
				wantedRefForList.AddWantedListEntry(targetDefName, mayRequireMod, mayRequireAnyMod);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06001A62 RID: 6754 RVA: 0x0009F228 File Offset: 0x0009D428
		public static void RegisterDictionaryWantsCrossRef<K, V>(Dictionary<K, V> wanterDict, XmlNode entryNode, object debugWanterInfo = null)
		{
			DeepProfiler.Start("RegisterDictionaryWantsCrossRef");
			try
			{
				DirectXmlCrossRefLoader.WantedRef wantedRef;
				DirectXmlCrossRefLoader.WantedRefForDictionary<K, V> wantedRefForDictionary;
				if (!DirectXmlCrossRefLoader.wantedListDictRefs.TryGetValue(wanterDict, out wantedRef))
				{
					wantedRefForDictionary = new DirectXmlCrossRefLoader.WantedRefForDictionary<K, V>(wanterDict, debugWanterInfo);
					DirectXmlCrossRefLoader.wantedRefs.Add(wantedRefForDictionary);
					DirectXmlCrossRefLoader.wantedListDictRefs.Add(wanterDict, wantedRefForDictionary);
				}
				else
				{
					wantedRefForDictionary = (DirectXmlCrossRefLoader.WantedRefForDictionary<K, V>)wantedRef;
				}
				wantedRefForDictionary.AddWantedDictEntry(entryNode);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06001A63 RID: 6755 RVA: 0x0009F298 File Offset: 0x0009D498
		public static T TryResolveDef<T>(string defName, FailMode failReportMode, object debugWanterInfo = null)
		{
			DeepProfiler.Start("TryResolveDef");
			T result;
			try
			{
				T t = (T)((object)GenDefDatabase.GetDefSilentFail(typeof(T), defName, true));
				if (t != null)
				{
					result = t;
				}
				else
				{
					if (failReportMode == FailMode.LogErrors)
					{
						string text = string.Concat(new object[]
						{
							"Could not resolve cross-reference to ",
							typeof(T),
							" named ",
							defName.ToStringSafe<string>()
						});
						if (debugWanterInfo != null)
						{
							text = text + " (wanter=" + debugWanterInfo.ToStringSafe<object>() + ")";
						}
						Log.Error(text);
					}
					result = default(T);
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			return result;
		}

		// Token: 0x06001A64 RID: 6756 RVA: 0x0009F34C File Offset: 0x0009D54C
		public static void Clear()
		{
			DeepProfiler.Start("Clear");
			try
			{
				DirectXmlCrossRefLoader.wantedRefs.Clear();
				DirectXmlCrossRefLoader.wantedListDictRefs.Clear();
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06001A65 RID: 6757 RVA: 0x0009F390 File Offset: 0x0009D590
		public static void ResolveAllWantedCrossReferences(FailMode failReportMode)
		{
			DeepProfiler.Start("ResolveAllWantedCrossReferences");
			try
			{
				HashSet<DirectXmlCrossRefLoader.WantedRef> resolvedRefs = new HashSet<DirectXmlCrossRefLoader.WantedRef>();
				object resolvedRefsLock = new object();
				DeepProfiler.enabled = false;
				GenThreading.ParallelForEach<DirectXmlCrossRefLoader.WantedRef>(DirectXmlCrossRefLoader.wantedRefs, delegate(DirectXmlCrossRefLoader.WantedRef wantedRef)
				{
					if (wantedRef.TryResolve(failReportMode))
					{
						object resolvedRefsLock = resolvedRefsLock;
						lock (resolvedRefsLock)
						{
							resolvedRefs.Add(wantedRef);
						}
					}
				}, -1);
				foreach (DirectXmlCrossRefLoader.WantedRef wantedRef2 in resolvedRefs)
				{
					wantedRef2.Apply();
				}
				DirectXmlCrossRefLoader.wantedRefs.RemoveAll((DirectXmlCrossRefLoader.WantedRef x) => resolvedRefs.Contains(x));
				DeepProfiler.enabled = true;
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06001A67 RID: 6759 RVA: 0x0009F484 File Offset: 0x0009D684
		[CompilerGenerated]
		internal static bool <MistypedMayRequire>g__ExpansionFound|0_0(string modID)
		{
			for (int i = 0; i < ModContentPack.ProductPackageIDs.Length; i++)
			{
				if (modID.EqualsIgnoreCase(ModContentPack.ProductPackageIDs[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04001340 RID: 4928
		private static List<DirectXmlCrossRefLoader.WantedRef> wantedRefs = new List<DirectXmlCrossRefLoader.WantedRef>();

		// Token: 0x04001341 RID: 4929
		private static Dictionary<object, DirectXmlCrossRefLoader.WantedRef> wantedListDictRefs = new Dictionary<object, DirectXmlCrossRefLoader.WantedRef>();

		// Token: 0x02001E6B RID: 7787
		private abstract class WantedRef
		{
			// Token: 0x0600B8FA RID: 47354
			public abstract bool TryResolve(FailMode failReportMode);

			// Token: 0x0600B8FB RID: 47355 RVA: 0x000034B7 File Offset: 0x000016B7
			public virtual void Apply()
			{
			}

			// Token: 0x040077DA RID: 30682
			public object wanter;
		}

		// Token: 0x02001E6C RID: 7788
		private class WantedRefForObject : DirectXmlCrossRefLoader.WantedRef
		{
			// Token: 0x17001ECE RID: 7886
			// (get) Token: 0x0600B8FD RID: 47357 RVA: 0x0041E04B File Offset: 0x0041C24B
			private bool BadCrossRefAllowed
			{
				get
				{
					return (!this.mayRequireMod.NullOrEmpty() && !ModsConfig.AreAllActive(this.mayRequireMod)) || (!this.mayRequireAnyMod.NullOrEmpty<string>() && !ModsConfig.IsAnyActiveOrEmpty(this.mayRequireAnyMod, false));
				}
			}

			// Token: 0x0600B8FE RID: 47358 RVA: 0x0041E088 File Offset: 0x0041C288
			public WantedRefForObject(object wanter, FieldInfo fi, string targetDefName, string mayRequireMod = null, string mayRequireAnyMod = null, Type overrideFieldType = null)
			{
				this.wanter = wanter;
				this.fi = fi;
				this.defName = targetDefName;
				this.mayRequireMod = mayRequireMod;
				this.overrideFieldType = overrideFieldType;
				this.mayRequireAnyMod = ((mayRequireAnyMod != null) ? mayRequireAnyMod.ToLower().Split(new char[]
				{
					','
				}) : null);
			}

			// Token: 0x0600B8FF RID: 47359 RVA: 0x0041E0E4 File Offset: 0x0041C2E4
			public override bool TryResolve(FailMode failReportMode)
			{
				if (this.fi == null)
				{
					Log.Error("Trying to resolve null field for def named " + this.defName.ToStringSafe<string>());
					return false;
				}
				Type type = this.overrideFieldType ?? this.fi.FieldType;
				this.resolvedDef = GenDefDatabase.GetDefSilentFail(type, this.defName, true);
				if (DirectXmlCrossRefLoader.MistypedMayRequire(this.mayRequireMod))
				{
					Log.Error("Faulty MayRequire at def " + this.defName.ToStringSafe<string>() + ": " + this.mayRequireMod);
				}
				if (!this.mayRequireAnyMod.NullOrEmpty<string>())
				{
					foreach (string str in this.mayRequireAnyMod)
					{
						if (DirectXmlCrossRefLoader.MistypedMayRequire(str))
						{
							Log.Error("Faulty MayRequire at def " + this.defName.ToStringSafe<string>() + ": " + str);
						}
					}
				}
				if (this.resolvedDef == null)
				{
					if (failReportMode == FailMode.LogErrors && !this.BadCrossRefAllowed)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not resolve cross-reference: No ",
							type,
							" named ",
							this.defName.ToStringSafe<string>(),
							" found to give to ",
							this.wanter.GetType(),
							" ",
							this.wanter.ToStringSafe<object>()
						}));
					}
					return false;
				}
				SoundDef soundDef = this.resolvedDef as SoundDef;
				if (soundDef != null && soundDef.isUndefined)
				{
					Log.Warning(string.Concat(new object[]
					{
						"Could not resolve cross-reference: No ",
						type,
						" named ",
						this.defName.ToStringSafe<string>(),
						" found to give to ",
						this.wanter.GetType(),
						" ",
						this.wanter.ToStringSafe<object>(),
						" (using undefined sound instead)"
					}));
				}
				this.fi.SetValue(this.wanter, this.resolvedDef);
				return true;
			}

			// Token: 0x040077DB RID: 30683
			public FieldInfo fi;

			// Token: 0x040077DC RID: 30684
			public string defName;

			// Token: 0x040077DD RID: 30685
			public Def resolvedDef;

			// Token: 0x040077DE RID: 30686
			public string mayRequireMod;

			// Token: 0x040077DF RID: 30687
			public string[] mayRequireAnyMod;

			// Token: 0x040077E0 RID: 30688
			public Type overrideFieldType;
		}

		// Token: 0x02001E6D RID: 7789
		private class WantedRefForList<T> : DirectXmlCrossRefLoader.WantedRef
		{
			// Token: 0x0600B900 RID: 47360 RVA: 0x0041E2D1 File Offset: 0x0041C4D1
			public WantedRefForList(object wanter, object debugWanterInfo)
			{
				this.wanter = wanter;
				this.debugWanterInfo = debugWanterInfo;
			}

			// Token: 0x0600B901 RID: 47361 RVA: 0x0041E2F4 File Offset: 0x0041C4F4
			public void AddWantedListEntry(string newTargetDefName, string mayRequireMod = null, string mayRequireAnyMod = null)
			{
				if (!mayRequireMod.NullOrEmpty() && this.mayRequireMods == null)
				{
					this.mayRequireMods = new List<string>();
					for (int i = 0; i < this.defNames.Count; i++)
					{
						this.mayRequireMods.Add(null);
					}
				}
				if (!mayRequireAnyMod.NullOrEmpty() && this.mayRequireModsAny == null)
				{
					this.mayRequireModsAny = new Dictionary<string, List<string>>();
					for (int j = 0; j < this.defNames.Count; j++)
					{
						this.mayRequireModsAny.Add(this.defNames[j], new List<string>());
					}
				}
				this.defNames.Add(newTargetDefName);
				if (this.mayRequireMods != null)
				{
					this.mayRequireMods.Add(mayRequireMod);
				}
				if (this.mayRequireModsAny != null)
				{
					foreach (string text in mayRequireAnyMod.ToLower().Split(new char[]
					{
						','
					}))
					{
						List<string> list;
						if (this.mayRequireModsAny.TryGetValue(newTargetDefName, out list))
						{
							list.Add(text.Trim());
						}
						else
						{
							this.mayRequireModsAny.Add(newTargetDefName, new List<string>
							{
								text.Trim()
							});
						}
					}
				}
			}

			// Token: 0x0600B902 RID: 47362 RVA: 0x0041E41C File Offset: 0x0041C61C
			public override bool TryResolve(FailMode failReportMode)
			{
				bool flag = false;
				for (int i = 0; i < this.defNames.Count; i++)
				{
					bool flag2 = this.mayRequireMods != null && i < this.mayRequireMods.Count && !this.mayRequireMods[i].NullOrEmpty() && !ModsConfig.AreAllActive(this.mayRequireMods[i]);
					List<string> mods;
					if (this.mayRequireModsAny != null && this.mayRequireModsAny.TryGetValue(this.defNames[i], out mods) && !ModsConfig.IsAnyActiveOrEmpty(mods, false))
					{
						flag2 = true;
					}
					if (this.mayRequireMods != null && i < this.mayRequireMods.Count && DirectXmlCrossRefLoader.MistypedMayRequire(this.mayRequireMods[i]))
					{
						Log.Error("Faulty MayRequire: " + this.mayRequireMods[i]);
					}
					T t = DirectXmlCrossRefLoader.TryResolveDef<T>(this.defNames[i], flag2 ? FailMode.Silent : failReportMode, this.debugWanterInfo);
					if (t != null)
					{
						((List<T>)this.wanter).Add(t);
						this.defNames.RemoveAt(i);
						if (this.mayRequireMods != null && i < this.mayRequireMods.Count)
						{
							this.mayRequireMods.RemoveAt(i);
						}
						i--;
					}
					else
					{
						flag = true;
					}
				}
				return !flag;
			}

			// Token: 0x040077E1 RID: 30689
			private List<string> defNames = new List<string>();

			// Token: 0x040077E2 RID: 30690
			private List<string> mayRequireMods;

			// Token: 0x040077E3 RID: 30691
			private Dictionary<string, List<string>> mayRequireModsAny;

			// Token: 0x040077E4 RID: 30692
			private object debugWanterInfo;
		}

		// Token: 0x02001E6E RID: 7790
		private class WantedRefForDictionary<K, V> : DirectXmlCrossRefLoader.WantedRef
		{
			// Token: 0x0600B903 RID: 47363 RVA: 0x0041E56D File Offset: 0x0041C76D
			public WantedRefForDictionary(object wanter, object debugWanterInfo)
			{
				this.wanter = wanter;
				this.debugWanterInfo = debugWanterInfo;
			}

			// Token: 0x0600B904 RID: 47364 RVA: 0x0041E599 File Offset: 0x0041C799
			public void AddWantedDictEntry(XmlNode entryNode)
			{
				this.wantedDictRefs.Add(entryNode);
			}

			// Token: 0x0600B905 RID: 47365 RVA: 0x0041E5A8 File Offset: 0x0041C7A8
			public override bool TryResolve(FailMode failReportMode)
			{
				failReportMode = FailMode.LogErrors;
				bool flag = GenTypes.IsDef(typeof(K));
				bool flag2 = GenTypes.IsDef(typeof(V));
				foreach (XmlNode xmlNode in this.wantedDictRefs)
				{
					XmlNode xmlNode2 = xmlNode["key"];
					XmlNode xmlNode3 = xmlNode["value"];
					string text = (xmlNode2 != null) ? xmlNode2.InnerText : null;
					string text2 = (xmlNode3 != null) ? xmlNode3.InnerText : null;
					object first;
					object second;
					if (text == null || text2 == null)
					{
						if (failReportMode == FailMode.LogErrors)
						{
							string text3 = "Missing 'key' and/or 'value'.";
							if (this.debugWanterInfo != null)
							{
								text3 = text3 + " (wanter=" + this.debugWanterInfo.ToStringSafe<object>() + ")";
							}
							Log.Error(text3);
						}
						first = default(K);
						second = default(V);
					}
					else
					{
						if (flag)
						{
							first = DirectXmlCrossRefLoader.TryResolveDef<K>(text, failReportMode, this.debugWanterInfo);
						}
						else
						{
							first = xmlNode2;
						}
						if (flag2)
						{
							second = DirectXmlCrossRefLoader.TryResolveDef<V>(text2, failReportMode, this.debugWanterInfo);
						}
						else
						{
							second = xmlNode3;
						}
					}
					this.makingData.Add(new Pair<object, object>(first, second));
				}
				return true;
			}

			// Token: 0x0600B906 RID: 47366 RVA: 0x0041E714 File Offset: 0x0041C914
			public override void Apply()
			{
				Dictionary<K, V> dictionary = (Dictionary<K, V>)this.wanter;
				dictionary.Clear();
				foreach (Pair<object, object> pair in this.makingData)
				{
					try
					{
						object obj = pair.First;
						object obj2 = pair.Second;
						if (obj is XmlNode)
						{
							obj = DirectXmlToObject.ObjectFromXml<K>(obj as XmlNode, true);
						}
						if (obj2 is XmlNode)
						{
							obj2 = DirectXmlToObject.ObjectFromXml<V>(obj2 as XmlNode, true);
						}
						dictionary.Add((K)((object)obj), (V)((object)obj2));
					}
					catch
					{
						Log.Error(string.Concat(new object[]
						{
							"Failed to load key/value pair: ",
							pair.First,
							", ",
							pair.Second
						}));
					}
				}
			}

			// Token: 0x040077E5 RID: 30693
			private List<XmlNode> wantedDictRefs = new List<XmlNode>();

			// Token: 0x040077E6 RID: 30694
			private object debugWanterInfo;

			// Token: 0x040077E7 RID: 30695
			private List<Pair<object, object>> makingData = new List<Pair<object, object>>();
		}
	}
}
