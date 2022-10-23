using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020003B1 RID: 945
	public static class ScribeExtractor
	{
		// Token: 0x06001AE7 RID: 6887 RVA: 0x000A3C38 File Offset: 0x000A1E38
		public static T ValueFromNode<T>(XmlNode subNode, T defaultValue)
		{
			if (subNode == null)
			{
				return defaultValue;
			}
			XmlAttribute xmlAttribute = subNode.Attributes["IsNull"];
			T result;
			if (xmlAttribute != null && xmlAttribute.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
			{
				result = default(T);
				return result;
			}
			try
			{
				try
				{
					return ParseHelper.FromString<T>(subNode.InnerText);
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception parsing node ",
						subNode.OuterXml,
						" into a ",
						typeof(T),
						":\n",
						ex.ToString()
					}));
				}
				result = default(T);
			}
			catch (Exception arg)
			{
				Log.Error("Exception loading XML: " + arg);
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x06001AE8 RID: 6888 RVA: 0x000A3D14 File Offset: 0x000A1F14
		public static T DefFromNode<T>(XmlNode subNode) where T : Def, new()
		{
			if (subNode == null || subNode.InnerText == null || subNode.InnerText == "null")
			{
				return default(T);
			}
			string text = BackCompatibility.BackCompatibleDefName(typeof(T), subNode.InnerText, false, subNode);
			T namedSilentFail = DefDatabase<T>.GetNamedSilentFail(text);
			if (namedSilentFail == null && !BackCompatibility.WasDefRemoved(subNode.InnerText, typeof(T)))
			{
				if (text == subNode.InnerText)
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not load reference to ",
						typeof(T),
						" named ",
						subNode.InnerText
					}));
				}
				else
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not load reference to ",
						typeof(T),
						" named ",
						subNode.InnerText,
						" after compatibility-conversion to ",
						text
					}));
				}
				BackCompatibility.PostCouldntLoadDef(subNode.InnerText);
			}
			return namedSilentFail;
		}

		// Token: 0x06001AE9 RID: 6889 RVA: 0x000A3E20 File Offset: 0x000A2020
		public static T DefFromNodeUnsafe<T>(XmlNode subNode)
		{
			Func<XmlNode, Def> func;
			if (!ScribeExtractor.defFromNodeCached.TryGetValue(typeof(T), out func))
			{
				MethodInfo method = typeof(ScribeExtractor).GetMethod("DefFromNode", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).MakeGenericMethod(new Type[]
				{
					typeof(T)
				});
				func = (Func<XmlNode, Def>)Delegate.CreateDelegate(typeof(Func<XmlNode, Def>), method);
				ScribeExtractor.defFromNodeCached.Add(typeof(T), func);
			}
			return (T)((object)func(subNode));
		}

		// Token: 0x06001AEA RID: 6890 RVA: 0x000A3EAC File Offset: 0x000A20AC
		public static T SaveableFromNode<T>(XmlNode subNode, object[] ctorArgs)
		{
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				Log.Error("Called SaveableFromNode(), but mode is " + Scribe.mode);
				return default(T);
			}
			if (subNode == null)
			{
				return default(T);
			}
			XmlAttribute xmlAttribute = subNode.Attributes["IsNull"];
			T result;
			if (xmlAttribute != null && xmlAttribute.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
			{
				result = default(T);
			}
			else
			{
				try
				{
					XmlAttribute xmlAttribute2 = subNode.Attributes["Class"];
					string text = (xmlAttribute2 != null) ? xmlAttribute2.Value : typeof(T).FullName;
					Type type = BackCompatibility.GetBackCompatibleType(typeof(T), text, subNode);
					if (type == null)
					{
						Type bestFallbackType = ScribeExtractor.GetBestFallbackType<T>(subNode);
						Log.Error(string.Concat(new object[]
						{
							"Could not find class ",
							text,
							" while resolving node ",
							subNode.Name,
							". Trying to use ",
							bestFallbackType,
							" instead. Full node: ",
							subNode.OuterXml
						}));
						type = bestFallbackType;
					}
					if (type.IsAbstract)
					{
						throw new ArgumentException("Can't load abstract class " + type);
					}
					IExposable exposable = (IExposable)Activator.CreateInstance(type, ctorArgs);
					bool flag = typeof(T).IsValueType || typeof(Name).IsAssignableFrom(typeof(T));
					if (!flag)
					{
						Scribe.loader.crossRefs.RegisterForCrossRefResolve(exposable);
					}
					XmlNode curXmlParent = Scribe.loader.curXmlParent;
					IExposable curParent = Scribe.loader.curParent;
					string curPathRelToParent = Scribe.loader.curPathRelToParent;
					Scribe.loader.curXmlParent = subNode;
					Scribe.loader.curParent = exposable;
					Scribe.loader.curPathRelToParent = null;
					try
					{
						exposable.ExposeData();
					}
					finally
					{
						Scribe.loader.curXmlParent = curXmlParent;
						Scribe.loader.curParent = curParent;
						Scribe.loader.curPathRelToParent = curPathRelToParent;
					}
					if (!flag)
					{
						Scribe.loader.initer.RegisterForPostLoadInit(exposable);
					}
					result = (T)((object)exposable);
				}
				catch (Exception ex)
				{
					result = default(T);
					Log.Error(string.Concat(new object[]
					{
						"SaveableFromNode exception: ",
						ex,
						"\nSubnode:\n",
						subNode.OuterXml
					}));
				}
			}
			return result;
		}

		// Token: 0x06001AEB RID: 6891 RVA: 0x000A413C File Offset: 0x000A233C
		private static Type GetBestFallbackType<T>(XmlNode node)
		{
			if (typeof(Thing).IsAssignableFrom(typeof(T)))
			{
				ThingDef thingDef = ScribeExtractor.TryFindDef<ThingDef>(node, "def");
				if (thingDef != null)
				{
					return thingDef.thingClass;
				}
			}
			else if (typeof(Hediff).IsAssignableFrom(typeof(T)))
			{
				HediffDef hediffDef = ScribeExtractor.TryFindDef<HediffDef>(node, "def");
				if (hediffDef != null)
				{
					return hediffDef.hediffClass;
				}
			}
			else if (typeof(Ability).IsAssignableFrom(typeof(T)))
			{
				AbilityDef abilityDef = ScribeExtractor.TryFindDef<AbilityDef>(node, "def");
				if (abilityDef != null)
				{
					return abilityDef.abilityClass;
				}
			}
			else if (typeof(Thought).IsAssignableFrom(typeof(T)))
			{
				ThoughtDef thoughtDef = ScribeExtractor.TryFindDef<ThoughtDef>(node, "def");
				if (thoughtDef != null)
				{
					return thoughtDef.thoughtClass;
				}
			}
			return typeof(T);
		}

		// Token: 0x06001AEC RID: 6892 RVA: 0x000A421C File Offset: 0x000A241C
		private static TDef TryFindDef<TDef>(XmlNode node, string defNodeName) where TDef : Def, new()
		{
			XmlElement xmlElement = node[defNodeName];
			if (xmlElement == null)
			{
				return default(TDef);
			}
			return DefDatabase<TDef>.GetNamedSilentFail(BackCompatibility.BackCompatibleDefName(typeof(TDef), xmlElement.InnerText, false, null));
		}

		// Token: 0x06001AED RID: 6893 RVA: 0x000A425C File Offset: 0x000A245C
		public static LocalTargetInfo LocalTargetInfoFromNode(XmlNode node, string label, LocalTargetInfo defaultValue)
		{
			LoadIDsWantedBank loadIDs = Scribe.loader.crossRefs.loadIDs;
			if (node != null && Scribe.EnterNode(label))
			{
				try
				{
					string innerText = node.InnerText;
					if (innerText.Length != 0 && innerText[0] == '(')
					{
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), "thing");
						return new LocalTargetInfo(IntVec3.FromString(innerText));
					}
					loadIDs.RegisterLoadIDReadFromXml(innerText, typeof(Thing), "thing");
					return LocalTargetInfo.Invalid;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), label + "/thing");
			return defaultValue;
		}

		// Token: 0x06001AEE RID: 6894 RVA: 0x000A4318 File Offset: 0x000A2518
		public static TargetInfo TargetInfoFromNode(XmlNode node, string label, TargetInfo defaultValue)
		{
			LoadIDsWantedBank loadIDs = Scribe.loader.crossRefs.loadIDs;
			if (node != null && Scribe.EnterNode(label))
			{
				try
				{
					string innerText = node.InnerText;
					if (innerText.Length != 0 && innerText[0] == '(')
					{
						string str;
						string targetLoadID;
						ScribeExtractor.ExtractCellAndMapPairFromTargetInfo(innerText, out str, out targetLoadID);
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), "thing");
						loadIDs.RegisterLoadIDReadFromXml(targetLoadID, typeof(Map), "map");
						return new TargetInfo(IntVec3.FromString(str), null, true);
					}
					loadIDs.RegisterLoadIDReadFromXml(innerText, typeof(Thing), "thing");
					loadIDs.RegisterLoadIDReadFromXml(null, typeof(Map), "map");
					return TargetInfo.Invalid;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), label + "/thing");
			loadIDs.RegisterLoadIDReadFromXml(null, typeof(Map), label + "/map");
			return defaultValue;
		}

		// Token: 0x06001AEF RID: 6895 RVA: 0x000A4430 File Offset: 0x000A2630
		public static GlobalTargetInfo GlobalTargetInfoFromNode(XmlNode node, string label, GlobalTargetInfo defaultValue)
		{
			LoadIDsWantedBank loadIDs = Scribe.loader.crossRefs.loadIDs;
			if (node != null && Scribe.EnterNode(label))
			{
				try
				{
					string innerText = node.InnerText;
					if (innerText.Length != 0 && innerText[0] == '(')
					{
						string str;
						string targetLoadID;
						ScribeExtractor.ExtractCellAndMapPairFromTargetInfo(innerText, out str, out targetLoadID);
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), "thing");
						loadIDs.RegisterLoadIDReadFromXml(targetLoadID, typeof(Map), "map");
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(WorldObject), "worldObject");
						return new GlobalTargetInfo(IntVec3.FromString(str), null, true);
					}
					int tile;
					if (int.TryParse(innerText, out tile))
					{
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), "thing");
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(Map), "map");
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(WorldObject), "worldObject");
						return new GlobalTargetInfo(tile);
					}
					if (innerText.Length != 0 && innerText[0] == '@')
					{
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), "thing");
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(Map), "map");
						loadIDs.RegisterLoadIDReadFromXml(innerText.Substring(1), typeof(WorldObject), "worldObject");
						return GlobalTargetInfo.Invalid;
					}
					loadIDs.RegisterLoadIDReadFromXml(innerText, typeof(Thing), "thing");
					loadIDs.RegisterLoadIDReadFromXml(null, typeof(Map), "map");
					loadIDs.RegisterLoadIDReadFromXml(null, typeof(WorldObject), "worldObject");
					return GlobalTargetInfo.Invalid;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), label + "/thing");
			loadIDs.RegisterLoadIDReadFromXml(null, typeof(Map), label + "/map");
			loadIDs.RegisterLoadIDReadFromXml(null, typeof(WorldObject), label + "/worldObject");
			return defaultValue;
		}

		// Token: 0x06001AF0 RID: 6896 RVA: 0x000A4660 File Offset: 0x000A2860
		public static LocalTargetInfo ResolveLocalTargetInfo(LocalTargetInfo loaded, string label)
		{
			if (Scribe.EnterNode(label))
			{
				try
				{
					Thing thing = Scribe.loader.crossRefs.TakeResolvedRef<Thing>("thing");
					IntVec3 cell = loaded.Cell;
					if (thing != null)
					{
						return new LocalTargetInfo(thing);
					}
					return new LocalTargetInfo(cell);
				}
				finally
				{
					Scribe.ExitNode();
				}
				return loaded;
			}
			return loaded;
		}

		// Token: 0x06001AF1 RID: 6897 RVA: 0x000A46C0 File Offset: 0x000A28C0
		public static TargetInfo ResolveTargetInfo(TargetInfo loaded, string label)
		{
			if (Scribe.EnterNode(label))
			{
				try
				{
					Thing thing = Scribe.loader.crossRefs.TakeResolvedRef<Thing>("thing");
					Map map = Scribe.loader.crossRefs.TakeResolvedRef<Map>("map");
					IntVec3 cell = loaded.Cell;
					if (thing != null)
					{
						return new TargetInfo(thing);
					}
					if (cell.IsValid && map != null)
					{
						return new TargetInfo(cell, map, false);
					}
					return TargetInfo.Invalid;
				}
				finally
				{
					Scribe.ExitNode();
				}
				return loaded;
			}
			return loaded;
		}

		// Token: 0x06001AF2 RID: 6898 RVA: 0x000A474C File Offset: 0x000A294C
		public static GlobalTargetInfo ResolveGlobalTargetInfo(GlobalTargetInfo loaded, string label)
		{
			if (Scribe.EnterNode(label))
			{
				try
				{
					Thing thing = Scribe.loader.crossRefs.TakeResolvedRef<Thing>("thing");
					Map map = Scribe.loader.crossRefs.TakeResolvedRef<Map>("map");
					WorldObject worldObject = Scribe.loader.crossRefs.TakeResolvedRef<WorldObject>("worldObject");
					IntVec3 cell = loaded.Cell;
					int tile = loaded.Tile;
					if (thing != null)
					{
						return new GlobalTargetInfo(thing);
					}
					if (worldObject != null)
					{
						return new GlobalTargetInfo(worldObject);
					}
					if (cell.IsValid)
					{
						if (map != null)
						{
							return new GlobalTargetInfo(cell, map, false);
						}
						return GlobalTargetInfo.Invalid;
					}
					else
					{
						if (tile >= 0)
						{
							return new GlobalTargetInfo(tile);
						}
						return GlobalTargetInfo.Invalid;
					}
				}
				finally
				{
					Scribe.ExitNode();
				}
				return loaded;
			}
			return loaded;
		}

		// Token: 0x06001AF3 RID: 6899 RVA: 0x000A4824 File Offset: 0x000A2A24
		public static BodyPartRecord BodyPartFromNode(XmlNode node, string label, BodyPartRecord defaultValue)
		{
			if (node != null && Scribe.EnterNode(label))
			{
				try
				{
					XmlAttribute xmlAttribute = node.Attributes["IsNull"];
					if (xmlAttribute != null && xmlAttribute.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
					{
						return null;
					}
					BodyDef bodyDef = ScribeExtractor.DefFromNode<BodyDef>(Scribe.loader.curXmlParent["body"]);
					if (bodyDef == null)
					{
						return null;
					}
					XmlElement xmlElement = Scribe.loader.curXmlParent["index"];
					int index = (xmlElement != null) ? int.Parse(xmlElement.InnerText) : -1;
					index = BackCompatibility.GetBackCompatibleBodyPartIndex(bodyDef, index);
					return bodyDef.GetPartAtIndex(index);
				}
				finally
				{
					Scribe.ExitNode();
				}
				return defaultValue;
			}
			return defaultValue;
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x000A48E4 File Offset: 0x000A2AE4
		private static void ExtractCellAndMapPairFromTargetInfo(string str, out string cell, out string map)
		{
			int num = str.IndexOf(')');
			cell = str.Substring(0, num + 1);
			int num2 = str.IndexOf(',', num + 1);
			map = str.Substring(num2 + 1);
			map = map.TrimStart(new char[]
			{
				' '
			});
		}

		// Token: 0x0400139B RID: 5019
		private static readonly Dictionary<Type, Func<XmlNode, Def>> defFromNodeCached = new Dictionary<Type, Func<XmlNode, Def>>();
	}
}
