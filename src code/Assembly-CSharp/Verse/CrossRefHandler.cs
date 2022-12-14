using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003AD RID: 941
	public class CrossRefHandler
	{
		// Token: 0x06001ACE RID: 6862 RVA: 0x000A2E54 File Offset: 0x000A1054
		public void RegisterForCrossRefResolve(IExposable s)
		{
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				Log.Error(string.Concat(new object[]
				{
					"Registered ",
					s,
					" for cross ref resolve, but current mode is ",
					Scribe.mode
				}));
				return;
			}
			if (s == null)
			{
				return;
			}
			if (DebugViewSettings.logMapLoad)
			{
				LogSimple.Message("RegisterForCrossRefResolve " + ((s != null) ? s.GetType().ToString() : "null"));
			}
			this.crossReferencingExposables.Add(s);
		}

		// Token: 0x06001ACF RID: 6863 RVA: 0x000A2ED8 File Offset: 0x000A10D8
		public void ResolveAllCrossReferences()
		{
			Scribe.mode = LoadSaveMode.ResolvingCrossRefs;
			if (DebugViewSettings.logMapLoad)
			{
				LogSimple.Message("==================Register the saveables all so we can find them later");
			}
			foreach (IExposable exposable in this.crossReferencingExposables)
			{
				ILoadReferenceable loadReferenceable = exposable as ILoadReferenceable;
				if (loadReferenceable != null)
				{
					if (DebugViewSettings.logMapLoad)
					{
						LogSimple.Message("RegisterLoaded " + loadReferenceable.GetType());
					}
					this.loadedObjectDirectory.RegisterLoaded(loadReferenceable);
				}
			}
			if (DebugViewSettings.logMapLoad)
			{
				LogSimple.Message("==================Fill all cross-references to the saveables");
			}
			foreach (IExposable exposable2 in this.crossReferencingExposables)
			{
				if (DebugViewSettings.logMapLoad)
				{
					LogSimple.Message("ResolvingCrossRefs ExposeData " + exposable2.GetType());
				}
				try
				{
					Scribe.loader.curParent = exposable2;
					Scribe.loader.curPathRelToParent = null;
					exposable2.ExposeData();
				}
				catch (Exception arg)
				{
					Log.Error("Could not resolve cross refs: " + arg);
				}
			}
			Scribe.loader.curParent = null;
			Scribe.loader.curPathRelToParent = null;
			Scribe.mode = LoadSaveMode.Inactive;
			this.Clear(true);
		}

		// Token: 0x06001AD0 RID: 6864 RVA: 0x000A3034 File Offset: 0x000A1234
		public T TakeResolvedRef<T>(string pathRelToParent, IExposable parent) where T : ILoadReferenceable
		{
			string loadID = this.loadIDs.Take<T>(pathRelToParent, parent);
			return this.loadedObjectDirectory.ObjectWithLoadID<T>(loadID);
		}

		// Token: 0x06001AD1 RID: 6865 RVA: 0x000A305C File Offset: 0x000A125C
		public T TakeResolvedRef<T>(string toAppendToPathRelToParent) where T : ILoadReferenceable
		{
			string text = Scribe.loader.curPathRelToParent;
			if (!toAppendToPathRelToParent.NullOrEmpty())
			{
				text = text + "/" + toAppendToPathRelToParent;
			}
			return this.TakeResolvedRef<T>(text, Scribe.loader.curParent);
		}

		// Token: 0x06001AD2 RID: 6866 RVA: 0x000A309C File Offset: 0x000A129C
		public List<T> TakeResolvedRefList<T>(string pathRelToParent, IExposable parent)
		{
			List<string> list = this.loadIDs.TakeList(pathRelToParent, parent);
			List<T> list2 = new List<T>();
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					list2.Add(this.loadedObjectDirectory.ObjectWithLoadID<T>(list[i]));
				}
			}
			return list2;
		}

		// Token: 0x06001AD3 RID: 6867 RVA: 0x000A30EC File Offset: 0x000A12EC
		public List<T> TakeResolvedRefList<T>(string toAppendToPathRelToParent)
		{
			string text = Scribe.loader.curPathRelToParent;
			if (!toAppendToPathRelToParent.NullOrEmpty())
			{
				text = text + "/" + toAppendToPathRelToParent;
			}
			return this.TakeResolvedRefList<T>(text, Scribe.loader.curParent);
		}

		// Token: 0x06001AD4 RID: 6868 RVA: 0x000A312A File Offset: 0x000A132A
		public void Clear(bool errorIfNotEmpty)
		{
			if (errorIfNotEmpty)
			{
				this.loadIDs.ConfirmClear();
			}
			else
			{
				this.loadIDs.Clear();
			}
			this.crossReferencingExposables.Clear();
			this.loadedObjectDirectory.Clear();
		}

		// Token: 0x04001393 RID: 5011
		private LoadedObjectDirectory loadedObjectDirectory = new LoadedObjectDirectory();

		// Token: 0x04001394 RID: 5012
		public LoadIDsWantedBank loadIDs = new LoadIDsWantedBank();

		// Token: 0x04001395 RID: 5013
		public List<IExposable> crossReferencingExposables = new List<IExposable>();
	}
}
