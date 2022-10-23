using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200057A RID: 1402
	public class KeyPrefsData
	{
		// Token: 0x06002B00 RID: 11008 RVA: 0x00112FDC File Offset: 0x001111DC
		public void ResetToDefaults()
		{
			this.keyPrefs.Clear();
			this.AddMissingDefaultBindings();
		}

		// Token: 0x06002B01 RID: 11009 RVA: 0x00112FF0 File Offset: 0x001111F0
		public void AddMissingDefaultBindings()
		{
			foreach (KeyBindingDef keyBindingDef in DefDatabase<KeyBindingDef>.AllDefs)
			{
				if (!this.keyPrefs.ContainsKey(keyBindingDef))
				{
					this.keyPrefs.Add(keyBindingDef, new KeyBindingData(keyBindingDef.defaultKeyCodeA, keyBindingDef.defaultKeyCodeB));
				}
			}
		}

		// Token: 0x06002B02 RID: 11010 RVA: 0x00113060 File Offset: 0x00111260
		public bool SetBinding(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot, KeyCode keyCode)
		{
			KeyBindingData keyBindingData;
			if (this.keyPrefs.TryGetValue(keyDef, out keyBindingData))
			{
				if (slot != KeyPrefs.BindingSlot.A)
				{
					if (slot != KeyPrefs.BindingSlot.B)
					{
						Log.Error("Tried to set a key binding for \"" + keyDef.LabelCap + "\" on a nonexistent slot: " + slot.ToString());
						return false;
					}
					keyBindingData.keyBindingB = keyCode;
				}
				else
				{
					keyBindingData.keyBindingA = keyCode;
				}
				return true;
			}
			Log.Error("Key not found in keyprefs: \"" + keyDef.LabelCap + "\"");
			return false;
		}

		// Token: 0x06002B03 RID: 11011 RVA: 0x001130F8 File Offset: 0x001112F8
		public KeyCode GetBoundKeyCode(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot)
		{
			KeyBindingData keyBindingData;
			if (!this.keyPrefs.TryGetValue(keyDef, out keyBindingData))
			{
				Log.Error("Key not found in keyprefs: \"" + keyDef.LabelCap + "\"");
				return KeyCode.None;
			}
			if (slot == KeyPrefs.BindingSlot.A)
			{
				return keyBindingData.keyBindingA;
			}
			if (slot != KeyPrefs.BindingSlot.B)
			{
				throw new InvalidOperationException();
			}
			return keyBindingData.keyBindingB;
		}

		// Token: 0x06002B04 RID: 11012 RVA: 0x00113157 File Offset: 0x00111357
		private IEnumerable<KeyBindingDef> ConflictingBindings(KeyBindingDef keyDef, KeyCode code)
		{
			using (IEnumerator<KeyBindingDef> enumerator = DefDatabase<KeyBindingDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyBindingDef def = enumerator.Current;
					KeyBindingData keyBindingData;
					if (def != keyDef && ((def.category == keyDef.category && def.category.selfConflicting) || keyDef.category.checkForConflicts.Contains(def.category) || (keyDef.extraConflictTags != null && def.extraConflictTags != null && keyDef.extraConflictTags.Any((string tag) => def.extraConflictTags.Contains(tag)))) && this.keyPrefs.TryGetValue(def, out keyBindingData) && (keyBindingData.keyBindingA == code || keyBindingData.keyBindingB == code))
					{
						yield return def;
					}
				}
			}
			IEnumerator<KeyBindingDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06002B05 RID: 11013 RVA: 0x00113178 File Offset: 0x00111378
		public void EraseConflictingBindingsForKeyCode(KeyBindingDef keyDef, KeyCode keyCode, Action<KeyBindingDef> callBackOnErase = null)
		{
			foreach (KeyBindingDef keyBindingDef in this.ConflictingBindings(keyDef, keyCode))
			{
				KeyBindingData keyBindingData = this.keyPrefs[keyBindingDef];
				if (keyBindingData.keyBindingA == keyCode)
				{
					keyBindingData.keyBindingA = KeyCode.None;
				}
				if (keyBindingData.keyBindingB == keyCode)
				{
					keyBindingData.keyBindingB = KeyCode.None;
				}
				if (callBackOnErase != null)
				{
					callBackOnErase(keyBindingDef);
				}
			}
		}

		// Token: 0x06002B06 RID: 11014 RVA: 0x001131F8 File Offset: 0x001113F8
		public void CheckConflictsFor(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot)
		{
			KeyCode boundKeyCode = this.GetBoundKeyCode(keyDef, slot);
			if (boundKeyCode != KeyCode.None)
			{
				this.EraseConflictingBindingsForKeyCode(keyDef, boundKeyCode, null);
				this.SetBinding(keyDef, slot, boundKeyCode);
			}
		}

		// Token: 0x06002B07 RID: 11015 RVA: 0x00113224 File Offset: 0x00111424
		public KeyPrefsData Clone()
		{
			KeyPrefsData keyPrefsData = new KeyPrefsData();
			foreach (KeyValuePair<KeyBindingDef, KeyBindingData> keyValuePair in this.keyPrefs)
			{
				keyPrefsData.keyPrefs[keyValuePair.Key] = new KeyBindingData(keyValuePair.Value.keyBindingA, keyValuePair.Value.keyBindingB);
			}
			return keyPrefsData;
		}

		// Token: 0x06002B08 RID: 11016 RVA: 0x001132A8 File Offset: 0x001114A8
		public void ErrorCheck()
		{
			foreach (KeyBindingDef keyDef in DefDatabase<KeyBindingDef>.AllDefs)
			{
				this.ErrorCheckOn(keyDef, KeyPrefs.BindingSlot.A);
				this.ErrorCheckOn(keyDef, KeyPrefs.BindingSlot.B);
			}
		}

		// Token: 0x06002B09 RID: 11017 RVA: 0x00113300 File Offset: 0x00111500
		private void ErrorCheckOn(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot)
		{
			KeyCode boundKeyCode = this.GetBoundKeyCode(keyDef, slot);
			if (boundKeyCode != KeyCode.None)
			{
				foreach (KeyBindingDef keyBindingDef in this.ConflictingBindings(keyDef, boundKeyCode))
				{
					bool flag = boundKeyCode != keyDef.GetDefaultKeyCode(slot);
					Log.Warning(string.Concat(new object[]
					{
						"Key binding conflict: ",
						keyBindingDef,
						" and ",
						keyDef,
						" are both bound to ",
						boundKeyCode,
						".",
						flag ? " Fixed automatically." : ""
					}));
					if (flag)
					{
						if (slot == KeyPrefs.BindingSlot.A)
						{
							this.keyPrefs[keyDef].keyBindingA = keyDef.defaultKeyCodeA;
						}
						else
						{
							this.keyPrefs[keyDef].keyBindingB = keyDef.defaultKeyCodeB;
						}
						KeyPrefs.Save();
					}
				}
			}
		}

		// Token: 0x04001C15 RID: 7189
		public Dictionary<KeyBindingDef, KeyBindingData> keyPrefs = new Dictionary<KeyBindingDef, KeyBindingData>();
	}
}
