using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000049 RID: 73
	public static class Log
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x000147AB File Offset: 0x000129AB
		public static IEnumerable<LogMessage> Messages
		{
			get
			{
				return Log.messageQueue.Messages;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x000147B7 File Offset: 0x000129B7
		private static bool ReachedMaxMessagesLimit
		{
			get
			{
				return Log.reachedMaxMessagesLimit;
			}
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x000147C0 File Offset: 0x000129C0
		public static void ResetMessageCount()
		{
			object obj = Log.logLock;
			lock (obj)
			{
				Log.messageCount = 0;
				if (Log.reachedMaxMessagesLimit)
				{
					Debug.unityLogger.logEnabled = true;
					Log.reachedMaxMessagesLimit = false;
					Log.Message("Message logging is now once again on.");
				}
			}
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00014824 File Offset: 0x00012A24
		[Obsolete]
		public static void Message(string text, bool ignoreStopLoggingLimit)
		{
			Log.Message(text);
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0001482C File Offset: 0x00012A2C
		public static void Message(string text)
		{
			object obj = Log.logLock;
			lock (obj)
			{
				if (!Log.ReachedMaxMessagesLimit)
				{
					Debug.Log(text);
					Log.messageQueue.Enqueue(new LogMessage(LogMessageType.Message, text, StackTraceUtility.ExtractStackTrace()));
					Log.PostMessage();
				}
			}
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00014890 File Offset: 0x00012A90
		[Obsolete]
		public static void Warning(string text, bool ignoreStopLoggingLimit)
		{
			Log.Warning(text);
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00014898 File Offset: 0x00012A98
		public static void Warning(string text)
		{
			object obj = Log.logLock;
			lock (obj)
			{
				if (!Log.ReachedMaxMessagesLimit)
				{
					Debug.LogWarning(text);
					Log.messageQueue.Enqueue(new LogMessage(LogMessageType.Warning, text, StackTraceUtility.ExtractStackTrace()));
					Log.PostMessage();
				}
			}
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x000148FC File Offset: 0x00012AFC
		public static void WarningOnce(string text, int key)
		{
			object obj = Log.logLock;
			lock (obj)
			{
				if (!Log.ReachedMaxMessagesLimit)
				{
					if (!Log.usedKeys.Contains(key))
					{
						Log.usedKeys.Add(key);
						Log.Warning(text);
					}
				}
			}
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00014960 File Offset: 0x00012B60
		[Obsolete]
		public static void Error(string text, bool ignoreStopLoggingLimit)
		{
			Log.Error(text);
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00014968 File Offset: 0x00012B68
		public static void Error(string text)
		{
			object obj = Log.logLock;
			lock (obj)
			{
				if (!Log.ReachedMaxMessagesLimit)
				{
					Debug.LogError(text);
					if (!Log.currentlyLoggingError)
					{
						Log.currentlyLoggingError = true;
						try
						{
							if (Prefs.PauseOnError && Current.ProgramState == ProgramState.Playing)
							{
								Find.TickManager.Pause();
							}
							Log.messageQueue.Enqueue(new LogMessage(LogMessageType.Error, text, StackTraceUtility.ExtractStackTrace()));
							Log.PostMessage();
							if (!PlayDataLoader.Loaded || Prefs.DevMode)
							{
								Log.TryOpenLogWindow();
							}
						}
						catch (Exception arg)
						{
							Debug.LogError("An error occurred while logging an error: " + arg);
						}
						finally
						{
							Log.currentlyLoggingError = false;
						}
					}
				}
			}
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00014A3C File Offset: 0x00012C3C
		[Obsolete]
		public static void ErrorOnce(string text, int key, bool ignoreStopLoggingLimit)
		{
			Log.ErrorOnce(text, key);
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00014A48 File Offset: 0x00012C48
		public static void ErrorOnce(string text, int key)
		{
			object obj = Log.logLock;
			lock (obj)
			{
				if (!Log.ReachedMaxMessagesLimit)
				{
					if (!Log.usedKeys.Contains(key))
					{
						Log.usedKeys.Add(key);
						Log.Error(text);
					}
				}
			}
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00014AAC File Offset: 0x00012CAC
		public static void Clear()
		{
			object obj = Log.logLock;
			lock (obj)
			{
				EditWindow_Log.ClearSelectedMessage();
				Log.messageQueue.Clear();
				Log.ResetMessageCount();
			}
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00014AFC File Offset: 0x00012CFC
		public static void TryOpenLogWindow()
		{
			if (StaticConstructorOnStartupUtility.coreStaticAssetsLoaded || UnityData.IsInMainThread)
			{
				EditWindow_Log.TryAutoOpen();
			}
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00014B11 File Offset: 0x00012D11
		private static void PostMessage()
		{
			if (Log.openOnMessage)
			{
				Log.TryOpenLogWindow();
				EditWindow_Log.SelectLastMessage(true);
			}
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00014B25 File Offset: 0x00012D25
		public static void Notify_MessageReceivedThreadedInternal(string msg, string stackTrace, LogType type)
		{
			if (++Log.messageCount == 1000)
			{
				Log.Warning("Reached max messages limit. Stopping logging to avoid spam.");
				Log.reachedMaxMessagesLimit = true;
				Debug.unityLogger.logEnabled = false;
			}
		}

		// Token: 0x040000FB RID: 251
		private static LogMessageQueue messageQueue = new LogMessageQueue();

		// Token: 0x040000FC RID: 252
		private static HashSet<int> usedKeys = new HashSet<int>();

		// Token: 0x040000FD RID: 253
		public static bool openOnMessage = false;

		// Token: 0x040000FE RID: 254
		private static bool currentlyLoggingError;

		// Token: 0x040000FF RID: 255
		private static int messageCount;

		// Token: 0x04000100 RID: 256
		private static bool reachedMaxMessagesLimit;

		// Token: 0x04000101 RID: 257
		private static object logLock = new object();

		// Token: 0x04000102 RID: 258
		private const int StopLoggingAtMessageCount = 1000;
	}
}
