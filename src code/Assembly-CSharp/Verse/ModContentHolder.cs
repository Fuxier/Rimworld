using System;
using System.Collections.Generic;
using System.IO;
using KTrie;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000279 RID: 633
	public class ModContentHolder<T> where T : class
	{
		// Token: 0x0600120E RID: 4622 RVA: 0x00069960 File Offset: 0x00067B60
		public ModContentHolder(ModContentPack mod)
		{
			this.mod = mod;
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x00069990 File Offset: 0x00067B90
		public void ClearDestroy()
		{
			if (typeof(UnityEngine.Object).IsAssignableFrom(typeof(T)))
			{
				foreach (T localObj2 in this.contentList.Values)
				{
					T localObj = localObj2;
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						UnityEngine.Object.Destroy((UnityEngine.Object)((object)localObj));
					});
				}
			}
			for (int i = 0; i < this.extraDisposables.Count; i++)
			{
				this.extraDisposables[i].Dispose();
			}
			this.extraDisposables.Clear();
			this.contentList.Clear();
			this.contentListTrie.Clear();
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x00069A60 File Offset: 0x00067C60
		public void ReloadAll()
		{
			foreach (Pair<string, LoadedContentItem<T>> pair in ModContentLoader<T>.LoadAllForMod(this.mod))
			{
				string text = pair.First;
				text = text.Replace('\\', '/');
				string text2 = GenFilePaths.ContentPath<T>();
				if (text.StartsWith(text2))
				{
					text = text.Substring(text2.Length);
				}
				if (text.EndsWith(Path.GetExtension(text)))
				{
					text = text.Substring(0, text.Length - Path.GetExtension(text).Length);
				}
				if (this.contentList.ContainsKey(text))
				{
					Log.Warning(string.Concat(new object[]
					{
						"Tried to load duplicate ",
						typeof(T),
						" with path: ",
						pair.Second.internalFile,
						" and internal path: ",
						text
					}));
				}
				else
				{
					this.contentList.Add(text, pair.Second.contentItem);
					this.contentListTrie.Add(text);
					if (pair.Second.extraDisposable != null)
					{
						this.extraDisposables.Add(pair.Second.extraDisposable);
					}
				}
			}
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x00069BB8 File Offset: 0x00067DB8
		public T Get(string path)
		{
			T result;
			if (this.contentList.TryGetValue(path, out result))
			{
				return result;
			}
			return default(T);
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x00069BE0 File Offset: 0x00067DE0
		public IEnumerable<T> GetAllUnderPath(string pathRoot)
		{
			string prefix = (pathRoot.NullOrEmpty() || pathRoot[pathRoot.Length - 1] == '/') ? pathRoot : (pathRoot + "/");
			foreach (string key in this.contentListTrie.GetByPrefix(prefix))
			{
				yield return this.contentList[key];
			}
			IEnumerator<string> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x04000F36 RID: 3894
		private ModContentPack mod;

		// Token: 0x04000F37 RID: 3895
		public Dictionary<string, T> contentList = new Dictionary<string, T>();

		// Token: 0x04000F38 RID: 3896
		public List<IDisposable> extraDisposables = new List<IDisposable>();

		// Token: 0x04000F39 RID: 3897
		private StringTrieSet contentListTrie = new StringTrieSet();
	}
}
