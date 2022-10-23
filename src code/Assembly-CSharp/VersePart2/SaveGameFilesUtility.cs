using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Verse
{
	// Token: 0x020003A8 RID: 936
	public static class SaveGameFilesUtility
	{
		// Token: 0x06001AC8 RID: 6856 RVA: 0x000A2D58 File Offset: 0x000A0F58
		public static bool IsAutoSave(string fileName)
		{
			return fileName.Length >= 8 && fileName.Substring(0, 8) == "Autosave";
		}

		// Token: 0x06001AC9 RID: 6857 RVA: 0x000A2D78 File Offset: 0x000A0F78
		public static bool SavedGameNamedExists(string fileName)
		{
			using (IEnumerator<string> enumerator = (from f in GenFilePaths.AllSavedGameFiles
			select Path.GetFileNameWithoutExtension(f.Name)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == fileName)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001ACA RID: 6858 RVA: 0x000A2DF0 File Offset: 0x000A0FF0
		public static string UnusedDefaultFileName(string factionLabel)
		{
			int num = 1;
			string text;
			do
			{
				text = factionLabel + num.ToString();
				num++;
			}
			while (SaveGameFilesUtility.SavedGameNamedExists(text));
			return text;
		}

		// Token: 0x06001ACB RID: 6859 RVA: 0x000A2E20 File Offset: 0x000A1020
		public static FileInfo GetAutostartSaveFile()
		{
			if (!Prefs.DevMode)
			{
				return null;
			}
			return GenFilePaths.AllSavedGameFiles.FirstOrDefault((FileInfo x) => Path.GetFileNameWithoutExtension(x.Name).ToLower() == "autostart".ToLower());
		}
	}
}
