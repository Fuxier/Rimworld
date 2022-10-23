using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Verse
{
	// Token: 0x02000560 RID: 1376
	public static class GenCommandLine
	{
		// Token: 0x06002A32 RID: 10802 RVA: 0x0010D4FC File Offset: 0x0010B6FC
		public static bool CommandLineArgPassed(string key)
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				if (string.Compare(commandLineArgs[i], key, true) == 0 || string.Compare(commandLineArgs[i], "-" + key, true) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002A33 RID: 10803 RVA: 0x0010D544 File Offset: 0x0010B744
		public static bool TryGetCommandLineArg(string key, out string value)
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				if (commandLineArgs[i].Contains('='))
				{
					string[] array = commandLineArgs[i].Split(new char[]
					{
						'='
					});
					if (array.Length == 2 && (string.Compare(array[0], key, true) == 0 || string.Compare(array[0], "-" + key, true) == 0))
					{
						value = array[1];
						return true;
					}
				}
			}
			value = null;
			return false;
		}

		// Token: 0x06002A34 RID: 10804 RVA: 0x0010D5B8 File Offset: 0x0010B7B8
		public static void Restart()
		{
			try
			{
				string[] commandLineArgs = Environment.GetCommandLineArgs();
				string fileName = commandLineArgs[0];
				string text = "";
				for (int i = 1; i < commandLineArgs.Length; i++)
				{
					if (!text.NullOrEmpty())
					{
						text += " ";
					}
					text = text + "\"" + commandLineArgs[i].Replace("\"", "\\\"") + "\"";
				}
				new Process
				{
					StartInfo = new ProcessStartInfo(fileName, text)
				}.Start();
				Root.Shutdown();
				LongEventHandler.QueueLongEvent(delegate()
				{
					Thread.Sleep(10000);
				}, "Restarting", true, null, true);
			}
			catch (Exception arg)
			{
				Log.Error("Error restarting: " + arg);
				Find.WindowStack.Add(new Dialog_MessageBox("FailedToRestart".Translate(), null, null, null, null, null, false, null, null, WindowLayer.Dialog));
			}
		}
	}
}
