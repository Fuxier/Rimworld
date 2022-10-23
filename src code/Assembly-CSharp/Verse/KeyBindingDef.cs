using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000106 RID: 262
	public class KeyBindingDef : Def
	{
		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000712 RID: 1810 RVA: 0x00025604 File Offset: 0x00023804
		public KeyCode MainKey
		{
			get
			{
				KeyBindingData keyBindingData;
				if (KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData))
				{
					if (keyBindingData.keyBindingA != KeyCode.None)
					{
						return keyBindingData.keyBindingA;
					}
					if (keyBindingData.keyBindingB != KeyCode.None)
					{
						return keyBindingData.keyBindingB;
					}
				}
				return KeyCode.None;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x00025644 File Offset: 0x00023844
		public string MainKeyLabel
		{
			get
			{
				return this.MainKey.ToStringReadable();
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000714 RID: 1812 RVA: 0x00025654 File Offset: 0x00023854
		public bool KeyDownEvent
		{
			get
			{
				KeyBindingData keyBindingData;
				return Event.current.type == EventType.KeyDown && Event.current.keyCode != KeyCode.None && KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData) && (keyBindingData.keyBindingA == KeyCode.LeftCommand || keyBindingData.keyBindingA == KeyCode.RightCommand || keyBindingData.keyBindingB == KeyCode.LeftCommand || keyBindingData.keyBindingB == KeyCode.RightCommand || !Event.current.command) && (Event.current.keyCode == keyBindingData.keyBindingA || Event.current.keyCode == keyBindingData.keyBindingB);
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x000256FC File Offset: 0x000238FC
		public bool IsDownEvent
		{
			get
			{
				KeyBindingData keyBindingData;
				return Event.current != null && KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData) && (this.KeyDownEvent || (Event.current.shift && (keyBindingData.keyBindingA == KeyCode.LeftShift || keyBindingData.keyBindingA == KeyCode.RightShift || keyBindingData.keyBindingB == KeyCode.LeftShift || keyBindingData.keyBindingB == KeyCode.RightShift)) || (Event.current.control && (keyBindingData.keyBindingA == KeyCode.LeftControl || keyBindingData.keyBindingA == KeyCode.RightControl || keyBindingData.keyBindingB == KeyCode.LeftControl || keyBindingData.keyBindingB == KeyCode.RightControl)) || (Event.current.alt && (keyBindingData.keyBindingA == KeyCode.LeftAlt || keyBindingData.keyBindingA == KeyCode.RightAlt || keyBindingData.keyBindingB == KeyCode.LeftAlt || keyBindingData.keyBindingB == KeyCode.RightAlt)) || (Event.current.command && (keyBindingData.keyBindingA == KeyCode.LeftCommand || keyBindingData.keyBindingA == KeyCode.RightCommand || keyBindingData.keyBindingB == KeyCode.LeftCommand || keyBindingData.keyBindingB == KeyCode.RightCommand)) || this.IsDown);
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000716 RID: 1814 RVA: 0x00025840 File Offset: 0x00023A40
		public bool JustPressed
		{
			get
			{
				KeyBindingData keyBindingData;
				return KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData) && (Input.GetKeyDown(keyBindingData.keyBindingA) || Input.GetKeyDown(keyBindingData.keyBindingB));
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000717 RID: 1815 RVA: 0x00025880 File Offset: 0x00023A80
		public bool IsDown
		{
			get
			{
				KeyBindingData keyBindingData;
				return KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData) && (Input.GetKey(keyBindingData.keyBindingA) || Input.GetKey(keyBindingData.keyBindingB));
			}
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x000258BD File Offset: 0x00023ABD
		public KeyCode GetDefaultKeyCode(KeyPrefs.BindingSlot slot)
		{
			if (slot == KeyPrefs.BindingSlot.A)
			{
				return this.defaultKeyCodeA;
			}
			if (slot == KeyPrefs.BindingSlot.B)
			{
				return this.defaultKeyCodeB;
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x000258D9 File Offset: 0x00023AD9
		public static KeyBindingDef Named(string name)
		{
			return DefDatabase<KeyBindingDef>.GetNamedSilentFail(name);
		}

		// Token: 0x04000662 RID: 1634
		public KeyBindingCategoryDef category;

		// Token: 0x04000663 RID: 1635
		public KeyCode defaultKeyCodeA;

		// Token: 0x04000664 RID: 1636
		public KeyCode defaultKeyCodeB;

		// Token: 0x04000665 RID: 1637
		public bool devModeOnly;

		// Token: 0x04000666 RID: 1638
		[NoTranslate]
		public List<string> extraConflictTags;
	}
}
