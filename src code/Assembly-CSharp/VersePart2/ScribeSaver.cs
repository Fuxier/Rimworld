using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Verse
{
	// Token: 0x020003B5 RID: 949
	public class ScribeSaver
	{
		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06001B02 RID: 6914 RVA: 0x000A5009 File Offset: 0x000A3209
		public string CurPath
		{
			get
			{
				return this.curPath;
			}
		}

		// Token: 0x06001B03 RID: 6915 RVA: 0x000A5014 File Offset: 0x000A3214
		public void InitSaving(string filePath, string documentElementName)
		{
			if (Scribe.mode != LoadSaveMode.Inactive)
			{
				Log.Error("Called InitSaving() but current mode is " + Scribe.mode);
				Scribe.ForceStop();
			}
			if (this.curPath != null)
			{
				Log.Error("Current path is not null in InitSaving");
				this.curPath = null;
				this.savedNodes.Clear();
				this.nextListElementTemporaryId = 0;
			}
			try
			{
				Scribe.mode = LoadSaveMode.Saving;
				this.saveStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
				XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
				xmlWriterSettings.Indent = true;
				xmlWriterSettings.IndentChars = "\t";
				this.writer = XmlWriter.Create(this.saveStream, xmlWriterSettings);
				this.writer.WriteStartDocument();
				this.EnterNode(documentElementName);
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception while init saving file: ",
					filePath,
					"\n",
					ex
				}));
				this.ForceStop();
				throw;
			}
		}

		// Token: 0x06001B04 RID: 6916 RVA: 0x000A510C File Offset: 0x000A330C
		public void FinalizeSaving()
		{
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				Log.Error("Called FinalizeSaving() but current mode is " + Scribe.mode);
				return;
			}
			if (this.anyInternalException)
			{
				this.ForceStop();
				throw new Exception("Can't finalize saving due to internal exception. The whole file would be most likely corrupted anyway.");
			}
			try
			{
				if (this.writer != null)
				{
					this.ExitNode();
					this.writer.WriteEndDocument();
					this.writer.Flush();
					this.writer.Close();
					this.writer = null;
				}
				if (this.saveStream != null)
				{
					this.saveStream.Flush();
					this.saveStream.Close();
					this.saveStream = null;
				}
				Scribe.mode = LoadSaveMode.Inactive;
				this.savingForDebug = false;
				this.loadIDsErrorsChecker.CheckForErrorsAndClear();
				this.curPath = null;
				this.savedNodes.Clear();
				this.nextListElementTemporaryId = 0;
				this.anyInternalException = false;
			}
			catch (Exception arg)
			{
				Log.Error("Exception in FinalizeLoading(): " + arg);
				this.ForceStop();
				throw;
			}
		}

		// Token: 0x06001B05 RID: 6917 RVA: 0x000A5214 File Offset: 0x000A3414
		public void WriteElement(string elementName, string value)
		{
			if (this.writer == null)
			{
				Log.Error("Called WriteElemenet(), but writer is null.");
				return;
			}
			try
			{
				this.writer.WriteElementString(elementName, value);
			}
			catch (Exception)
			{
				this.anyInternalException = true;
				throw;
			}
		}

		// Token: 0x06001B06 RID: 6918 RVA: 0x000A5260 File Offset: 0x000A3460
		public void WriteAttribute(string attributeName, string value)
		{
			if (this.writer == null)
			{
				Log.Error("Called WriteAttribute(), but writer is null.");
				return;
			}
			try
			{
				this.writer.WriteAttributeString(attributeName, value);
			}
			catch (Exception)
			{
				this.anyInternalException = true;
				throw;
			}
		}

		// Token: 0x06001B07 RID: 6919 RVA: 0x000A52AC File Offset: 0x000A34AC
		public string DebugOutputFor(IExposable saveable)
		{
			if (Scribe.mode != LoadSaveMode.Inactive)
			{
				Log.Error("DebugOutput needs current mode to be Inactive");
				return "";
			}
			string result;
			try
			{
				using (StringWriter stringWriter = new StringWriter())
				{
					XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
					xmlWriterSettings.Indent = true;
					xmlWriterSettings.IndentChars = "  ";
					xmlWriterSettings.OmitXmlDeclaration = true;
					try
					{
						using (this.writer = XmlWriter.Create(stringWriter, xmlWriterSettings))
						{
							Scribe.mode = LoadSaveMode.Saving;
							this.savingForDebug = true;
							Scribe_Deep.Look<IExposable>(ref saveable, "saveable", Array.Empty<object>());
						}
						result = stringWriter.ToString();
					}
					finally
					{
						this.ForceStop();
					}
				}
			}
			catch (Exception arg)
			{
				Log.Error("Exception while getting debug output: " + arg);
				this.ForceStop();
				result = "";
			}
			return result;
		}

		// Token: 0x06001B08 RID: 6920 RVA: 0x000A53A4 File Offset: 0x000A35A4
		public bool EnterNode(string nodeName)
		{
			if (this.writer == null)
			{
				return false;
			}
			try
			{
				this.writer.WriteStartElement(nodeName);
			}
			catch (Exception)
			{
				this.anyInternalException = true;
				throw;
			}
			return true;
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x000A53E8 File Offset: 0x000A35E8
		public void ExitNode()
		{
			if (this.writer == null)
			{
				return;
			}
			try
			{
				this.writer.WriteEndElement();
			}
			catch (Exception)
			{
				this.anyInternalException = true;
				throw;
			}
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x000A5428 File Offset: 0x000A3628
		public void ForceStop()
		{
			if (this.writer != null)
			{
				this.writer.Close();
				this.writer = null;
			}
			if (this.saveStream != null)
			{
				this.saveStream.Close();
				this.saveStream = null;
			}
			this.savingForDebug = false;
			this.loadIDsErrorsChecker.Clear();
			this.curPath = null;
			this.savedNodes.Clear();
			this.nextListElementTemporaryId = 0;
			this.anyInternalException = false;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				Scribe.mode = LoadSaveMode.Inactive;
			}
		}

		// Token: 0x040013AE RID: 5038
		public DebugLoadIDsSavingErrorsChecker loadIDsErrorsChecker = new DebugLoadIDsSavingErrorsChecker();

		// Token: 0x040013AF RID: 5039
		public bool savingForDebug;

		// Token: 0x040013B0 RID: 5040
		private Stream saveStream;

		// Token: 0x040013B1 RID: 5041
		private XmlWriter writer;

		// Token: 0x040013B2 RID: 5042
		private string curPath;

		// Token: 0x040013B3 RID: 5043
		private HashSet<string> savedNodes = new HashSet<string>();

		// Token: 0x040013B4 RID: 5044
		private int nextListElementTemporaryId;

		// Token: 0x040013B5 RID: 5045
		private bool anyInternalException;
	}
}
