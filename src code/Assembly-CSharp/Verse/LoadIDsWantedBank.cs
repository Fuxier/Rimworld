using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x020003AE RID: 942
	public class LoadIDsWantedBank
	{
		// Token: 0x06001AD6 RID: 6870 RVA: 0x000A3188 File Offset: 0x000A1388
		public void ConfirmClear()
		{
			if (this.idsRead.Count > 0 || this.idListsRead.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Not all loadIDs which were read were consumed.");
				if (this.idsRead.Count > 0)
				{
					stringBuilder.AppendLine("Singles:");
					foreach (KeyValuePair<ValueTuple<IExposable, string>, LoadIDsWantedBank.IdRecord> keyValuePair in this.idsRead)
					{
						stringBuilder.AppendLine(string.Concat(new object[]
						{
							"  ",
							keyValuePair.Value.targetLoadID.ToStringSafe<string>(),
							" of type ",
							keyValuePair.Value.targetType,
							". pathRelToParent=",
							keyValuePair.Value.pathRelToParent,
							", parent=",
							keyValuePair.Value.parent.ToStringSafe<IExposable>()
						}));
					}
				}
				if (this.idListsRead.Count > 0)
				{
					stringBuilder.AppendLine("Lists:");
					foreach (KeyValuePair<ValueTuple<IExposable, string>, LoadIDsWantedBank.IdListRecord> keyValuePair2 in this.idListsRead)
					{
						stringBuilder.AppendLine(string.Concat(new object[]
						{
							"  List with ",
							(keyValuePair2.Value.targetLoadIDs != null) ? keyValuePair2.Value.targetLoadIDs.Count : 0,
							" elements. pathRelToParent=",
							keyValuePair2.Value.pathRelToParent,
							", parent=",
							keyValuePair2.Value.parent.ToStringSafe<IExposable>()
						}));
					}
				}
				Log.Warning(stringBuilder.ToString().TrimEndNewlines());
			}
			this.Clear();
		}

		// Token: 0x06001AD7 RID: 6871 RVA: 0x000A338C File Offset: 0x000A158C
		public void Clear()
		{
			this.idsRead.Clear();
			this.idListsRead.Clear();
		}

		// Token: 0x06001AD8 RID: 6872 RVA: 0x000A33A4 File Offset: 0x000A15A4
		public void RegisterLoadIDReadFromXml(string targetLoadID, Type targetType, string pathRelToParent, IExposable parent)
		{
			if (this.idsRead.ContainsKey(new ValueTuple<IExposable, string>(parent, pathRelToParent)))
			{
				Log.Error(string.Concat(new string[]
				{
					"Tried to register the same load ID twice: ",
					targetLoadID,
					", pathRelToParent=",
					pathRelToParent,
					", parent=",
					parent.ToStringSafe<IExposable>()
				}));
				return;
			}
			LoadIDsWantedBank.IdRecord value = new LoadIDsWantedBank.IdRecord(targetLoadID, targetType, pathRelToParent, parent);
			this.idsRead.Add(new ValueTuple<IExposable, string>(parent, pathRelToParent), value);
		}

		// Token: 0x06001AD9 RID: 6873 RVA: 0x000A3424 File Offset: 0x000A1624
		public void RegisterLoadIDReadFromXml(string targetLoadID, Type targetType, string toAppendToPathRelToParent)
		{
			string text = Scribe.loader.curPathRelToParent;
			if (!toAppendToPathRelToParent.NullOrEmpty())
			{
				text = text + "/" + toAppendToPathRelToParent;
			}
			this.RegisterLoadIDReadFromXml(targetLoadID, targetType, text, Scribe.loader.curParent);
		}

		// Token: 0x06001ADA RID: 6874 RVA: 0x000A3464 File Offset: 0x000A1664
		public void RegisterLoadIDListReadFromXml(List<string> targetLoadIDList, string pathRelToParent, IExposable parent)
		{
			if (this.idListsRead.ContainsKey(new ValueTuple<IExposable, string>(parent, pathRelToParent)))
			{
				Log.Error("Tried to register the same list of load IDs twice. pathRelToParent=" + pathRelToParent + ", parent=" + parent.ToStringSafe<IExposable>());
				return;
			}
			LoadIDsWantedBank.IdListRecord value = new LoadIDsWantedBank.IdListRecord(targetLoadIDList, pathRelToParent, parent);
			this.idListsRead.Add(new ValueTuple<IExposable, string>(parent, pathRelToParent), value);
		}

		// Token: 0x06001ADB RID: 6875 RVA: 0x000A34C0 File Offset: 0x000A16C0
		public void RegisterLoadIDListReadFromXml(List<string> targetLoadIDList, string toAppendToPathRelToParent)
		{
			string text = Scribe.loader.curPathRelToParent;
			if (!toAppendToPathRelToParent.NullOrEmpty())
			{
				text = text + "/" + toAppendToPathRelToParent;
			}
			this.RegisterLoadIDListReadFromXml(targetLoadIDList, text, Scribe.loader.curParent);
		}

		// Token: 0x06001ADC RID: 6876 RVA: 0x000A3500 File Offset: 0x000A1700
		public string Take<T>(string pathRelToParent, IExposable parent)
		{
			LoadIDsWantedBank.IdRecord idRecord;
			if (this.idsRead.TryGetValue(new ValueTuple<IExposable, string>(parent, pathRelToParent), out idRecord))
			{
				string targetLoadID = idRecord.targetLoadID;
				if (typeof(T) != idRecord.targetType)
				{
					Log.Error(string.Concat(new object[]
					{
						"Trying to get load ID of object of type ",
						typeof(T),
						", but it was registered as ",
						idRecord.targetType,
						". pathRelToParent=",
						pathRelToParent,
						", parent=",
						parent.ToStringSafe<IExposable>()
					}));
				}
				this.idsRead.Remove(new ValueTuple<IExposable, string>(parent, pathRelToParent));
				return targetLoadID;
			}
			Log.Error("Could not get load ID. We're asking for something which was never added during LoadingVars. pathRelToParent=" + pathRelToParent + ", parent=" + parent.ToStringSafe<IExposable>());
			return null;
		}

		// Token: 0x06001ADD RID: 6877 RVA: 0x000A35C8 File Offset: 0x000A17C8
		public List<string> TakeList(string pathRelToParent, IExposable parent)
		{
			LoadIDsWantedBank.IdListRecord idListRecord;
			if (this.idListsRead.TryGetValue(new ValueTuple<IExposable, string>(parent, pathRelToParent), out idListRecord))
			{
				List<string> targetLoadIDs = idListRecord.targetLoadIDs;
				this.idListsRead.Remove(new ValueTuple<IExposable, string>(parent, pathRelToParent));
				return targetLoadIDs;
			}
			Log.Error("Could not get load IDs list. We're asking for something which was never added during LoadingVars. pathRelToParent=" + pathRelToParent + ", parent=" + parent.ToStringSafe<IExposable>());
			return new List<string>();
		}

		// Token: 0x04001396 RID: 5014
		private Dictionary<ValueTuple<IExposable, string>, LoadIDsWantedBank.IdRecord> idsRead = new Dictionary<ValueTuple<IExposable, string>, LoadIDsWantedBank.IdRecord>();

		// Token: 0x04001397 RID: 5015
		private Dictionary<ValueTuple<IExposable, string>, LoadIDsWantedBank.IdListRecord> idListsRead = new Dictionary<ValueTuple<IExposable, string>, LoadIDsWantedBank.IdListRecord>();

		// Token: 0x02001E85 RID: 7813
		private struct IdRecord
		{
			// Token: 0x0600B94C RID: 47436 RVA: 0x0041F229 File Offset: 0x0041D429
			public IdRecord(string targetLoadID, Type targetType, string pathRelToParent, IExposable parent)
			{
				this.targetLoadID = targetLoadID;
				this.targetType = targetType;
				this.pathRelToParent = pathRelToParent;
				this.parent = parent;
			}

			// Token: 0x04007822 RID: 30754
			public string targetLoadID;

			// Token: 0x04007823 RID: 30755
			public Type targetType;

			// Token: 0x04007824 RID: 30756
			public string pathRelToParent;

			// Token: 0x04007825 RID: 30757
			public IExposable parent;
		}

		// Token: 0x02001E86 RID: 7814
		private struct IdListRecord
		{
			// Token: 0x0600B94D RID: 47437 RVA: 0x0041F248 File Offset: 0x0041D448
			public IdListRecord(List<string> targetLoadIDs, string pathRelToParent, IExposable parent)
			{
				this.targetLoadIDs = targetLoadIDs;
				this.pathRelToParent = pathRelToParent;
				this.parent = parent;
			}

			// Token: 0x04007826 RID: 30758
			public List<string> targetLoadIDs;

			// Token: 0x04007827 RID: 30759
			public string pathRelToParent;

			// Token: 0x04007828 RID: 30760
			public IExposable parent;
		}
	}
}
