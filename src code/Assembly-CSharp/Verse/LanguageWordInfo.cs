using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RimWorld.IO;

namespace Verse
{
	// Token: 0x02000187 RID: 391
	public class LanguageWordInfo
	{
		// Token: 0x06000AC2 RID: 2754 RVA: 0x0003AD50 File Offset: 0x00038F50
		public void LoadFrom(Tuple<VirtualDirectory, ModContentPack, string> dir, LoadedLanguage lang)
		{
			VirtualDirectory directory = dir.Item1.GetDirectory("WordInfo").GetDirectory("Gender");
			this.TryLoadFromFile(directory.GetFile("Male.txt"), Gender.Male, dir, lang);
			this.TryLoadFromFile(directory.GetFile("Female.txt"), Gender.Female, dir, lang);
			this.TryLoadFromFile(directory.GetFile("Neuter.txt"), Gender.None, dir, lang);
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x0003ADB4 File Offset: 0x00038FB4
		public Gender ResolveGender(string str, string fallback = null, Gender defaultGender = Gender.Male)
		{
			if (str == null)
			{
				return defaultGender;
			}
			Gender result;
			if (this.TryResolveGender(str, out result))
			{
				return result;
			}
			if (fallback != null && this.TryResolveGender(str, out result))
			{
				return result;
			}
			return defaultGender;
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x0003ADE4 File Offset: 0x00038FE4
		private bool TryResolveGender(string str, out Gender gender)
		{
			LanguageWordInfo.tmpLowercase.Length = 0;
			for (int i = 0; i < str.Length; i++)
			{
				LanguageWordInfo.tmpLowercase.Append(char.ToLower(str[i]));
			}
			string key = LanguageWordInfo.tmpLowercase.ToString();
			if (this.genders.TryGetValue(key, out gender))
			{
				return true;
			}
			gender = Gender.Male;
			return false;
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x0003AE44 File Offset: 0x00039044
		private void TryLoadFromFile(VirtualFile file, Gender gender, Tuple<VirtualDirectory, ModContentPack, string> dir, LoadedLanguage lang)
		{
			string[] array;
			try
			{
				array = file.ReadAllLines();
			}
			catch (DirectoryNotFoundException)
			{
				return;
			}
			catch (FileNotFoundException)
			{
				return;
			}
			if (!lang.TryRegisterFileIfNew(dir, file.FullPath))
			{
				return;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].NullOrEmpty() && !this.genders.ContainsKey(array[i]))
				{
					this.genders.Add(array[i], gender);
				}
			}
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x0003AEC4 File Offset: 0x000390C4
		public void RegisterLut(string name)
		{
			if (this.lookupTables.ContainsKey(name.ToLower()))
			{
				Log.Error("Tried registering language look up table named " + name + " twice.");
				return;
			}
			Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			try
			{
				foreach (Tuple<VirtualDirectory, ModContentPack, string> tuple in activeLanguage.AllDirectories)
				{
					VirtualFile file = tuple.Item1.GetDirectory("WordInfo").GetFile(name + ".txt");
					if (file.Exists)
					{
						foreach (string text in GenText.LinesFromString(file.ReadAllText()))
						{
							string[] array;
							if (GenText.TryGetSeparatedValues(text, ';', out array))
							{
								string key = array[0].ToLower();
								if (!dictionary.ContainsKey(key))
								{
									dictionary.Add(key, array);
								}
							}
							else
							{
								Log.ErrorOnce(string.Concat(new string[]
								{
									"Failed parsing lookup items from line ",
									text,
									" in ",
									file.FullPath,
									". Line: ",
									text
								}), name.GetHashCode() ^ 1857221523);
							}
						}
					}
				}
			}
			catch (Exception arg)
			{
				Log.Error("Exception parsing a language lookup table: " + arg);
			}
			this.lookupTables.Add(name.ToLower(), dictionary);
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x0003B080 File Offset: 0x00039280
		public Dictionary<string, string[]> GetLookupTable(string name)
		{
			string text = name.ToLower();
			if (this.lookupTables.ContainsKey(text))
			{
				return this.lookupTables[text];
			}
			this.RegisterLut(text);
			if (this.lookupTables.ContainsKey(text))
			{
				return this.lookupTables[text];
			}
			return null;
		}

		// Token: 0x04000A84 RID: 2692
		private Dictionary<string, Gender> genders = new Dictionary<string, Gender>();

		// Token: 0x04000A85 RID: 2693
		private Dictionary<string, Dictionary<string, string[]>> lookupTables = new Dictionary<string, Dictionary<string, string[]>>();

		// Token: 0x04000A86 RID: 2694
		private const string FolderName = "WordInfo";

		// Token: 0x04000A87 RID: 2695
		private const string GendersFolderName = "Gender";

		// Token: 0x04000A88 RID: 2696
		private const string MaleFileName = "Male.txt";

		// Token: 0x04000A89 RID: 2697
		private const string FemaleFileName = "Female.txt";

		// Token: 0x04000A8A RID: 2698
		private const string NeuterFileName = "Neuter.txt";

		// Token: 0x04000A8B RID: 2699
		private static StringBuilder tmpLowercase = new StringBuilder();
	}
}
