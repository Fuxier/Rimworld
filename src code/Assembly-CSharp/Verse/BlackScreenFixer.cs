using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000512 RID: 1298
	internal class BlackScreenFixer : MonoBehaviour
	{
		// Token: 0x060027B2 RID: 10162 RVA: 0x00102452 File Offset: 0x00100652
		private void Start()
		{
			if (Screen.width != 0 && Screen.height != 0)
			{
				Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreen);
			}
		}
	}
}
