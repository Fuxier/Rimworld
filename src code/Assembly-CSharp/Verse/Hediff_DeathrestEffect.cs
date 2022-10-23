using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000343 RID: 835
	public class Hediff_DeathrestEffect : Hediff
	{
		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x0600164B RID: 5707 RVA: 0x000836B6 File Offset: 0x000818B6
		public override string LabelBase
		{
			get
			{
				return base.LabelBase + " x" + Mathf.FloorToInt(this.Severity);
			}
		}
	}
}
