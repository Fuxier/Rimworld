using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000543 RID: 1347
	public static class LogSimple
	{
		// Token: 0x06002955 RID: 10581 RVA: 0x001089C0 File Offset: 0x00106BC0
		public static void Message(string text)
		{
			for (int i = 0; i < LogSimple.tabDepth; i++)
			{
				text = "  " + text;
			}
			LogSimple.messages.Add(text);
		}

		// Token: 0x06002956 RID: 10582 RVA: 0x001089F5 File Offset: 0x00106BF5
		public static void BeginTabMessage(string text)
		{
			LogSimple.Message(text);
			LogSimple.tabDepth++;
		}

		// Token: 0x06002957 RID: 10583 RVA: 0x00108A09 File Offset: 0x00106C09
		public static void EndTab()
		{
			LogSimple.tabDepth--;
		}

		// Token: 0x06002958 RID: 10584 RVA: 0x00108A18 File Offset: 0x00106C18
		public static void FlushToFileAndOpen()
		{
			if (LogSimple.messages.Count == 0)
			{
				return;
			}
			string value = LogSimple.CompiledLog();
			string path = GenFilePaths.SaveDataFolderPath + Path.DirectorySeparatorChar.ToString() + "LogSimple.txt";
			using (StreamWriter streamWriter = new StreamWriter(path, false))
			{
				streamWriter.Write(value);
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				Application.OpenURL(path);
			});
			LogSimple.messages.Clear();
		}

		// Token: 0x06002959 RID: 10585 RVA: 0x00108AAC File Offset: 0x00106CAC
		public static void FlushToStandardLog()
		{
			if (LogSimple.messages.Count == 0)
			{
				return;
			}
			Log.Message(LogSimple.CompiledLog());
			LogSimple.messages.Clear();
		}

		// Token: 0x0600295A RID: 10586 RVA: 0x00108AD0 File Offset: 0x00106CD0
		private static string CompiledLog()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string value in LogSimple.messages)
			{
				stringBuilder.AppendLine(value);
			}
			return stringBuilder.ToString().TrimEnd(Array.Empty<char>());
		}

		// Token: 0x04001B54 RID: 6996
		private static List<string> messages = new List<string>();

		// Token: 0x04001B55 RID: 6997
		private static int tabDepth = 0;
	}
}
