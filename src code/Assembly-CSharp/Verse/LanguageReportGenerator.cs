using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000186 RID: 390
	public static class LanguageReportGenerator
	{
		// Token: 0x06000AAF RID: 2735 RVA: 0x00039BE4 File Offset: 0x00037DE4
		public static void SaveTranslationReport()
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			if (activeLanguage == defaultLanguage && !defaultLanguage.anyError)
			{
				Messages.Message("Please activate a non-English language to scan.", MessageTypeDefOf.RejectInput, false);
				return;
			}
			activeLanguage.LoadData();
			defaultLanguage.LoadData();
			LongEventHandler.QueueLongEvent(new Action(LanguageReportGenerator.DoSaveTranslationReport), "GeneratingTranslationReport", true, null, true);
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x00039C40 File Offset: 0x00037E40
		private static void DoSaveTranslationReport()
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Translation report for " + activeLanguage);
			if (activeLanguage.defInjections.Any((DefInjectionPackage x) => x.usedOldRepSyntax))
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Consider using <Something.Field.Example.Etc>translation</Something.Field.Example.Etc> def-injection syntax instead of <rep>.");
			}
			try
			{
				LanguageReportGenerator.AppendGeneralLoadErrors(stringBuilder);
			}
			catch (Exception arg)
			{
				Log.Error("Error while generating translation report (general load errors): " + arg);
			}
			try
			{
				LanguageReportGenerator.AppendDefInjectionsLoadErros(stringBuilder);
			}
			catch (Exception arg2)
			{
				Log.Error("Error while generating translation report (def-injections load errors): " + arg2);
			}
			try
			{
				LanguageReportGenerator.AppendMissingKeyedTranslations(stringBuilder);
			}
			catch (Exception arg3)
			{
				Log.Error("Error while generating translation report (missing keyed translations): " + arg3);
			}
			List<string> list = new List<string>();
			try
			{
				LanguageReportGenerator.AppendMissingDefInjections(stringBuilder, list);
			}
			catch (Exception arg4)
			{
				Log.Error("Error while generating translation report (missing def-injections): " + arg4);
			}
			try
			{
				LanguageReportGenerator.AppendMissingBackstories(stringBuilder);
			}
			catch (Exception arg5)
			{
				Log.Error("Error while generating translation report (missing backstories): " + arg5);
			}
			try
			{
				LanguageReportGenerator.AppendUnnecessaryDefInjections(stringBuilder, list);
			}
			catch (Exception arg6)
			{
				Log.Error("Error while generating translation report (unnecessary def-injections): " + arg6);
			}
			try
			{
				LanguageReportGenerator.AppendRenamedDefInjections(stringBuilder);
			}
			catch (Exception arg7)
			{
				Log.Error("Error while generating translation report (renamed def-injections): " + arg7);
			}
			try
			{
				LanguageReportGenerator.AppendArgumentCountMismatches(stringBuilder);
			}
			catch (Exception arg8)
			{
				Log.Error("Error while generating translation report (argument count mismatches): " + arg8);
			}
			try
			{
				LanguageReportGenerator.AppendUnnecessaryKeyedTranslations(stringBuilder);
			}
			catch (Exception arg9)
			{
				Log.Error("Error while generating translation report (unnecessary keyed translations): " + arg9);
			}
			try
			{
				LanguageReportGenerator.AppendKeyedTranslationsMatchingEnglish(stringBuilder);
			}
			catch (Exception arg10)
			{
				Log.Error("Error while generating translation report (keyed translations matching English): " + arg10);
			}
			try
			{
				LanguageReportGenerator.AppendBackstoriesMatchingEnglish(stringBuilder);
			}
			catch (Exception arg11)
			{
				Log.Error("Error while generating translation report (backstories matching English): " + arg11);
			}
			try
			{
				LanguageReportGenerator.AppendDefInjectionsSyntaxSuggestions(stringBuilder);
			}
			catch (Exception arg12)
			{
				Log.Error("Error while generating translation report (def-injections syntax suggestions): " + arg12);
			}
			try
			{
				LanguageReportGenerator.AppendTKeySystemErrors(stringBuilder);
			}
			catch (Exception arg13)
			{
				Log.Error("Error while generating translation report (TKeySystem errors): " + arg13);
			}
			try
			{
				LanguageReportGenerator.AppendObsoleteBackstoryTranslations(stringBuilder);
			}
			catch (Exception arg14)
			{
				Log.Error("Error while generating translation report (backstory syntax suggestions): " + arg14);
			}
			string text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			if (text.NullOrEmpty())
			{
				text = GenFilePaths.SaveDataFolderPath;
			}
			text = Path.Combine(text, "TranslationReport.txt");
			File.WriteAllText(text, stringBuilder.ToString());
			Messages.Message("MessageTranslationReportSaved".Translate(Path.GetFullPath(text)), MessageTypeDefOf.TaskCompletion, false);
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x00039F54 File Offset: 0x00038154
		private static void AppendGeneralLoadErrors(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string value in activeLanguage.loadErrors)
			{
				num++;
				stringBuilder.AppendLine(value);
			}
			sb.AppendLine();
			sb.AppendLine("========== General load errors (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x00039FE4 File Offset: 0x000381E4
		private static void AppendDefInjectionsLoadErros(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (DefInjectionPackage defInjectionPackage in activeLanguage.defInjections)
			{
				foreach (string value in defInjectionPackage.loadErrors)
				{
					num++;
					stringBuilder.AppendLine(value);
				}
			}
			sb.AppendLine();
			sb.AppendLine("========== Def-injected translations load errors (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x0003A0B0 File Offset: 0x000382B0
		private static void AppendMissingKeyedTranslations(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			if (activeLanguage == defaultLanguage)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (KeyValuePair<string, LoadedLanguage.KeyedReplacement> keyValuePair in defaultLanguage.keyedReplacements)
			{
				if (!activeLanguage.HaveTextForKey(keyValuePair.Key, false))
				{
					string text = string.Concat(new string[]
					{
						keyValuePair.Key,
						" '",
						keyValuePair.Value.value.Replace("\n", "\\n"),
						"' (English file: ",
						defaultLanguage.GetKeySourceFileAndLine(keyValuePair.Key),
						")"
					});
					if (activeLanguage.HaveTextForKey(keyValuePair.Key, true))
					{
						text = text + " (placeholder exists in " + activeLanguage.GetKeySourceFileAndLine(keyValuePair.Key) + ")";
					}
					num++;
					stringBuilder.AppendLine(text);
				}
			}
			sb.AppendLine();
			sb.AppendLine("========== Missing keyed translations (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x0003A1F8 File Offset: 0x000383F8
		private static void AppendMissingDefInjections(StringBuilder sb, List<string> outUnnecessaryDefInjections)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			if (activeLanguage == defaultLanguage)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (DefInjectionPackage defInjectionPackage in activeLanguage.defInjections)
			{
				foreach (string str in defInjectionPackage.MissingInjections(outUnnecessaryDefInjections))
				{
					num++;
					stringBuilder.AppendLine(defInjectionPackage.defType.Name + ": " + str);
				}
			}
			sb.AppendLine();
			sb.AppendLine("========== Def-injected translations missing (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x0003A2EC File Offset: 0x000384EC
		private static void AppendMissingBackstories(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			if (activeLanguage == defaultLanguage)
			{
				return;
			}
			if (!BackstoryTranslationUtility.AnyLegacyBackstoryFiles(activeLanguage.AllDirectories))
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string value in BackstoryTranslationUtility.MissingBackstoryTranslations(activeLanguage))
			{
				num++;
				stringBuilder.AppendLine(value);
			}
			sb.AppendLine();
			sb.AppendLine("========== Backstory translations missing (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x0003A39C File Offset: 0x0003859C
		private static void AppendUnnecessaryDefInjections(StringBuilder sb, List<string> unnecessaryDefInjections)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string value in unnecessaryDefInjections)
			{
				num++;
				stringBuilder.AppendLine(value);
			}
			sb.AppendLine();
			sb.AppendLine("========== Unnecessary def-injected translations (marked as NoTranslate) (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x0003A424 File Offset: 0x00038624
		private static void AppendRenamedDefInjections(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (DefInjectionPackage defInjectionPackage in activeLanguage.defInjections)
			{
				foreach (KeyValuePair<string, DefInjectionPackage.DefInjection> keyValuePair in defInjectionPackage.injections)
				{
					if (!(keyValuePair.Value.path == keyValuePair.Value.nonBackCompatiblePath))
					{
						string text = keyValuePair.Value.nonBackCompatiblePath.Split(new char[]
						{
							'.'
						})[0];
						string text2 = keyValuePair.Value.path.Split(new char[]
						{
							'.'
						})[0];
						if (text != text2)
						{
							stringBuilder.AppendLine(string.Concat(new string[]
							{
								"Def has been renamed: ",
								text,
								" -> ",
								text2,
								", translation ",
								keyValuePair.Value.nonBackCompatiblePath,
								" should be renamed as well."
							}));
						}
						else
						{
							stringBuilder.AppendLine("Translation " + keyValuePair.Value.nonBackCompatiblePath + " should be renamed to " + keyValuePair.Value.path);
						}
						num++;
					}
				}
			}
			sb.AppendLine();
			sb.AppendLine("========== Def-injected translations using old, renamed defs (fixed automatically but can break in the next RimWorld version) (" + num + ") =========");
			sb.Append(stringBuilder);
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x0003A5F4 File Offset: 0x000387F4
		private static void AppendArgumentCountMismatches(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			if (activeLanguage == defaultLanguage)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string text in defaultLanguage.keyedReplacements.Keys.Intersect(activeLanguage.keyedReplacements.Keys))
			{
				if (!activeLanguage.keyedReplacements[text].isPlaceholder && !LanguageReportGenerator.SameSimpleGrammarResolverSymbols(defaultLanguage.keyedReplacements[text].value, activeLanguage.keyedReplacements[text].value))
				{
					num++;
					stringBuilder.AppendLine(string.Format("{0} ({1})\n  - '{2}'\n  - '{3}'", new object[]
					{
						text,
						activeLanguage.GetKeySourceFileAndLine(text),
						defaultLanguage.keyedReplacements[text].value.Replace("\n", "\\n"),
						activeLanguage.keyedReplacements[text].value.Replace("\n", "\\n")
					}));
				}
			}
			sb.AppendLine();
			sb.AppendLine("========== Argument count mismatches (may or may not be incorrect) (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x0003A754 File Offset: 0x00038954
		private static void AppendUnnecessaryKeyedTranslations(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			if (activeLanguage == defaultLanguage)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (KeyValuePair<string, LoadedLanguage.KeyedReplacement> keyValuePair in activeLanguage.keyedReplacements)
			{
				if (!defaultLanguage.HaveTextForKey(keyValuePair.Key, false))
				{
					num++;
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						keyValuePair.Key,
						" '",
						keyValuePair.Value.value.Replace("\n", "\\n"),
						"' (",
						activeLanguage.GetKeySourceFileAndLine(keyValuePair.Key),
						")"
					}));
				}
			}
			sb.AppendLine();
			sb.AppendLine("========== Unnecessary keyed translations (will never be used) (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x0003A860 File Offset: 0x00038A60
		private static void AppendKeyedTranslationsMatchingEnglish(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			if (activeLanguage == defaultLanguage)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (KeyValuePair<string, LoadedLanguage.KeyedReplacement> keyValuePair in activeLanguage.keyedReplacements)
			{
				TaggedString taggedString;
				if (!keyValuePair.Value.isPlaceholder && defaultLanguage.TryGetTextFromKey(keyValuePair.Key, out taggedString) && keyValuePair.Value.value == taggedString)
				{
					num++;
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						keyValuePair.Key,
						" '",
						keyValuePair.Value.value.Replace("\n", "\\n"),
						"' (",
						activeLanguage.GetKeySourceFileAndLine(keyValuePair.Key),
						")"
					}));
				}
			}
			sb.AppendLine();
			sb.AppendLine("========== Keyed translations matching English (maybe ok) (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x0003A9A0 File Offset: 0x00038BA0
		private static void AppendBackstoriesMatchingEnglish(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			if (activeLanguage == defaultLanguage)
			{
				return;
			}
			if (!BackstoryTranslationUtility.AnyLegacyBackstoryFiles(activeLanguage.AllDirectories))
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string value in BackstoryTranslationUtility.BackstoryTranslationsMatchingEnglish(activeLanguage))
			{
				num++;
				stringBuilder.AppendLine(value);
			}
			sb.AppendLine();
			sb.AppendLine("========== Backstory translations matching English (maybe ok) (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x0003AA50 File Offset: 0x00038C50
		private static void AppendObsoleteBackstoryTranslations(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			if (activeLanguage == defaultLanguage)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string value in BackstoryTranslationUtility.ObsoleteBackstoryTranslations(activeLanguage))
			{
				num++;
				stringBuilder.AppendLine(value);
			}
			sb.AppendLine();
			sb.AppendLine("========== Backstories translation using obsolete format (def injection is now enabled for backstories) (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x0003AAF0 File Offset: 0x00038CF0
		private static void AppendDefInjectionsSyntaxSuggestions(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (DefInjectionPackage defInjectionPackage in activeLanguage.defInjections)
			{
				foreach (string value in defInjectionPackage.loadSyntaxSuggestions)
				{
					num++;
					stringBuilder.AppendLine(value);
				}
			}
			if (num == 0)
			{
				return;
			}
			sb.AppendLine();
			sb.AppendLine("========== Def-injected translations syntax suggestions (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x0003ABC0 File Offset: 0x00038DC0
		private static void AppendTKeySystemErrors(StringBuilder sb)
		{
			if (TKeySystem.loadErrors.Count == 0)
			{
				return;
			}
			sb.AppendLine();
			sb.AppendLine("========== TKey system errors (" + TKeySystem.loadErrors.Count + ") ==========");
			sb.Append(string.Join("\r\n", TKeySystem.loadErrors));
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x0003AC1C File Offset: 0x00038E1C
		public static bool SameSimpleGrammarResolverSymbols(string str1, string str2)
		{
			LanguageReportGenerator.tmpStr1Symbols.Clear();
			LanguageReportGenerator.tmpStr2Symbols.Clear();
			LanguageReportGenerator.CalculateSimpleGrammarResolverSymbols(str1, LanguageReportGenerator.tmpStr1Symbols);
			LanguageReportGenerator.CalculateSimpleGrammarResolverSymbols(str2, LanguageReportGenerator.tmpStr2Symbols);
			for (int i = 0; i < LanguageReportGenerator.tmpStr1Symbols.Count; i++)
			{
				if (!LanguageReportGenerator.tmpStr2Symbols.Contains(LanguageReportGenerator.tmpStr1Symbols[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x0003AC84 File Offset: 0x00038E84
		private static void CalculateSimpleGrammarResolverSymbols(string str, List<string> outSymbols)
		{
			outSymbols.Clear();
			for (int i = 0; i < str.Length; i++)
			{
				if (str[i] == '{')
				{
					LanguageReportGenerator.tmpSymbol.Length = 0;
					bool flag = false;
					bool flag2 = false;
					bool flag3 = false;
					for (i++; i < str.Length; i++)
					{
						char c = str[i];
						if (c == '}')
						{
							flag = true;
							break;
						}
						if (c == '_')
						{
							flag2 = true;
						}
						else if (c == '?')
						{
							flag3 = true;
						}
						else if (!flag2 && !flag3)
						{
							LanguageReportGenerator.tmpSymbol.Append(c);
						}
					}
					if (flag)
					{
						outSymbols.Add(LanguageReportGenerator.tmpSymbol.ToString().Trim());
					}
				}
			}
		}

		// Token: 0x04000A80 RID: 2688
		private const string FileName = "TranslationReport.txt";

		// Token: 0x04000A81 RID: 2689
		private static List<string> tmpStr1Symbols = new List<string>();

		// Token: 0x04000A82 RID: 2690
		private static List<string> tmpStr2Symbols = new List<string>();

		// Token: 0x04000A83 RID: 2691
		private static StringBuilder tmpSymbol = new StringBuilder();
	}
}
