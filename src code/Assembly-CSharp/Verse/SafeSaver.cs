using System;
using System.IO;
using System.Threading;

namespace Verse
{
	// Token: 0x020003A6 RID: 934
	public static class SafeSaver
	{
		// Token: 0x06001AB4 RID: 6836 RVA: 0x000A2670 File Offset: 0x000A0870
		private static string GetFileFullPath(string path)
		{
			return Path.GetFullPath(path);
		}

		// Token: 0x06001AB5 RID: 6837 RVA: 0x000A2678 File Offset: 0x000A0878
		private static string GetNewFileFullPath(string path)
		{
			return Path.GetFullPath(path + SafeSaver.NewFileSuffix);
		}

		// Token: 0x06001AB6 RID: 6838 RVA: 0x000A268A File Offset: 0x000A088A
		private static string GetOldFileFullPath(string path)
		{
			return Path.GetFullPath(path + SafeSaver.OldFileSuffix);
		}

		// Token: 0x06001AB7 RID: 6839 RVA: 0x000A269C File Offset: 0x000A089C
		public static void Save(string path, string documentElementName, Action saveAction, bool leaveOldFile = false)
		{
			try
			{
				SafeSaver.CleanSafeSaverFiles(path);
				if (!File.Exists(SafeSaver.GetFileFullPath(path)))
				{
					SafeSaver.DoSave(SafeSaver.GetFileFullPath(path), documentElementName, saveAction);
				}
				else
				{
					SafeSaver.DoSave(SafeSaver.GetNewFileFullPath(path), documentElementName, saveAction);
					try
					{
						SafeSaver.SafeMove(SafeSaver.GetFileFullPath(path), SafeSaver.GetOldFileFullPath(path));
					}
					catch (Exception ex)
					{
						Log.Warning(string.Concat(new object[]
						{
							"Could not move file from \"",
							SafeSaver.GetFileFullPath(path),
							"\" to \"",
							SafeSaver.GetOldFileFullPath(path),
							"\": ",
							ex
						}));
						throw;
					}
					try
					{
						SafeSaver.SafeMove(SafeSaver.GetNewFileFullPath(path), SafeSaver.GetFileFullPath(path));
					}
					catch (Exception ex2)
					{
						Log.Warning(string.Concat(new object[]
						{
							"Could not move file from \"",
							SafeSaver.GetNewFileFullPath(path),
							"\" to \"",
							SafeSaver.GetFileFullPath(path),
							"\": ",
							ex2
						}));
						SafeSaver.RemoveFileIfExists(SafeSaver.GetFileFullPath(path), false);
						SafeSaver.RemoveFileIfExists(SafeSaver.GetNewFileFullPath(path), false);
						try
						{
							SafeSaver.SafeMove(SafeSaver.GetOldFileFullPath(path), SafeSaver.GetFileFullPath(path));
						}
						catch (Exception ex3)
						{
							Log.Warning(string.Concat(new object[]
							{
								"Could not move file from \"",
								SafeSaver.GetOldFileFullPath(path),
								"\" back to \"",
								SafeSaver.GetFileFullPath(path),
								"\": ",
								ex3
							}));
						}
						throw;
					}
					if (!leaveOldFile)
					{
						SafeSaver.RemoveFileIfExists(SafeSaver.GetOldFileFullPath(path), true);
					}
				}
			}
			catch (Exception ex4)
			{
				GenUI.ErrorDialog("ProblemSavingFile".Translate(SafeSaver.GetFileFullPath(path), ex4.ToString()));
				throw;
			}
		}

		// Token: 0x06001AB8 RID: 6840 RVA: 0x000A2898 File Offset: 0x000A0A98
		private static void CleanSafeSaverFiles(string path)
		{
			SafeSaver.RemoveFileIfExists(SafeSaver.GetOldFileFullPath(path), true);
			SafeSaver.RemoveFileIfExists(SafeSaver.GetNewFileFullPath(path), true);
		}

		// Token: 0x06001AB9 RID: 6841 RVA: 0x000A28B4 File Offset: 0x000A0AB4
		private static void DoSave(string fullPath, string documentElementName, Action saveAction)
		{
			try
			{
				Scribe.saver.InitSaving(fullPath, documentElementName);
				saveAction();
				Scribe.saver.FinalizeSaving();
			}
			catch (Exception ex)
			{
				Log.Warning(string.Concat(new object[]
				{
					"An exception was thrown during saving to \"",
					fullPath,
					"\": ",
					ex
				}));
				Scribe.saver.ForceStop();
				SafeSaver.RemoveFileIfExists(fullPath, false);
				throw;
			}
		}

		// Token: 0x06001ABA RID: 6842 RVA: 0x000A292C File Offset: 0x000A0B2C
		private static void RemoveFileIfExists(string path, bool rethrow)
		{
			try
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			catch (Exception ex)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Could not remove file \"",
					path,
					"\": ",
					ex
				}));
				if (rethrow)
				{
					throw;
				}
			}
		}

		// Token: 0x06001ABB RID: 6843 RVA: 0x000A298C File Offset: 0x000A0B8C
		private static void SafeMove(string from, string to)
		{
			Exception ex = null;
			for (int i = 0; i < 50; i++)
			{
				try
				{
					File.Move(from, to);
					return;
				}
				catch (Exception ex2)
				{
					if (ex == null)
					{
						ex = ex2;
					}
				}
				Thread.Sleep(1);
			}
			throw ex;
		}

		// Token: 0x0400137A RID: 4986
		private static readonly string NewFileSuffix = ".new";

		// Token: 0x0400137B RID: 4987
		private static readonly string OldFileSuffix = ".old";
	}
}
