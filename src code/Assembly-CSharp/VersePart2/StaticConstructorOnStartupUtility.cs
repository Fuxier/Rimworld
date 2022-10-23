using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200005E RID: 94
	public static class StaticConstructorOnStartupUtility
	{
		// Token: 0x06000450 RID: 1104 RVA: 0x00018118 File Offset: 0x00016318
		public static void CallAll()
		{
			DeepProfiler.Start("StaticConstructorOnStartupUtility.CallAll()");
			foreach (Type type in GenTypes.AllTypesWithAttribute<StaticConstructorOnStartup>())
			{
				try
				{
					RuntimeHelpers.RunClassConstructor(type.TypeHandle);
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Error in static constructor of ",
						type,
						": ",
						ex
					}));
				}
			}
			DeepProfiler.End();
			StaticConstructorOnStartupUtility.coreStaticAssetsLoaded = true;
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x000181BC File Offset: 0x000163BC
		public static void ReportProbablyMissingAttributes()
		{
			BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
			Parallel.ForEach<Type>(GenTypes.AllTypes, delegate(Type t)
			{
				if (t.HasAttribute<StaticConstructorOnStartup>())
				{
					return;
				}
				FieldInfo fieldInfo = t.GetFields(bindingFlags).FirstOrDefault(delegate(FieldInfo x)
				{
					Type type = x.FieldType;
					if (type.IsArray)
					{
						type = type.GetElementType();
					}
					return typeof(Texture).IsAssignableFrom(type) || typeof(Material).IsAssignableFrom(type) || typeof(Shader).IsAssignableFrom(type) || typeof(Graphic).IsAssignableFrom(type) || typeof(GameObject).IsAssignableFrom(type) || typeof(MaterialPropertyBlock).IsAssignableFrom(type);
				});
				if (fieldInfo != null)
				{
					Log.Warning(string.Concat(new string[]
					{
						"Type ",
						t.Name,
						" probably needs a StaticConstructorOnStartup attribute, because it has a field ",
						fieldInfo.Name,
						" of type ",
						fieldInfo.FieldType.Name,
						". All assets must be loaded in the main thread."
					}));
				}
			});
		}

		// Token: 0x0400016C RID: 364
		public static bool coreStaticAssetsLoaded;
	}
}
