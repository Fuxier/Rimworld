using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using RimWorld;
using RimWorld.QuestGen;

namespace Verse
{
	// Token: 0x0200003B RID: 59
	public static class GenTypes
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060002FB RID: 763 RVA: 0x0000FD54 File Offset: 0x0000DF54
		private static IEnumerable<Assembly> AllActiveAssemblies
		{
			get
			{
				yield return Assembly.GetExecutingAssembly();
				foreach (ModContentPack mod in LoadedModManager.RunningMods)
				{
					int num;
					for (int i = 0; i < mod.assemblies.loadedAssemblies.Count; i = num + 1)
					{
						yield return mod.assemblies.loadedAssemblies[i];
						num = i;
					}
					mod = null;
				}
				IEnumerator<ModContentPack> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060002FC RID: 764 RVA: 0x0000FD60 File Offset: 0x0000DF60
		public static List<Type> AllTypes
		{
			get
			{
				if (GenTypes.allTypesCached == null)
				{
					GenTypes.allTypesCached = new List<Type>();
					foreach (Assembly assembly in GenTypes.AllActiveAssemblies)
					{
						Type[] array = null;
						try
						{
							array = assembly.GetTypes();
						}
						catch (ReflectionTypeLoadException ex)
						{
							Log.Error(string.Concat(new object[]
							{
								"Exception getting types in assembly ",
								assembly.ToString(),
								". Some types may not work correctly. Exception: ",
								ex
							}));
							try
							{
								Type[] types = ex.Types;
								if (types != null)
								{
									array = (from x in types
									where x != null && x.TypeInitializer != null
									select x).ToArray<Type>();
								}
							}
							catch (Exception arg)
							{
								Log.Error("Could not resolve assembly types fallback. Exception: " + arg);
							}
						}
						if (array != null)
						{
							GenTypes.allTypesCached.AddRange(array);
						}
					}
				}
				return GenTypes.allTypesCached;
			}
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000FE74 File Offset: 0x0000E074
		public static List<Type> AllTypesWithAttribute<TAttr>() where TAttr : Attribute
		{
			List<Type> result;
			if (GenTypes.cachedTypesWithAttribute.TryGetValue(typeof(TAttr), out result))
			{
				return result;
			}
			List<Type> list = (from x in GenTypes.AllTypes.AsParallel<Type>()
			where x.HasAttribute<TAttr>()
			select x).ToList<Type>();
			GenTypes.cachedTypesWithAttribute.Add(typeof(TAttr), list);
			return list;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000FEE8 File Offset: 0x0000E0E8
		public static List<Type> AllSubclasses(this Type baseType)
		{
			if (!GenTypes.cachedSubclasses.ContainsKey(baseType))
			{
				GenTypes.cachedSubclasses.Add(baseType, (from x in GenTypes.AllTypes.AsParallel<Type>()
				where x.IsSubclassOf(baseType)
				select x).ToList<Type>());
			}
			return GenTypes.cachedSubclasses[baseType];
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000FF54 File Offset: 0x0000E154
		public static List<Type> AllSubclassesNonAbstract(this Type baseType)
		{
			if (!GenTypes.cachedSubclassesNonAbstract.ContainsKey(baseType))
			{
				GenTypes.cachedSubclassesNonAbstract.Add(baseType, (from x in GenTypes.AllTypes.AsParallel<Type>()
				where x.IsSubclassOf(baseType) && !x.IsAbstract
				select x).ToList<Type>());
			}
			return GenTypes.cachedSubclassesNonAbstract[baseType];
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000FFC0 File Offset: 0x0000E1C0
		public static void ClearCache()
		{
			GenTypes.cachedSubclasses.Clear();
			GenTypes.cachedSubclassesNonAbstract.Clear();
			GenTypes.cachedTypesWithAttribute.Clear();
			GenTypes.allTypesCached = null;
			AlertsReadout.allAlertTypesCached = null;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000FFEC File Offset: 0x0000E1EC
		public static IEnumerable<Type> AllLeafSubclasses(this Type baseType)
		{
			return from type in baseType.AllSubclasses()
			where !type.AllSubclasses().Any<Type>()
			select type;
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00010018 File Offset: 0x0000E218
		public static IEnumerable<Type> InstantiableDescendantsAndSelf(this Type baseType)
		{
			if (!baseType.IsAbstract)
			{
				yield return baseType;
			}
			foreach (Type type in baseType.AllSubclasses())
			{
				if (!type.IsAbstract)
				{
					yield return type;
				}
			}
			List<Type>.Enumerator enumerator = default(List<Type>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00010028 File Offset: 0x0000E228
		public static Type GetTypeInAnyAssembly(string typeName, string namespaceIfAmbiguous = null)
		{
			GenTypes.TypeCacheKey key = new GenTypes.TypeCacheKey(typeName, namespaceIfAmbiguous);
			Type type = null;
			if (!GenTypes.typeCache.TryGetValue(key, out type))
			{
				type = GenTypes.GetTypeInAnyAssemblyInt(typeName, namespaceIfAmbiguous);
				GenTypes.typeCache.Add(key, type);
			}
			return type;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00010064 File Offset: 0x0000E264
		public static bool HasFlagsAttribute(Type type)
		{
			bool result;
			if (GenTypes.hasFlagsAttributeCache.TryGetValue(type, out result))
			{
				return result;
			}
			bool flag = Attribute.IsDefined(type, typeof(FlagsAttribute));
			GenTypes.hasFlagsAttributeCache.Add(type, flag);
			return flag;
		}

		// Token: 0x06000305 RID: 773 RVA: 0x000100A0 File Offset: 0x0000E2A0
		public static bool IsList(Type type)
		{
			bool result;
			if (GenTypes.isListCache.TryGetValue(type, out result))
			{
				return result;
			}
			bool flag = type.HasGenericDefinition(typeof(List<>));
			GenTypes.isListCache.Add(type, flag);
			return flag;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x000100DC File Offset: 0x0000E2DC
		public static bool IsDictionary(Type type)
		{
			bool result;
			if (GenTypes.isDictionaryCache.TryGetValue(type, out result))
			{
				return result;
			}
			bool flag = type.HasGenericDefinition(typeof(Dictionary<, >));
			GenTypes.isDictionaryCache.Add(type, flag);
			return flag;
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00010118 File Offset: 0x0000E318
		public static bool IsSlateRef(Type type)
		{
			bool result;
			if (GenTypes.isSlateRefCache.TryGetValue(type, out result))
			{
				return result;
			}
			bool flag = typeof(ISlateRef).IsAssignableFrom(type);
			GenTypes.isSlateRefCache.Add(type, flag);
			return flag;
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00010154 File Offset: 0x0000E354
		public static bool IsListHashSetOrDictionary(Type type)
		{
			bool result;
			if (GenTypes.isListHashSetOrDictionaryCached.TryGetValue(type, out result))
			{
				return result;
			}
			bool flag = false;
			if (type.IsGenericType)
			{
				Type genericTypeDefinition = type.GetGenericTypeDefinition();
				if (genericTypeDefinition == typeof(List<>) || genericTypeDefinition == typeof(HashSet<>) || genericTypeDefinition == typeof(Dictionary<, >))
				{
					flag = true;
				}
			}
			GenTypes.isListHashSetOrDictionaryCached.Add(type, flag);
			return flag;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x000101C8 File Offset: 0x0000E3C8
		public static bool IsDef(Type type)
		{
			bool result;
			if (GenTypes.isDefCache.TryGetValue(type, out result))
			{
				return result;
			}
			bool flag = typeof(Def).IsAssignableFrom(type);
			GenTypes.isDefCache.Add(type, flag);
			return flag;
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00010204 File Offset: 0x0000E404
		public static bool IsDefThreaded(Type type)
		{
			object obj = GenTypes.isDefCacheLock;
			bool result;
			lock (obj)
			{
				result = GenTypes.IsDef(type);
			}
			return result;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00010248 File Offset: 0x0000E448
		private static Type GetTypeInAnyAssemblyInt(string typeName, string namespaceIfAmbiguous = null)
		{
			Type typeInAnyAssemblyRaw = GenTypes.GetTypeInAnyAssemblyRaw(typeName);
			if (typeInAnyAssemblyRaw != null)
			{
				return typeInAnyAssemblyRaw;
			}
			if (!namespaceIfAmbiguous.NullOrEmpty() && GenTypes.IgnoredNamespaceNames.Contains(namespaceIfAmbiguous))
			{
				typeInAnyAssemblyRaw = GenTypes.GetTypeInAnyAssemblyRaw(namespaceIfAmbiguous + "." + typeName);
				if (typeInAnyAssemblyRaw != null)
				{
					return typeInAnyAssemblyRaw;
				}
			}
			for (int i = 0; i < GenTypes.IgnoredNamespaceNames.Count; i++)
			{
				typeInAnyAssemblyRaw = GenTypes.GetTypeInAnyAssemblyRaw(GenTypes.IgnoredNamespaceNames[i] + "." + typeName);
				if (typeInAnyAssemblyRaw != null)
				{
					return typeInAnyAssemblyRaw;
				}
			}
			if (GenTypes.TryGetMixedAssemblyGenericType(typeName, out typeInAnyAssemblyRaw))
			{
				return typeInAnyAssemblyRaw;
			}
			return null;
		}

		// Token: 0x0600030C RID: 780 RVA: 0x000102E4 File Offset: 0x0000E4E4
		private static bool TryGetMixedAssemblyGenericType(string typeName, out Type type)
		{
			type = GenTypes.GetTypeInAnyAssemblyRaw(typeName);
			if (type == null && typeName.Contains("`"))
			{
				try
				{
					Match match = Regex.Match(typeName, "(?<MainType>.+`(?<ParamCount>[0-9]+))(?<Types>\\[.*\\])");
					if (match.Success)
					{
						int capacity = int.Parse(match.Groups["ParamCount"].Value);
						string value = match.Groups["Types"].Value;
						List<string> list = new List<string>(capacity);
						foreach (object obj in Regex.Matches(value, "\\[(?<Type>.*?)\\],?"))
						{
							Match match2 = (Match)obj;
							if (match2.Success)
							{
								list.Add(match2.Groups["Type"].Value.Trim());
							}
						}
						Type[] array = new Type[list.Count];
						for (int i = 0; i < list.Count; i++)
						{
							Type type2;
							if (!GenTypes.TryGetMixedAssemblyGenericType(list[i], out type2))
							{
								return false;
							}
							array[i] = type2;
						}
						Type type3;
						if (GenTypes.TryGetMixedAssemblyGenericType(match.Groups["MainType"].Value, out type3))
						{
							type = type3.MakeGenericType(array);
						}
					}
				}
				catch (Exception ex)
				{
					Log.ErrorOnce(string.Concat(new object[]
					{
						"Error in TryGetMixedAssemblyGenericType with typeName=",
						typeName,
						": ",
						ex
					}), typeName.GetHashCode());
				}
			}
			return type != null;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x000104AC File Offset: 0x0000E6AC
		private static Type GetTypeInAnyAssemblyRaw(string typeName)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(typeName);
			if (num <= 2299065237U)
			{
				if (num <= 1092586446U)
				{
					if (num <= 431052896U)
					{
						if (num != 296283782U)
						{
							if (num != 398550328U)
							{
								if (num == 431052896U)
								{
									if (typeName == "byte?")
									{
										return typeof(byte?);
									}
								}
							}
							else if (typeName == "string")
							{
								return typeof(string);
							}
						}
						else if (typeName == "char?")
						{
							return typeof(char?);
						}
					}
					else if (num != 513669818U)
					{
						if (num != 520654156U)
						{
							if (num == 1092586446U)
							{
								if (typeName == "float?")
								{
									return typeof(float?);
								}
							}
						}
						else if (typeName == "decimal")
						{
							return typeof(decimal);
						}
					}
					else if (typeName == "uint?")
					{
						return typeof(uint?);
					}
				}
				else if (num <= 1454009365U)
				{
					if (num != 1189328644U)
					{
						if (num != 1299622921U)
						{
							if (num == 1454009365U)
							{
								if (typeName == "sbyte?")
								{
									return typeof(sbyte?);
								}
							}
						}
						else if (typeName == "decimal?")
						{
							return typeof(decimal?);
						}
					}
					else if (typeName == "long?")
					{
						return typeof(long?);
					}
				}
				else if (num <= 1630192034U)
				{
					if (num != 1603400371U)
					{
						if (num == 1630192034U)
						{
							if (typeName == "ushort")
							{
								return typeof(ushort);
							}
						}
					}
					else if (typeName == "int?")
					{
						return typeof(int?);
					}
				}
				else if (num != 1683620383U)
				{
					if (num == 2299065237U)
					{
						if (typeName == "double?")
						{
							return typeof(double?);
						}
					}
				}
				else if (typeName == "byte")
				{
					return typeof(byte);
				}
			}
			else if (num <= 2823553821U)
			{
				if (num <= 2515107422U)
				{
					if (num != 2471414311U)
					{
						if (num != 2508976771U)
						{
							if (num == 2515107422U)
							{
								if (typeName == "int")
								{
									return typeof(int);
								}
							}
						}
						else if (typeName == "ulong?")
						{
							return typeof(ulong?);
						}
					}
					else if (typeName == "ushort?")
					{
						return typeof(ushort?);
					}
				}
				else if (num <= 2699759368U)
				{
					if (num != 2667225454U)
					{
						if (num == 2699759368U)
						{
							if (typeName == "double")
							{
								return typeof(double);
							}
						}
					}
					else if (typeName == "ulong")
					{
						return typeof(ulong);
					}
				}
				else if (num != 2797886853U)
				{
					if (num == 2823553821U)
					{
						if (typeName == "char")
						{
							return typeof(char);
						}
					}
				}
				else if (typeName == "float")
				{
					return typeof(float);
				}
			}
			else if (num <= 3286667814U)
			{
				if (num != 3122818005U)
				{
					if (num != 3270303571U)
					{
						if (num == 3286667814U)
						{
							if (typeName == "bool?")
							{
								return typeof(bool?);
							}
						}
					}
					else if (typeName == "long")
					{
						return typeof(long);
					}
				}
				else if (typeName == "short")
				{
					return typeof(short);
				}
			}
			else if (num <= 3415750305U)
			{
				if (num != 3365180733U)
				{
					if (num == 3415750305U)
					{
						if (typeName == "uint")
						{
							return typeof(uint);
						}
					}
				}
				else if (typeName == "bool")
				{
					return typeof(bool);
				}
			}
			else if (num != 3996115294U)
			{
				if (num == 4088464520U)
				{
					if (typeName == "sbyte")
					{
						return typeof(sbyte);
					}
				}
			}
			else if (typeName == "short?")
			{
				return typeof(short?);
			}
			foreach (Assembly assembly in GenTypes.AllActiveAssemblies)
			{
				Type type = assembly.GetType(typeName, false, true);
				if (type != null)
				{
					return type;
				}
			}
			Type type2 = Type.GetType(typeName, false, true);
			if (type2 != null)
			{
				return type2;
			}
			return null;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00010A38 File Offset: 0x0000EC38
		public static string GetTypeNameWithoutIgnoredNamespaces(Type type)
		{
			if (type.IsGenericType)
			{
				return type.ToString();
			}
			for (int i = 0; i < GenTypes.IgnoredNamespaceNames.Count; i++)
			{
				if (type.Namespace == GenTypes.IgnoredNamespaceNames[i])
				{
					return type.Name;
				}
			}
			return type.FullName;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00010A90 File Offset: 0x0000EC90
		public static bool IsCustomType(Type type)
		{
			string @namespace = type.Namespace;
			return @namespace == null || (!@namespace.StartsWith("System") && !@namespace.StartsWith("UnityEngine") && !@namespace.StartsWith("Steamworks"));
		}

		// Token: 0x040000AE RID: 174
		public static readonly List<string> IgnoredNamespaceNames = new List<string>
		{
			"RimWorld",
			"Verse",
			"Verse.AI",
			"Verse.Sound",
			"Verse.Grammar",
			"RimWorld.Planet",
			"RimWorld.BaseGen",
			"RimWorld.QuestGen",
			"RimWorld.SketchGen",
			"System"
		};

		// Token: 0x040000AF RID: 175
		private static List<Type> allTypesCached;

		// Token: 0x040000B0 RID: 176
		private static Dictionary<Type, List<Type>> cachedTypesWithAttribute = new Dictionary<Type, List<Type>>();

		// Token: 0x040000B1 RID: 177
		private static Dictionary<Type, List<Type>> cachedSubclasses = new Dictionary<Type, List<Type>>();

		// Token: 0x040000B2 RID: 178
		private static Dictionary<Type, List<Type>> cachedSubclassesNonAbstract = new Dictionary<Type, List<Type>>();

		// Token: 0x040000B3 RID: 179
		private static Dictionary<GenTypes.TypeCacheKey, Type> typeCache = new Dictionary<GenTypes.TypeCacheKey, Type>(EqualityComparer<GenTypes.TypeCacheKey>.Default);

		// Token: 0x040000B4 RID: 180
		private static Dictionary<Type, bool> hasFlagsAttributeCache = new Dictionary<Type, bool>();

		// Token: 0x040000B5 RID: 181
		private static Dictionary<Type, bool> isListCache = new Dictionary<Type, bool>();

		// Token: 0x040000B6 RID: 182
		private static Dictionary<Type, bool> isDictionaryCache = new Dictionary<Type, bool>();

		// Token: 0x040000B7 RID: 183
		private static Dictionary<Type, bool> isSlateRefCache = new Dictionary<Type, bool>();

		// Token: 0x040000B8 RID: 184
		private static Dictionary<Type, bool> isListHashSetOrDictionaryCached = new Dictionary<Type, bool>();

		// Token: 0x040000B9 RID: 185
		private static Dictionary<Type, bool> isDefCache = new Dictionary<Type, bool>();

		// Token: 0x040000BA RID: 186
		private static object isDefCacheLock = new object();

		// Token: 0x02001C74 RID: 7284
		private struct TypeCacheKey : IEquatable<GenTypes.TypeCacheKey>
		{
			// Token: 0x0600AF6C RID: 44908 RVA: 0x003FD5EB File Offset: 0x003FB7EB
			public override int GetHashCode()
			{
				if (this.namespaceIfAmbiguous == null)
				{
					return this.typeName.GetHashCode();
				}
				return (17 * 31 + this.typeName.GetHashCode()) * 31 + this.namespaceIfAmbiguous.GetHashCode();
			}

			// Token: 0x0600AF6D RID: 44909 RVA: 0x003FD621 File Offset: 0x003FB821
			public bool Equals(GenTypes.TypeCacheKey other)
			{
				return string.Equals(this.typeName, other.typeName) && string.Equals(this.namespaceIfAmbiguous, other.namespaceIfAmbiguous);
			}

			// Token: 0x0600AF6E RID: 44910 RVA: 0x003FD649 File Offset: 0x003FB849
			public override bool Equals(object obj)
			{
				return obj is GenTypes.TypeCacheKey && this.Equals((GenTypes.TypeCacheKey)obj);
			}

			// Token: 0x0600AF6F RID: 44911 RVA: 0x003FD661 File Offset: 0x003FB861
			public TypeCacheKey(string typeName, string namespaceIfAmbigous = null)
			{
				this.typeName = typeName;
				this.namespaceIfAmbiguous = namespaceIfAmbigous;
			}

			// Token: 0x0400703A RID: 28730
			public string typeName;

			// Token: 0x0400703B RID: 28731
			public string namespaceIfAmbiguous;
		}
	}
}
