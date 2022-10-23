using System;
using System.IO;
using System.Threading;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003A7 RID: 935
	public class SaveFileInfo
	{
		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06001ABD RID: 6845 RVA: 0x000A29EC File Offset: 0x000A0BEC
		private bool Valid
		{
			get
			{
				object obj = this.lockObject;
				bool result;
				lock (obj)
				{
					result = (this.gameVersion != null);
				}
				return result;
			}
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06001ABE RID: 6846 RVA: 0x000A2A34 File Offset: 0x000A0C34
		public FileInfo FileInfo
		{
			get
			{
				return this.fileInfo;
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06001ABF RID: 6847 RVA: 0x000A2A3C File Offset: 0x000A0C3C
		public string FileName
		{
			get
			{
				return this.fileName;
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001AC0 RID: 6848 RVA: 0x000A2A44 File Offset: 0x000A0C44
		public DateTime LastWriteTime
		{
			get
			{
				return this.lastWriteTime;
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001AC1 RID: 6849 RVA: 0x000A2A4C File Offset: 0x000A0C4C
		public string GameVersion
		{
			get
			{
				bool flag = false;
				try
				{
					if (flag = this.TryLock(0))
					{
						if (!this.loaded)
						{
							return "LoadingVersionInfo".Translate();
						}
						if (!this.Valid)
						{
							return "???";
						}
						return this.gameVersion;
					}
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(this.lockObject);
					}
				}
				return "LoadingVersionInfo".Translate();
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06001AC2 RID: 6850 RVA: 0x000A2ACC File Offset: 0x000A0CCC
		public Color VersionColor
		{
			get
			{
				bool flag = false;
				try
				{
					if (flag = this.TryLock(0))
					{
						if (!this.loaded)
						{
							return Color.gray;
						}
						if (!this.Valid)
						{
							return ColorLibrary.RedReadable;
						}
						if (VersionControl.MajorFromVersionString(this.gameVersion) == VersionControl.CurrentMajor && VersionControl.MinorFromVersionString(this.gameVersion) == VersionControl.CurrentMinor)
						{
							return SaveFileInfo.UnimportantTextColor;
						}
						if (BackCompatibility.IsSaveCompatibleWith(this.gameVersion))
						{
							return Color.yellow;
						}
						return ColorLibrary.RedReadable;
					}
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(this.lockObject);
					}
				}
				return Color.gray;
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06001AC3 RID: 6851 RVA: 0x000A2B78 File Offset: 0x000A0D78
		public TipSignal CompatibilityTip
		{
			get
			{
				bool flag = false;
				try
				{
					if (flag = this.TryLock(0))
					{
						if (!this.loaded)
						{
							return "LoadingVersionInfo".Translate();
						}
						if (!this.Valid)
						{
							return "SaveIsUnknownFormat".Translate();
						}
						if ((VersionControl.MajorFromVersionString(this.gameVersion) != VersionControl.CurrentMajor || VersionControl.MinorFromVersionString(this.gameVersion) != VersionControl.CurrentMinor) && !BackCompatibility.IsSaveCompatibleWith(this.gameVersion))
						{
							return "SaveIsFromDifferentGameVersion".Translate(VersionControl.CurrentVersionString, this.gameVersion);
						}
						if (VersionControl.BuildFromVersionString(this.gameVersion) != VersionControl.CurrentBuild)
						{
							return "SaveIsFromDifferentGameBuild".Translate(VersionControl.CurrentVersionString, this.gameVersion);
						}
						return "SaveIsFromThisGameBuild".Translate();
					}
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(this.lockObject);
					}
				}
				return "LoadingVersionInfo".Translate();
			}
		}

		// Token: 0x06001AC4 RID: 6852 RVA: 0x000A2CA4 File Offset: 0x000A0EA4
		private bool TryLock(int timeoutMilliseconds)
		{
			return Monitor.TryEnter(this.lockObject, timeoutMilliseconds);
		}

		// Token: 0x06001AC5 RID: 6853 RVA: 0x000A2CB2 File Offset: 0x000A0EB2
		public SaveFileInfo(FileInfo fileInfo)
		{
			this.fileInfo = fileInfo;
			this.fileName = fileInfo.Name;
			this.lastWriteTime = fileInfo.LastWriteTime;
		}

		// Token: 0x06001AC6 RID: 6854 RVA: 0x000A2CE4 File Offset: 0x000A0EE4
		public void LoadData()
		{
			object obj = this.lockObject;
			lock (obj)
			{
				this.gameVersion = ScribeMetaHeaderUtility.GameVersionOf(this.fileInfo);
				this.loaded = true;
			}
		}

		// Token: 0x0400137C RID: 4988
		private FileInfo fileInfo;

		// Token: 0x0400137D RID: 4989
		private string gameVersion;

		// Token: 0x0400137E RID: 4990
		private DateTime lastWriteTime;

		// Token: 0x0400137F RID: 4991
		private string fileName;

		// Token: 0x04001380 RID: 4992
		private bool loaded;

		// Token: 0x04001381 RID: 4993
		private object lockObject = new object();

		// Token: 0x04001382 RID: 4994
		public static readonly Color UnimportantTextColor = new Color(1f, 1f, 1f, 0.5f);
	}
}
