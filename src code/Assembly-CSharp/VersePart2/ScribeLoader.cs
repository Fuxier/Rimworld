using System;
using System.IO;
using System.Xml;

namespace Verse
{
	// Token: 0x020003B2 RID: 946
	public class ScribeLoader
	{
		// Token: 0x06001AF6 RID: 6902 RVA: 0x000A4940 File Offset: 0x000A2B40
		public void InitLoading(string filePath)
		{
			if (Scribe.mode != LoadSaveMode.Inactive)
			{
				Log.Error("Called InitLoading() but current mode is " + Scribe.mode);
				Scribe.ForceStop();
			}
			if (this.curParent != null)
			{
				Log.Error("Current parent is not null in InitLoading");
				this.curParent = null;
			}
			if (this.curPathRelToParent != null)
			{
				Log.Error("Current path relative to parent is not null in InitLoading");
				this.curPathRelToParent = null;
			}
			try
			{
				using (StreamReader streamReader = new StreamReader(filePath))
				{
					using (XmlTextReader xmlTextReader = new XmlTextReader(streamReader))
					{
						XmlDocument xmlDocument = new XmlDocument();
						xmlDocument.Load(xmlTextReader);
						this.curXmlParent = xmlDocument.DocumentElement;
					}
				}
				Scribe.mode = LoadSaveMode.LoadingVars;
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception while init loading file: ",
					filePath,
					"\n",
					ex
				}));
				this.ForceStop();
				throw;
			}
		}

		// Token: 0x06001AF7 RID: 6903 RVA: 0x000A4A48 File Offset: 0x000A2C48
		public void InitLoadingMetaHeaderOnly(string filePath)
		{
			if (Scribe.mode != LoadSaveMode.Inactive)
			{
				Log.Error("Called InitLoadingMetaHeaderOnly() but current mode is " + Scribe.mode);
				Scribe.ForceStop();
			}
			try
			{
				using (StreamReader streamReader = new StreamReader(filePath))
				{
					using (XmlTextReader xmlTextReader = new XmlTextReader(streamReader))
					{
						if (ScribeMetaHeaderUtility.ReadToMetaElement(xmlTextReader))
						{
							using (XmlReader xmlReader = xmlTextReader.ReadSubtree())
							{
								XmlDocument xmlDocument = new XmlDocument();
								xmlDocument.Load(xmlReader);
								XmlElement xmlElement = xmlDocument.CreateElement("root");
								xmlElement.AppendChild(xmlDocument.DocumentElement);
								this.curXmlParent = xmlElement;
								goto IL_81;
							}
							goto IL_7F;
							IL_81:
							goto IL_8D;
						}
						IL_7F:
						return;
					}
					IL_8D:;
				}
				Scribe.mode = LoadSaveMode.LoadingVars;
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception while init loading meta header: ",
					filePath,
					"\n",
					ex
				}));
				this.ForceStop();
				throw;
			}
		}

		// Token: 0x06001AF8 RID: 6904 RVA: 0x000A4B60 File Offset: 0x000A2D60
		public void FinalizeLoading()
		{
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				Log.Error("Called FinalizeLoading() but current mode is " + Scribe.mode);
				return;
			}
			try
			{
				Scribe.ExitNode();
				this.curXmlParent = null;
				this.curParent = null;
				this.curPathRelToParent = null;
				Scribe.mode = LoadSaveMode.Inactive;
				DeepProfiler.Start("ResolveAllCrossReferences()");
				this.crossRefs.ResolveAllCrossReferences();
				DeepProfiler.End();
				DeepProfiler.Start("DoAllPostLoadInits()");
				this.initer.DoAllPostLoadInits();
				DeepProfiler.End();
			}
			catch (Exception arg)
			{
				Log.Error("Exception in FinalizeLoading(): " + arg);
				this.ForceStop();
				throw;
			}
		}

		// Token: 0x06001AF9 RID: 6905 RVA: 0x000A4C10 File Offset: 0x000A2E10
		public bool EnterNode(string nodeName)
		{
			if (this.curXmlParent != null)
			{
				XmlNode xmlNode = this.curXmlParent[nodeName];
				if (xmlNode == null && char.IsDigit(nodeName[0]))
				{
					xmlNode = this.curXmlParent.ChildNodes[int.Parse(nodeName)];
				}
				if (xmlNode == null)
				{
					return false;
				}
				this.curXmlParent = xmlNode;
			}
			this.curPathRelToParent = this.curPathRelToParent + "/" + nodeName;
			return true;
		}

		// Token: 0x06001AFA RID: 6906 RVA: 0x000A4C80 File Offset: 0x000A2E80
		public void ExitNode()
		{
			if (this.curXmlParent != null)
			{
				this.curXmlParent = this.curXmlParent.ParentNode;
			}
			if (this.curPathRelToParent != null)
			{
				int num = this.curPathRelToParent.LastIndexOf('/');
				this.curPathRelToParent = ((num > 0) ? this.curPathRelToParent.Substring(0, num) : null);
			}
		}

		// Token: 0x06001AFB RID: 6907 RVA: 0x000A4CD8 File Offset: 0x000A2ED8
		public void ForceStop()
		{
			this.curXmlParent = null;
			this.curParent = null;
			this.curPathRelToParent = null;
			this.crossRefs.Clear(false);
			this.initer.Clear();
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Scribe.mode = LoadSaveMode.Inactive;
			}
		}

		// Token: 0x0400139C RID: 5020
		public CrossRefHandler crossRefs = new CrossRefHandler();

		// Token: 0x0400139D RID: 5021
		public PostLoadIniter initer = new PostLoadIniter();

		// Token: 0x0400139E RID: 5022
		public IExposable curParent;

		// Token: 0x0400139F RID: 5023
		public XmlNode curXmlParent;

		// Token: 0x040013A0 RID: 5024
		public string curPathRelToParent;
	}
}
