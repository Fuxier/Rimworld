using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Verse
{
	// Token: 0x02000579 RID: 1401
	public class KeyPrefs
	{
		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x06002AFB RID: 11003 RVA: 0x00112DA3 File Offset: 0x00110FA3
		// (set) Token: 0x06002AFC RID: 11004 RVA: 0x00112DAA File Offset: 0x00110FAA
		public static KeyPrefsData KeyPrefsData
		{
			get
			{
				return KeyPrefs.data;
			}
			set
			{
				KeyPrefs.data = value;
			}
		}

		// Token: 0x06002AFD RID: 11005 RVA: 0x00112DB4 File Offset: 0x00110FB4
		public static void Init()
		{
			bool flag = !new FileInfo(GenFilePaths.KeyPrefsFilePath).Exists;
			Dictionary<string, KeyBindingData> dictionary = DirectXmlLoader.ItemFromXmlFile<Dictionary<string, KeyBindingData>>(GenFilePaths.KeyPrefsFilePath, true);
			KeyPrefs.data = new KeyPrefsData();
			KeyPrefs.unresolvedBindings = new Dictionary<string, KeyBindingData>();
			foreach (KeyValuePair<string, KeyBindingData> keyValuePair in dictionary)
			{
				KeyBindingDef namedSilentFail = DefDatabase<KeyBindingDef>.GetNamedSilentFail(keyValuePair.Key);
				if (namedSilentFail != null)
				{
					KeyPrefs.data.keyPrefs[namedSilentFail] = keyValuePair.Value;
				}
				else
				{
					KeyPrefs.unresolvedBindings[keyValuePair.Key] = keyValuePair.Value;
				}
			}
			if (flag)
			{
				KeyPrefs.data.ResetToDefaults();
			}
			KeyPrefs.data.AddMissingDefaultBindings();
			KeyPrefs.data.ErrorCheck();
			if (flag)
			{
				KeyPrefs.Save();
			}
		}

		// Token: 0x06002AFE RID: 11006 RVA: 0x00112E98 File Offset: 0x00111098
		public static void Save()
		{
			try
			{
				Dictionary<string, KeyBindingData> dictionary = new Dictionary<string, KeyBindingData>();
				foreach (KeyValuePair<KeyBindingDef, KeyBindingData> keyValuePair in KeyPrefs.data.keyPrefs)
				{
					dictionary[keyValuePair.Key.defName] = keyValuePair.Value;
				}
				foreach (KeyValuePair<string, KeyBindingData> keyValuePair2 in KeyPrefs.unresolvedBindings)
				{
					try
					{
						dictionary.Add(keyValuePair2.Key, keyValuePair2.Value);
					}
					catch (ArgumentException)
					{
					}
				}
				XDocument xdocument = new XDocument();
				XElement content = DirectXmlSaver.XElementFromObject(dictionary, typeof(KeyPrefsData));
				xdocument.Add(content);
				xdocument.Save(GenFilePaths.KeyPrefsFilePath);
			}
			catch (Exception ex)
			{
				GenUI.ErrorDialog("ProblemSavingFile".Translate(GenFilePaths.KeyPrefsFilePath, ex.ToString()));
				Log.Error("Exception saving keyprefs: " + ex);
			}
		}

		// Token: 0x04001C13 RID: 7187
		private static KeyPrefsData data;

		// Token: 0x04001C14 RID: 7188
		private static Dictionary<string, KeyBindingData> unresolvedBindings;

		// Token: 0x02002141 RID: 8513
		public enum BindingSlot : byte
		{
			// Token: 0x040083E9 RID: 33769
			A,
			// Token: 0x040083EA RID: 33770
			B
		}
	}
}
