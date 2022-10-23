using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000278 RID: 632
	public class ModAssetBundlesHandler
	{
		// Token: 0x06001209 RID: 4617 RVA: 0x00069804 File Offset: 0x00067A04
		public ModAssetBundlesHandler(ModContentPack mod)
		{
			this.mod = mod;
		}

		// Token: 0x0600120A RID: 4618 RVA: 0x00069820 File Offset: 0x00067A20
		public void ReloadAll()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(this.mod.RootDir, "AssetBundles"));
			if (!directoryInfo.Exists)
			{
				return;
			}
			foreach (FileInfo fileInfo in directoryInfo.GetFiles("*", SearchOption.AllDirectories))
			{
				if (fileInfo.Extension.NullOrEmpty())
				{
					AssetBundle assetBundle = AssetBundle.LoadFromFile(fileInfo.FullName);
					if (assetBundle != null)
					{
						this.loadedAssetBundles.Add(assetBundle);
					}
					else
					{
						Log.Error("Could not load asset bundle at " + fileInfo.FullName);
					}
				}
			}
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x000698B8 File Offset: 0x00067AB8
		public void ClearDestroy()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				for (int i = 0; i < this.loadedAssetBundles.Count; i++)
				{
					this.loadedAssetBundles[i].Unload(true);
				}
				this.loadedAssetBundles.Clear();
			});
		}

		// Token: 0x04000F32 RID: 3890
		private ModContentPack mod;

		// Token: 0x04000F33 RID: 3891
		public List<AssetBundle> loadedAssetBundles = new List<AssetBundle>();

		// Token: 0x04000F34 RID: 3892
		public static readonly string[] TextureExtensions = new string[]
		{
			".png",
			".psd",
			".jpg",
			".jpeg"
		};

		// Token: 0x04000F35 RID: 3893
		public static readonly string[] AudioClipExtensions = new string[]
		{
			".wav",
			".mp3"
		};
	}
}
