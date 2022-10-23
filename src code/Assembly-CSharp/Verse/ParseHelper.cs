using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using RimWorld;
using RimWorld.QuestGen;
using Steamworks;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000041 RID: 65
	public static class ParseHelper
	{
		// Token: 0x06000372 RID: 882 RVA: 0x0001368B File Offset: 0x0001188B
		public static string ParseString(string str)
		{
			return str.Replace("\\n", "\n");
		}

		// Token: 0x06000373 RID: 883 RVA: 0x000136A0 File Offset: 0x000118A0
		public static int ParseIntPermissive(string str)
		{
			int result;
			if (!int.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				result = (int)float.Parse(str, CultureInfo.InvariantCulture);
				Log.Warning("Parsed " + str + " as int.");
			}
			return result;
		}

		// Token: 0x06000374 RID: 884 RVA: 0x000136E4 File Offset: 0x000118E4
		public static Vector3 FromStringVector3(string Str)
		{
			Str = Str.TrimStart(new char[]
			{
				'('
			});
			Str = Str.TrimEnd(new char[]
			{
				')'
			});
			string[] array = Str.Split(new char[]
			{
				','
			});
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			float x = Convert.ToSingle(array[0], invariantCulture);
			float y = Convert.ToSingle(array[1], invariantCulture);
			float z = Convert.ToSingle(array[2], invariantCulture);
			return new Vector3(x, y, z);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x00013754 File Offset: 0x00011954
		public static Vector2 FromStringVector2(string Str)
		{
			Str = Str.TrimStart(new char[]
			{
				'('
			});
			Str = Str.TrimEnd(new char[]
			{
				')'
			});
			string[] array = Str.Split(new char[]
			{
				','
			});
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			float x;
			float y;
			if (array.Length == 1)
			{
				y = (x = Convert.ToSingle(array[0], invariantCulture));
			}
			else
			{
				if (array.Length != 2)
				{
					throw new InvalidOperationException();
				}
				x = Convert.ToSingle(array[0], invariantCulture);
				y = Convert.ToSingle(array[1], invariantCulture);
			}
			return new Vector2(x, y);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x000137DC File Offset: 0x000119DC
		public static Vector4 FromStringVector4Adaptive(string Str)
		{
			Str = Str.TrimStart(new char[]
			{
				'('
			});
			Str = Str.TrimEnd(new char[]
			{
				')'
			});
			string[] array = Str.Split(new char[]
			{
				','
			});
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			float x = 0f;
			float y = 0f;
			float z = 0f;
			float w = 0f;
			if (array.Length >= 1)
			{
				x = Convert.ToSingle(array[0], invariantCulture);
			}
			if (array.Length >= 2)
			{
				y = Convert.ToSingle(array[1], invariantCulture);
			}
			if (array.Length >= 3)
			{
				z = Convert.ToSingle(array[2], invariantCulture);
			}
			if (array.Length >= 4)
			{
				w = Convert.ToSingle(array[3], invariantCulture);
			}
			if (array.Length >= 5)
			{
				Log.ErrorOnce(string.Format("Too many elements in vector {0}", Str), 16139142);
			}
			return new Vector4(x, y, z, w);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x000138AC File Offset: 0x00011AAC
		public static Rect FromStringRect(string str)
		{
			str = str.TrimStart(new char[]
			{
				'('
			});
			str = str.TrimEnd(new char[]
			{
				')'
			});
			string[] array = str.Split(new char[]
			{
				','
			});
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			float x = Convert.ToSingle(array[0], invariantCulture);
			float y = Convert.ToSingle(array[1], invariantCulture);
			float width = Convert.ToSingle(array[2], invariantCulture);
			float height = Convert.ToSingle(array[3], invariantCulture);
			return new Rect(x, y, width, height);
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00013928 File Offset: 0x00011B28
		public static float ParseFloat(string str)
		{
			return float.Parse(str, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00013935 File Offset: 0x00011B35
		public static bool ParseBool(string str)
		{
			return bool.Parse(str);
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0001393D File Offset: 0x00011B3D
		public static long ParseLong(string str)
		{
			return long.Parse(str, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0001394A File Offset: 0x00011B4A
		public static double ParseDouble(string str)
		{
			return double.Parse(str, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600037C RID: 892 RVA: 0x00013957 File Offset: 0x00011B57
		public static sbyte ParseSByte(string str)
		{
			return sbyte.Parse(str, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600037D RID: 893 RVA: 0x00013964 File Offset: 0x00011B64
		public static Type ParseType(string str)
		{
			if (str == "null" || str == "Null")
			{
				return null;
			}
			Type type = GenTypes.GetTypeInAnyAssembly(str, null);
			if (type == null)
			{
				type = BackCompatibility.GetBackCompatibleTypeDirect(typeof(Type), str);
				if (type == null)
				{
					Log.Error("Could not find a type named " + str);
				}
			}
			return type;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x000139CC File Offset: 0x00011BCC
		public static Action ParseAction(string str)
		{
			string[] array = str.Split(new char[]
			{
				'.'
			});
			string methodName = array[array.Length - 1];
			string typeName;
			if (array.Length == 3)
			{
				typeName = array[0] + "." + array[1];
			}
			else
			{
				typeName = array[0];
			}
			MethodInfo method = GenTypes.GetTypeInAnyAssembly(typeName, null).GetMethods().First((MethodInfo m) => m.Name == methodName);
			return (Action)Delegate.CreateDelegate(typeof(Action), method);
		}

		// Token: 0x0600037F RID: 895 RVA: 0x00013A50 File Offset: 0x00011C50
		public static Color ParseColor(string str)
		{
			str = str.TrimStart(ParseHelper.colorTrimStartParameters);
			str = str.TrimEnd(ParseHelper.colorTrimEndParameters);
			string[] array = str.Split(new char[]
			{
				','
			});
			float num = ParseHelper.ParseFloat(array[0]);
			float num2 = ParseHelper.ParseFloat(array[1]);
			float num3 = ParseHelper.ParseFloat(array[2]);
			bool flag = num > 1f || num3 > 1f || num2 > 1f;
			float num4 = (float)(flag ? 255 : 1);
			if (array.Length == 4)
			{
				num4 = ParseHelper.FromString<float>(array[3]);
			}
			Color result;
			if (!flag)
			{
				result.r = num;
				result.g = num2;
				result.b = num3;
				result.a = num4;
			}
			else
			{
				result = GenColor.FromBytes(Mathf.RoundToInt(num), Mathf.RoundToInt(num2), Mathf.RoundToInt(num3), Mathf.RoundToInt(num4));
			}
			return result;
		}

		// Token: 0x06000380 RID: 896 RVA: 0x00013B24 File Offset: 0x00011D24
		public static PublishedFileId_t ParsePublishedFileId(string str)
		{
			return new PublishedFileId_t(ulong.Parse(str));
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00013B31 File Offset: 0x00011D31
		public static IntVec2 ParseIntVec2(string str)
		{
			return IntVec2.FromString(str);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x00013B39 File Offset: 0x00011D39
		public static IntVec3 ParseIntVec3(string str)
		{
			return IntVec3.FromString(str);
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00013B41 File Offset: 0x00011D41
		public static Rot4 ParseRot4(string str)
		{
			return Rot4.FromString(str);
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00013B49 File Offset: 0x00011D49
		public static CellRect ParseCellRect(string str)
		{
			return CellRect.FromString(str);
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00013B51 File Offset: 0x00011D51
		public static CurvePoint ParseCurvePoint(string str)
		{
			return CurvePoint.FromString(str);
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00013B59 File Offset: 0x00011D59
		public static NameTriple ParseNameTriple(string str)
		{
			return NameTriple.FromString(str, false);
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00013B62 File Offset: 0x00011D62
		public static FloatRange ParseFloatRange(string str)
		{
			return FloatRange.FromString(str);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00013B6A File Offset: 0x00011D6A
		public static IntRange ParseIntRange(string str)
		{
			return IntRange.FromString(str);
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00013B72 File Offset: 0x00011D72
		public static QualityRange ParseQualityRange(string str)
		{
			return QualityRange.FromString(str);
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00013B7C File Offset: 0x00011D7C
		public static ColorInt ParseColorInt(string str)
		{
			str = str.TrimStart(ParseHelper.colorTrimStartParameters);
			str = str.TrimEnd(ParseHelper.colorTrimEndParameters);
			string[] array = str.Split(new char[]
			{
				','
			});
			ColorInt result = new ColorInt(255, 255, 255, 255);
			result.r = ParseHelper.ParseIntPermissive(array[0]);
			result.g = ParseHelper.ParseIntPermissive(array[1]);
			result.b = ParseHelper.ParseIntPermissive(array[2]);
			if (array.Length == 4)
			{
				result.a = ParseHelper.ParseIntPermissive(array[3]);
			}
			else
			{
				result.a = 255;
			}
			return result;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00013C21 File Offset: 0x00011E21
		public static TaggedString ParseTaggedString(string str)
		{
			return str;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00013C2C File Offset: 0x00011E2C
		static ParseHelper()
		{
			ParseHelper.Parsers<string>.Register(new Func<string, string>(ParseHelper.ParseString));
			ParseHelper.Parsers<int>.Register(new Func<string, int>(ParseHelper.ParseIntPermissive));
			ParseHelper.Parsers<Vector3>.Register(new Func<string, Vector3>(ParseHelper.FromStringVector3));
			ParseHelper.Parsers<Vector2>.Register(new Func<string, Vector2>(ParseHelper.FromStringVector2));
			ParseHelper.Parsers<Vector4>.Register(new Func<string, Vector4>(ParseHelper.FromStringVector4Adaptive));
			ParseHelper.Parsers<Rect>.Register(new Func<string, Rect>(ParseHelper.FromStringRect));
			ParseHelper.Parsers<float>.Register(new Func<string, float>(ParseHelper.ParseFloat));
			ParseHelper.Parsers<bool>.Register(new Func<string, bool>(ParseHelper.ParseBool));
			ParseHelper.Parsers<long>.Register(new Func<string, long>(ParseHelper.ParseLong));
			ParseHelper.Parsers<double>.Register(new Func<string, double>(ParseHelper.ParseDouble));
			ParseHelper.Parsers<sbyte>.Register(new Func<string, sbyte>(ParseHelper.ParseSByte));
			ParseHelper.Parsers<Type>.Register(new Func<string, Type>(ParseHelper.ParseType));
			ParseHelper.Parsers<Action>.Register(new Func<string, Action>(ParseHelper.ParseAction));
			ParseHelper.Parsers<Color>.Register(new Func<string, Color>(ParseHelper.ParseColor));
			ParseHelper.Parsers<PublishedFileId_t>.Register(new Func<string, PublishedFileId_t>(ParseHelper.ParsePublishedFileId));
			ParseHelper.Parsers<IntVec2>.Register(new Func<string, IntVec2>(ParseHelper.ParseIntVec2));
			ParseHelper.Parsers<IntVec3>.Register(new Func<string, IntVec3>(ParseHelper.ParseIntVec3));
			ParseHelper.Parsers<Rot4>.Register(new Func<string, Rot4>(ParseHelper.ParseRot4));
			ParseHelper.Parsers<CellRect>.Register(new Func<string, CellRect>(ParseHelper.ParseCellRect));
			ParseHelper.Parsers<CurvePoint>.Register(new Func<string, CurvePoint>(ParseHelper.ParseCurvePoint));
			ParseHelper.Parsers<NameTriple>.Register(new Func<string, NameTriple>(ParseHelper.ParseNameTriple));
			ParseHelper.Parsers<FloatRange>.Register(new Func<string, FloatRange>(ParseHelper.ParseFloatRange));
			ParseHelper.Parsers<IntRange>.Register(new Func<string, IntRange>(ParseHelper.ParseIntRange));
			ParseHelper.Parsers<QualityRange>.Register(new Func<string, QualityRange>(ParseHelper.ParseQualityRange));
			ParseHelper.Parsers<ColorInt>.Register(new Func<string, ColorInt>(ParseHelper.ParseColorInt));
			ParseHelper.Parsers<TaggedString>.Register(new Func<string, TaggedString>(ParseHelper.ParseTaggedString));
		}

		// Token: 0x0600038D RID: 909 RVA: 0x00013E24 File Offset: 0x00012024
		public static T FromString<T>(string str)
		{
			Func<string, T> parser = ParseHelper.Parsers<T>.parser;
			if (parser != null)
			{
				return parser(str);
			}
			return (T)((object)ParseHelper.FromString(str, typeof(T)));
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00013E58 File Offset: 0x00012058
		public static object FromString(string str, Type itemType)
		{
			object result;
			try
			{
				itemType = (Nullable.GetUnderlyingType(itemType) ?? itemType);
				if (itemType.IsEnum)
				{
					try
					{
						object obj = BackCompatibility.BackCompatibleEnum(itemType, str);
						if (obj != null)
						{
							return obj;
						}
						return Enum.Parse(itemType, str);
					}
					catch (ArgumentException innerException)
					{
						throw new ArgumentException(string.Concat(new object[]
						{
							"'",
							str,
							"' is not a valid value for ",
							itemType,
							". Valid values are: \n"
						}) + GenText.StringFromEnumerable(Enum.GetValues(itemType)), innerException);
					}
				}
				Func<string, object> func;
				if (ParseHelper.parsers.TryGetValue(itemType, out func))
				{
					result = func(str);
				}
				else if (GenTypes.IsSlateRef(itemType))
				{
					ISlateRef slateRef = (ISlateRef)Activator.CreateInstance(itemType);
					slateRef.SlateRef = str;
					result = slateRef;
				}
				else
				{
					if (!itemType.IsSubclassOf(typeof(Delegate)))
					{
						throw new ArgumentException(string.Concat(new string[]
						{
							"Trying to parse to unknown data type ",
							itemType.Name,
							". Content is '",
							str,
							"'."
						}));
					}
					int num = str.LastIndexOf('.');
					string typeName = (num >= 0) ? str.Substring(0, num) : "";
					string method = str.Substring(num + 1, str.Length - (num + 1));
					Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(typeName, null);
					result = Delegate.CreateDelegate(itemType, typeInAnyAssembly, method);
				}
			}
			catch (Exception innerException2)
			{
				throw new ArgumentException(string.Concat(new object[]
				{
					"Exception parsing ",
					itemType,
					" from \"",
					str,
					"\""
				}), innerException2);
			}
			return result;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00014018 File Offset: 0x00012218
		public static bool HandlesType(Type type)
		{
			type = (Nullable.GetUnderlyingType(type) ?? type);
			return type.IsPrimitive || type.IsEnum || ParseHelper.parsers.ContainsKey(type) || GenTypes.IsSlateRef(type);
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0001404C File Offset: 0x0001224C
		public static bool CanParse(Type type, string str)
		{
			if (!ParseHelper.HandlesType(type))
			{
				return false;
			}
			try
			{
				ParseHelper.FromString(str, type);
			}
			catch (ArgumentException)
			{
				return false;
			}
			catch (FormatException)
			{
				return false;
			}
			return true;
		}

		// Token: 0x040000D9 RID: 217
		private static Dictionary<Type, Func<string, object>> parsers = new Dictionary<Type, Func<string, object>>();

		// Token: 0x040000DA RID: 218
		private static readonly char[] colorTrimStartParameters = new char[]
		{
			'(',
			'R',
			'G',
			'B',
			'A'
		};

		// Token: 0x040000DB RID: 219
		private static readonly char[] colorTrimEndParameters = new char[]
		{
			')'
		};

		// Token: 0x02001C7F RID: 7295
		public static class Parsers<T>
		{
			// Token: 0x0600AF9D RID: 44957 RVA: 0x003FDC30 File Offset: 0x003FBE30
			public static void Register(Func<string, T> method)
			{
				ParseHelper.Parsers<T>.parser = method;
				ParseHelper.parsers.Add(typeof(T), (string str) => method(str));
			}

			// Token: 0x0400705B RID: 28763
			public static Func<string, T> parser;

			// Token: 0x0400705C RID: 28764
			public static readonly string profilerLabel = "ParseHelper.FromString<" + typeof(T).FullName + ">()";
		}
	}
}
