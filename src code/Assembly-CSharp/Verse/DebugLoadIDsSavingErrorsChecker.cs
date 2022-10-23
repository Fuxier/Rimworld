using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003B4 RID: 948
	public class DebugLoadIDsSavingErrorsChecker
	{
		// Token: 0x06001AFD RID: 6909 RVA: 0x000A4D4D File Offset: 0x000A2F4D
		public void Clear()
		{
			if (!Prefs.DevMode)
			{
				return;
			}
			this.deepSaved.Clear();
			this.deepSavedInfo.Clear();
			this.referenced.Clear();
		}

		// Token: 0x06001AFE RID: 6910 RVA: 0x000A4D78 File Offset: 0x000A2F78
		public void CheckForErrorsAndClear()
		{
			if (!Prefs.DevMode)
			{
				return;
			}
			if (!Scribe.saver.savingForDebug)
			{
				foreach (DebugLoadIDsSavingErrorsChecker.ReferencedObject referencedObject in this.referenced)
				{
					if (!this.deepSaved.Contains(referencedObject.loadID))
					{
						Log.Warning(string.Concat(new string[]
						{
							"Object with load ID ",
							referencedObject.loadID,
							" is referenced (xml node name: ",
							referencedObject.label,
							") but is not deep-saved. This will cause errors during loading."
						}));
					}
				}
			}
			this.Clear();
		}

		// Token: 0x06001AFF RID: 6911 RVA: 0x000A4E2C File Offset: 0x000A302C
		public void RegisterDeepSaved(object obj, string label)
		{
			if (!Prefs.DevMode)
			{
				return;
			}
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				Log.Error(string.Concat(new object[]
				{
					"Registered ",
					obj,
					", but current mode is ",
					Scribe.mode
				}));
				return;
			}
			if (obj == null)
			{
				return;
			}
			ILoadReferenceable loadReferenceable = obj as ILoadReferenceable;
			if (loadReferenceable != null)
			{
				try
				{
					string uniqueLoadID = loadReferenceable.GetUniqueLoadID();
					if (!this.deepSaved.Add(uniqueLoadID))
					{
						Log.Warning(string.Concat(new string[]
						{
							"DebugLoadIDsSavingErrorsChecker error: tried to register deep-saved object with loadID ",
							uniqueLoadID,
							", but it's already here. label=",
							label,
							" (not cleared after the previous save? different objects have the same load ID? the same object is deep-saved twice?)"
						}));
						string text;
						if (this.deepSavedInfo.TryGetValue(uniqueLoadID, out text))
						{
							Log.Warning(string.Concat(new object[]
							{
								loadReferenceable.GetType(),
								" was already deepsaved at ",
								text,
								"."
							}));
						}
					}
					else
					{
						this.deepSavedInfo.Add(uniqueLoadID, Scribe.saver.CurPath);
					}
				}
				catch (Exception arg)
				{
					Log.Error("Error in GetUniqueLoadID(): " + arg);
				}
			}
		}

		// Token: 0x06001B00 RID: 6912 RVA: 0x000A4F4C File Offset: 0x000A314C
		public void RegisterReferenced(ILoadReferenceable obj, string label)
		{
			if (!Prefs.DevMode)
			{
				return;
			}
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				Log.Error(string.Concat(new object[]
				{
					"Registered ",
					obj,
					", but current mode is ",
					Scribe.mode
				}));
				return;
			}
			if (obj == null)
			{
				return;
			}
			try
			{
				this.referenced.Add(new DebugLoadIDsSavingErrorsChecker.ReferencedObject(obj.GetUniqueLoadID(), label));
			}
			catch (Exception arg)
			{
				Log.Error("Error in GetUniqueLoadID(): " + arg);
			}
		}

		// Token: 0x040013AB RID: 5035
		private HashSet<string> deepSaved = new HashSet<string>();

		// Token: 0x040013AC RID: 5036
		private Dictionary<string, string> deepSavedInfo = new Dictionary<string, string>();

		// Token: 0x040013AD RID: 5037
		private HashSet<DebugLoadIDsSavingErrorsChecker.ReferencedObject> referenced = new HashSet<DebugLoadIDsSavingErrorsChecker.ReferencedObject>();

		// Token: 0x02001E87 RID: 7815
		private struct ReferencedObject : IEquatable<DebugLoadIDsSavingErrorsChecker.ReferencedObject>
		{
			// Token: 0x0600B94E RID: 47438 RVA: 0x0041F25F File Offset: 0x0041D45F
			public ReferencedObject(string loadID, string label)
			{
				this.loadID = loadID;
				this.label = label;
			}

			// Token: 0x0600B94F RID: 47439 RVA: 0x0041F26F File Offset: 0x0041D46F
			public override bool Equals(object obj)
			{
				return obj is DebugLoadIDsSavingErrorsChecker.ReferencedObject && this.Equals((DebugLoadIDsSavingErrorsChecker.ReferencedObject)obj);
			}

			// Token: 0x0600B950 RID: 47440 RVA: 0x0041F287 File Offset: 0x0041D487
			public bool Equals(DebugLoadIDsSavingErrorsChecker.ReferencedObject other)
			{
				return this.loadID == other.loadID && this.label == other.label;
			}

			// Token: 0x0600B951 RID: 47441 RVA: 0x0041F2AF File Offset: 0x0041D4AF
			public override int GetHashCode()
			{
				return Gen.HashCombine<string>(Gen.HashCombine<string>(0, this.loadID), this.label);
			}

			// Token: 0x0600B952 RID: 47442 RVA: 0x0041F2C8 File Offset: 0x0041D4C8
			public static bool operator ==(DebugLoadIDsSavingErrorsChecker.ReferencedObject lhs, DebugLoadIDsSavingErrorsChecker.ReferencedObject rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x0600B953 RID: 47443 RVA: 0x0041F2D2 File Offset: 0x0041D4D2
			public static bool operator !=(DebugLoadIDsSavingErrorsChecker.ReferencedObject lhs, DebugLoadIDsSavingErrorsChecker.ReferencedObject rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x04007829 RID: 30761
			public string loadID;

			// Token: 0x0400782A RID: 30762
			public string label;
		}
	}
}
