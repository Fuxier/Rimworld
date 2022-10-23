using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Verse
{
	// Token: 0x020000A2 RID: 162
	public static class GenDefDatabase
	{
		// Token: 0x0600057B RID: 1403 RVA: 0x0001E7B4 File Offset: 0x0001C9B4
		public static Def GetDef(Type defType, string defName, bool errorOnFail = true)
		{
			object obj = GenDefDatabase.cachedGetNamedLock;
			Func<string, bool, Def> func;
			lock (obj)
			{
				if (!GenDefDatabase.cachedGetNamed.TryGetValue(defType, out func))
				{
					MethodInfo method = GenGeneric.MethodOnGenericType(typeof(DefDatabase<>), defType, "GetNamed");
					func = (Func<string, bool, Def>)Delegate.CreateDelegate(typeof(Func<string, bool, Def>), method);
					GenDefDatabase.cachedGetNamed.Add(defType, func);
				}
			}
			return func(defName, errorOnFail);
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0001E83C File Offset: 0x0001CA3C
		public static Def GetDefSilentFail(Type type, string targetDefName, bool specialCaseForSoundDefs = true)
		{
			if (specialCaseForSoundDefs && type == typeof(SoundDef))
			{
				return SoundDef.Named(targetDefName);
			}
			object obj = GenDefDatabase.cachedGetNamedSilentFailLock;
			Func<string, Def> func;
			lock (obj)
			{
				if (!GenDefDatabase.cachedGetNamedSilentFail.TryGetValue(type, out func))
				{
					MethodInfo method = GenGeneric.MethodOnGenericType(typeof(DefDatabase<>), type, "GetNamedSilentFail");
					func = (Func<string, Def>)Delegate.CreateDelegate(typeof(Func<string, Def>), method);
					GenDefDatabase.cachedGetNamedSilentFail.Add(type, func);
				}
			}
			return func(targetDefName);
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0001E8E0 File Offset: 0x0001CAE0
		public static IEnumerable<Def> GetAllDefsInDatabaseForDef(Type defType)
		{
			return ((IEnumerable)GenGeneric.GetStaticPropertyOnGenericType(typeof(DefDatabase<>), defType, "AllDefs")).Cast<Def>();
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x0001E901 File Offset: 0x0001CB01
		public static IEnumerable<Type> AllDefTypesWithDatabases()
		{
			foreach (Type type in typeof(Def).AllSubclasses())
			{
				if (!type.IsAbstract && !(type == typeof(Def)))
				{
					bool flag = false;
					Type baseType = type.BaseType;
					while (baseType != null && baseType != typeof(Def))
					{
						if (!baseType.IsAbstract)
						{
							flag = true;
							break;
						}
						baseType = baseType.BaseType;
					}
					if (!flag)
					{
						yield return type;
					}
				}
			}
			List<Type>.Enumerator enumerator = default(List<Type>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0001E90A File Offset: 0x0001CB0A
		public static IEnumerable<T> DefsToGoInDatabase<T>(ModContentPack mod)
		{
			return mod.AllDefs.OfType<T>();
		}

		// Token: 0x04000294 RID: 660
		private static Dictionary<Type, Func<string, bool, Def>> cachedGetNamed = new Dictionary<Type, Func<string, bool, Def>>();

		// Token: 0x04000295 RID: 661
		private static object cachedGetNamedLock = new object();

		// Token: 0x04000296 RID: 662
		private static Dictionary<Type, Func<string, Def>> cachedGetNamedSilentFail = new Dictionary<Type, Func<string, Def>>();

		// Token: 0x04000297 RID: 663
		private static object cachedGetNamedSilentFailLock = new object();
	}
}
