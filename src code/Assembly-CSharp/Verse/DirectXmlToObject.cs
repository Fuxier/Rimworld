using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace Verse
{
	// Token: 0x0200039E RID: 926
	public static class DirectXmlToObject
	{
		// Token: 0x06001A7E RID: 6782 RVA: 0x000A00B4 File Offset: 0x0009E2B4
		public static Func<XmlNode, bool, object> GetObjectFromXmlMethod(Type type)
		{
			Func<XmlNode, bool, object> func;
			if (!DirectXmlToObject.objectFromXmlMethods.TryGetValue(type, out func))
			{
				MethodInfo method = typeof(DirectXmlToObject).GetMethod("ObjectFromXmlReflection", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				DirectXmlToObject.tmpOneTypeArray[0] = type;
				func = (Func<XmlNode, bool, object>)Delegate.CreateDelegate(typeof(Func<XmlNode, bool, object>), method.MakeGenericMethod(DirectXmlToObject.tmpOneTypeArray));
				DirectXmlToObject.objectFromXmlMethods.Add(type, func);
			}
			return func;
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x000A011C File Offset: 0x0009E31C
		private static object ObjectFromXmlReflection<T>(XmlNode xmlRoot, bool doPostLoad)
		{
			return DirectXmlToObject.ObjectFromXml<T>(xmlRoot, doPostLoad);
		}

		// Token: 0x06001A80 RID: 6784 RVA: 0x000A012C File Offset: 0x0009E32C
		public static T ObjectFromXml<T>(XmlNode xmlRoot, bool doPostLoad)
		{
			XmlAttribute xmlAttribute = xmlRoot.Attributes["IsNull"];
			T result;
			if (xmlAttribute != null && xmlAttribute.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
			{
				result = default(T);
				return result;
			}
			Type typeFromHandle = typeof(T);
			MethodInfo methodInfo = DirectXmlToObject.CustomDataLoadMethodOf(typeFromHandle);
			if (methodInfo != null)
			{
				xmlRoot = XmlInheritance.GetResolvedNodeFor(xmlRoot);
				Type type = DirectXmlToObject.ClassTypeOf<T>(xmlRoot);
				DirectXmlToObject.currentlyInstantiatingObjectOfType.Push(type);
				T t;
				try
				{
					t = (T)((object)Activator.CreateInstance(type));
				}
				finally
				{
					DirectXmlToObject.currentlyInstantiatingObjectOfType.Pop();
				}
				try
				{
					methodInfo.Invoke(t, new object[]
					{
						xmlRoot
					});
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception in custom XML loader for ",
						typeFromHandle,
						". Node is:\n ",
						xmlRoot.OuterXml,
						"\n\nException is:\n ",
						ex.ToString()
					}));
					t = default(T);
				}
				if (doPostLoad)
				{
					DirectXmlToObject.TryDoPostLoad(t);
				}
				return t;
			}
			if (GenTypes.IsSlateRef(typeFromHandle))
			{
				try
				{
					return ParseHelper.FromString<T>(DirectXmlToObject.InnerTextWithReplacedNewlinesOrXML(xmlRoot));
				}
				catch (Exception ex2)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception parsing ",
						xmlRoot.OuterXml,
						" to type ",
						typeFromHandle,
						": ",
						ex2
					}));
				}
				return default(T);
			}
			if (xmlRoot.ChildNodes.Count == 1 && xmlRoot.FirstChild.NodeType == XmlNodeType.Text)
			{
				try
				{
					return ParseHelper.FromString<T>(xmlRoot.InnerText);
				}
				catch (Exception ex3)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception parsing ",
						xmlRoot.OuterXml,
						" to type ",
						typeFromHandle,
						": ",
						ex3
					}));
				}
				return default(T);
			}
			if (xmlRoot.ChildNodes.Count == 1 && xmlRoot.FirstChild.NodeType == XmlNodeType.CDATA)
			{
				if (typeFromHandle != typeof(string))
				{
					Log.Error("CDATA can only be used for strings. Bad xml: " + xmlRoot.OuterXml);
					return default(T);
				}
				return (T)((object)xmlRoot.FirstChild.Value);
			}
			else
			{
				if (GenTypes.HasFlagsAttribute(typeFromHandle))
				{
					List<T> list = DirectXmlToObject.ListFromXml<T>(xmlRoot);
					int num = 0;
					foreach (T t2 in list)
					{
						int num2 = (int)((object)t2);
						num |= num2;
					}
					return (T)((object)num);
				}
				if (GenTypes.IsList(typeFromHandle))
				{
					Func<XmlNode, object> func = null;
					if (!DirectXmlToObject.listFromXmlMethods.TryGetValue(typeFromHandle, out func))
					{
						MethodInfo method = typeof(DirectXmlToObject).GetMethod("ListFromXmlReflection", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
						Type[] genericArguments = typeFromHandle.GetGenericArguments();
						func = (Func<XmlNode, object>)Delegate.CreateDelegate(typeof(Func<XmlNode, object>), method.MakeGenericMethod(genericArguments));
						DirectXmlToObject.listFromXmlMethods.Add(typeFromHandle, func);
					}
					return (T)((object)func(xmlRoot));
				}
				if (GenTypes.IsDictionary(typeFromHandle))
				{
					Func<XmlNode, object> func2 = null;
					if (!DirectXmlToObject.dictionaryFromXmlMethods.TryGetValue(typeFromHandle, out func2))
					{
						MethodInfo method2 = typeof(DirectXmlToObject).GetMethod("DictionaryFromXmlReflection", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
						Type[] genericArguments2 = typeFromHandle.GetGenericArguments();
						func2 = (Func<XmlNode, object>)Delegate.CreateDelegate(typeof(Func<XmlNode, object>), method2.MakeGenericMethod(genericArguments2));
						DirectXmlToObject.dictionaryFromXmlMethods.Add(typeFromHandle, func2);
					}
					return (T)((object)func2(xmlRoot));
				}
				if (!xmlRoot.HasChildNodes)
				{
					if (typeFromHandle == typeof(string))
					{
						return (T)((object)"");
					}
					XmlAttribute xmlAttribute2 = xmlRoot.Attributes["IsNull"];
					if (xmlAttribute2 != null && xmlAttribute2.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
					{
						return default(T);
					}
					if (GenTypes.IsListHashSetOrDictionary(typeFromHandle))
					{
						return Activator.CreateInstance<T>();
					}
				}
				xmlRoot = XmlInheritance.GetResolvedNodeFor(xmlRoot);
				Type type2 = DirectXmlToObject.ClassTypeOf<T>(xmlRoot);
				Type type3 = Nullable.GetUnderlyingType(type2) ?? type2;
				DirectXmlToObject.currentlyInstantiatingObjectOfType.Push(type3);
				T t3;
				try
				{
					t3 = (T)((object)Activator.CreateInstance(type3));
				}
				catch (InvalidCastException)
				{
					throw new InvalidCastException(string.Format("Cannot cast XML type {0} to C# type {1}.", type3, typeof(T)));
				}
				finally
				{
					DirectXmlToObject.currentlyInstantiatingObjectOfType.Pop();
				}
				HashSet<string> hashSet = null;
				if (xmlRoot.ChildNodes.Count > 1)
				{
					hashSet = new HashSet<string>();
				}
				XmlNodeList childNodes = xmlRoot.ChildNodes;
				for (int i = 0; i < childNodes.Count; i++)
				{
					XmlNode xmlNode = childNodes[i];
					if (!(xmlNode is XmlComment))
					{
						if (childNodes.Count > 1 && !hashSet.Add(xmlNode.Name))
						{
							Log.Error(string.Concat(new object[]
							{
								"XML ",
								typeFromHandle,
								" defines the same field twice: ",
								xmlNode.Name,
								".\n\nField contents: ",
								xmlNode.InnerText,
								".\n\nWhole XML:\n\n",
								xmlRoot.OuterXml
							}));
						}
						FieldInfo fieldInfo = DirectXmlToObject.GetFieldInfoForType(type3, xmlNode.Name, xmlRoot);
						if (fieldInfo == null)
						{
							DeepProfiler.Start("Field search");
							try
							{
								DirectXmlToObject.FieldAliasCache key = new DirectXmlToObject.FieldAliasCache(type3, xmlNode.Name);
								if (!DirectXmlToObject.fieldAliases.TryGetValue(key, out fieldInfo))
								{
									foreach (FieldInfo fieldInfo2 in type3.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
									{
										object[] customAttributes = fieldInfo2.GetCustomAttributes(typeof(LoadAliasAttribute), true);
										for (int k = 0; k < customAttributes.Length; k++)
										{
											if (((LoadAliasAttribute)customAttributes[k]).alias.EqualsIgnoreCase(xmlNode.Name))
											{
												fieldInfo = fieldInfo2;
												break;
											}
										}
										if (fieldInfo != null)
										{
											break;
										}
									}
									DirectXmlToObject.fieldAliases.Add(key, fieldInfo);
								}
							}
							finally
							{
								DeepProfiler.End();
							}
						}
						if (fieldInfo != null)
						{
							UnsavedAttribute unsavedAttribute = fieldInfo.TryGetAttribute<UnsavedAttribute>();
							if (unsavedAttribute != null && !unsavedAttribute.allowLoading)
							{
								Log.Error(string.Concat(new string[]
								{
									"XML error: ",
									xmlNode.OuterXml,
									" corresponds to a field in type ",
									type3.Name,
									" which has an Unsaved attribute. Context: ",
									xmlRoot.OuterXml
								}));
								goto IL_835;
							}
						}
						if (fieldInfo == null)
						{
							DeepProfiler.Start("Field search 2");
							try
							{
								bool flag = false;
								XmlAttributeCollection attributes = xmlNode.Attributes;
								XmlAttribute xmlAttribute3 = (attributes != null) ? attributes["IgnoreIfNoMatchingField"] : null;
								if (xmlAttribute3 != null && xmlAttribute3.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
								{
									flag = true;
								}
								else
								{
									object[] customAttributes = type3.GetCustomAttributes(typeof(IgnoreSavedElementAttribute), true);
									for (int j = 0; j < customAttributes.Length; j++)
									{
										if (string.Equals(((IgnoreSavedElementAttribute)customAttributes[j]).elementToIgnore, xmlNode.Name, StringComparison.OrdinalIgnoreCase))
										{
											flag = true;
											break;
										}
									}
								}
								if (flag)
								{
									goto IL_835;
								}
								Log.Error(string.Concat(new string[]
								{
									"XML error: ",
									xmlNode.OuterXml,
									" doesn't correspond to any field in type ",
									type3.Name,
									". Context: ",
									xmlRoot.OuterXml
								}));
								goto IL_835;
							}
							finally
							{
								DeepProfiler.End();
							}
						}
						if (GenTypes.IsDef(fieldInfo.FieldType))
						{
							if (xmlNode.InnerText.NullOrEmpty())
							{
								fieldInfo.SetValue(t3, null);
							}
							else
							{
								XmlAttribute xmlAttribute4 = xmlNode.Attributes["MayRequire"];
								XmlAttribute xmlAttribute5 = xmlNode.Attributes["MayRequireAnyOf"];
								DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(t3, fieldInfo, xmlNode.InnerText, (xmlAttribute4 != null) ? xmlAttribute4.Value.ToLower() : null, (xmlAttribute5 != null) ? xmlAttribute5.Value.ToLower() : null, null);
							}
						}
						else
						{
							object value = null;
							try
							{
								value = DirectXmlToObject.GetObjectFromXmlMethod(fieldInfo.FieldType)(xmlNode, doPostLoad);
							}
							catch (Exception ex4)
							{
								Log.Error("Exception loading from " + xmlNode.ToString() + ": " + ex4.ToString());
								goto IL_835;
							}
							if (!typeFromHandle.IsValueType)
							{
								fieldInfo.SetValue(t3, value);
							}
							else
							{
								object obj = t3;
								fieldInfo.SetValue(obj, value);
								t3 = (T)((object)obj);
							}
						}
					}
					IL_835:;
				}
				if (doPostLoad)
				{
					DirectXmlToObject.TryDoPostLoad(t3);
				}
				return t3;
			}
			return result;
		}

		// Token: 0x06001A81 RID: 6785 RVA: 0x000A0A14 File Offset: 0x0009EC14
		private static Type ClassTypeOf<T>(XmlNode xmlRoot)
		{
			XmlAttribute xmlAttribute = xmlRoot.Attributes["Class"];
			if (xmlAttribute == null)
			{
				return typeof(T);
			}
			Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(xmlAttribute.Value, typeof(T).Namespace);
			if (typeInAnyAssembly == null)
			{
				Log.Error("Could not find type named " + xmlAttribute.Value + " from node " + xmlRoot.OuterXml);
				return typeof(T);
			}
			return typeInAnyAssembly;
		}

		// Token: 0x06001A82 RID: 6786 RVA: 0x000A0A90 File Offset: 0x0009EC90
		private static void TryDoPostLoad(object obj)
		{
			DeepProfiler.Start("TryDoPostLoad");
			try
			{
				MethodInfo methodInfo = DirectXmlToObject.PostLoadMethodOf(obj.GetType());
				if (methodInfo != null)
				{
					methodInfo.Invoke(obj, null);
				}
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception while executing PostLoad on ",
					obj.ToStringSafe<object>(),
					": ",
					ex
				}));
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x000A0B1C File Offset: 0x0009ED1C
		private static object ListFromXmlReflection<T>(XmlNode listRootNode)
		{
			return DirectXmlToObject.ListFromXml<T>(listRootNode);
		}

		// Token: 0x06001A84 RID: 6788 RVA: 0x000A0B24 File Offset: 0x0009ED24
		private static List<T> ListFromXml<T>(XmlNode listRootNode)
		{
			List<T> list = new List<T>();
			try
			{
				bool flag = GenTypes.IsDef(typeof(T));
				foreach (object obj in listRootNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (DirectXmlToObject.ValidateListNode(xmlNode, listRootNode, typeof(T)))
					{
						XmlAttribute xmlAttribute = xmlNode.Attributes["MayRequire"];
						XmlAttribute xmlAttribute2 = xmlNode.Attributes["MayRequireAnyOf"];
						if (flag)
						{
							DirectXmlCrossRefLoader.RegisterListWantsCrossRef<T>(list, xmlNode.InnerText, listRootNode.Name, (xmlAttribute != null) ? xmlAttribute.Value : null, (xmlAttribute2 != null) ? xmlAttribute2.Value : null);
						}
						else if (xmlAttribute != null && !xmlAttribute.Value.NullOrEmpty() && !ModsConfig.AreAllActive(xmlAttribute.Value))
						{
							if (DirectXmlCrossRefLoader.MistypedMayRequire(xmlAttribute.Value))
							{
								Log.Error("Faulty MayRequire: " + xmlAttribute.Value);
							}
						}
						else if (xmlAttribute2 == null || xmlAttribute2.Value.NullOrEmpty() || ModsConfig.IsAnyActiveOrEmpty(xmlAttribute2.Value.Split(new char[]
						{
							','
						}), true))
						{
							try
							{
								list.Add(DirectXmlToObject.ObjectFromXml<T>(xmlNode, true));
							}
							catch (Exception arg)
							{
								Log.Error(string.Format("Exception loading list element {0} from XML: {1}\nXML:\n{2}", typeof(T), arg, listRootNode.OuterXml));
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception loading list from XML: ",
					ex,
					"\nXML:\n",
					listRootNode.OuterXml
				}));
			}
			return list;
		}

		// Token: 0x06001A85 RID: 6789 RVA: 0x000A0D24 File Offset: 0x0009EF24
		private static object DictionaryFromXmlReflection<K, V>(XmlNode dictRootNode)
		{
			return DirectXmlToObject.DictionaryFromXml<K, V>(dictRootNode);
		}

		// Token: 0x06001A86 RID: 6790 RVA: 0x000A0D2C File Offset: 0x0009EF2C
		private static Dictionary<K, V> DictionaryFromXml<K, V>(XmlNode dictRootNode)
		{
			Dictionary<K, V> dictionary = new Dictionary<K, V>();
			try
			{
				bool flag = GenTypes.IsDef(typeof(K));
				bool flag2 = GenTypes.IsDef(typeof(V));
				if (!flag && !flag2)
				{
					using (IEnumerator enumerator = dictRootNode.ChildNodes.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							XmlNode xmlNode = (XmlNode)obj;
							if (DirectXmlToObject.ValidateListNode(xmlNode, dictRootNode, typeof(KeyValuePair<K, V>)))
							{
								K key = DirectXmlToObject.ObjectFromXml<K>(xmlNode["key"], true);
								V value = DirectXmlToObject.ObjectFromXml<V>(xmlNode["value"], true);
								dictionary.Add(key, value);
							}
						}
						goto IL_100;
					}
				}
				foreach (object obj2 in dictRootNode.ChildNodes)
				{
					XmlNode xmlNode2 = (XmlNode)obj2;
					if (DirectXmlToObject.ValidateListNode(xmlNode2, dictRootNode, typeof(KeyValuePair<K, V>)))
					{
						DirectXmlCrossRefLoader.RegisterDictionaryWantsCrossRef<K, V>(dictionary, xmlNode2, dictRootNode.Name);
					}
				}
				IL_100:;
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Malformed dictionary XML. Node: ",
					dictRootNode.OuterXml,
					".\n\nException: ",
					ex
				}));
			}
			return dictionary;
		}

		// Token: 0x06001A87 RID: 6791 RVA: 0x000A0E98 File Offset: 0x0009F098
		private static MethodInfo CustomDataLoadMethodOf(Type type)
		{
			MethodInfo result;
			if (DirectXmlToObject.customDataLoadMethodCache.TryGetValue(type, out result))
			{
				return result;
			}
			MethodInfo method = type.GetMethod("LoadDataFromXmlCustom", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			DirectXmlToObject.customDataLoadMethodCache.Add(type, method);
			return method;
		}

		// Token: 0x06001A88 RID: 6792 RVA: 0x000A0ED4 File Offset: 0x0009F0D4
		private static MethodInfo PostLoadMethodOf(Type type)
		{
			MethodInfo result;
			if (DirectXmlToObject.postLoadMethodCache.TryGetValue(type, out result))
			{
				return result;
			}
			MethodInfo method = type.GetMethod("PostLoad");
			DirectXmlToObject.postLoadMethodCache.Add(type, method);
			return method;
		}

		// Token: 0x06001A89 RID: 6793 RVA: 0x000A0F0C File Offset: 0x0009F10C
		private static bool ValidateListNode(XmlNode listEntryNode, XmlNode listRootNode, Type listItemType)
		{
			if (listEntryNode is XmlComment)
			{
				return false;
			}
			if (listEntryNode is XmlText)
			{
				Log.Error("XML format error: Raw text found inside a list element. Did you mean to surround it with list item <li> tags? " + listRootNode.OuterXml);
				return false;
			}
			if (listEntryNode.Name != "li" && DirectXmlToObject.CustomDataLoadMethodOf(listItemType) == null)
			{
				Log.Error("XML format error: List item found with name that is not <li>, and which does not have a custom XML loader method, in " + listRootNode.OuterXml);
				return false;
			}
			return true;
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x000A0F7C File Offset: 0x0009F17C
		private static FieldInfo GetFieldInfoForType(Type type, string token, XmlNode debugXmlNode)
		{
			Dictionary<string, FieldInfo> dictionary;
			if (!DirectXmlToObject.fieldInfoLookup.TryGetValue(type, out dictionary))
			{
				dictionary = new Dictionary<string, FieldInfo>();
				DirectXmlToObject.fieldInfoLookup.Add(type, dictionary);
			}
			FieldInfo fieldInfo;
			if (!dictionary.TryGetValue(token, out fieldInfo))
			{
				fieldInfo = DirectXmlToObject.SearchTypeHierarchy(type, token, false);
				if (fieldInfo == null)
				{
					fieldInfo = DirectXmlToObject.SearchTypeHierarchy(type, token, true);
					if (fieldInfo != null && !type.HasAttribute<CaseInsensitiveXMLParsing>())
					{
						string text = string.Format("Attempt to use string {0} to refer to field {1} in type {2}; xml tags are now case-sensitive", token, fieldInfo.Name, type);
						if (debugXmlNode != null)
						{
							text = text + ". XML: " + debugXmlNode.OuterXml;
						}
						Log.Error(text);
					}
				}
				dictionary.Add(token, fieldInfo);
			}
			return fieldInfo;
		}

		// Token: 0x06001A8B RID: 6795 RVA: 0x000A1018 File Offset: 0x0009F218
		private static FieldInfo SearchTypeHierarchy(Type type, string token, bool ignoreCase)
		{
			Dictionary<ValueTuple<Type, string>, FieldInfo> dictionary = ignoreCase ? DirectXmlToObject.getFieldIgnoreCaseCache : DirectXmlToObject.getFieldCache;
			FieldInfo fieldInfo = null;
			for (;;)
			{
				if (!dictionary.TryGetValue(new ValueTuple<Type, string>(type, token), out fieldInfo))
				{
					fieldInfo = type.GetField(token, (ignoreCase ? BindingFlags.IgnoreCase : BindingFlags.Default) | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
					dictionary.Add(new ValueTuple<Type, string>(type, token), fieldInfo);
				}
				if (!(fieldInfo == null) || !(type.BaseType != typeof(object)))
				{
					break;
				}
				type = type.BaseType;
			}
			return fieldInfo;
		}

		// Token: 0x06001A8C RID: 6796 RVA: 0x000A1098 File Offset: 0x0009F298
		public static string InnerTextWithReplacedNewlinesOrXML(XmlNode xmlNode)
		{
			if (xmlNode.ChildNodes.Count == 1 && xmlNode.FirstChild.NodeType == XmlNodeType.Text)
			{
				return xmlNode.InnerText.Replace("\\n", "\n");
			}
			return xmlNode.InnerXml;
		}

		// Token: 0x04001344 RID: 4932
		public static Stack<Type> currentlyInstantiatingObjectOfType = new Stack<Type>();

		// Token: 0x04001345 RID: 4933
		public const string DictionaryKeyName = "key";

		// Token: 0x04001346 RID: 4934
		public const string DictionaryValueName = "value";

		// Token: 0x04001347 RID: 4935
		public const string LoadDataFromXmlCustomMethodName = "LoadDataFromXmlCustom";

		// Token: 0x04001348 RID: 4936
		public const string PostLoadMethodName = "PostLoad";

		// Token: 0x04001349 RID: 4937
		public const string ObjectFromXmlMethodName = "ObjectFromXmlReflection";

		// Token: 0x0400134A RID: 4938
		public const string ListFromXmlMethodName = "ListFromXmlReflection";

		// Token: 0x0400134B RID: 4939
		public const string DictionaryFromXmlMethodName = "DictionaryFromXmlReflection";

		// Token: 0x0400134C RID: 4940
		private static Dictionary<Type, Func<XmlNode, object>> listFromXmlMethods = new Dictionary<Type, Func<XmlNode, object>>();

		// Token: 0x0400134D RID: 4941
		private static Dictionary<Type, Func<XmlNode, object>> dictionaryFromXmlMethods = new Dictionary<Type, Func<XmlNode, object>>();

		// Token: 0x0400134E RID: 4942
		private static readonly Type[] tmpOneTypeArray = new Type[1];

		// Token: 0x0400134F RID: 4943
		private static readonly Dictionary<Type, Func<XmlNode, bool, object>> objectFromXmlMethods = new Dictionary<Type, Func<XmlNode, bool, object>>();

		// Token: 0x04001350 RID: 4944
		private static Dictionary<DirectXmlToObject.FieldAliasCache, FieldInfo> fieldAliases = new Dictionary<DirectXmlToObject.FieldAliasCache, FieldInfo>(EqualityComparer<DirectXmlToObject.FieldAliasCache>.Default);

		// Token: 0x04001351 RID: 4945
		private static Dictionary<Type, MethodInfo> customDataLoadMethodCache = new Dictionary<Type, MethodInfo>();

		// Token: 0x04001352 RID: 4946
		private static Dictionary<Type, MethodInfo> postLoadMethodCache = new Dictionary<Type, MethodInfo>();

		// Token: 0x04001353 RID: 4947
		private static Dictionary<Type, Dictionary<string, FieldInfo>> fieldInfoLookup = new Dictionary<Type, Dictionary<string, FieldInfo>>();

		// Token: 0x04001354 RID: 4948
		private static Dictionary<ValueTuple<Type, string>, FieldInfo> getFieldCache = new Dictionary<ValueTuple<Type, string>, FieldInfo>();

		// Token: 0x04001355 RID: 4949
		private static Dictionary<ValueTuple<Type, string>, FieldInfo> getFieldIgnoreCaseCache = new Dictionary<ValueTuple<Type, string>, FieldInfo>();

		// Token: 0x02001E78 RID: 7800
		private struct FieldAliasCache : IEquatable<DirectXmlToObject.FieldAliasCache>
		{
			// Token: 0x0600B92F RID: 47407 RVA: 0x0041F063 File Offset: 0x0041D263
			public FieldAliasCache(Type type, string fieldName)
			{
				this.type = type;
				this.fieldName = fieldName.ToLower();
			}

			// Token: 0x0600B930 RID: 47408 RVA: 0x0041F078 File Offset: 0x0041D278
			public bool Equals(DirectXmlToObject.FieldAliasCache other)
			{
				return this.type == other.type && string.Equals(this.fieldName, other.fieldName);
			}

			// Token: 0x0400780C RID: 30732
			public Type type;

			// Token: 0x0400780D RID: 30733
			public string fieldName;
		}
	}
}
