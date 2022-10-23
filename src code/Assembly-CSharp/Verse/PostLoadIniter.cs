using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003B0 RID: 944
	public class PostLoadIniter
	{
		// Token: 0x06001AE3 RID: 6883 RVA: 0x000A3AAC File Offset: 0x000A1CAC
		public void RegisterForPostLoadInit(IExposable s)
		{
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				Log.Error(string.Concat(new object[]
				{
					"Registered ",
					s,
					" for post load init, but current mode is ",
					Scribe.mode
				}));
				return;
			}
			if (s == null)
			{
				Log.Warning("Trying to register null in RegisterforPostLoadInit.");
				return;
			}
			try
			{
				if (!this.saveablesToPostLoad.Add(s))
				{
					Log.Warning("Tried to register in RegisterforPostLoadInit when already registered: " + s);
				}
			}
			catch (Exception arg)
			{
				Log.Error("Could not register an object for post load init: " + arg);
			}
		}

		// Token: 0x06001AE4 RID: 6884 RVA: 0x000A3B48 File Offset: 0x000A1D48
		public void DoAllPostLoadInits()
		{
			Scribe.mode = LoadSaveMode.PostLoadInit;
			foreach (IExposable exposable in this.saveablesToPostLoad)
			{
				try
				{
					Scribe.loader.curParent = exposable;
					Scribe.loader.curPathRelToParent = null;
					exposable.ExposeData();
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not do PostLoadInit on ",
						exposable.ToStringSafe<IExposable>(),
						": ",
						ex
					}));
				}
			}
			this.Clear();
			Scribe.loader.curParent = null;
			Scribe.loader.curPathRelToParent = null;
			Scribe.mode = LoadSaveMode.Inactive;
		}

		// Token: 0x06001AE5 RID: 6885 RVA: 0x000A3C18 File Offset: 0x000A1E18
		public void Clear()
		{
			this.saveablesToPostLoad.Clear();
		}

		// Token: 0x0400139A RID: 5018
		private HashSet<IExposable> saveablesToPostLoad = new HashSet<IExposable>();
	}
}
