using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Xml.XPath;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000501 RID: 1281
	public static class LayerLoader
	{
		// Token: 0x06002700 RID: 9984 RVA: 0x000FA86C File Offset: 0x000F8A6C
		public static void LoadFileIntoList(TextAsset ass, List<DiaNodeMold> NodeListToFill, List<DiaNodeList> ListListToFill, DiaNodeType NodesType)
		{
			XPathNavigator xpathNavigator = new XPathDocument(new StringReader(ass.text)).CreateNavigator();
			xpathNavigator.MoveToFirst();
			xpathNavigator.MoveToFirstChild();
			foreach (object obj in xpathNavigator.Select("Node"))
			{
				XPathNavigator xpathNavigator2 = (XPathNavigator)obj;
				try
				{
					TextReader textReader = new StringReader(xpathNavigator2.OuterXml);
					DiaNodeMold diaNodeMold = (DiaNodeMold)new XmlSerializer(typeof(DiaNodeMold)).Deserialize(textReader);
					diaNodeMold.nodeType = NodesType;
					NodeListToFill.Add(diaNodeMold);
					textReader.Dispose();
				}
				catch (Exception ex)
				{
					Log.Message(string.Concat(new object[]
					{
						"Exception deserializing ",
						xpathNavigator2.OuterXml,
						":\n",
						ex.InnerException
					}));
				}
			}
			foreach (object obj2 in xpathNavigator.Select("NodeList"))
			{
				XPathNavigator xpathNavigator3 = (XPathNavigator)obj2;
				try
				{
					TextReader textReader2 = new StringReader(xpathNavigator3.OuterXml);
					DiaNodeList item = (DiaNodeList)new XmlSerializer(typeof(DiaNodeList)).Deserialize(textReader2);
					ListListToFill.Add(item);
				}
				catch (Exception ex2)
				{
					Log.Message(string.Concat(new object[]
					{
						"Exception deserializing ",
						xpathNavigator3.OuterXml,
						":\n",
						ex2.InnerException
					}));
				}
			}
		}

		// Token: 0x06002701 RID: 9985 RVA: 0x000FAA3C File Offset: 0x000F8C3C
		public static void MarkNonRootNodes(List<DiaNodeMold> NodeList)
		{
			foreach (DiaNodeMold d in NodeList)
			{
				LayerLoader.RecursiveSetIsRootFalse(d);
			}
			foreach (DiaNodeMold diaNodeMold in NodeList)
			{
				foreach (DiaNodeMold diaNodeMold2 in NodeList)
				{
					foreach (DiaOptionMold diaOptionMold in diaNodeMold2.optionList)
					{
						bool flag = false;
						using (List<string>.Enumerator enumerator4 = diaOptionMold.ChildNodeNames.GetEnumerator())
						{
							while (enumerator4.MoveNext())
							{
								if (enumerator4.Current == diaNodeMold.name)
								{
									flag = true;
								}
							}
						}
						if (flag)
						{
							diaNodeMold.isRoot = false;
						}
					}
				}
			}
		}

		// Token: 0x06002702 RID: 9986 RVA: 0x000FAB90 File Offset: 0x000F8D90
		private static void RecursiveSetIsRootFalse(DiaNodeMold d)
		{
			foreach (DiaOptionMold diaOptionMold in d.optionList)
			{
				foreach (DiaNodeMold diaNodeMold in diaOptionMold.ChildNodes)
				{
					diaNodeMold.isRoot = false;
					LayerLoader.RecursiveSetIsRootFalse(diaNodeMold);
				}
			}
		}
	}
}
