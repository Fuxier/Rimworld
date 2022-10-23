using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000577 RID: 1399
	public class KeyBindingData
	{
		// Token: 0x06002AF6 RID: 10998 RVA: 0x00003724 File Offset: 0x00001924
		public KeyBindingData()
		{
		}

		// Token: 0x06002AF7 RID: 10999 RVA: 0x00112D14 File Offset: 0x00110F14
		public KeyBindingData(KeyCode keyBindingA, KeyCode keyBindingB)
		{
			this.keyBindingA = keyBindingA;
			this.keyBindingB = keyBindingB;
		}

		// Token: 0x06002AF8 RID: 11000 RVA: 0x00112D2C File Offset: 0x00110F2C
		public override string ToString()
		{
			string str = "[";
			if (this.keyBindingA != KeyCode.None)
			{
				str += this.keyBindingA.ToString();
			}
			if (this.keyBindingB != KeyCode.None)
			{
				str = str + ", " + this.keyBindingB.ToString();
			}
			return str + "]";
		}

		// Token: 0x04001C11 RID: 7185
		public KeyCode keyBindingA;

		// Token: 0x04001C12 RID: 7186
		public KeyCode keyBindingB;
	}
}
