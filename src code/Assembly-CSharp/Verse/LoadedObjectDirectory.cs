using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003AF RID: 943
	public class LoadedObjectDirectory
	{
		// Token: 0x06001ADF RID: 6879 RVA: 0x000A3643 File Offset: 0x000A1843
		public void Clear()
		{
			this.allObjectsByLoadID.Clear();
			this.allThingsByThingID.Clear();
		}

		// Token: 0x06001AE0 RID: 6880 RVA: 0x000A365C File Offset: 0x000A185C
		public void RegisterLoaded(ILoadReferenceable reffable)
		{
			if (Prefs.DevMode)
			{
				string text = "[excepted]";
				try
				{
					text = reffable.GetUniqueLoadID();
				}
				catch (Exception)
				{
				}
				string text2 = "[excepted]";
				try
				{
					text2 = reffable.ToString();
				}
				catch (Exception)
				{
				}
				ILoadReferenceable loadReferenceable;
				if (this.allObjectsByLoadID.TryGetValue(text, out loadReferenceable))
				{
					string text3 = "";
					Log.Error(string.Concat(new object[]
					{
						"Cannot register ",
						reffable.GetType(),
						" ",
						text2,
						", (id=",
						text,
						" in loaded object directory. Id already used by ",
						loadReferenceable.GetType(),
						" ",
						loadReferenceable.ToStringSafe<ILoadReferenceable>(),
						".",
						text3
					}));
					return;
				}
			}
			try
			{
				this.allObjectsByLoadID.Add(reffable.GetUniqueLoadID(), reffable);
			}
			catch (Exception ex)
			{
				string text4 = "[excepted]";
				try
				{
					text4 = reffable.GetUniqueLoadID();
				}
				catch (Exception)
				{
				}
				string text5 = "[excepted]";
				try
				{
					text5 = reffable.ToString();
				}
				catch (Exception)
				{
				}
				Log.Error(string.Concat(new object[]
				{
					"Exception registering ",
					reffable.GetType(),
					" ",
					text5,
					" in loaded object directory with unique load ID ",
					text4,
					": ",
					ex
				}));
			}
			Thing thing;
			if ((thing = (reffable as Thing)) != null)
			{
				try
				{
					this.allThingsByThingID.Add(thing.thingIDNumber, reffable);
				}
				catch (Exception ex2)
				{
					string text6 = "[excepted]";
					try
					{
						text6 = reffable.ToString();
					}
					catch (Exception)
					{
					}
					Log.Error(string.Concat(new object[]
					{
						"Exception registering ",
						reffable.GetType(),
						" ",
						text6,
						" in loaded object directory with unique thing ID ",
						thing.thingIDNumber,
						": ",
						ex2
					}));
				}
			}
		}

		// Token: 0x06001AE1 RID: 6881 RVA: 0x000A3884 File Offset: 0x000A1A84
		public T ObjectWithLoadID<T>(string loadID)
		{
			if (loadID.NullOrEmpty() || loadID == "null")
			{
				T result = default(T);
				return result;
			}
			ILoadReferenceable loadReferenceable;
			if (this.allObjectsByLoadID.TryGetValue(loadID, out loadReferenceable))
			{
				if (loadReferenceable == null)
				{
					T result = default(T);
					return result;
				}
				try
				{
					return (T)((object)loadReferenceable);
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception getting object with load id ",
						loadID,
						" of type ",
						typeof(T),
						". What we loaded was ",
						loadReferenceable.ToStringSafe<ILoadReferenceable>(),
						". Exception:\n",
						ex
					}));
					return default(T);
				}
			}
			if (typeof(Thing).IsAssignableFrom(typeof(T)) && this.allThingsByThingID.TryGetValue(Thing.IDNumberFromThingID(loadID), out loadReferenceable))
			{
				Log.Warning("Could not resolve reference to Thing with loadID " + loadID + ". Resolving reference by using thingIDNumber instead (back-compat).");
				if (loadReferenceable == null)
				{
					return default(T);
				}
				try
				{
					return (T)((object)loadReferenceable);
				}
				catch (Exception ex2)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception getting object with thing id ",
						Thing.IDNumberFromThingID(loadID),
						" of type ",
						typeof(T),
						". What we loaded was ",
						loadReferenceable.ToStringSafe<ILoadReferenceable>(),
						". Exception:\n",
						ex2
					}));
					return default(T);
				}
			}
			Log.Warning(string.Concat(new object[]
			{
				"Could not resolve reference to object with loadID ",
				loadID,
				" of type ",
				typeof(T),
				". Was it compressed away, destroyed, had no ID number, or not saved/loaded right? curParent=",
				Scribe.loader.curParent.ToStringSafe<IExposable>(),
				" curPathRelToParent=",
				Scribe.loader.curPathRelToParent
			}));
			return default(T);
		}

		// Token: 0x04001398 RID: 5016
		private Dictionary<string, ILoadReferenceable> allObjectsByLoadID = new Dictionary<string, ILoadReferenceable>();

		// Token: 0x04001399 RID: 5017
		private Dictionary<int, ILoadReferenceable> allThingsByThingID = new Dictionary<int, ILoadReferenceable>();
	}
}
