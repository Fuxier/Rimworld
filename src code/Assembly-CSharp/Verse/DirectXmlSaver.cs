using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Verse
{
	// Token: 0x0200039D RID: 925
	public static class DirectXmlSaver
	{
		// Token: 0x06001A79 RID: 6777 RVA: 0x0009FBC8 File Offset: 0x0009DDC8
		public static bool IsSimpleTextType(Type type)
		{
			return type == typeof(float) || type == typeof(double) || type == typeof(long) || type == typeof(ulong) || type == typeof(char) || type == typeof(byte) || type == typeof(sbyte) || type == typeof(int) || type == typeof(uint) || type == typeof(bool) || type == typeof(short) || type == typeof(ushort) || type == typeof(string) || type.IsEnum;
		}

		// Token: 0x06001A7A RID: 6778 RVA: 0x0009FCDC File Offset: 0x0009DEDC
		public static void SaveDataObject(object obj, string filePath)
		{
			try
			{
				XDocument xdocument = new XDocument();
				XElement content = DirectXmlSaver.XElementFromObject(obj, obj.GetType());
				xdocument.Add(content);
				xdocument.Save(filePath);
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception saving data object ",
					obj.ToStringSafe<object>(),
					": ",
					ex
				}));
				GenUI.ErrorDialog("ProblemSavingFile".Translate(filePath, ex.ToString()));
			}
		}

		// Token: 0x06001A7B RID: 6779 RVA: 0x0009FD70 File Offset: 0x0009DF70
		public static XElement XElementFromObject(object obj, Type expectedClass)
		{
			return DirectXmlSaver.XElementFromObject(obj, expectedClass, expectedClass.Name, null, false);
		}

		// Token: 0x06001A7C RID: 6780 RVA: 0x0009FD84 File Offset: 0x0009DF84
		public static XElement XElementFromObject(object obj, Type expectedType, string nodeName, FieldInfo owningField = null, bool saveDefsAsRefs = false)
		{
			DefaultValueAttribute defaultValueAttribute;
			if (owningField != null && owningField.TryGetAttribute(out defaultValueAttribute) && defaultValueAttribute.ObjIsDefault(obj))
			{
				return null;
			}
			if (obj == null)
			{
				XElement xelement = new XElement(nodeName);
				xelement.SetAttributeValue("IsNull", "True");
				return xelement;
			}
			Type type = obj.GetType();
			XElement xelement2 = new XElement(nodeName);
			if (DirectXmlSaver.IsSimpleTextType(type))
			{
				xelement2.Add(new XText(obj.ToString()));
			}
			else if (saveDefsAsRefs && GenTypes.IsDef(type))
			{
				string defName = ((Def)obj).defName;
				xelement2.Add(new XText(defName));
			}
			else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
			{
				Type expectedType2 = type.GetGenericArguments()[0];
				int num = (int)type.GetProperty("Count").GetValue(obj, null);
				for (int i = 0; i < num; i++)
				{
					object[] index = new object[]
					{
						i
					};
					XNode content = DirectXmlSaver.XElementFromObject(type.GetProperty("Item").GetValue(obj, index), expectedType2, "li", null, true);
					xelement2.Add(content);
				}
			}
			else
			{
				if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<, >))
				{
					Type expectedType3 = type.GetGenericArguments()[0];
					Type expectedType4 = type.GetGenericArguments()[1];
					using (IEnumerator enumerator = (obj as IEnumerable).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj2 = enumerator.Current;
							object value = obj2.GetType().GetProperty("Key").GetValue(obj2, null);
							object value2 = obj2.GetType().GetProperty("Value").GetValue(obj2, null);
							XElement xelement3 = new XElement("li");
							xelement3.Add(DirectXmlSaver.XElementFromObject(value, expectedType3, "key", null, true));
							xelement3.Add(DirectXmlSaver.XElementFromObject(value2, expectedType4, "value", null, true));
							xelement2.Add(xelement3);
						}
						return xelement2;
					}
				}
				if (type != expectedType)
				{
					XAttribute content2 = new XAttribute("Class", GenTypes.GetTypeNameWithoutIgnoredNamespaces(obj.GetType()));
					xelement2.Add(content2);
				}
				foreach (FieldInfo fi in from f in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				orderby f.MetadataToken
				select f)
				{
					try
					{
						XElement xelement4 = DirectXmlSaver.XElementFromField(fi, obj);
						if (xelement4 != null)
						{
							xelement2.Add(xelement4);
						}
					}
					catch
					{
						throw;
					}
				}
			}
			return xelement2;
		}

		// Token: 0x06001A7D RID: 6781 RVA: 0x000A0084 File Offset: 0x0009E284
		private static XElement XElementFromField(FieldInfo fi, object owningObj)
		{
			if (Attribute.IsDefined(fi, typeof(UnsavedAttribute)))
			{
				return null;
			}
			return DirectXmlSaver.XElementFromObject(fi.GetValue(owningObj), fi.FieldType, fi.Name, fi, false);
		}
	}
}
